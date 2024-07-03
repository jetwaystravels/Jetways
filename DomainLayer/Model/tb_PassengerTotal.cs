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
        public double TotalMealsAmount { get; set; }
        public double TotalMealsAmount_Tax { get; set; }
        public double TotalSeatAmount { get; set; }
        public double TotalSeatAmount_Tax { get; set; }
        public double TotalBookingAmount { get; set; }
        public double totalBookingAmount_Tax { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
