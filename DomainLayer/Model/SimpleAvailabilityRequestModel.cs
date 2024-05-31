using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class SimpleAvailabilityRequestModel
    {
        public string adulttype { get; set; }
        public int adultcount { get; set; }
        public string childtype { get; set; }
        public int childcount { get; set; }
        public string infanttype { get; set; }
        public int infantcount { get; set; }
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
        public passengercount passengercount { get; set; }

        public bool searchDestinationMacs { get; set; }
        public bool searchOriginMacs { get; set; }
        public bool getAllDetails { get; set; }

    }
    public class passengercount
    {
        public string adulttype { get; set; }
        public int adultcount { get; set; }
        public string childtype { get; set; }
        public int childcount { get; set; }
        public string infanttype { get; set; }
        public int infantcount { get; set; }
    }

    public class Passengerssimple
    {
        [NotMapped]
        public List<Typesimple>? types { get; set; }
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
        public string[] sortOptions { get; set; }
        public int maxConnections { get; set; }
        public string[] fareTypes { get; set; }
        public string[] productClasses { get; set; }

        // Akasa Property
        public bool groupByDate { get; set; }
        public string carrierCode { get; set; }
        public string type { get; set; }



    }


}
