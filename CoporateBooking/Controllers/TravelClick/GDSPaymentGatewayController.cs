using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers.TravelClick
{
    public class GDSPaymentGatewayController : Controller
    {
        public IActionResult GDSPayment()
        {
            return View();
        }
    }
}
