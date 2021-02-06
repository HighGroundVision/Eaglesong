using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Dto
{
    public class Profile
    {
        [JsonProperty("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("steam_id")]
        public ulong SteamId { get; set; }

        [JsonProperty("persona")]
        public string Persona { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }
}
