using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HGV.Daedalus;
using Polly.Registry;
using Polly;
using System.Net.Http;
using HGV.Eaglesong.Models;
using System.Collections.Generic;
using HGV.Eaglesong.Services;
using Microsoft.Extensions.Caching.Distributed;
using Polly.Caching;
using HGV.Eaglesong.Dto;

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

        [FunctionName("PlayerProfile")]
        public async Task<IActionResult> Profile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "profile/{account}")] HttpRequest req,
            int account,
            ILogger log)
        {
            var item = await this.profileCache.TryGetAsync($"PlayerProfileFunction.Profile.{account}", default, false);
            if(item.Item1)
            {
                return new OkObjectResult(item.Item2);
            }
            else
            {
                var data = await this.service.GetProfile(account);
                var dto = new Profile()
                {
                    SteamId = data.SteamId,
                    AccountId = account,
                    Persona = data.Persona,
                    Avatar = data.AvatarLarge,
                };
                var ttl = new Ttl(TimeSpan.FromMinutes(60));
                await this.profileCache.PutAsync($"PlayerProfileFunction.Profile.{account}", dto, ttl, default, false);
                return new OkObjectResult(dto);
            }
        }

        [FunctionName("PlayerDetails")]
        public async Task<IActionResult> Details(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "details/{account}")] HttpRequest req,
            int account,
            ILogger log)
        {
            var item = await this.detailsCache.TryGetAsync($"PlayerProfileFunction.Details.{account}", default, false);
            if(item.Item1)
            {
                return new OkObjectResult(item.Item2);
            }
            else
            {
                var summary = await this.service.GetSummary(account);
                var history = await this.service.GetHistory(account);
                var dto = new Details() 
                { 
                    Summary = summary, 
                    History = history
                };
                var ttl = new Ttl(TimeSpan.FromMinutes(60));
                await this.detailsCache.PutAsync($"PlayerProfileFunction.Details.{account}", dto, ttl, default, false);
                return new OkObjectResult(dto);
            }
        }
    }
}
