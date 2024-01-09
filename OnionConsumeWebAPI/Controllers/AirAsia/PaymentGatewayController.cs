using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers
{
    public class PaymentGatewayController : Controller
    {
        public IActionResult Payment()
        {
            return View();
        }
    }
}
