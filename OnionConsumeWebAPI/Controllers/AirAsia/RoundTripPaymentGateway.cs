using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{
    public class RoundTripPaymentGateway : Controller
    {
        public IActionResult RoundTripPaymentView()
        {
            return View();
        }
    }
}
