using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_PassengerDetails
    {
        [Key]
        public int Id { get; set; }
        //[ForeignKey("tb_booking")]
        public string BookingID { get; set; }
        public string SegmentsKey { get; set; }
        public string PassengerKey { get; set; }
        //public string Gender { get; set; } 
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string TypeCode { get; set; }
        public DateTime? Dob { get; set; }
        public string? Seatnumber { get; set; }
        public int? Handbages { get; set; }
        public string? Carrybages { get; set; }
        public string? MealsCode { get; set; }
        public string? contact_Emailid { get; set; }
        public int? contact_Mobileno { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAmount_tax { get; set; }
        public double TotalAmount_Meals { get; set; }
        public double? TotalAmount_Meals_tax { get; set; }
        public double BaggageTotalAmount { get; set; }
        public double TotalAmount_Seat { get; set; }
        public decimal? TotalAmount_Seat_tax { get; set; }
        public string? Inf_Firstname { get; set; }
        public string? Inf_Middlename { get; set; }
        public string? Inf_Lastname { get; set; }
        public string? Inf_TypeCode { get; set; }
        public string? Inf_Gender { get; set; }
        public DateTime? Inf_Dob { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifyBy { get; set; }
        public string Status { get; set; }


    }
    public class FeeDetails
    {
        public string Seatnumber { get; set; }
        public string Carrybages { get; set; }
        public string ssrCode { get; set; }
    }
}
