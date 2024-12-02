namespace OnionConsumeWebAPI.Models
{
    public class MongoResponces
    {
        public MongoDB.Bson.ObjectId _id;
        public string Guid;
        public DateTime CreatedDate;
        public string KeyRef;
        // public List<ResultList> ResList;
        public string Response;
        public string SupplierCode;
    }

    public class MongoRequest
    {
        public MongoDB.Bson.ObjectId _id;
        public string Guid;
        public string Request;
        public DateTime CreatedDate;

    }
}
