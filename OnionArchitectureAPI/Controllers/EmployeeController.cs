using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase    
    {
        private readonly IEmployee _employee;

        public EmployeeController(IEmployee employee) 
        {
            this._employee = employee;
        }

        [HttpGet]
        [Route("getallemp")]
        public IActionResult GetAllEmployeeRecords()
        {
            var response = this._employee.GetAllEmployeeRepo();
            return Ok(response);
        }
    }
}
