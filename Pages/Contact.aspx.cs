using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SML {
    public partial class Contact : Page {
        protected void Page_Load(object sender, EventArgs e) {
            // Ensure the master page is correctly cast before accessing EnableDynamicBackground
            SiteMaster master = Master as SiteMaster;
            if (master != null) {
                master.EnableDynamicBackground = true; // Enable background effect for this page
            }
        }
    }
}