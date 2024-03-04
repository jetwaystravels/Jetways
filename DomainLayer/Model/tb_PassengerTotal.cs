using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_PassengerTotal
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("tb_booking")]
        public string BookingID { get; set; }
        public decimal TotalMealsAmount { get; set; }
        public decimal TotalMealsAmount_Tax { get; set; }
        public decimal TotalSeatAmount { get; set; }
        public decimal TotalSeatAmount_Tax { get; set; }
        public decimal TotalBookingAmount { get; set; }
        public decimal totalBookingAmount_Tax { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
