using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models.DatDota
{
    public class Details
    {
        [JsonProperty("profile")]
        public SummaryData Profile { get; set; }

        [JsonProperty("history")]
        public List<HistoryMatch> History { get; set; }
    }
}
