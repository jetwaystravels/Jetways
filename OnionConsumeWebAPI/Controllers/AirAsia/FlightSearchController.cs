using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionConsumeWebAPI.Extensions;

namespace OnionConsumeWebAPI.Controllers
{
    public class FlightSearchController : Controller
    {
        private readonly ILogger<FlightSearchController> _logger;

        public FlightSearchController(ILogger<FlightSearchController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> SearchFlight()
        {
            List<City> city = new List<City>();
            HttpClient client = new HttpClient();
           
            //client.BaseAddress = new Uri("http://localhost:5225/");
            client.BaseAddress = new Uri(AppUrlConstant.BaseURL);
            HttpResponseMessage response = await client.GetAsync("api/City/getallCity");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                city = JsonConvert.DeserializeObject<List<City>>(results);
                city.Insert(0, new City { CityCode = "Select", CityName = "Select" });
            }
            else
            {
                city.Insert(0, new City { CityCode = "Select", CityName = "Select" });
            }
            ViewBag.ListofCountry= city;    
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSearch(FlightSearchPara flightSearch)
        {
            var s = string.Empty;
            if (flightSearch != null)
            {
                var client = new HttpClient();

            //Start with a list of URLs
            var urls = new string[]
                {
        "http://localhost:24790/api/City/getallCity"
        
                };
                 
            //Start requests for all of them
            var requests = urls.Select
                (
                    url => client.GetAsync(url)
                ).ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requests);

            //Get the responses
            var responses = requests.Select
                (
                    task => task.Result
                );
         
            foreach (var r in responses)
            {
                // Extract the message body
                s = await r.Content.ReadAsStringAsync();
                //Console.WriteLine(s);
            }
            }
            return Content(s);
        }


    }
}

