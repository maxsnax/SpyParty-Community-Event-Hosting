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
        public List<Division> Divisions { get; set; } = new List<Division>();

        public Season(int id, string name, string status, int unregistered_upload) {
            SeasonID = id;
            Name = name;
            Status = status;
            UnregisteredUpload = unregistered_upload;
        }

        public override string ToString() {
            return $"SeasonID: {SeasonID}, " +
                   $"Name: {Name}, " +
                   $"Status: {Status}, " +
                   $"UnregisteredUpload: {UnregisteredUpload}, " +
                   $"Divisions: [" + string.Join("; ", Divisions.Select(d => d.ToString())) + "]";
        }

    }
}
