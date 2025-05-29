using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Web.UI.WebControls.Image;

namespace SML {
    public partial class _Default : Page {
        private readonly EventsService _eventsService = new EventsService();
        private string _eventName;

        protected void Page_Load(object sender, EventArgs e) {
            if (Master is SiteMaster master) {
                master.EnableDynamicBackground = true;
            }
        }
        

        
        public void Submit_EditDivision(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Edit Division Click");
            //ShowModalPopup();
        }

        public void Submit_EditPlayers(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Edit Players Click");
            //ShowModalPopup();
        }

        public void Submit_EditSettings(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Edit Settings Click");
            ViewState["ShowModal"] = true;
            ShowModalPopup();
        }

        // Navigates back to normal view events page
        public void Submit_Quit(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Quit Click");
            Response.Redirect("/Pages/Events?season=" + HttpUtility.UrlEncode(_eventName));
        }

        public void ShowModalPopup() {
            Panel modalPanel = Build_Panel();
            modalPanel.ID = "ModalPanel";
            modalPanel.Attributes.Add("class", "modal-popup");

            ModalPopupExtender modalPopupExtender = new ModalPopupExtender {
                ID = "ModalPopupExtender",
                TargetControlID = "HiddenTargetButton",
                PopupControlID = modalPanel.ID,
                BackgroundCssClass = "modal-background",
                DropShadow = true,
            };

            modalPopupExtender.Controls.Add(modalPanel);

            this.Form.Controls.Add(modalPopupExtender);

            Button hiddenTargetButton = new Button {
                ID = "HiddenTargetButton",
                CssClass = "hidden-button"
            };

            this.Form.Controls.Add(hiddenTargetButton);

            modalPopupExtender.Show();

            // Register client-side script to add a class to the foreground element
            string script = @"
                document.addEventListener('DOMContentLoaded', function() {
                    var element = document.getElementById('elementId');
                    if (element) {
                        element.style.display = 'none';
                    }
                    var foregroundElement = document.getElementById('ModalPopupExtender_foregroundElement');
                    if (foregroundElement) {
                        foregroundElement.classList.add('modal-foreground');
                    }
                });
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "AddClassToForeground", script, true);
        }


        public Panel Build_Panel() {
            // Build overall container for object interactions
            Panel panel = new Panel {
                ID = "modal-content-container",
                CssClass = "modal-panel"
            };

            // Div for title
            Panel titlePanel = new Panel {
                ID = "modal-title-container",
                CssClass = "title-panel",
            };

            Label title = new Label {
                CssClass = "title-font d-flex flex-grow",
                Text = "Title Label",
                ID = "title"
            };

            titlePanel.Controls.Add(title);

            // Create a panel just for the options within it
            Panel mainPanel = new Panel {
                CssClass = "d-flex main-panel"
            };

            Panel listPanel = Build_Settings_List();
            Panel optionsPanel = Build_Options_Panel();
            mainPanel.Controls.Add(listPanel);
            mainPanel.Controls.Add(optionsPanel);

            Button ConfirmButton = new Button {
                Text = "Confirm"
            };
            ConfirmButton.Attributes["style"] = "background-color: green";
            ConfirmButton.Click += new EventHandler(this.Close_Popup);

            Button CancelButton = new Button {
                Text = "Cancel"
            };
            CancelButton.Attributes["style"] = "background-color: red";
            CancelButton.Click += new EventHandler(this.Close_Popup);

            panel.Controls.Add(titlePanel);
            panel.Controls.Add(mainPanel);
            panel.Controls.Add(ConfirmButton);
            panel.Controls.Add(CancelButton);

            return panel;
        }

        public void Close_Popup(object sender, EventArgs e) {
            ViewState["ShowModal"] = null;
        }

        public Panel Build_Settings_List() {
            Panel panel = new Panel {
                CssClass = "settings-list-panel text-font",
            };

            Label EventLabel = new Label {
                Text = "Event Type"
            };

            Label TiebreakerLabel = new Label {
                Text = "Tiebreaker"
            };

            Label MatchPointsLabel = new Label {
                Text = "Match Points"
            };

            panel.Controls.Add(EventLabel);
            panel.Controls.Add(TiebreakerLabel);
            panel.Controls.Add(MatchPointsLabel);

            return panel;
        }

        public Panel Build_Options_Panel() {
            Panel panel = new Panel {
                CssClass = "settings-options-panel text-font",
                ScrollBars = ScrollBars.Vertical,
            };


            Panel EventContainer = new Panel {
                CssClass = "container d-flex flex-column"
            };

            Label EventLabel = new Label {
                CssClass = "center-text align-self-center",
                Text = "Event Type"
            };

            Panel EventButtons = new Panel();

            RadioButton LeagueButton = new RadioButton {
                ID = "LeagueButton",
                CssClass = "radio-button",
                Text = "League",
                Visible = true,
                AutoPostBack = true
            };

            LeagueButton.CheckedChanged += new EventHandler(RadioButton_Click);

            RadioButton TournamentButton = new RadioButton {
                ID = "TournamentButton",
                CssClass = "radio-button",
                Text = "Tournament",
                Visible = true,
                AutoPostBack = true
            };

            TournamentButton.CheckedChanged += new EventHandler(RadioButton_Click);

            EventButtons.Controls.Add(LeagueButton);
            EventButtons.Controls.Add(TournamentButton);

            EventContainer.Controls.Add(EventLabel);
            EventContainer.Controls.Add(EventButtons);

            panel.Controls.Add(EventContainer);

            return panel;
        }

        private void LeagueButton_CheckedChanged(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        public Panel Build_RadioButton(string text) {
            Panel Container = new Panel {
                CssClass = "block"
            };

            RadioButton button = new RadioButton {
                CssClass = "checkbox",
            };

            button.CheckedChanged += new EventHandler(RadioButton_Click);


            Label label = new Label {
                Text = text
            };

            Container.Controls.Add(button);
            Container.Controls.Add(label);

            return Container;
        }

        public void RadioButton_Click(object sender, EventArgs e) {
            RadioButton clickedButton = (RadioButton)sender;
            Panel parentPanel = (Panel)clickedButton.Parent;

            foreach (Control control in parentPanel.Controls) {
                if (control is RadioButton radioButton && radioButton != clickedButton) {
                    radioButton.Checked = false;
                }
            }

            System.Diagnostics.Debug.WriteLine("RadioButton Click");
        }

    }
}
