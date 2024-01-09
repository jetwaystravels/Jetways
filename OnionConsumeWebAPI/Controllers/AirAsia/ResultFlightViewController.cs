using System;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using static DomainLayer.Model.GetItenaryModel;
using static DomainLayer.Model.SeatMapResponceModel;
//using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers
{
    public class ResultFlightViewController : Controller
    {
        string passengerkey12 = string.Empty;
        public IActionResult FlightView()
        {
            var searchcount = TempData["count"];
            ViewData["count"] = searchcount;
            string OnewayFlightData = HttpContext.Session.GetString("OneWayFlightView");
            List<SimpleAvailibilityaAddResponce> OnewaydeserializedObjects = null;
            OnewaydeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(OnewayFlightData);
            return View(OnewaydeserializedObjects);
        }
        [HttpPost]
        public IActionResult FlightView(List<int> FilterId)
        {
            var searchcount = TempData["count"];
            ViewData["count"] = searchcount;
            string OnewayFlightData = HttpContext.Session.GetString("OneWayFlightView");
            List<SimpleAvailibilityaAddResponce> OnewaydeserializedObjects = null;
            OnewaydeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(OnewayFlightData);
            var FilterStopsData = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
            return View(FilterStopsData);

        }

        [HttpPost]
        public async Task<ActionResult> Tripsell(string fareKey, string journeyKey)
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
                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
                var jsonData = TempData["PassengerModel"];
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
                //key.inventoryControl = inventoryControl;
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
                HttpResponseMessage responseTripsell = await client.PostAsJsonAsync(AppUrlConstant.AirasiaTripsell, AirAsiaTripSellRequestobj);

                if (responseTripsell.IsSuccessStatusCode)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                    var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                    var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                    var totalAmount = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalAmount;
                    var totalTax = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalTax;


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
                }
                #region Itenary 
                if (infanttype != null)
                {

                    string passengerdatainfant = HttpContext.Session.GetString("keypassenger");
                    AirAsiaTripResponceModel passeengerKeyListinfant = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerdatainfant, typeof(AirAsiaTripResponceModel));

                    SimpleAvailabilityRequestModel _SimpleAvailabilityobject = new SimpleAvailabilityRequestModel();
                    var jsonDataObject = TempData["PassengerModel"];
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

                    var jsonPassengers = JsonConvert.SerializeObject(itenaryInfant, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.Airasiainfantquote, itenaryInfant);
                    if (responsePassengers.IsSuccessStatusCode)
                    {
                        AirAsiaTripResponceModel AirAsiaTripResponceobject = new AirAsiaTripResponceModel();
                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                        var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                        var TotalAmount = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalAmount;
                        var TotalTax = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalTax;
                        int Journeyscount = JsonObjPassengers.data.journeys.Count;




                        //end


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
                HttpResponseMessage responseSeatmap = await client.GetAsync(AppUrlConstant.Airasiaseatmap + journeyKey + "?IncludePropertyLookup=true");

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
                        List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();// to do
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
                            string a = JsonObjSeatmap.data[x].fees["MCFBRFQ-"].groups["1"].fees[0].serviceCharges[0].amount;
                            string strTextdata = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;


                            foreach (Match item in Regex.Matches(strTextdata, @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                            {
                                string farearraygroupid = Regex.Match(item.ToString(), @"group"":(?<key>[\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["key"].Value;

                                var feesgroupserviceChargescount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges.Count;

                                if (compartmentsunitobj.group == Convert.ToInt32(farearraygroupid))
                                {
                                    for (int l = 0; l < feesgroupserviceChargescount; l++)
                                    {
                                        compartmentsunitobj.servicechargefeeAmount += Convert.ToInt32(JsonObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges[l].Amount);
                                    }
                                }
                            }

                            //compartmentsunitobj.unitKey = compartmentsunitobj.designator;

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

                            GroupsFeeobj.groupid = myString1;
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
                //Tripobj.departureDate = passeengerKeyList.journeys[0].designator.departure; //commneted by vivek 09/01/2024 duyring code merge
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
                HttpResponseMessage responseSSRAvailabilty = await client.PostAsJsonAsync(AppUrlConstant.Airasiassravailability, _SSRAvailabilty);
                if (responseSSRAvailabilty.IsSuccessStatusCode)
                {

                    var _responseSSRAvailabilty = responseSSRAvailabilty.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);


                    //  var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                    // ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                    var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
                    journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();


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

            }
            return RedirectToAction("Tripsell", "AATripsell");

            // return View();
        }


    }


}
