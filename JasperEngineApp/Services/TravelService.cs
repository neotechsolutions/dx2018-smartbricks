using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TravelSearchContracts;

namespace TravelSearchApp.Services
{
    public static class TravelService
    {
        public async static Task<List<Travel>> SearchTravelsAsync(string continent, List<string> activities, int maxResults = 1)
        {
            using (var client = new HttpClient())
            {
                var input = new SearchQuery { Continent = continent, Activities = activities, MaxResults = maxResults };
                var response = await client.PostAsJsonAsync("[<Backend URL>]", input).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Travel>>(content);
            }
        }
    }
}
