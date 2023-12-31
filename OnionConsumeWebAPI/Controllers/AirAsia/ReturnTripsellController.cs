﻿using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Bookingmanager_;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using Utility;
using static DomainLayer.Model.SeatMapResponceModel;
namespace OnionConsumeWebAPI.Controllers
{
    public class ReturnTripsellController : Controller
    {

        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string passengerkey12 = string.Empty;
        Logs logs = new Logs();
        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
        public async Task<IActionResult> ReturnTripsellView(List<string> fareKey, List<string> journeyKey)
             {

            bool AirLinefound1 = fareKey.Any(s => s.Contains("Airasia"));
            bool AirLinefound2 = fareKey.Any(s => s.Contains("Spicejet"));
            if (AirLinefound1==true && AirLinefound2==false)
            {
                HttpContext.Session.Remove("SpicejetSignautre");
            }
            else if (AirLinefound1 == false && AirLinefound2 == true)
            {
                HttpContext.Session.Remove("AirasiaTokan");
            }

            List<string> MainPassengerdata = new List<string>();
            List<string> MainSeatMapdata = new List<string>();
            List<string> MainMealsdata = new List<string>();

            List<string> Passengerdata = new List<string>();
            List<string> SeatMapdata = new List<string>();
            List<string> Mealsdata = new List<string>();

            List<string> _Passengerdata = new List<string>();
            List<string> _SeatMapdata = new List<string>();
            List<string> _Mealsdata = new List<string>();
            for (int p = 0; p < 2; p++)
            {
                #region AirAsia
                string token = string.Empty;
                AirAsiaTripResponceModel AirAsiaTripResponceobj = null;
                List<_credentials> credentialslist = new List<_credentials>();
                string _JourneykeyData = string.Empty;
                string _FareKeyData = string.Empty;

                //
                string _JourneyKeyOneway = journeyKey[p];
                string[] _Jparts = _JourneyKeyOneway.Split('@');
                string _JourneykeyRTData = _Jparts[2];


                if (_JourneykeyRTData.ToLower() == "airasia")
                {
                    var a = 1;
                }
                if (_JourneykeyRTData.ToLower() == "spicejet")
                {
                    var b = 2;

                }

                //

                using (HttpClient client = new HttpClient())
                {
                    if (_JourneykeyRTData.ToLower() == "airasia")
                    {
                        string tokenview = HttpContext.Session.GetString("AirasiaTokan");
                        if (tokenview == "" || tokenview == null)
                        {
                            return RedirectToAction("Index");
                        }
                        token = tokenview.Replace(@"""", string.Empty);

                        HttpContext.Session.SetString("journeyKey", JsonConvert.SerializeObject(journeyKey));
                        string Leftshowpopupdata = HttpContext.Session.GetString("PassengerModel");
                        //SimpleAvailabilityRequestModel _SimpleAvailabilityobj = null;
                        SimpleAvailabilityRequestModel _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(Leftshowpopupdata);
                        //SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
                        //var jsonData = TempData["PassengerModel"];
                        //_SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());
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


                        string JourneyKeyOneway = journeyKey[p];
                        string[] Jparts = JourneyKeyOneway.Split('@');
                        _JourneykeyData = Jparts[0];
                        key.journeyKey = _JourneykeyData;

                        HttpContext.Session.SetString("journeySellKeyAA", JsonConvert.SerializeObject(_JourneykeyData));

                        string fareKeyKeyOneway = fareKey[p];
                        string[] Fparts = fareKeyKeyOneway.Split('@');
                        _FareKeyData = Fparts[0];
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
                                _Types.type = infanttype;
                                _Types.count = _SimpleAvailabilityobj.passengers.types[i].count;
                                //continue;
                            }
                            //	
                            _typeslist.Add(_Types);
                        }

                        //passengers.types = _typeslist;

                        List<_Types> _typeslistsell = new List<_Types>();
                        for (int i = 0; i < _typeslist.Count; i++)
                        {
                            if (_typeslist[i].type == "INFT")
                                continue;
                            _typeslistsell.Add(_typeslist[i]);

                        }
                        passengers.types = _typeslistsell;

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
                            AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                            var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                            var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                            //var totalAmount = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalAmount;
                            // var totalTax = JsonObjTripsell.data.breakdown.journeys[journeyKey].totalTax;


                            int journeyscount = JsonObjTripsell.data.journeys.Count;
                            List<AAJourney> AAJourneyList = new List<AAJourney>();
                            for (int i = 0; i < journeyscount; i++)
                            {
                                if (journeyscount == 2 && i == 0)
                                {
                                    continue;
                                }

                                AAJourney AAJourneyobj = new AAJourney();
                                AAJourneyobj.Airlinename = Airlines.Airasia.ToString();
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
                                    //var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
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
                                        //passengerkey12 = passkeytypeobj.passengerKey;
                                    }

                                }
                            }
                            #endregion

                            AirAsiaTripResponceobj.journeys = AAJourneyList;
                            AirAsiaTripResponceobj.passengers = passkeylist;
                            AirAsiaTripResponceobj.passengerscount = passengercount;

                            _Passengerdata.Add("<Start>" + JsonConvert.SerializeObject(AirAsiaTripResponceobj) + "<End>");
                            HttpContext.Session.SetString("keypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));
                            HttpContext.Session.SetString("_keypassengerdata", JsonConvert.SerializeObject(_Passengerdata));
                            
                            if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(_Passengerdata)))
                            {
                                if (_Passengerdata.Count == 2)
                                {
                                    MainPassengerdata = new List<string>();
                                }
                                MainPassengerdata.Add(JsonConvert.SerializeObject(_Passengerdata));
                            }
                            #region Itenary 

                            if (infanttype != null && infanttype != "")
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
                                Keyobj.journeyKey = journeyKey[0];
                                Keyobj.fareAvailabilityKey = fareKey[0];
                                JourneyKeyOneway = journeyKey[p];
                                Jparts = JourneyKeyOneway.Split('@');
                                _JourneykeyData = Jparts[0];
                                Keyobj.journeyKey = _JourneykeyData;


                                fareKeyKeyOneway = fareKey[p];
                                Fparts = fareKeyKeyOneway.Split('@');
                                _FareKeyData = Fparts[0];
                                Keyobj.fareAvailabilityKey = _FareKeyData;

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
                                    var TotalAmount = JsonObjPassengers.data.breakdown.journeys[_JourneykeyData].totalAmount;
                                    var TotalTax = JsonObjPassengers.data.breakdown.journeys[_JourneykeyData].totalTax;
                                    int Journeyscount = JsonObjPassengers.data.journeys.Count;




                                    //end


                                    AAJourneyList = new List<AAJourney>();
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
                                    passengercount = ((Newtonsoft.Json.Linq.JContainer)Passanger).Count;
                                    List<AAPassengers> passkeyList = new List<AAPassengers>();
                                    Infant infantobject = null;
                                    DomainLayer.Model.Fee feeobject = null;
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
                                                List<DomainLayer.Model.Fee> feeList = new List<DomainLayer.Model.Fee>();
                                                for (int i = 0; i < Feecount; i++)
                                                {
                                                    infantobject = new Infant();
                                                    feeobject = new DomainLayer.Model.Fee();
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
                        }
                    }






                    #endregion

                    //Spicejet

                    #region SpiceJetSellRequest
                    //for (int Rtidx = 0; Rtidx < 2; Rtidx++)
                    //{
                    string Signature = string.Empty;
                    int TotalCount = 0;
                    //SimpleAvailabilityRequestModel _SimpleAvailabilityobj = null;
                    //AirAsiaTripResponceModel AirAsiaTripResponceobj = null;
                    if (_JourneykeyRTData.ToLower() == "spicejet")
                    {
                        Signature = HttpContext.Session.GetString("SpicejetSignautre");
                        Signature = Signature.Replace(@"""", string.Empty);
                        int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                        int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                        int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                        TotalCount = adultcount + childcount;
                        SellResponse _getSellRS = null;
                        SellRequest _getSellRQ = null;
                        _getSellRQ = new SellRequest();
                        _getSellRQ.SellRequestData = new SellRequestData(); ;
                        _getSellRQ.Signature = Signature;
                        _getSellRQ.ContractVersion = 420;
                        _getSellRQ.SellRequestData.SellBy = SellBy.JourneyBySellKey;


                        //string JourneyKeyreturnway = journeyKey[1];
                        //string[] JRTparts = JourneyKeyreturnway.Split('@');
                        _JourneykeyData = _Jparts[0];


                        string fareKeyRTway = fareKey[p];
                        string[] FRTparts = fareKeyRTway.Split('@');
                        _FareKeyData = FRTparts[0];

                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest = new SellJourneyByKeyRequest();
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new SellJourneyByKeyRequestData();

                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.CurrencyCode = "INR";

                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new SellKeyList[1];
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new SellKeyList();

                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = _JourneykeyData;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = _FareKeyData;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType = getPaxdetails(adultcount, childcount, 0);
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS = GetPointOfSale();

                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCountSpecified = true;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCount = Convert.ToInt16(TotalCount);
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFare = false;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PreventOverLap = false;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ReplaceAllPassengersOnUpdate = false;
                        _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ApplyServiceBundle = ApplyServiceBundle.No;
                        _getSellRQ.SellRequestData.SellSSR = new SellSSR();
                        _getSellRS = await objSpiceJet.GetSellAsync(_getSellRQ);

                        string str = JsonConvert.SerializeObject(_getSellRS);

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest");


                        #endregion

                        #region SpiceJet ItenaryRequest
                        string stravailibitilityrequest = HttpContext.Session.GetString("SpicejetAvailibilityRequest");
                        GetAvailabilityRequest availibiltyRQ = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                        PriceItineraryResponse _getPriceItineraryRS = null;
                        PriceItineraryRequest _getPriceItineraryRQ = null;
                        _getPriceItineraryRQ = new PriceItineraryRequest();
                        _getPriceItineraryRQ.ItineraryPriceRequest = new ItineraryPriceRequest();
                        _getPriceItineraryRQ.Signature = Signature;
                        _getPriceItineraryRQ.ContractVersion = 420;
                        _getPriceItineraryRQ.ItineraryPriceRequest.PriceItineraryBy = PriceItineraryBy.JourneyBySellKey;

                        _getPriceItineraryRQ.ItineraryPriceRequest.BookingStatus = default;
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest = new SellJourneyByKeyRequestData();
                        SellKeyList _getSellKeyList = new SellKeyList();
                        _getSellKeyList.JourneySellKey = _JourneykeyData;
                        _getSellKeyList.FareSellKey = _FareKeyData;
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys = new SellKeyList[1];
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0] = new SellKeyList();
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].JourneySellKey = _getSellKeyList.JourneySellKey;
                        //"SG~8169~ ~~DEL~12/10/2023 20:00~BOM~12/10/2023 22:05~~";
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].FareSellKey = _getSellKeyList.FareSellKey;
                        //"0~V~ ~SG~VSAV~5511~~0~6~~X";
                        // Changes for Adult child infant
                        //int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                        //int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                        //int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                        //int TotalCount = adultcount + childcount;
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxCount = Convert.ToInt16(TotalCount);
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.CurrencyCode = "INR";

                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxPriceType = getPaxdetails(adultcount, childcount, 0);
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.SourcePOS = GetPointOfSale();
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
                        _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.IsAllotmentMarketFare = false;
                        _getPriceItineraryRQ.ItineraryPriceRequest.SSRRequest = new SSRRequest();
                        _getPriceItineraryRS = await objSpiceJet.GetItineraryPriceAsync(_getPriceItineraryRQ);

                        str = JsonConvert.SerializeObject(_getPriceItineraryRS);

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getPriceItineraryRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getPriceItineraryRS), "PriceIteniry");


                        #endregion

                        HttpContext.Session.SetString("journeySellKey", JsonConvert.SerializeObject(_JourneykeyData));
                        SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();

                        var jsonData = TempData["SpiceJetPassengerModel"];
                        _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());

                        if (_getPriceItineraryRS != null)
                        {
                            AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                            //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                            //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                            var totalAmount = _getPriceItineraryRS.Booking.BookingSum.TotalCost;

                            var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

                            #region Itenary segment and legs
                            int journeyscount = _getPriceItineraryRS.Booking.Journeys.Length;
                            List<AAJourney> AAJourneyList = new List<AAJourney>();
                            for (int i = 0; i < journeyscount; i++)
                            {

                                AAJourney AAJourneyobj = new AAJourney();
                                AAJourneyobj.Airlinename = Airlines.Spicejet.ToString();
                                //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                                //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                                AAJourneyobj.journeyKey = _getPriceItineraryRS.Booking.Journeys[i].JourneySellKey;

                                int segmentscount = _getPriceItineraryRS.Booking.Journeys[i].Segments.Length;
                                List<AASegment> AASegmentlist = new List<AASegment>();
                                for (int j = 0; j < segmentscount; j++)
                                {
                                    AADesignator AADesignatorobj = new AADesignator();
                                    AADesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[0].DepartureStation;
                                    AADesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[segmentscount - 1].ArrivalStation;
                                    AADesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[0].STD;
                                    AADesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[segmentscount - 1].STA;
                                    AAJourneyobj.designator = AADesignatorobj;


                                    AASegment AASegmentobj = new AASegment();
                                    //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                                    //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                                    AADesignator AASegmentDesignatorobj = new AADesignator();

                                    AASegmentDesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].DepartureStation;
                                    AASegmentDesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].ArrivalStation;
                                    AASegmentDesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].STD;
                                    AASegmentDesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].STA;
                                    AASegmentobj.designator = AASegmentDesignatorobj;

                                    int fareCount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares.Length;
                                    List<AAFare> AAFarelist = new List<AAFare>();
                                    for (int k = 0; k < fareCount; k++)
                                    {
                                        AAFare AAFareobj = new AAFare();
                                        AAFareobj.fareKey = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                        AAFareobj.productClass = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                        var passengerFares = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                        int passengerFarescount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                        List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                                        for (int l = 0; l < passengerFarescount; l++)
                                        {
                                            AAPassengerfare AAPassengerfareobj = new AAPassengerfare();
                                            AAPassengerfareobj.passengerType = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                            var serviceCharges1 = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                            int serviceChargescount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                            List<AAServicecharge> AAServicechargelist = new List<AAServicecharge>();
                                            for (int m = 0; m < serviceChargescount; m++)
                                            {
                                                AAServicecharge AAServicechargeobj = new AAServicecharge();

                                                AAServicechargeobj.amount = Convert.ToInt32(_getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);



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

                                    AAIdentifierobj.identifier = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                    AAIdentifierobj.carrierCode = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                    AASegmentobj.identifier = AAIdentifierobj;

                                    var leg = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs;
                                    int legcount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs.Length;
                                    List<AALeg> AALeglist = new List<AALeg>();
                                    for (int n = 0; n < legcount; n++)
                                    {
                                        AALeg AALeg = new AALeg();
                                        //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                        AADesignator AAlegDesignatorobj = new AADesignator();
                                        AAlegDesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                        AAlegDesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                        AAlegDesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                        AAlegDesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                        AALeg.designator = AAlegDesignatorobj;

                                        AALeginfo AALeginfoobj = new AALeginfo();
                                        AALeginfoobj.arrivalTerminal = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                        AALeginfoobj.arrivalTime = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                        AALeginfoobj.departureTerminal = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                        AALeginfoobj.departureTime = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
                                        AALeg.legInfo = AALeginfoobj;
                                        AALeglist.Add(AALeg);

                                    }

                                    AASegmentobj.legs = AALeglist;


                                    AASegmentlist.Add(AASegmentobj);




                                }



                                AAJourneyobj.segments = AASegmentlist;


                                AAJourneyList.Add(AAJourneyobj);

                            }

                            #endregion
                            var passanger = _getPriceItineraryRS.Booking.Passengers;
                            int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;

                            List<AAPassengers> passkeylist = new List<AAPassengers>();
                            int a = 0;
                            foreach (var items in availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes)
                            {
                                for (int i = 0; i < items.PaxCount; i++)
                                {

                                    AAPassengers passkeytypeobj = new AAPassengers();
                                    passkeytypeobj.passengerKey = a.ToString();
                                    //if (items.PaxType == "ADT")
                                    //{
                                    passkeytypeobj.passengerTypeCode = items.PaxType;
                                    //}

                                    passkeylist.Add(passkeytypeobj);
                                    a++;
                                }
                                //passengerkey12 = passkeytypeobj.passengerKey;


                            }

                            AirAsiaTripResponceobj.journeys = AAJourneyList;
                            AirAsiaTripResponceobj.passengers = passkeylist;
                            AirAsiaTripResponceobj.passengerscount = passengercount;
                            
                            Passengerdata.Add("<Start>"+JsonConvert.SerializeObject(AirAsiaTripResponceobj)+"<End>");
                            HttpContext.Session.SetString("SGkeypassengerRT", JsonConvert.SerializeObject(AirAsiaTripResponceobj));
                            HttpContext.Session.SetString("keypassengerdata", JsonConvert.SerializeObject(Passengerdata));

                            if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(Passengerdata)))
                            {
                                if (Passengerdata.Count == 2)
                                {
                                    MainPassengerdata = new List<string>();
                                }
                                MainPassengerdata.Add(JsonConvert.SerializeObject(Passengerdata));
                            }

                        }
                    }
                    //}

                    #region SeatMap

                    //AirAsia SeatMap
                    #region SeatMap
                    if (_JourneykeyRTData.ToLower() == "airasia")
                    {
                        string _JourneykeyDataAA = HttpContext.Session.GetString("journeySellKeyAA");
                        _JourneykeyDataAA = _JourneykeyDataAA.Replace(@"""", string.Empty);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responseSeatmap = await client.GetAsync(BaseURL + "/api/nsk/v3/booking/seatmaps/journey/" + _JourneykeyDataAA + "?IncludePropertyLookup=true");

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
                            SeatMapResponceModel SeatMapResponceModel = null;
                            for (int x = 0; x < data; x++)
                            {
                                data dataobj = new data();

                                SeatMapResponceModel = new SeatMapResponceModel();
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

                                    try
                                    {
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
                                        compartmentsunitobj.Airline = Airlines.Airasia;
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
                                        compartmentsunitobj.properties = Propertieslist;
                                        Decksobj.units = compartmentsunitlist;
                                    }
                                    catch(Exception ex) 
                                    { 
                                    }

                                    //compartmentsunitlist.Add(compartmentsunitobj);
                                    
                                }

                                //var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                                //var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                                string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                                    RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;


                                List<Groups> GroupsFeelist = new List<Groups>();
                                foreach (Match item in Regex.Matches(strText, @"group"":(?<key>[\s\S]*?),[\s\S]*?type[\s\S]*?}"))
                                {

                                    Groups Groupsobj = new Groups();
                                    int myString1 = Convert.ToInt32(item.Groups["key"].Value.Trim());
                                    string myString = myString1.ToString();
                                    // x.data[0].fees["MCFBRFQ-"].groups.1
                                    //var group = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString.ToString()];

                                    //var fees = JsonObjSeatmap.data[0].fees["MCFBRFQ-"].groups[myString].fees;
                                    //  Groups Groups = new Groups();

                                    GroupsFee GroupsFeeobj = new GroupsFee();

                                    string test = passengerkey12;
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




                                //HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));
                                //_SeatMapdata.Add("<Start>" + JsonConvert.SerializeObject(SeatMapResponceModel) + "<End>");
                                HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));
                            }
                            _SeatMapdata.Add("<Start>" + JsonConvert.SerializeObject(SeatMapResponceModel) + "<End>");
                                HttpContext.Session.SetString("_SeatmapData", JsonConvert.SerializeObject(_SeatMapdata));

                                if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(_SeatMapdata)))
                                {
                                    if (_SeatMapdata.Count == 2)
                                    {
                                        MainSeatMapdata = new List<string>();
                                    }
                                    MainSeatMapdata.Add(JsonConvert.SerializeObject(_SeatMapdata));
                                //TempData["Seatmap"] = JsonConvert.SerializeObject(SeatMapResponceModellist);
                            }
                        }
                    }
                    #endregion

                    //SpicejetSeat map
                    #region SeatMap
                    if (_JourneykeyRTData.ToLower() == "spicejet")
                    {
                        GetSeatAvailabilityRequest _getseatAvailabilityRequest = new GetSeatAvailabilityRequest();
                        GetSeatAvailabilityResponse _getSeatAvailabilityResponse = new GetSeatAvailabilityResponse();

                        _getseatAvailabilityRequest.Signature = Signature;
                        _getseatAvailabilityRequest.ContractVersion = 420;

                        SeatAvailabilityRequest _seatRequest = new SeatAvailabilityRequest();
                        List<GetSeatAvailabilityResponse> SeatGroup = new List<GetSeatAvailabilityResponse>();
                        for (int i = 0; i < AirAsiaTripResponceobj.journeys[0].segments.Count; i++)
                        {
                            _seatRequest = new SeatAvailabilityRequest();
                            _seatRequest.STDSpecified = true;
                            _seatRequest.STD = AirAsiaTripResponceobj.journeys[0].segments[i].designator.departure;
                            _seatRequest.DepartureStation = AirAsiaTripResponceobj.journeys[0].segments[i].designator.origin;
                            _seatRequest.ArrivalStation = AirAsiaTripResponceobj.journeys[0].segments[i].designator.destination;
                            _seatRequest.IncludeSeatFees = true;
                            _seatRequest.IncludeSeatFeesSpecified = true;
                            _seatRequest.SeatAssignmentModeSpecified = true;
                            _seatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                            _seatRequest.FlightNumber = AirAsiaTripResponceobj.journeys[0].segments[i].identifier.identifier;
                            _seatRequest.OverrideSTDSpecified = true;
                            _seatRequest.OverrideSTD = AirAsiaTripResponceobj.journeys[0].segments[i].designator.departure;
                            _seatRequest.CarrierCode = AirAsiaTripResponceobj.journeys[0].segments[i].identifier.carrierCode;
                            _getseatAvailabilityRequest.SeatAvailabilityRequest = _seatRequest;
                            _getSeatAvailabilityResponse = await objSpiceJet.GetseatAvaialbility(_getseatAvailabilityRequest);
                            SeatGroup.Add(_getSeatAvailabilityResponse);

                        }


                        string str1 = JsonConvert.SerializeObject(SeatGroup);
                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getseatAvailabilityRequest) + "\n\n Response: " + JsonConvert.SerializeObject(SeatGroup), "GetSeatAvailability");



                        // data[0].seatMap.decks['1'].compartments.Y.units[0].unitKey


                        if (SeatGroup != null)
                        {

                            var data = SeatGroup.Count;// _getSeatAvailabilityResponse.SeatAvailabilityResponse.EquipmentInfos.Length;

                            List<data> datalist = new List<data>();
                            SeatMapResponceModel SeatMapResponceModel = new SeatMapResponceModel();
                            List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();
                            for (int x = 0; x < data; x++)
                            {
                                data dataobj = new data();

                                SeatMapResponceModel = new SeatMapResponceModel();
                                SeatMapResponceModellist = new List<SeatMapResponceModel>();
                                Fees Fees = new Fees();
                                Seatmap Seatmapobj = new Seatmap();
                                //Seatmapobj.name = _getSeatAvailabilityResponse.SeatAvailabilityResponse.EquipmentInfos[x].Compartments[0].Seats[x].SeatDesignator;
                                Seatmapobj.arrivalStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].ArrivalStation;
                                Seatmapobj.departureStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].DepartureStation;
                                Seatmapobj.marketingCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].MarketingCode;
                                Seatmapobj.equipmentType = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentType;
                                Seatmapobj.equipmentTypeSuffix = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentTypeSuffix;
                                //doubt
                                //Seatmapobj.categorySG = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].EquipmentCategory.ToString();
                                //doubt
                                //Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;
                                Decks Decksobj = new Decks();
                                Decksobj.availableUnits = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].AvailableUnits;
                                Decksobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].CompartmentDesignator;
                                Decksobj.length = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Length;
                                Decksobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Width;
                                Decksobj.sequence = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Sequence;
                                Decksobj.orientation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Orientation;
                                Seatmapobj.decks = Decksobj;

                                int compartmentsunitCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats.Length;
                                List<Unit> compartmentsunitlist = new List<Unit>();
                                for (int i = 0; i < compartmentsunitCount; i++)
                                {
                                    Unit compartmentsunitobj = new Unit();

                                    compartmentsunitobj.assignable = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Assignable;
                                    compartmentsunitobj.availability = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatAvailability);
                                    compartmentsunitobj.compartmentDesignator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].CompartmentDesignator;
                                    compartmentsunitobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatDesignator;
                                    compartmentsunitobj.type = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatGroup);
                                    compartmentsunitobj.travelClassCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].TravelClassCode;
                                    compartmentsunitobj.set = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatSet;
                                    compartmentsunitobj.group = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatGroup;
                                    compartmentsunitobj.priority = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Priority;
                                    compartmentsunitobj.text = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Text;
                                    //compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                                    compartmentsunitobj.angle = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatAngle;
                                    compartmentsunitobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Width;
                                    compartmentsunitobj.height = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Height;
                                    compartmentsunitobj.zone = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Zone;
                                    compartmentsunitobj.x = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].X;
                                    compartmentsunitobj.y = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Y;
                                    compartmentsunitobj.Airline = Airlines.Spicejet;

                                    for (int k = 0; k < SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length; k++)
                                    {
                                        if (compartmentsunitobj.group == Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].SeatGroup))
                                        {
                                            var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges.Length;

                                            List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                                            for (int l = 0; l < feesgroupserviceChargescount; l++)
                                            {
                                                Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                                compartmentsunitobj.servicechargefeeAmount += Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges[l].Amount);
                                            }
                                        }
                                    }
                                    //if (compartmentsunitobj.assignable == true)
                                    //{
                                    compartmentsunitobj.unitKey = compartmentsunitobj.designator;

                                    compartmentsunitlist.Add(compartmentsunitobj);
                                    //}

                                    int compartmentypropertiesCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList.Length;
                                    List<Properties> Propertieslist = new List<Properties>();
                                    for (int j = 0; j < compartmentypropertiesCount; j++)
                                    {
                                        Properties compartmentyproperties = new Properties();
                                        compartmentyproperties.code = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList[j].TypeCode;
                                        compartmentyproperties.value = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList[j].Value;
                                        Propertieslist.Add(compartmentyproperties);
                                    }

                                    compartmentsunitobj.properties = Propertieslist;
                                    Decksobj.units = compartmentsunitlist;
                                }

                                //var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                                //var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                                //string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                                // RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;

                                //string seatgroup = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].Compartments[0].Seats[i].SeatGroup.ToString();

                                List<Groups> GroupsFeelist = new List<Groups>();

                                int testcount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length;
                                for (int i = 0; i < testcount; i++)
                                {
                                    Groups Groupsobj = new Groups();
                                    GroupsFee GroupsFeeobj = new GroupsFee();
                                    string feeseatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                                    //if (seatgroup == feeseatGroup)
                                    //{
                                    //doubt
                                    GroupsFeeobj.SeatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                                    GroupsFeeobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeNumber;
                                    GroupsFeeobj.ssrCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRCode;
                                    GroupsFeeobj.ssrNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRNumber;
                                    GroupsFeeobj.paymentNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.PaymentNumber;
                                    GroupsFeeobj.isConfirmed = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                    GroupsFeeobj.isConfirming = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                    GroupsFeeobj.isConfirmingExternal = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                    GroupsFeeobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeCode;
                                    GroupsFeeobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeDetail;
                                    //Dout
                                    // GroupsFeeobj.passengerFeeKey = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].passengerFeeKey;
                                    GroupsFeeobj.flightReference = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FlightReference;
                                    GroupsFeeobj.note = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.Note;
                                    GroupsFeeobj.createdDate = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.CreatedDate;
                                    GroupsFeeobj.isProtected = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.IsProtected;

                                    var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges.Length;

                                    List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                                    for (int l = 0; l < feesgroupserviceChargescount; l++)
                                    {
                                        Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                        feesgroupserviceChargesobj.amount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].Amount);
                                        feesgroupserviceChargesobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeCode; ;
                                        feesgroupserviceChargesobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeDetail;
                                        // feesgroupserviceChargesobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeType;
                                        // feesgroupserviceChargesobj.collectType = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CollectType;
                                        feesgroupserviceChargesobj.currencyCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CurrencyCode;

                                        feesgroupserviceChargesobj.foreignAmount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ForeignAmount);
                                        feesgroupserviceChargesobj.ticketCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].TicketCode;
                                        feesgroupserviceChargeslist.Add(feesgroupserviceChargesobj);

                                    }
                                    GroupsFeeobj.serviceCharges = feesgroupserviceChargeslist;

                                    Groupsobj.groupsFee = GroupsFeeobj;
                                    GroupsFeelist.Add(Groupsobj);

                                    Fees.groups = GroupsFeelist;

                                }
                                dataobj.seatMap = Seatmapobj;
                                dataobj.seatMapfees = Fees;
                                datalist.Add(dataobj);
                                SeatMapResponceModel.datalist = datalist;
                            }
                            string strseat = JsonConvert.SerializeObject(SeatMapResponceModel);
                            
                            
                            SeatMapdata.Add("<Start>" + JsonConvert.SerializeObject(SeatMapResponceModel) + "<End>");
                            HttpContext.Session.SetString("SeatmapRT", JsonConvert.SerializeObject(SeatMapResponceModel));
                            HttpContext.Session.SetString("SeatmapData", JsonConvert.SerializeObject(SeatMapdata));
                            if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(SeatMapdata)))
                            {
                                if (SeatMapdata.Count == 2)
                                {
                                    MainSeatMapdata = new List<string>();
                                }
                                MainSeatMapdata.Add(JsonConvert.SerializeObject(SeatMapdata));
                            }
                        }
                    }

                    #endregion

                    #endregion
                    #region Meals AirAsia
                    if (_JourneykeyRTData.ToLower() == "airasia")
                    {
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
                        Tripobj.departureDate = passeengerKeyList.journeys[0].designator.departure.ToString();
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
                            var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
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
                                        passengersdetail.Airline = Airlines.Airasia;
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
                            //HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));
                           
                            _Mealsdata.Add("<Start>" + JsonConvert.SerializeObject(SSRAvailabiltyResponceobj) + "<End>");
                            HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));
                            HttpContext.Session.SetString("_MealsData", JsonConvert.SerializeObject(_Mealsdata));
                            if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(_Mealsdata)))
                            {
                                if (_Mealsdata.Count == 2)
                                {
                                    MainMealsdata = new List<string>();
                                }
                                MainMealsdata.Add(JsonConvert.SerializeObject(_Mealsdata));
                            }
                        }
                    }
                    #endregion
                    //Spicejet Roundtrip SSR
                    #region ssravailability
                    if (_JourneykeyRTData.ToLower() == "spicejet")
                    {
                        List<legSsrs> SSRAvailabiltyLegssrlist = new List<legSsrs>();
                        SSRAvailabiltyResponceModel SSRAvailabiltyResponceobj = null;
                        AirAsiaTripResponceModel passeengerlist = null;
                        string passenger = HttpContext.Session.GetString("SGkeypassengerRT");
                        passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));

                        //availibiltyRQ

                        GetSSRAvailabilityForBookingRequest _req = new GetSSRAvailabilityForBookingRequest();
                        GetSSRAvailabilityForBookingResponse _res = new GetSSRAvailabilityForBookingResponse();
                        try
                        {
                            int segmentcount = 0;
                            int journeyscount = passeengerlist.journeys.Count;
                            _req.Signature = Signature;
                            _req.ContractVersion = 420;
                            SSRAvailabilityForBookingRequest _SSRAvailabilityForBookingRequest = new SSRAvailabilityForBookingRequest();
                            for (int i = 0; i < journeyscount; i++)
                            {
                                int segmentscount = passeengerlist.journeys[i].segments.Count;
                                _SSRAvailabilityForBookingRequest.SegmentKeyList = new LegKey[segmentscount];
                                for (int j = 0; j < segmentscount; j++)
                                {
                                    int legcount = passeengerlist.journeys[i].segments[j].legs.Count;
                                    for (int n = 0; n < legcount; n++)
                                    {
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j] = new LegKey();
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].CarrierCode = passeengerlist.journeys[i].segments[j].identifier.carrierCode;
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].FlightNumber = passeengerlist.journeys[i].segments[j].identifier.identifier;
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureDateSpecified = true;
                                        //string strdate = Convert.ToDateTime(passengerdetails.departure).ToString("yyyy-MM-dd");
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureDate = Convert.ToDateTime(AirAsiaTripResponceobj.journeys[i].segments[j].designator.departure);//DateTime.ParseExact(strdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].ArrivalStation = AirAsiaTripResponceobj.journeys[i].segments[j].designator.destination;
                                        _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureStation = AirAsiaTripResponceobj.journeys[i].segments[j].designator.origin;
                                        segmentcount++;
                                    }
                                }
                            }
                            _SSRAvailabilityForBookingRequest.PassengerNumberList = new short[Convert.ToInt16(TotalCount)];//new short[1];
                            int paxCount = _SSRAvailabilityForBookingRequest.PassengerNumberList.Length;//passeengerlist.passengerscount;
                            for (int i = 0; i < paxCount; i++)
                            {
                                if (i > 0)
                                    continue;
                                _SSRAvailabilityForBookingRequest.PassengerNumberList[i] = Convert.ToInt16(i);
                            }
                            _SSRAvailabilityForBookingRequest.InventoryControlled = true;
                            _SSRAvailabilityForBookingRequest.InventoryControlledSpecified = true;
                            _SSRAvailabilityForBookingRequest.NonInventoryControlled = true;
                            _SSRAvailabilityForBookingRequest.NonInventoryControlledSpecified = true;
                            _SSRAvailabilityForBookingRequest.SeatDependent = true;
                            _SSRAvailabilityForBookingRequest.SeatDependentSpecified = true;
                            _SSRAvailabilityForBookingRequest.NonSeatDependent = true;
                            _SSRAvailabilityForBookingRequest.NonSeatDependentSpecified = true;
                            _SSRAvailabilityForBookingRequest.CurrencyCode = "INR";
                            _SSRAvailabilityForBookingRequest.SSRAvailabilityMode = SSRAvailabilityMode.NonBundledSSRs;
                            _SSRAvailabilityForBookingRequest.SSRAvailabilityModeSpecified = true;
                            _req.SSRAvailabilityForBookingRequest = _SSRAvailabilityForBookingRequest;
                            objSpiceJet = new SpiceJetApiController();
                            _res = await objSpiceJet.GetSSRAvailabilityForBooking(_req);

                            string Str2 = JsonConvert.SerializeObject(_res);
                            logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_req) + "\n\n Response: " + JsonConvert.SerializeObject(_res), "GetSSRAvailabilityForBooking");


                            //******Vinay***********//
                            if (_res != null)
                            {
                                SSRAvailabiltyLegssrlist = new List<legSsrs>();

                                SSRAvailabiltyResponceobj = new SSRAvailabiltyResponceModel();
                                int PaxssrListcount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[0].AvailablePaxSSRList.Length;
                                try
                                {
                                    legSsrs SSRAvailabiltyLegssrobj = new legSsrs();
                                    legDetails legDetailsobj = null;
                                    List<childlegssrs> legssrslist = new List<childlegssrs>();
                                    for (int i1 = 0; i1 < _res.SSRAvailabilityForBookingResponse.SSRSegmentList.Length; i1++)
                                    {
                                        legssrslist = new List<childlegssrs>();
                                        for (int j = 0; j < _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList.Length; j++)
                                        {
                                            if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].InventoryControlled == true)
                                            {
                                                int legSsrscount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList.Length;
                                                try
                                                {
                                                    for (int i = 0; i < legSsrscount; i++)
                                                    {

                                                        SSRAvailabiltyLegssrobj = new legSsrs();
                                                        SSRAvailabiltyLegssrobj.legKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.ToString();
                                                        legDetailsobj = new legDetails();
                                                        legDetailsobj.destination = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.ArrivalStation;
                                                        legDetailsobj.origin = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.DepartureStation;
                                                        legDetailsobj.departureDate = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.DepartureDate.ToString();
                                                        legidentifier legidentifierobj = new legidentifier();
                                                        legidentifierobj.identifier = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.FlightNumber;
                                                        legidentifierobj.carrierCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.CarrierCode;
                                                        legDetailsobj.legidentifier = legidentifierobj;
                                                        childlegssrs legssrs = new childlegssrs();
                                                        legssrs.ssrCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode.ToString();
                                                        legssrs.available = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].Available;
                                                        if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0)
                                                        {
                                                            legssrs.feeCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.FeeCode;
                                                            List<legpassengers> legpassengerslist = new List<legpassengers>();
                                                            Decimal Amount = decimal.Zero;
                                                            legpassengers passengersdetail = new legpassengers();
                                                            foreach (var items in _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges)
                                                            {
                                                                Amount += items.Amount;
                                                                passengersdetail.price = Math.Round(Amount).ToString(); //Ammount

                                                            }
                                                            passengersdetail.passengerKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PassengerNumberList.ToString();
                                                            passengersdetail.ssrKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode;
                                                            passengersdetail.Airline = Airlines.Spicejet;
                                                            legpassengerslist.Add(passengersdetail);
                                                            legssrs.legpassengers = legpassengerslist;
                                                            legssrslist.Add(legssrs);
                                                        }


                                                    }


                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }
                                        SSRAvailabiltyLegssrobj.legDetails = legDetailsobj;
                                        SSRAvailabiltyLegssrobj.legssrs = legssrslist;
                                        SSRAvailabiltyLegssrlist.Add(SSRAvailabiltyLegssrobj);
                                    }

                                }
                                catch (Exception ex)
                                {

                                }


                                SSRAvailabiltyResponceobj.legSsrs = SSRAvailabiltyLegssrlist;
                                

                                Mealsdata.Add("<Start>" + JsonConvert.SerializeObject(SSRAvailabiltyResponceobj) + "<End>");
                                HttpContext.Session.SetString("SGMealsRT", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));
                                HttpContext.Session.SetString("MealsData", JsonConvert.SerializeObject(Mealsdata));
                                if (!string.IsNullOrEmpty(JsonConvert.SerializeObject(Mealsdata)))
                                {
                                    if (Mealsdata.Count == 2)
                                    {
                                        MainMealsdata = new List<string>();
                                    }
                                    MainMealsdata.Add(JsonConvert.SerializeObject(Mealsdata));
                                }

                            }
                        }
                        catch
                        {

                        }
                    }
                    #endregion
                }
            }
            HttpContext.Session.SetString("Mainpassengervm", JsonConvert.SerializeObject(MainPassengerdata));
            HttpContext.Session.SetString("Mainseatmapvm", JsonConvert.SerializeObject(MainSeatMapdata));
            HttpContext.Session.SetString("Mainmealvm", JsonConvert.SerializeObject(MainMealsdata));

            return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
        }

        PaxPriceType[] getPaxdetails(int adult_, int child_, int infant_)
        {
            PaxPriceType[] paxPriceTypes = null;
            try
            {
                //int tcount = adult_ + child_ + infant_;
                int i = 0;
                if (adult_ > 0) i++;
                if (child_ > 0) i++;
                if (infant_ > 0) i++;

                paxPriceTypes = new PaxPriceType[i];
                int j = 0;
                if (adult_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "ADT";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(adult_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }

                if (child_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "CHD";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(child_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }

                if (infant_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "INFT";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(infant_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }
            }
            catch (Exception e)
            {
            }

            return paxPriceTypes;
        }


        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = Bookingmanager_.MessageState.New;
                SourcePOS.OrganizationCode = "APITESTID";
                SourcePOS.AgentCode = "AG";
                SourcePOS.LocationCode = "";
                SourcePOS.DomainCode = "WWW";
            }
            catch (Exception e)
            {
                string exp = e.Message;
                exp = null;
            }
            return SourcePOS;
        }

    }

}
