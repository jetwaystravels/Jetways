using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{
    public class IndigoPaymentGatewayController : Controller
    {
        public IActionResult IndigoPayment()
        {
            return View();
        }
    }
}
