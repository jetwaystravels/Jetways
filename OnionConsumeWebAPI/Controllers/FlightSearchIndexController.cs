using System;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NuGet.Common;

namespace OnionConsumeWebAPI.Controllers
{
    //[Route("jetways/[controller]/[action]")]
    public class FlightSearchIndexController : Controller
    {
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";

        string token = string.Empty;


        [Route("")]
        public async Task<IActionResult> Index()
        {
            List<City> city = new List<City>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5225/");
            HttpResponseMessage response = await client.GetAsync("api/City/getallCity");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                city = JsonConvert.DeserializeObject<List<City>>(results);
                city.Insert(0, new City { CityCode = "Select", CityName = "Select" });
            }
            else
            {
                city.Insert(0, new City { CityCode = "Select", CityName = "Select" });
            }
            ViewBag.ListofCountry = city;

            return View();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SearchResultFlight(SimpleAvailabilityRequestModel _GetfligthModel)
        {

            string destination1 = string.Empty;
            string origin1 = string.Empty;
            string arrival1 = string.Empty;
            string departure1 = string.Empty;
            string identifier1 = string.Empty;
            string carrierCode1 = string.Empty;
            string totalfare1 = string.Empty;
            string journeyKey1 = string.Empty;
            string fareAvailabilityKey1 = string.Empty;
            string inventoryControl1 = string.Empty;
            string ssrKey = string.Empty;
            string passengerkey = string.Empty;
            string uniquekey = string.Empty;
            decimal fareTotalsum = 0;

            _credentials credentialsobj = new _credentials();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5225/");
                HttpResponseMessage response = await client.GetAsync("api/Login/getotacredairasia");
                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsStringAsync().Result;

                    credentialsobj = JsonConvert.DeserializeObject<_credentials>(results);


                }

                AirAsiaLogin login = new AirAsiaLogin();
                login.credentials = credentialsobj;

                TempData["AirAsiaLogin"] = login.credentials.Image;
                AirasiaTokan AirasiaTokan = new AirasiaTokan();
                var AirasialoginRequest = JsonConvert.SerializeObject(login, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responce = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/token", login);
                if (responce.IsSuccessStatusCode)
                {
                    var results = responce.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    AirasiaTokan.token = JsonObj.data.token;
                    AirasiaTokan.idleTimeoutInMinutes = JsonObj.data.idleTimeoutInMinutes;
                    //token = ((Newtonsoft.Json.Linq.JValue)value).Value.ToString();
                }


                HttpContext.Session.SetString("AirasiaTokan", JsonConvert.SerializeObject(AirasiaTokan.token));


                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
                _SimpleAvailabilityobj.origin = _GetfligthModel.origin;
                _SimpleAvailabilityobj.destination = _GetfligthModel.destination;
                _SimpleAvailabilityobj.beginDate = _GetfligthModel.beginDate;
                _SimpleAvailabilityobj.endDate = "";
                Codessimple _codes = new Codessimple();


                List<Typesimple> _typeslist = new List<Typesimple>();
                var AdtType = _GetfligthModel.passengercount.adulttype;
                var AdtCount = _GetfligthModel.passengercount.adultcount;
                var chdtype = _GetfligthModel.passengercount.childtype;
                var chdcount = _GetfligthModel.passengercount.childcount;
                var infanttype = _GetfligthModel.passengercount.infanttype;
                var infantcount = _GetfligthModel.passengercount.infantcount;

                if (AdtType == "ADT" && AdtCount != 0)
                {
                    Typesimple Types = new Typesimple();
                    Types.type = AdtType;
                    Types.count = AdtCount;

                    _typeslist.Add(Types);
                }
                if (chdtype == "CHD" && chdcount != 0)
                {
                    Typesimple Types = new Typesimple();
                    Types.type = chdtype;
                    Types.count = chdcount;

                    _typeslist.Add(Types);
                }
                if (infanttype == "INFT" && infantcount != 0)
                {
                    Typesimple Types = new Typesimple();
                    Types.type = infanttype;
                    Types.count = infantcount;

                    _typeslist.Add(Types);
                }


                Passengerssimple _Passengerssimple = new Passengerssimple();
                _Passengerssimple.types = _typeslist;
                _SimpleAvailabilityobj.passengers = _Passengerssimple;

                //_codes.currencyCode = "INR";
                _SimpleAvailabilityobj.codes = _codes;
                _SimpleAvailabilityobj.sourceOrganization = "";
                _SimpleAvailabilityobj.currentSourceOrganization = "";
                _SimpleAvailabilityobj.promotionCode = "";
                string[] sortOptions = new string[1];
                sortOptions[0] = "ServiceType";
                string[] fareTypes = new string[1];
                fareTypes[0] = "R";
                string[] productClasses = new string[2];
                productClasses[0] = "EC";
                productClasses[1] = "HF";
                Filters Filters = new Filters();
                Filters.exclusionType = "Default";
                Filters.loyalty = "MonetaryOnly";
                Filters.includeAllotments = true;
                Filters.connectionType = "Both";
                Filters.compressionType = "CompressByProductClass";
                Filters.sortOptions = sortOptions;
                Filters.maxConnections = 10;
                Filters.fareTypes = fareTypes;
                Filters.productClasses = productClasses;
                _SimpleAvailabilityobj.filters = Filters;
                _SimpleAvailabilityobj.taxesAndFees = "Taxes";
                _SimpleAvailabilityobj.ssrCollectionsMode = "Leg";
                _SimpleAvailabilityobj.numberOfFaresPerJourney = 10;


                //end code start




                List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist = new List<SimpleAvailibilityaAddResponce>();
                SimpleAvailibilityaAddResponce _SimpleAvailibilityaAddResponceobj = new SimpleAvailibilityaAddResponce();

                var json = JsonConvert.SerializeObject(_SimpleAvailabilityobj, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AirasiaTokan.token);
                HttpResponseMessage responce1 = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v4/availability/search/simple", _SimpleAvailabilityobj);
                if (responce1.IsSuccessStatusCode)
                {
                    var results = responce1.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    // var value = JsonObj.data.token;
                    //var value = JsonObj.data.results[0].trips[0].date;
                    //  var oriDes = _GetfligthModel.origin + "|" + _GetfligthModel.destination;
                    var finddate = JsonObj.data.results[0].trips[0].date;
                    var bookingdate = finddate.ToString("dddd, dd MMMM yyyy");

                    string oriDes = _SimpleAvailabilityobj.origin + "|" + _SimpleAvailabilityobj.destination;


                    int count = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count;
                    TempData["count"] = count;



                    for (int i = 0; i < JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count; i++)
                    {

                        string journeyKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].journeyKey;
                        //journeyKey1 = ((Newtonsoft.Json.Linq.JValue)journeyKey).Value.ToString();
                        var destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i];
                        Designator Designatorobj = new Designator();
                        Designatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.origin;
                        Designatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.destination;
                        Designatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.departure;
                        Designatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.arrival;

                        var segmentscount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments.Count;
                        List<Segment> Segmentobjlist = new List<Segment>();

                        for (int l = 0; l < segmentscount; l++)
                        {
                            Segment Segmentobj = new Segment();
                            Designator SegmentDesignatorobj = new Designator();
                            SegmentDesignatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.origin;
                            SegmentDesignatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.destination;
                            SegmentDesignatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.departure;
                            SegmentDesignatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.arrival;
                            Segmentobj.designator = SegmentDesignatorobj;
                            Identifier Identifier = new Identifier();
                            Identifier.identifier = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.identifier;
                            Identifier.carrierCode = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.carrierCode;
                            Segmentobj.identifier = Identifier;

                            int legscount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs.Count;
                            List<Leg> Leglist = new List<Leg>();

                            for (int m = 0; m < legscount; m++)
                            {
                                Leg Legobj = new Leg();
                                Designator legdesignatorobj = new Designator();
                                legdesignatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.origin;
                                legdesignatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.destination;
                                legdesignatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.departure;
                                legdesignatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.arrival;
                                Legobj.designator = legdesignatorobj;
                                Legobj.legKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
                                Legobj.flightReference = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
                                Leglist.Add(Legobj);
                                LegInfo LegInfo = new LegInfo();
                                LegInfo.arrivalTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTerminal;
                                LegInfo.departureTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTerminal;
                                LegInfo.arrivalTime = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTime;
                                LegInfo.departureTime = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTime;
                                Legobj.legInfo = LegInfo;

                            }
                            //  Leglist.Add(Legobj);
                            Segmentobj.legs = Leglist;
                            Segmentobjlist.Add(Segmentobj);

                        }


                        var arrivalTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.arrivalTerminal;
                        var departureTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.departureTerminal;
                        int FareCount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares.Count;


                        if (FareCount > 0)
                        {
                            List<FareIndividual> fareIndividualsList = new List<FareIndividual>();

                            for (int j = 0; j < FareCount; j++)
                            {
                                //x.data.results[0].trips[0].journeysAvailableByMarket["DEL|BLR"][0].fares[0].fareAvailabilityKey


                                FareIndividual fareIndividual = new FareIndividual();

                                string fareAvailabilityKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[j].fareAvailabilityKey;
                                string fareAvailabilityKeyhead = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[0].fareAvailabilityKey;
                                var fareAvilableCount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares.Count;
                                var isGoverning = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].isGoverning;
                                var procuctclass = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].productClass;
                                var passengertype = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].passengerType;
                                var fareAmount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].fareAmount;
                                fareTotalsum = JsonObj.data.faresAvailable[fareAvailabilityKeyhead].fares[0].passengerFares[0].fareAmount;



                                decimal discountamount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].discountedFare;

                                int servicecharge = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges.Count;
                                decimal finalamount = 0;
                                for (int k = 1; k < servicecharge; k++)
                                {

                                    decimal amount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges[k].amount;
                                    finalamount += amount;

                                }
                                //TempData["fareTotalsum"] = fareTotalsum;

                                decimal taxamount = finalamount;
                                fareIndividual.taxamount = taxamount;
                                fareIndividual.faretotal = fareAmount;
                                fareIndividual.discountamount = discountamount;
                                fareIndividual.passengertype = passengertype;
                                fareIndividual.fareKey = fareAvailabilityKey;
                                fareIndividual.procuctclass = procuctclass;
                                fareIndividualsList.Add(fareIndividual);

                            }

                            var expandoconverter = new ExpandoObjectConverter();
                            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(destination.ToString(), expandoconverter);
                            string jsonresult = JsonConvert.SerializeObject(obj);
                            _SimpleAvailibilityaAddResponceobj = JsonConvert.DeserializeObject<SimpleAvailibilityaAddResponce>(jsonresult);
                            _SimpleAvailibilityaAddResponceobj.designator = Designatorobj;
                            _SimpleAvailibilityaAddResponceobj.segments = Segmentobjlist;

                            _SimpleAvailibilityaAddResponceobj.arrivalTerminal = arrivalTerminal;
                            _SimpleAvailibilityaAddResponceobj.departureTerminal = departureTerminal;
                            _SimpleAvailibilityaAddResponceobj.bookingdate = bookingdate;
                            _SimpleAvailibilityaAddResponceobj.fareTotalsum = fareTotalsum;
                            _SimpleAvailibilityaAddResponceobj.journeyKey = journeyKey;
                            _SimpleAvailibilityaAddResponceobj.faresIndividual = fareIndividualsList;
                            SimpleAvailibilityaAddResponcelist.Add(_SimpleAvailibilityaAddResponceobj);
                        }
                    }


                }
                TempData["Mymodel"] = JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist);
                TempData["PassengerModel"] = JsonConvert.SerializeObject(_SimpleAvailabilityobj);
                //method/Controller
                return RedirectToAction("FlightView", "ResultFlightView");



            }

        }
        //public async Task<IActionResult> SearchResultFlight(GetfligthModel _GetfligthModel )
        //{

        //    string destination1 = string.Empty;
        //    string origin1 = string.Empty;
        //    string arrival1 = string.Empty;
        //    string departure1 = string.Empty;
        //    string identifier1 = string.Empty;
        //    string carrierCode1 = string.Empty;
        //    string totalfare1 = string.Empty;
        //    string journeyKey1 = string.Empty;
        //    string fareAvailabilityKey1 = string.Empty;
        //    string inventoryControl1 = string.Empty;
        //    string ssrKey = string.Empty;
        //    string passengerkey = string.Empty;
        //    string uniquekey = string.Empty;
        //    decimal fareTotalsum = 0;

        //    List<_credentials> credentialslist = new List<_credentials>();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:5225/");
        //        HttpResponseMessage response = await client.GetAsync("api/Login/getotacredairasia");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var results = response.Content.ReadAsStringAsync().Result;

        //            credentialslist = JsonConvert.DeserializeObject<List<_credentials>>(results);


        //        }

        //        AirAsiaLogin login = new AirAsiaLogin();
        //        login.credentials = credentialslist;

        //        AirasiaTokan AirasiaTokan = new AirasiaTokan();
        //        var AirasialoginRequest = JsonConvert.SerializeObject(login, Formatting.Indented);
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage responce = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/token", login);
        //        if (responce.IsSuccessStatusCode)
        //        {
        //            var results = responce.Content.ReadAsStringAsync().Result;
        //            var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
        //            AirasiaTokan.token = JsonObj.data.token;
        //            AirasiaTokan.idleTimeoutInMinutes = JsonObj.data.idleTimeoutInMinutes;
        //            //token = ((Newtonsoft.Json.Linq.JValue)value).Value.ToString();
        //        }

        //        HttpContext.Session.SetString("AirasiaTokan", JsonConvert.SerializeObject(AirasiaTokan.token));


        //        SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
        //        _SimpleAvailabilityobj.origin = _GetfligthModel.origin;
        //        _SimpleAvailabilityobj.destination = _GetfligthModel.destination;
        //        _SimpleAvailabilityobj.beginDate = _GetfligthModel.beginDate;
        //        _SimpleAvailabilityobj.endDate = "";
        //        Codessimple _codes = new Codessimple();
        //        Typesimple Types = new Typesimple();

        //        List<Typesimple> _typeslist = new List<Typesimple>();
        //        Types.type = "ADT";
        //        Types.count = 2;
        //        _typeslist.Add(Types);
        //        Passengerssimple _Passengerssimple = new Passengerssimple();
        //        _Passengerssimple.types = _typeslist;
        //        _SimpleAvailabilityobj.passengers = _Passengerssimple;

        //        //_codes.currencyCode = "INR";
        //        _SimpleAvailabilityobj.codes = _codes;
        //        _SimpleAvailabilityobj.sourceOrganization = "";
        //        _SimpleAvailabilityobj.currentSourceOrganization = "";
        //        _SimpleAvailabilityobj.promotionCode = "";
        //        string[] sortOptions = new string[1];
        //        sortOptions[0] = "ServiceType";
        //        string[] fareTypes = new string[1];
        //        fareTypes[0] = "R";
        //        string[] productClasses = new string[2];
        //        productClasses[0] = "EC";
        //        productClasses[1] = "HF";
        //        Filters Filters = new Filters();
        //        Filters.exclusionType = "Default";
        //        Filters.loyalty = "MonetaryOnly";
        //        Filters.includeAllotments = true;
        //        Filters.connectionType = "Both";
        //        Filters.compressionType = "CompressByProductClass";
        //        Filters.sortOptions = sortOptions;
        //        Filters.maxConnections = 10;
        //        Filters.fareTypes = fareTypes;
        //        Filters.productClasses = productClasses;
        //        _SimpleAvailabilityobj.filters = Filters;
        //        _SimpleAvailabilityobj.taxesAndFees = "Taxes";
        //        _SimpleAvailabilityobj.ssrCollectionsMode = "Leg";
        //        _SimpleAvailabilityobj.numberOfFaresPerJourney = 10;


        //        //end code start




        //        List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist = new List<SimpleAvailibilityaAddResponce>();
        //        SimpleAvailibilityaAddResponce _SimpleAvailibilityaAddResponceobj = new SimpleAvailibilityaAddResponce();

        //        var json = JsonConvert.SerializeObject(_SimpleAvailabilityobj, Formatting.Indented);
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AirasiaTokan.token);
        //        HttpResponseMessage responce1 = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v4/availability/search/simple", _SimpleAvailabilityobj);
        //        if (responce1.IsSuccessStatusCode)
        //        {
        //            var results = responce1.Content.ReadAsStringAsync().Result;
        //            var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
        //            // var value = JsonObj.data.token;
        //            //var value = JsonObj.data.results[0].trips[0].date;
        //            //  var oriDes = _GetfligthModel.origin + "|" + _GetfligthModel.destination;
        //            var finddate = JsonObj.data.results[0].trips[0].date;
        //            var bookingdate = finddate.ToString("dddd, dd MMMM yyyy");

        //            string oriDes = _SimpleAvailabilityobj.origin + "|" + _SimpleAvailabilityobj.destination;


        //            int count = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count;
        //            TempData["count"] = count;



        //            for (int i = 0; i < JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count; i++)
        //            {

        //                string journeyKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].journeyKey;
        //                //journeyKey1 = ((Newtonsoft.Json.Linq.JValue)journeyKey).Value.ToString();
        //                var destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i];
        //                Designator Designatorobj = new Designator();
        //                Designatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.origin;
        //                Designatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.destination;
        //                Designatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.departure;
        //                Designatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.arrival;

        //                var segmentscount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments.Count;
        //                List<Segment> Segmentobjlist = new List<Segment>();

        //                for (int l = 0; l < segmentscount; l++)
        //                {
        //                    Segment Segmentobj = new Segment();
        //                    Designator SegmentDesignatorobj = new Designator();
        //                    SegmentDesignatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.origin;
        //                    SegmentDesignatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.destination;
        //                    SegmentDesignatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.departure;
        //                    SegmentDesignatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.arrival;
        //                    Segmentobj.designator = SegmentDesignatorobj;
        //                    Identifier Identifier = new Identifier();
        //                    Identifier.identifier = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.identifier;
        //                    Identifier.carrierCode = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.carrierCode;
        //                    Segmentobj.identifier = Identifier;

        //                    int legscount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs.Count;
        //                    List<Leg> Leglist = new List<Leg>();

        //                    for (int m = 0; m < legscount; m++)
        //                    {
        //                        Leg Legobj = new Leg();
        //                        Designator legdesignatorobj = new Designator();
        //                        legdesignatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.origin;
        //                        legdesignatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.destination;
        //                        legdesignatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.departure;
        //                        legdesignatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.arrival;
        //                        Legobj.designator = legdesignatorobj;
        //                        Legobj.legKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
        //                        Legobj.flightReference = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
        //                        Leglist.Add(Legobj);
        //                        LegInfo LegInfo = new LegInfo();
        //                        LegInfo.arrivalTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTerminal;
        //                        LegInfo.departureTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTerminal;
        //                        LegInfo.arrivalTime = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTime;
        //                        LegInfo.departureTime = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTime;
        //                        Legobj.legInfo = LegInfo;

        //                    }
        //                    //  Leglist.Add(Legobj);
        //                    Segmentobj.legs = Leglist;
        //                    Segmentobjlist.Add(Segmentobj);

        //                }


        //                var arrivalTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.arrivalTerminal;
        //                var departureTerminal = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.departureTerminal;
        //                int FareCount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares.Count;


        //                if (FareCount > 0)
        //                {
        //                    List<FareIndividual> fareIndividualsList = new List<FareIndividual>();

        //                    for (int j = 0; j < FareCount; j++)
        //                    {
        //                        //x.data.results[0].trips[0].journeysAvailableByMarket["DEL|BLR"][0].fares[0].fareAvailabilityKey


        //                        FareIndividual fareIndividual = new FareIndividual();

        //                        string fareAvailabilityKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[j].fareAvailabilityKey;
        //                        string fareAvailabilityKeyhead = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[0].fareAvailabilityKey;
        //                        var fareAvilableCount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares.Count;
        //                        var isGoverning = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].isGoverning;
        //                        var procuctclass = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].productClass;
        //                        var passengertype = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].passengerType;
        //                        var fareAmount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].fareAmount;
        //                        fareTotalsum = JsonObj.data.faresAvailable[fareAvailabilityKeyhead].fares[0].passengerFares[0].fareAmount;



        //                        decimal discountamount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].discountedFare;

        //                        int servicecharge = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges.Count;
        //                        decimal finalamount = 0;
        //                        for (int k = 1; k < servicecharge; k++)
        //                        {

        //                            decimal amount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges[k].amount;
        //                            finalamount += amount;

        //                        }
        //                        //TempData["fareTotalsum"] = fareTotalsum;

        //                        decimal taxamount = finalamount;
        //                        fareIndividual.taxamount = taxamount;
        //                        fareIndividual.faretotal = fareAmount;
        //                        fareIndividual.discountamount = discountamount;
        //                        fareIndividual.passengertype = passengertype;
        //                        fareIndividual.fareKey = fareAvailabilityKey;
        //                        fareIndividual.procuctclass = procuctclass;
        //                        fareIndividualsList.Add(fareIndividual);

        //                    }

        //                    var expandoconverter = new ExpandoObjectConverter();
        //                    dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(destination.ToString(), expandoconverter);
        //                    string jsonresult = JsonConvert.SerializeObject(obj);
        //                    _SimpleAvailibilityaAddResponceobj = JsonConvert.DeserializeObject<SimpleAvailibilityaAddResponce>(jsonresult);
        //                    _SimpleAvailibilityaAddResponceobj.designator = Designatorobj;
        //                    _SimpleAvailibilityaAddResponceobj.segments = Segmentobjlist;

        //                    _SimpleAvailibilityaAddResponceobj.arrivalTerminal = arrivalTerminal;
        //                    _SimpleAvailibilityaAddResponceobj.departureTerminal = departureTerminal;
        //                    _SimpleAvailibilityaAddResponceobj.bookingdate = bookingdate;
        //                    _SimpleAvailibilityaAddResponceobj.fareTotalsum = fareTotalsum;
        //                    _SimpleAvailibilityaAddResponceobj.journeyKey = journeyKey;
        //                    _SimpleAvailibilityaAddResponceobj.faresIndividual = fareIndividualsList;
        //                    SimpleAvailibilityaAddResponcelist.Add(_SimpleAvailibilityaAddResponceobj);
        //                }
        //            }


        //        }
        //        TempData["Mymodel"] = JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist);
        //        //method/Controller
        //        return RedirectToAction("FlightView", "ResultFlightView");



        //    }

        //}
        public IActionResult PassengeDetails(Passengers passengers)
        {
            Passengers passengers1 = new Passengers();
            List<_Types> types = new List<_Types>();
            passengers1.types = passengers.types;
            return View();
        }



    }
}

