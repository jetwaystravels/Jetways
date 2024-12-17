using Azure;
using DomainLayer.Model;
using SpicejetSessionManager_;
using Spicejet;
using Newtonsoft.Json;
using Utility;

namespace OnionConsumeWebAPI.Controllers.Spicejet
{
    public class _login
    {
        Logs logs = new Logs();

        public async Task<LogonResponse> Login(string JourneyType,string _Airline = "")
        {
            #region Logon
            LogonRequest _logonRequestobj = new LogonRequest();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5225/");
                HttpResponseMessage responsindigo = await client.GetAsync("api/Login/getotacredairasia");
                if (responsindigo.IsSuccessStatusCode)
                {
                    _logonRequestobj.ContractVersion = 420;
                    LogonRequestData LogonRequestDataobj = new LogonRequestData();
                    var results = responsindigo.Content.ReadAsStringAsync().Result;
                    var JsonObject = JsonConvert.DeserializeObject<List<_credentials>>(results);
                    if (JsonObject[1].FlightCode == 3)
                    {
                        LogonRequestDataobj.AgentName = JsonObject[1].username;
                        LogonRequestDataobj.Password = JsonObject[1].password;
                        LogonRequestDataobj.DomainCode = JsonObject[1].domain;
                        _logonRequestobj.logonRequestData = LogonRequestDataobj;
                    }
                }
            }
            _getapi objSpicejet = new _getapi();
            LogonResponse _logonResponseobj = await objSpicejet.Signature(_logonRequestobj);
            if (_Airline.ToLower() == "spicejetoneway")
            {
                logs.WriteLogs(JsonConvert.SerializeObject(_logonRequestobj), "1-LogonReq", "SpicejetOneWay", JourneyType);
                logs.WriteLogs(JsonConvert.SerializeObject(_logonResponseobj), "1-LogonRes", "SpicejetOneWay", JourneyType);
            }
            else
            {
                logs.WriteLogsR(JsonConvert.SerializeObject(_logonRequestobj), "1-LogonReq", "SpicejetRT");
                logs.WriteLogsR(JsonConvert.SerializeObject(_logonResponseobj), "1-LogonRes", "SpicejetRT");
            }

            return (LogonResponse)_logonResponseobj;
            #endregion

        }



    }
}
