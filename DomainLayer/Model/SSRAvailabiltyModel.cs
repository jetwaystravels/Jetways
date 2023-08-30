using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{

    public class SSRAvailabiltyModel
    {
        public List<Trip> trips { get; set; }
        public string[] passengerKeys { get; set; }
        public string currencyCode { get; set; } = "INR";
    }

    public class Trip
    {
        public List<TripIdentifier> identifier { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public string departureDate { get; set; }
    }

    public class TripIdentifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
    }


    
}
