using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionConsumeWebAPI.Extensions;

namespace OnionConsumeWebAPI.Controllers
{
    public class AirLinesTicketController : Controller
    {
        public async Task<IActionResult> GetTicketBooking()
        {
            List<TicketBooking> ticket = new List<TicketBooking>();
            HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri("http://localhost:5225/");
            client.BaseAddress = new Uri(AppUrlConstant.BaseURL);
            HttpResponseMessage response = await client.GetAsync("api/TicketBooking/getallBooking");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                ticket = JsonConvert.DeserializeObject<List<TicketBooking>>(results);

            }
            return View(ticket);
        }

    }
}
