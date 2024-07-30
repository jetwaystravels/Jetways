using DomainLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModel
{
    public class AirLineFlightTicketBooking
    {
        [ForeignKey("tb_booking")]
        public string BookingID { get; set; }
        public tb_Booking tb_Booking { get; set; }
        public ContactDetail ContactDetail { get; set; }
        public GSTDetails GSTDetails { get; set; }
        public tb_AirCraft tb_AirCraft { get; set; }
        public tb_PassengerTotal tb_PassengerTotal { get; set; }
        public List<tb_PassengerDetails> tb_PassengerDetails { get; set; }
        public List<tb_journeys> tb_journeys { get; set; }
        public List<tb_Segments> tb_Segments { get; set; }
    }

}
