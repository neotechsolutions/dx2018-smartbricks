using System.Collections.Generic;

namespace JasperEngineApp.State
{
    public class UserTravelState
    {
        public bool GoodHealth { get; set; }

        public string Continent { get; set; }

        public List<string> Activities { get; set; }

        public int? CurrentTravelId { get; set; }
    }
}
