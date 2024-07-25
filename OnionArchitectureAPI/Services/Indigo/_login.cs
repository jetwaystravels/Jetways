using Azure;
using DomainLayer.Model;
using Indigo;
using IndigoSessionmanager_;
using Newtonsoft.Json;
using Utility;

namespace OnionConsumeWebAPI.Controllers.Indigo
{
    public class _login
    {
        Logs logs=new Logs ();
       
        public async Task<LogonResponse> Login(string _Airline="")
        {
            #region Logon
            LogonRequest _logonRequestobj = new LogonRequest();
           
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5225/");
                HttpResponseMessage responsindigo = await client.GetAsync("api/Login/getotacredairasia");
                if (responsindigo.IsSuccessStatusCode)
                {
                   
                    _logonRequestobj.ContractVersion = 452;
                    LogonRequestData LogonRequestDataobj = new LogonRequestData();
                    var results = responsindigo.Content.ReadAsStringAsync().Result;
                    var JsonObject = JsonConvert.DeserializeObject<List<_credentials>>(results);
                    if (JsonObject[2].FlightCode == 4)
                    {
                        LogonRequestDataobj.AgentName = JsonObject[2].username;
                        LogonRequestDataobj.Password = JsonObject[2].password;
                        LogonRequestDataobj.DomainCode = JsonObject[2].domain;
                        _logonRequestobj.logonRequestData = LogonRequestDataobj;

                    }
                }
            }
                      
           
           

            _getapi objIndigo = new _getapi();
            LogonResponse _logonResponseobj = await objIndigo.Signature(_logonRequestobj);
            if (_Airline.ToLower() == "indigooneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logonRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logonResponseobj), "Logon", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_logonRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logonResponseobj), "Logon", "IndigoRT");
            }

            return (LogonResponse)_logonResponseobj;
            #endregion

        }

        

    }
}
