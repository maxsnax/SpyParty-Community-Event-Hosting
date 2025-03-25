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
        <asp:Label id="eventErrorLabel" CssClass="label-red" runat="server"></asp:Label>
        <asp:Label id="passwordLabel" CssClass="label-white" runat="server"></asp:Label>
        <asp:Panel ID="eventPasswordPanel" CssClass="panel" runat="server" DefaultButton="buttonContinue2">
            <asp:TextBox id="passwordTextbox" CssClass="textbox" MaxLength="50" runat="server"></asp:TextBox>
            <asp:Button ID="buttonContinue2" Style="display:none;" OnClick="Submit_EventPassword" runat="server" />
        </asp:Panel>
        <asp:Label ID="charErrorLabel" runat="server" CssClass="label-red"></asp:Label>
    </section>
    
    <section id="AuthenticatedContent" class="edit-button-container"  runat="server">
        <section class="edit-button-container" id="AuthButtons" runat="server">
            <asp:Label id="EventNameLabel" CssClass="edit-header" runat="server"></asp:Label>

            <asp:Button id="DivisionsButton" CssClass="edit-button" Text="Divisions" OnClientClick="openModal('DivisionsModal'); return false;" runat="server"/>
            <asp:Button id="PlayersButton" CssClass="edit-button" Text="Players" OnClientClick="openModal('PlayersModal'); return false;" runat="server"/>
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
            <div class="modal fade" id="PlayersModal" tabindex="-1" aria-labelledby="PlayersModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="PlayersModalLabel">Edit Players</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <!-- Your players content goes here -->
                            <p>Manage players for the event.</p>
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
                            </div>
                            <div id="settings-options" class="settings-options-column">
                                <div class="settings-options-panel" data-bs-spy="scroll" data-bs-target="settings-list" data-bs-offset="0" tabindex="0"> <!--class="modal-body" -->
                                    <!---------------------->
                                    <!-- Settings Options -->
                                    <!---------------------->
                                    <!-- Event Type -->
                                    <div id="scrollspyHeading1" class="option-div">
                                        <h3 id="event-type-options" class="option-heading">Event Type</h3>
                                        <asp:RadioButtonList runat="server">
                                            <asp:ListItem>Tournament</asp:ListItem>
                                            <asp:ListItem>League</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <!-- Tiebreakers -->
                                    <div id="tiebreaker-container" class="option-div">
                                        <h3 id="tiebreaker-options" class="option-heading">Tiebreaker</h3>
                                        
                                    </div>
                                    <!-- W/T/L Values -->
                                    <div id="scrollspyHeading3" class="option-div">
                                        <h3 id="wtl-values" class="option-heading">W/T/L Values</h3>
                                        <!-- Add in more divs and buttons in js file -->
                                    </div>
                                    <div id="scrollspyHeading4" class="option-div">
                                        <h3 id="forfeit-processing" class="option-heading">Forfeit Processing</h3>
                                        <asp:RadioButtonList runat="server">
                                            <asp:ListItem>Automatically drop player from event</asp:ListItem>
                                            <asp:ListItem>Allow forfeit from a single match</asp:ListItem>
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

