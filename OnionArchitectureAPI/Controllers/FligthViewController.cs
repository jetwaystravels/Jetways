using Microsoft.AspNetCore.Mvc;

namespace OnionArchitectureAPI.Controllers
{
    public class FligthViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
