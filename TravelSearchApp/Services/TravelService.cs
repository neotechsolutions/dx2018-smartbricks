using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TravelSearchContracts;

namespace TravelSearchApp.Services
{
    public static class TravelService
    {
        private static List<Travel> _loadedTravels;

        public static async Task<List<Travel>> GetAllTravelsAsync()
        {
            if (_loadedTravels == null)
            {
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync("[<Backend URL>]");
                    _loadedTravels = JsonConvert.DeserializeObject<List<Travel>>(content);
                    IsTravelLoaded = true;
                }
            }

            return _loadedTravels;
        }

        public static bool IsTravelLoaded { get; private set; }
    }
}
