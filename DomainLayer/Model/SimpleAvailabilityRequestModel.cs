using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class SimpleAvailabilityRequestModel
    {
               
            public string origin { get; set; }
            public string destination { get; set; }
            public string beginDate { get; set; }
            public string endDate { get; set; }
            public Passengerssimple passengers { get; set; }
            public Codessimple codes { get; set; }
            public string sourceOrganization { get; set; }
            public string currentSourceOrganization { get; set; }
            public string promotionCode { get; set; }
            public Filters filters { get; set; }
            public string taxesAndFees { get; set; }
            public string ssrCollectionsMode { get; set; }
            public int numberOfFaresPerJourney { get; set; }
            
        }

        public class Passengerssimple
        {
            public List<Typesimple> types { get; set; }
        }

        public class Typesimple
        {
            public string type { get; set; }
            public int count { get; set; }
        }

        public class Codessimple
        {
            public string currencyCode { get; set; }
        }

        public class Filters
        {
            public string exclusionType { get; set; }
            public string loyalty { get; set; }
            public bool includeAllotments { get; set; }
            public string connectionType { get; set; }
            public string compressionType { get; set; }
            public string []sortOptions { get; set; }
            public int maxConnections { get; set; }
            public string []fareTypes { get; set; }
            public string []productClasses { get; set; }
        }
   

}
