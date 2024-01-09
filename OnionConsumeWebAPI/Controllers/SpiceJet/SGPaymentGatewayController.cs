using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{
    public class SGPaymentGatewayController : Controller
    {
        public IActionResult SpiceJetPayment()
        {
            return View();
        }
    }
}
