using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class FlightSearchPara
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }  
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }  
        public string Trip { get; set; }
        public int NumberOfTraveller { get; set; } = 1;

    }
}
