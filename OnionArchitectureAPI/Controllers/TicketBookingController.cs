using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TicketBookingController : ControllerBase
	{
		private readonly ITicketBooking _ticket;

		public TicketBookingController(ITicketBooking ticketBooking)
		{
			this._ticket = ticketBooking;
		}		
		[HttpPost]
		[Route("GetTicketBooking")]
		public IActionResult GetTicketBooking(TicketBooking ticketBooking)
		{
			var response = _ticket.AddTicketRepo(ticketBooking);
			return Ok(response);
		}
	}
}
