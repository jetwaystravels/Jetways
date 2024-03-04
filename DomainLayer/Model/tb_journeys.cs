using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_journeys
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("tb_booking")]
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
}
