<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scoreboard.aspx.cs" Inherits="SML.Scoreboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   

    <main>
        
        <section aria-labelledby="aspnetTitle">
            
            <asp:Panel runat="server" ID="masterTablePanel" CssClass="master-table">
                <asp:DropDownList ID="selectSeasonList"
                    AutoPostBack="True"
                    OnSelectedIndexChanged="Season_Selected_Change"
                    runat="server"
                    CssClass="DropdownMenu scoreboard-dropdown">
                </asp:DropDownList>
            </asp:Panel>
            

        </section>

        

        
    </main>

</asp:Content>
