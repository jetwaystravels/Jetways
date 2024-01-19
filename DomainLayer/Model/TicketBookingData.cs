using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class TicketBookingData
    {
        public int Id { get; set; }
        public string BookingID { get; set; }
        public int AirLineID { get; set; }
        public string RecordLocator { get; set; }  
        public DateTime CurrencyCode { get; set; }   
        public int BookedDate { get; set;}
        public int Origin { get; set; }
        public int Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SpecialServicesTotal { get; set; }
        public decimal SeatTotalAmount_Tax { get; set;}

        public DateTime ExpirationDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string BookingDoc { get; set; }
        public string Status { get; set;}

        public List<Journey> journey { get; set; }


    }

    public class Journey
    {
        public string BookingID { get; set; }
        public string JourneyKey { get; set; }
        public int JourneyKeyCount { get; set; }
        public string FlightType { get; set; }
        public int Stops { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set;}
        public string Status { get;}


    }
}
