<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SML._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Event.css" asp-append-version="true"/>
    <link rel="stylesheet" type="text/css" href="/Content/site.css" asp-append-version="true"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="events-create-new">

    
    <section id="AuthenticatedContent" class="edit-button-container"  runat="server">
        <section class="menu-container" id="AuthButtons" runat="server">
            <asp:Label id="EventNameLabel" CssClass="edit-header" runat="server">Welcome To SpyParty Community Events</asp:Label>

            <asp:Button id="FeaturesButton" CssClass="menu-button" Text="Beta Features and Future Updates" OnClientClick="openModal('FeaturesModal'); return false;" runat="server"/>
            <asp:Button id="ReplaysButton" CssClass="menu-button" Text="How to Upload Replays" OnClientClick="openModal('ReplaysModal'); return false;" runat="server"/>
                        
            <asp:Button id="SettingsButton" CssClass="menu-button" Text="How to Host an Event" OnClientClick="openModal('AdminModal'); return false;" runat="server"/>
            <asp:Button id="QuitButton" CssClass="menu-button" Text="Contact Me" OnClick="Submit_Quit" runat="server"/>
        </section>
        <panel id="EditPanel" runat="server">
            <!-- Features Modal -->
            <div class="modal fade" id="FeaturesModal" tabindex="-1" aria-labelledby="FeaturesModalLabel" aria-hidden="true">
                <div class="modal-dialog d-flex flex-column justify-content-center align-items-center">
                    <div class="modal-panel modal-header p-3">
                        <!-- Settings Title Panel -->
                        <div class="title-panel">
                            <div class="title-font" id="FeaturesModalLabel">Site Content</div>
                        </div>
                        <div class="modal-content main-panel d-flex flex-row">

                            <!-- Features List Panel -->
                            <div id="features-list" class="settings-list-panel list-group">
                                <a class="list-group-item list-group-item-action" href="#beta-features"><h3 data-tip="Designate which type of event is being run.">Beta Features</h3></a>
                                <a class="list-group-item list-group-item-action" href="#full-release-features"><h3 data-tip="Apply different tiebreakers for the scoreboard.">Full Release Features</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-3"><h3 data-tip="Change the points applied to a Win, Tie, or Loss.">Section 3</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-4"><h3 data-tip="Determine what happens when a player forfeits during the event.">Section 4</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-5"><h3 data-tip="Dynamically add matches without registering the player. Anyone can upload replays of two players and they will be added as registered players.">Section 5</h3></a>
                            </div>
                            <div id="features-options" class="settings-options-column">
                                <div class="settings-options-panel" data-bs-spy="scroll" data-bs-target="settings-list" data-bs-offset="0" tabindex="0">
 
                                    <!-- Beta Features -->
                                    <div id="beta-features" class="option-div">
                                        <h3 class="option-heading">Current Beta Features</h3>
                                        <ol>
                                            <li>Upload match replays
                                                <ul>
                                                    <li>Anyone can upload the replays for a match within divisions/seasons.</li>
                                                    <li>.zip files can contain multiple matches within a single season across divisions.</li>
                                                    <li>Stores replays as blob files for later retrieval.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Live scoreboard
                                                <ul>
                                                    <li>Scoreboard orders can be determined by league administrator for tiebreakers and forfeits.</li>
                                                    <li>A player's points are updated upon new match uploads, but can be updated manually by TOs.</li>
                                                    <li>Users can submit their profile pictures to their TO for upload onto their profile.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Hosting leagues/tournaments
                                                <ul>
                                                    <li>Anyone can create and host an event.</li>
                                                    <li>Events are password protected, but multiple TOs can use the same password to modify events.</li>
                                                    <li>Settings restrictions can be tuned to meet TOs specific needs for editting permissions and replay uploads.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Viewing players
                                                <ul>
                                                    <li>All players participating in an event can be seen on the Events page, or a list of all players who have participated in all events.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                        </ol>
                                    </div>

                                    <!-- Full Release -->
                                    <div id="full-release-features" class="option-div">
                                        <h3 class="option-heading">Planned Full Release Features</h3>
                                        <ol>
                                            <li>Improved player statistics
                                                <ul>
                                                    <li>Swap out the current parser service created by LtHummus to webscrape Wobble's tool for more in-depth data.</li>
                                                    <li>Be able to pull specific player statistics across multiple events.</li>
                                                    <li>Stores replays as blob files for later retrieval.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Replay fetching
                                                <ul>
                                                    <li>Match replays can be searched for and downloaded.</li>
                                                    <li>Advanced filters can be applied to replay queries for game states.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Finer tuned event settings
                                                <ul>
                                                    <li>Event customization will be more customizable to support all league/event types.</li>
                                                    <li>More layers of permissions can be chosen between admins and players.</li>
                                                    <li>Ability to publish event rules and store .prop files for download.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Custom profiles
                                                <ul>
                                                    <li>All players can create their own account to manage their own profile.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                        </ol>
                                    </div>

                                </div>
                            </div>
                
                        </div> 
                    </div>
                    <div class="close-button-div">
                        <button type="button" class="button-close modal-button" data-bs-dismiss="modal" aria-label="Close">Close</button>
                    </div>
                </div>
            </div>

            <!-- Upload Replays Modal -->
            <div class="modal fade" id="ReplaysModal" tabindex="-1" aria-labelledby="ReplaysModalLabel" aria-hidden="true">
                <div class="modal-dialog d-flex flex-column justify-content-center align-items-center">
                    <div class="modal-panel modal-header p-3">
                        <!-- Settings Title Panel -->
                        <div class="title-panel">
                            <div class="title-font" id="ReplaysModalLabel">Upload Replays</div>
                        </div>
                        <div class="modal-content main-panel d-flex flex-row">

                            <!-- Features List Panel 
                            
                            <div id="replays-list" class="settings-list-panel list-group">
                                <a class="list-group-item list-group-item-action" href="#beta-features"><h3 data-tip="Designate which type of event is being run.">Beta Features</h3></a>
                                <a class="list-group-item list-group-item-action" href="#full-release-features"><h3 data-tip="Apply different tiebreakers for the scoreboard.">Full Release Features</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-3"><h3 data-tip="Change the points applied to a Win, Tie, or Loss.">Section 3</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-4"><h3 data-tip="Determine what happens when a player forfeits during the event.">Section 4</h3></a>
                                <a class="list-group-item list-group-item-action" href="#section-5"><h3 data-tip="Dynamically add matches without registering the player. Anyone can upload replays of two players and they will be added as registered players.">Section 5</h3></a>
                            </div>
                                -->

                            <div id="replay-options" class="settings-options-column">
                                <div class="settings-options-panel" data-bs-spy="scroll" data-bs-target="settings-list" data-bs-offset="0" tabindex="0">
 
                                    <!-- Beta Features -->
                                    <div id="replays" class="option-div">
                                        <h3 class="option-heading">Current Beta Features</h3>
                                        <ol>
                                            <li>Select the currently open event.</li>
                                            <img src="..\images\features\ReplayPage.png"/>
                                            <hr />
                                            <li>Review the scores to verify all the replays in the match were correctly uploaded.</li>
                                            <img src="..\images\features\ReplayUploads.png" />
                                            <hr />
                                            <li>Click to Confirm Upload and wait for the page icon to stop spinning. The page will reload when the upload is complete. Forcing the page to close before the upload finishes will require some files to be reuploaded.</li>
                                        </ol>
                                    </div>

                                    <!-- Full Release -->
                                    <div id="" class="option-div">
                                        <!-- Full Release 
                                        <h3 class="option-heading">Planned Full Release Features</h3>
                                        <ol>
                                            <li>Improved player statistics
                                                <ul>
                                                    <li>Swap out the current parser service created by LtHummus to webscrape Wobble's tool for more in-depth data.</li>
                                                    <li>Be able to pull specific player statistics across multiple events.</li>
                                                    <li>Stores replays as blob files for later retrieval.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Replay fetching
                                                <ul>
                                                    <li>Match replays can be searched for and downloaded.</li>
                                                    <li>Advanced filters can be applied to replay queries for game states.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Finer tuned event settings
                                                <ul>
                                                    <li>Event customization will be more customizable to support all league/event types.</li>
                                                    <li>More layers of permissions can be chosen between admins and players.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                            <li>Custom profiles
                                                <ul>
                                                    <li>All players can create their own account to manage their own profile.</li>
                                                </ul>
                                                <hr>
                                            </li>
                                        </ol>
                                        -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div> 

                    <div class="close-button-div">
                        <button type="button" class="button-close modal-button" data-bs-dismiss="modal" aria-label="Close">Close</button>
                    </div>
                </div>
            </div>

            <!-- Settings Modal -->
            <div class="modal fade" id="AdminModal" tabindex="-1" aria-labelledby="AdminModalLabel" aria-hidden="true">
                <div class="modal-dialog d-flex flex-column justify-content-center align-items-center">
                    <div class="modal-panel modal-header p-3">
                        <!-- Settings Title Panel -->
                        <div class="title-panel">
                            <div class="title-font" id="SettingsModalLabel">Event Settings</div>
                        </div>
                        <div class="modal-content main-panel d-flex flex-row">

                            <!-- Settings List Panel -->
                            <div id="settings-list" class="settings-list-panel list-group">
                                <a class="list-group-item list-group-item-action" href="#event-create"><h3 data-tip="Designate which type of event is being run.">Create an Event</h3></a>

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
                                    <div id="event-create" class="option-div">
                                        <h3 class="option-heading">Create Event</h3>
                                        <ol>
                                            <li>From the Events page click the Make Event button at the bottom of the list of events.</li>
                                            <img class="menu-image" src="..\images\features\MakeEvent.png" />
                                            <li>Enter the name of your event and hit Enter.</li>
                                            <img class="menu-image" src="..\images\features\ExampleName.png" />
                                            <li>Enter the password for your event and hit Enter. Only share the password to other tournament organizers. Players will be able to upload their own replays to the event.</li>
                                            <img class="menu-image" src="..\images\features\ExamplePassword.png" />
                                            <li>After setting the password for the event you'll be directed to the Edit Event page. Here you can tweak the settings for the event's format.</li>
                                            <img class="menu-image" src="..\images\features\ExampleAdmin.png" />
                                            <li>To manage the event and make any changes after it's been created you can go to the Event page, click the event's name, and scroll to the bottom to click Edit Event.</li>
                                            <img class="menu-image" src="..\images\features\EditEvent1.png" />
                                            <img style="height:250px" src="..\images\features\EditEvent23.png" />
                                            <li>Below I've included some of the event customizations available on the Edit Events page. Just in case you like to press buttons.</li>
                                        </ol>
                                    </div>

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

    <script>
        function openModal(modalId) {
            var modal = new bootstrap.Modal(document.getElementById(modalId));
            modal.show();
        }
    </script>
    <script src="js/EventsEdit.js"></script>

</main>
</asp:Content>

