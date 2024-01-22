using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class BookingDetailsModel
    {
        public string BookingID { get; set; }
        public int AirLineID { get; set; }
        public string RecordLocator { get; set; }
        public string CurrencyCode { get; set; }
        public string BookedDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SpecialServicesTotal { get; set; }
        public decimal SpecialServicesTotal_Tax { get; set; }
        public decimal SeatTotalAmount { get; set; }
        public decimal SeatTotalAmount_Tax { get; set; }
        public string ExpirationDate { get; set; }
        public string CreatedDate { get; set; }
        public string Createdby { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifyBy { get; set; }
        public string BookingDoc { get; set; }
        public string Status { get; set; }
    }
    public class JourneyDetailsModel
    {
        public int id { get; set; }
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
        public DateTime ModifiedDate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }   


    }
    public class PassengerDetails
    {
        public int Id { get; set; }
        public string BookingID { get; set; }
        public string PassengerKey { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TypeCode { get; set; }
        public DateTime Dob { get; set; }
        public string Seatnumber { get; set; }
        public int Handbages { get; set; }
        public int Carrybages { get; set; }
        public string contact_Emailid { get; set; }
        public int contact_Mobileno { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmount_tax { get; set; }
        public decimal TotalAmount_Meals { get; set; }
        public decimal TotalAmount_Meals_tax { get; set; }
        public decimal TotalAmount_Seat { get; set; }
        public decimal TotalAmount_Seat_tax { get; set; }
        public string Inf_Firstname { get; set; }
        public string Inf_Middlename { get; set; }
        public string Inf_Lastname { get; set; }
        public string Inf_TypeCode { get; set; }
        public string Inf_Gender { get; set; }
        public DateTime Inf_Dob { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifyBy { get; set; }
        public string Status { get; set; }
    }

    public class PassengerTotalModel
    {
        public int Id { get; set; }
        public string BookingID { get; set; }
        public decimal TotalMealsAmount { get; set; }
        public decimal TotalMealsAmount_Tax { get; set; }
        public decimal TotalSeatAmount { get; set; }
        public decimal TotalSeatAmount_Tax { get; set; }
        public decimal TotalBookingAmount { get; set; }
        public decimal totalBookingAmount_Tax { get; set; }
        public string CreatedDate { get; set; }
        public string Createdby { get; set; }
        public string ModifiedDate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
