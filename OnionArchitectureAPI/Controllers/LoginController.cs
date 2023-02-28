using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Ilogin _login;

        public LoginController(Ilogin login)
        {
            this._login = login;
        }

        [HttpGet]
        [Route("getalllogin")]
        public IActionResult GetAllEmployeeRecords()
        {
            var response = this._login.GetAllLoginRepo();
            return Ok(response);
        }
    }
}
