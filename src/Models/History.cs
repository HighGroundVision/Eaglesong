using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong.Models.DatDota
{

    public class Record
    {
        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("winrate")]
        public double? Winrate { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }
    }

    public class HistoryPlayer
    {
        [JsonProperty("abilities")]
        public List<int> Abilities { get; set; }

        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        [JsonProperty("gpm")]
        public int Gpm { get; set; }

        [JsonProperty("hero")]
        public int Hero { get; set; }

        [JsonProperty("heroDamage")]
        public int HeroDamage { get; set; }

        [JsonProperty("heroHealing")]
        public int HeroHealing { get; set; }

        [JsonProperty("items")]
        public List<int> Items { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; }

        [JsonProperty("lastHits")]
        public int LastHits { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("steamId")]
        public long SteamId { get; set; }

        [JsonProperty("topX")]
        public int? TopX { get; set; }

        [JsonProperty("winLoss")]
        public Record WinLoss { get; set; }

        [JsonProperty("xpm")]
        public int Xpm { get; set; }
    }

    public class IgnoredSpell
    {
        [JsonProperty("abilityId")]
        public int AbilityId { get; set; }
    }

    public class Pick
    {
        [JsonProperty("abilityId")]
        public int AbilityId { get; set; }

        [JsonProperty("pickOrder")]
        public int PickOrder { get; set; }
    }

    public class HistoryMatch
    {
        [JsonProperty("dire")]
        public List<HistoryPlayer> Dire { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("gameStart")]
        public DateTime GameStart { get; set; }

        [JsonProperty("ignoredSpells")]
        public List<IgnoredSpell> IgnoredSpells { get; set; }

        [JsonProperty("matchId")]
        public ulong MatchId { get; set; }

        [JsonProperty("picks")]
        public List<Pick> Picks { get; set; }

        [JsonProperty("radiant")]
        public List<HistoryPlayer> Radiant { get; set; }

        [JsonProperty("radiantWin")]
        public bool RadiantWin { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }

}
