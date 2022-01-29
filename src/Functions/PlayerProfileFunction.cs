using HGV.Eaglesong.Dto;
using HGV.Eaglesong.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System;
using System.Threading.Tasks;

namespace HGV.Eaglesong
{
    public class PlayerProfileFunction
    {
        private readonly IPlayerService service;
        private readonly IAsyncCacheProvider<Profile> profileCache;
        private readonly IAsyncCacheProvider<Details> detailsCache;

        public PlayerProfileFunction(IPlayerService service, IAsyncCacheProvider<Profile> profileCache,  IAsyncCacheProvider<Details> detailsCache)
        {
            this.service = service;
            this.profileCache = profileCache;
            this.detailsCache = detailsCache;
        }

        [FunctionName("CheckIdentity")]
        public async Task<IActionResult> Check(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "check/{identity}")] HttpRequest req,
            string identity,
            ILogger log)
        {
            var id = new Models.SteamID();

            identity = identity.Trim();

            if (identity.StartsWith("https://windrun.io/players/"))
            {
                var value = identity.Replace("https://windrun.io/players/", "");
                var accountId = uint.Parse(value);
                id.Set(accountId, Models.EUniverse.Public, Models.EAccountType.Individual);
            }
            else if (identity.StartsWith("https://www.opendota.com/players/"))
            {
                var value = identity.Replace("https://www.opendota.com/players/", "");
                var accountId = uint.Parse(value);
                id.Set(accountId, Models.EUniverse.Public, Models.EAccountType.Individual);
            }
            else if (identity.StartsWith("https://dotabuff.com/players/"))
            {
                var value = identity.Replace("https://dotabuff.com/players/", "");
                var accountId = uint.Parse(value);
                id.Set(accountId, Models.EUniverse.Public, Models.EAccountType.Individual);
            }
            else if (identity.StartsWith("https://steamcommunity.com/profiles/"))
            {
                var value = identity.Replace("https://steamcommunity.com/profiles/", "");
                var steamId = ulong.Parse(value);
                id.SetFromUInt64(steamId);
            }
            else if (id.SetFromString(identity, Models.EUniverse.Public))
            {
                // parsed "STEAM_" into SteamID.
            }
            else if (id.SetFromSteam3String(identity))
            {
                // parsed "[X:1:2:3]" into SteamID.
            }
            else if (uint.TryParse(identity, out uint accountId))
            {
                id.Set(accountId, Models.EUniverse.Public, Models.EAccountType.Individual);
            }
            else if (ulong.TryParse(identity, out ulong steamId))
            {
                id.SetFromUInt64(steamId);
            }
            else
            {
                return new NotFoundResult();
            }

            var dto = new Check()
            {
                AccountId = id.AccountID,
                SteamId = id.ConvertToUInt64(),
            };
            return new OkObjectResult(dto);
        }

        [FunctionName("PlayerProfile")]
        public async Task<IActionResult> Profile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "profile/{account}")] HttpRequest req,
            uint account,
            ILogger log)
        {
            var item = await this.profileCache.TryGetAsync($"PlayerProfileFunction.Profile.{account}", default, false);
            if(item.Item1)
                return new OkObjectResult(item.Item2);

            var data = await this.service.GetProfile(account);
            if (data is null)
                return new NotFoundResult();

            var dto = new Profile()
            {
                AccountId = account,
                Persona = data.Persona,
                Avatar = data.Avatar,
            };
            var ttl = new Ttl(TimeSpan.FromMinutes(60));
            await this.profileCache.PutAsync($"PlayerProfileFunction.Profile.{account}", dto, ttl, default, false);

            return new OkObjectResult(dto);
        }

        [FunctionName("PlayerDetails")]
        public async Task<IActionResult> Details(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "details/{account}")] HttpRequest req,
            uint account,
            ILogger log)
        {
            var item = await this.detailsCache.TryGetAsync($"PlayerProfileFunction.Details.{account}", default, false);
            if(item.Item1)
                return new OkObjectResult(item.Item2);

            var profile = await this.service.GetProfile(account);
            if (profile is null)
                return new NotFoundResult();

            var history = await this.service.GetHistory(account);
            var dto = new Details()
            {
                Summary = profile,
                History = history
            };
            var ttl = new Ttl(TimeSpan.FromMinutes(60));
            await this.detailsCache.PutAsync($"PlayerProfileFunction.Details.{account}", dto, ttl, default, false);

            return new OkObjectResult(dto);
        }
    }
}
