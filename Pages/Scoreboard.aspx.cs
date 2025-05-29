using Newtonsoft.Json.Linq;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static SML.Util;
using static SML.Models.Player;
using static SML.Models.Division;
using static SML.Models.Season;
using SML.Models;
using SML.DAL;
using System.Configuration;


namespace SML {
    public partial class Scoreboard : System.Web.UI.Page {


        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                PopulateSeasons();

                // Check if there's a previously selected season
                if (Session["SelectedSeason"] != null) {
                    string lastSeason = Session["SelectedSeason"].ToString();
                    ListItem item = selectSeasonList.Items.FindByValue(lastSeason);
                    if (item != null) {
                        selectSeasonList.ClearSelection();
                        item.Selected = true;
                    }
                }
                else if (selectSeasonList.Items.Count > 0) {
                    selectSeasonList.SelectedIndex = 0;
                }

                int seasonID = Int32.Parse(selectSeasonList.SelectedValue);

                HtmlTable rankTable = Util.CreateSeasonTable(seasonID);
                masterTablePanel.Controls.Add(rankTable); // Add the table to the master panel
            }

            // Ensure the master page is correctly cast before accessing EnableDynamicBackground
            SiteMaster master = Master as SiteMaster;
            if (master != null) {
                master.EnableDynamicBackground = true; // Enable background effect for this page
            }

        }


        private void PopulateSeasons() {
            ScoreboardService dataLayer = new ScoreboardService();
            List<Season> seasons = dataLayer.LoadSeasons()
                //.Where(s => s.Status == "open")
                .OrderBy(s => s.Name)
                .ToList();

            selectSeasonList.Items.Clear();

            foreach (var season in seasons) {
                selectSeasonList.Items.Add(new ListItem(season.Name, season.SeasonID.ToString()));
            }
        }

        protected void Season_Selected_Change(object sender, EventArgs e) {
            string selectedSeason = selectSeasonList.SelectedValue;
            Session["SelectedSeason"] = selectedSeason;

            // Reload table or do any necessary logic
            //CreateSeasonTable();
            Util instance = new Util();
            HtmlTable rankTable = Util.CreateSeasonTable(Int32.Parse(selectedSeason));
            masterTablePanel.Controls.Add(rankTable); // Add the table to the master panel
        }

    }

}
