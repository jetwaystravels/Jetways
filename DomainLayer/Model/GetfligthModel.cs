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
        //public string type { get; set; }
        public string Adult { get; set; }
        public string children { get; set; }
        public string infant { get; set; }
        public string count { get; set; }

        //public List<_Types> _Types { get; set; }


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
    public class PassengersReturn
    {
        public List<_TypesReturn> types { get; set; }
    }
    public class _TypesReturn
    {
        public string type { get; set; }
        public int count { get; set; }
    }




}
