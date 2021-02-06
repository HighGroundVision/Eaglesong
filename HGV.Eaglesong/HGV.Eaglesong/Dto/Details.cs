using HGV.Eaglesong.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Dto
{
    public class Details
    {
        [JsonProperty("summary")]
        public PlayerSummary Summary { get; set; }

        [JsonProperty("history")]
        public List<PlayerHistory> History { get; set; }
    }
}
