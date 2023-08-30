using Microsoft.AspNetCore.Mvc;

namespace OnionArchitectureAPI.Controllers
{
    public class SearchController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
       
    }
}
