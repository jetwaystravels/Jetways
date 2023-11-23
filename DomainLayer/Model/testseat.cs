//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DomainLayer.Model
//{
//    public class testseat
//    {

//        public class Rootobject
//        {
//            public Datum[] data { get; set; }
//        }

//        public class Datum
//        {
//            public Seatmap seatMap { get; set; }
//            public Fees fees { get; set; }
//            public object ssrLookup { get; set; }
//        }

//        public class Seatmap
//        {
//            public string name { get; set; }
//            public string arrivalStation { get; set; }
//            public string departureStation { get; set; }
//            public object marketingCode { get; set; }
//            public string equipmentType { get; set; }
//            public string equipmentTypeSuffix { get; set; }
//            public int category { get; set; }
//            public int availableUnits { get; set; }
//            public Decks decks { get; set; }
//            public string seatmapReference { get; set; }
//        }

//        public class Decks
//        {
//            public _1 _1 { get; set; }
//        }

//        public class _1
//        {
//            public int number { get; set; }
//            public Compartments compartments { get; set; }
//        }

//        public class Compartments
//        {
//            public Y Y { get; set; }
//        }

//        public class Y
//        {
//            public int availableUnits { get; set; }
//            public string designator { get; set; }
//            public int length { get; set; }
//            public int width { get; set; }
//            public int sequence { get; set; }
//            public int orientation { get; set; }
//            public Unit[] units { get; set; }
//        }

//        public class Unit
//        {
//            public string unitKey { get; set; }
//            public bool assignable { get; set; }
//            public int availability { get; set; }
//            public string compartmentDesignator { get; set; }
//            public string designator { get; set; }
//            public int type { get; set; }
//            public string travelClassCode { get; set; }
//            public int set { get; set; }
//            public int group { get; set; }
//            public int priority { get; set; }
//            public string text { get; set; }
//            public int setVacancy { get; set; }
//            public int angle { get; set; }
//            public int width { get; set; }
//            public int height { get; set; }
//            public int zone { get; set; }
//            public int x { get; set; }
//            public int y { get; set; }
//            public object[] allowedSsrs { get; set; }
//            public Property1[] properties { get; set; }
//        }

//        public class Property1
//        {
//            public string code { get; set; }
//            public string value { get; set; }
//        }

//        public class Fees
//        {
//            public MCFBRFQ MCFBRFQ { get; set; }
//        }

//        public class MCFBRFQ
//        {
//            public string passengerKey { get; set; }
//            public Groups groups { get; set; }
//        }

//        public class Groups
//        {
//            public _11 _1 { get; set; }
//            public _2 _2 { get; set; }
//            public _3 _3 { get; set; }
//            public _4 _4 { get; set; }
//            public _5 _5 { get; set; }
//            public _6 _6 { get; set; }
//            public _7 _7 { get; set; }
//            public _8 _8 { get; set; }
//            public _9 _9 { get; set; }
//            public _10 _10 { get; set; }
//            public _111 _11 { get; set; }
//            public _12 _12 { get; set; }
//            public _13 _13 { get; set; }
//            public _14 _14 { get; set; }
//            public _15 _15 { get; set; }
//            public _16 _16 { get; set; }
//            public _17 _17 { get; set; }
//            public _18 _18 { get; set; }
//            public _19 _19 { get; set; }
//            public _20 _20 { get; set; }
//            public _21 _21 { get; set; }
//            public _22 _22 { get; set; }
//            public _23 _23 { get; set; }
//            public _24 _24 { get; set; }
//            public _25 _25 { get; set; }
//            public _26 _26 { get; set; }
//            public _27 _27 { get; set; }
//            public _28 _28 { get; set; }
//            public _29 _29 { get; set; }
//            public _30 _30 { get; set; }
//            public _31 _31 { get; set; }
//            public _32 _32 { get; set; }
//        }

//        public class _11
//        {
//            public int group { get; set; }
//            public Fee[] fees { get; set; }
//        }

//        public class Fee
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge[] serviceCharges { get; set; }
//        }

//        public class Servicecharge
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _2
//        {
//            public int group { get; set; }
//            public Fee1[] fees { get; set; }
//        }

//        public class Fee1
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge1[] serviceCharges { get; set; }
//        }

//        public class Servicecharge1
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _3
//        {
//            public int group { get; set; }
//            public Fee2[] fees { get; set; }
//        }

//        public class Fee2
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge2[] serviceCharges { get; set; }
//        }

//        public class Servicecharge2
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _4
//        {
//            public int group { get; set; }
//            public Fee3[] fees { get; set; }
//        }

//        public class Fee3
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge3[] serviceCharges { get; set; }
//        }

//        public class Servicecharge3
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _5
//        {
//            public int group { get; set; }
//            public Fee4[] fees { get; set; }
//        }

//        public class Fee4
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge4[] serviceCharges { get; set; }
//        }

//        public class Servicecharge4
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _6
//        {
//            public int group { get; set; }
//            public Fee5[] fees { get; set; }
//        }

//        public class Fee5
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge5[] serviceCharges { get; set; }
//        }

//        public class Servicecharge5
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _7
//        {
//            public int group { get; set; }
//            public Fee6[] fees { get; set; }
//        }

//        public class Fee6
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge6[] serviceCharges { get; set; }
//        }

//        public class Servicecharge6
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _8
//        {
//            public int group { get; set; }
//            public Fee7[] fees { get; set; }
//        }

//        public class Fee7
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge7[] serviceCharges { get; set; }
//        }

//        public class Servicecharge7
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _9
//        {
//            public int group { get; set; }
//            public Fee8[] fees { get; set; }
//        }

//        public class Fee8
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge8[] serviceCharges { get; set; }
//        }

//        public class Servicecharge8
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _10
//        {
//            public int group { get; set; }
//            public Fee9[] fees { get; set; }
//        }

//        public class Fee9
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge9[] serviceCharges { get; set; }
//        }

//        public class Servicecharge9
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _111
//        {
//            public int group { get; set; }
//            public Fee10[] fees { get; set; }
//        }

//        public class Fee10
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge10[] serviceCharges { get; set; }
//        }

//        public class Servicecharge10
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _12
//        {
//            public int group { get; set; }
//            public Fee11[] fees { get; set; }
//        }

//        public class Fee11
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge11[] serviceCharges { get; set; }
//        }

//        public class Servicecharge11
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _13
//        {
//            public int group { get; set; }
//            public Fee12[] fees { get; set; }
//        }

//        public class Fee12
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge12[] serviceCharges { get; set; }
//        }

//        public class Servicecharge12
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _14
//        {
//            public int group { get; set; }
//            public Fee13[] fees { get; set; }
//        }

//        public class Fee13
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge13[] serviceCharges { get; set; }
//        }

//        public class Servicecharge13
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _15
//        {
//            public int group { get; set; }
//            public Fee14[] fees { get; set; }
//        }

//        public class Fee14
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge14[] serviceCharges { get; set; }
//        }

//        public class Servicecharge14
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _16
//        {
//            public int group { get; set; }
//            public Fee15[] fees { get; set; }
//        }

//        public class Fee15
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge15[] serviceCharges { get; set; }
//        }

//        public class Servicecharge15
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _17
//        {
//            public int group { get; set; }
//            public Fee16[] fees { get; set; }
//        }

//        public class Fee16
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge16[] serviceCharges { get; set; }
//        }

//        public class Servicecharge16
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _18
//        {
//            public int group { get; set; }
//            public Fee17[] fees { get; set; }
//        }

//        public class Fee17
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge17[] serviceCharges { get; set; }
//        }

//        public class Servicecharge17
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _19
//        {
//            public int group { get; set; }
//            public Fee18[] fees { get; set; }
//        }

//        public class Fee18
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge18[] serviceCharges { get; set; }
//        }

//        public class Servicecharge18
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _20
//        {
//            public int group { get; set; }
//            public Fee19[] fees { get; set; }
//        }

//        public class Fee19
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge19[] serviceCharges { get; set; }
//        }

//        public class Servicecharge19
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _21
//        {
//            public int group { get; set; }
//            public Fee20[] fees { get; set; }
//        }

//        public class Fee20
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge20[] serviceCharges { get; set; }
//        }

//        public class Servicecharge20
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _22
//        {
//            public int group { get; set; }
//            public Fee21[] fees { get; set; }
//        }

//        public class Fee21
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge21[] serviceCharges { get; set; }
//        }

//        public class Servicecharge21
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _23
//        {
//            public int group { get; set; }
//            public Fee22[] fees { get; set; }
//        }

//        public class Fee22
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge22[] serviceCharges { get; set; }
//        }

//        public class Servicecharge22
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _24
//        {
//            public int group { get; set; }
//            public Fee23[] fees { get; set; }
//        }

//        public class Fee23
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge23[] serviceCharges { get; set; }
//        }

//        public class Servicecharge23
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _25
//        {
//            public int group { get; set; }
//            public Fee24[] fees { get; set; }
//        }

//        public class Fee24
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge24[] serviceCharges { get; set; }
//        }

//        public class Servicecharge24
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _26
//        {
//            public int group { get; set; }
//            public Fee25[] fees { get; set; }
//        }

//        public class Fee25
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge25[] serviceCharges { get; set; }
//        }

//        public class Servicecharge25
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _27
//        {
//            public int group { get; set; }
//            public Fee26[] fees { get; set; }
//        }

//        public class Fee26
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge26[] serviceCharges { get; set; }
//        }

//        public class Servicecharge26
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _28
//        {
//            public int group { get; set; }
//            public Fee27[] fees { get; set; }
//        }

//        public class Fee27
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge27[] serviceCharges { get; set; }
//        }

//        public class Servicecharge27
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _29
//        {
//            public int group { get; set; }
//            public Fee28[] fees { get; set; }
//        }

//        public class Fee28
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge28[] serviceCharges { get; set; }
//        }

//        public class Servicecharge28
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _30
//        {
//            public int group { get; set; }
//            public Fee29[] fees { get; set; }
//        }

//        public class Fee29
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge29[] serviceCharges { get; set; }
//        }

//        public class Servicecharge29
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _31
//        {
//            public int group { get; set; }
//            public Fee30[] fees { get; set; }
//        }

//        public class Fee30
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge30[] serviceCharges { get; set; }
//        }

//        public class Servicecharge30
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//        public class _32
//        {
//            public int group { get; set; }
//            public Fee31[] fees { get; set; }
//        }

//        public class Fee31
//        {
//            public int type { get; set; }
//            public object ssrCode { get; set; }
//            public int ssrNumber { get; set; }
//            public int paymentNumber { get; set; }
//            public bool isConfirmed { get; set; }
//            public bool isConfirming { get; set; }
//            public bool isConfirmingExternal { get; set; }
//            public string code { get; set; }
//            public object detail { get; set; }
//            public string passengerFeeKey { get; set; }
//            public bool _override { get; set; }
//            public string flightReference { get; set; }
//            public object note { get; set; }
//            public DateTime createdDate { get; set; }
//            public bool isProtected { get; set; }
//            public Servicecharge31[] serviceCharges { get; set; }
//        }

//        public class Servicecharge31
//        {
//            public int amount { get; set; }
//            public string code { get; set; }
//            public string detail { get; set; }
//            public int type { get; set; }
//            public int collectType { get; set; }
//            public string currencyCode { get; set; }
//            public string foreignCurrencyCode { get; set; }
//            public int foreignAmount { get; set; }
//            public string ticketCode { get; set; }
//        }

//    }
//}
