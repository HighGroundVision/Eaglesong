using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models.DatDota
{
     public class HistoryWinLoss   
     {
        [JsonProperty("losses")]
        public int Losses { get; set; } 

        [JsonProperty("winrate")]
        public object Winrate { get; set; } 

        [JsonProperty("wins")]
        public int Wins { get; set; } 
    }

    public class HistoryTeam    
    {
        [JsonProperty("abilities")]
        public List<int> Abilities { get; set; } 

        [JsonProperty("hero")]
        public int Hero { get; set; } 

        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("rating")]
        public double Rating { get; set; } 

        [JsonProperty("steamId")]
        public int AccountId { get; set; } 

        [JsonProperty("topX")]
        public int? TopX { get; set; } 

        [JsonProperty("winLoss")]
        public HistoryWinLoss WinLoss { get; set; } 
    }

    public class HistoryMatch    
    {
        [JsonProperty("dire")]
        public List<HistoryTeam> Dire { get; set; } 

        [JsonProperty("duration")]
        public int Duration { get; set; } 

        [JsonProperty("gameStart")]
        public DateTime GameStart { get; set; } 

        [JsonProperty("matchId")]
        public long MatchId { get; set; } 

        [JsonProperty("radiant")]
        public List<HistoryTeam> Radiant { get; set; } 

        [JsonProperty("radiantWin")]
        public bool RadiantWin { get; set; } 

        [JsonProperty("region")]
        public string Region { get; set; } 
    }

}
