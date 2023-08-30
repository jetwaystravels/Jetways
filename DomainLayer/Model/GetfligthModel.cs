using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class GetfligthModel
    {
        [Required]
        public string origin { get; set; }
        [Required]
        public string destination { get; set; } 
        public bool searchDestinationMacs { get; set; } 

        public bool searchOriginMacs { get; set; }
        [Required]
        public string beginDate { get; set; }
        public Passengers passengers { get; set; }

        public string currencyCode { get; set; } = "INR";

    }
    public class Passengers
    {
        public List<_Types> types { get; set; }
    }
    public class _Types
    {
        public string type { get; set; }
        public int count { get; set; }
    }

   
}
