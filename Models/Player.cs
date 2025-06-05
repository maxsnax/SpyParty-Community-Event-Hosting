using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static SML.Models.Replays;

namespace SML.Models {
    public class Player {
        public int PlayerID { get; set; } = 0;
        public string Username { get; set; } = null;
        public string Name { get; set; } = null;
        public int Forfeit { get; set; } = 0;
        public int Season { get; set; } = 0;
        public string SeasonName { get; set; } = null;
        public int Division { get; set; } = 0;
        public string DivisionName { get; set; } = null;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
        public int Points => (Wins * 2) + Ties;
        public int? LoadOrder { get; set; } = 0;
        public Results Results { get; set; } = new Results();

        public Player() { }

        public Player(string name) {
            Name = name;
        }

        public void Add(Player other) {
            Wins += other.Wins;
            Ties += other.Ties;
            Losses += other.Losses;
            Results.Add(other.Results);
        }

        public void UpdatePlayerInfo(Player SQL) {
            if (SQL == null) return;
            // Update only if the SQL object has valid data
            PlayerID = SQL.PlayerID != 0 ? SQL.PlayerID : PlayerID;
            Username = SQL.Username ?? Username;
            Name = SQL.Name ?? Name;
            Forfeit = SQL.Forfeit != 0 ? SQL.Forfeit : Forfeit;
            Season = SQL.Season != 0 ? SQL.Season : Season;
            SeasonName = SQL.SeasonName ?? SeasonName;
            Division = SQL.Division != 0 ? SQL.Division : Division;
            DivisionName = SQL.DivisionName ?? DivisionName;
            Wins = SQL.Wins != 0 ? SQL.Wins : Wins;
            Losses = SQL.Losses != 0 ? SQL.Losses : Losses;
            Ties = SQL.Ties != 0 ? SQL.Ties : Ties;
            LoadOrder = SQL.LoadOrder.HasValue ? SQL.LoadOrder.Value : LoadOrder;
        }

        public override string ToString() {
            return $"PlayerID: {PlayerID}, " +
                   $"Username: {Username ?? "N/A"}, " +
                   $"Name: {Name ?? "N/A"}, " +
                   $"Forfeit: {Forfeit}, " +
                   $"Season: {Season}, " +
                   $"SeasonName: {SeasonName ?? "N/A"}, " +
                   $"Division: {Division}, " +
                   $"DivisionName: {DivisionName ?? "N/A"}, " +
                   $"Wins: {Wins}, " +
                   $"Losses: {Losses}, " +
                   $"Ties: {Ties}, " +
                   $"Points: {Points}, " +
                   $"Results: {Results}" +
                   $"LoadOrder: {LoadOrder}";
        }


    }

}
