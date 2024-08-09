using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static DomainLayer.Model.testseat;

namespace DomainLayer.Model
{
    public class AirAsiaTripResponceModel
    {
        public int inftcount { get; set; }
        public int inftbasefare { get; set; }
        public int inftbasefaretax { get; set; }
        public int basefaretax {  get; set; }

        public int infttax { get; set; }
        public bool selfServiceMoveAvailable { get; set; }
        public object bookingKey { get; set; }
        public object recordLocator { get; set; }
        public string currencyCode { get; set; }
        public object systemCode { get; set; }
        public object groupName { get; set; }
        public AALocators locators { get; set; }
        public AAInfo info { get; set; }
        public AASales sales { get; set; }
        public AATypeofsale typeOfSale { get; set; }
        public object hold { get; set; }
        public AABreakdown breakdown { get; set; }
        public object receivedBy { get; set; }
        public AAContacts contacts { get; set; }
        public string passengerKey { get; set; }

        public List<AAPassengers> passengers { get; set; }
        public int passengerscount { get; set; }
        public List<AAJourney> journeys { get; set; }
        public object[] comments { get; set; }
        public object[] queues { get; set; }
        public object[] history { get; set; }
        public object[] payments { get; set; }

        public object[] orders { get; set; }
        public AAJourneytotals Journeytotals { get; set; }
        public passkeytype passkeytype { get; set; }
        public AADesignator designator { get; set; }
        public string PriceSolution { get; set; }
        public AAIdentifier Identifier { get; set; }
        public List<AADesignator> designatorlegs { get; set; }
        public List<AASsr> Ssr { get; set; }

    }

    //public class Infant
    //{
    //    public List<Fee> fees { get; set; }
    //    public object nationality { get; set; }
    //    public string dateOfBirth { get; set; }
    //    public object travelDocuments { get; set; }
    //    public object residentCountry { get; set; }
    //    public int gender { get; set; }
    //    public object name { get; set; }
    //    public object type { get; set; }
    //    public int total { get; set; }
    //    public int taxes { get; set; }
    //    public object adjustments { get; set; }
    //    //public List<Charge> charges { get; set; }
    //}
    //public class Fee
    //{
    //    public int type { get; set; }
    //    public object ssrCode { get; set; }
    //    public int ssrNumber { get; set; }
    //    public int paymentNumber { get; set; }
    //    public bool isConfirmed { get; set; }
    //    public bool isConfirming { get; set; }
    //    public bool isConfirmingExternal { get; set; }
    //    public string code { get; set; }
    //    public object detail { get; set; }
    //    public string passengerFeeKey { get; set; }
    //    public bool _override { get; set; }
    //    public string flightReference { get; set; }
    //    public object note { get; set; }
    //    public DateTime createdDate { get; set; }
    //    public bool isProtected { get; set; }
    //    //public Servicecharge[] serviceCharges { get; set; }
    //}
    public class AAJourney
    {
        public string Airlinename { get; set; }
        public int flightType { get; set; }
        public int stops { get; set; }
        public AADesignator designator { get; set; }
        public object move { get; set; }
        public List<AASegment> segments { get; set; }
        public string journeyKey { get; set; }
        public bool notForGeneralUser { get; set; }
    }

    public class AADesignator
    {
        public string destination { get; set; }
        public string origin { get; set; }
        public DateTime arrival { get; set; }
        public DateTime departure { get; set; }


    }

    public class AASegment
    {
        public bool isStandby { get; set; }
        public bool isConfirming { get; set; }
        public bool isConfirmingExternal { get; set; }
        public bool isBlocked { get; set; }
        public bool isHosted { get; set; }
        public bool isChangeOfGauge { get; set; }
        public AADesignator designator { get; set; }
        public bool isSeatmapViewable { get; set; }
        public List<AAFare> fares { get; set; }
        public string segmentKey { get; set; }
        public AAIdentifier identifier { get; set; }
        public AAPassengersegment passengerSegment { get; set; }
        public int channelType { get; set; }
        public string cabinOfService { get; set; }
        public object externalIdentifier { get; set; }
        public object priorityCode { get; set; }
        public int changeReasonCode { get; set; }
        public int segmentType { get; set; }
        public object salesDate { get; set; }
        public bool international { get; set; }
        public string flightReference { get; set; }
        public List<AALeg> legs { get; set; }
        public int status { get; set; }
    }

    public class AAIdentifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public object opSuffix { get; set; }
    }
    public class AALeg
    {
        public string legKey { get; set; }
        public object operationsInfo { get; set; }
        public AADesignator designator { get; set; }
        public AALeginfo legInfo { get; set; }
        public AANest[] nests { get; set; }
        public List<AASsr> ssrs { get; set; }
        public string seatmapReference { get; set; }
        public string flightReference { get; set; }
    }

    public class AALeginfo
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
        public object marketingCode { get; set; }
        public bool marketingOverride { get; set; }
        public object operatedByText { get; set; }
        public object operatingCarrier { get; set; }
        public object operatingFlightNumber { get; set; }
        public object operatingOpSuffix { get; set; }
        public int outMoveDays { get; set; }
        public DateTime arrivalTime { get; set; }
        public DateTime departureTime { get; set; }
        public string prbcCode { get; set; }
        public string scheduleServiceType { get; set; }
        public int sold { get; set; }
        public int status { get; set; }
        public bool subjectToGovtApproval { get; set; }
    }

    public class AASsr
    {
        public int available { get; set; }
        public string ssrNestCode { get; set; }
        public int lid { get; set; }
        public int sold { get; set; }
        public int unitSold { get; set; }
    }

    public class AALocators
    {
        public object numericRecordLocator { get; set; }
        public object parentRecordLocator { get; set; }
        public int parentId { get; set; }
        public object[] recordLocators { get; set; }
    }

    public class AAInfo
    {
        public int status { get; set; }
        public int paidStatus { get; set; }
        public int priceStatus { get; set; }
        public int profileStatus { get; set; }
        public object bookingType { get; set; }
        public int channelType { get; set; }
        public DateTime bookedDate { get; set; }
        public object createdDate { get; set; }
        public object expirationDate { get; set; }
        public object modifiedDate { get; set; }
        public int modifiedAgentId { get; set; }
        public int createdAgentId { get; set; }
        public object owningCarrierCode { get; set; }
        public bool changeAllowed { get; set; }
    }

    public class AASales
    {
        public AACreated created { get; set; }
        public AASource source { get; set; }
        public object modified { get; set; }
    }

    public class AACreated
    {
        public string agentCode { get; set; }
        public string domainCode { get; set; }
        public string locationCode { get; set; }
        public string organizationCode { get; set; }
    }

    public class AASource
    {
        public object isoCountryCode { get; set; }
        public object sourceSystemCode { get; set; }
        public string agentCode { get; set; }
        public string domainCode { get; set; }
        public string locationCode { get; set; }
        public string organizationCode { get; set; }
    }

    public class AATypeofsale
    {
        public string residentCountry { get; set; }
        public object promotionCode { get; set; }
        public object[] fareTypes { get; set; }
    }

    public class AABreakdown
    {
        public float balanceDue { get; set; }
        public float pointsBalanceDue { get; set; }
        public float authorizedBalanceDue { get; set; }
        public float totalAmount { get; set; }
        public float totalPoints { get; set; }
        public float totalToCollect { get; set; }
        public float totalPointsToCollect { get; set; }
        public float totalCharged { get; set; }
        public AAPassengertotals passengerTotals { get; set; }
        public AAPassengers passengers { get; set; }
        public AAJourneytotals journeyTotals { get; set; }
        public AAJourneys journeys { get; set; }
        public AAAddontotals addOnTotals { get; set; }
    }

    public class AAPassengertotals
    {
        public object services { get; set; }
        public object specialServices { get; set; }
        public object seats { get; set; }
        public object upgrades { get; set; }
        public object spoilage { get; set; }
        public object nameChanges { get; set; }
        public object convenience { get; set; }
        public object infant { get; set; }
    }



    public class AAMCFBRFQ
    {
        public string passengerKey { get; set; }
        public object services { get; set; }
        public object specialServices { get; set; }
        public object seats { get; set; }
        public object upgrades { get; set; }
        public object spoilage { get; set; }
        public object nameChanges { get; set; }
        public object convenience { get; set; }
        public object infant { get; set; }
    }

    public class AAJourneytotals
    {
        public float totalAmount { get; set; }
        public float totalPoints { get; set; }
        public float totalTax { get; set; }
        public float totalDiscount { get; set; }
    }

    public class AAJourneys
    {
        public AASTV_Idc4nh4gfn5erux_Mdyvmjqvmjaymyaxnjoynx5ct01_Mdyvmjqvmjaymyaymdoynx5ms09_ STV_IDc4NH4gfn5ERUx_MDYvMjQvMjAyMyAxNjoyNX5CT01_MDYvMjQvMjAyMyAyMDoyNX5MS09_ { get; set; }
    }

    public class AASTV_Idc4nh4gfn5erux_Mdyvmjqvmjaymyaxnjoynx5ct01_Mdyvmjqvmjaymyaymdoynx5ms09_
    {
        public string journeyKey { get; set; }
        public float totalAmount { get; set; }
        public float totalPoints { get; set; }
        public float totalTax { get; set; }
        public float totalDiscount { get; set; }
    }

    public class AAAddontotals
    {
        public object car { get; set; }
        public object hotel { get; set; }
        public object activities { get; set; }
    }

    public class AAContacts
    {
    }



    public class AAPassengers
    {
        public string passengerKey { get; set; }
        public object passengerAlternateKey { get; set; }
        public object customerNumber { get; set; }
        public object[] fees { get; set; }
        public object name { get; set; }
        public string passengerTypeCode { get; set; }
        public object discountCode { get; set; }
        public object[] bags { get; set; }
        public object program { get; set; }
        public Infant infant { get; set; }
        public AAInfo1 info { get; set; }
        public object[] travelDocuments { get; set; }
        public object[] addresses { get; set; }
        public int weightCategory { get; set; }
        public object[] emdCoupons { get; set; }

        public string _Airlinename { get; set; }
    }
    public class Infant
    {
        public List<Fee> fees { get; set; }
        public object nationality { get; set; }
        public string dateOfBirth { get; set; }
        public object travelDocuments { get; set; }
        public object residentCountry { get; set; }
        public int gender { get; set; }
        public object name { get; set; }
        public object type { get; set; }
        public int total { get; set; }
        public int taxes { get; set; }
        public object adjustments { get; set; }
        //public List<Charge> charges { get; set; }
    }
    public class Fee
    {
        public int type { get; set; }
        public object ssrCode { get; set; }
        public int ssrNumber { get; set; }
        public int paymentNumber { get; set; }
        public bool isConfirmed { get; set; }
        public bool isConfirming { get; set; }
        public bool isConfirmingExternal { get; set; }
        public string code { get; set; }
        public object detail { get; set; }
        public string passengerFeeKey { get; set; }
        public bool _override { get; set; }
        public string flightReference { get; set; }
        public object note { get; set; }
        public DateTime createdDate { get; set; }
        public bool isProtected { get; set; }
        public ServicechargeInfant ServicechargeInfant { get; set; }
        //public Servicecharge[] serviceCharges { get; set; }
    }
    public class ServicechargeInfant
    {
        public int amount { get; set; }
        public int ServiceTaxCount { get; set; }
        public string code { get; set; }
        public string detail { get; set; }
        public int type { get; set; }
        public int collectType { get; set; }
        public string currencyCode { get; set; }
        public string foreignCurrencyCode { get; set; }
        public int foreignAmount { get; set; }
        public string ticketCode { get; set; }
    }

    public class AAInfo1
    {
        public object nationality { get; set; }
        public object residentCountry { get; set; }
        public int gender { get; set; }
        public object dateOfBirth { get; set; }
        public object familyNumber { get; set; }
    }
    public class AADesignator1
    {
        public string destination { get; set; }
        public string origin { get; set; }
        public DateTime arrival { get; set; }
        public DateTime departure { get; set; }
    }



    public class AAPassengersegment
    {
        public AAMCFBRFQ2 MCFBRFQ { get; set; }
    }

    public class AAMCFBRFQ2
    {
        public object[] seats { get; set; }
        public string passengerKey { get; set; }
        public object activityDate { get; set; }
        public object boardingSequence { get; set; }
        public object createdDate { get; set; }
        public int liftStatus { get; set; }
        public object modifiedDate { get; set; }
        public int overBookIndicator { get; set; }
        public object priorityDate { get; set; }
        public bool timeChanged { get; set; }
        public object verifiedTravelDocs { get; set; }
        public object sourcePointOfSale { get; set; }
        public object pointOfSale { get; set; }
        public object[] ssrs { get; set; }
        public object[] tickets { get; set; }
        public object[] bags { get; set; }
        public object[] scores { get; set; }
        public object boardingPassDetail { get; set; }
        public bool hasInfant { get; set; }
        public AASeatpreferences seatPreferences { get; set; }
        public object bundleCode { get; set; }
        public object verifiedTravelDocuments { get; set; }
        public int referenceNumber { get; set; }
    }

    public class AASeatpreferences
    {
        public int seat { get; set; }
        public int travelClass { get; set; }
        public object[] advancedPreferences { get; set; }
    }
    public class AAFare
    {
        public bool isGoverning { get; set; }
        public bool downgradeAvailable { get; set; }
        public string carrierCode { get; set; }
        public string fareKey { get; set; }
        public string classOfService { get; set; }
        public object classType { get; set; }
        public int fareApplicationType { get; set; }
        public string fareClassOfService { get; set; }
        public string fareBasisCode { get; set; }
        public int fareSequence { get; set; }
        public int inboundOutBound { get; set; }
        public int fareStatus { get; set; }
        public bool isAllotmentMarketFare { get; set; }
        public string originalClassOfService { get; set; }
        public string ruleNumber { get; set; }
        public string productClass { get; set; }
        public object ruleTariff { get; set; }
        public string travelClassCode { get; set; }
        public object crossReferenceClassOfService { get; set; }
        public List<AAPassengerfare> passengerFares { get; set; }
        public int fareLink { get; set; }
    }

    public class AAPassengerfare
    {
        public List<AAServicecharge> serviceCharges { get; set; }
       
        public object discountCode { get; set; }
        public object fareDiscountCode { get; set; }
        public string passengerType { get; set; }
        public int basicAmount { get; set; }
    }
    public class AAServicecharge
    {
        public int amount { get; set; }
        public string code { get; set; }
        public string detail { get; set; }
        public int type { get; set; }
        public int collectType { get; set; }
        public string currencyCode { get; set; }
        public string foreignCurrencyCode { get; set; }
        public int foreignAmount { get; set; }
        public string ticketCode { get; set; }
    }
    public class AADesignator2
    {
        public string destination { get; set; }
        public string origin { get; set; }
        public DateTime arrival { get; set; }
        public DateTime departure { get; set; }
    }


    public class AANest
    {
        public int adjustedCapacity { get; set; }
        public int classNest { get; set; }
        public int lid { get; set; }
        public string travelClassCode { get; set; }
        public int nestType { get; set; }
        public List<AALegclass> legClasses { get; set; }
    }

    public class AALegclass
    {
        public int classNest { get; set; }
        public int classAllotted { get; set; }
        public string classType { get; set; }
        public int classAuthorizedUnits { get; set; }
        public string classOfService { get; set; }
        public int classRank { get; set; }
        public int classSold { get; set; }
        public int cnxSold { get; set; }
        public int latestAdvancedReservation { get; set; }
        public int status { get; set; }
        public int thruSold { get; set; }
    }



}



