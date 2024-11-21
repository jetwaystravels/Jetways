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
        public string TripName { get; set; }
        public string ApiName { get; set; }
        public string SupplierName { get; set; }
        public string Origin_Departure { get; set; }
        public string Key { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime InsertedOn { get; set; } = DateTime.Now;


    }
}
