using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SML.Models {
    public class Season {
        public int SeasonID { get; set; } = 0;
        public string Name { get; set; }
        public string Status { get; set; }
        public List<Division> Divisions { get; set; } = new List<Division>();

        public Season(int id, string name, string status) {
            SeasonID = id;
            Name = name;
            Status = status;
        }


    }
}
