using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Ilogin _login;
        private readonly ICredential _Credential;

        public LoginController(Ilogin login,ICredential credential)
        {
            this._login = login;
            this._Credential= credential;
        }

        [HttpGet]
        [Route("getalllogin")]
        public IActionResult GetAllEmployeeRecords()
        {
            var response = this._login.GetAllLoginRepo();
            return Ok(response);
        }

        [HttpGet]
        [Route("getotacredairasia")]
        public IActionResult Getcredential()
        {
            var response = this._Credential.GetAllCredentialRepo();
            return Ok(response);
        }
    }
}
