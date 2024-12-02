using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnionConsumeWebAPI.Models
{
    public class SearchLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int TripType { get; set; } //0-OneWay, 1-RoundTrip, 2-SameAirlineRT
        //public string TripName { get; set; }
        //public string ApiName { get; set; }
        //public string SupplierName { get; set; }
        //public string Origin_Departure { get; set; }
        //public string Key { get; set; }
        //public string Request { get; set; }
        //public string Response { get; set; }
        //public DateTime InsertedOn { get; set; } = DateTime.Now;

        public int Log_WSServer { get; set; }
        public string Log_WSGUID { get; set; }
        public int Log_SearchTypeID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Log_RefNumber { get; set; }
        public string DepartDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
        public int Teenagers { get; set; }
        public string CheapestPrice { get; set; }
        public double CheapestData { get; set; }

        public string SelectedID { get; set; }
        public DateTime Log_DateTime { get; set; }

        public string Device { get; set; }

        //   public String Meta { get; set; }

        //   public String currencycode { get; set; }
        public string FlightClass { get; set; }
        public string IP { get; set; }

    }
}
