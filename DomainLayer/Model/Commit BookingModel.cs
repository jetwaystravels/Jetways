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
    }

}

