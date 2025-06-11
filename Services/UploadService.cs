using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Diagnostics;
using SML.Exceptions;
using System.IO.Compression;
using SML.Models;
using static SML.Models.Replays;
using SML.DAL.Repositories;
using SML.DAL;
using System.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Web.UI.WebControls;
using Microsoft.Data.SqlClient;

namespace SML {
    public class UploadService {

        private string replayDirectoryLocation;
        private List<Match> matchList = new List<Match>();

        // =======================================================================================
        // Returns all registered seasons and their divisions
        // =======================================================================================
        public List<Season> LoadSeasons() {
            using UnitOfWork uow = new UnitOfWork(ConfigurationManager.ConnectionStrings["SML_db-connection"].ToString());
            
            List<Season> seasons = uow.SeasonsRepo.LoadSeasons();

            foreach (Season season in seasons) {
                List<Division> divisions = uow.SeasonsRepo.GetSeasonDivisions(season.SeasonID);

                season.Divisions = divisions;
            }

            return seasons;
        }


        // =======================================================================================
        // Upload the file into the server's App_Data/replays directory as a .zip and directory
        // =======================================================================================
        public string MoveReplaysToServer(HttpPostedFile uploadedFile, string serverPath) {
            Logger.Log("moveReplaysToServer()");

            // Get the name of the uploaded file
            string uploadFileName = uploadedFile.FileName;
            FileInfo fileInfo = new FileInfo(uploadFileName);
            string fileExtension = fileInfo.Extension;

            Logger.Log($"Uploaded File: {uploadedFile.FileName} | Extension: {fileExtension} | serverPath: {serverPath}");


            // Reject the uploaded file if it's not a .ZIP format
            if (fileExtension != ".zip") {
                throw new InvalidFileFormatException($"{fileExtension} is not a valid .zip file! Please upload a .zip file");
            }

            // Remove illegal characters)
            string zipSanitizedFileName = string.Join("_", Path.GetFileNameWithoutExtension(uploadFileName).Split(Path.GetInvalidFileNameChars()));
            zipSanitizedFileName = zipSanitizedFileName.Replace("%2fsteam", "");

            string tempDir = Path.Combine(serverPath, "App_Data", "replays");
            string zipFilePath = Path.Combine(tempDir, zipSanitizedFileName + ".zip");
            string extractPath = Path.Combine(tempDir, zipSanitizedFileName);

            // Ensure replays directory exists
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            // Save the uploaded .zip file to the server in the replays directory
            uploadedFile.SaveAs(zipFilePath);
            Logger.Log($"Zip saved at: {zipFilePath}");

            // Ensure directory exists to extract the .zip contents into, in case there are loose .replay files
            // Ensure replays directory exists
            if (!Directory.Exists(extractPath)) {
                Logger.Log($"{extractPath} directory does not exist. Creating directory");
                Directory.CreateDirectory(extractPath);
            }

            Logger.Log($"Attempting to extract {zipFilePath} to: {extractPath}");
            try {
                string result = ExtractReplayZip(zipFilePath, extractPath);
                Logger.Log("Result from Python script: " + result);

            }
            catch (Exception ex) {
                Logger.Log($"Files failed to extract {zipFilePath} to: {extractPath}");
                Logger.Log(ex.Message);
                throw ex;
            }

            // Get all extracted files, searching nested directories within
            string[] files = Directory.GetFiles(extractPath, "*.*", SearchOption.AllDirectories);

            // Ensure only .replay files are uploaded before we start opening them and reading
            foreach (string file in files) {
                Logger.Log($"Searching for non-replay files. {file}");
                FileInfo replayFileInfo = new FileInfo(file);
                string replayFileExtension = replayFileInfo.Extension;
                if (replayFileExtension != ".replay") {
                    throw new InvalidFileFormatException($"{replayFileExtension} is not a valid .replay file! Please upload a .zip file containing .replay files");
                }

            }

            replayDirectoryLocation = extractPath;

            Logger.Log($"Returning replayDirectoryLocation moved to server {replayDirectoryLocation}");
            return extractPath;
        }

        public static string ExtractReplayZip(string zipFilePath, string outputDir) {
            string pythonExe = HttpContext.Current.Server.MapPath("~/Python313/python.exe");
            string script = HttpContext.Current.Server.MapPath("~/SpyPartyParse-master/extract_replays.py");

            // Ensure output directory exists before calling Python
            if (!Directory.Exists(outputDir)) {
                Logger.Log("Creating output directory: " + outputDir);
                Directory.CreateDirectory(outputDir);
            }

            var psi = new ProcessStartInfo {
                FileName = pythonExe,
                Arguments = $"\"{script}\" \"{zipFilePath}\" \"{outputDir}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = Path.GetDirectoryName(script)
            };

            string output;
            string errors;

            Logger.Log("Running extract_replays.py...");
            Logger.Log($"Arguments: {psi.Arguments}");

            using (var process = Process.Start(psi)) {
                output = process.StandardOutput.ReadToEnd();
                errors = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Logger.Log("Python stdout:");
                Logger.Log(output);

                if (!string.IsNullOrWhiteSpace(errors)) {
                    Logger.Log("Python stderr:");
                    Logger.Log(errors);
                }

                if (process.ExitCode != 0) {
                    Logger.Log("Python script exited with code: " + process.ExitCode);
                    throw new Exception("Python script failed with exit code " + process.ExitCode + ":\n" + errors);
                }
            }

            return output;
        }



        // =======================================================================================
        //  Deletes the pathway and all it's subdirectories
        // =======================================================================================
        public void ClearReplaysDirectory() {
            DeletePath(replayDirectoryLocation + ".zip");
            DeletePath(replayDirectoryLocation);
            matchList = new List<Match>();
        }

        // =======================================================================================
        //  Deletes the pathway and all it's subdirectories
        // =======================================================================================
        private static void DeletePath(string path) {
            Logger.Log($"DeletePath({path})");
            // Delete extracted files before removing the directory
            if (Directory.Exists(path)) {
                foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)) {
                    try {
                        File.Delete(file);
                        Logger.Log($"Deleted file: {file}");
                    }
                    catch (Exception ex) {
                        Logger.Log($"Failed to delete file: {file} - {ex.Message}");
                    }
                }

                // Ensure all subdirectories are deleted
                foreach (string dir in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)) {
                    try {
                        Directory.Delete(dir, true);
                        Logger.Log($"Deleted directory: {dir}");
                    }
                    catch (Exception ex) {
                        Logger.Log($"Failed to delete directory: {dir} - {ex.Message}");
                    }
                }
            }

            if (Directory.Exists(path)) {
                try {
                    Directory.Delete(path, true);
                    Logger.Log($"Deleted dir: {path}");
                }
                catch (Exception ex) {
                    Logger.Log($"Failed to delete directory: {path} - {ex.Message}");
                }
            }
            if (File.Exists(path)) {
                try {
                    File.Delete(path);
                    Logger.Log($"Deleted file: {path}");
                }
                catch (Exception ex) {
                    Logger.Log($"Failed to delete file: {path} - {ex.Message}");
                }
            }
        }


        // =======================================================================================
        //  Process the replays uploaded and then return the match list
        // =======================================================================================
        // Call from business layer to start recursive exploration for replays from the top directory
        public List<Match> ProcessSeasonMatches(int seasonID, int divisionID) {
            ProcessDirectoryMatches(replayDirectoryLocation, seasonID, divisionID);

            return matchList;
        }


        // =======================================================================================
        //  Directory processing of all uploaded directories/replays
        // =======================================================================================
        // Given the server directory path and the seasonID from the webpage, will return all the match data collected
        private List<Match> ProcessDirectoryMatches(string replayFilesLocation, int seasonID, int divisionID) {
            try {
                string[] directories = Directory.GetDirectories(replayFilesLocation, "*", SearchOption.TopDirectoryOnly);
                string[] files = Directory.GetFiles(replayFilesLocation, "*.replay", SearchOption.TopDirectoryOnly);

                Logger.Log($"Checking directory for match: {replayFilesLocation}: season {seasonID}");
                ProcessMatch(replayFilesLocation, seasonID, divisionID);


                // Recursively process subdirectories
                foreach (string directory in directories) {
                    Logger.Log($"Processing Directory: {directory}: season {seasonID}, division {divisionID}");
                    ProcessDirectoryMatches(directory, seasonID, divisionID);
                }

                return matchList;
            }
            catch (Exception ex) {
                throw new Exception($"Error processing directory {replayFilesLocation}: {ex.Message}", ex);
            }
        }

        // =======================================================================================
        //  Directory processing of all uploaded directories/replays
        // =======================================================================================
        private void ProcessMatch(string directoryPath, int seasonID, int divisionID) {
            // Ensure directory exists
            if (!Directory.Exists(directoryPath)) {
                Logger.Log($"Invalid path: {directoryPath}");
                return;
            }

            // Get all replay files in the directory
            string[] files = Directory.GetFiles(directoryPath, "*.replay", SearchOption.TopDirectoryOnly);
            if (files.Length == 0) return; // No replay files found, nothing to process

            Match match = new Match {
                SeasonID = seasonID,
                DivisionID = divisionID
            };

            foreach (var file in files) {
                ReplayData replay = Replays.ReadFile(Path.GetFullPath(file));

                // Skip processing this file
                if (replay == null) {
                    Logger.Log($"Warning: Failed to process replay file {file}. It returned null.");
                    continue;
                } else if (replay.result == "In_Progress") {
                    Logger.Log($"Warning: Skipped replay file {file}. It returned In_Progress.");
                    continue;
                }

                ProcessReplay(match, replay);
            }

            // Add the match to the global match list
            matchList.Add(match);
        }

        // =======================================================================================
        //  Match processing of replay data
        // =======================================================================================
        private void ProcessReplay(Match match, ReplayData replay) {
            string spy = replay.spy_displayname.Replace("/steam", "");
            string sniper = replay.sniper_displayname.Replace("/steam", "");

            if (match.PlayerOne == null && match.PlayerTwo == null) {
                match.PlayerOne = new Player(spy);
                match.PlayerTwo = new Player(sniper);
            }
            else {
                bool playerOneFound = match.PlayerOne.Name == spy || match.PlayerOne.Name == sniper;
                bool playerTwoFound = match.PlayerTwo.Name == spy || match.PlayerTwo.Name == sniper;

                if (!playerOneFound || !playerTwoFound) {
                    string replayPlayers = $"{spy} vs {sniper}";
                    string matchPlayers = $"{match.PlayerOne.Name} vs {match.PlayerTwo.Name}";
                    throw new InvalidMatchException($"{replayPlayers} invalid replay for match {matchPlayers}");
                }
            }

            match.ProcessReplay(replay);
        }


        // =======================================================================================
        // Azure SQL
        // =======================================================================================
        // =======================================================================================
        // Process upload confirmation of business layer
        // =======================================================================================
        public async Task ConfirmUpload() {
            Logger.Log($"ConfirmUpload()");

            using UnitOfWork uow = new UnitOfWork(ConfigurationManager.ConnectionStrings["SML_db-connection"].ToString());

            try {
                uow.BeginTransaction();

                // Upload all the matches to SQL first in case we run into any issues before uploading Blobs
                foreach (Match match in matchList) {
                    try {
                        SaveMatchToSQL(match, uow);
                    } catch (Exception ex) {

                        Logger.Log(ex.Message);
                    }
                }

                // Less likely to have issues, and lost replays is less important than saving the replay data to SQL
                foreach (Match match in matchList) {
                    await UploadReplayBlobs(match);
                }

                // Once all the SQL entries are made and the Blobs uploaded then we commit
                uow.CommitTransaction();
                Logger.Log($"Matches successfully uploaded to SQL and Azure Blob Storage");

            }
            catch {
                // If there was an issue with either SQL or the Blob upload then we roll it back
                uow.Rollback();
                throw;
            }
            finally {
                ClearReplaysDirectory();
                matchList = new List<Match>();
            }
        }


        // =======================================================================================
        // Save the match's ReplayData to SQL
        // =======================================================================================
        private void SaveMatchToSQL(Match match, UnitOfWork uow) {
            Logger.Log($"SaveMatchToSQL()");

            // Handle if null player data was passed
            if (match == null || match.PlayerOne == null || match.PlayerTwo == null || match.SeasonID <= 0) {
                throw new Exception("Unable to use player data to save match. Invalid player data or season provided.");
            }

            match.CalculateWinner();
            Player playerOne = match.PlayerOne;
            Player playerTwo = match.PlayerTwo;
            int season_id = match.SeasonID;


            try {
                Player playerOneSQL = uow.PlayersRepo.GetPlayerByNameAndSeason(playerOne.Name, season_id);
                Player playerTwoSQL = uow.PlayersRepo.GetPlayerByNameAndSeason(playerTwo.Name, season_id);

                // Handle if either of the players were unable to be found in the current season
                if (playerOneSQL == null || playerTwoSQL == null) {
                    Season currentSeason = uow.SeasonsRepo.GetSeasonByID(season_id);
                    if (currentSeason.UnregisteredUpload == 1) {
                        Logger.Log($"Unregistered upload for {playerOne.Name} vs {playerTwo.Name} in season {season_id}.");

                        //// If the season allows unregistered uploads, create the players
                        if (playerOneSQL == null) {
                            playerOne.Season = match.SeasonID;
                            playerOne.Division = match.DivisionID;
                            uow.PlayersRepo.CreatePlayer(playerOne);
                        }

                        if (playerTwoSQL == null) {
                            playerTwo.Season = match.SeasonID;
                            playerTwo.Division = match.DivisionID;
                            uow.PlayersRepo.CreatePlayer(playerTwo);
                        }
                    } else {
                        throw new NullReferenceException($"Unable to find players in current season database.");

                    }
                }

                // Update our current player info with their player_id, username, division_id, forfeit status
                playerOne.UpdatePlayerInfo(playerOneSQL);
                playerTwo.UpdatePlayerInfo(playerTwoSQL);

                // Handle if the players are both in the season, but are in different divisions
                if (playerOne.Division != playerTwo.Division) {
                    throw new Exception($"{playerOne.Name}:{playerOne.Division} and {playerTwo.Name}:{playerTwo.Division} are not in the same division!");
                }

                // Upload the match replays to SQL
                uow.MatchesRepo.CreateMatchWithReplays(match);

                // Set the username values if they were null, since this would be this player's first games
                // We're gonna need the in-game username to query replays later on
                if (playerOne.Username == null) {
                    uow.PlayersRepo.UpdatePlayerUsername(playerOne);
                }
                if (playerTwo.Username == null) {
                    uow.PlayersRepo.UpdatePlayerUsername(playerTwo);
                }

                // Update the player's stats for win/tie/loss in the division and their spy/sniper
                uow.PlayersRepo.UpdatePlayerStatsFromMatch(match);

                //updateLabel(fileUploadNameLabel, Color.Green, "Upload Successful!");
            }
            catch (Exception ex) {
                Logger.Log($"Error Caught: Rolling back transaction");
                //updateLabel(fileUploadNameLabel, Color.Red, $"Error: {ex.Message}");
                throw ex;
            }
        }


        // =======================================================================================
        //  Azure Blobs
        // =======================================================================================
        //  Iterates through a filePath, uploading all of the files to the /replays/ blob container
        // =======================================================================================
        private async Task UploadReplayBlobs(Match match) {
            try {
                Logger.Log($"Uploading match {match.PlayerOne.Name} vs {match.PlayerTwo.Name}");
                foreach (ReplayData replay in match.Replays) {
                    try {
                        var service = new AzureBlobService();
                        await service.UploadFilesAsync(replay.file_path, replay.uuid);
                        Logger.Log($"Uploaded blob {replay.file_path}:{replay.uuid} to server.");
                    }
                    catch (Exception ex) {
                        Logger.Log($"{replay.file_path}:{replay.uuid} \n unable to upload replays to cloud service \n {ex.Message}");
                        //updateLabel(fileUploadNameLabel, Color.Red, $"{replay.file_path}:{replay.uuid} \n unable to upload replays to cloud service \n {ex.Message}");
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
                //fileUploadNameLabel.Text = $"{match.PlayerOne.Name} vs {match.PlayerTwo.Name} \n unable to get replays \n {ex.Message}";
            }

        }

    }
}
