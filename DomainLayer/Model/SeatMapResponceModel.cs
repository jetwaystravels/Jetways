using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Model.SeatMapResponceModel;

namespace DomainLayer.Model
{
    public class SeatMapResponceModel
    {

        public List<data> datalist { get; set; }
        public class data
        {
            public Seatmap seatMap { get; set; }
            public Fees seatMapfees { get; set; }
            public object ssrLookup { get; set; }

        }
       

        public class Seatmap
        {
            public string name { get; set; }
            public string arrivalStation { get; set; }
            public string departureStation { get; set; }
            public object marketingCode { get; set; }
            public string equipmentType { get; set; }
            public string equipmentTypeSuffix { get; set; }
            public int category { get; set; }
            public int availableUnits { get; set; }
            public string seatmapReference { get; set; }
            public Decks decks { get; set; }
            
        }
        public class Decks
        {
            public int availableUnits { get; set; }
            public string designator { get; set; }
            public int length { get; set; }
            public int width { get; set; }
            public int sequence { get; set; }
            public int orientation { get; set; }
            public List<Unit> units { get; set; }
        }

        public class Unit
        {
            public string unitKey { get; set; }
            public bool assignable { get; set; }
            public int availability { get; set; }
            public string compartmentDesignator { get; set; }
            public string designator { get; set; }
            public int type { get; set; }
            public string travelClassCode { get; set; }
            public int set { get; set; }
            public int group { get; set; }
            public int priority { get; set; }
            public string text { get; set; }
            public int setVacancy { get; set; }
            public int angle { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int zone { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            //  public object[] allowedSsrs { get; set; }
             public List<Properties> properties { get; set; }
            public decimal servicechargefeeAmount { get; set; }
        }

        public class Properties
        {
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Fees
        {
            public string passengerKey { get; set; }
            public List<Groups> groups { get; set; }
        }

        public class Groups
        {
            public GroupsFee groupsFee { get; set; }

        }


        public class GroupsFee
        {
            public int groupid { get; set; }
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
            public List<Servicecharge> serviceCharges { get; set; }
        }

        public class Servicecharge
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

        
    }
}
