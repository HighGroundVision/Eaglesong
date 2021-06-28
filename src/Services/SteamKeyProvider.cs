using HGV.Daedalus;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGV.Eaglesong
{
    public class SteamKeyProvider : ISteamKeyProvider
    {
        public string GetKey()
        {
            return Environment.GetEnvironmentVariable("STEAM_KEY");
        }
    }
}
