using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;
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
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using Utility;
using static DomainLayer.Model.GetItenaryModel;
using static DomainLayer.Model.SeatMapResponceModel;
//using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers
{
    public class ResultFlightViewController : Controller
    {
        string passengerkey12 = string.Empty;
        string infant = string.Empty;
        Logs logs = new Logs();
        public IActionResult FlightView()
        {
            var searchcount = TempData["count"];
            ViewData["count"] = searchcount;
            ViewModel viewModelobject = new ViewModel();
            string OnewayFlightData = HttpContext.Session.GetString("OneWayFlightView");
            List<SimpleAvailibilityaAddResponce> OnewaydeserializedObjects = null;
            OnewaydeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(OnewayFlightData);
            viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
            string OneWayFlightEditData = HttpContext.Session.GetString("OneWayPassengerModel");
            SimpleAvailabilityRequestModel simpleAvailabilityRequestModel = null;
            if (!string.IsNullOrEmpty(OneWayFlightEditData))
            {
                simpleAvailabilityRequestModel = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(OneWayFlightEditData);
            }
            viewModelobject.simpleAvailabilityRequestModelEdit = simpleAvailabilityRequestModel;

            return View(viewModelobject);
        }

        [HttpPost]
        // Nonstop:: FilterId - Airline Filetr -
        public IActionResult GetFilteredFlights(string sortOrderName, List<string> FilterIdAirLine, List<int> FilterId, List<string> departure, List<string> arrival)
        {
            
            ViewModel viewModelobject = new ViewModel();
            string OnewayFlightData = HttpContext.Session.GetString("OneWayFlightView");
            List<SimpleAvailibilityaAddResponce> OnewaydeserializedObjects = null;
            OnewaydeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(OnewayFlightData);

          
            if (departure.Count > 0)
            {
                if (departure[0] == null)
                {
                    departure = departure.Where(d => d != null).ToList();
                    //departure = new List<string>();
                }
            }
            if (arrival.Count > 0)
            {
                if (arrival[0] == null)
                {
                    //arrival = new List<string>();
                    arrival = arrival.Where(d => d != null).ToList();
                }
            }


            if (string.IsNullOrEmpty(OnewayFlightData))
            {
                return View("Error");
            }
            List<SimpleAvailibilityaAddResponce> filteredFlights = OnewaydeserializedObjects;
            if (departure != null && departure.Count > 0)
            {
                filteredFlights = filteredFlights.Where(flight =>
                    departure.Any(d =>
                        (d.ToLower() == "before_6am" && flight.designator.departure.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (d.ToLower() == "6am_to_12pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (d.ToLower() == "12pm_to_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (d.ToLower() == "after_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();

                 OnewaydeserializedObjects = filteredFlights.ToList();
                viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;


            }
            if (arrival != null && arrival.Count > 0)
            {
                filteredFlights = filteredFlights.Where(flight =>
                    arrival.Any(a =>
                        (a.ToLower() == "before_6am" && flight.designator.arrival.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (a.ToLower() == "6am_to_12pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (a.ToLower() == "12pm_to_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (a.ToLower() == "after_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();
                 OnewaydeserializedObjects = filteredFlights.ToList();
                viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
            }
          
            ViewBag.NameSortParam = sortOrderName == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.PriceSortParam = sortOrderName == "price_desc" ? "price_asc" : "price_desc";
            ViewBag.DepartSortParam = sortOrderName == "deprt_desc" ? "deprt_asc" : "deprt_desc";
            ViewBag.arriveSortParam = sortOrderName == "arrive_desc" ? "arrive_asc" : "arrive_desc";
            ViewBag.durationSortParam = sortOrderName == "duration_desc" ? "duration_asc" : "duration_desc";

            switch (sortOrderName)
            {
                case "name_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(f => f.Airline.ToString()).ToList();
                    break;
                case "name_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(f => f.Airline.ToString()).ToList();
                    break;
                case "price_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(p => p.fareTotalsum).ToList();
                    break;
                case "price_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(p => p.fareTotalsum).ToList();
                    break;
                case "deprt_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.departure).ToList();
                    break;
                case "deprt_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.departure).ToList();
                    break;
                case "arrive_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.arrival).ToList();
                    break;
                case "arrive_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.arrival).ToList();
                    break;
                case "duration_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.formatTime).ToList();
                    break;
                case "duration_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.formatTime).ToList();
                    break;
                default:
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(p => p.fareTotalsum).ToList();
                    break;

            }

            if (FilterId != null && FilterId.Count > 0)
            {
                foreach (int value in FilterId)
                {
                    switch (value)
                    {
                        case 0:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        case 1:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        case 2:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        default:
                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                    }
                }

            }

          
            if (FilterIdAirLine.Count > 0 && FilterIdAirLine.Count >= 0)
            {
                 OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterIdAirLine.Contains(x.Airline.ToString())).ToList();
                viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
                
            }


           


            if (FilterId.Count > 0 && FilterId.Count >= 0)
            {
                viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
                //return PartialView("_FlightResultsSortingPartialView", viewModelobject);

            }


          
            viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
            //return PartialView("_FlightResultsSortingPartialView", viewModelobject);
            return PartialView("_FlightResultsSortingPartialView", viewModelobject);
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
                //var jsonData = TempData["PassengerModel"];
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
                    logs.WriteLogs("Url: " + AppUrlConstant.AirasiaTripsell + "Request: " + JsonConvert.SerializeObject(AirAsiaTripSellRequestobj) + "\n Response: " + resultsTripsell, "Tripsell", "AirAsiaOneWay");
                    var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                    var totalAmount = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalAmount;
                    var totalTax = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalTax;
                    var basefaretax = JsonObjTripsell.data.breakdown.journeyTotals.totalTax;



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
                            var cityname = Citynamelist.GetAllCityData().Where(x => x.citycode == "DEL");
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
                                // int count123= JsonObjTripsell.data.journeys[0].segments[0].fares[0].passengerFares.Count();

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

                        passkeylist.Add(passkeytypeobj);
                        passengerkey12 = passkeytypeobj.passengerKey;
                    }
                    #region  for passenger view list
                    for (int i = 0; i < _typeslist.Count; i++)
                    {
                        if (_typeslist[i].type == "INFT")
                        {
                            for (int i1 = 0; i1 < _typeslist[i].count; i1++)
                            {
                                AAPassengers passkeytypeobj = new AAPassengers();
                                passkeytypeobj.passengerKey = "";
                                passkeytypeobj.passengerTypeCode = "INFT";
                                passkeylist.Add(passkeytypeobj);
                            }

                        }
                    }
                    #endregion
                    AirAsiaTripResponceobj.basefaretax = basefaretax;

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
                    var jsonDataObject = HttpContext.Session.GetString("PassengerModel");
                    _SimpleAvailabilityobject = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonDataObject.ToString());
                    //var jsonDataObject = TempData["PassengerModel"];
                    //_SimpleAvailabilityobject = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonDataObject.ToString());

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
                    if (infant == "INFT")
                    {
                        var jsonPassengers = JsonConvert.SerializeObject(itenaryInfant, Formatting.Indented);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.Airasiainfantquote, itenaryInfant);
                        if (responsePassengers.IsSuccessStatusCode)
                        {
                            AirAsiaTripResponceModel AirAasiaobjectInfantdata = new AirAsiaTripResponceModel();
                            var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            logs.WriteLogs("Url: " + AppUrlConstant.Airasiainfantquote + "Request: " + JsonConvert.SerializeObject(itenaryInfant) + "\n Response: " + _responsePassengers, "itenaryInfant", "AirAsiaOneWay");
                            var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                            var TotalAmount = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalAmount;
                            var TotalTax = JsonObjPassengers.data.breakdown.journeys[journeyKey].totalTax;
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

                            int ServiceInfttax = 0;
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


                               // if (passkeytypeobject.passengerTypeCode != "CHD")
                              //  {

                                    if (JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant != null)
                                    {
                                        int Feecount = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees.Count;
                                        Inftcount += Feecount;
                                        Inftbasefare = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[0].serviceCharges[0].amount;
                                        var ServiceInft = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[0].serviceCharges;
                                        int ServiceInftcount = ((Newtonsoft.Json.Linq.JContainer)ServiceInft).Count;

                                        for (int inf = 1; inf < ServiceInftcount; inf++)
                                        {
                                            ServiceInfttax = JsonObjPassengers.data.passengers[passkeytypeobject.passengerKey].infant.fees[0].serviceCharges[inf].amount;
                                            ServiceInfttax += ServiceInfttax;
                                        }
                                        Inftbasefare = Inftbasefare - ServiceInfttax;
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
                               // }

                                AirAasiaobjectInfantdata.inftcount = Inftcount;
                                AirAasiaobjectInfantdata.inftbasefare = Inftbasefare;
                                AirAasiaobjectInfantdata.inftbasefaretax = ServiceInfttax;

                                AirAasiaobjectInfantdata.journeys = AAJourneyList;
                                AirAasiaobjectInfantdata.passengers = passkeyList;
                                AirAasiaobjectInfantdata.passengerscount = passengercount;
                                HttpContext.Session.SetString("InfantData", JsonConvert.SerializeObject(AirAasiaobjectInfantdata));
                                //string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");

                            }


                        }
                    }
                }
                #endregion
                #region SeatMap
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseSeatmap = await client.GetAsync(AppUrlConstant.Airasiaseatmap + journeyKey + "?IncludePropertyLookup=true");
                if (responseSeatmap.IsSuccessStatusCode)
                {
                    string passengerInfant = HttpContext.Session.GetString("InfantData");
                    string columncount0 = string.Empty;
                    Logs logs = new Logs();
                    var _responseSeatmap = responseSeatmap.Content.ReadAsStringAsync().Result;
                    logs.WriteLogs("Url: " + JsonConvert.SerializeObject(AppUrlConstant.Airasiaseatmap + journeyKey + "?IncludePropertyLookup=true") + "\n\n Response: " + JsonConvert.SerializeObject(_responseSeatmap), "SeatMap", "AirAsiaOneWay");
                    var JsonObjSeatmap = JsonConvert.DeserializeObject<dynamic>(_responseSeatmap);
                    //var uniquekey1 = JsonObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.units[0].unitKey;
                    var data = JsonObjSeatmap.data.Count;
                    List<data> datalist = new List<data>();
                    SeatMapResponceModel SeatMapResponceModel = null;
                    int x = 0;
                    foreach (Match mitem in Regex.Matches(_responseSeatmap, @"seatMap"":[\s\S]*?ssrLookup", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    {
                        data dataobj = new data();

                        SeatMapResponceModel = new SeatMapResponceModel();
                        List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();
                        Fees Fees = new Fees();
                        Seatmap Seatmapobj = new Seatmap();
                        Seatmapobj.name = JsonObjSeatmap.data[x].seatMap.name;
                        TempData["AirLineName"] = Seatmapobj.name;
                        Seatmapobj.arrivalStation = JsonObjSeatmap.data[x].seatMap.arrivalStation;
                        Seatmapobj.departureStation = JsonObjSeatmap.data[x].seatMap.departureStation;
                        Seatmapobj.marketingCode = JsonObjSeatmap.data[x].seatMap.marketingCode;
                        Seatmapobj.equipmentType = JsonObjSeatmap.data[x].seatMap.equipmentType;
                        Seatmapobj.equipmentTypeSuffix = JsonObjSeatmap.data[x].seatMap.equipmentTypeSuffix;
                        Seatmapobj.category = JsonObjSeatmap.data[x].seatMap.category;
                        Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;
                        List<Unit> compartmentsunitlist = new List<Unit>();
                        Seatmapobj.decksindigo = new List<Decks>();
                        Decks Decksobj = null;
                        //string strnewText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup""[\s\S]*?}]}\s",
                            //RegexOptions.IgnoreCase | RegexOptions.Multiline).Value.ToString();
                        string compartmenttext = Regex.Match(mitem.Value, "compartments\":(?<data>[\\s\\S]*?),\"seatmapReference", RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value.ToString();
                        foreach (Match itemn in Regex.Matches(compartmenttext, @"availableunits[\s\S]*?""designator"":""(?<t>[^\""""]+)""[\s\S]*?]}]", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        {
                            string _compartmentblock = itemn.Groups["t"].Value.Trim();
                            compartmentsunitlist = new List<Unit>();
                            Decksobj = new Decks();
                            Decksobj.availableUnits = JsonObjSeatmap.data[x].seatMap.availableUnits;
                            Decksobj.designator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].designator;
                            Decksobj.length = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].length;
                            Decksobj.width = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].width;
                            Decksobj.sequence = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].sequence;
                            Decksobj.orientation = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].orientation;
                            Seatmapobj.decks = Decksobj;
                            int _count = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock]["units"].Count;
                            for (int i1 = 0; i1 < JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock]["units"].Count; i1++)
                            {
                                try
                                {
                                    Unit compartmentsunitobj = new Unit();
                                    compartmentsunitobj.unitKey = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].unitKey;
                                    compartmentsunitobj.assignable = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].assignable;
                                    compartmentsunitobj.availability = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].availability;
                                    compartmentsunitobj.compartmentDesignator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].compartmentDesignator;
                                    compartmentsunitobj.designator = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].designator;
                                    compartmentsunitobj.type = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].type;
                                    compartmentsunitobj.travelClassCode = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].travelClassCode;
                                    compartmentsunitobj.set = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].set;
                                    compartmentsunitobj.group = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].group;
                                    compartmentsunitobj.priority = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].priority;
                                    compartmentsunitobj.text = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].text;
                                    compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].setVacancy;
                                    compartmentsunitobj.angle = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].angle;
                                    compartmentsunitobj.width = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].width;
                                    compartmentsunitobj.height = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].height;
                                    compartmentsunitobj.zone = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].zone;
                                    compartmentsunitobj.x = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].x;
                                    compartmentsunitobj.y = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].y;

                                    foreach (var strTextdata in Regex.Matches(mitem.Value, @"seatMap"":[\s\S]*?ssrLookup"))
                                    {
                                        foreach (Match item in Regex.Matches(strTextdata.ToString(), @"fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup"))
                                        {
                                            foreach (var groupid in Regex.Matches(item.ToString(), @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                                            {

                                                string farearraygroupid = Regex.Match(groupid.ToString(), @"group"":(?<key>[\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["key"].Value;

                                                var feesgroupserviceChargescount = JsonObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges.Count;

                                                if (compartmentsunitobj.group == Convert.ToInt32(farearraygroupid))
                                                {
                                                    compartmentsunitobj.servicechargefeeAmount = Convert.ToInt32(JsonObjSeatmap.data[x].fees[passengerkey12].groups[farearraygroupid].fees[0].serviceCharges[0].amount);
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    compartmentsunitlist.Add(compartmentsunitobj);
                                    int compartmentypropertiesCount = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].properties.Count;
                                    List<Properties> Propertieslist = new List<Properties>();
                                    for (int j = 0; j < compartmentypropertiesCount; j++)
                                    {
                                        Properties compartmentyproperties = new Properties();
                                        compartmentyproperties.code = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].properties[j].code;
                                        compartmentyproperties.value = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1].properties[j].value;
                                        Propertieslist.Add(compartmentyproperties);
                                    }
                                    compartmentsunitobj.properties = Propertieslist;
                                    if (compartmentsunitobj.designator.Contains('$'))
                                    {
                                        columncount0 = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments[_compartmentblock].units[i1 - 1].designator;
                                        break;
                                    }

                                    compartmentsunitlist.Add(compartmentsunitobj);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            Decksobj.units = compartmentsunitlist;
                            Seatmapobj.SeatColumnCount = Regex.Replace(columncount0, "[^0-9]", "");
                            Seatmapobj.decksindigo.Add(Decksobj);
                        }//end here foreach
                        dataobj.seatMap = Seatmapobj;
                        datalist.Add(dataobj);
                        SeatMapResponceModel.datalist = datalist;

                        x++;
                    }
                    HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));
                }
                //}
                #endregion
                #region Meals -Baggage

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
                Tripobj.destination = passeengerKeyList.journeys[0].designator.destination;
                //Tripobj.departureDate = passeengerKeyList.journeys[0].designator.departure;
                Tripobj.departureDate = passeengerKeyList.journeys[0].designator.departure.ToString("yyyy-MM-dd HH:mm:ss");


                List<TripIdentifier> TripIdentifierlist = new List<TripIdentifier>();
                TripIdentifier TripIdentifierobj = new TripIdentifier();
                TripIdentifierobj.carrierCode = passeengerKeyList.journeys[0].segments[0].identifier.carrierCode;
                TripIdentifierobj.identifier = passeengerKeyList.journeys[0].segments[0].identifier.identifier;
                TripIdentifierlist.Add(TripIdentifierobj);
                Tripobj.identifier = TripIdentifierlist;
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
                    logs.WriteLogs("Url: " + JsonConvert.SerializeObject(AppUrlConstant.Airasiassravailability) + "Request: " + jsonSSRAvailabiltyRequest+ "\n\n Response: " + JsonConvert.SerializeObject(_responseSSRAvailabilty), "SSrAvailability", "AirAsiaOneWay");
                    var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);
                    var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
                    journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();

                    #region Segment 
                    int SegmentBaggageCount = JsonObjresponseSSRAvailabilty.data.segmentSsrs.Count;
                    List<segmentSsrs> segmentSsrsList = new List<segmentSsrs>();
                    for (int sj = 0; sj < SegmentBaggageCount; sj++)
                    {
                        segmentSsrs segmentobj = new segmentSsrs();
                        segmentobj.segmentKey = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentKey;
                        segmentDetails segmentDetailsobj = new segmentDetails();
                        segmentDetailsobj.origin = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentDetails.origin;
                        segmentDetailsobj.destination = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentDetails.destination;
                        segmentDetailsobj.departureDate = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentDetails.departureDate;

                        segidentifier segidentifierobj = new segidentifier();
                        segidentifierobj.identifier = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentDetails.identifier.identifier;
                        segidentifierobj.carrierCode = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].segmentDetails.identifier.carrierCode;
                        segmentDetailsobj.segmentidentifier = segidentifierobj;

                        int SegBaggageCount = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs.Count;
                        List<segSsrs> SegSsrsList = new List<segSsrs>();
                        for (int seg = 0; seg < SegBaggageCount; seg++)
                        {
                            segSsrs SegSsrObj = new segSsrs();
                            SegSsrObj.ssrCode = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].ssrCode;
                            SegSsrObj.ssrType = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].ssrType;
                            SegSsrObj.name = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].name;
                            SegSsrObj.limitPerPassenger = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].limitPerPassenger;
                            SegSsrObj.available = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].available;
                            SegSsrObj.feeCode = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].feeCode;
                            SegSsrObj.seatRestriction = JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].seatRestriction;

                            List<segpassengers> passengersSegBaggageList = new List<segpassengers>();
                            foreach (var itemObject in JsonObjresponseSSRAvailabilty.data.segmentSsrs[sj].ssrs[seg].passengersAvailability)
                            {
                                segpassengers Segpassengerobj = new segpassengers();
                                Segpassengerobj.passengerKey = itemObject.Value.passengerKey;
                                Segpassengerobj.price = itemObject.Value.price;
                                Segpassengerobj.ssrKey = itemObject.Value.ssrKey;
                                passengersSegBaggageList.Add(Segpassengerobj);
                            }
                            SegSsrObj.segpassengers = passengersSegBaggageList;
                            SegSsrsList.Add(SegSsrObj);

                        }
                        segmentobj.segmentDetails = segmentDetailsobj;
                        segmentobj.segmentssrs = SegSsrsList;
                        segmentSsrsList.Add(segmentobj);

                    }
                    SSRAvailabiltyResponceobj.segmentSsrs = segmentSsrsList;
                    HttpContext.Session.SetString("BaggageDetailsData", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));


                    #endregion
                    int JouneyBaggage = JsonObjresponseSSRAvailabilty.data.journeySsrs.Count;
                    List<JourneyssrBaggage> journeyssrBaggagesList = new List<JourneyssrBaggage>();
                    for (int k = 0; k < JouneyBaggage; k++)
                    {
                        JourneyssrBaggage journeyssrBaggageObj = new JourneyssrBaggage();

                        journeyssrBaggageObj.journeyBaggageKey = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyKey;
                        JourneyDetailsBaggage journeydetailsBaggageObj = new JourneyDetailsBaggage();

                        journeydetailsBaggageObj.origin = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyDetails.origin;
                        journeydetailsBaggageObj.destination = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyDetails.destination;
                        journeydetailsBaggageObj.departureDate = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyDetails.departureDate;

                        JBaggageIdentifier jBaggageIdentifierObj = new JBaggageIdentifier();
                        jBaggageIdentifierObj.identifier = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyDetails.identifier.identifier;
                        jBaggageIdentifierObj.carrierCode = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].journeyDetails.identifier.carrierCode;
                        journeydetailsBaggageObj.identifier = jBaggageIdentifierObj;

                        int SSrCodeBaggageCount = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs.Count;
                        List<BaggageSsr> baggageSsrsList = new List<BaggageSsr>();
                        for (int l = 0; l < SSrCodeBaggageCount; l++)
                        {
                            BaggageSsr baggageSsrObj = new BaggageSsr();
                            baggageSsrObj.ssrCode = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].ssrCode;
                            baggageSsrObj.ssrType = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].ssrType;
                            baggageSsrObj.name = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].name;
                            baggageSsrObj.limitPerPassenger = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].limitPerPassenger;
                            baggageSsrObj.available = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].available;
                            baggageSsrObj.feeCode = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].feeCode;
                            baggageSsrObj.seatRestriction = JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].seatRestriction;

                            List<PassengersAvailabilityBaggage> passengersAvailabilityBaggageList = new List<PassengersAvailabilityBaggage>();
                            foreach (var itemObject in JsonObjresponseSSRAvailabilty.data.journeySsrs[k].ssrs[l].passengersAvailability)
                            {
                                PassengersAvailabilityBaggage passengersAvailabilityBaggageObj = new PassengersAvailabilityBaggage();
                                passengersAvailabilityBaggageObj.passengerKey = itemObject.Value.passengerKey;
                                passengersAvailabilityBaggageObj.price = itemObject.Value.price;
                                passengersAvailabilityBaggageObj.ssrKey = itemObject.Value.ssrKey;
                                passengersAvailabilityBaggageList.Add(passengersAvailabilityBaggageObj);
                            }
                            baggageSsrObj.passengersAvailabilityBaggage = passengersAvailabilityBaggageList;
                            baggageSsrsList.Add(baggageSsrObj);

                        }
                        journeyssrBaggageObj.journeydetailsBaggage = journeydetailsBaggageObj;
                        journeyssrBaggageObj.baggageSsr = baggageSsrsList;
                        journeyssrBaggagesList.Add(journeyssrBaggageObj);

                    }
                    SSRAvailabiltyResponceobj.journeySsrsBaggage = journeyssrBaggagesList;
                    HttpContext.Session.SetString("BaggageDetails", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));
                    int SegmentSSrcount = JsonObjresponseSSRAvailabilty.data.segmentSsrs.Count;

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
                    SSRAvailabiltyResponceobj.SegmentSSrcount = SegmentSSrcount;
                    HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));

                }
                #endregion
            }
            return RedirectToAction("Tripsell", "AATripsell");
        }

    }
}

