using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase 
    {
        private readonly ICity _city;      

       public CityController(ICity city) 
        {
            this._city = city;
        }
        // Get All City
        [HttpGet]
        [Route("getallCity")]
        public IActionResult GetAllCity()
        {
            var response = this._city.GetAllCityRepo();
            return Ok(response);
        }
    }
}
