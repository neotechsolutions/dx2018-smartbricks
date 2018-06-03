using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelSearchContracts;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace TravelSearchBackend
{
    public class DataStore
    {
        private static readonly List<string> Activities = new List<string>
        {
            "Plage",
            "Piscine",
            "Tennis",
            "Ping-pong",
            "Paddle",
            "Beachvolley",
            "Aerobic",
            "Aquagym",
            "Voile",
            "Pêche",
            "Plongée",
            "Windsurf",
            "Pédalos",
            "Spa",
            "Salle de sport",
            "Golf",
            "Equitation",
            "Ski nautique",
            "Canoé et kayak",
            "Kite surf",
            "Catamaran",
            "Billard",
            "Bowling",
            "Planche à voile",
        };

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly Random _rand = new Random();

        public DataStore(IHostingEnvironment hostingEnvironment, IMemoryCache cache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
        }

        public Task<List<Travel>> GetAllTravelsAsync()
        {
            return _cache.GetOrCreateAsync("TRAVELS", async _ =>
            {
                string data = await File.ReadAllTextAsync($"{_hostingEnvironment.ContentRootPath}/data.json");
                JObject json = JObject.Parse(data);

                dynamic travels = json.GetValue("results") as JArray;

                var results = new List<Travel>();

                foreach (dynamic travel in travels)
                {
                    results.Add(ParseTravel(travel));
                }

                return results;
            });
        }

        public async Task<Travel> GetTravelAsync(int id)
        {
            var travels = await GetAllTravelsAsync();
            return travels.SingleOrDefault(x => x.Id == id);
        }

        public async Task<List<Travel>> SearchTravelsAsync(SearchQuery query)
        {
            IEnumerable<Travel> travels = await GetAllTravelsAsync();

            if (!string.IsNullOrEmpty(query.Continent))
            {
                travels = travels.Where(x => x.Continent == query.Continent);
            }

            if (query.Activities != null && query.Activities.Count > 0)
            {
                travels = travels.Where(x => x.Activities.Any(a => query.Activities.Contains(a)));
            }

            if (query.IgnoredResults != null && query.IgnoredResults.Count > 0)
            {
                travels = travels.Where(x => !query.IgnoredResults.Contains(x.Id));
            }

            if (query.MaxResults.HasValue)
            {
                travels = travels.Take(query.MaxResults.Value);
            }

            return travels.ToList();
        }

        private Travel ParseTravel(dynamic item)
        {
            return new Travel
            {
                Id = item.id,
                Title = item.title,
                Desc = item.sb_location?.text ?? $"{item.nom_pays} / {item.nom_ville}",
                Address = item.address,
                Continent = ParseContinent(item.destinations[0]),
                Country = item.nom_pays,
                Days = item.browse_data.best_sale_price_transport["1"].with_transport?.nights ?? item.browse_data.best_sale_price_transport["1"].without_transport.nights ?? _rand.Next(1, 14),
                City = item.nom_ville,
                MainImage = item.main_image == null || !item.main_image.ToString().EndsWith(".jpg") ? ParseImage(item.images[0]) : new TravelImage { Url = $"https://images1.bovpg.net/v/back/fr/{item.main_image}" },
                Images = ParseImages(item.images),
                Activities = GetActivities()
            };
        }

        private string ParseContinent(dynamic item)
        {
            var prefix = item.ToString().Substring(0, 2);
            switch (prefix)
            {
                case "eu":
                    return "Europe";

                case "as":
                    return "Asie";

                case "af":
                    return "Afrique";

                case "na":
                    return "Amérique du nord";

                case "sa":
                    return "Amérique du sud";

                case "oc":
                    return "Océanie";

                default:
                    return "Non communiqué";
            }

        }

        private static List<TravelImage> ParseImages(dynamic items)
        {
            var results = new List<TravelImage>();

            foreach (dynamic item in items)
            {
                results.Add(ParseImage(item));
            }

            return results;
        }

        private static TravelImage ParseImage(dynamic item)
        {
            return new TravelImage
            {
                Url = $"https://images1.bovpg.net/v/back/fr/{item.path}",
                Legend = item.legend
            };
        }

        private List<string> GetActivities()
        {
            var count = _rand.Next(10, 20);

            return Enumerable.Range(0, count).Select(_ =>
            {
                var index = _rand.Next(0, Activities.Count);
                return Activities[index];
            }).Distinct().ToList();
        }
    }
}
