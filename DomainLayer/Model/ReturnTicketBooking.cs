using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Model.ReturnTicketBooking;

namespace DomainLayer.Model
{
    public class ReturnTicketBooking
    {
        public string airLines { get; set; }
        public bool selfServiceMoveAvailable { get; set; }
        public string bookingKey { get; set; }
        public DateTime bookingdate { get; set; }
        public string recordLocator { get; set; }
        public string currencyCode { get; set; }
        public object systemCode { get; set; }
        public object groupName { get; set; }
        public Locators locators { get; set; }
        public Info info { get; set; }
        public Sales sales { get; set; }
        public TypeOfSale typeOfSale { get; set; }
        public object hold { get; set; }
        public Breakdown breakdown { get; set; }
        public double totalAmount { get; set; }
        public double totalAmountBaggage { get; set; }
        public double taxMinusMeal { get; set; }
        public double totalMealTax { get; set; }
        public double taxMinusBaggage { get; set; }
        public double TotalAmountMeal { get; set; }
        public double TotaAmountBaggage { get; set; }
        public ReceivedBy receivedBy { get; set; }
        public Contacts contacts { get; set; }
        public int passengerscount { get; set; }
        public List<ReturnPassengers> passengers { get; set; }
        public List<JourneysReturn> journeys { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<object> comments { get; set; }
        public List<object> queues { get; set; }
        public List<object> history { get; set; }
        public List<Payment> payments { get; set; }
        public List<object> orders { get; set; }

        public string customerNumber { get; set; }
        public string companyName { get; set; }
        public string emailAddressgst { get; set; }
        public string emailAddress { get; set; }
        public Hashtable Seatdata { get; set; }
        public Hashtable Mealdata { get; set; }
        public Hashtable Bagdata { get; set; }
        public Hashtable htpax { get; set; }
        public Hashtable htname { get; set; }
        public Hashtable htnameempty { get; set; }




        public Hashtable TicketSeat { get; set; }
        public Hashtable TicketCarryBag { get; set; }
        public Hashtable TicketMeal { get; set; }
        public Hashtable TicketMealAmount { get; set; }
        public Hashtable TicketCarryBagAMount { get; set; }

        public class Address
        {
            public string lineOne { get; set; }
            public string lineTwo { get; set; }
            public object lineThree { get; set; }
            public string countryCode { get; set; }
            public string provinceState { get; set; }
            public string city { get; set; }
            public string postalCode { get; set; }
        }
        public class Amounts
        {
            public double amount { get; set; }
            public string currencyCode { get; set; }
            public double collected { get; set; }
            public string collectedCurrencyCode { get; set; }
            public double quoted { get; set; }
            public string quotedCurrencyCode { get; set; }
        }
        public class Breakdown
        {
            public double balanceDue { get; set; }
            public double pointsBalanceDue { get; set; }
            public double authorizedBalanceDue { get; set; }
            public double totalAmount { get; set; }
            public double totalPoints { get; set; }
            public double totalToCollect { get; set; }
            public double totalPointsToCollect { get; set; }
            public double totalCharged { get; set; }
            public double totalAmountSum { get; set; }
            public double totaltax { get; set; }
            public double totalplusAmountSumtax { get; set; }
            public decimal TotalMealBagAmount { get; set; }
            public double baseTotalAmount { get; set; }
            public double BaseTotalTax { get; set; }
            public double ToatalBasePrice { get; set; }
            public PassengerTotals passengerTotals { get; set; }
            public ReturnPassengers passengers { get; set; }
            public JourneyTotals journeyTotals { get; set; }
            public List<JourneyTotals> journeyfareTotals { get; set; }
            public JourneyTotals InfantfareTotals { get; set; }
            public JourneysReturn journeys { get; set; }

        }
        public class ReturnCharge
        {
            public double amount { get; set; }
            public double totalAmount { get; set; }
            public double totalAmountBaggage { get; set; }
            public string code { get; set; }
            public string detail { get; set; }
            public int type { get; set; }
            public int collectType { get; set; }
            public string currencyCode { get; set; }
            public string foreignCurrencyCode { get; set; }
            public double foreignAmount { get; set; }
            public string ticketCode { get; set; }
        }
        public class Contacts
        {
            public string ReturnPaxSeats { get; set; }
            public string phoneNumbers { get; set; }
            public P P { get; set; }
            //public List<PhoneNumber> PhoneNumbers { get; set; }
        }
        public class PhoneNumber
        {
            public int type { get; set; }
            public string number { get; set; }
        }
        public class Created
        {
            public string agentCode { get; set; }
            public string domainCode { get; set; }
            public string locationCode { get; set; }
            public string organizationCode { get; set; }
        }
        public class DesignatorReturn
        {
            public string destination { get; set; }
            public string origin { get; set; }
            public DateTime arrival { get; set; }
            public DateTime departure { get; set; }
        }
        public class Details
        {
            public int accountNumberId { get; set; }
            public object parentPaymentId { get; set; }
            public object accountName { get; set; }
            public string accountNumber { get; set; }
            public object expirationDate { get; set; }
            public object text { get; set; }
            public int installments { get; set; }
            public int binRange { get; set; }
            public Fields fields { get; set; }
        }
        public class FareReturn
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
            public List<PassengerFareReturn> passengerFares { get; set; }
            public int fareLink { get; set; }
        }
        public class Fee
        {
            public int type { get; set; }
            public string ssrCode { get; set; }
            public int ssrNumber { get; set; }
            public int paymentNumber { get; set; }
            public bool isConfirmed { get; set; }
            public bool isConfirming { get; set; }
            public bool isConfirmingExternal { get; set; }
            public string code { get; set; }
            public object detail { get; set; }
            public string passengerFeeKey { get; set; }
            public bool @override { get; set; }
            public string flightReference { get; set; }
            public object note { get; set; }
            public DateTime createdDate { get; set; }
            public bool isProtected { get; set; }
            public List<ServiceChargeReturn> serviceCharges { get; set; }
        }
        public class Fields
        {
            public string AMT { get; set; }
        }
        public class IdentifierReturn
        {
            public string identifier { get; set; }
            public string carrierCode { get; set; }
            public object opSuffix { get; set; }
        }
        public class InfantReturn
        {
            public double total { get; set; }
            public double taxes { get; set; }
            public object adjustments { get; set; }
            public List<ReturnCharge> charges { get; set; }
            public List<Fee> fees { get; set; }
            public string nationality { get; set; }
            public DateTime dateOfBirth { get; set; }
            public object travelDocuments { get; set; }
            public string residentCountry { get; set; }
            public int gender { get; set; }
            public Name name { get; set; }
            public object type { get; set; }
        }
        public class Info
        {
            public int status { get; set; }
            public int paidStatus { get; set; }
            public int priceStatus { get; set; }
            public int profileStatus { get; set; }
            public object bookingType { get; set; }
            public int channelType { get; set; }
            public DateTime bookedDate { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime expirationDate { get; set; }
            public DateTime modifiedDate { get; set; }
            public int modifiedAgentId { get; set; }
            public int createdAgentId { get; set; }
            public string owningCarrierCode { get; set; }
            public bool changeAllowed { get; set; }
            public string nationality { get; set; }
            public string residentCountry { get; set; }
            public int gender { get; set; }
            public DateTime dateOfBirth { get; set; }
            public object familyNumber { get; set; }
        }
        public class JourneysReturn
        {
            public int flightType { get; set; }
            public int stops { get; set; }
            public DesignatorReturn designator { get; set; }
            public object move { get; set; }
            public List<SegmentReturn> segments { get; set; }
            public string journeyKey { get; set; }
            public bool notForGeneralUser { get; set; }
            public STVIDcyMn4gfn5CTFJMTIvMzAvMjAyMiAwMDo0MH5ERUxMTIvMzAvMjAyMiAwMzozMH5 STV_IDcyMn4gfn5CTFJ_MTIvMzAvMjAyMiAwMDo0MH5ERUx_MTIvMzAvMjAyMiAwMzozMH5_ { get; set; }
            public STVMTUyOX4gfn5ERUxMTIvMzEvMjAyMiAwOTozNX5CTFJMTIvMzEvMjAyMiAxMjoyNX5 STV_MTUyOX4gfn5ERUx_MTIvMzEvMjAyMiAwOTozNX5CTFJ_MTIvMzEvMjAyMiAxMjoyNX5_ { get; set; }
        }
        public class JourneyTotals
        {
            public double totalAmount { get; set; }
            public double totalPoints { get; set; }
            public double totalTax { get; set; }
            public double totalDiscount { get; set; }
        }
        public class LegReturn
        {
            public string legKey { get; set; }
            public object operationsInfo { get; set; }
            public DesignatorReturn designator { get; set; }
            public LegInfoReturn legInfo { get; set; }
            public List<Nest> nests { get; set; }
            public List<Ssr> ssrs { get; set; }
            public string seatmapReference { get; set; }
            public string flightReference { get; set; }
        }
        public class LegClass
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
        public class LegInfoReturn
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
            public object departureTerminal { get; set; }
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
        public class Locators
        {
            public string numericRecordLocator { get; set; }
            public object parentRecordLocator { get; set; }
            public int parentId { get; set; }
            public List<object> recordLocators { get; set; }
        }
        public class Market
        {
            public Identifier identifier { get; set; }
            public string destination { get; set; }
            public string origin { get; set; }
            public DateTime departureDate { get; set; }
        }
        public class MCFBRFQ
        {
            public string passengerKey { get; set; }
            public object services { get; set; }
            public SpecialServices specialServices { get; set; }
            public ReturnSeats seats { get; set; }
            public object upgrades { get; set; }
            public object spoilage { get; set; }
            public object nameChanges { get; set; }
            public object convenience { get; set; }
            public Infant infant { get; set; }
            public string passengerAlternateKey { get; set; }
            public object customerNumber { get; set; }
            public List<Fee> fees { get; set; }
            public Name name { get; set; }
            public string passengerTypeCode { get; set; }
            public object discountCode { get; set; }
            public List<object> bags { get; set; }
            public object program { get; set; }
            public Info info { get; set; }
            public List<object> travelDocuments { get; set; }
            public List<object> addresses { get; set; }
            public int weightCategory { get; set; }
            public List<object> emdCoupons { get; set; }
            public DateTime activityDate { get; set; }
            public object boardingSequence { get; set; }
            public DateTime createdDate { get; set; }
            public int liftStatus { get; set; }
            public object modifiedDate { get; set; }
            public int overBookIndicator { get; set; }
            public DateTime priorityDate { get; set; }
            public bool timeChanged { get; set; }
            public object verifiedTravelDocs { get; set; }
            public object sourcePointOfSale { get; set; }
            public object pointOfSale { get; set; }
            public List<Ssr> ssrs { get; set; }
            public List<object> tickets { get; set; }
            public List<object> scores { get; set; }
            public object boardingPassDetail { get; set; }
            public bool hasInfant { get; set; }
            public SeatPreferences seatPreferences { get; set; }
            public object bundleCode { get; set; }
            public object verifiedTravelDocuments { get; set; }
            public int referenceNumber { get; set; }
        }
        public class MiFDSEQ
        {
            public string passengerKey { get; set; }
            public object services { get; set; }
            public SpecialServices specialServices { get; set; }
            public ReturnSeats seats { get; set; }
            public object upgrades { get; set; }
            public object spoilage { get; set; }
            public object nameChanges { get; set; }
            public object convenience { get; set; }
            public object infant { get; set; }
            public string passengerAlternateKey { get; set; }
            public object customerNumber { get; set; }
            public List<Fee> fees { get; set; }
            public Name name { get; set; }
            public string passengerTypeCode { get; set; }
            public object discountCode { get; set; }
            public List<object> bags { get; set; }
            public object program { get; set; }
            public Info info { get; set; }
            public List<object> travelDocuments { get; set; }
            public List<object> addresses { get; set; }
            public int weightCategory { get; set; }
            public List<object> emdCoupons { get; set; }
            public DateTime activityDate { get; set; }
            public object boardingSequence { get; set; }
            public DateTime createdDate { get; set; }
            public int liftStatus { get; set; }
            public object modifiedDate { get; set; }
            public int overBookIndicator { get; set; }
            public DateTime priorityDate { get; set; }
            public bool timeChanged { get; set; }
            public object verifiedTravelDocs { get; set; }
            public object sourcePointOfSale { get; set; }
            public object pointOfSale { get; set; }
            public List<Ssr> ssrs { get; set; }
            public List<object> tickets { get; set; }
            public List<object> scores { get; set; }
            public object boardingPassDetail { get; set; }
            public bool hasInfant { get; set; }
            public SeatPreferences seatPreferences { get; set; }
            public object bundleCode { get; set; }
            public object verifiedTravelDocuments { get; set; }
            public int referenceNumber { get; set; }
        }
        public class Modified
        {
            public string agentCode { get; set; }
            public string domainCode { get; set; }
            public string locationCode { get; set; }
            public string organizationCode { get; set; }
        }
        public class MSFBRFQ
        {
            public string passengerKey { get; set; }
            public object services { get; set; }
            public SpecialServices specialServices { get; set; }
            public ReturnSeats seats { get; set; }
            public object upgrades { get; set; }
            public object spoilage { get; set; }
            public object nameChanges { get; set; }
            public object convenience { get; set; }
            public Infant infant { get; set; }
            public string passengerAlternateKey { get; set; }
            public object customerNumber { get; set; }
            public List<Fee> fees { get; set; }
            public Name name { get; set; }
            public string passengerTypeCode { get; set; }
            public object discountCode { get; set; }
            public List<object> bags { get; set; }
            public object program { get; set; }
            public Info info { get; set; }
            public List<object> travelDocuments { get; set; }
            public List<object> addresses { get; set; }
            public int weightCategory { get; set; }
            public List<object> emdCoupons { get; set; }
            public DateTime activityDate { get; set; }
            public object boardingSequence { get; set; }
            public DateTime createdDate { get; set; }
            public int liftStatus { get; set; }
            public object modifiedDate { get; set; }
            public int overBookIndicator { get; set; }
            public DateTime priorityDate { get; set; }
            public bool timeChanged { get; set; }
            public object verifiedTravelDocs { get; set; }
            public object sourcePointOfSale { get; set; }
            public object pointOfSale { get; set; }
            public List<Ssr> ssrs { get; set; }
            public List<object> tickets { get; set; }
            public List<object> scores { get; set; }
            public object boardingPassDetail { get; set; }
            public bool hasInfant { get; set; }
            public SeatPreferences seatPreferences { get; set; }
            public object bundleCode { get; set; }
            public object verifiedTravelDocuments { get; set; }
            public int referenceNumber { get; set; }
        }
        public class MyFDSEQ
        {
            public string passengerKey { get; set; }
            public object services { get; set; }
            public SpecialServices specialServices { get; set; }
            public ReturnSeats seats { get; set; }
            public object upgrades { get; set; }
            public object spoilage { get; set; }
            public object nameChanges { get; set; }
            public object convenience { get; set; }
            public object infant { get; set; }
            public string passengerAlternateKey { get; set; }
            public object customerNumber { get; set; }
            public List<Fee> fees { get; set; }
            public Name name { get; set; }
            public string passengerTypeCode { get; set; }
            public object discountCode { get; set; }
            public List<object> bags { get; set; }
            public object program { get; set; }
            public Info info { get; set; }
            public List<object> travelDocuments { get; set; }
            public List<object> addresses { get; set; }
            public int weightCategory { get; set; }
            public List<object> emdCoupons { get; set; }
            public DateTime activityDate { get; set; }
            public object boardingSequence { get; set; }
            public DateTime createdDate { get; set; }
            public int liftStatus { get; set; }
            public object modifiedDate { get; set; }
            public int overBookIndicator { get; set; }
            public DateTime priorityDate { get; set; }
            public bool timeChanged { get; set; }
            public object verifiedTravelDocs { get; set; }
            public object sourcePointOfSale { get; set; }
            public object pointOfSale { get; set; }
            public List<Ssr> ssrs { get; set; }
            public List<object> tickets { get; set; }
            public List<object> scores { get; set; }
            public object boardingPassDetail { get; set; }
            public bool hasInfant { get; set; }
            public SeatPreferences seatPreferences { get; set; }
            public object bundleCode { get; set; }
            public object verifiedTravelDocuments { get; set; }
            public int referenceNumber { get; set; }
        }
        public class Name
        {
            public string first { get; set; }
            public object middle { get; set; }
            public string last { get; set; }
            public string title { get; set; }
            public object suffix { get; set; }
        }
        public class Nest
        {
            public int adjustedCapacity { get; set; }
            public int classNest { get; set; }
            public int lid { get; set; }
            public string travelClassCode { get; set; }
            public int nestType { get; set; }
            public List<LegClass> legClasses { get; set; }
        }
        public class P
        {
            public string contactTypeCode { get; set; }
            public List<PhoneNumber> phoneNumbers { get; set; }
            public object cultureCode { get; set; }
            public Address address { get; set; }
            public string emailAddress { get; set; }
            public object customerNumber { get; set; }
            public object sourceOrganization { get; set; }
            public int distributionOption { get; set; }
            public int notificationPreference { get; set; }
            public object companyName { get; set; }
            public Name name { get; set; }
        }
        public class PassengerFareReturn
        {
            public List<ServiceChargeReturn> serviceCharges { get; set; }
            public object discountCode { get; set; }
            public object fareDiscountCode { get; set; }
            public string passengerType { get; set; }
        }
        public class ReturnPassengers
        {
            public List<string> barcodestringlst { get; set; }
            public string barcodestring { get; set; }
            public string BarcodeInfantString { get; set; }
            public string passengerKey { get; set; }
            public object passengerAlternateKey { get; set; }
            public object customerNumber { get; set; }
            public object[] fees { get; set; }
            public Name name { get; set; }
            public string MobNumber { get; set; }
            public string passengerTypeCode { get; set; }
            public object discountCode { get; set; }
            public object[] bags { get; set; }
            public object program { get; set; }
            public InfantReturn infant { get; set; }
            public AAInfo1 info { get; set; }
            public object[] travelDocuments { get; set; }
            public object[] addresses { get; set; }
            public int weightCategory { get; set; }
            public object[] emdCoupons { get; set; }
            public ServiceChargeReturn ServiceChargeReturn { get; set; }
            public List<ReturnCharge> charges { get; set; }
            public ReturnSeats seats { get; set; }
        }
        public class PassengerSegment
        {
            public string passengerKey { get; set; }
            public List<ReturnSeats> seats { get; set; }
            public List<SsrReturn> SsrReturn { get; set; }

        }
        public class ReturnSeats
        {
            public string compartmentDesignator { get; set; }
            public int penalty { get; set; }
            public string unitDesignator { get; set; }
            public string SSRCode { get; set; }
            public SeatInformation seatInformation { get; set; }
            public string arrivalStation { get; set; }
            public string departureStation { get; set; }
            public string passengerKey { get; set; }
            public string unitKey { get; set; }
            public object crossReferenceSeatingPreference { get; set; }
            public bool isPending { get; set; }
            public int total { get; set; }
            public int totalSeatAmount { get; set; }
            public int taxes { get; set; }
            public object adjustments { get; set; }
            public List<ReturnCharge> charges { get; set; }
        }
        public class PassengerTotals
        {
            public object services { get; set; }
            public SpecialServices specialServices { get; set; }
            public SpecialServices fastForward { get; set; }
            public SpecialServices baggage { get; set; }
            public ReturnSeats seats { get; set; }
            public object upgrades { get; set; }
            public object spoilage { get; set; }
            public object nameChanges { get; set; }
            public object convenience { get; set; }
            public InfantReturn infant { get; set; }
        }
        public class SsrReturn
        {
            public bool isConfirmed { get; set; }
            public bool isConfirmingUnheld { get; set; }
            public string note { get; set; }
            public int ssrDuration { get; set; }
            public string ssrKey { get; set; }
            public int count { get; set; }
            public string ssrCode { get; set; }
            public string MealName { get; set; }
            public string feeCode { get; set; }
            public bool inBundle { get; set; }
            public string passengerKey { get; set; }
            public object ssrDetail { get; set; }
            public int ssrNumber { get; set; }
            public Market market { get; set; }
            public int available { get; set; }
            public string ssrNestCode { get; set; }
            public int lid { get; set; }
            public int sold { get; set; }
            public int unitSold { get; set; }
        }
        public class Payment
        {
            public string paymentKey { get; set; }
            public string code { get; set; }
            public DateTime approvalDate { get; set; }
            public Details details { get; set; }
            public Amounts amounts { get; set; }
            public object authorizationCode { get; set; }
            public int authorizationStatus { get; set; }
            public DateTime fundedDate { get; set; }
            public object transactionCode { get; set; }
            public object dcc { get; set; }
            public ThreeDSecure threeDSecure { get; set; }
            public List<object> attachments { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime modifiedDate { get; set; }
            public int type { get; set; }
            public int status { get; set; }
            public bool transferred { get; set; }
            public int channelType { get; set; }
            public PointOfSale pointOfSale { get; set; }
            public SourcePointOfSale sourcePointOfSale { get; set; }
            public bool deposit { get; set; }
            public int accountId { get; set; }
            public object voucher { get; set; }
            public bool addedToState { get; set; }
            public int createdAgentId { get; set; }
            public int modifiedAgentId { get; set; }
            public int reference { get; set; }
        }

        public class PointOfSale
        {
            public string agentCode { get; set; }
            public string domainCode { get; set; }
            public string locationCode { get; set; }
            public string organizationCode { get; set; }
        }
        public class PropertyList
        {
            public string BULKHEAD { get; set; }
            public string LEGROOM { get; set; }
            public string LAVATORY { get; set; }
            public string EXITROW { get; set; }
            public string AISLE { get; set; }
            public string BRDZONE { get; set; }
            public string SRVZONE { get; set; }
            public string TCC { get; set; }
            public string WINDOW { get; set; }
        }
        public class ReceivedBy
        {
            public object receivedBy { get; set; }
            public object latestReceivedBy { get; set; }
            public object receivedReference { get; set; }
            public object latestReceivedReference { get; set; }
            public object referralCode { get; set; }
        }
        public class Sales
        {
            public Created created { get; set; }
            public Source source { get; set; }
            public Modified modified { get; set; }
        }
        public class ReturnPaxSeats
        {
            public string unitDesignatorPax { get; set; }
        }

        public class SeatInformation
        {
            public int deck { get; set; }
            public int seatSet { get; set; }
            public PropertyList propertyList { get; set; }
        }
        public class SeatPreferences
        {
            public int seat { get; set; }
            public int travelClass { get; set; }
            public List<object> advancedPreferences { get; set; }
        }
        public class SegmentReturn
        {
            public bool isStandby { get; set; }
            public bool isConfirming { get; set; }
            public bool isConfirmingExternal { get; set; }
            public bool isBlocked { get; set; }
            public bool isHosted { get; set; }
            public bool isChangeOfGauge { get; set; }
            public DesignatorReturn designator { get; set; }
            public bool isSeatmapViewable { get; set; }
            public List<FareReturn> fares { get; set; }
            public string segmentKey { get; set; }
            public IdentifierReturn identifier { get; set; }
            public List<PassengerSegment> passengerSegment { get; set; }
            public int channelType { get; set; }
            public string cabinOfService { get; set; }
            public object externalIdentifier { get; set; }
            public object priorityCode { get; set; }
            public int changeReasonCode { get; set; }
            public int segmentType { get; set; }
            public DateTime salesDate { get; set; }
            public bool international { get; set; }
            public string flightReference { get; set; }
            public List<LegReturn> legs { get; set; }
            public int status { get; set; }
            public string unitdesignator { get; set; }
            public string SSRCode { get; set; }
        }
        public class ServiceChargeReturn
        {
            public double amount { get; set; }
            public string code { get; set; }
            public string detail { get; set; }
            public int type { get; set; }
            public int collectType { get; set; }
            public string currencyCode { get; set; }
            public string foreignCurrencyCode { get; set; }
            public double foreignAmount { get; set; }
            public string ticketCode { get; set; }
            public List<ReturnCharge> charges { get; set; }
        }
        public class Source
        {
            public object isoCountryCode { get; set; }
            public object sourceSystemCode { get; set; }
            public string agentCode { get; set; }
            public string domainCode { get; set; }
            public string locationCode { get; set; }
            public string organizationCode { get; set; }
        }
        public class SourcePointOfSale
        {
            public string agentCode { get; set; }
            public string domainCode { get; set; }
            public string locationCode { get; set; }
            public string organizationCode { get; set; }
        }
        public class SpecialServices
        {
            public decimal total { get; set; }
            public decimal taxes { get; set; }
            public object adjustments { get; set; }
            public List<ReturnCharge> charges { get; set; }
        }

        public class STVIDcyMn4gfn5CTFJMTIvMzAvMjAyMiAwMDo0MH5ERUxMTIvMzAvMjAyMiAwMzozMH5
        {
            public string journeyKey { get; set; }
            public double totalAmount { get; set; }
            public double totalPoints { get; set; }
            public double totalTax { get; set; }
            public double totalDiscount { get; set; }
        }
        public class STVMTUyOX4gfn5ERUxMTIvMzEvMjAyMiAwOTozNX5CTFJMTIvMzEvMjAyMiAxMjoyNX5
        {
            public string journeyKey { get; set; }
            public double totalAmount { get; set; }
            public double totalPoints { get; set; }
            public double totalTax { get; set; }
            public double totalDiscount { get; set; }
        }
        public class ThreeDSecure
        {
            public string browserUserAgent { get; set; }
            public string browserAccept { get; set; }
            public string remoteIpAddress { get; set; }
            public string termUrl { get; set; }
            public object paReq { get; set; }
            public object acsUrl { get; set; }
            public object paRes { get; set; }
            public object authResult { get; set; }
            public object cavv { get; set; }
            public object cavvAlgorithm { get; set; }
            public object eci { get; set; }
            public object xid { get; set; }
            public bool applicable { get; set; }
            public bool successful { get; set; }
        }
        public class TypeOfSale
        {
            public string residentCountry { get; set; }
            public string promotionCode { get; set; }
            public List<object> fareTypes { get; set; }
        }
    }
}
