using DomainLayer.Model;
using Indigo;
using IndigoSessionmanager_;

namespace OnionConsumeWebAPI.Controllers.Indigo
{
    public class _login
    {
        public async Task<LogonResponse> Login(SimpleAvailabilityRequestModel _GetfligthModel)
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

            //logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logonRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logonResponseobj), "Logon");

            return (LogonResponse)_logonResponseobj;
            #endregion

        }
    }
}
