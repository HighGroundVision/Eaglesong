using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Dto
{
    public class Check
    {
        [JsonProperty("account_id")]
        public uint AccountId { get; set; }

        [JsonProperty("steam_id")]
        public ulong SteamId { get; set; }
    }
}
