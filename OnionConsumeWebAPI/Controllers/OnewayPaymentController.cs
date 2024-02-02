using Microsoft.AspNetCore.Mvc;

namespace OnionConsumeWebAPI.Controllers
{
	public class OnewayPaymentController : Controller
	{
		public IActionResult OnewayPaymentView()
		{
			return View();
		}
	}
}
