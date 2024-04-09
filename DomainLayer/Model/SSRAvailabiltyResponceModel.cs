using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Model.ReturnTicketBooking;

namespace DomainLayer.Model
{
    public class SSRAvailabiltyResponceModel
    {
        public List<JourneyssrBaggage> journeySsrsBaggage { get; set; }
        public List<segmentSsrs> segmentSsrs { get; set; }
        public List<legSsrs> legSsrs { get; set; }
        public int SegmentSSrcount { get; set; }
    }
    //BagggageModel
    public class JourneyssrBaggage
    {
        public string journeyBaggageKey { get; set; }
        public JourneyDetailsBaggage journeydetailsBaggage { get; set; }
        public List<BaggageSsr> baggageSsr { get; set; }
    }

    public class JourneyDetailsBaggage
    {
        public JBaggageIdentifier identifier { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public DateTime departureDate { get; set; }
    }

    public class JBaggageIdentifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public object opSuffix { get; set; }
    }

    public class BaggageSsr
    {
        public string ssrCode { get; set; }
        public List<PassengersAvailabilityBaggage> passengersAvailabilityBaggage { get; set; }
        public int ssrType { get; set; }
        public string name { get; set; }
        public int limitPerPassenger { get; set; }
        public int? available { get; set; }
        public bool inventoryControlled { get; set; }
        public bool seatDependent { get; set; }
        public string feeCode { get; set; }
        public string nest { get; set; }
        public int seatRestriction { get; set; }
    }
    public class PassengersAvailabilityBaggage
    {
        public Airlines Airline { get; set; }
        public string ssrKey { get; set; }
        public string passengerKey { get; set; }
        public string price { get; set; }
    }

    public class segmentSsrs
    {
        public string segmentKey { get; set; }
        public segmentDetails segmentDetails { get; set; }

        public segSsrs segmentssrs { get; set; }
    }
    public class segmentDetails
    {
        public segidentifier segmentidentifier { get; set; }
        public string destination { get; set; }

        public string origin { get; set; }
        public string departureDate { get; set; }



    }
    public class segidentifier
    {

        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public object opSuffix { get; set; }

    }
    public class segSsrs
    {
        public string ssrCode { get; set; }
        public segpassengers[] segpassengers { get; set; }
        public int ssrType { get; set; }
        public string name { get; set; }
        public int limitPerPassenger { get; set; }
        public int? available { get; set; }
        public bool inventoryControlled { get; set; }
        public bool seatDependent { get; set; }
        public string feeCode { get; set; }
        public string nest { get; set; }
        public int seatRestriction { get; set; }


    }
    public class segpassengers
    {
        public string ssrKey { get; set; }
        public string passengerKey { get; set; }
        public string price { get; set; }
    }
    public class legSsrs
    {
        public string legKey { get; set; }

        public legDetails legDetails { get; set; }

        public List<childlegssrs> legssrs { get; set; }

    }
    public class legDetails
    {
        public legidentifier legidentifier { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public string departureDate { get; set; }

    }
    public class legidentifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public object opSuffix { get; set; }

    }
    public class childlegssrs
    {
        public string ssrCode { get; set; }
        public List<legpassengers> legpassengers { get; set; }
        public int ssrType { get; set; }
        public string name { get; set; }
        public int limitPerPassenger { get; set; }
        public int? available { get; set; }
        public bool inventoryControlled { get; set; }
        public bool seatDependent { get; set; }
        public string feeCode { get; set; }
        public string nest { get; set; }
        public int seatRestriction { get; set; }

    }
    public class legpassengers
    {
        public Airlines Airline { get; set; }
        public string ssrKey { get; set; }
        public string passengerKey { get; set; }
        public string price { get; set; }

        public string ssrKeydesc { get; set; }
    }

}






















