using Microsoft.AspNetCore.Mvc;

namespace OnionArchitectureAPI.Controllers
{
    public class BookingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
       
        
    }
}
