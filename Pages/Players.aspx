﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Players.aspx.cs" Inherits="SML.Players" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
    body {
        background-color: #D9D9BF;
    }
    </style>

    <main class="players-page">
    
        <section id="playerProfile" class="row playerProfileCard" CssClass="playerProfileCard"  runat="server" aria-labelledby="aspnetTitle">
            <asp:Image class="profilePicture" ID="playerProfilePhoto" width="150px" Height="150px" runat="server"></asp:Image>
            <asp:TableCell ID="playerPhotoCell" runat="server"></asp:TableCell>
            <asp:Label ID="lblPlayerName" runat="server" Font-Size="20px"></asp:Label>
            <asp:Label ID="lblPlayerUsername" runat="server" Font-Size="14px"></asp:Label>
            <!--
            <asp:DropDownList ID="selectSeasonList" CssClass="DropdownMenu profileDropdown" runat="server"></asp:DropDownList>
            -->
            <asp:GridView ID="MatchView" runat="server"></asp:GridView>

            <Panel id="playerEventsPanel" class="master-table" runat="server">

            </Panel>
        </section>


        <%-- Contains the Lobby Menu --%>
        <section ID="LobbyMenu" runat="server" class="lobby-container">

            <section>
                <asp:Label ID="LobbyLabel" runat="server" CssClass="lobby-header">Lobby</asp:Label>
                <section id="lobbyInstructionsRow" class="row" runat="server">
                    <asp:Label ID="PressLabel1" runat="server" CssClass="lobby-instruction" style="font-size:12px;">Press</asp:Label>    
                    <asp:Image ID="Mouse" Cssclass="mouse-image" runat="server" ImageUrl="~/images/icons/mouse.png" />
                    <asp:Label ID="PressLabel2" runat="server" CssClass="lobby-instruction">to view player or sort columns.</asp:Label>
                </section>
            </section>

            <div style="overflow-x: auto; max-width: 100%">
                <asp:GridView ID="PlayerGridView" runat="server" AutoGenerateColumns="false" AllowSorting="true" CssClass="players-grid-table"
                    HeaderStyle-CssClass="players-grid-table" RowStyle-CssClass="player" OnSorting="PlayerGridView_Sorting" OnRowDataBound="PlayerGridView_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="player_name" HeaderText="Player" HeaderStyle-CssClass="header-col truncate" ItemStyle-CssClass="column-data truncate" sortexpression="player_name"/>
                        <asp:BoundField DataField="division_name" HeaderText="Division" HeaderStyle-CssClass="header-col truncate" ItemStyle-CssClass="column-data truncate" sortexpression="division_name"/>
                        <asp:BoundField DataField="season_name" HeaderText="Season" HeaderStyle-CssClass="header-col truncate" ItemStyle-CssClass="column-data" sortexpression="season_name"/>
                        <asp:BoundField DataField="win" HeaderText="Wins" HeaderStyle-CssClass="player-stat truncate" ItemStyle-CssClass="player-stat" sortexpression="win"/>
                        <asp:BoundField DataField="tie" HeaderText="Ties" HeaderStyle-CssClass="player-stat truncate" ItemStyle-CssClass="player-stat" sortexpression="tie"/>
                        <asp:BoundField DataField="loss" HeaderText="Losses" HeaderStyle-CssClass="player-stat truncate" ItemStyle-CssClass="player-stat" sortexpression="loss"/>
                    </Columns>
                </asp:GridView>
            </div>

        </section>


    </main>

    <!-- jQuery Script to Apply Hover Effect -->
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id*=PlayerGridView] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover-row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover-row");
            });
        });
    </script>

</asp:Content>
