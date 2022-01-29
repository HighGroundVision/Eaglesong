using HGV.Eaglesong.Models.DatDota;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models
{
    public class PlayerSummary
    {
        [JsonProperty("account_id")]
        public uint AccountId { get; set; }

        [JsonProperty("steam_id")]
        public ulong SteamId { get; set; }

        [JsonProperty("persona")]
        public string Persona { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("overall_rank")]
        public int? OverallRank { get; set; } 

        [JsonProperty("rating")]
        public double? Rating { get; set; } 

        [JsonProperty("region")]
        public string Region { get; set; } 

        [JsonProperty("regional_rank")]
        public int? RegionalRank { get; set; } 

        [JsonProperty("win_rate")]
        public double Winrate { get; set; } 

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; } 
    }
}
