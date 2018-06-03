using System.Collections.Generic;

namespace TravelSearchContracts
{
    public class SearchQuery
    {
        public string Continent { get; set; }

        public List<string> Activities { get; set; }

        public List<int> IgnoredResults { get; set; }

        public int? MaxResults { get; set; }
    }
}
