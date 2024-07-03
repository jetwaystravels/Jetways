using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_Booking
    {
        [Key]
        public int Id { get; set; }
        public string BookingID { get; set; }

        public string ? FlightID { get; set; }
        public int AirLineID { get; set; }
        public string RecordLocator { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BookedDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        //public DateTime DepartureDate { get; set; }
        //public DateTime ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public double TotalAmount { get; set; }
        public double SpecialServicesTotal { get; set; }
        public double SpecialServicesTotal_Tax { get; set; }
        public double SeatTotalAmount { get; set; }
        public double SeatTotalAmount_Tax { get; set; }      
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }       
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }        
        public string ModifyBy { get; set; }
        public string BookingDoc { get; set; }
        public string Status { get; set; }
    }
}
