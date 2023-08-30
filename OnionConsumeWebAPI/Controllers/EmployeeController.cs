using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OnionConsumeWebAPI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> EmployeeIndex()
        {
            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5225/");
            HttpResponseMessage response = await client.GetAsync("api/Employee/getallemp");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(results);

            }
            return View(employees);
        }
    }
}
