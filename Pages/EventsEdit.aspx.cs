using AjaxControlToolkit;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SML.Pages {
    public partial class EventsEdit : System.Web.UI.Page {
        private readonly EventsService _eventsService = new EventsService();
        private string _eventName;

        protected void Page_Load(object sender, EventArgs e) {
            if (Master is SiteMaster master) {
                master.EnableDynamicBackground = true;
            }

            LoadEventsData();

            //if (IsPostBack && ViewState["ShowModal"] != null && (bool)ViewState["ShowModal"]) {
            //    ShowModalPopup();
            //}
        }


        private void LoadEventsData() {
            string eventName = Request.QueryString["season"];


            if (string.IsNullOrEmpty(eventName)) {
                System.Diagnostics.Debug.WriteLine($"{eventName} event not found.");
                EventNameLabel.Text = $"Event not found.";
            }
            else if (_eventsService.CheckEventName(eventName) == false) {
                System.Diagnostics.Debug.WriteLine($"{eventName} event not found.");
                EventNameLabel.Text = $"{eventName} not found.";
            }
            else {

                // Temporarily disabling this to test edit events 

                //_eventName = eventName;
                //string authorizedEvent = HttpContext.Current.Session["AuthorizedEvent"] as string ?? string.Empty;

                //if (authorizedEvent == null || authorizedEvent != eventName) {
                //    System.Diagnostics.Debug.WriteLine($"{eventName} not currently authorized for this user.");
                //    EventPasswordContainer.Visible = true;
                //    passwordLabel.Text = $"Enter {eventName} Password:";
                //    return;
                //}
                //System.Diagnostics.Debug.WriteLine($"{eventName} authorized.");

                //EventPasswordContainer.Visible = false;
                //ViewState["eventData"] = null;


                EventNameLabel.Text = $"{eventName}";

                //PopulateAuthenticatedUI();
            }
        }

        protected void Submit_EventPassword(object sender, EventArgs e) {

            string eventPassword = passwordTextbox.Text;

            if (string.IsNullOrEmpty(eventPassword)) {
                eventErrorLabel.Text = "Event password cannot be empty.";
                return;
            }
            else if (eventPassword.Length > 50) {
                eventErrorLabel.Text = "Event password cannot exceed 50 characters length";
                return;
            }


            HttpContext.Current.Session["AuthorizedEvent"] = _eventName;
            Response.Redirect("/Pages/EventsEdit.aspx?season=" + HttpUtility.UrlEncode(_eventName));

        }


        //  ================================================================================
        //  Buttons for when the user is authenticated to edit Divisions, Players, Settings
        //  ================================================================================
        public void PopulateAuthenticatedUI() {
            Button EditDivisionButton = new Button {
                ID = "EditDivisionButton",
                Text = "Divisions",
                CssClass = "edit-button"
            };

            EditDivisionButton.Click += new EventHandler(Submit_EditDivision);

            Button EditPlayersButton = new Button {
                ID = "EditPlayersButton",
                Text = "Players",
                CssClass = "edit-button"
            };

            EditPlayersButton.Click += new EventHandler(Submit_EditPlayers);

            Button EditSettingsButton = new Button {
                ID = "EditSettingsButton",
                Text = "Settings",
                CssClass = "edit-button"
            };

            EditSettingsButton.Click += new EventHandler(Submit_EditSettings);

            Button QuitButton = new Button {
                ID = "QuitButton",
                Text = "Quit",
                CssClass = "edit-button"
            };

            QuitButton.Click += new EventHandler(Submit_Quit);

            AuthButtons.Controls.Add(EditDivisionButton);
            AuthButtons.Controls.Add(EditPlayersButton);
            AuthButtons.Controls.Add(EditSettingsButton);
            AuthButtons.Controls.Add(QuitButton);
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
