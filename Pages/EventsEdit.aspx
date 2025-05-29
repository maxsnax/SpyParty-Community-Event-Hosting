<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EventsEdit.aspx.cs" Inherits="SML.Pages.EventsEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Event.css" asp-append-version="true"/>
    <link rel="stylesheet" type="text/css" href="/Content/site.css" asp-append-version="true"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="events-create-new">

        
    <section id="EventPasswordContainer" class="new-event-name-container" Visible="false" runat="server">
        <asp:Label id="eventErrorLabel" CssClass="label-red" runat="server">This feature has been temporarily disabled during beta.</asp:Label>
        <!--
        <asp:Label id="passwordLabel" CssClass="label-white" runat="server"></asp:Label>
        <asp:Panel ID="eventPasswordPanel" CssClass="panel" runat="server" DefaultButton="buttonContinue2">
            <asp:TextBox id="passwordTextbox" CssClass="textbox" MaxLength="50" runat="server"></asp:TextBox>
            <asp:Button ID="buttonContinue2" Style="display:none;" OnClick="Submit_EventPassword" runat="server" />
        </asp:Panel>
        <asp:Label ID="charErrorLabel" runat="server" CssClass="label-red"></asp:Label>
        -->
    </section>
    
    <section id="AuthenticatedContent" style="visibility:hidden;" class="edit-button-container"  runat="server">
        <section class="edit-button-container" id="AuthButtons" runat="server">
            <asp:Label id="EventNameLabel" CssClass="edit-header" runat="server"></asp:Label>

            <asp:Button id="DivisionsButton" CssClass="edit-button" Text="Divisions" OnClientClick="openModal('DivisionsModal'); return false;" runat="server"/>
            <asp:Button id="PlayersButton" CssClass="edit-button" Text="Players" OnClientClick="openModal('PlayersModal'); return false;" runat="server"/>
            <asp:Button id="MatchesButton" CssClass="edit-button" Text="Matches" OnClientClick="openModal('MatchesModal'); return false;" runat="server"/>                        
            <asp:Button id="SettingsButton" CssClass="edit-button" Text="Settings" OnClientClick="openModal('SettingsModal'); return false;" runat="server"/>
            <asp:Button id="QuitButton" CssClass="edit-button" Text="Quit" OnClick="Submit_Quit" runat="server"/>
        </section>
        <panel id="EditPanel" runat="server">
            <!-- Divisions Modal -->
            <div class="modal fade" id="DivisionsModal" tabindex="-1" aria-labelledby="DivisionsModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="DivisionsModalLabel">Edit Divisions</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <!-- Your divisions content goes here -->
                            <p>Manage divisions for the event.</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Players Modal -->
            <div class="modal fade" id="PlayersModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog d-flex flex-column justify-content-center align-items-center">
                    <div class="modal-panel modal-header p-3">
                        <!-- Settings Title Panel -->
                        <div class="title-panel">
                            <div class="title-font" id="PlayersModalLabel">Player Settings</div>
                        </div>
                        <div class="modal-content main-panel d-flex flex-row">

                            <!-- Options List Panel -->
                            <div id="players-list" class="settings-list-panel list-group">
                                <a class="list-group-item list-group-item-action" href="#event-type-options"><h3 data-tip="Designate which type of event is being run.">Event Type</h3></a>
                                <a class="list-group-item list-group-item-action" href="#tiebreaker-options"><h3 data-tip="Apply different tiebreakers for the scoreboard.">Tiebreaker</h3></a>
                                <a class="list-group-item list-group-item-action" href="#wtl-values"><h3 data-tip="Change the points applied to a Win, Tie, or Loss.">W/T/L Values</h3></a>
                                <a class="list-group-item list-group-item-action" href="#forfeit-processing"><h3 data-tip="Determine what happens when a player forfeits during the event.">Forfeit Processing</h3></a>
                                <a class="list-group-item list-group-item-action" href="#upload-restrictions"><h3 data-tip="Dynamically add matches without registering the player. Anyone can upload replays of two players and they will be added as registered players.">Upload Restrictions</h3></a>
                            </div>
                            <div id="players-options" class="settings-options-column">
                                 <asp:UpdatePanel ID="PlayersUpdatePanel" runat="server">
                                     <ContentTemplate>
                                         <div id="Players-scrollable" class="grid-container">
                                             <asp:GridView ID="PlayersGridView" runat="server" AutoGenerateColumns="False"
                                                    DataKeyNames="player_name"
                                                    OnRowDataBound="PlayersGridView_RowDataBound"
                                                    OnRowEditing="Players_RowEditing"
                                                    OnRowCancelingEdit="Players_RowCancelingEdit"
                                                    OnRowUpdating="Players_RowUpdating"
                                                    CssClass="scrollable-grid">
                                                <Columns>
                                                    <asp:BoundField DataField="player_name" HeaderText="Player" ReadOnly="True" />
                                                    <asp:BoundField DataField="division_name" HeaderText="Division" />
                                                    
                                                    <asp:TemplateField HeaderText="Forfeit">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="PlayerForfeit" runat="server"
                                                                DataValueField="forfeit"
                                                                DataTextField="Player Status"
                                                                OnClientClick="updateDropdownColor(this)"
                                                                AutoPostBack="false"
                                                                OnSelectedIndexChanged="Forfeit_SelectedIndexChanged">
                                                                <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                                <asp:ListItem Text="Forfeit" Value="Forfeit"></asp:ListItem>                                            
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ShowEditButton="True" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!---------------------->
                                <!-- Hover Option Text -->
                                <!---------------------->
                                <div id="player-hover-text-div">
                                    <p id="player-hover-text">
                                        Hover over the options to see more information.
                                    </p>
                                </div>
                            </div>
                        </div> 
                    </div>
                    <div class="close-button-div">
                        <button type="button" class="button-close modal-button" data-bs-dismiss="modal" aria-label="Close">Close</button>
                    </div>
                </div>
            </div>

            <!-- Matches Modal -->
            <div class="modal fade" id="MatchesModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="MatchesModalLabel">Edit Divisions</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <!-- Your divisions content goes here -->
                            <p>Manage matches for the event.</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Settings Modal -->
            <div class="modal fade" id="SettingsModal" tabindex="-1" aria-labelledby="SettingsModalLabel" aria-hidden="true">
                <div class="modal-dialog d-flex flex-column justify-content-center align-items-center">
                    <div class="modal-panel modal-header p-3">
                        <!-- Settings Title Panel -->
                        <div class="title-panel">
                            <div class="title-font" id="SettingsModalLabel">Event Settings</div>
                        </div>
                        <div class="modal-content main-panel d-flex flex-row">

                            <!-- Settings List Panel -->
                            <div id="settings-list" class="settings-list-panel list-group">
                                <a class="list-group-item list-group-item-action" href="#event-type-options"><h3 data-tip="Designate which type of event is being run.">Event Type</h3></a>
                                <a class="list-group-item list-group-item-action" href="#tiebreaker-options"><h3 data-tip="Apply different tiebreakers for the scoreboard.">Tiebreaker</h3></a>
                                <a class="list-group-item list-group-item-action" href="#wtl-values"><h3 data-tip="Change the points applied to a Win, Tie, or Loss.">W/T/L Values</h3></a>
                                <a class="list-group-item list-group-item-action" href="#forfeit-processing"><h3 data-tip="Determine what happens when a player forfeits during the event.">Forfeit Processing</h3></a>
                                <a class="list-group-item list-group-item-action" href="#upload-restrictions"><h3 data-tip="Dynamically add matches without registering the player. Anyone can upload replays of two players and they will be added as registered players.">Upload Restrictions</h3></a>
                            </div>
                            <div id="settings-options" class="settings-options-column">
                                <div class="settings-options-panel" data-bs-spy="scroll" data-bs-target="settings-list" data-bs-offset="0" tabindex="0">
                                    <!---------------------->
                                    <!-- Settings Options -->
                                    <!---------------------->
                                    <!-- Event Type -->
                                    <div id="event-type-options" class="option-div">
                                        <h3 class="option-heading">Event Type</h3>
                                        <asp:RadioButtonList runat="server">
                                            <asp:ListItem class="RadioButton">Tournament</asp:ListItem>
                                            <asp:ListItem class="RadioButton">League</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <!-- Tiebreakers -->
                                    <div id="tiebreaker-options" class="option-div">
                                        <h3 class="option-heading">Tiebreaker</h3>
                                        
                                    </div>
                                    <!-- W/T/L Values -->
                                    <div id="wtl-values" class="option-div">
                                        <h3 class="option-heading">W/T/L Values</h3>
                                        <!-- Add in more divs and buttons in js file -->
                                    </div>
                                    <div id="forfeit-processing" class="option-div">
                                        <h3 class="option-heading">Forfeit Processing</h3>
                                        <asp:RadioButtonList runat="server">
                                            <asp:ListItem class="RadioButton">Automatically drop player from event</asp:ListItem>
                                            <asp:ListItem class="RadioButton">Allow forfeit from a single match</asp:ListItem>
                                        </asp:RadioButtonList>                                    
                                    </div>
                                    <!-- Player Restrictions on Match Upload -->
                                    <div id="upload-restrictions" class="option-div">
                                        <h3 class="option-heading">Upload Restrictions</h3>
                                        <asp:RadioButtonList runat="server">
                                            <asp:ListItem class="RadioButton">Allow unregistered players to upload matches</asp:ListItem>
                                            <asp:ListItem class="RadioButton">Require players be added to this Event to upload matches</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                </div>
                                <!---------------------->
                                <!-- Hover Option Text -->
                                <!---------------------->
                                <div id="hover-text-div">
                                    <p id="hover-text">
                                        Hover over the options to see more information.
                                    </p>
                                </div>
                            </div>
                            
                        </div> 
                    </div>
                    <div class="close-button-div">
                        <button type="button" class="button-close modal-button" data-bs-dismiss="modal" aria-label="Close">Close</button>
                    </div>
                </div>
            </div>
        </panel>

        <asp:HiddenField ID="hfUserAuthorized" runat="server" />
    </section>
    <script type="text/javascript">
        function updateDropdownColor(dropdown) {
            // Get the selected value
            var selectedValue = dropdown.value;

            // Change the background color based on the selected value
            if (selectedValue === "Forfeit") {
                dropdown.style.backgroundColor = "red"; // Red for Forfeit
                dropdown.style.color = "white"; // White text for readability
            } else if (selectedValue === "Active") {
                dropdown.style.backgroundColor = "green"; // Green for Active
                dropdown.style.color = "white"; // White text for readability
            } else {
                // Default styling if necessary
                dropdown.style.backgroundColor = "white"; // Default white background
                dropdown.style.color = "black"; // Default black text
            }
        }
    </script>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            var textboxes = [
                { input: "<%= passwordTextbox.ClientID %>", error: "<%= charErrorLabel.ClientID %>" }
        ];

        textboxes.forEach(function (item) {
            var textbox = document.getElementById(item.input);
            var errorLabel = document.getElementById(item.error);

            if (textbox) {
                textbox.addEventListener("input", function () {
                    var regex = /^[a-zA-Z0-9\s]{1,50}$/;
                    if (!regex.test(textbox.value)) {
                        errorLabel.innerText = "Only letters and numbers are allowed. Max 50 characters.";
                        textbox.value = textbox.value.replace(/[^a-zA-Z0-9\s]/g, "").substring(0, 50);
                    } else {
                        errorLabel.innerText = "";
                    }
                });
            }
        });

    });
    </script>
    <script>
        function openModal(modalId) {
            var modal = new bootstrap.Modal(document.getElementById(modalId));
            modal.show();
        }
    </script>
    <script src="js/EventsEdit.js"></script>

</main>
</asp:Content>

