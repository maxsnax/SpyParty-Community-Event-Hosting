using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Diagnostics;
using SML.Models;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Microsoft.Extensions.Azure;


namespace SML {
    public partial class Players : System.Web.UI.Page {
        private readonly PlayersService _playersService = new PlayersService();

        protected void Page_Load(object sender, EventArgs e) {
            LoadPlayersData();

            // Ensure the master page is correctly cast before accessing EnableDynamicBackground
            SiteMaster master = Master as SiteMaster;
            if (master != null) {
                master.EnableDynamicBackground = true; // Enable background effect for this page
            }

        }

        private void LoadPlayersData() {
            string playerName = Page.RouteData.Values["playerName"] as string ?? Request.QueryString["playerName"];

            if (string.IsNullOrEmpty(playerName)) {
                Logger.Log("No specific player requested. Loading all players...");

                DataTable rawData;
                rawData = _playersService.PopulateAllPlayerData(PlayerGridView);
                ViewState["dataTable"] = rawData;
                playerProfile.Visible = false;
            }
            else {
                List<Player> playerData = _playersService.GetPlayerData(playerName);
                lblPlayerName.Text = playerData != null ? $"{playerName}" : "Player not found.";
                LobbyMenu.Visible = false;
                playerProfile.Visible = true;
                PopulatePlayerData(playerData);
            }
        }

        public void PlayerGridView_Sorting(object sender, GridViewSortEventArgs e) {
            Logger.Log("Sort GridView");

            if (ViewState["dataTable"] is DataTable dataTable) {
                DataView dataView = new DataView(dataTable);

                string sortDirection = GetSortDirection(e.SortExpression);
                dataView.Sort = e.SortExpression + " " + sortDirection;

                PlayerGridView.DataSource = dataView;
                PlayerGridView.DataBind();

                // Apply underline to the sorted column
                int columnIndex = GetColumnIndexBySortExpression(e.SortExpression);
                if (PlayerGridView.HeaderRow != null && columnIndex >= 0) {
                    foreach (TableCell cell in PlayerGridView.HeaderRow.Cells) {
                        cell.Style["border-bottom"] = "1px solid black"; // Remove underline from all headers
                    }
                    PlayerGridView.HeaderRow.Cells[columnIndex].Style["border-bottom"]= "3px solid black"; // Underline sorted column
                }
            }
        }

        protected void PlayerGridView_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                string playerName = DataBinder.Eval(e.Row.DataItem, "player_name").ToString();
                e.Row.Attributes["onclick"] = "window.location='Players/" + playerName + "';";
                e.Row.Style["cursor"] = "pointer";  // Optional: changes the cursor to a pointer when hovering
            }
        }


        // Helper function to get column index
        private int GetColumnIndexBySortExpression(string sortExpression) {
            for (int i = 0; i < PlayerGridView.Columns.Count; i++) {
                if (PlayerGridView.Columns[i] is BoundField field && field.SortExpression == sortExpression) {
                    return i;
                }
            }
            return -1;
        }

        private string GetSortDirection(string column) {
            string sortDirection = "ASC";

            if (ViewState["SortColumn"] as string == column) {
                if (ViewState["SortDirection"] as string == "ASC") {
                    sortDirection = "DESC";
                }
            }

            ViewState["SortColumn"] = column;
            ViewState["SortDirection"] = sortDirection;

            return sortDirection;
        }


        private void PopulatePlayerData(List<Player> playerList) {
            if (playerList == null || playerList.Count == 0) return;

            Player player = playerList[0];

            // Player Icon  
            string playerIconVirtualPath = $"/Images/playerIcons/{player.Name}.png";
            string playerIconPhysicalPath = Server.MapPath(playerIconVirtualPath);

            if (!File.Exists(playerIconPhysicalPath)) {
                playerIconVirtualPath = "/Images/icons/Smallman.png";
            }

            playerProfilePhoto.ImageUrl = playerIconVirtualPath;

            if (player.Name != player.Username) {
                lblPlayerUsername.Text = player.Username;
            }


            Division division = new Division("", 0);
            HtmlTable table = new HtmlTable();
            table.Attributes["class"] = "playerSeasonTable rank-table";
            Util.AddDivisionHeaderRow(table, division);

            Player runningStats = new Player(player.Name);

            foreach (Player p in playerList) {
                runningStats.Add(p);

                Util.AddPlayerToTable(table, p, false);
                playerEventsPanel.Controls.Add(table);
            }

            Util.AddPlayerToTable(table, runningStats, true);

        }

        private List<Player> GetPlayerData(string playerName) {
            DatabaseHelper db = new DatabaseHelper ();

            List<Player> playerData = null;

            //selectSeasonList.Items.Clear();
            List<Tuple<int, string>> seasonList = new List<Tuple<int, string>>();

            using (SqlConnection connection = DatabaseHelper.GetConnection()) {
                connection.Open();

                try {
                    playerData = db.GetPlayerByName(playerName, connection, null);
                    if (playerData == null) return null;

                    foreach (Player p in playerData) {
                        int seasonID = p.Season;
                        string seasonName = db.GetSeasonNameById(seasonID, connection, null);
                        Logger.Log($"SeasonID:{seasonID}:{seasonName}");
                        seasonList.Add(Tuple.Create(seasonID, seasonName));
                    }
                }
                catch (Exception ex) {
                    Logger.Log(ex.Message);
                }
            }

            //PopulateSeasons(seasonList);

            return playerData;
        }

    }
}
