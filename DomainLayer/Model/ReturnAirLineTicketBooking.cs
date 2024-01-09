using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class ReturnAirLineTicketBooking
    {  
            public int State { get; set; }
            public bool StateSpecified { get; set; }
            public string RecordLocatorReturn { get; set; }
            public string CurrencyCode { get; set; }
            public int PaxCount { get; set; }
            public bool PaxCountSpecified { get; set; }
            public string SystemCode { get; set; }
            public int BookingID { get; set; }
            public bool BookingIDSpecified { get; set; }
            public int BookingParentID { get; set; }
            public bool BookingParentIDSpecified { get; set; }
            public string ParentRecordLocator { get; set; }
            public string BookingChangeCode { get; set; }
            public string GroupName { get; set; }
            public BookingInfo BookingInfoRT { get; set; }
            public POS POSRT { get; set; }
            public SourcePOS SourcePOSRT { get; set; }
            public TypeOfSale TypeOfSaleRT { get; set; }
            public BookingHold BookingHoldRT { get; set; }
            public BookingSum BookingSumRT { get; set; }
            public ReceivedBy ReceivedByRT { get; set; }
            public List<RecordLocator> RecordLocators { get; set; }
            public List<PassengerRT> Passengers { get; set; }
            public List<JourneyRT> Journeys { get; set; }
            public List<BookingComment> BookingComments { get; set; }
            public List<object> BookingQueueInfos { get; set; }
            public List<BookingContact> BookingContacts { get; set; }
            public List<object> Payments { get; set; }
            public List<object> BookingComponents { get; set; }
            public string NumericRecordLocator { get; set; }
            public SourceBookingPOS SourceBookingPOSRT { get; set; }
            public int ReissueCount { get; set; }
            public bool ReissueCountSpecified { get; set; }
            public string PricingReissueMode { get; set; }
            public string BaggagePricingSystemCode { get; set; }
            public List<object> OtherServiceInfoList { get; set; }
       

        public class BookingComment
        {
            public int CommentType { get; set; }
            public bool CommentTypeSpecified { get; set; }
            public string CommentText { get; set; }
            public PointOfSale PointOfSale { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool CreatedDateSpecified { get; set; }
            public bool SendToBookingSource { get; set; }
            public bool SendToBookingSourceSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class BookingContact
        {
            public string TypeCode { get; set; }
            public List<NameRT> Names { get; set; }
            public string EmailAddress { get; set; }
            public string HomePhone { get; set; }
            public string WorkPhone { get; set; }
            public string OtherPhone { get; set; }
            public string Fax { get; set; }
            public string CompanyName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string City { get; set; }
            public string ProvinceState { get; set; }
            public string PostalCode { get; set; }
            public string CountryCode { get; set; }
            public string CultureCode { get; set; }
            public int DistributionOption { get; set; }
            public bool DistributionOptionSpecified { get; set; }
            public string CustomerNumber { get; set; }
            public int NotificationPreference { get; set; }
            public bool NotificationPreferenceSpecified { get; set; }
            public string SourceOrganization { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class BookingHold
        {
            public DateTime HoldDateTime { get; set; }
            public bool HoldDateTimeSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class BookingInfo
        {
            public int BookingStatus { get; set; }
            public bool BookingStatusSpecified { get; set; }
            public string BookingType { get; set; }
            public int ChannelType { get; set; }
            public bool ChannelTypeSpecified { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool CreatedDateSpecified { get; set; }
            public DateTime ExpiredDate { get; set; }
            public bool ExpiredDateSpecified { get; set; }
            public DateTime ModifiedDate { get; set; }
            public bool ModifiedDateSpecified { get; set; }
            public int PriceStatus { get; set; }
            public bool PriceStatusSpecified { get; set; }
            public int ProfileStatus { get; set; }
            public bool ProfileStatusSpecified { get; set; }
            public bool ChangeAllowed { get; set; }
            public bool ChangeAllowedSpecified { get; set; }
            public int CreatedAgentID { get; set; }
            public bool CreatedAgentIDSpecified { get; set; }
            public int ModifiedAgentID { get; set; }
            public bool ModifiedAgentIDSpecified { get; set; }
            public DateTime BookingDate { get; set; }
            public bool BookingDateSpecified { get; set; }
            public string OwningCarrierCode { get; set; }
            public int PaidStatus { get; set; }
            public bool PaidStatusSpecified { get; set; }
            public DateTime ActivityDate { get; set; }
            public bool ActivityDateSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class BookingSum
        {
            public int BalanceDue { get; set; }
            public bool BalanceDueSpecified { get; set; }
            public int AuthorizedBalanceDue { get; set; }
            public bool AuthorizedBalanceDueSpecified { get; set; }
            public int SegmentCount { get; set; }
            public bool SegmentCountSpecified { get; set; }
            public int PassiveSegmentCount { get; set; }
            public bool PassiveSegmentCountSpecified { get; set; }
            public int TotalCost { get; set; }
            public bool TotalCostSpecified { get; set; }
            public int PointsBalanceDue { get; set; }
            public bool PointsBalanceDueSpecified { get; set; }
            public int TotalPointCost { get; set; }
            public bool TotalPointCostSpecified { get; set; }
            public string AlternateCurrencyCode { get; set; }
            public int AlternateCurrencyBalanceDue { get; set; }
            public bool AlternateCurrencyBalanceDueSpecified { get; set; }
        }

        public class FareRT
        {
            public string ClassOfService { get; set; }
            public string ClassType { get; set; }
            public string RuleTariff { get; set; }
            public string CarrierCode { get; set; }
            public string RuleNumber { get; set; }
            public string FareBasisCode { get; set; }
            public int FareSequence { get; set; }
            public bool FareSequenceSpecified { get; set; }
            public string FareClassOfService { get; set; }
            public int FareStatus { get; set; }
            public bool FareStatusSpecified { get; set; }
            public int FareApplicationType { get; set; }
            public bool FareApplicationTypeSpecified { get; set; }
            public string OriginalClassOfService { get; set; }
            public string XrefClassOfService { get; set; }
            public List<PaxFareRT> PaxFares { get; set; }
            public string ProductClass { get; set; }
            public bool IsAllotmentMarketFare { get; set; }
            public bool IsAllotmentMarketFareSpecified { get; set; }
            public string TravelClassCode { get; set; }
            public string FareSellKey { get; set; }
            public int InboundOutbound { get; set; }
            public bool InboundOutboundSpecified { get; set; }
            public int FareLink { get; set; }
            public bool FareLinkSpecified { get; set; }
            public FareDesignatorRT FareDesignator { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class FareDesignatorRT
        {
            public string FareTypeIndicator { get; set; }
            public string CityCode { get; set; }
            public string TravelCityCode { get; set; }
            public string RuleFareTypeCode { get; set; }
            public string BaseFareFareClassCode { get; set; }
            public string DowType { get; set; }
            public string SeasonType { get; set; }
            public int RoutingNumber { get; set; }
            public bool RoutingNumberSpecified { get; set; }
            public string OneWayRoundTrip { get; set; }
            public bool OpenJawAllowed { get; set; }
            public bool OpenJawAllowedSpecified { get; set; }
            public string TripDirection { get; set; }
        }

        public class FlightDesignator
        {
            public string CarrierCode { get; set; }
            public string FlightNumber { get; set; }
            public string OpSuffix { get; set; }
        }

        public class JourneyRT
        {
            public bool NotForGeneralUse { get; set; }
            public bool NotForGeneralUseSpecified { get; set; }
            public List<SegmentRT> Segments { get; set; }
            public string JourneySellKey { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class Leg
        {
            public string ArrivalStation { get; set; }
            public string DepartureStation { get; set; }
            public DateTime STA { get; set; }
            public bool STASpecified { get; set; }
            public DateTime STD { get; set; }
            public bool STDSpecified { get; set; }
            public FlightDesignator FlightDesignator { get; set; }
            public LegInfo LegInfo { get; set; }
            public object OperationsInfo { get; set; }
            public int InventoryLegID { get; set; }
            public bool InventoryLegIDSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class LegInfoRT
        {
            public int AdjustedCapacity { get; set; }
            public bool AdjustedCapacitySpecified { get; set; }
            public string EquipmentType { get; set; }
            public string EquipmentTypeSuffix { get; set; }
            public string ArrivalTerminal { get; set; }
            public int ArrvLTV { get; set; }
            public bool ArrvLTVSpecified { get; set; }
            public int Capacity { get; set; }
            public bool CapacitySpecified { get; set; }
            public string CodeShareIndicator { get; set; }
            public string DepartureTerminal { get; set; }
            public int DeptLTV { get; set; }
            public bool DeptLTVSpecified { get; set; }
            public bool ETicket { get; set; }
            public bool ETicketSpecified { get; set; }
            public bool FlifoUpdated { get; set; }
            public bool FlifoUpdatedSpecified { get; set; }
            public bool IROP { get; set; }
            public bool IROPSpecified { get; set; }
            public int Status { get; set; }
            public bool StatusSpecified { get; set; }
            public int Lid { get; set; }
            public bool LidSpecified { get; set; }
            public string OnTime { get; set; }
            public DateTime PaxSTA { get; set; }
            public bool PaxSTASpecified { get; set; }
            public DateTime PaxSTD { get; set; }
            public bool PaxSTDSpecified { get; set; }
            public string PRBCCode { get; set; }
            public string ScheduleServiceType { get; set; }
            public int Sold { get; set; }
            public bool SoldSpecified { get; set; }
            public int OutMoveDays { get; set; }
            public bool OutMoveDaysSpecified { get; set; }
            public int BackMoveDays { get; set; }
            public bool BackMoveDaysSpecified { get; set; }
            public List<object> LegNests { get; set; }
            public List<object> LegSSRs { get; set; }
            public string OperatingFlightNumber { get; set; }
            public string OperatedByText { get; set; }
            public string OperatingCarrier { get; set; }
            public string OperatingOpSuffix { get; set; }
            public bool SubjectToGovtApproval { get; set; }
            public bool SubjectToGovtApprovalSpecified { get; set; }
            public string MarketingCode { get; set; }
            public bool ChangeOfDirection { get; set; }
            public bool ChangeOfDirectionSpecified { get; set; }
            public bool MarketingOverride { get; set; }
            public bool MarketingOverrideSpecified { get; set; }
            public string AircraftOwner { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class NameRT
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Suffix { get; set; }
            public string Title { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerRT
        {
            public object PassengerPrograms { get; set; }
            public string CustomerNumber { get; set; }
            public int PassengerNumber { get; set; }
            public bool PassengerNumberSpecified { get; set; }
            public int FamilyNumber { get; set; }
            public bool FamilyNumberSpecified { get; set; }
            public string PaxDiscountCode { get; set; }
            public List<NameRT> Names { get; set; }
            public object Infant { get; set; }
            public PassengerInfoRT PassengerInfo { get; set; }
            public PassengerProgram PassengerProgram { get; set; }
            public List<PassengerFee> PassengerFees { get; set; }
            public List<object> PassengerAddresses { get; set; }
            public List<object> PassengerTravelDocuments { get; set; }
            public List<object> PassengerBags { get; set; }
            public int PassengerID { get; set; }
            public bool PassengerIDSpecified { get; set; }
            public List<PassengerTypeInfo> PassengerTypeInfos { get; set; }
            public List<PassengerInfoRT> PassengerInfos { get; set; }
            public List<object> PassengerInfants { get; set; }
            public bool PseudoPassenger { get; set; }
            public bool PseudoPassengerSpecified { get; set; }
            public PassengerTypeInfo PassengerTypeInfo { get; set; }
            public List<object> PassengerEMDCouponList { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerFee
        {
            public string ActionStatusCode { get; set; }
            public string FeeCode { get; set; }
            public string FeeDetail { get; set; }
            public int FeeNumber { get; set; }
            public bool FeeNumberSpecified { get; set; }
            public int FeeType { get; set; }
            public bool FeeTypeSpecified { get; set; }
            public bool FeeOverride { get; set; }
            public bool FeeOverrideSpecified { get; set; }
            public string FlightReference { get; set; }
            public string Note { get; set; }
            public string SSRCode { get; set; }
            public int SSRNumber { get; set; }
            public bool SSRNumberSpecified { get; set; }
            public int PaymentNumber { get; set; }
            public bool PaymentNumberSpecified { get; set; }
            public List<ServiceCharge> ServiceCharges { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool CreatedDateSpecified { get; set; }
            public bool IsProtected { get; set; }
            public bool IsProtectedSpecified { get; set; }
            public int FeeApplicationType { get; set; }
            public bool FeeApplicationTypeSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerInfoRT
        {
            public int BalanceDue { get; set; }
            public bool BalanceDueSpecified { get; set; }
            public int Gender { get; set; }
            public bool GenderSpecified { get; set; }
            public string Nationality { get; set; }
            public string ResidentCountry { get; set; }
            public int TotalCost { get; set; }
            public bool TotalCostSpecified { get; set; }
            public int WeightCategory { get; set; }
            public bool WeightCategorySpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerInfo2
        {
            public int BalanceDue { get; set; }
            public bool BalanceDueSpecified { get; set; }
            public int Gender { get; set; }
            public bool GenderSpecified { get; set; }
            public string Nationality { get; set; }
            public string ResidentCountry { get; set; }
            public int TotalCost { get; set; }
            public bool TotalCostSpecified { get; set; }
            public int WeightCategory { get; set; }
            public bool WeightCategorySpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerProgram
        {
            public string ProgramCode { get; set; }
            public string ProgramLevelCode { get; set; }
            public string ProgramNumber { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PassengerTypeInfo
        {
            public int State { get; set; }
            public bool StateSpecified { get; set; }
            public DateTime DOB { get; set; }
            public bool DOBSpecified { get; set; }
            public string PaxType { get; set; }
        }

        public class PassengerTypeInfo2
        {
            public int State { get; set; }
            public bool StateSpecified { get; set; }
            public DateTime DOB { get; set; }
            public bool DOBSpecified { get; set; }
            public string PaxType { get; set; }
        }

        public class PaxFareRT
        {
            public string PaxType { get; set; }
            public string PaxDiscountCode { get; set; }
            public string FareDiscountCode { get; set; }
            public List<ServiceCharge> ServiceCharges { get; set; }
            public string TicketFareBasisCode { get; set; }
            public bool ProofOfStatusRequired { get; set; }
            public bool ProofOfStatusRequiredSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PaxSeat
        {
            public int PassengerNumber { get; set; }
            public bool PassengerNumberSpecified { get; set; }
            public string ArrivalStation { get; set; }
            public string DepartureStation { get; set; }
            public string UnitDesignator { get; set; }
            public string CompartmentDesignator { get; set; }
            public int SeatPreference { get; set; }
            public bool SeatPreferenceSpecified { get; set; }
            public int Penalty { get; set; }
            public bool PenaltySpecified { get; set; }
            public bool SeatTogetherPreference { get; set; }
            public bool SeatTogetherPreferenceSpecified { get; set; }
            public PaxSeatInfo PaxSeatInfo { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PaxSeatInfo
        {
            public int SeatSet { get; set; }
            public bool SeatSetSpecified { get; set; }
            public int Deck { get; set; }
            public bool DeckSpecified { get; set; }
            public List<Property> Properties { get; set; }
        }

        public class PaxSegment
        {
            public string BoardingSequence { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool CreatedDateSpecified { get; set; }
            public int LiftStatus { get; set; }
            public bool LiftStatusSpecified { get; set; }
            public string OverBookIndicator { get; set; }
            public int PassengerNumber { get; set; }
            public bool PassengerNumberSpecified { get; set; }
            public DateTime PriorityDate { get; set; }
            public bool PriorityDateSpecified { get; set; }
            public int TripType { get; set; }
            public bool TripTypeSpecified { get; set; }
            public bool TimeChanged { get; set; }
            public bool TimeChangedSpecified { get; set; }
            public POS POS { get; set; }
            public SourcePOS SourcePOS { get; set; }
            public string VerifiedTravelDocs { get; set; }
            public DateTime ModifiedDate { get; set; }
            public bool ModifiedDateSpecified { get; set; }
            public DateTime ActivityDate { get; set; }
            public bool ActivityDateSpecified { get; set; }
            public int BaggageAllowanceWeight { get; set; }
            public bool BaggageAllowanceWeightSpecified { get; set; }
            public int BaggageAllowanceWeightType { get; set; }
            public bool BaggageAllowanceWeightTypeSpecified { get; set; }
            public bool BaggageAllowanceUsed { get; set; }
            public bool BaggageAllowanceUsedSpecified { get; set; }
            public int ReferenceNumber { get; set; }
            public bool ReferenceNumberSpecified { get; set; }
            public object BoardingPassDetail { get; set; }
            public string ServiceBundleCode { get; set; }
            public int BaggageGroupNumber { get; set; }
            public bool BaggageGroupNumberSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class PointOfSale
        {
            public string AgentCode { get; set; }
            public string OrganizationCode { get; set; }
            public string DomainCode { get; set; }
            public string LocationCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class POS
        {
            public string AgentCode { get; set; }
            public string OrganizationCode { get; set; }
            public string DomainCode { get; set; }
            public string LocationCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class Property
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class ReceivedBy
        {
            public string ReceivedByRT { get; set; }
            public string ReceivedReference { get; set; }
            public string ReferralCode { get; set; }
            public string LatestReceivedBy { get; set; }
            public string LatestReceivedReference { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class RecordLocator
        {
            public string SystemDomainCode { get; set; }
            public string SystemCode { get; set; }
            public string RecordCode { get; set; }
            public string InteractionPurpose { get; set; }
            public string HostedCarrierCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }        

        public class SegmentRT
        {
            public string ActionStatusCode { get; set; }
            public string ArrivalStation { get; set; }
            public string CabinOfService { get; set; }
            public string ChangeReasonCode { get; set; }
            public string DepartureStation { get; set; }
            public string PriorityCode { get; set; }
            public string SegmentType { get; set; }
            public DateTime STA { get; set; }
            public bool STASpecified { get; set; }
            public DateTime STD { get; set; }
            public bool STDSpecified { get; set; }
            public bool International { get; set; }
            public bool InternationalSpecified { get; set; }
            public FlightDesignator FlightDesignator { get; set; }
            public object XrefFlightDesignator { get; set; }
            public List<Fare> Fares { get; set; }
            public List<Leg> Legs { get; set; }
            public List<object> PaxBags { get; set; }
            public List<PaxSeat> PaxSeats { get; set; }
            public List<object> PaxSSRs { get; set; }
            public List<PaxSegment> PaxSegments { get; set; }
            public List<object> PaxTickets { get; set; }
            public object PaxSeatPreferences { get; set; }
            public DateTime SalesDate { get; set; }
            public bool SalesDateSpecified { get; set; }
            public string SegmentSellKey { get; set; }
            public List<object> PaxScores { get; set; }
            public int ChannelType { get; set; }
            public bool ChannelTypeSpecified { get; set; }
            public string AvailabilitySourceCode { get; set; }
            public string InventorySourceCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class ServiceCharge
        {
            public int ChargeType { get; set; }
            public bool ChargeTypeSpecified { get; set; }
            public int CollectType { get; set; }
            public bool CollectTypeSpecified { get; set; }
            public string ChargeCode { get; set; }
            public string TicketCode { get; set; }
            public string CurrencyCode { get; set; }
            public int Amount { get; set; }
            public bool AmountSpecified { get; set; }
            public string ChargeDetail { get; set; }
            public string ForeignCurrencyCode { get; set; }
            public int ForeignAmount { get; set; }
            public bool ForeignAmountSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class SourceBookingPOS
        {
            public string AgentCode { get; set; }
            public string OrganizationCode { get; set; }
            public string DomainCode { get; set; }
            public string LocationCode { get; set; }
            public string ISOCountryCode { get; set; }
            public string SourceSystemCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class SourcePOS
        {
            public string AgentCode { get; set; }
            public string OrganizationCode { get; set; }
            public string DomainCode { get; set; }
            public string LocationCode { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }

        public class TypeOfSale
        {
            public string PaxResidentCountry { get; set; }
            public string PromotionCode { get; set; }
            public List<object> FareTypes { get; set; }
            public bool PromotionSold { get; set; }
            public bool PromotionSoldSpecified { get; set; }
            public int State { get; set; }
            public bool StateSpecified { get; set; }
        }
    }
}
