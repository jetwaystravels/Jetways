using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Commit_BookingModel
    {
        public bool notifyContacts { get; set; }
        public string[] contactTypesToNotify { get; set; }
        public string receivedBy { get; set; }
        public bool restrictionOverride { get; set; }
        public string hold { get; set; }
        public string comments { get; set; }


    }
	public class AkasaAirCommitBooking
	{

        public string receivedBy { get; set; }
        public bool restrictionOverride { get; set; }
        public string hold { get; set; }
        public bool notifyContacts { get; set; }
        public string comments { get; set; }
        public string contactTypesToNotify { get; set; }
 

    }

}

