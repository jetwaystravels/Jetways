using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_Segments
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("tb_booking")]
        public string BookingID { get; set; }
        public string journeyKey { get; set; }
        public string SegmentKey { get; set; }
        public int SegmentCount { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string Identifier { get; set; }
        public string CarrierCode { get; set; }
        public string Seatnumber { get; set; }
        public string MealCode { get; set; }
        public string MealDiscription { get; set; }
        public int ArrivalTerminal { get; set; }
        public int DepartureTerminal { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
