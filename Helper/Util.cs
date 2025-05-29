using SML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using static SML.Util;
using static SML.Models.Player;
using static SML.Models.Division;
using static SML.Models.Season;
using SML.Models;
using SML.DAL;
using System.Configuration;
using System.IO;

namespace SML {
    public class Util {

        public static HtmlTableRow row(string text, string className = "defaultClassName", int height = -1) {

            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(cellText(text, className));

            if (height > -1) {
                row.Height = height.ToString();
            }

            return row;
        }


        public static HtmlTableCell cellText(string text, string className = "defaultClassName", int colSpan = 1) {

            HtmlTableCell cell = new HtmlTableCell();
            cell.InnerText = text;
            cell.Attributes["class"] = className;
            cell.ColSpan = colSpan;

            return cell;
        }

        public static HtmlTableCell cellImage(string imagePath, string className = "defaultClassName", int colSpan = 1, int width = -1) {

            HtmlTableCell cell = new HtmlTableCell();
            HtmlImage image = new HtmlImage();
            image.Src = imagePath;
            image.Alt = "image";
            cell.ColSpan = colSpan;
            cell.Controls.Add(image);
            cell.Attributes.Add("class", className);

            if (width > 0) {
                image.Width = width;
            }

            return cell;
        }

        public static void AddCells(HtmlTableRow row, HtmlTableCell[] cells) {
            foreach (HtmlTableCell cell in cells) {
                row.Cells.Add(cell);
            }
        }


        public static string scrubName(string name) {
            name = name.Replace("/steam", "");
            return name;
        }


        public static HtmlTable CreateSeasonTable(int seasonID) {
            HtmlTable rankTable = new HtmlTable();
            try {
                ScoreboardService dataLayer = new ScoreboardService();
                List<Division> divisions = dataLayer.FetchOrderSortedDivisions(seasonID);

                foreach (Division division in divisions) {
                    //division.Players = dataLayer.FetchPlayerSortedDivisions(division);
                    division.Players = dataLayer.FetchPlayerLoadOrder(division);

                    // Create a table for each rank
                    rankTable.Attributes["class"] = $"rank-table {division.DivisionName}";

                    AddDivisionHeaderRow(rankTable, division);  // Create the division header
                    CreateDivisionTable(rankTable, division);   // Populate the players information
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return rankTable;
        }


        // Logo | Name | Points | W | T | L | Games W-L | Sniper % | Sniper W-L | Spy % | Spy W-L
        public static HtmlTableRow FillTableRow(HtmlTableRow row, HtmlTableCell[] imageColumns, Dictionary<string, string> columns) {
            // Identify whether the row is a header or a normal row based on the number of image columns
            bool isHeaderRow = imageColumns.Length > 1;

            // Image column (Always exists for both header & normal rows)
            HtmlTableCell imageCol = imageColumns[0];

            // Sniper and Spy logos (only in header rows)
            HtmlTableCell sniperLogo = isHeaderRow ? imageColumns[1] : null;
            HtmlTableCell spyLogo = isHeaderRow ? imageColumns[2] : null;

            // Retrieve values by key
            string name = columns.ContainsKey("name") ? columns["name"] : "";
            string points = columns.ContainsKey("points") ? columns["points"] : "";
            string win = columns.ContainsKey("win") ? columns["win"] : "";
            string tie = columns.ContainsKey("tie") ? columns["tie"] : "";
            string loss = columns.ContainsKey("loss") ? columns["loss"] : "";
            string gamesWinLoss = columns.ContainsKey("gamesWinLoss") ? columns["gamesWinLoss"] : "";
            string sniperPercent = columns.ContainsKey("sniperPercent") ? columns["sniperPercent"] : "";
            string sniperScore = columns.ContainsKey("sniperScore") ? columns["sniperScore"] : "";
            string spyPercent = columns.ContainsKey("spyPercent") ? columns["spyPercent"] : "";
            string spyScore = columns.ContainsKey("spyScore") ? columns["spyScore"] : "";


            // Create text cells
            HtmlTableCell nameCell = Util.cellText(name);
            nameCell.Attributes["class"] = isHeaderRow ? "rank-name" : "player-name";

            HtmlTableCell pointCell = Util.cellText(points, className: "stat-column");
            HtmlTableCell winCell = Util.cellText(win, className: "stat-column");
            HtmlTableCell tieCell = Util.cellText(tie, className: "stat-column");
            HtmlTableCell lossCell = Util.cellText(loss, className: "stat-column");

            HtmlTableCell gamesWinLossCell = Util.cellText(gamesWinLoss, className: "stat-column");

            // Create spy/sniper columns (only for normal rows)
            HtmlTableCell sniperCell = isHeaderRow ? sniperLogo : Util.cellText(sniperPercent, className: "stat-column");
            HtmlTableCell sniperScoreCell = Util.cellText(sniperScore, className: "stat-column");
            HtmlTableCell spyCell = isHeaderRow ? spyLogo : Util.cellText(spyPercent, className: "stat-column");
            HtmlTableCell spyScoreCell = Util.cellText(spyScore, className: "stat-column");

            // Add all cells to the row
            List<HtmlTableCell> cells = new List<HtmlTableCell> {
                imageCol, nameCell, pointCell,
                winCell, tieCell, lossCell,
                gamesWinLossCell,
                sniperCell, sniperScoreCell,
                spyCell, spyScoreCell
            };

            // Add cells to the row
            AddCells(row, cells.ToArray());

            return row;
        }



        // ================================================================================================
        //  Create two header rows for the top of the Division's table for division name and column names
        // ================================================================================================
        public static void AddDivisionHeaderRow(HtmlTable rankTable, Division division) {
            string rankName = division.DivisionName;

            // =====================================================================================================
            //  Div Img | Div Name | Points | Win | Tie | Loss | Sniper Logo | Sniper Stats | Spy Logo | Spy Stats
            // =====================================================================================================
            // Create the table header
            HtmlTableRow rankHeaderRow = new HtmlTableRow();
            rankHeaderRow.Attributes["class"] = $"rank-header-row {rankName}";

            // Create the cells for division logo and sniper/spy logos
            string divisionImgPath = $@"\Images\divisions\{rankName}.png";
            string sniperImagePath = @"\Images\icons\sniper.png";
            string spyImagePath = @"\Images\icons\spy.png";

            HtmlTableCell logoCell = new HtmlTableCell();
            if (File.Exists(divisionImgPath)) {
                 logoCell = Util.cellImage(divisionImgPath, className: "picture-cell");
            } else {
                logoCell.Attributes["class"] = "picture-cell";
            }

                HtmlTableCell sniperCell = Util.cellImage(sniperImagePath, className: "stat-column");
            HtmlTableCell spyCell = Util.cellImage(spyImagePath, className: "stat-column");

            var headerImages = new HtmlTableCell[] { logoCell, sniperCell, spyCell };

            // Define the column values for the row
            var headerColumns = new Dictionary<string, string> {
                { "name", rankName }, { "points", "Points" },
                { "win", "W" }, { "tie", "T" }, { "loss", "L" },
                { "gamesWinLoss", "W-L" },
                { "sniperScore", "Win-Loss" },
                { "spyScore", "Win-Loss" }
            };

            // Fill the row with all it's column data then add it to the table
            rankHeaderRow = FillTableRow(rankHeaderRow, headerImages, headerColumns);
            rankTable.Controls.Add(rankHeaderRow);
        }


        // Converts two ints into a percentage value
        public static string PercentageWin(int wins, int totalGames) {
            string stats = "";
            // To avoid division by zero error
            if (totalGames != 0) {
                float statsPercentage = (float)wins / totalGames * 100;
                int statsRounded = (int)Math.Round(statsPercentage);
                stats = $"{statsRounded}%";
            }
            return stats;
        }


        // Populates all of the player information into the given rank's table
        public static void CreateDivisionTable(HtmlTable rankTable, Division division) {
            List<Player> playersList = division.Players;

            int divisionSpyWin = 0;
            int divisionSniperWin = 0;
            // Add each rank to the table
            foreach (Player player in playersList) {

                if (player == null) {
                    System.Diagnostics.Debug.WriteLine("Error null player");
                }

                divisionSpyWin += player.Results.Spy_Wins;
                divisionSniperWin += player.Results.Sniper_Wins;
                AddPlayerToTable(rankTable, player, true);
            }

            if (playersList.Count > 0) {
                HtmlTableRow averageRow = GetDivisionAverageRow(division.DivisionName, divisionSpyWin, divisionSniperWin);
                rankTable.Controls.Add(averageRow);
            }
        }


        // Displays the averages as a footer row
        public static HtmlTableRow GetDivisionAverageRow(string divisionName, int spyWins, int sniperWins) {
            // Create the initial row and assign it's class
            HtmlTableRow dataRow = new HtmlTableRow();
            dataRow.Attributes["class"] = $"rank-header-row {divisionName}";

            // Get the division logo symbol and at it to an image cell for the row
            string divisionImgPath = $@"\Images\divisions\{divisionName}.png";
            HtmlTableCell logoCell = Util.cellImage(divisionImgPath, className: "picture-cell");

            // Add the logo to an array for the FillTableRow function
            var footerImages = new HtmlTableCell[] { logoCell };

            // Prep the stats we'll be adding into the row
            int totalGames = spyWins + sniperWins;
            string spyStats = PercentageWin(spyWins, totalGames);
            string sniperStats = PercentageWin(sniperWins, totalGames);

            // Define the column values for the row
            var footerColumns = new Dictionary<string, string> {
                { "sniperPercent", sniperStats }, { "sniperScore", $"{sniperWins} - {spyWins}" },
                { "spyPercent", spyStats }, { "spyScore", $"{spyWins} - {sniperWins}" }
            };

            dataRow = FillTableRow(dataRow, footerImages, footerColumns);

            return dataRow;
        }


        public static void AddPlayerToTable(HtmlTable rankTable, Player player, bool isPlayer) {
            HtmlTableRow dataRow = new HtmlTableRow();
            dataRow.Attributes["class"] = "player-row";

            // Player Icon Column
            string playerIconVirtualPath;
            // Build the virtual path for the player's icon
            if (isPlayer) {
                playerIconVirtualPath = $"/Images/playerIcons/{player.Name}.png";
            } else {
                playerIconVirtualPath = $"/Images/divisions/{player.DivisionName}.png";
            }

            // Replace the problematic line with the following code:  
            string playerIconPhysicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), playerIconVirtualPath.TrimStart('/'));
            // Use the virtual path for the image src in the HTML
            HtmlTableCell logoCell = new HtmlTableCell();


            // Check if the image file exists on the server, else use an empty cell
            if (!File.Exists(playerIconPhysicalPath)) {
                logoCell.Attributes["class"] = "picture-cell";
            } else {
                logoCell = Util.cellImage(playerIconVirtualPath, className: "picture-cell");
            }




                // =========================================
                //  Logo, Rank, Points, Win, Tie, Loss, Spy, Sniper
                // =========================================
                string playerName;
            if (isPlayer) {
                playerName = player.Forfeit == 0 ? player.Name : player.Name + " (Forfeit)";
            } else {
                playerName = player.Forfeit == 0 ? player.DivisionName : player.DivisionName + " (Forfeit)";
            }

            int spyWinsCount = player.Results.Spy_Wins;
            int spyLossCount = player.Results.Spy_Losses;

            int spyGamesCount = spyWinsCount + player.Results.Spy_Losses;
            string spyStats = PercentageWin(spyWinsCount, spyGamesCount);
            string spyScore = $"{spyWinsCount} - {spyLossCount}";

            int sniperWinsCount = player.Results.Sniper_Wins;
            int sniperLossCount = player.Results.Sniper_Losses;
            int sniperGamesCount = sniperWinsCount + player.Results.Sniper_Losses;
            string sniperStats = PercentageWin(sniperWinsCount, sniperGamesCount);
            string sniperScore = $"{sniperWinsCount} - {sniperLossCount}";

            // Define the column values for the row
            var dataColumns = new Dictionary<string, string> {
                { "name", playerName }, { "points", player.Points.ToString() },
                { "win", player.Wins.ToString() }, { "tie", player.Ties.ToString() }, { "loss", player.Losses.ToString() },
                { "gamesWinLoss", $"{spyWinsCount + sniperWinsCount} - {spyLossCount + sniperLossCount}" },
                { "sniperPercent", sniperStats }, { "sniperScore", sniperScore },
                { "spyPercent", spyStats}, { "spyScore", spyScore }
            };

            dataRow = FillTableRow(dataRow, new HtmlTableCell[] { logoCell }, dataColumns);
            rankTable.Rows.Add(dataRow);



            //string spyScoreTooltip = 
            //    $"Wins:\n" +
            //    $"Mission Win: {player.Results.Spy_MissionsWin}\n" +
            //    $"Civilian Shot: {player.Results.Spy_CivilianShot}\n" +
            //    $"Losses:\n" +
            //    $"Timeouts: {player.Results.Spy_TimeOut}\n" +
            //    $"Spy Shot: {player.Results.Spy_SpyShot}\n";

            //spyCell.Attributes["title"] = spyScoreTooltip;
            //spyScoreCell.Attributes["title"] = spyScoreTooltip;
        }

    }
}
