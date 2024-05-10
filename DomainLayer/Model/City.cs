using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class City
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
  

        public bool IsCity { get; set; }       
        

    }
}
