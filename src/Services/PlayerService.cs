using HGV.Eaglesong.Models;
using HGV.Eaglesong.Models.DatDota;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HGV.Eaglesong.Services
{
    public interface IPlayerService
    { 
        Task<PlayerSummary> GetProfile(uint accountId);
        Task<List<PlayerHistory>> GetHistory(uint accountId);
    }

    public class PlayerService : IPlayerService
    {
        private readonly HttpClient httpClient;

        public PlayerService(IHttpClientFactory factory)
        {
            this.httpClient = factory.CreateClient("Windrun");
        }

        public async Task<PlayerSummary> GetProfile(uint accountId)
        {
            var summary = await DotaProfile(accountId);
            if (summary == null)
                return null;

            var profile = new PlayerSummary()
            {
                AccountId = accountId,
                Avatar = summary.Avatar,
                OverallRank = summary.OverallRank,
                Persona = summary.Nickname,
                Rating = summary.Rating,
                Region = summary.Region,
                RegionalRank = summary.RegionalRank,
                Winrate = summary.WinLoss.Winrate,
                Wins = summary.WinLoss.Wins,
                Losses = summary.WinLoss.Losses,
            };
            return profile;
        }

        public async Task<List<PlayerHistory>> GetHistory(uint accountId)
        {
            static void AddPlayer(List<PlayerHistory> collection, HistoryMatch m, HistoryPlayer p, bool v)
            {
                var data = new PlayerHistory()
                {
                    MatchId = m.MatchId,
                    When = (DateTime.UtcNow - m.GameStart).Days,
                    Victory = v,
                    Region = m.Region,
                    Hero = p.Hero,
                    Abilities = p.Abilities,
                    Kills = p.Kills,
                    Deaths = p.Deaths,
                    Assists = p.Assists,
                };
                collection.Add(data);
            }

            var history = await DotaHistory(accountId);
            var collection = new List<PlayerHistory>();

            foreach (var m in history)
            {
                foreach (var p in m.Radiant)
                    AddPlayer(collection, m, p, m.RadiantWin == true);

                foreach (var p in m.Dire)
                    AddPlayer(collection, m, p, m.RadiantWin == false);
            }

            return collection;
        }

        private async Task<SummaryData> DotaProfile(uint accountId)
        {
            var url = new Uri(httpClient.BaseAddress + $"players/{accountId}");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetPolicyExecutionContext(new Context($"PlayerService.Profile.{accountId}"));
            var response = await this.httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new NotSupportedException();

            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<SummaryRoot>(json);

            if (root.SummaryData.SteamId != accountId)
                return null;
            else 
                return root.SummaryData;
        }

        private async Task<List<HistoryMatch>> DotaHistory(uint accountId)
        {
            var url = new Uri(httpClient.BaseAddress + $"players/{accountId}/matches");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetPolicyExecutionContext(new Context($"PlayerService.History.{accountId}"));
            var response = await this.httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new NotSupportedException();

            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<List<HistoryMatch>>(json);
            return root;
        }
    }
}
