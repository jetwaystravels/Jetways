using System.Diagnostics;
using System.Text.Json.Serialization;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.Models;

namespace OnionConsumeWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
           List<User> users = new List<User>();
            HttpClient client = new HttpClient();
            
            //  client.BaseAddress = new Uri("http://localhost:5225/");
              client.BaseAddress = new Uri(AppUrlConstant.BaseURL);
            HttpResponseMessage response = await client.GetAsync("api/User/getall");
            if(response.IsSuccessStatusCode) 
            {
                var results=response.Content.ReadAsStringAsync().Result;
                users=JsonConvert.DeserializeObject<List<User>>(results);

            }
            return View(users);
        }

       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}