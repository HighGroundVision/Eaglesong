using HGV.Daedalus;
using HGV.Daedalus.Models;
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
        Task<PlayerSummary> GetSummary(int accountId);
        Task<Profile> GetProfile(int accountId);
        Task<List<PlayerHistory>> GetHistory(int accountId);
    }

    public class PlayerService : IPlayerService
    {
        private readonly IDotaApiClient dotaClient;
        private readonly HttpClient httpClient;

        public PlayerService(IDotaApiClient client, IHttpClientFactory factory)
        {
            this.dotaClient = client;
            this.httpClient = factory.CreateClient("Datdota");
        }

        public async Task<PlayerSummary> GetSummary(int accountId)
        {
            var steamId = ConvertId(accountId);
            var steamProfile = await SteamProfile(steamId);
            var dotaProfile = await DotaProfile(accountId);
            var profile = new PlayerSummary()
            {
                AccountId = accountId,
                Avatar = steamProfile.AvatarLarge,
                OverallRank = dotaProfile.OverallRank,
                Persona = steamProfile.Persona,
                Rating = dotaProfile.Rating,
                Region = dotaProfile.Region,
                RegionalRank = dotaProfile.RegionalRank,
                SteamId = steamId,
                Winrate = dotaProfile.WinLoss.Winrate,
                Wins = dotaProfile.WinLoss.Wins,
                Losses = dotaProfile.WinLoss.Losses,
            };
            return profile;
        }

        public async Task<Profile> GetProfile(int accountId)
        {
            var steamId = ConvertId(accountId);
            var steamProfile = await SteamProfile(steamId);
            return steamProfile;
        }

        public async Task<List<PlayerHistory>> GetHistory(int accountId)
        {
            var history = await DotaHistory(accountId);
            var collection = new List<PlayerHistory>();

            foreach (var item in history)
            {
                var match = await this.dotaClient.GetMatchDetails((ulong)item.MatchId);
                var player = match.Players.Find(_ => _.AccountId == accountId);

                foreach (var team in item.Radiant)
                {
                    if(team.AccountId == accountId)
                    {
                        var data = new PlayerHistory()
                        {
                            MatchId = item.MatchId,
                            When = (DateTime.UtcNow - item.GameStart).Days,
                            Hero = team.Hero,
                            Abilities = team.Abilities,
                            Victory = item.RadiantWin == true,
                            Region = item.Region,
                            Kills = player?.Kills ?? 0,
                            Deaths = player?.Deaths ?? 0,
                            Assists = player?.Assists ?? 0,
                        };
                        collection.Add(data);
                    }
                }
                foreach (var team in item.Dire)
                {
                    if(team.AccountId == accountId)
                    {
                        var data = new PlayerHistory()
                        {
                            MatchId = item.MatchId,
                            When = (DateTime.UtcNow - item.GameStart).Days,
                            Hero = team.Hero,
                            Abilities = team.Abilities,
                            Victory = item.RadiantWin == false,
                            Region = item.Region,
                            Kills = player?.Kills ?? 0,
                            Deaths = player?.Deaths ?? 0,
                            Assists = player?.Assists ?? 0,
                        };
                        collection.Add(data);
                    }
                }
            }

            return collection;
        }

         private static ulong ConvertId(int account) {
            ulong input = (ulong)account;
            if (input < 1L)
                return 0;
            else
                return input + 76561197960265728L;
        }

        private async Task<Profile> SteamProfile(ulong steamId)
        {
            var profile = await this.dotaClient.GetPlayerSummary(steamId);
            return profile;
        }

        private async Task<SummaryData> DotaProfile(int accountId)
        {
            var url = new Uri(httpClient.BaseAddress + $"players/{accountId}");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetPolicyExecutionContext(new Context($"PlayerService.Profile.{accountId}"));
            var response = await this.httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new NotSupportedException();

            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<SummaryRoot>(json);
            return root.SummaryData;
        }

        private async Task<List<HistoryMatch>> DotaHistory(int accountId)
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
