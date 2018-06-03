using System.Collections.Generic;

namespace TravelSearchContracts
{
    public class Travel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public List<string> Activities { get; set; }

        public int Days { get; set; }

        public TravelImage MainImage { get; set; }

        public List<TravelImage> Images { get; set; }
    }
}
