using IndigoSessionmanager_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Indigo
{
    public class _getapi
    {
        #region Signature
        public async Task<LogonResponse> Signature(LogonRequest _logonRequestobj)
        {
            ISessionManager Sessionmanager = null;
            LogonResponse logonResponse = null;
            Sessionmanager = new SessionManagerClient();
            try
            {
                logonResponse = await Sessionmanager.LogonAsync(_logonRequestobj);
                return logonResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return logonResponse;
        }
        #endregion
    }
}
