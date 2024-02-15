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
            _logonRequestobj.ContractVersion = 452;
            LogonRequestData LogonRequestDataobj = new LogonRequestData();
            LogonRequestDataobj.AgentName = "OTI122";
            LogonRequestDataobj.DomainCode = "WWW";
            LogonRequestDataobj.Password = "Indigo@2023";
            _logonRequestobj.logonRequestData = LogonRequestDataobj;

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
