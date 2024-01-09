using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using static DomainLayer.Model.SeatMapResponceModel;
namespace OnionConsumeWebAPI.Controllers
{
    public class ReturnTripsellController : Controller
    {

        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string passengerkey12 = string.Empty;
        public async Task<IActionResult> ReturnTripsellView(List<string> fareKey, List<string> journeyKey)
        {
            string token = string.Empty;
            List<_credentials> credentialslist = new List<_credentials>();
            using (HttpClient client = new HttpClient())
            {

                string tokenview = HttpContext.Session.GetString("AirasiaTokan");

                if (tokenview == "" || tokenview == null)
                {
                    return RedirectToAction("Index");
                }
                token = tokenview.Replace(@"""", string.Empty);

                HttpContext.Session.SetString("journeyKey", JsonConvert.SerializeObject(journeyKey));
                string Leftshowpopupdata = HttpContext.Session.GetString("PassengerModel");
                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = null;
                _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(Leftshowpopupdata);
                var AdtType = "";
                var AdtCount = 0;

                var chdtype = "";
                var chdcount = 0;

                var infanttype = "";
                var infantcount = 0;

                int countpassenger = _SimpleAvailabilityobj.passengers.types.Count;
                AdtType = _SimpleAvailabilityobj.passengers.types[0].type;
                AirAsiaTripSellRequest AirAsiaTripSellRequestobj = new AirAsiaTripSellRequest();
                _Key key = new _Key();
                List<_Key> _keylist = new List<_Key>();


                string JourneyKeyOneway = journeyKey[0];
                string[] Jparts = JourneyKeyOneway.Split('/');
                string _JourneykeyData = Jparts[0];
                key.journeyKey = _JourneykeyData;
                string fareKeyKeyOneway = fareKey[0];
                string[] Fparts = fareKeyKeyOneway.Split('/');
                string _FareKeyData = Fparts[0];
                key.fareAvailabilityKey = _FareKeyData;
                _keylist.Add(key);
                AirAsiaTripSellRequestobj.keys = _keylist;

                Passengers passengers = new Passengers();
                List<_Types> _typeslist = new List<_Types>();

                for (int i = 0; i < _SimpleAvailabilityobj.passengers.types.Count; i++)
                {
                    _Types _Types = new _Types();

                    if (_SimpleAvailabilityobj.passengers.types[i].type == "ADT")
                    {
                        AdtType = _SimpleAvailabilityobj.passengers.types[i].type;
                        _Types.type = AdtType;
                        _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                    }
                    else if (_SimpleAvailabilityobj.passengers.types[i].type == "CHD")
                    {
                        chdtype = _SimpleAvailabilityobj.passengers.types[i].type;
                        _Types.type = chdtype;
                        _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                    }
                    else if (_SimpleAvailabilityobj.passengers.types[i].type == "INFT")
                    {
                        infanttype = _SimpleAvailabilityobj.passengers.types[i].type;
                        continue;
                    }
                    //	
                    _typeslist.Add(_Types);
                }
                passengers.types = _typeslist;
                AirAsiaTripSellRequestobj.passengers = passengers;
                AirAsiaTripSellRequestobj.currencyCode = "INR";
                AirAsiaTripSellRequestobj.preventOverlap = true;
                AirAsiaTripSellRequestobj.suppressPassengerAgeValidation = true;
                var AirasiaTripSellRequest = JsonConvert.SerializeObject(AirAsiaTripSellRequestobj, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseTripsell = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v4/trip/sell", AirAsiaTripSellRequestobj);
                if (responseTripsell.IsSuccessStatusCode)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                    var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                    var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                    //var totalAmount = JsonObjTripsell.data.breakdown.journeys[journeyKey[0]].totalAmount;
                    //var totalTax = JsonObjTripsell.data.breakdown.journeys[journeyKey[0]].totalTax;
                    #region Comment
                    int journeyscount = JsonObjTripsell.data.journeys.Count;
                    List<AAJourney> AAJourneyList = new List<AAJourney>();
                    for (int i = 0; i < journeyscount; i++)
                    {
                        AAJourney AAJourneyobj = new AAJourney();

                        AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                        AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                        AAJourneyobj.journeyKey = JsonObjTripsell.data.journeys[i].journeyKey;

                        AADesignator AADesignatorobj = new AADesignator();
                        AADesignatorobj.origin = JsonObjTripsell.data.journeys[0].designator.origin;
                        AADesignatorobj.destination = JsonObjTripsell.data.journeys[0].designator.destination;
                        AADesignatorobj.departure = JsonObjTripsell.data.journeys[0].designator.departure;
                        AADesignatorobj.arrival = JsonObjTripsell.data.journeys[0].designator.arrival;
                        AAJourneyobj.designator = AADesignatorobj;


                        int segmentscount = JsonObjTripsell.data.journeys[i].segments.Count;
                        List<AASegment> AASegmentlist = new List<AASegment>();
                        for (int j = 0; j < segmentscount; j++)
                        {
                            AASegment AASegmentobj = new AASegment();
                            AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                            AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                            AADesignator AASegmentDesignatorobj = new AADesignator();
                            var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
                            AASegmentDesignatorobj.origin = JsonObjTripsell.data.journeys[i].segments[j].designator.origin;
                            AASegmentDesignatorobj.destination = JsonObjTripsell.data.journeys[i].segments[j].designator.destination;
                            AASegmentDesignatorobj.departure = JsonObjTripsell.data.journeys[i].segments[j].designator.departure;
                            AASegmentDesignatorobj.arrival = JsonObjTripsell.data.journeys[i].segments[j].designator.arrival;
                            AASegmentobj.designator = AASegmentDesignatorobj;

                            int fareCount = JsonObjTripsell.data.journeys[i].segments[j].fares.Count;
                            List<AAFare> AAFarelist = new List<AAFare>();
                            for (int k = 0; k < fareCount; k++)
                            {
                                AAFare AAFareobj = new AAFare();
                                AAFareobj.fareKey = JsonObjTripsell.data.journeys[i].segments[j].fares[k].fareKey;
                                AAFareobj.productClass = JsonObjTripsell.data.journeys[i].segments[j].fares[k].productClass;

                                var passengerFares = JsonObjTripsell.data.journeys[i].segments[j].fares[k].passengerFares;

                                int passengerFarescount = ((Newtonsoft.Json.Linq.JContainer)passengerFares).Count;
                                List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                                for (int l = 0; l < passengerFarescount; l++)
                                {
                                    AAPassengerfare AAPassengerfareobj = new AAPassengerfare();
                                    AAPassengerfareobj.passengerType = JsonObjTripsell.data.journeys[i].segments[j].fares[k].passengerFares[l].passengerType;

                                    var serviceCharges1 = JsonObjTripsell.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges;
                                    int serviceChargescount = ((Newtonsoft.Json.Linq.JContainer)serviceCharges1).Count;
                                    List<AAServicecharge> AAServicechargelist = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescount; m++)
                                    {
                                        AAServicecharge AAServicechargeobj = new AAServicecharge();

                                        AAServicechargeobj.amount = JsonObjTripsell.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;


                                        AAServicechargelist.Add(AAServicechargeobj);
                                    }



                                    AAPassengerfareobj.serviceCharges = AAServicechargelist;

                                    AAPassengerfarelist.Add(AAPassengerfareobj);

                                }
                                AAFareobj.passengerFares = AAPassengerfarelist;

                                AAFarelist.Add(AAFareobj);




                            }
                            AASegmentobj.fares = AAFarelist;
                            AAIdentifier AAIdentifierobj = new AAIdentifier();

                            AAIdentifierobj.identifier = JsonObjTripsell.data.journeys[i].segments[j].identifier.identifier;
                            AAIdentifierobj.carrierCode = JsonObjTripsell.data.journeys[i].segments[j].identifier.carrierCode;

                            AASegmentobj.identifier = AAIdentifierobj;

                            var leg = JsonObjTripsell.data.journeys[i].segments[j].legs;
                            int legcount = ((Newtonsoft.Json.Linq.JContainer)leg).Count;
                            List<AALeg> AALeglist = new List<AALeg>();
                            for (int n = 0; n < legcount; n++)
                            {
                                AALeg AALeg = new AALeg();
                                AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                AADesignator AAlegDesignatorobj = new AADesignator();
                                AAlegDesignatorobj.origin = JsonObjTripsell.data.journeys[i].segments[j].legs[n].designator.origin;
                                AAlegDesignatorobj.destination = JsonObjTripsell.data.journeys[i].segments[j].legs[n].designator.destination;
                                AAlegDesignatorobj.departure = JsonObjTripsell.data.journeys[i].segments[j].legs[n].designator.departure;
                                AAlegDesignatorobj.arrival = JsonObjTripsell.data.journeys[i].segments[j].legs[n].designator.arrival;
                                AALeg.designator = AAlegDesignatorobj;

                                AALeginfo AALeginfoobj = new AALeginfo();
                                AALeginfoobj.arrivalTerminal = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                AALeginfoobj.arrivalTime = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                AALeginfoobj.departureTerminal = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                AALeginfoobj.departureTime = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                AALeg.legInfo = AALeginfoobj;
                                AALeglist.Add(AALeg);

                            }

                            AASegmentobj.legs = AALeglist;
                            AASegmentlist.Add(AASegmentobj);
                        }

                        AAJourneyobj.segments = AASegmentlist;
                        AAJourneyList.Add(AAJourneyobj);

                    }
                    var passanger = JsonObjTripsell.data.passengers;
                    int passengercount = ((Newtonsoft.Json.Linq.JContainer)passanger).Count;
                    List<AAPassengers> passkeylist = new List<AAPassengers>();
                    foreach (var items in JsonObjTripsell.data.passengers)
                    {
                        AAPassengers passkeytypeobj = new AAPassengers();
                        passkeytypeobj.passengerKey = items.Value.passengerKey;
                        passkeytypeobj.passengerTypeCode = items.Value.passengerTypeCode;
                        //  passkeytypeobj.passengertypecount = items.Count;

                        passkeylist.Add(passkeytypeobj);
                        passengerkey12 = passkeytypeobj.passengerKey;
                    }
                    AirAsiaTripResponceobj.journeys = AAJourneyList;
                    AirAsiaTripResponceobj.passengers = passkeylist;
                    AirAsiaTripResponceobj.passengerscount = passengercount;
                    HttpContext.Session.SetString("keypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));
                    #endregion
                }
                #region Itenary 
                if (infanttype != null)
                {
                    string passengerdatainfant = HttpContext.Session.GetString("keypassenger");
                    AirAsiaTripResponceModel passeengerKeyListinfant = null;
                    passeengerKeyListinfant = JsonConvert.DeserializeObject<AirAsiaTripResponceModel>(passengerdatainfant);
                    string LeftpassengerModal = HttpContext.Session.GetString("PassengerModel");
                    SimpleAvailabilityRequestModel _SimpleAvailabilityobject = null;
                    _SimpleAvailabilityobject = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(LeftpassengerModal);

                    GetItenaryModel itenaryInfant = new GetItenaryModel();
                    List<Ssr1> ssr1slist = new List<Ssr1>();
                    Ssr1 ssr1 = new Ssr1();
                    Market marketobj = new Market();
                    Identifier1 identifier1 = new Identifier1();
                    //Market
                    identifier1.identifier = passeengerKeyListinfant.journeys[0].segments[0].identifier.identifier;
                    identifier1.carrierCode = passeengerKeyListinfant.journeys[0].segments[0].identifier.carrierCode;
                    marketobj.identifier = identifier1;
                    marketobj.destination = passeengerKeyListinfant.journeys[0].segments[0].designator.destination;
                    marketobj.origin = passeengerKeyListinfant.journeys[0].segments[0].designator.origin;
                    //marketobj.departureDate = passeengerKeyListinfant.journeys[0].segments[0].designator.departure.ToString("yyyy-MM-dd");
                    marketobj.departureDate = _SimpleAvailabilityobject.beginDate;
                    ssr1.market = marketobj;
                    ssr1slist.Add(ssr1);
                    //item
                    List<Item> itemList = new List<Item>();
                    //int passtypedatacount = passeengerKeyListinfant.passengers.Count;	
                    int typecount = _SimpleAvailabilityobject.passengers.types.Count;

                    for (int i = 0; i < typecount; i++)
                    {
                        var infant = _SimpleAvailabilityobject.passengers.types[i].type;
                        if (infant == "INFT")
                        {
                            int infantCount1 = _SimpleAvailabilityobject.passengers.types[i].count;

                            for (int j = 0; j < infantCount1; j++)
                            {

                                Item itemobj = new Item();
                                List<SsrItem> ssrItemslist = new List<SsrItem>();
                                SsrItem ssrItemobj = new SsrItem();

                                ssrItemobj.ssrCode = "INFT";
                                ssrItemobj.count = 1;
                                ssrItemslist.Add(ssrItemobj);

                                Designatorr designatorr = new Designatorr();
                                designatorr.destination = passeengerKeyListinfant.journeys[0].segments[0].designator.destination;
                                designatorr.origin = passeengerKeyListinfant.journeys[0].segments[0].designator.origin;
                                //designatorr.departureDate = passeengerKeyListinfant.journeys[0].segments[0].designator.departure;
                                designatorr.departureDate = _SimpleAvailabilityobject.beginDate;
                                ssrItemobj.designator = designatorr;
                                //ssrItemslist.Add(ssrItemobj);
                                itemobj.passengerType = passeengerKeyListinfant.passengers[0].passengerTypeCode;
                                itemobj.ssrs = ssrItemslist;
                                itemList.Add(itemobj);
                            }
                        }
                    }
                    //      int infantCount = _SimpleAvailabilityobject.passengers.types[1].count;			

                    ssr1.items = itemList;
                    itenaryInfant.ssrs = ssr1slist;
                    List<Key> keylist = new List<Key>();
                    Key Keyobj = new Key();
                    Keyobj.journeyKey = journeyKey[0];
                    Keyobj.fareAvailabilityKey = fareKey[0];
                    Keyobj.standbyPriorityCode = "";
                    Keyobj.inventoryControl = "HoldSpace";
                    keylist.Add(Keyobj);
                    itenaryInfant.keys = keylist;
                    Passengers1 passengers1 = new Passengers1();
                    passengers1.residentCountry = "IN";
                    List<Type2> typelist = new List<Type2>();
                    for (int i = 0; i < _SimpleAvailabilityobj.passengers.types.Count; i++)
                    {
                        Type2 _Types = new Type2();

                        if (_SimpleAvailabilityobj.passengers.types[i].type == "ADT")
                        {
                            AdtType = _SimpleAvailabilityobj.passengers.types[i].type;
                            _Types.type = AdtType;
                            _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                        }
                        else if (_SimpleAvailabilityobj.passengers.types[i].type == "CHD")
                        {
                            chdtype = _SimpleAvailabilityobj.passengers.types[i].type;
                            _Types.type = chdtype;
                            _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                        }
                        else if (_SimpleAvailabilityobj.passengers.types[i].type == "INFT")
                        {
                            infanttype = _SimpleAvailabilityobj.passengers.types[i].type;
                            continue;
                        }
                        //	
                        typelist.Add(_Types);
                    }
                    passengers1.types = typelist;
                    itenaryInfant.passengers = passengers1;
                    itenaryInfant.currencyCode = "INR";

                    var jsonPassengers = JsonConvert.SerializeObject(itenaryInfant, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/bookings/quote", itenaryInfant);
                    if (responsePassengers.IsSuccessStatusCode)
                    {
                        AirAsiaTripResponceModel AirAsiaTripResponceobject = new AirAsiaTripResponceModel();
                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                        var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                        var TotalAmount = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalAmount;
                        var TotalTax = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalTax;
                        int Journeyscount = JsonObjPassengers.data.journeys.Count;
                        List<AAJourney> AAJourneyList = new List<AAJourney>();
                        for (int i = 0; i < Journeyscount; i++)
                        {
                            AAJourney AAJourneyobject = new AAJourney();

                            AAJourneyobject.flightType = JsonObjPassengers.data.journeys[i].flightType;
                            AAJourneyobject.stops = JsonObjPassengers.data.journeys[i].stops;
                            AAJourneyobject.journeyKey = JsonObjPassengers.data.journeys[i].journeyKey;

                            AADesignator AADesignatorobject = new AADesignator();
                            AADesignatorobject.origin = JsonObjPassengers.data.journeys[0].designator.origin;
                            AADesignatorobject.destination = JsonObjPassengers.data.journeys[0].designator.destination;
                            AADesignatorobject.departure = JsonObjPassengers.data.journeys[0].designator.departure;
                            AADesignatorobject.arrival = JsonObjPassengers.data.journeys[0].designator.arrival;
                            AAJourneyobject.designator = AADesignatorobject;


                            int Segmentscount = JsonObjPassengers.data.journeys[i].segments.Count;
                            List<AASegment> AASegmentlist = new List<AASegment>();
                            for (int j = 0; j < Segmentscount; j++)
                            {
                                AASegment AASegmentobject = new AASegment();
                                AASegmentobject.isStandby = JsonObjPassengers.data.journeys[i].segments[j].isStandby;
                                AASegmentobject.isHosted = JsonObjPassengers.data.journeys[i].segments[j].isHosted;

                                AADesignator AASegmentDesignatorobject = new AADesignator();

                                AASegmentDesignatorobject.origin = JsonObjPassengers.data.journeys[i].segments[j].designator.origin;
                                AASegmentDesignatorobject.destination = JsonObjPassengers.data.journeys[i].segments[j].designator.destination;
                                AASegmentDesignatorobject.departure = JsonObjPassengers.data.journeys[i].segments[j].designator.departure;
                                AASegmentDesignatorobject.arrival = JsonObjPassengers.data.journeys[i].segments[j].designator.arrival;
                                AASegmentobject.designator = AASegmentDesignatorobject;

                                int FareCount = JsonObjPassengers.data.journeys[i].segments[j].fares.Count;
                                List<AAFare> AAFareList = new List<AAFare>();
                                for (int k = 0; k < FareCount; k++)
                                {
                                    AAFare AAFareobject = new AAFare();
                                    AAFareobject.fareKey = JsonObjPassengers.data.journeys[i].segments[j].fares[k].fareKey;
                                    AAFareobject.productClass = JsonObjPassengers.data.journeys[i].segments[j].fares[k].productClass;

                                    var PassengerFares = JsonObjPassengers.data.journeys[i].segments[j].fares[k].passengerFares;

                                    int PassengerFarescount = ((Newtonsoft.Json.Linq.JContainer)PassengerFares).Count;
                                    List<AAPassengerfare> AAPassengerfareList = new List<AAPassengerfare>();
                                    for (int l = 0; l < PassengerFarescount; l++)
                                    {
                                        AAPassengerfare AAPassengerfareobject = new AAPassengerfare();
                                        AAPassengerfareobject.passengerType = JsonObjPassengers.data.journeys[i].segments[j].fares[k].passengerFares[l].passengerType;

                                        var ServiceCharges1 = JsonObjPassengers.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges;
                                        int ServiceChargescount = ((Newtonsoft.Json.Linq.JContainer)ServiceCharges1).Count;
                                        List<AAServicecharge> AAServicechargeList = new List<AAServicecharge>();
                                        for (int m = 0; m < ServiceChargescount; m++)
                                        {
                                            AAServicecharge AAServicechargeobject = new AAServicecharge();

                                            AAServicechargeobject.amount = JsonObjPassengers.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;


                                            AAServicechargeList.Add(AAServicechargeobject);
                                        }



                                        AAPassengerfareobject.serviceCharges = AAServicechargeList;

                                        AAPassengerfareList.Add(AAPassengerfareobject);

                                    }
                                    AAFareobject.passengerFares = AAPassengerfareList;

                                    AAFareList.Add(AAFareobject);




                                }
                                AASegmentobject.fares = AAFareList;
                                AAIdentifier AAIdentifierobj = new AAIdentifier();

                                AAIdentifierobj.identifier = JsonObjPassengers.data.journeys[i].segments[j].identifier.identifier;
                                AAIdentifierobj.carrierCode = JsonObjPassengers.data.journeys[i].segments[j].identifier.carrierCode;

                                AASegmentobject.identifier = AAIdentifierobj;

                                var Leg = JsonObjPassengers.data.journeys[i].segments[j].legs;
                                int Legcount = ((Newtonsoft.Json.Linq.JContainer)Leg).Count;
                                List<AALeg> AALeglist = new List<AALeg>();
                                for (int n = 0; n < Legcount; n++)
                                {
                                    AALeg AALegobj = new AALeg();
                                    AALegobj.legKey = JsonObjPassengers.data.journeys[i].segments[j].legs[n].legKey;
                                    AADesignator AAlegDesignatorobject = new AADesignator();
                                    AAlegDesignatorobject.origin = JsonObjPassengers.data.journeys[i].segments[j].legs[n].designator.origin;
                                    AAlegDesignatorobject.destination = JsonObjPassengers.data.journeys[i].segments[j].legs[n].designator.destination;
                                    AAlegDesignatorobject.departure = JsonObjPassengers.data.journeys[i].segments[j].legs[n].designator.departure;
                                    AAlegDesignatorobject.arrival = JsonObjPassengers.data.journeys[i].segments[j].legs[n].designator.arrival;
                                    AALegobj.designator = AAlegDesignatorobject;

                                    AALeginfo AALeginfoobject = new AALeginfo();
                                    AALeginfoobject.arrivalTerminal = JsonObjPassengers.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                    AALeginfoobject.arrivalTime = JsonObjPassengers.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                    AALeginfoobject.departureTerminal = JsonObjPassengers.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                    AALeginfoobject.departureTime = JsonObjPassengers.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                    AALegobj.legInfo = AALeginfoobject;
                                    AALeglist.Add(AALegobj);

                                }

                                AASegmentobject.legs = AALeglist;
                                AASegmentlist.Add(AASegmentobject);
                            }

                            AAJourneyobject.segments = AASegmentlist;
                            AAJourneyList.Add(AAJourneyobject);

                        }
                        var Passanger = JsonObjPassengers.data.passengers;
                        int passengercount = ((Newtonsoft.Json.Linq.JContainer)Passanger).Count;
                        List<AAPassengers> passkeyList = new List<AAPassengers>();
                        Infant infantobject = null;
                        Fee feeobject = null;
                        foreach (var items in JsonObjPassengers.data.passengers)
                        {
                            AAPassengers passkeytypeobject = new AAPassengers();
                            passkeytypeobject.passengerKey = items.Value.passengerKey;
                            passkeytypeobject.passengerTypeCode = items.Value.passengerTypeCode;
                            passkeyList.Add(passkeytypeobject);
                            passengerkey12 = passkeytypeobject.passengerKey;
                            //infant
                            if (passkeytypeobject.passengerTypeCode != "CHD")
                            {

                                if (JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant != null)
                                {
                                    int Feecount = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees.Count;
                                    List<Fee> feeList = new List<Fee>();
                                    for (int i = 0; i < Feecount; i++)
                                    {
                                        infantobject = new Infant();
                                        feeobject = new Fee();
                                        feeobject.isConfirmed = false;
                                        feeobject.isConfirming = false;
                                        feeobject.isConfirmingExternal = false;
                                        feeobject.code = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[i].code;
                                        feeobject._override = false;
                                        feeobject.note = "";
                                        feeobject.isProtected = false;
                                        infantobject.nationality = "";
                                        infantobject.dateOfBirth = "";
                                        infantobject.travelDocuments = "";
                                        infantobject.residentCountry = "";
                                        infantobject.gender = 1;
                                        infantobject.name = "";
                                        infantobject.type = "";
                                        feeList.Add(feeobject);

                                        infantobject.fees = feeList;
                                        passkeytypeobject.infant = infantobject;
                                    }
                                }
                            }


                            AirAsiaTripResponceobject.journeys = AAJourneyList;
                            AirAsiaTripResponceobject.passengers = passkeyList;
                            AirAsiaTripResponceobject.passengerscount = passengercount;
                            HttpContext.Session.SetString("keypassengerItanary", JsonConvert.SerializeObject(AirAsiaTripResponceobject));
                            //string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");

                        }
                    }
                }
                #endregion
                #region SeatMap
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage responseSeatmap = await client.PostAsJsonAsync(BaseURL + "api/nsk/v3/booking/seatmaps/journey/ {{journeyKey}}", _SSRAvailabilty);
                //  var REQUEST1= (BaseURL + "/api/nsk/v3/booking/seatmaps/journey/{{" +journeyKey1+ "}}?IncludePropertyLookup=true");
                HttpResponseMessage responseSeatmap = await client.GetAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/seatmaps/journey/" + key.journeyKey + "?IncludePropertyLookup=true");

                // data[0].seatMap.decks['1'].compartments.Y.units[0].unitKey
                if (responseSeatmap.IsSuccessStatusCode)
                {
                    var _responseSeatmap = responseSeatmap.Content.ReadAsStringAsync().Result;
                    var JsonObjSeatmap = JsonConvert.DeserializeObject<dynamic>(_responseSeatmap);
                    // var decks1 = "1";
                    //  x.data[0].seatMap.decks.1.compartments.Y.units[0].unitKey
                    var uniquekey1 = JsonObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.units[0].unitKey;
                    //  uniquekey = ((Newtonsoft.Json.Linq.JValue)uniquekey1).Value.ToString();
                    var data = JsonObjSeatmap.data.Count;

                    List<data> datalist = new List<data>();
                    for (int x = 0; x < data; x++)
                    {
                        data dataobj = new data();

                        SeatMapResponceModel SeatMapResponceModel = new SeatMapResponceModel();
                        List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();
                        Fees Fees = new Fees();
                        Seatmap Seatmapobj = new Seatmap();
                        Seatmapobj.name = JsonObjSeatmap.data[x].seatMap.name;
                        Seatmapobj.arrivalStation = JsonObjSeatmap.data[x].seatMap.arrivalStation;
                        Seatmapobj.departureStation = JsonObjSeatmap.data[x].seatMap.departureStation;
                        Seatmapobj.marketingCode = JsonObjSeatmap.data[x].seatMap.marketingCode;
                        Seatmapobj.equipmentType = JsonObjSeatmap.data[x].seatMap.equipmentType;
                        Seatmapobj.equipmentTypeSuffix = JsonObjSeatmap.data[x].seatMap.equipmentTypeSuffix;
                        Seatmapobj.category = JsonObjSeatmap.data[x].seatMap.category;
                        Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;
                        Decks Decksobj = new Decks();
                        Decksobj.availableUnits = JsonObjSeatmap.data[x].seatMap.availableUnits;
                        // Seatmap Seatmapobj = JsonObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.availableUnits;
                        Decksobj.designator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.designator;
                        Decksobj.length = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.length;
                        Decksobj.width = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.width;
                        Decksobj.sequence = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.sequence;
                        Decksobj.orientation = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.orientation;
                        Seatmapobj.decks = Decksobj;


                        //  int feesgroup1 = JsonObjSeatmap.data[0].fees[passengerdetails.passengerkey].groups.Count;

                        int compartmentsunitCount = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units.Count;
                        List<Unit> compartmentsunitlist = new List<Unit>();
                        for (int i = 0; i < compartmentsunitCount; i++)
                        {
                            Unit compartmentsunitobj = new Unit();
                            compartmentsunitobj.unitKey = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].unitKey;
                            compartmentsunitobj.assignable = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].assignable;
                            compartmentsunitobj.availability = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].availability;
                            compartmentsunitobj.compartmentDesignator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].compartmentDesignator;
                            compartmentsunitobj.designator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].designator;
                            compartmentsunitobj.type = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].type;
                            compartmentsunitobj.travelClassCode = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].travelClassCode;
                            compartmentsunitobj.set = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].set;
                            compartmentsunitobj.group = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].group;
                            compartmentsunitobj.priority = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].priority;
                            compartmentsunitobj.text = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].text;
                            compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                            compartmentsunitobj.angle = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].angle;
                            compartmentsunitobj.width = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].width;
                            compartmentsunitobj.height = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].height;
                            compartmentsunitobj.zone = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].zone;
                            compartmentsunitobj.x = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].x;
                            compartmentsunitobj.y = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].y;
                            compartmentsunitlist.Add(compartmentsunitobj);

                            int compartmentypropertiesCount = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].properties.Count;
                            List<Properties> Propertieslist = new List<Properties>();
                            for (int j = 0; j < compartmentypropertiesCount; j++)
                            {
                                Properties compartmentyproperties = new Properties();
                                compartmentyproperties.code = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].properties[j].code;
                                compartmentyproperties.value = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].properties[j].value;
                                Propertieslist.Add(compartmentyproperties);
                            }

                            //compartmentsunitlist.Add(compartmentsunitobj);
                            compartmentsunitobj.properties = Propertieslist;
                            Decksobj.units = compartmentsunitlist;
                        }

                        var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                        var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                        string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;


                        List<Groups> GroupsFeelist = new List<Groups>();
                        foreach (Match item in Regex.Matches(strText, @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                        {

                            Groups Groupsobj = new Groups();
                            int myString1 = Convert.ToInt32(item.Groups["key"].Value.Trim());
                            string myString = myString1.ToString();
                            // x.data[0].fees["MCFBRFQ-"].groups.1
                            var group = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString.ToString()];

                            //var fees = JsonObjSeatmap.data[0].fees["MCFBRFQ-"].groups[myString].fees;
                            //  Groups Groups = new Groups();

                            GroupsFee GroupsFeeobj = new GroupsFee();


                            GroupsFeeobj.type = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].type;
                            GroupsFeeobj.ssrCode = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].ssrCode;
                            GroupsFeeobj.ssrNumber = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].ssrNumber;
                            GroupsFeeobj.paymentNumber = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].paymentNumber;
                            GroupsFeeobj.isConfirmed = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].isConfirmed;
                            GroupsFeeobj.isConfirming = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].isConfirming;
                            GroupsFeeobj.isConfirmingExternal = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].isConfirmingExternal;
                            GroupsFeeobj.code = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].code;
                            GroupsFeeobj.detail = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].detail;
                            GroupsFeeobj.passengerFeeKey = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].passengerFeeKey;
                            //     Feegroupsobj._override = JsonObjSeatmap.data[0].fees[passengerdetails.passengerkey].groups[myString].fees[0]._override;
                            GroupsFeeobj.flightReference = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].flightReference;
                            GroupsFeeobj.note = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].note;
                            GroupsFeeobj.createdDate = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].createdDate;
                            GroupsFeeobj.isProtected = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].isProtected;
                            //Groups.groupsFee = GroupsFeeobj;
                            var feesgroupserviceChargescount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges.Count;

                            List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                            for (int l = 0; l < feesgroupserviceChargescount; l++)
                            {

                                Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                feesgroupserviceChargesobj.amount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].amount;
                                feesgroupserviceChargesobj.code = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].code;
                                feesgroupserviceChargesobj.detail = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].detail;
                                feesgroupserviceChargesobj.type = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].type;
                                feesgroupserviceChargesobj.collectType = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].collectType;
                                feesgroupserviceChargesobj.currencyCode = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].currencyCode;
                                feesgroupserviceChargesobj.amount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].amount;
                                feesgroupserviceChargesobj.foreignAmount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].foreignAmount;
                                feesgroupserviceChargesobj.ticketCode = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].serviceCharges[l].ticketCode;
                                feesgroupserviceChargeslist.Add(feesgroupserviceChargesobj);
                            }

                            GroupsFeeobj.serviceCharges = feesgroupserviceChargeslist;

                            Groupsobj.groupsFee = GroupsFeeobj;
                            //var data = from obj in Groupsobj
                            GroupsFeelist.Add(Groupsobj);

                            Fees.groups = GroupsFeelist;



                        }

                        dataobj.seatMap = Seatmapobj;
                        dataobj.seatMapfees = Fees;
                        datalist.Add(dataobj);
                        SeatMapResponceModel.datalist = datalist;
                        //SeatMapResponceModel.seatMapfees = Fees;
                        //SeatMapResponceModellist.Add(SeatMapResponceModel);




                        HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));
                        //TempData["Seatmap"] = JsonConvert.SerializeObject(SeatMapResponceModellist);
                    }
                }
                #endregion
                #region Meals

                string passengerdata = HttpContext.Session.GetString("keypassenger");

                AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerdata, typeof(AirAsiaTripResponceModel));
                int passengerscount = passeengerKeyList.passengerscount;

                string departuredate = string.Empty;
                SSRAvailabiltyModel _SSRAvailabilty = new SSRAvailabiltyModel();
                _SSRAvailabilty.passengerKeys = new string[passengerscount];
                for (int i = 0; i < passengerscount; i++)
                {
                    _SSRAvailabilty.passengerKeys[i] = passeengerKeyList.passengers[i].passengerKey;
                }
                _SSRAvailabilty.currencyCode = _SSRAvailabilty.currencyCode;

                List<Trip> Tripslist = new List<Trip>();
                Trip Tripobj = new Trip();
                Tripobj.origin = passeengerKeyList.journeys[0].designator.origin;
                //Tripobj.departureDate = passeengerKeyList.journeys[0].designator.departure;
                //DateTime dateOnly = passeengerKeyList.journeys[0].designator.departure;
                //string departuredaterequest = (dateOnly.ToString("yyyy-MM-dd"));
                //Tripobj.departureDate = departuredaterequest;
                //  _Trip.departureDate = "2023-07-02";
                List<TripIdentifier> TripIdentifierlist = new List<TripIdentifier>();
                TripIdentifier TripIdentifierobj = new TripIdentifier();
                TripIdentifierobj.carrierCode = passeengerKeyList.journeys[0].segments[0].identifier.carrierCode;
                //_Identifier.carrierCode = "I5";
                TripIdentifierobj.identifier = passeengerKeyList.journeys[0].segments[0].identifier.identifier;
                TripIdentifierlist.Add(TripIdentifierobj);
                Tripobj.identifier = TripIdentifierlist;
                // Tripobj.destination = passengerdetails.destination;
                Tripslist.Add(Tripobj);
                _SSRAvailabilty.trips = Tripslist;



                var jsonSSRAvailabiltyRequest = JsonConvert.SerializeObject(_SSRAvailabilty, Formatting.Indented);

                SSRAvailabiltyResponceModel SSRAvailabiltyResponceobj = new SSRAvailabiltyResponceModel();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseSSRAvailabilty = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/availability", _SSRAvailabilty);
                if (responseSSRAvailabilty.IsSuccessStatusCode)
                {

                    var _responseSSRAvailabilty = responseSSRAvailabilty.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);


                    //  var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                    // ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                    var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
                    //journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();


                    int legSsrscount = JsonObjresponseSSRAvailabilty.data.legSsrs.Count;


                    List<legSsrs> SSRAvailabiltyLegssrlist = new List<legSsrs>();


                    for (int i = 0; i < legSsrscount; i++)
                    {
                        legSsrs SSRAvailabiltyLegssrobj = new legSsrs();
                        SSRAvailabiltyLegssrobj.legKey = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legKey;
                        legDetails legDetailsobj = new legDetails();
                        legDetailsobj.destination = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legDetails.destination;
                        legDetailsobj.origin = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legDetails.origin;
                        legDetailsobj.departureDate = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legDetails.departureDate;
                        legidentifier legidentifierobj = new legidentifier();
                        legidentifierobj.identifier = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legDetails.identifier.identifier;
                        legidentifierobj.carrierCode = JsonObjresponseSSRAvailabilty.data.legSsrs[i].legDetails.identifier.carrierCode;
                        legDetailsobj.legidentifier = legidentifierobj;

                        var ssrscount = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs.Count;

                        List<childlegssrs> legssrslist = new List<childlegssrs>();


                        for (int j = 0; j < ssrscount; j++)
                        {
                            childlegssrs legssrs = new childlegssrs();
                            legssrs.ssrCode = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].ssrCode;
                            legssrs.ssrType = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].ssrType;
                            legssrs.name = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].name;
                            legssrs.limitPerPassenger = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].limitPerPassenger;
                            legssrs.available = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].available;
                            legssrs.feeCode = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].feeCode;
                            List<legpassengers> legpassengerslist = new List<legpassengers>();

                            foreach (var items in JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].passengersAvailability)
                            {
                                legpassengers passengersdetail = new legpassengers();
                                passengersdetail.passengerKey = items.Value.passengerKey;
                                passengersdetail.price = items.Value.price;
                                passengersdetail.ssrKey = items.Value.ssrKey;
                                legpassengerslist.Add(passengersdetail);

                            }

                            legssrs.legpassengers = legpassengerslist;
                            legssrslist.Add(legssrs);
                        }
                        SSRAvailabiltyLegssrobj.legDetails = legDetailsobj;
                        SSRAvailabiltyLegssrobj.legssrs = legssrslist;
                        SSRAvailabiltyLegssrlist.Add(SSRAvailabiltyLegssrobj);

                    }
                    SSRAvailabiltyResponceobj.legSsrs = SSRAvailabiltyLegssrlist;
                    HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));

                }
                #endregion


                HttpContext.Session.SetString("journeyKey", JsonConvert.SerializeObject(journeyKey));
                string LeftshowpopupdataR = HttpContext.Session.GetString("PassengerModel");
                SimpleAvailabilityRequestModel _SimpleAvailabilityobjR = null;
                _SimpleAvailabilityobjR = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(LeftshowpopupdataR);
                var AdtTypeR = "";
                var AdtCountR = 0;

                var chdtypeR = "";
                var chdcountR = 0;

                var infanttypeR = "";
                var infantcountR = 0;

                int countpassengerR = _SimpleAvailabilityobjR.passengers.types.Count;
                AdtType = _SimpleAvailabilityobjR.passengers.types[0].type;
                AirAsiaTripSellRequestReturn AirAsiaTripSellRequestobjR = new AirAsiaTripSellRequestReturn();
                _KeyReturn keyR = new _KeyReturn();
                List<_KeyReturn> _keylistR = new List<_KeyReturn>();


                string JourneyKeyOnewayR = journeyKey[1];
                string[] JpartsR = JourneyKeyOnewayR.Split('/');
                string _JourneykeyDataR = JpartsR[0];
                keyR.journeyKey = _JourneykeyDataR;


                string fareKeyKeyOnewayR = fareKey[1];
                string[] FpartsR = fareKeyKeyOnewayR.Split('/');
                string _FareKeyDataR = FpartsR[0];
                keyR.fareAvailabilityKey = _FareKeyDataR;

                _keylistR.Add(keyR);
                AirAsiaTripSellRequestobjR.keys = _keylistR;

                PassengersReturn passengersR = new PassengersReturn();
                List<_TypesReturn> _typeslistR = new List<_TypesReturn>();

                for (int i = 0; i < _SimpleAvailabilityobjR.passengers.types.Count; i++)
                {
                    _TypesReturn _TypesR = new _TypesReturn();

                    if (_SimpleAvailabilityobjR.passengers.types[i].type == "ADT")
                    {
                        AdtTypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                        _TypesR.type = AdtTypeR;
                        _TypesR.count = _SimpleAvailabilityobjR.passengers.types[i].count;
                    }
                    else if (_SimpleAvailabilityobjR.passengers.types[i].type == "CHD")
                    {
                        chdtypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                        _TypesR.type = chdtypeR;
                        _TypesR.count = _SimpleAvailabilityobjR.passengers.types[i].count;
                    }
                    else if (_SimpleAvailabilityobjR.passengers.types[i].type == "INFT")
                    {
                        infanttypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                        continue;
                    }
                    //	
                    _typeslistR.Add(_TypesR);
                }
                passengersR.types = _typeslistR;
                AirAsiaTripSellRequestobjR.passengers = passengersR;
                AirAsiaTripSellRequestobjR.currencyCode = "INR";
                AirAsiaTripSellRequestobjR.preventOverlap = true;
                AirAsiaTripSellRequestobjR.suppressPassengerAgeValidation = true;
                var AirasiaTripSellRequestR = JsonConvert.SerializeObject(AirAsiaTripSellRequestobjR, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseTripsellR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v4/trip/sell", AirAsiaTripSellRequestobjR);
                if (responseTripsellR.IsSuccessStatusCode)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobjR = new AirAsiaTripResponceModel();
                    var resultsTripsellR = responseTripsellR.Content.ReadAsStringAsync().Result;
                    var JsonObjTripsellR = JsonConvert.DeserializeObject<dynamic>(resultsTripsellR);
                    //var totalAmount = JsonObjTripsell.data.breakdown.journeys[journeyKey[0]].totalAmount;
                    //var totalTax = JsonObjTripsell.data.breakdown.journeys[journeyKey[0]].totalTax;
                    #region Comment
                    int journeyscountR = JsonObjTripsellR.data.journeys.Count;
                    List<AAJourney> AAJourneyListR = new List<AAJourney>();
                    for (int i = 0; i < journeyscountR; i++)
                    {
                        if (journeyscountR == 2 && i == 0)
                        {
                            continue;
                        }
                        AAJourney AAJourneyobjR = new AAJourney();

                        AAJourneyobjR.flightType = JsonObjTripsellR.data.journeys[i].flightType;
                        AAJourneyobjR.stops = JsonObjTripsellR.data.journeys[i].stops;
                        AAJourneyobjR.journeyKey = JsonObjTripsellR.data.journeys[i].journeyKey;

                        AADesignator AADesignatorobjR = new AADesignator();
                        AADesignatorobjR.origin = JsonObjTripsellR.data.journeys[0].designator.origin;
                        AADesignatorobjR.destination = JsonObjTripsellR.data.journeys[0].designator.destination;
                        AADesignatorobjR.departure = JsonObjTripsellR.data.journeys[0].designator.departure;
                        AADesignatorobjR.arrival = JsonObjTripsellR.data.journeys[0].designator.arrival;
                        AAJourneyobjR.designator = AADesignatorobjR;


                        int segmentscountR = JsonObjTripsellR.data.journeys[i].segments.Count;
                        List<AASegment> AASegmentlistR = new List<AASegment>();
                        for (int j = 0; j < segmentscountR; j++)
                        {
                            AASegment AASegmentobjR = new AASegment();
                            AASegmentobjR.isStandby = JsonObjTripsellR.data.journeys[i].segments[j].isStandby;
                            AASegmentobjR.isHosted = JsonObjTripsellR.data.journeys[i].segments[j].isHosted;

                            AADesignator AASegmentDesignatorobjR = new AADesignator();
                            var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
                            AASegmentDesignatorobjR.origin = JsonObjTripsellR.data.journeys[i].segments[j].designator.origin;
                            AASegmentDesignatorobjR.destination = JsonObjTripsellR.data.journeys[i].segments[j].designator.destination;
                            AASegmentDesignatorobjR.departure = JsonObjTripsellR.data.journeys[i].segments[j].designator.departure;
                            AASegmentDesignatorobjR.arrival = JsonObjTripsellR.data.journeys[i].segments[j].designator.arrival;
                            AASegmentobjR.designator = AASegmentDesignatorobjR;

                            int fareCountR = JsonObjTripsellR.data.journeys[i].segments[j].fares.Count;
                            List<AAFare> AAFarelistR = new List<AAFare>();
                            for (int k = 0; k < fareCountR; k++)
                            {
                                AAFare AAFareobjR = new AAFare();
                                AAFareobjR.fareKey = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].fareKey;
                                AAFareobjR.productClass = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].productClass;

                                var passengerFaresR = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].passengerFares;

                                int passengerFarescountR = ((Newtonsoft.Json.Linq.JContainer)passengerFaresR).Count;
                                List<AAPassengerfare> AAPassengerfarelistR = new List<AAPassengerfare>();
                                for (int l = 0; l < passengerFarescountR; l++)
                                {
                                    AAPassengerfare AAPassengerfareobjR = new AAPassengerfare();
                                    AAPassengerfareobjR.passengerType = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].passengerFares[l].passengerType;

                                    var serviceCharges1R = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges;
                                    int serviceChargescountR = ((Newtonsoft.Json.Linq.JContainer)serviceCharges1R).Count;
                                    List<AAServicecharge> AAServicechargelistR = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescountR; m++)
                                    {
                                        AAServicecharge AAServicechargeobjR = new AAServicecharge();

                                        AAServicechargeobjR.amount = JsonObjTripsellR.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;


                                        AAServicechargelistR.Add(AAServicechargeobjR);
                                    }



                                    AAPassengerfareobjR.serviceCharges = AAServicechargelistR;

                                    AAPassengerfarelistR.Add(AAPassengerfareobjR);

                                }
                                AAFareobjR.passengerFares = AAPassengerfarelistR;

                                AAFarelistR.Add(AAFareobjR);




                            }
                            AASegmentobjR.fares = AAFarelistR;
                            AAIdentifier AAIdentifierobjR = new AAIdentifier();

                            AAIdentifierobjR.identifier = JsonObjTripsellR.data.journeys[i].segments[j].identifier.identifier;
                            AAIdentifierobjR.carrierCode = JsonObjTripsellR.data.journeys[i].segments[j].identifier.carrierCode;

                            AASegmentobjR.identifier = AAIdentifierobjR;

                            var legR = JsonObjTripsellR.data.journeys[i].segments[j].legs;
                            int legcountR = ((Newtonsoft.Json.Linq.JContainer)legR).Count;
                            List<AALeg> AALeglistR = new List<AALeg>();
                            for (int n = 0; n < legcountR; n++)
                            {
                                AALeg AALegR = new AALeg();
                                AALegR.legKey = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].legKey;
                                AADesignator AAlegDesignatorobjR = new AADesignator();
                                AAlegDesignatorobjR.origin = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].designator.origin;
                                AAlegDesignatorobjR.destination = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].designator.destination;
                                AAlegDesignatorobjR.departure = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].designator.departure;
                                AAlegDesignatorobjR.arrival = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].designator.arrival;
                                AALegR.designator = AAlegDesignatorobjR;

                                AALeginfo AALeginfoobjR = new AALeginfo();
                                AALeginfoobjR.arrivalTerminal = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                AALeginfoobjR.arrivalTime = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                AALeginfoobjR.departureTerminal = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                AALeginfoobjR.departureTime = JsonObjTripsellR.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                AALegR.legInfo = AALeginfoobjR;
                                AALeglistR.Add(AALegR);

                            }

                            AASegmentobjR.legs = AALeglistR;
                            AASegmentlistR.Add(AASegmentobjR);
                        }

                        AAJourneyobjR.segments = AASegmentlistR;
                        AAJourneyListR.Add(AAJourneyobjR);

                    }
                    var passangerR = JsonObjTripsellR.data.passengers;
                    int passengercountR = ((Newtonsoft.Json.Linq.JContainer)passangerR).Count;
                    List<AAPassengers> passkeylistR = new List<AAPassengers>();
                    foreach (var items in JsonObjTripsellR.data.passengers)
                    {
                        AAPassengers passkeytypeobjR = new AAPassengers();
                        passkeytypeobjR.passengerKey = items.Value.passengerKey;
                        passkeytypeobjR.passengerTypeCode = items.Value.passengerTypeCode;
                        //  passkeytypeobj.passengertypecount = items.Count;

                        passkeylistR.Add(passkeytypeobjR);
                        passengerkey12 = passkeytypeobjR.passengerKey;
                    }
                    AirAsiaTripResponceobjR.journeys = AAJourneyListR;
                    AirAsiaTripResponceobjR.passengers = passkeylistR;
                    AirAsiaTripResponceobjR.passengerscount = passengercountR;
                    HttpContext.Session.SetString("keypassengerReturn", JsonConvert.SerializeObject(AirAsiaTripResponceobjR));
                    #endregion
                }
                #region itenary 
                if (infanttypeR != null)
                {
                    string passengerdatainfantR = HttpContext.Session.GetString("keypassengerReturn");
                    AirAsiaTripResponceModel passeengerKeyListinfantR = null;
                    passeengerKeyListinfantR = JsonConvert.DeserializeObject<AirAsiaTripResponceModel>(passengerdatainfantR);
                    string RightpassengerModal = HttpContext.Session.GetString("PassengerModelR");
                    SimpleAvailabilityRequestModel _SimpleAvailabilityobjectR = null;
                    _SimpleAvailabilityobjectR = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(RightpassengerModal);

                    GetItenaryModel itenaryInfantR = new GetItenaryModel();
                    List<Ssr1> ssr1slistR = new List<Ssr1>();
                    Ssr1 ssr1R = new Ssr1();
                    Market marketobjR = new Market();
                    Identifier1 identifier1R = new Identifier1();
                    //Market
                    identifier1R.identifier = passeengerKeyListinfantR.journeys[0].segments[0].identifier.identifier;
                    identifier1R.carrierCode = passeengerKeyListinfantR.journeys[0].segments[0].identifier.carrierCode;
                    marketobjR.identifier = identifier1R;
                    marketobjR.destination = passeengerKeyListinfantR.journeys[0].segments[0].designator.destination;
                    marketobjR.origin = passeengerKeyListinfantR.journeys[0].segments[0].designator.origin;
                    //marketobj.departureDate = passeengerKeyListinfant.journeys[0].segments[0].designator.departure.ToString("yyyy-MM-dd");
                    marketobjR.departureDate = _SimpleAvailabilityobjectR.beginDate;
                    ssr1R.market = marketobjR;
                    ssr1slistR.Add(ssr1R);
                    //item
                    List<Item> itemListR = new List<Item>();
                    //int passtypedatacount = passeengerKeyListinfant.passengers.Count;	
                    int typecountR = _SimpleAvailabilityobjectR.passengers.types.Count;

                    for (int i = 0; i < typecountR; i++)
                    {
                        var infantR = _SimpleAvailabilityobjectR.passengers.types[i].type;
                        if (infantR == "INFT")
                        {
                            int infantCount1R = _SimpleAvailabilityobjectR.passengers.types[i].count;

                            for (int j = 0; j < infantCount1R; j++)
                            {

                                Item itemobjR = new Item();
                                List<SsrItem> ssrItemslistR = new List<SsrItem>();
                                SsrItem ssrItemobjR = new SsrItem();

                                ssrItemobjR.ssrCode = "INFT";
                                ssrItemobjR.count = 1;
                                ssrItemslistR.Add(ssrItemobjR);

                                Designatorr designatorrR = new Designatorr();
                                designatorrR.destination = passeengerKeyListinfantR.journeys[0].segments[0].designator.destination;
                                designatorrR.origin = passeengerKeyListinfantR.journeys[0].segments[0].designator.origin;
                                //designatorr.departureDate = passeengerKeyListinfant.journeys[0].segments[0].designator.departure;
                                designatorrR.departureDate = _SimpleAvailabilityobjectR.beginDate;
                                ssrItemobjR.designator = designatorrR;
                                //ssrItemslist.Add(ssrItemobj);
                                itemobjR.passengerType = passeengerKeyListinfantR.passengers[0].passengerTypeCode;
                                itemobjR.ssrs = ssrItemslistR;
                                itemListR.Add(itemobjR);
                            }
                        }
                    }
                    //      int infantCount = _SimpleAvailabilityobject.passengers.types[1].count;			

                    ssr1R.items = itemListR;
                    itenaryInfantR.ssrs = ssr1slistR;
                    List<Key> keylistR = new List<Key>();
                    Key KeyobjR = new Key();
                    KeyobjR.journeyKey = journeyKey[0];
                    KeyobjR.fareAvailabilityKey = fareKey[0];
                    KeyobjR.standbyPriorityCode = "";
                    KeyobjR.inventoryControl = "HoldSpace";
                    keylistR.Add(KeyobjR);
                    itenaryInfantR.keys = keylistR;
                    Passengers1 passengers1R = new Passengers1();
                    passengers1R.residentCountry = "IN";
                    List<Type2> typelistR = new List<Type2>();
                    for (int i = 0; i < _SimpleAvailabilityobjR.passengers.types.Count; i++)
                    {
                        Type2 _TypesR = new Type2();

                        if (_SimpleAvailabilityobjR.passengers.types[i].type == "ADT")
                        {
                            AdtTypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                            _TypesR.type = AdtTypeR;
                            _TypesR.count = _SimpleAvailabilityobjR.passengers.types[i].count;
                        }
                        else if (_SimpleAvailabilityobjR.passengers.types[i].type == "CHD")
                        {
                            chdtypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                            _TypesR.type = chdtypeR;
                            _TypesR.count = _SimpleAvailabilityobjR.passengers.types[i].count;
                        }
                        else if (_SimpleAvailabilityobjR.passengers.types[i].type == "INFT")
                        {
                            infanttypeR = _SimpleAvailabilityobjR.passengers.types[i].type;
                            continue;
                        }
                        //	
                        typelistR.Add(_TypesR);
                    }
                    passengers1R.types = typelistR;
                    itenaryInfantR.passengers = passengers1R;
                    itenaryInfantR.currencyCode = "INR";

                    var jsonPassengersR = JsonConvert.SerializeObject(itenaryInfantR, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responsePassengersR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/bookings/quote", itenaryInfantR);
                    if (responsePassengersR.IsSuccessStatusCode)
                    {
                        AirAsiaTripResponceModel AirAsiaTripResponceobjectR = new AirAsiaTripResponceModel();
                        var _responsePassengersR = responsePassengersR.Content.ReadAsStringAsync().Result;
                        var JsonObjPassengersR = JsonConvert.DeserializeObject<dynamic>(_responsePassengersR);
                        //var TotalAmount = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalAmount;
                        //var TotalTax = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalTax;
                        int JourneyscountR = JsonObjPassengersR.data.journeys.Count;
                        List<AAJourney> AAJourneyListR = new List<AAJourney>();
                        for (int i = 0; i < JourneyscountR; i++)
                        {
                            AAJourney AAJourneyobjectR = new AAJourney();

                            AAJourneyobjectR.flightType = JsonObjPassengersR.data.journeys[i].flightType;
                            AAJourneyobjectR.stops = JsonObjPassengersR.data.journeys[i].stops;
                            AAJourneyobjectR.journeyKey = JsonObjPassengersR.data.journeys[i].journeyKey;

                            AADesignator AADesignatorobjectR = new AADesignator();
                            AADesignatorobjectR.origin = JsonObjPassengersR.data.journeys[0].designator.origin;
                            AADesignatorobjectR.destination = JsonObjPassengersR.data.journeys[0].designator.destination;
                            AADesignatorobjectR.departure = JsonObjPassengersR.data.journeys[0].designator.departure;
                            AADesignatorobjectR.arrival = JsonObjPassengersR.data.journeys[0].designator.arrival;
                            AAJourneyobjectR.designator = AADesignatorobjectR;


                            int SegmentscountR = JsonObjPassengersR.data.journeys[i].segments.Count;
                            List<AASegment> AASegmentlistR = new List<AASegment>();
                            for (int j = 0; j < SegmentscountR; j++)
                            {
                                AASegment AASegmentobjectR = new AASegment();
                                AASegmentobjectR.isStandby = JsonObjPassengersR.data.journeys[i].segments[j].isStandby;
                                AASegmentobjectR.isHosted = JsonObjPassengersR.data.journeys[i].segments[j].isHosted;

                                AADesignator AASegmentDesignatorobjectR = new AADesignator();
                                AASegmentDesignatorobjectR.origin = JsonObjPassengersR.data.journeys[i].segments[j].designator.origin;
                                AASegmentDesignatorobjectR.destination = JsonObjPassengersR.data.journeys[i].segments[j].designator.destination;
                                AASegmentDesignatorobjectR.departure = JsonObjPassengersR.data.journeys[i].segments[j].designator.departure;
                                AASegmentDesignatorobjectR.arrival = JsonObjPassengersR.data.journeys[i].segments[j].designator.arrival;
                                AASegmentobjectR.designator = AASegmentDesignatorobjectR;

                                int FareCountR = JsonObjPassengersR.data.journeys[i].segments[j].fares.Count;
                                List<AAFare> AAFareListR = new List<AAFare>();
                                for (int k = 0; k < FareCountR; k++)
                                {
                                    AAFare AAFareobjectR = new AAFare();
                                    AAFareobjectR.fareKey = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].fareKey;
                                    AAFareobjectR.productClass = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].productClass;

                                    var PassengerFaresR = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].passengerFares;

                                    int PassengerFarescountR = ((Newtonsoft.Json.Linq.JContainer)PassengerFaresR).Count;
                                    List<AAPassengerfare> AAPassengerfareListR = new List<AAPassengerfare>();
                                    for (int l = 0; l < PassengerFarescountR; l++)
                                    {
                                        AAPassengerfare AAPassengerfareobjectR = new AAPassengerfare();
                                        AAPassengerfareobjectR.passengerType = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].passengerFares[l].passengerType;

                                        var ServiceCharges1R = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges;
                                        int ServiceChargescountR = ((Newtonsoft.Json.Linq.JContainer)ServiceCharges1R).Count;
                                        List<AAServicecharge> AAServicechargeListR = new List<AAServicecharge>();
                                        for (int m = 0; m < ServiceChargescountR; m++)
                                        {
                                            AAServicecharge AAServicechargeobjectR = new AAServicecharge();

                                            AAServicechargeobjectR.amount = JsonObjPassengersR.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;


                                            AAServicechargeListR.Add(AAServicechargeobjectR);
                                        }



                                        AAPassengerfareobjectR.serviceCharges = AAServicechargeListR;

                                        AAPassengerfareListR.Add(AAPassengerfareobjectR);

                                    }
                                    AAFareobjectR.passengerFares = AAPassengerfareListR;

                                    AAFareListR.Add(AAFareobjectR);




                                }
                                AASegmentobjectR.fares = AAFareListR;
                                AAIdentifier AAIdentifierobjR = new AAIdentifier();

                                AAIdentifierobjR.identifier = JsonObjPassengersR.data.journeys[i].segments[j].identifier.identifier;
                                AAIdentifierobjR.carrierCode = JsonObjPassengersR.data.journeys[i].segments[j].identifier.carrierCode;

                                AASegmentobjectR.identifier = AAIdentifierobjR;

                                var LegR = JsonObjPassengersR.data.journeys[i].segments[j].legs;
                                int LegcountR = ((Newtonsoft.Json.Linq.JContainer)LegR).Count;
                                List<AALeg> AALeglistR = new List<AALeg>();
                                for (int n = 0; n < LegcountR; n++)
                                {
                                    AALeg AALegobjR = new AALeg();
                                    AALegobjR.legKey = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].legKey;
                                    AADesignator AAlegDesignatorobjectR = new AADesignator();
                                    AAlegDesignatorobjectR.origin = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].designator.origin;
                                    AAlegDesignatorobjectR.destination = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].designator.destination;
                                    AAlegDesignatorobjectR.departure = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].designator.departure;
                                    AAlegDesignatorobjectR.arrival = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].designator.arrival;
                                    AALegobjR.designator = AAlegDesignatorobjectR;

                                    AALeginfo AALeginfoobjectR = new AALeginfo();
                                    AALeginfoobjectR.arrivalTerminal = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                    AALeginfoobjectR.arrivalTime = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                    AALeginfoobjectR.departureTerminal = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                    AALeginfoobjectR.departureTime = JsonObjPassengersR.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                    AALegobjR.legInfo = AALeginfoobjectR;
                                    AALeglistR.Add(AALegobjR);

                                }

                                AASegmentobjectR.legs = AALeglistR;
                                AASegmentlistR.Add(AASegmentobjectR);
                            }

                            AAJourneyobjectR.segments = AASegmentlistR;
                            AAJourneyListR.Add(AAJourneyobjectR);

                        }
                        var PassangerR = JsonObjPassengersR.data.passengers;
                        int passengercountR = ((Newtonsoft.Json.Linq.JContainer)PassangerR).Count;
                        List<AAPassengers> passkeyListR = new List<AAPassengers>();
                        Infant infantobjectR = null;
                        Fee feeobjectR = null;
                        foreach (var items in JsonObjPassengersR.data.passengers)
                        {
                            AAPassengers passkeytypeobjectR = new AAPassengers();
                            passkeytypeobjectR.passengerKey = items.Value.passengerKey;
                            passkeytypeobjectR.passengerTypeCode = items.Value.passengerTypeCode;
                            passkeyListR.Add(passkeytypeobjectR);
                            passengerkey12 = passkeytypeobjectR.passengerKey;
                            //infant
                            if (passkeytypeobjectR.passengerTypeCode != "CHD")
                            {

                                if (JsonObjPassengersR.data.passengers[passkeytypeobjectR.passengerKey].infant != null)
                                {
                                    int FeecountR = JsonObjPassengersR.data.passengers[passkeytypeobjectR.passengerKey].infant.fees.Count;
                                    List<Fee> feeListR = new List<Fee>();
                                    for (int i = 0; i < FeecountR; i++)
                                    {
                                        infantobjectR = new Infant();
                                        feeobjectR = new Fee();
                                        feeobjectR.isConfirmed = false;
                                        feeobjectR.isConfirming = false;
                                        feeobjectR.isConfirmingExternal = false;
                                        feeobjectR.code = JsonObjPassengersR.data.passengers[passkeytypeobjectR.passengerKey].infant.fees[i].code;
                                        feeobjectR._override = false;
                                        feeobjectR.note = "";
                                        feeobjectR.isProtected = false;
                                        infantobjectR.nationality = "";
                                        infantobjectR.dateOfBirth = "";
                                        infantobjectR.travelDocuments = "";
                                        infantobjectR.residentCountry = "";
                                        infantobjectR.gender = 1;
                                        infantobjectR.name = "";
                                        infantobjectR.type = "";
                                        feeListR.Add(feeobjectR);
                                        infantobjectR.fees = feeListR;
                                        passkeytypeobjectR.infant = infantobjectR;
                                    }
                                }
                            }
                            AirAsiaTripResponceobjectR.journeys = AAJourneyListR;
                            AirAsiaTripResponceobjectR.passengers = passkeyListR;
                            AirAsiaTripResponceobjectR.passengerscount = passengercountR;
                            HttpContext.Session.SetString("keypassengerItanaryReturn", JsonConvert.SerializeObject(AirAsiaTripResponceobjectR));

                            //string passengerInfant = HttpContext.Session.GetString("keypassengerItanaryReturn");

                        }
                    }
                }
                #endregion
                #region seatmap
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage responseSeatmap = await client.PostAsJsonAsync(BaseURL + "api/nsk/v3/booking/seatmaps/journey/ {{journeyKey}}", _SSRAvailabilty);
                //  var REQUEST1= (BaseURL + "/api/nsk/v3/booking/seatmaps/journey/{{" +journeyKey1+ "}}?IncludePropertyLookup=true");
                HttpResponseMessage responseSeatmapR = await client.GetAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/seatmaps/journey/" + keyR.journeyKey + "?IncludePropertyLookup=true");
                // data[0].seatMap.decks['1'].compartments.Y.units[0].unitKey
                if (responseSeatmapR.IsSuccessStatusCode)
                {
                    var _responseSeatmapR = responseSeatmapR.Content.ReadAsStringAsync().Result;
                    var JsonObjSeatmapR = JsonConvert.DeserializeObject<dynamic>(_responseSeatmapR);
                    // var decks1 = "1";
                    //  x.data[0].seatMap.decks.1.compartments.Y.units[0].unitKey
                    var uniquekey1R = JsonObjSeatmapR.data[0].seatMap.decks["1"].compartments.Y.units[0].unitKey;
                    //  uniquekey = ((Newtonsoft.Json.Linq.JValue)uniquekey1).Value.ToString();
                    var dataR = JsonObjSeatmapR.data.Count;

                    List<data> datalistR = new List<data>();
                    for (int x = 0; x < dataR; x++)
                    {
                        data dataobjR = new data();
                        SeatMapResponceModel SeatMapResponceModelR = new SeatMapResponceModel();
                        List<SeatMapResponceModel> SeatMapResponceModellistR = new List<SeatMapResponceModel>();
                        Fees FeesR = new Fees();
                        Seatmap SeatmapobjR = new Seatmap();
                        SeatmapobjR.name = JsonObjSeatmapR.data[x].seatMap.name;
                        SeatmapobjR.arrivalStation = JsonObjSeatmapR.data[x].seatMap.arrivalStation;
                        SeatmapobjR.departureStation = JsonObjSeatmapR.data[x].seatMap.departureStation;
                        SeatmapobjR.marketingCode = JsonObjSeatmapR.data[x].seatMap.marketingCode;
                        SeatmapobjR.equipmentType = JsonObjSeatmapR.data[x].seatMap.equipmentType;
                        SeatmapobjR.equipmentTypeSuffix = JsonObjSeatmapR.data[x].seatMap.equipmentTypeSuffix;
                        SeatmapobjR.category = JsonObjSeatmapR.data[x].seatMap.category;
                        SeatmapobjR.seatmapReference = JsonObjSeatmapR.data[x].seatMap.seatmapReference;
                        Decks DecksobjR = new Decks();
                        DecksobjR.availableUnits = JsonObjSeatmapR.data[x].seatMap.availableUnits;
                        // Seatmap Seatmapobj = JsonObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.availableUnits;
                        DecksobjR.designator = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.designator;
                        DecksobjR.length = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.length;
                        DecksobjR.width = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.width;
                        DecksobjR.sequence = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.sequence;
                        DecksobjR.orientation = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.orientation;
                        SeatmapobjR.decks = DecksobjR;
                        //  int feesgroup1 = JsonObjSeatmap.data[0].fees[passengerdetails.passengerkey].groups.Count;

                        int compartmentsunitCountR = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units.Count;
                        List<Unit> compartmentsunitlistR = new List<Unit>();
                        for (int i = 0; i < compartmentsunitCountR; i++)
                        {
                            Unit compartmentsunitobjR = new Unit();
                            compartmentsunitobjR.unitKey = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].unitKey;
                            compartmentsunitobjR.assignable = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].assignable;
                            compartmentsunitobjR.availability = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].availability;
                            compartmentsunitobjR.compartmentDesignator = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].compartmentDesignator;
                            compartmentsunitobjR.designator = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].designator;
                            compartmentsunitobjR.type = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].type;
                            compartmentsunitobjR.travelClassCode = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].travelClassCode;
                            compartmentsunitobjR.set = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].set;
                            compartmentsunitobjR.group = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].group;
                            compartmentsunitobjR.priority = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].priority;
                            compartmentsunitobjR.text = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].text;
                            compartmentsunitobjR.setVacancy = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                            compartmentsunitobjR.angle = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].angle;
                            compartmentsunitobjR.width = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].width;
                            compartmentsunitobjR.height = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].height;
                            compartmentsunitobjR.zone = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].zone;
                            compartmentsunitobjR.x = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].x;
                            compartmentsunitobjR.y = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].y;
                            compartmentsunitlistR.Add(compartmentsunitobjR);

                            int compartmentypropertiesCountR = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].properties.Count;
                            List<Properties> PropertieslistR = new List<Properties>();
                            for (int j = 0; j < compartmentypropertiesCountR; j++)
                            {
                                Properties compartmentypropertiesR = new Properties();
                                compartmentypropertiesR.code = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].properties[j].code;
                                compartmentypropertiesR.value = JsonObjSeatmapR.data[x].seatMap.decks["1"].compartments.Y.units[i].properties[j].value;
                                PropertieslistR.Add(compartmentypropertiesR);
                            }

                            //compartmentsunitlist.Add(compartmentsunitobj);
                            compartmentsunitobjR.properties = PropertieslistR;
                            DecksobjR.units = compartmentsunitlistR;
                        }

                        var groupscountR = JsonObjSeatmapR.data[x].fees[passengerkey12].groups;
                        var feesgroupcountR = ((Newtonsoft.Json.Linq.JContainer)groupscountR).Count;
                        string strTextR = Regex.Match(_responseSeatmapR, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;


                        List<Groups> GroupsFeelistR = new List<Groups>();
                        foreach (Match item in Regex.Matches(strTextR, @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                        {

                            Groups GroupsobjR = new Groups();
                            int myString1R = Convert.ToInt32(item.Groups["key"].Value.Trim());
                            string myStringR = myString1R.ToString();
                            // x.data[0].fees["MCFBRFQ-"].groups.1
                            var groupR = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR.ToString()];

                            //var fees = JsonObjSeatmap.data[0].fees["MCFBRFQ-"].groups[myString].fees;
                            //  Groups Groups = new Groups();

                            GroupsFee GroupsFeeobjR = new GroupsFee();


                            GroupsFeeobjR.type = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].type;
                            GroupsFeeobjR.ssrCode = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].ssrCode;
                            GroupsFeeobjR.ssrNumber = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].ssrNumber;
                            GroupsFeeobjR.paymentNumber = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].paymentNumber;
                            GroupsFeeobjR.isConfirmed = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].isConfirmed;
                            GroupsFeeobjR.isConfirming = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].isConfirming;
                            GroupsFeeobjR.isConfirmingExternal = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].isConfirmingExternal;
                            GroupsFeeobjR.code = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].code;
                            GroupsFeeobjR.detail = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].detail;
                            GroupsFeeobjR.passengerFeeKey = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].passengerFeeKey;
                            //     Feegroupsobj._override = JsonObjSeatmap.data[0].fees[passengerdetails.passengerkey].groups[myString].fees[0]._override;
                            GroupsFeeobjR.flightReference = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].flightReference;
                            GroupsFeeobjR.note = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].note;
                            GroupsFeeobjR.createdDate = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].createdDate;
                            GroupsFeeobjR.isProtected = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].isProtected;
                            //Groups.groupsFee = GroupsFeeobj;
                            var feesgroupserviceChargescountR = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges.Count;

                            List<Servicecharge> feesgroupserviceChargeslistR = new List<Servicecharge>();
                            for (int l = 0; l < feesgroupserviceChargescountR; l++)
                            {

                                Servicecharge feesgroupserviceChargesobjR = new Servicecharge();
                                feesgroupserviceChargesobjR.amount = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].amount;
                                feesgroupserviceChargesobjR.code = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].code;
                                feesgroupserviceChargesobjR.detail = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].detail;
                                feesgroupserviceChargesobjR.type = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].type;
                                feesgroupserviceChargesobjR.collectType = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].collectType;
                                feesgroupserviceChargesobjR.currencyCode = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].currencyCode;
                                feesgroupserviceChargesobjR.amount = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].amount;
                                feesgroupserviceChargesobjR.foreignAmount = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].foreignAmount;
                                feesgroupserviceChargesobjR.ticketCode = JsonObjSeatmapR.data[x].fees[passengerkey12].groups[myStringR].fees[0].serviceCharges[l].ticketCode;
                                feesgroupserviceChargeslistR.Add(feesgroupserviceChargesobjR);
                            }

                            GroupsFeeobjR.serviceCharges = feesgroupserviceChargeslistR;

                            GroupsobjR.groupsFee = GroupsFeeobjR;
                            //var data = from obj in Groupsobj
                            GroupsFeelistR.Add(GroupsobjR);
                            FeesR.groups = GroupsFeelistR;



                        }

                        dataobjR.seatMap = SeatmapobjR;
                        dataobjR.seatMapfees = FeesR;
                        datalistR.Add(dataobjR);
                        SeatMapResponceModelR.datalist = datalistR;
                        //SeatMapResponceModel.seatMapfees = Fees;
                        //SeatMapResponceModellist.Add(SeatMapResponceModel);




                        HttpContext.Session.SetString("SeatmapReturn", JsonConvert.SerializeObject(SeatMapResponceModelR));
                        //TempData["Seatmap"] = JsonConvert.SerializeObject(SeatMapResponceModellist);
                    }
                }
                #endregion
                #region meals

                string passengerdataR = HttpContext.Session.GetString("keypassengerReturn");

                AirAsiaTripResponceModel passeengerKeyListR = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerdataR, typeof(AirAsiaTripResponceModel));
                int passengerscountR = passeengerKeyListR.passengerscount;

                string departuredateR = string.Empty;
                SSRAvailabiltyModel _SSRAvailabiltyR = new SSRAvailabiltyModel();
                _SSRAvailabiltyR.passengerKeys = new string[passengerscountR];
                for (int i = 0; i < passengerscountR; i++)
                {
                    _SSRAvailabiltyR.passengerKeys[i] = passeengerKeyListR.passengers[i].passengerKey;
                }
                _SSRAvailabiltyR.currencyCode = _SSRAvailabiltyR.currencyCode;

                List<Trip> TripslistR = new List<Trip>();
                Trip TripobjR = new Trip();
                //TripobjR.origin = passeengerKeyListR.journeys[0].designator.origin;
                //TripobjR.departureDate = passeengerKeyListR.journeys[0].designator.departure;
                TripobjR.origin = passeengerKeyListR.journeys[0].segments[0].designator.origin;
                //TripobjR.departureDate = passeengerKeyListR.journeys[0].segments[0].designator.departure; //commented by vivek to code error during merge




                //DateTime dateOnly = passeengerKeyList.journeys[0].designator.departure;
                //string departuredaterequest = (dateOnly.ToString("yyyy-MM-dd"));
                //Tripobj.departureDate = departuredaterequest;
                //  _Trip.departureDate = "2023-07-02";
                List<TripIdentifier> TripIdentifierlistR = new List<TripIdentifier>();
                TripIdentifier TripIdentifierobjR = new TripIdentifier();
                TripIdentifierobjR.carrierCode = passeengerKeyListR.journeys[0].segments[0].identifier.carrierCode;
                //_Identifier.carrierCode = "I5";
                TripIdentifierobjR.identifier = passeengerKeyListR.journeys[0].segments[0].identifier.identifier;
                TripIdentifierlistR.Add(TripIdentifierobjR);
                TripobjR.identifier = TripIdentifierlistR;
                // Tripobj.destination = passengerdetails.destination;
                TripslistR.Add(TripobjR);
                _SSRAvailabiltyR.trips = TripslistR;
                var jsonSSRAvailabiltyRequestR = JsonConvert.SerializeObject(_SSRAvailabiltyR, Formatting.Indented);
                SSRAvailabiltyResponceModel SSRAvailabiltyResponceobjR = new SSRAvailabiltyResponceModel();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseSSRAvailabiltyR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/availability", _SSRAvailabiltyR);
                if (responseSSRAvailabiltyR.IsSuccessStatusCode)
                {

                    var _responseSSRAvailabiltyR = responseSSRAvailabilty.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseSSRAvailabiltyR = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabiltyR);


                    //  var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                    // ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                    var journeyKey1R = JsonObjresponseSSRAvailabiltyR.data.journeySsrs[0].journeyKey;
                    //journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();


                    int legSsrscountR = JsonObjresponseSSRAvailabiltyR.data.legSsrs.Count;


                    List<legSsrs> SSRAvailabiltyLegssrlistR = new List<legSsrs>();


                    for (int i = 0; i < legSsrscountR; i++)
                    {
                        legSsrs SSRAvailabiltyLegssrobjR = new legSsrs();
                        SSRAvailabiltyLegssrobjR.legKey = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legKey;
                        legDetails legDetailsobjR = new legDetails();
                        legDetailsobjR.destination = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legDetails.destination;
                        legDetailsobjR.origin = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legDetails.origin;
                        legDetailsobjR.departureDate = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legDetails.departureDate;
                        legidentifier legidentifierobjR = new legidentifier();
                        legidentifierobjR.identifier = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legDetails.identifier.identifier;
                        legidentifierobjR.carrierCode = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].legDetails.identifier.carrierCode;
                        legDetailsobjR.legidentifier = legidentifierobjR;

                        var ssrscountR = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs.Count;

                        List<childlegssrs> legssrslistR = new List<childlegssrs>();


                        for (int j = 0; j < ssrscountR; j++)
                        {
                            childlegssrs legssrsR = new childlegssrs();
                            legssrsR.ssrCode = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].ssrCode;
                            legssrsR.ssrType = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].ssrType;
                            legssrsR.name = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].name;
                            legssrsR.limitPerPassenger = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].limitPerPassenger;
                            legssrsR.available = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].available;
                            legssrsR.feeCode = JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].feeCode;
                            List<legpassengers> legpassengerslistR = new List<legpassengers>();

                            foreach (var items in JsonObjresponseSSRAvailabiltyR.data.legSsrs[i].ssrs[j].passengersAvailability)
                            {
                                legpassengers passengersdetailR = new legpassengers();
                                passengersdetailR.passengerKey = items.Value.passengerKey;
                                passengersdetailR.price = items.Value.price;
                                passengersdetailR.ssrKey = items.Value.ssrKey;
                                legpassengerslistR.Add(passengersdetailR);

                            }

                            legssrsR.legpassengers = legpassengerslistR;
                            legssrslistR.Add(legssrsR);
                        }
                        SSRAvailabiltyLegssrobjR.legDetails = legDetailsobjR;
                        SSRAvailabiltyLegssrobjR.legssrs = legssrslistR;
                        SSRAvailabiltyLegssrlistR.Add(SSRAvailabiltyLegssrobjR);

                    }
                    SSRAvailabiltyResponceobjR.legSsrs = SSRAvailabiltyLegssrlistR;
                    HttpContext.Session.SetString("MealsReturn", JsonConvert.SerializeObject(SSRAvailabiltyResponceobjR));

                }
                #endregion

            }

            return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
        }

    }

}
