using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class SimpleAvailibilityaAddResponce
    {
        public int uniqueId { get; set; }
        public Airlines Airline { get; set; }
        public int flightType { get; set; }
        public int stops { get; set; }
        public Designator designator { get; set; }
        public string journeyKey { get; set; }
        public List<Segment> segments { get; set; }
        public List<Fare> fares { get; set; }
        public bool notForGeneralUser { get; set; }

        public List<FareIndividual> faresIndividual { get; set; }

        public decimal fareTotalsum { get; set; }

        public Passengerssimple passengers { get; set; }
        public string carriercode { get; set; }

        public string Identifier { get; set; }

        public int flightcount { get; set; }

        public string arrivalTerminal { get; set; }
        public string departureTerminal { get; set; }

        public string bookingdate { get; set; }


    }

    public enum Airlines
    {
        Airasia = 0,
        Spicejet = 1,
        Indigo=2
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Designator
    {
        public string destination { get; set; }
        public string origin { get; set; }
        public TimeSpan formatTime { get; set; }

        public DateTime arrival { get; set; }
        public DateTime departure { get; set; }
    }
    
    public class Detail
    {
        public int availableCount { get; set; }
        public int status { get; set; }
        public string reference { get; set; }
        public string serviceBundleSetCode { get; set; }
        public string bundleReferences { get; set; }
        public string ssrReferences { get; set; }
    }

    public class FareIndividual
    {
        public decimal faretotal { get; set; } 
        public string procuctclass { get; set; }
        public string fareKey { get; set; }
        public string passengertype { get; set; }
        public decimal discountamount { get; set; }
        public decimal taxamount { get; set; }

       

    }

    public class Total
    {

        public int? faretotalfinal { get; set; }
        


    }

    public class Fare
    {
        public string fareAvailabilityKey { get; set; }
        public List<Detail> details { get; set; }
        public bool isSumOfSector { get; set; }
        
    }

    public class Identifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public string opSuffix { get; set; }
    }

    public class Leg
    {
        public string legKey { get; set; }
        public object operationsInfo { get; set; }
        public Designator designator { get; set; }
        public LegInfo legInfo { get; set; }
        public List<string> nests { get; set; }
        public List<Ssr> ssrs { get; set; }
        public string seatmapReference { get; set; }
        public string flightReference { get; set; }
    }

    public class LegInfo
    {
        public DateTime departureTimeUtc { get; set; }
        public DateTime arrivalTimeUtc { get; set; }
        public int adjustedCapacity { get; set; }
        public string arrivalTerminal { get; set; }
        public int arrivalTimeVariant { get; set; }
        public int backMoveDays { get; set; }
        public int capacity { get; set; }
        public bool changeOfDirection { get; set; }
        public int codeShareIndicator { get; set; }
        public string departureTerminal { get; set; }
        public int departureTimeVariant { get; set; }
        public string equipmentType { get; set; }
        public string equipmentTypeSuffix { get; set; }
        public bool eTicket { get; set; }
        public bool irop { get; set; }
        public int lid { get; set; }
        public string marketingCode { get; set; }
        public bool marketingOverride { get; set; }
        public string operatedByText { get; set; }
        public string operatingCarrier { get; set; }
        public string operatingFlightNumber { get; set; }
        public string operatingOpSuffix { get; set; }
        public int outMoveDays { get; set; }
        public DateTime arrivalTime { get; set; }
        public DateTime departureTime { get; set; }
        public string prbcCode { get; set; }
        public string scheduleServiceType { get; set; }
        public int sold { get; set; }
        public int status { get; set; }
        public bool subjectToGovtApproval { get; set; }
    }
    public class Segment
    {
        public bool isChangeOfGauge { get; set; }
        public bool isBlocked { get; set; }
        public bool isHosted { get; set; }
        public Designator designator { get; set; }
        public bool isSeatmapViewable { get; set; }
        public string segmentKey { get; set; }
        public Identifier identifier { get; set; }
        public string cabinOfService { get; set; }
        public string externalIdentifier { get; set; }
        public bool international { get; set; }
        public int segmentType { get; set; }
        public List<Leg> legs { get; set; }
    }

    public class Ssr
    {
        public int available { get; set; }
        public string ssrNestCode { get; set; }
        public int lid { get; set; }
        public int sold { get; set; }
        public int unitSold { get; set; }
    }


}
