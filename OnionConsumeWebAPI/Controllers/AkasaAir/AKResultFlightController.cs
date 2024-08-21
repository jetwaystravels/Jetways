using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using Utility;
using static DomainLayer.Model.GetItenaryModel;
using static DomainLayer.Model.SeatMapResponceModel;
//using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers.AkasaAir
{
    public class AKResultFlightController : Controller
    {
        string passengerkey12 = string.Empty;
        string infant = string.Empty;
        Logs logs = new Logs();
        public async Task<IActionResult> AkasaAirTripsell(string journeyKey, string fareKey, string segmentKey)
        {
            string token = string.Empty;
            List<_credentials> credentialslist = new List<_credentials>();
            using (HttpClient client = new HttpClient())
            {

                string tokenview = HttpContext.Session.GetString("AkasaTokan");

                if (tokenview == "" || tokenview == null)
                {
                    return RedirectToAction("Index");
                }
                if (tokenview == null) { tokenview = ""; }
                token = tokenview.Replace(@"""", string.Empty);


                HttpContext.Session.SetString("journeyKey", JsonConvert.SerializeObject(journeyKey));
                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
                var jsonData = HttpContext.Session.GetString("PassengerModel");
                _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());
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
                key.journeyKey = journeyKey;
                key.fareAvailabilityKey = fareKey;
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
                        _Types.type = infanttype;
                        _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                    }
                    //	
                    _typeslist.Add(_Types);
                }
                List<_Types> _typeslistsell = new List<_Types>();
                for (int i = 0; i < _typeslist.Count; i++)
                {
                    if (_typeslist[i].type == "INFT")
                        continue;
                    _typeslistsell.Add(_typeslist[i]);

                }
                passengers.types = _typeslistsell;
                //passengers.types = _typeslist;
                AirAsiaTripSellRequestobj.passengers = passengers;
                AirAsiaTripSellRequestobj.currencyCode = "INR";
                //AirAsiaTripSellRequestobj.preventOverlap = true;
                AirAsiaTripSellRequestobj.suppressPassengerAgeValidation = true;
                var AirasiaTripSellRequest = JsonConvert.SerializeObject(AirAsiaTripSellRequestobj, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseTripsellAK = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirTripsell, AirAsiaTripSellRequestobj);
                if (responseTripsellAK.IsSuccessStatusCode)
                {
                    AirAsiaTripResponceModel AkasaAirTripResponceobj = new AirAsiaTripResponceModel();
                    var AKjsondata = responseTripsellAK.Content.ReadAsStringAsync().Result;
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(AirAsiaTripSellRequestobj) + "\n Response: " + AKjsondata, "Tripsell", "AkasaOneWay");
                    var Akasajsondata = JsonConvert.DeserializeObject<dynamic>(AKjsondata);

                    var totalAmount = Akasajsondata.data.breakdown.journeys[journeyKey].totalAmount;
                    var totalTax = Akasajsondata.data.breakdown.journeys[journeyKey].totalTax;
                    var basefaretax = Akasajsondata.data.breakdown.journeyTotals.totalTax;
                    int journeyscount = Akasajsondata.data.journeys.Count;
                    List<AAJourney> AkasaJourneyList = new List<AAJourney>();
                    for (int i = 0; i < journeyscount; i++)
                    {
                        AAJourney AkasaJourneyobj = new AAJourney();

                        AkasaJourneyobj.flightType = Akasajsondata.data.journeys[i].flightType;
                        AkasaJourneyobj.stops = Akasajsondata.data.journeys[i].stops;
                        AkasaJourneyobj.journeyKey = Akasajsondata.data.journeys[i].journeyKey;

                        AADesignator AkasaDesignatorobj = new AADesignator();
                        AkasaDesignatorobj.origin = Akasajsondata.data.journeys[0].designator.origin;
                        AkasaDesignatorobj.destination = Akasajsondata.data.journeys[0].designator.destination;
                        AkasaDesignatorobj.departure = Akasajsondata.data.journeys[0].designator.departure;
                        AkasaDesignatorobj.arrival = Akasajsondata.data.journeys[0].designator.arrival;
                        AkasaJourneyobj.designator = AkasaDesignatorobj;


                        int segmentscount = Akasajsondata.data.journeys[i].segments.Count;
                        List<AASegment> AkasaSegmentlist = new List<AASegment>();
                        for (int j = 0; j < segmentscount; j++)
                        {
                            AASegment AkasaSegmentobj = new AASegment();
                            AkasaSegmentobj.isStandby = Akasajsondata.data.journeys[i].segments[j].isStandby;
                            AkasaSegmentobj.isHosted = Akasajsondata.data.journeys[i].segments[j].isHosted;

                            AADesignator AkasaSegmentDesignatorobj = new AADesignator();
                            AkasaSegmentDesignatorobj.origin = Akasajsondata.data.journeys[i].segments[j].designator.origin;
                            AkasaSegmentDesignatorobj.destination = Akasajsondata.data.journeys[i].segments[j].designator.destination;
                            AkasaSegmentDesignatorobj.departure = Akasajsondata.data.journeys[i].segments[j].designator.departure;
                            AkasaSegmentDesignatorobj.arrival = Akasajsondata.data.journeys[i].segments[j].designator.arrival;
                            AkasaSegmentobj.designator = AkasaSegmentDesignatorobj;

                            int fareCount = Akasajsondata.data.journeys[i].segments[j].fares.Count;
                            List<AAFare> AkasaFarelist = new List<AAFare>();
                            for (int k = 0; k < fareCount; k++)
                            {
                                AAFare AkasaFareobj = new AAFare();
                                AkasaFareobj.fareKey = Akasajsondata.data.journeys[i].segments[j].fares[k].fareKey;
                                AkasaFareobj.productClass = Akasajsondata.data.journeys[i].segments[j].fares[k].productClass;
                                var passengerFares = Akasajsondata.data.journeys[i].segments[j].fares[k].passengerFares;

                                int passengerFarescount = ((Newtonsoft.Json.Linq.JContainer)passengerFares).Count;
                                List<AAPassengerfare> AkasaPassengerfarelist = new List<AAPassengerfare>();
                                for (int l = 0; l < passengerFarescount; l++)
                                {
                                    AAPassengerfare AkasaPassengerfareobj = new AAPassengerfare();

                                    AkasaPassengerfareobj.passengerType = Akasajsondata.data.journeys[i].segments[j].fares[k].passengerFares[l].passengerType;

                                    var serviceCharges1 = Akasajsondata.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges;
                                    int serviceChargescount = ((Newtonsoft.Json.Linq.JContainer)serviceCharges1).Count;
                                    List<AAServicecharge> AkasaServicechargelist = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescount; m++)
                                    {
                                        AAServicecharge AkasaServicechargeobj = new AAServicecharge();

                                        AkasaServicechargeobj.amount = Akasajsondata.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;
                                        AkasaServicechargelist.Add(AkasaServicechargeobj);
                                    }
                                    AkasaPassengerfareobj.serviceCharges = AkasaServicechargelist;
                                    AkasaPassengerfarelist.Add(AkasaPassengerfareobj);
                                }
                                AkasaFareobj.passengerFares = AkasaPassengerfarelist;

                                AkasaFarelist.Add(AkasaFareobj);




                            }
                            AkasaSegmentobj.fares = AkasaFarelist;
                            AAIdentifier AkasaIdentifierobj = new AAIdentifier();

                            AkasaIdentifierobj.identifier = Akasajsondata.data.journeys[i].segments[j].identifier.identifier;
                            AkasaIdentifierobj.carrierCode = Akasajsondata.data.journeys[i].segments[j].identifier.carrierCode;

                            AkasaSegmentobj.identifier = AkasaIdentifierobj;

                            var leg = Akasajsondata.data.journeys[i].segments[j].legs;
                            int legcount = ((Newtonsoft.Json.Linq.JContainer)leg).Count;
                            List<AALeg> AkasaLeglist = new List<AALeg>();
                            for (int n = 0; n < legcount; n++)
                            {
                                AALeg AkasaLeg = new AALeg();
                                AkasaLeg.legKey = Akasajsondata.data.journeys[i].segments[j].legs[n].legKey;
                                AADesignator AkasalegDesignatorobj = new AADesignator();
                                AkasalegDesignatorobj.origin = Akasajsondata.data.journeys[i].segments[j].legs[n].designator.origin;
                                AkasalegDesignatorobj.destination = Akasajsondata.data.journeys[i].segments[j].legs[n].designator.destination;
                                AkasalegDesignatorobj.departure = Akasajsondata.data.journeys[i].segments[j].legs[n].designator.departure;
                                AkasalegDesignatorobj.arrival = Akasajsondata.data.journeys[i].segments[j].legs[n].designator.arrival;
                                AkasaLeg.designator = AkasalegDesignatorobj;

                                AALeginfo AkasaLeginfoobj = new AALeginfo();
                                AkasaLeginfoobj.arrivalTerminal = Akasajsondata.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                AkasaLeginfoobj.arrivalTime = Akasajsondata.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                AkasaLeginfoobj.departureTerminal = Akasajsondata.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                AkasaLeginfoobj.departureTime = Akasajsondata.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                AkasaLeg.legInfo = AkasaLeginfoobj;
                                AkasaLeglist.Add(AkasaLeg);

                            }

                            AkasaSegmentobj.legs = AkasaLeglist;
                            AkasaSegmentlist.Add(AkasaSegmentobj);
                        }

                        AkasaJourneyobj.segments = AkasaSegmentlist;
                        AkasaJourneyList.Add(AkasaJourneyobj);

                    }


                    var passanger = Akasajsondata.data.passengers;
                    int passengercount = ((Newtonsoft.Json.Linq.JContainer)passanger).Count;
                    List<AAPassengers> Akasapasskeylist = new List<AAPassengers>();

                    foreach (var items in Akasajsondata.data.passengers)
                    {
                        AAPassengers Akasapasskeytypeobj = new AAPassengers();
                        Akasapasskeytypeobj.passengerKey = items.Value.passengerKey;
                        Akasapasskeytypeobj.passengerTypeCode = items.Value.passengerTypeCode;

                        Akasapasskeylist.Add(Akasapasskeytypeobj);
                        passengerkey12 = Akasapasskeytypeobj.passengerKey;
                    }
                    #region  for passenger view list
                    for (int i = 0; i < _typeslist.Count; i++)
                    {
                        if (_typeslist[i].type == "INFT")
                        {
                            for (int i1 = 0; i1 < _typeslist[i].count; i1++)
                            {
                                AAPassengers Akasapasskeytypeobj = new AAPassengers();
                                Akasapasskeytypeobj.passengerKey = "";
                                Akasapasskeytypeobj.passengerTypeCode = "INFT";
                                Akasapasskeylist.Add(Akasapasskeytypeobj);
                            }

                        }
                    }
                    #endregion
                    AkasaAirTripResponceobj.basefaretax = basefaretax;

                    AkasaAirTripResponceobj.journeys = AkasaJourneyList;
                    AkasaAirTripResponceobj.passengers = Akasapasskeylist;
                    AkasaAirTripResponceobj.passengerscount = passengercount;

                    HttpContext.Session.SetString("ResultFlightPassenger", JsonConvert.SerializeObject(AkasaAirTripResponceobj));
                }
               
                #region Itenary 
                if (infanttype != null)
                {
                    string passengerdatainfant = HttpContext.Session.GetString("ResultFlightPassenger");
                    AirAsiaTripResponceModel passeengerKeyListinfant = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerdatainfant, typeof(AirAsiaTripResponceModel));

                    SimpleAvailabilityRequestModel _SimpleAvailabilityobject = new SimpleAvailabilityRequestModel();
                    var jsonDataObject = HttpContext.Session.GetString("PassengerModel");
                    _SimpleAvailabilityobject = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonDataObject.ToString());

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
                        infant = _SimpleAvailabilityobject.passengers.types[i].type;
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

                    ssr1.items = itemList;
                    itenaryInfant.ssrs = ssr1slist;
                    List<Key> keylist = new List<Key>();
                    Key Keyobj = new Key();
                    Keyobj.journeyKey = journeyKey;
                    Keyobj.fareAvailabilityKey = fareKey;
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
                    if (infant == "INFT")
                    {
                        var jsonPassengers = JsonConvert.SerializeObject(itenaryInfant, Formatting.Indented);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        //HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirInfantDetails, itenaryInfant);
                        HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/bookings/quote", itenaryInfant);
                        if (responsePassengers.IsSuccessStatusCode)
                        {
                            AirAsiaTripResponceModel AirAsiaTripResponceobject = new AirAsiaTripResponceModel();
                            var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            //logs.WriteLogs("Request: " + JsonConvert.SerializeObject(itenaryInfant) + "Url: " + "\n\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "", "AkasaOneWay");
                            var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                            var TotalAmount = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalAmount;
                            var TotalTax = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalTax;
                            var infanttotal= JsonObjPassengers.data.breakdown.passengerTotals.infant.total;
                            var infanttax = JsonObjPassengers.data.breakdown.passengerTotals.infant.taxes;
                            int Journeyscount = JsonObjPassengers.data.journeys.Count;
                            int Inftcount = 0;
                            int Inftbasefare = 0;

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
                                        Inftcount += Feecount;
                                        Inftbasefare = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[0].serviceCharges[0].amount;
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
                                            ServicechargeInfant servicechargeInfantobj = new ServicechargeInfant();
                                            var serviceChargesCount = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[i].serviceCharges.Count;
                                            servicechargeInfantobj.amount = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[i].serviceCharges[0].amount;
                                            feeobject.ServicechargeInfant = servicechargeInfantobj;
                                            //feeobject.ServicechargeInfant = servicechargeInfantList;
                                        }
                                    }
                                }

                                AirAsiaTripResponceobject.inftcount = Inftcount;
                                AirAsiaTripResponceobject.inftbasefare = infanttotal;
                                AirAsiaTripResponceobject.inftbasefaretax = infanttax;

                                AirAsiaTripResponceobject.journeys = AAJourneyList;
                                AirAsiaTripResponceobject.passengers = passkeyList;
                                AirAsiaTripResponceobject.passengerscount = passengercount;
                                HttpContext.Session.SetString("AkasaAirItanary", JsonConvert.SerializeObject(AirAsiaTripResponceobject));
                                //string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");

                            }


                        }
                    }
                }
                #endregion

                #region Meals& Baggage
                string Akpassengerdata = HttpContext.Session.GetString("ResultFlightPassenger");
                AirAsiaTripResponceModel AKpasseengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(Akpassengerdata, typeof(AirAsiaTripResponceModel));
                int passengerscount = AKpasseengerKeyList.passengerscount;
                string departuredate = string.Empty;
                SSRAvailabiltyModel _AkasaSSRAvailabilty = new SSRAvailabiltyModel();
                _AkasaSSRAvailabilty.passengerKeys = new string[passengerscount];
                for (int i = 0; i < passengerscount; i++)
                {
                    _AkasaSSRAvailabilty.passengerKeys[i] = AKpasseengerKeyList.passengers[i].passengerKey;
                }
                _AkasaSSRAvailabilty.currencyCode = _AkasaSSRAvailabilty.currencyCode;

                List<Trip> AkasaTripslist = new List<Trip>();
                Trip AkasaTripobj = new Trip();
                AkasaTripobj.origin = AKpasseengerKeyList.journeys[0].designator.origin;
                List<TripIdentifier> AkasaTripIdentifierlist = new List<TripIdentifier>();
                TripIdentifier AkasaTripIdentifierobj = new TripIdentifier();
                AkasaTripIdentifierobj.carrierCode = AKpasseengerKeyList.journeys[0].segments[0].identifier.carrierCode;
                AkasaTripIdentifierobj.identifier = AKpasseengerKeyList.journeys[0].segments[0].identifier.identifier;
                AkasaTripIdentifierlist.Add(AkasaTripIdentifierobj);
                AkasaTripobj.identifier = AkasaTripIdentifierlist;
                AkasaTripslist.Add(AkasaTripobj);
                _AkasaSSRAvailabilty.trips = AkasaTripslist;
                var jsonAkasaSSRAvailabiltyRequest = JsonConvert.SerializeObject(_AkasaSSRAvailabilty, Formatting.Indented);
                SSRAvailabiltyResponceModel AkasaSSRAvailabiltyResponceobj = new SSRAvailabiltyResponceModel();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAkasaSSRAvailabilty = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirMealBaggage, _AkasaSSRAvailabilty);
                if (responseAkasaSSRAvailabilty.IsSuccessStatusCode)
                {
                    var _AkasaResponseSSRAvailabilty = responseAkasaSSRAvailabilty.Content.ReadAsStringAsync().Result;
                    var JsonAkasaSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_AkasaResponseSSRAvailabilty);
                    var journeyKey1 = JsonAkasaSSRAvailabilty.data.journeySsrs[0].journeyKey;
                    journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();
                    int JouneyBaggage = JsonAkasaSSRAvailabilty.data.journeySsrs.Count;
                    List<JourneyssrBaggage> AkjourneyssrBaggagesList = new List<JourneyssrBaggage>();
                    for (int k = 0; k < JouneyBaggage; k++)
                    {
                        JourneyssrBaggage AkjourneyssrBaggageObj = new JourneyssrBaggage();

                        AkjourneyssrBaggageObj.journeyBaggageKey = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyKey;
                        JourneyDetailsBaggage AkjourneydetailsBaggageObj = new JourneyDetailsBaggage();

                        AkjourneydetailsBaggageObj.origin = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyDetails.origin;
                        AkjourneydetailsBaggageObj.destination = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyDetails.destination;
                        AkjourneydetailsBaggageObj.departureDate = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyDetails.departureDate;

                        JBaggageIdentifier AkjBaggageIdentifierObj = new JBaggageIdentifier();
                        AkjBaggageIdentifierObj.identifier = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyDetails.identifier.identifier;
                        AkjBaggageIdentifierObj.carrierCode = JsonAkasaSSRAvailabilty.data.journeySsrs[k].journeyDetails.identifier.carrierCode;
                        AkjourneydetailsBaggageObj.identifier = AkjBaggageIdentifierObj;

                        int SSrCodeBaggageCount = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs.Count;
                        List<BaggageSsr> AkbaggageSsrsList = new List<BaggageSsr>();
                        for (int l = 0; l < SSrCodeBaggageCount; l++)
                        {
                            BaggageSsr AkbaggageSsrObj = new BaggageSsr();
                            AkbaggageSsrObj.ssrCode = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].ssrCode;
                            AkbaggageSsrObj.ssrType = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].ssrType;
                            AkbaggageSsrObj.name = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].name;
                            AkbaggageSsrObj.limitPerPassenger = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].limitPerPassenger;
                            AkbaggageSsrObj.available = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].available;
                            AkbaggageSsrObj.feeCode = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].feeCode;
                            AkbaggageSsrObj.seatRestriction = JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].seatRestriction;

                            List<PassengersAvailabilityBaggage> AkpassengersAvailabilityBaggageList = new List<PassengersAvailabilityBaggage>();
                            foreach (var itemObject in JsonAkasaSSRAvailabilty.data.journeySsrs[k].ssrs[l].passengersAvailability)
                            {
                                PassengersAvailabilityBaggage AkpassengersAvailabilityBaggageObj = new PassengersAvailabilityBaggage();
                                AkpassengersAvailabilityBaggageObj.passengerKey = itemObject.Value.passengerKey;
                                AkpassengersAvailabilityBaggageObj.Airline = Airlines.AkasaAir;
                                AkpassengersAvailabilityBaggageObj.price = itemObject.Value.price;
                                AkpassengersAvailabilityBaggageObj.ssrKey = itemObject.Value.ssrKey;
                                AkpassengersAvailabilityBaggageList.Add(AkpassengersAvailabilityBaggageObj);
                            }
                            AkbaggageSsrObj.passengersAvailabilityBaggage = AkpassengersAvailabilityBaggageList;
                            AkbaggageSsrsList.Add(AkbaggageSsrObj);

                        }
                        AkjourneyssrBaggageObj.journeydetailsBaggage = AkjourneydetailsBaggageObj;
                        AkjourneyssrBaggageObj.baggageSsr = AkbaggageSsrsList;
                        AkjourneyssrBaggagesList.Add(AkjourneyssrBaggageObj);

                    }
                    AkasaSSRAvailabiltyResponceobj.journeySsrsBaggage = AkjourneyssrBaggagesList;
                    HttpContext.Session.SetString("AKBaggageDetails", JsonConvert.SerializeObject(AkasaSSRAvailabiltyResponceobj));
                    int SegmentSSrcount = JsonAkasaSSRAvailabilty.data.segmentSsrs.Count;
                    int legSsrscount = JsonAkasaSSRAvailabilty.data.legSsrs.Count;
                    List<legSsrs> AkSSRAvailabiltyLegssrlist = new List<legSsrs>();
                    for (int i = 0; i < legSsrscount; i++)
                    {
                        legSsrs AkSSRAvailabiltyLegssrobj = new legSsrs();
                        AkSSRAvailabiltyLegssrobj.legKey = JsonAkasaSSRAvailabilty.data.legSsrs[i].legKey;
                        legDetails AklegDetailsobj = new legDetails();
                        AklegDetailsobj.destination = JsonAkasaSSRAvailabilty.data.legSsrs[i].legDetails.destination;
                        AklegDetailsobj.origin = JsonAkasaSSRAvailabilty.data.legSsrs[i].legDetails.origin;
                        AklegDetailsobj.departureDate = JsonAkasaSSRAvailabilty.data.legSsrs[i].legDetails.departureDate;

                        legidentifier Aklegidentifierobj = new legidentifier();
                        Aklegidentifierobj.identifier = JsonAkasaSSRAvailabilty.data.legSsrs[i].legDetails.identifier.identifier;
                        Aklegidentifierobj.carrierCode = JsonAkasaSSRAvailabilty.data.legSsrs[i].legDetails.identifier.carrierCode;
                        AklegDetailsobj.legidentifier = Aklegidentifierobj;

                        var ssrscount = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs.Count;
                        List<childlegssrs> Aklegssrslist = new List<childlegssrs>();
                        for (int j = 0; j < ssrscount; j++)
                        {
                            childlegssrs Aklegssrs = new childlegssrs();
                            Aklegssrs.ssrCode = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].ssrCode;
                            Aklegssrs.ssrType = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].ssrType;
                            Aklegssrs.name = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].name;
                            Aklegssrs.limitPerPassenger = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].limitPerPassenger;
                            Aklegssrs.available = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].available;
                            Aklegssrs.feeCode = JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].feeCode;
                            List<legpassengers> Aklegpassengerslist = new List<legpassengers>();
                            foreach (var items in JsonAkasaSSRAvailabilty.data.legSsrs[i].ssrs[j].passengersAvailability)
                            {
                                legpassengers Akpassengersdetail = new legpassengers();
                                Akpassengersdetail.passengerKey = items.Value.passengerKey;
                                Akpassengersdetail.Airline = Airlines.AkasaAir;
                                Akpassengersdetail.price = items.Value.price;
                                Akpassengersdetail.ssrKey = items.Value.ssrKey;
                                Aklegpassengerslist.Add(Akpassengersdetail);
                            }
                            Aklegssrs.legpassengers = Aklegpassengerslist;
                            Aklegssrslist.Add(Aklegssrs);
                        }
                        AkSSRAvailabiltyLegssrobj.legDetails = AklegDetailsobj;
                        AkSSRAvailabiltyLegssrobj.legssrs = Aklegssrslist;
                        AkSSRAvailabiltyLegssrlist.Add(AkSSRAvailabiltyLegssrobj);
                    }
                    AkasaSSRAvailabiltyResponceobj.legSsrs = AkSSRAvailabiltyLegssrlist;
                    AkasaSSRAvailabiltyResponceobj.SegmentSSrcount = SegmentSSrcount;
                    HttpContext.Session.SetString("AKMealsBaggage", JsonConvert.SerializeObject(AkasaSSRAvailabiltyResponceobj));

                }
                #endregion
                #region SeatMap
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage AkresponseSeatmap = await client.GetAsync(AppUrlConstant.AkasaAirSeatMap + journeyKey + "?IncludePropertyLookup=true");


                if (AkresponseSeatmap.IsSuccessStatusCode)
                {
                    //Logs logs = new Logs();
                    string columncount0 = string.Empty;
                    var _AkresponseSeatmap = AkresponseSeatmap.Content.ReadAsStringAsync().Result;
                    logs.WriteLogs("Url: " + JsonConvert.SerializeObject(AppUrlConstant.AkasaAirSeatMap + journeyKey + "?IncludePropertyLookup=true") + "\n\n Response: " + JsonConvert.SerializeObject(_AkresponseSeatmap), "SeatMap", "AkasaOneWay");
                    var JsonAkasaObjSeatmap = JsonConvert.DeserializeObject<dynamic>(_AkresponseSeatmap);
                    var uniquekey1 = JsonAkasaObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.units[0].unitKey;
                    var data = JsonAkasaObjSeatmap.data.Count;
                    List<data> Akdatalist = new List<data>();
                    SeatMapResponceModel AkSeatMapResponceModel = null;
                    int x = 0;
                    foreach (Match mitem in Regex.Matches(_AkresponseSeatmap, @"seatMap"":[\s\S]*?ssrLookup", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    {
                        data Akasadataobj = new data();
                        AkSeatMapResponceModel = new SeatMapResponceModel();
                        List<SeatMapResponceModel> AkSeatMapResponceModellist = new List<SeatMapResponceModel>();
                        Fees AkFees = new Fees();
                        Seatmap AkSeatmapobj = new Seatmap();
                        AkSeatmapobj.name = JsonAkasaObjSeatmap.data[x].seatMap.name;
                        AkSeatmapobj.arrivalStation = JsonAkasaObjSeatmap.data[x].seatMap.arrivalStation;
                        AkSeatmapobj.departureStation = JsonAkasaObjSeatmap.data[x].seatMap.departureStation;
                        AkSeatmapobj.marketingCode = JsonAkasaObjSeatmap.data[x].seatMap.marketingCode;
                        AkSeatmapobj.equipmentType = JsonAkasaObjSeatmap.data[x].seatMap.equipmentType;
                        AkSeatmapobj.equipmentTypeSuffix = JsonAkasaObjSeatmap.data[x].seatMap.equipmentTypeSuffix;
                        AkSeatmapobj.category = JsonAkasaObjSeatmap.data[x].seatMap.category;
                        AkSeatmapobj.seatmapReference = JsonAkasaObjSeatmap.data[x].seatMap.seatmapReference;
                        List<Unit> Akcompartmentsunitlist = new List<Unit>();
                        AkSeatmapobj.decksindigo = new List<Decks>();
                        Decks AkDecksobj = null;
                        //string strnewText = Regex.Match(_AkresponseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup""[\s\S]*?}]}\s",
                            //RegexOptions.IgnoreCase | RegexOptions.Multiline).Value.ToString();
                        string compartmenttext = Regex.Match(mitem.Value, "compartments\":(?<data>[\\s\\S]*?),\"seatmapReference", RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value.ToString();
                        foreach (Match itemn in Regex.Matches(compartmenttext, @"(?:availableunits|availableUnits)[\s\S]*?""designator"":""(?<t>[^\""""]+)""[\s\S]*?]}", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        {
                            string _compartmentblock = itemn.Groups["t"].Value.Trim();
                            Akcompartmentsunitlist = new List<Unit>();
                            AkDecksobj = new Decks();
                            AkDecksobj.availableUnits = JsonAkasaObjSeatmap.data[x].seatMap.availableUnits;
                            AkDecksobj.designator = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.designator;
                            AkDecksobj.length = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.length;
                            AkDecksobj.width = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.width;
                            AkDecksobj.sequence = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.sequence;
                            AkDecksobj.orientation = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.orientation;
                            AkSeatmapobj.decks = AkDecksobj;
                            int _count = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock]["units"].Count;
                            //List<Unit> Akcompartmentsunitlist = new List<Unit>();
                            for (int i1 = 0; i1 < JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock]["units"].Count; i1++)
                            {
                                Unit Akcompartmentsunitobj = new Unit();
                                Akcompartmentsunitobj.unitKey = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].unitKey;
                                Akcompartmentsunitobj.assignable = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].assignable;
                                Akcompartmentsunitobj.availability = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].availability;
                                Akcompartmentsunitobj.compartmentDesignator = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].compartmentDesignator;
                                Akcompartmentsunitobj.designator = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].designator;
                                Akcompartmentsunitobj.type = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].type;
                                Akcompartmentsunitobj.travelClassCode = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].travelClassCode;
                                Akcompartmentsunitobj.set = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].set;
                                Akcompartmentsunitobj.group = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].group;
                                Akcompartmentsunitobj.priority = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].priority;
                                Akcompartmentsunitobj.text = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].text;
                                Akcompartmentsunitobj.setVacancy = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].setVacancy;
                                Akcompartmentsunitobj.angle = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].angle;
                                Akcompartmentsunitobj.width = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].width;
                                Akcompartmentsunitobj.height = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].height;
                                Akcompartmentsunitobj.zone = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].zone;
                                Akcompartmentsunitobj.x = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].x;
                                Akcompartmentsunitobj.y = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].y;
                                //string a = JsonObjSeatmap.data[x].fees["MCFBRFQ-"].groups["1"].fees[0].serviceCharges[0].amount;

                                foreach (var strTextdata in Regex.Matches(mitem.Value, @"seatMap"":[\s\S]*?ssrLookup"))
                                {
                                    foreach (Match item in Regex.Matches(strTextdata.ToString(), @"fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup"))
                                    {
                                        foreach (var groupid in Regex.Matches(item.ToString(), @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                                        {

                                            string farearraygroupid = Regex.Match(groupid.ToString(), @"group"":(?<key>[\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["key"].Value;

                                            var feesgroupserviceChargescount = JsonAkasaObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges.Count;

                                            if (Akcompartmentsunitobj.group == Convert.ToInt32(farearraygroupid))
                                            {
                                                Akcompartmentsunitobj.servicechargefeeAmount = Convert.ToInt32(JsonAkasaObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges[0].amount);
                                                break;
                                            }
                                        }
                                    }
                                }

                                Akcompartmentsunitlist.Add(Akcompartmentsunitobj);
                                int compartmentypropertiesCount = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].properties.Count;
                                List<Properties> Propertieslist = new List<Properties>();
                                for (int j = 0; j < compartmentypropertiesCount; j++)
                                {
                                    Properties compartmentyproperties = new Properties();
                                    compartmentyproperties.code = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].properties[j].code;
                                    compartmentyproperties.value = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i1].properties[j].value;
                                    Propertieslist.Add(compartmentyproperties);
                                }
                                Akcompartmentsunitobj.properties = Propertieslist;
                                if (Akcompartmentsunitobj.designator.Contains('$'))
                                {
                                    columncount0 = JsonAkasaObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1 - 1].designator;
                                    break;
                                }
                                Akcompartmentsunitlist.Add(Akcompartmentsunitobj);
                            }
                            AkDecksobj.units = Akcompartmentsunitlist;
                            AkSeatmapobj.SeatColumnCount = Regex.Replace(columncount0, "[^0-9]", "");
                            AkSeatmapobj.decksindigo.Add(AkDecksobj);
                        }
                        Akasadataobj.seatMap = AkSeatmapobj;
                        Akdatalist.Add(Akasadataobj);
                        AkSeatMapResponceModel.datalist = Akdatalist;
                        x++;
                    }
                    HttpContext.Session.SetString("AKSeatmap", JsonConvert.SerializeObject(AkSeatMapResponceModel));
                }
                //}
                #endregion
            }
            return RedirectToAction("AkTripsellView", "AKTripsell");
        }
    }
}




