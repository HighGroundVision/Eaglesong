using HGV.Eaglesong;
using HGV.Eaglesong.Dto;
using HGV.Eaglesong.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Caching;
using Polly.Caching.Distributed;
using Polly.Caching.Serialization.Json;
using Polly.Extensions.Http;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]

namespace HGV.Eaglesong
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serializerSettings = new JsonSerializerSettings();

            builder.Services.AddStackExchangeRedisCache(options => { 
                options.Configuration = Environment.GetEnvironmentVariable("REDIS_CACHE");
                options.InstanceName = "Eaglesong";
            });

            builder.Services.AddSingleton<Polly.Caching.IAsyncCacheProvider<HttpResponseMessage>>(sp =>
                sp.GetRequiredService<IDistributedCache>()
                  .AsAsyncCacheProvider<string>()
                  .WithSerializer<HttpResponseMessage,string>(
                     new JsonSerializer<HttpResponseMessage>(serializerSettings)
                  )
            );
            builder.Services.AddSingleton<Polly.Caching.IAsyncCacheProvider<Profile>>(sp =>
                sp.GetRequiredService<IDistributedCache>()
                  .AsAsyncCacheProvider<string>()
                  .WithSerializer<Profile,string>(
                     new JsonSerializer<Profile>(serializerSettings)
                  )
            );
            builder.Services.AddSingleton<Polly.Caching.IAsyncCacheProvider<Details>>(sp =>
                sp.GetRequiredService<IDistributedCache>()
                  .AsAsyncCacheProvider<string>()
                  .WithSerializer<Details,string>(
                     new JsonSerializer<Details>(serializerSettings)
                  )
            );

            builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((sp) =>
            {
                var registry = new PolicyRegistry();
                {
                    var provider = sp.GetRequiredService<IAsyncCacheProvider<HttpResponseMessage>>();
                    var policy = Policy.CacheAsync(provider, TimeSpan.FromMinutes(1));                
                    registry.Add("WindrunaPolicy", policy);
                }
                return registry;
            });

            builder.Services.AddHttpClient("Windrun", client =>
            {
                client.BaseAddress = new Uri("https://windrun.io/api/");
            })
            .AddPolicyHandlerFromRegistry("WindrunaPolicy");

            builder.Services.AddSingleton<IPlayerService, PlayerService>();
        }
    }
}
