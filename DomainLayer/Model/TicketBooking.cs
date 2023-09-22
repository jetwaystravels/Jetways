using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class TicketBooking
    {


        [Key]
        public int tripId { get; set; }
        public string passengerName { get; set; }
        public string Title { get; set; }
        public string phoneNumber { get; set; }
        public DateTime bookingDateTime { get; set; }
        public string emailId { get; set; }
        public string seatNumber { get; set; }
        public string airLines { get; set; }
        public string Class { get; set; }
        public string guestBooking { get; set; }
        public string bookingStatus { get; set; }
        public string bookingReferenceNumber { get; set; }
        public string airlinePNR { get; set; }
        public int identifier { get; set; }
        public string carrierCode { get; set; }
        public string desination { get; set; }
        public string origin { get; set; }
        public DateTime arrival { get; set; }
        public DateTime departure { get; set; }
        public string desinationTerminal { get; set; }
        public string sourceTerminal { get; set; }
        public int price { get; set; }
        public string taxex { get; set; }
        public string response { get; set; }


    }
   



}
