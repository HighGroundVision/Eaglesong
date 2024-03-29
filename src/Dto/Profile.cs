﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Dto
{
    public class Profile
    {
        [JsonProperty("account_id")]
        public uint AccountId { get; set; }

        [JsonProperty("persona")]
        public string Persona { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }
}
