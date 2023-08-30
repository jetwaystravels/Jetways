using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;

namespace DomainLayer.ViewModel
{
    public class PassengerDetails
    {
        public IEnumerable<AirAsiaTripResponceModel> AirAsiaTripResponceModel { get; set; }
        public IEnumerable<passkeytype> passkeytype { get; set; }

    }
}
