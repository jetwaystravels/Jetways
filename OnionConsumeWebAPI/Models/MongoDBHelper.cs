using DomainLayer.Model;
using MongoDB.Driver;
using OnionConsumeWebAPI.ApiService;
using System.Xml.Serialization;

namespace OnionConsumeWebAPI.Models
{
    public class MongoDBHelper
    {
        private readonly MongoDbService _mongoDbService;

        public MongoDBHelper(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<string> GetFlightSearchByKeyRef(string keyref)
        {
            string guid = "";
            try
            {
                MongoResponces srchData = new MongoResponces();

                //  _mongoDbService = new MongoDbService();

                srchData = await _mongoDbService.GetCollection<MongoResponces>("KeyLog").Find(Builders<MongoResponces>.Filter.Eq("KeyRef", keyref)).Sort(Builders<MongoResponces>.Sort.Descending("CreatedDate")).FirstOrDefaultAsync().ConfigureAwait(false);

                if (srchData != null)
                {
                    if (srchData.CreatedDate < DateTime.UtcNow)
                    {
                        srchData.Guid = null;
                    }
                    else
                    {
                        guid = srchData.Guid;
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return guid;
        }

        public async Task<MongoResponces> GetALLFlightResulByGUID(string guid)
        {
            MongoResponces srchDataALL = new MongoResponces();
            try
            {
                await _mongoDbService.GetCollection<MongoResponces>("Result").Find(Builders<MongoResponces>.Filter.Eq("Guid", guid)).Sort(Builders<MongoResponces>.Sort.Descending("CreatedDate")).FirstOrDefaultAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
            return srchDataALL;
        }

        public void SaveKeyRequest(string guid, string keyref)
        {
            try
            {
                MongoResponces srchData = new MongoResponces();
                srchData.CreatedDate = DateTime.UtcNow.AddMinutes(Convert.ToInt16(20));
                srchData.KeyRef = keyref;
                srchData.Guid = guid;

                _mongoDbService.GetCollection<MongoResponces>("KeyLog").InsertOneAsync(srchData);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveFlightSearch(MongoResponces srchData, string prefix, string loc)
        {
            try
            {
                srchData.CreatedDate = DateTime.UtcNow.AddMinutes(Convert.ToInt16(20));
                _mongoDbService.GetCollection<MongoResponces>("Result").InsertOneAsync(srchData);
            }
            catch (Exception ex)
            {
            }
        }

        public void SaveRequest(SimpleAvailabilityRequestModel sCriteria, string Guid)
        {
            MongoRequest srchData = new MongoRequest();
            MongoHelper mongoHelper = new MongoHelper();
            try
            {
                 srchData.Guid = Guid;
                srchData.CreatedDate = DateTime.UtcNow.AddMinutes(Convert.ToInt16(20));

                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SimpleAvailabilityRequestModel));
                    serializer.Serialize(stringWriter, sCriteria);
                    srchData.Request = mongoHelper.Zip(stringWriter.ToString());
                }

                _mongoDbService.GetCollection<MongoRequest>("Requests").InsertOneAsync(srchData);

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<SimpleAvailabilityRequestModel> GetRequests(string guid)
        {
            SimpleAvailabilityRequestModel sCriteria = new SimpleAvailabilityRequestModel();
            MongoRequest srchData = new MongoRequest();
            MongoHelper mongoHelper = new MongoHelper();
            //service1 src = new service1();
            try
            {
                await _mongoDbService.GetCollection<MongoRequest>("Requests").Find(Builders<MongoRequest>.Filter.Eq("Guid", guid)).FirstOrDefaultAsync().ConfigureAwait(false);

                if (srchData != null && srchData.Request != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SimpleAvailabilityRequestModel));

                    StringReader textReader = new StringReader(mongoHelper.UnZip(srchData.Request));

                    sCriteria = (SimpleAvailabilityRequestModel)serializer.Deserialize(textReader);

                    // sCriteria.LogDateTime = srchData.CreatedDate;
                }
            }
            catch (Exception ex)
            {
                // Logger.WriteLog(ex, prefix, "");
            }

            return sCriteria;
        }


        public void SaveSearchLog(SimpleAvailabilityRequestModel requestModel, string Guid)
        {
            MongoHelper mongoHelper = new MongoHelper();

            SearchLog searchLog = new SearchLog();
            searchLog.TripType = requestModel.trip;
            searchLog.Log_WSGUID = Guid;
            searchLog.Log_SearchTypeID = 1;
            searchLog.Origin = requestModel.origin.Split("-")[1];
            searchLog.Destination = requestModel.destination.Split("-")[1];
            searchLog.Log_RefNumber = mongoHelper.Get8Digits();
            searchLog.DepartDateTime = requestModel.beginDate;
            searchLog.ArrivalDateTime = requestModel.endDate;

            if (requestModel.passengercount != null)
            {
                searchLog.Adults = requestModel.passengercount.adultcount;
                searchLog.Children = requestModel.passengercount.childcount;
                searchLog.Infants = requestModel.passengercount.infantcount;
            }
            else
            {
                searchLog.Adults = requestModel.adultcount;
                searchLog.Children = requestModel.childcount;
                searchLog.Infants = requestModel.infantcount;
            }
            searchLog.Log_DateTime = DateTime.Now;
            searchLog.IP = mongoHelper.GetIp();

            _mongoDbService.GetCollection<SearchLog>("LogSearchData").InsertOneAsync(searchLog);
        }

        public async Task<SearchLog> GetFlightSearchLog(string Guid)
        {
            SearchLog srchData = null;
            try
            {
                srchData = new SearchLog();

                //  _mongoDbService = new MongoDbService();

                srchData = await _mongoDbService.GetCollection<SearchLog>("LogSearchData").Find(Builders<SearchLog>.Filter.Eq("Log_WSGUID", Guid)).Sort(Builders<SearchLog>.Sort.Descending("Log_DateTime")).FirstOrDefaultAsync().ConfigureAwait(false);

               
            }
            catch (Exception ex)
            {


            }

            return srchData;

        }
    }
}
