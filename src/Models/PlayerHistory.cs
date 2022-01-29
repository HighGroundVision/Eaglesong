using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models
{
    public class PlayerHistory
    {
        [JsonProperty("match_id")]
        public long MatchId { get; set; }

        [JsonProperty("when")]
        public int When { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; } 

        [JsonProperty("victory")]
        public bool Victory { get; set; } 

        [JsonProperty("hero")]
        public int Hero { get; set; } 

        [JsonProperty("abilities")]
        public List<int> Abilities { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; } 

        [JsonProperty("deaths")]
        public int Deaths { get; set; } 

        [JsonProperty("assists")]
        public int Assists { get; set; } 
    }
}
