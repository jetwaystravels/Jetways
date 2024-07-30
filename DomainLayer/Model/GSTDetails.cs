using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class GSTDetails
    {
        [Key]
        public int? Id { get; set; }
        public string? bookingReferenceNumber { get; set; }
        public string? airLinePNR { get; set; }
        public string? GSTNumber { get; set; }
        public string? GSTName { get; set; }
        public string? GSTEmail { get; set; }
        public int? status { get; set; }
    }
}
