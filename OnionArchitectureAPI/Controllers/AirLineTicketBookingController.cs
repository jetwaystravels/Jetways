using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirLineTicketBookingController : ControllerBase
    {
        private readonly Itb_Booking _ticketbook;
        public AirLineTicketBookingController(Itb_Booking ticketBooking)
        {
            this._ticketbook = ticketBooking;
        }

        [HttpPost]
        [Route("PostairlineTicketData")]
        public IActionResult PostairlineTicketData(AirLineFlightTicketBooking ticketObject)
        {
            if (ticketObject.tb_Booking != null)
            {
                var response = _ticketbook.PostTicketDataRepo(ticketObject);

                return Ok(response);
            }
            return Ok();
        }
    }
}
