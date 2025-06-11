using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SML.Models {
    public class Season {
        public int SeasonID { get; set; } = 0;
        public string Name { get; set; }
        public string Status { get; set; }
        public int UnregisteredUpload { get; set; } = 0;
        public int ScoreboardVisible { get; set; } = 1;
        public List<Division> Divisions { get; set; } = new List<Division>();

        public Season(int id, string name, string status, int unregistered_upload) {
            SeasonID = id;
            Name = name;
            Status = status;
            UnregisteredUpload = unregistered_upload;
            ScoreboardVisible = 1;
        }

        public Season(int id, string name, string status, int unregistered_upload, int scoreboardVisible) {
            SeasonID = id;
            Name = name;
            Status = status;
            UnregisteredUpload = unregistered_upload;
            ScoreboardVisible = scoreboardVisible;
        }

        public override string ToString() {
            return $"SeasonID: {SeasonID}, " +
                   $"Name: {Name}, " +
                   $"Status: {Status}, " +
                   $"UnregisteredUpload: {UnregisteredUpload}, " +
                   $"ScoreboardVisible: {ScoreboardVisible}, " +
                   $"Divisions: [" + string.Join("; ", Divisions.Select(d => d.ToString())) + "]";
        }

    }
}
