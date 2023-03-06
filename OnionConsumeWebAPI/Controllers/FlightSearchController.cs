using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers
{
    public class FlightSearchController : Controller
    {
        public IActionResult SearchFlight()
        {
            return View();
        }
    }
}
