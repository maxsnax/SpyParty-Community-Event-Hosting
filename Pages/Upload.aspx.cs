﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SML.Exceptions;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ListItem = System.Web.UI.WebControls.ListItem;
using static SML.Models.Replays;
using Match = SML.Models.Match;
using Season = SML.Models.Season;
using SML.Models;

namespace SML {

    public partial class Upload : Page {
        private static UploadService dataLayer = new UploadService();

        protected void Page_Load(object sender, EventArgs e) {
            Logger.Log("Upload Page Loaded.");


            if (!Page.IsPostBack) {
                InitializeSession();
                PopulateSeasons();
            }
            else {
                //if (Session["matchList"] != null) {
                //    ResetMatchResults();
                //}
            }

            // Ensure the master page is correctly cast before accessing EnableDynamicBackground
            SiteMaster master = Master as SiteMaster;
            if (master != null) {
                master.EnableDynamicBackground = true; // Enable background effect for this page
            }

        }

        protected void Session_End(object sender, EventArgs e) {
            dataLayer?.ClearReplaysDirectory();
        }


        // =======================================================================================
        // On initial page load, populate the seasons for user to choose from and initialize empty player data
        // =======================================================================================
        private void PopulateSeasons() {
            ScoreboardService dataLayer = new ScoreboardService();
            List<Season> seasons = dataLayer.LoadSeasons()
                .Where(s => s.Status == "open")
                .OrderBy(s => s.Name)
                .ToList();

            Session["seasonList"] = seasons;

            selectSeasonList.Items.Clear();

            foreach (var season in seasons) {
                selectSeasonList.Items.Add(new ListItem(season.Name, season.SeasonID.ToString()));
            }
            
        }

        // =======================================================================================
        // When user selects a season, populate the division options in proper load order
        // =======================================================================================
        protected void SelectSeasonList_SelectedIndexChanged(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(selectSeasonList.SelectedValue)) {
                int seasonID = int.Parse(selectSeasonList.SelectedValue);
                string seasonName = selectSeasonList.SelectedItem.Text;

                ScoreboardService dataLayer = new ScoreboardService();
                List<Division> divisions = dataLayer.FetchOrderSortedDivisions(seasonID);

                Logger.Log($"Season Selected {seasonName}");
                selectDivisionList.Items.Clear();

                Logger.Log("Divisions:");
                foreach (Division division in divisions) {
                    selectDivisionList.Items.Add(new ListItem(division.DivisionName, division.DivisionID.ToString()));
                    Logger.Log(division.DivisionName);
                }
            } else {
                selectDivisionList.Items.Clear();
            }
        }
        protected void SelectDivisionList_SelectedIndexChanged(object sender, EventArgs e) {

        }
        


        private void InitializeSession() {
            PopulateSeasons();
            Session["matchList"] = new List<Match>();
            Session["seasonList"] = new List<Season>();
            Session["currentReplayDirectory"] = "";
        }

        private void ClearSession() {
            Session["matchList"] = null;
            Session["matchList"] = new List<Match>();
            Session["seasonList"] = new List<Season>();
            Session["currentReplayDirectory"] = "";
        }

        private void ResetMatchResults() {
            uploadFileSection.Visible = true;
            matchResultSection.Visible = false;
            fileUploadNameLabel.Text = $"";
            ClearSession();
        }


        private void AddMatchResults(Match match) {
            Player playerOne = match.PlayerOne;
            Player playerTwo = match.PlayerTwo;

            Panel matchPanel = new Panel {
                CssClass = "results-container"
            };

            Label player1 = new Label {
                CssClass = "player-one",
                Text = $"{playerOne.Results.Points_Won} - {playerOne.Name}"
            };

            Label versus = new Label {
                CssClass = "vs",
                Text = " vs "
            };

            Label player2 = new Label {
                CssClass = "player-two",
                Text = $"{playerTwo.Results.Points_Won} - {playerTwo.Name}"
            };

            matchPanel.Controls.Add(player1);
            matchPanel.Controls.Add(versus);
            matchPanel.Controls.Add(player2);

            matchResultsDiv.Controls.Add(matchPanel);
        }

        protected async void ConfirmUploadButton_Click(object sender, EventArgs e) {
            Logger.Log($"confirmUploadButton_Click()");

            try {
                await dataLayer.ConfirmUpload();

            } catch (Exception ex) {
                Logger.Log(ex.Message);
            }

            ResetMatchResults();
            //UpdateLabel(matchResultLabel, Color.Green, "Match results successfully uploaded.");
            ClearSession();
        }

        protected void CancelUploadButton_Click(object sender, EventArgs e) {
            Logger.Log($"cancelUploadButton_Click()");

            ResetMatchResults();
            //UpdateLabel(matchResultLabel, Color.Red, "Match results were not uploaded.");
            ClearSession();
            dataLayer.ClearReplaysDirectory();
        }

        // =======================================================================================
        // Button click function to initiate upload of replays into the server's directories
        // =======================================================================================
        protected void UploadFileButton_Click(object sender, EventArgs e) {
            Logger.Log("uploadFileButton_Click()");

            // Check if the fileUploadData object in the browser has anything uploaded into it
            if (!fileUploadData.HasFile) {
                UpdateLabel(fileUploadNameLabel, Color.Red, "Please upload a .zip file containing replays, or a .zip containing a folder of replays.");
                return;
            }

            // Place the uploaded file into a temporary directory server-side to confirm it is .ZIP containing .replay files
            try {

                // Set the upload files section of the page invisible
                uploadFileSection.Visible = false;

                if (Session["currentReplayDirectory"] != null) { dataLayer.ClearReplaysDirectory(); }
                // Move the uploaded files into the server directory
                string replayDirectory = dataLayer.MoveReplaysToServer(fileUploadData.PostedFile, Server.MapPath("~"));
                Session["currentReplayDirectory"] = replayDirectory;
                // Call for the matches to be processed from the selected season value in the dropdown
                int seasonID = Int32.Parse(selectSeasonList.SelectedValue);
                string seasonName = selectSeasonList.SelectedItem.Text;
                int divisionID = Int32.Parse(selectDivisionList.SelectedValue);
                string divisionName = selectDivisionList.SelectedItem.Text;

                Logger.Log($"Creating matches for {seasonID}: {seasonName} | {divisionID}: {divisionName}");
                List<Match> matchList = dataLayer.ProcessSeasonMatches(seasonID, divisionID);

                // Generate a table with a header to add replay results to
                HtmlTable uploadedReplaysTable = BuildMatchReplayTable(matchList);
                masterTablePlaceholder.Controls.Add(uploadedReplaysTable);

                // Make the text at the top of the screen above the Replays Menu visible for all match results uploaded
                matchResultSection.Visible = true;
            }
            catch (InvalidFileFormatException ex) {
                UpdateLabel(fileUploadNameLabel, Color.Red, $"{ex.Message}");
                Logger.Log($"Exception: {ex.Message}");
                logdump.InnerText = ex.Message;
            }
            catch (Exception ex) {
                UpdateLabel(fileUploadNameLabel, Color.Red, "An error occurred during upload.");
                Logger.Log($"Exception: {ex.Message}");
                logdump.InnerText = "An error occurred while uploading replays from .zip file";
            }
        }


        // =======================================================================================
        //  Generates an initial table with a header, ready for game replay rows to be added
        // =======================================================================================
        public HtmlTable BuildMatchReplayTable(List<Match> matchList) {
            //// Create a new table that will hold our rows of games played
            HtmlTable uploadedReplaysTable = new HtmlTable();
            uploadedReplaysTable.Attributes["class"] = "match-table";

            // Create a header row for the entire table and add it into the table
            HtmlTableRow replayFolderHeader = new HtmlTableRow();
            replayFolderHeader.Attributes["class"] = "match-categories-row";
            uploadedReplaysTable.Controls.Add(replayFolderHeader);

            HtmlTableCell spyCell = Util.cellText("Spy", className: "match-categories-left-column", colSpan: 1);
            HtmlTableCell sniperCell = Util.cellText("Sniper", className: "match-categories-left-column", colSpan: 1);
            HtmlTableCell venueCell = Util.cellText("Venue", className: "match-categories-left-column", colSpan: 1);
            HtmlTableCell typeCell = Util.cellText("Type", className: "match-type-column", colSpan: 1);
            HtmlTableCell resultCell = Util.cellText("Result", className: "match-categories-left-column", colSpan: 1);
            HtmlTableCell durationCell = Util.cellText("Duration", className: "match-duration-column", colSpan: 1);
            HtmlTableCell dateCell = Util.cellText("Date", className: "match-date-column", colSpan: 1);

            // Add all the cells to the header
            Util.AddCells(replayFolderHeader, new HtmlTableCell[] { spyCell, sniperCell, venueCell, typeCell, resultCell, durationCell, dateCell});

            foreach (Match match in matchList) {
                AddMatchResults(match);
                AddMatchToTable(uploadedReplaysTable, match);
            }

            return uploadedReplaysTable;
        }

        // =======================================================================================
        //  Iterates through a filePath, creating rows for each game and adding it to the table
        // =======================================================================================
        private void AddMatchToTable(HtmlTable table, Match match) {
            Logger.Log($"Processing {match.PlayerOne.Name} vs {match.PlayerTwo.Name}");

            foreach (ReplayData replay in match.Replays) {
                BuildReplayRow(table, replay);

                HtmlTableRow matchSeparator = new HtmlTableRow();
                HtmlTableCell cell = Util.cellText("");
                matchSeparator.Controls.Add(cell);
                matchSeparator.Attributes["class"] = "match-separator";
                table.Controls.Add(matchSeparator);
            }

        }


        // =======================================================================================
        //  Creates a single HtmlTableRow of a game's venue, players, and result
        // =======================================================================================
        private void BuildReplayRow(HtmlTable table, ReplayData replay) {
            //Logger.Log($"Creating replay row");
            HtmlTableRow replayRow = new HtmlTableRow();
            replayRow.Attributes["class"] = "replay-row";

            //playersCell.InnerText = $"{spy} vs {sniper}"; // Sets the header value to show the players
            string spy = replay.spy_displayname;
            string sniper = replay.sniper_displayname;
            string venue = replay.level;
            string type = replay.game_type;
            string result = replay.result;
            string duration = $"{replay.duration / 60}:{(replay.duration % 60):D2}";
            string date = replay.start_time.ToString();


            HtmlTableCell spyRoleTextCell = Util.cellText(spy.Replace("/steam", ""), className: "replay-left-column truncate");
            HtmlTableCell sniperRoleTextCell = Util.cellText(sniper.Replace("/steam", ""), className: "replay-left-column truncate");
            HtmlTableCell venueTextCell = Util.cellText(venue, className: "replay-left-column truncate");
            HtmlTableCell resultTextCell = Util.cellText(result, className: "replay-left-column truncate");
            HtmlTableCell typeTextCell = Util.cellText(type, className: "match-type-column truncate");
            HtmlTableCell durationTextCell = Util.cellText(duration, className: ".match-duration-column truncate");
            HtmlTableCell dateTextCell = Util.cellText(date, className: "match-date-column truncate");


            if (replay.result == "Missions Win" || replay.result == "Civilian Shot") {
                spyRoleTextCell.Attributes["Bgcolor"] = "#d9ead7";
            } else { 
                sniperRoleTextCell.Attributes["Bgcolor"] = "#d9ead7";
            }

            // Add the cells to the row
            Util.AddCells(replayRow, new HtmlTableCell[] {  
                spyRoleTextCell,
                sniperRoleTextCell,
                venueTextCell,
                typeTextCell,
                resultTextCell,
                durationTextCell,
                dateTextCell
            });

            //Logger.Log("Adding replayRow to masterTablePlaceholder");
            table.Controls.Add(replayRow);
        }


        // =======================================================================================
        // Update labels in a single line of code
        // =======================================================================================
        private void UpdateLabel(Label label, System.Drawing.Color forecolor, string text) {
            Logger.Log($"updateLabel()");
            label.ForeColor = forecolor;
            label.Text = text;
        }
    }
}
