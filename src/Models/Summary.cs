using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models.DatDota
{
    public class SummaryWinLoss    
    {
        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("winrate")]
        public double Winrate { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }
    }

    public class SummaryData    
    {
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("lastMatch")]
        public DateTime LastMatch { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("overallRank")]
        public int? OverallRank { get; set; }

        [JsonProperty("percentile")]
        public double? Percentile { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("regionalRank")]
        public int? RegionalRank { get; set; }

        [JsonProperty("steamId")]
        public int? SteamId { get; set; }

        [JsonProperty("winLoss")]
        public SummaryWinLoss WinLoss { get; set; }
    }

    public class SummaryRoot    
    {
        [JsonProperty("data")]
        public SummaryData SummaryData { get; set; } 
    }


}
