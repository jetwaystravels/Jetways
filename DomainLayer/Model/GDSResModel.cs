using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class GDSResModel
    {

        public class Segment
        {
            public List<Bond> Bonds { get; set; }
            public Fare Fare { get; set; }
            public string SegIndex { get; set; }
            public int JourneyIndex { get; set; }
            public string NearByAirport { get; set; }
            public string FareIndicator { get; set; }

            public string FareRule { get; set; }
            public string EngineID { get; set; }
            public string ItineraryKey { get; set; }
            public string SearchId { get; set; }
            public string IsSpecial { get; set; }
            public string JourneyType { get; set; }
            public bool IsRoundTrip { get; set; }
            public string BondType { get; set; }
            public string Segmentid { get; set; }
            public string _flightnumber { get; set; }
            public string PricingSolutionValue { get; set; }


        }
        public class Fare
        {
            public decimal TotalFareWithOutMarkUp { get; set; }
            public decimal BasicFare { get; set; }

            public decimal TotalTaxWithOutMarkUp { get; set; }
            public List<PaxFare> PaxFares { get; set; }
        }

        public class PaxFare
        {
            public List<FareDetail> Fare { get; set; }
            public bool Refundable { get; set; }
            public string FareBasisCode { get; set; }
            public string BaggageWeight { get; set; }
            public string BaggageUnit { get; set; }
            public string FareInfoKey { get; set; }
            public string FareInfoValue { get; set; }
            public decimal BasicFare { get; set; }
            public decimal ChangePenalty { get; set; }
            public decimal CancelPenalty { get; set; }
            public decimal TotalTax { get; set; }
            public decimal TotalFare { get; set; }
            public PAXTYPE PaxType { get; set; }
            public string FareID { get; set; }

        }
        public enum PAXTYPE
        {
            ADT,
            CHD,
            INF

        }
        public class Leg
        {
            public string BoundType { get; set; }
            public string Group { get; set; }
            public string NumberOfStops { get; set; }
            public string FlightNumber { get; set; }
            public string AirlineName { get; set; }
            public string CarrierCode { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string DepartureDate { get; set; }
            public string DepartureTime { get; set; }
            public string ArrivalDate { get; set; }
            public string ArrivalTime { get; set; }
            public string Duration { get; set; }
            public string AircraftCode { get; set; }
            public string FlightName { get; set; }
            public string ProviderCode { get; set; }
            public string FlightDetailRefKey { get; set; }
            public string DepartureTerminal { get; set; }
            public string ArrivalTerminal { get; set; }
            public string FareClassOfService { get; set; }
            public string AvailableSeat { get; set; }
            public string Cabin { get; set; }
            public string FareRulesKey { get; set; }
            public string FareRulesValue { get; set; }
            public string BaggageWeight { get; set; }
            public string BaggageUnit { get; set; }
            public string Remarks { get; set; }
            public string Branddesc { get; set; }
            public string _DepartureDate { get; set; }
            public string _ArrivalDate { get; set; }
            public string _Distance { get; set; }
            public string _Equipment { get; set; }
            public string _AvailabilitySource { get; set; }
            public string _AvailabilityDisplayType { get; set; }
            public string _Classofservice { get; set; }
            


        }
        public class Bond
        {
            public string BoundType { get; set; }
            public string JourneyTime { get; set; }
            public string FlightNumber { get; set; }
            public List<Leg> Legs { get; set; }


        }

        public class TPAirSegment
        {
            public string AirSegment { get; set; }
            public string AirSegmentDetail { get; set; }
        }

        public class FareDetail
        {
            public decimal Amount { get; set; }
            public string ChargeCode { get; set; }
            public string ChargeDetail { get; set; }
            public string ChargeType { get; set; }
            public string ClassOfService { get; set; }
            public string CurrencyCode { get; set; }
        }

        public class PnrResponseDetails
        {
            public string AirReservationLocatorCode { get; set; }
            public ArrayList PaxeDetailList { get; set; }
            public ArrayList PaxFareList { get; set; }
            public string PnrMessage { get; set; }
            public bool PnrStatus { get; set; }
            public string ProviderReservationLocator { get; set; }
            public string SupplierLocatorCode { get; set; }
            public string TPTransactionId { get; set; }
            public string UniversalRecordLocator { get; set; }
            public string PlatingCarrier { get; set; }
            public string DealCodeVersion { get; set; }
            public bool IsAirPricingInfo { get; set; }
            public bool IsCreditPayment { get; set; }
            public string PaymentKey { get; set; }
            public Bond Bonds { get; set; }
            public DateTime bookingdate { get; set; }
        }
        public class TravellerDetail
        {
            public string PaxId { get; set; }
            public Enum PaxType { get; set; }
            public string FirstName { get; set; }

            public string LastName { get; set; }

        }
    }
}
