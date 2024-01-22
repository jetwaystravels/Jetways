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
using Sessionmanager;
using Bookingmanager_;
using System.ComponentModel;
using Utility;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OnionConsumeWebAPI.Extensions;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{
    //[Route("jetways/[controller]/[action]")]
    public class FlightSearchIndexController : Controller
    {

        //    string BaseURL = "https://dotrezapi.test.I5.navitaire.com";

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
            Logs logs = new Logs();
            string destination1 = string.Empty;
            string origin = string.Empty;
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
            int adultcount = _GetfligthModel.passengercount.adultcount;
            int childcount = _GetfligthModel.passengercount.childcount;
            int _infantcount = _GetfligthModel.passengercount.infantcount;
            int TotalCount = adultcount + childcount + _infantcount;

            HttpContext.Session.SetString("adultCount", JsonConvert.SerializeObject(adultcount));
            HttpContext.Session.SetString("childCount", JsonConvert.SerializeObject(childcount));
            HttpContext.Session.SetString("infantCount", JsonConvert.SerializeObject(_infantcount));
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
                HttpResponseMessage responce = await client.PostAsJsonAsync(AppUrlConstant.AirasiaTokan, login);

                if (responce.IsSuccessStatusCode)
                {
                    var results = responce.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    AirasiaTokan.token = JsonObj.data.token;
                    AirasiaTokan.idleTimeoutInMinutes = JsonObj.data.idleTimeoutInMinutes;
                    //token = ((Newtonsoft.Json.Linq.JValue)value).Value.ToString();
                }
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(AirasialoginRequest) + "\n Response: " + JsonConvert.SerializeObject(AirasiaTokan.token), "Logon");


                HttpContext.Session.SetString("AirasiaTokan", JsonConvert.SerializeObject(AirasiaTokan.token));



                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();
                string orgincity = string.Empty;
                string destinationcode = string.Empty;
                string destinationCity = string.Empty;
                string orgincode = string.Empty;

                string input = _GetfligthModel.origin;
                string[] parts = input.Split('-');

                if (parts.Length == 2)
                {
                    orgincity = parts[0].Trim(); // Contains "New Delhi"
                    orgincode = parts[1].Trim(); // Contains "DEL"
                    _GetfligthModel.origin = orgincode;
                }
                input = _GetfligthModel.destination;
                parts = input.Split('-');
                if (parts.Length == 2)
                {
                    destinationCity = parts[0].Trim(); // Contains "New Delhi"
                    destinationcode = parts[1].Trim(); // Contains "DEL"
                    _GetfligthModel.destination = destinationcode;
                }
                _SimpleAvailabilityobj.origin = _GetfligthModel.origin;
                _SimpleAvailabilityobj.destination = _GetfligthModel.destination;
                _SimpleAvailabilityobj.beginDate = _GetfligthModel.beginDate;
                //if (_GetfligthModel.endDate == null)
                //{
                //    _SimpleAvailabilityobj.endDate = _GetfligthModel.beginDate;
                //}
                //else
                _SimpleAvailabilityobj.endDate = _GetfligthModel.endDate;


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
                string[] fareTypes = new string[2];
                fareTypes[0] = "R";
                fareTypes[1] = "M";

                string[] productClasses = new string[3];
                productClasses[0] = "EC";
                productClasses[1] = "HF";
                productClasses[2] = "EP";
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
                List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelistR = new List<SimpleAvailibilityaAddResponce>();
                SimpleAvailibilityaAddResponce _SimpleAvailibilityaAddResponceobjR = new SimpleAvailibilityaAddResponce();

                var json = JsonConvert.SerializeObject(_SimpleAvailabilityobj, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AirasiaTokan.token);
                HttpResponseMessage responce1 = await client.PostAsJsonAsync(AppUrlConstant.Airasiasearchsimple, _SimpleAvailabilityobj);
                int uniqueidx = 0;
                if (responce1.IsSuccessStatusCode)
                {
                    var results = responce1.Content.ReadAsStringAsync().Result;
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_SimpleAvailabilityobj) + "\n Response: " + JsonConvert.SerializeObject(results), "GetAvailability");
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    // var value = JsonObj.data.token;
                    //var value = JsonObj.data.results[0].trips[0].date;
                    var oriDes = _GetfligthModel.origin + "|" + _GetfligthModel.destination;
                    TempData["origin"] = _SimpleAvailabilityobj.origin;
                    TempData["destination"] = _SimpleAvailabilityobj.destination;

                    var finddate = JsonObj.data.results[0].trips[0].date;
                    var bookingdate = finddate.ToString("dddd, dd MMMM yyyy");


                    //var abc = JsonObj.data.results[0].trips[0].journeysAvailableByMarket["DEL|BOM"][3];
                    int count = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count;
                    TempData["count"] = count;



                    for (int i = 0; i < JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count; i++)
                    {


                        string journeyKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].journeyKey;
                        //journeyKey1 = ((Newtonsoft.Json.Linq.JValue)journeyKey).Value.ToString();
                        var uniqueJourney = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i];
                        Designator Designatorobj = new Designator();
                        string queryorigin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.origin;
                        origin = Citydata.GetAllcity().Where(x => x.cityCode == queryorigin).SingleOrDefault().cityName;
                        Designatorobj.origin = origin;
                        string querydestination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.destination;
                        destination1 = Citydata.GetAllcity().Where(x => x.cityCode == querydestination).SingleOrDefault().cityName;
                        Designatorobj.destination = destination1;
                        Designatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.departure;
                        Designatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.arrival;
                        //Citydata citydata1 = new Citydata();


                        var segmentscount = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments.Count;
                        List<DomainLayer.Model.Segment> Segmentobjlist = new List<DomainLayer.Model.Segment>();

                        for (int l = 0; l < segmentscount; l++)
                        {
                            DomainLayer.Model.Segment Segmentobj = new DomainLayer.Model.Segment();
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
                            List<DomainLayer.Model.Leg> Leglist = new List<DomainLayer.Model.Leg>();

                            for (int m = 0; m < legscount; m++)
                            {
                                DomainLayer.Model.Leg Legobj = new DomainLayer.Model.Leg();
                                Designator legdesignatorobj = new Designator();
                                legdesignatorobj.origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.origin;
                                legdesignatorobj.destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.destination;
                                legdesignatorobj.departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.departure;
                                legdesignatorobj.arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.arrival;
                                Legobj.designator = legdesignatorobj;
                                Legobj.legKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
                                Legobj.flightReference = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
                                Leglist.Add(Legobj);
                                DomainLayer.Model.LegInfo LegInfo = new DomainLayer.Model.LegInfo();
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
                            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(uniqueJourney.ToString(), expandoconverter);
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
                            _SimpleAvailibilityaAddResponceobj.uniqueId = i;
                            _SimpleAvailibilityaAddResponceobj.Airline = Airlines.Airasia;
                            _SimpleAvailibilityaAddResponceobj.uniqueId = uniqueidx;
                            uniqueidx++;
                            SimpleAvailibilityaAddResponcelist.Add(_SimpleAvailibilityaAddResponceobj);
                        }
                    }


                }


                #region spicejet
                List<SimpleAvailibilityaAddResponce> SpiceJetAvailibilityaAddResponcelist = new List<SimpleAvailibilityaAddResponce>();
                //Logon 
                #region Logon
                LogonRequest _logonRequestobj = new LogonRequest();
                _logonRequestobj.ContractVersion = 420;
                LogonRequestData LogonRequestDataobj = new LogonRequestData();
                LogonRequestDataobj.AgentName = "APITESTID";
                LogonRequestDataobj.DomainCode = "WWW";
                LogonRequestDataobj.Password = "Spice@123";
                _logonRequestobj.logonRequestData = LogonRequestDataobj;

                SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                LogonResponse _logonResponseobj = await objSpiceJet.Signature(_logonRequestobj);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logonRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logonResponseobj), "Logon");


                #endregion
                //GetAvailability
                #region GetAvailability
                GetAvailabilityVer2Response _getAvailabilityRS = null;
                GetAvailabilityRequest _getAvailabilityRQ = null;
                _getAvailabilityRQ = new GetAvailabilityRequest();
                _getAvailabilityRQ.Signature = _logonResponseobj.Signature;
                _getAvailabilityRQ.ContractVersion = _logonRequestobj.ContractVersion;


                //_GetfligthModel.origin = "BOM";
                //_GetfligthModel.destination = "IXJ";
                _getAvailabilityRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.origin;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.destination;

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime("2024-01-18");
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
                //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime("2024-01-18");
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = default;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.


                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "SG";

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

                //string[] faretypes = { "R", "MX", "IO", "SF" };
                string[] faretypes = { "R" };
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypes;

                string[] productclasses = new string[1];
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclasses;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

                HttpContext.Session.SetString("SpicejetSignature", JsonConvert.SerializeObject(_getAvailabilityRQ.Signature));
                HttpContext.Session.SetString("SpicejetAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityRQ));

                GetAvailabilityVer2Response _getAvailabilityVer2Response = await objSpiceJet.GetAvailabilityVer2Async(_getAvailabilityRQ);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getAvailabilityRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2Response), "GetAvailability");

                //list of spicejet flights
                int count1 = 0;
                if (_getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0].Length > 0)
                {
                    count1 = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys.Length;
                }
                for (int i = 0; i < count1; i++)
                {

                    string _journeysellkey = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey;

                    _SimpleAvailibilityaAddResponceobj = new SimpleAvailibilityaAddResponce();
                    string journeyKey = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey;
                    Designator Designatorobj = new Designator();
                    Designatorobj.origin = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].DepartureStation;
                    Designatorobj.destination = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].ArrivalStation;

                    string journeykey = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey.ToString();
                    string departureTime = Regex.Match(journeykey, @Designatorobj.origin + @"[\s\S]*?\s(?<STD>[\s\S]*?)~").Groups["STD"].Value.Trim();
                    string arrivalTime = Regex.Match(journeykey, @Designatorobj.destination + @"[\s\S]*?\s(?<STA>[\s\S]*?)~").Groups["STA"].Value.Trim();

                    Designatorobj.departure = Convert.ToDateTime(departureTime);


                    Designatorobj.arrival = Convert.ToDateTime(arrivalTime);


                    var segmentscount = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment.Length;
                    List<DomainLayer.Model.Segment> Segmentobjlist = new List<DomainLayer.Model.Segment>();
                    List<FareIndividual> fareIndividualsList = new List<FareIndividual>();
                    List<FareIndividual> fareIndividualsconnectedList = new List<FareIndividual>();
                    for (int l = 0; l < segmentscount; l++)
                    {
                        //Designatorobj.departure = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STD;
                        //Designatorobj.arrival = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STA;


                        DomainLayer.Model.Segment Segmentobj = new DomainLayer.Model.Segment();
                        Designator SegmentDesignatorobj = new Designator();
                        SegmentDesignatorobj.origin = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].DepartureStation;
                        SegmentDesignatorobj.destination = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].ArrivalStation; ;
                        SegmentDesignatorobj.departure = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STD;
                        SegmentDesignatorobj.arrival = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STA;
                        Segmentobj.designator = SegmentDesignatorobj;
                        Identifier Identifier = new Identifier();
                        Identifier.identifier = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].FlightDesignator.FlightNumber; ;
                        Identifier.carrierCode = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].FlightDesignator.CarrierCode;
                        Segmentobj.identifier = Identifier;

                        int legscount = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs.Length;
                        List<DomainLayer.Model.Leg> Leglist = new List<DomainLayer.Model.Leg>();

                        for (int m = 0; m < legscount; m++)
                        {
                            DomainLayer.Model.Leg Legobj = new DomainLayer.Model.Leg();
                            Designator legdesignatorobj = new Designator();
                            legdesignatorobj.origin = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].DepartureStation; ;
                            legdesignatorobj.destination = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].ArrivalStation; legdesignatorobj.departure = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].STD;
                            legdesignatorobj.arrival = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].STA;
                            Legobj.designator = legdesignatorobj;
                            //Legobj.legKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
                            //Legobj.flightReference = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
                            Leglist.Add(Legobj);

                            DomainLayer.Model.LegInfo LegInfo = new DomainLayer.Model.LegInfo();
                            LegInfo.arrivalTerminal = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.ArrivalTerminal;
                            LegInfo.departureTerminal = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.DepartureTerminal;
                            LegInfo.arrivalTime = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.PaxSTA;
                            LegInfo.departureTime = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.PaxSTD;
                            var arrivalTerminal = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.ArrivalTerminal;
                            var departureTerminal = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.DepartureTerminal;

                            Legobj.legInfo = LegInfo;


                            _SimpleAvailibilityaAddResponceobj.arrivalTerminal = arrivalTerminal;
                            _SimpleAvailibilityaAddResponceobj.departureTerminal = departureTerminal;

                        }
                        Segmentobj.legs = Leglist;
                        Segmentobjlist.Add(Segmentobj);
                        FareIndividual fareIndividual = new FareIndividual();
                        for (int k2 = 0; k2 < _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].AvailableFares.Length; k2++)
                        {

                            string fareindex = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].AvailableFares[k2].FareIndex.ToString();

                            #region fare
                            int FareCount = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares.Length;

                            if (FareCount > 0)
                            {
                                //fareIndividualsList = new List<FareIndividual>();

                                try
                                {
                                    for (int j = 0; j < FareCount; j++)
                                    {
                                        if (fareindex == j.ToString())
                                        {

                                            fareIndividual = new FareIndividual();
                                            string _fareSellkey = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                            string fareAvailabilityKey = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                            string fareAvailabilityKeyhead = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                            //var fareAvilableCount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares.Count;
                                            //var isGoverning = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].isGoverning;
                                            var procuctclass = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].ProductClass;
                                            var passengertype = "";
                                            decimal fareAmount = 0.0M;
                                            int servicecharge = 0;
                                            servicecharge = 0;
                                            if (_getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares.Length > 0)
                                            {
                                                passengertype = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].PaxType;
                                                fareAmount = Math.Round(_getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[0].Amount);
                                                fareTotalsum = Math.Round(_getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[0].Amount);
                                                servicecharge = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges.Length;

                                            }
                                            else
                                            {

                                            }

                                            decimal discountamount = 0M;// JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].discountedFare;

                                            decimal finalamount = 0;
                                            //for (int k = 1; k < servicecharge; k++) // one way
                                            for (int k = 0; k < servicecharge; k++)
                                            {

                                                decimal amount = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[k].Amount;
                                                finalamount += amount;

                                            }
                                            decimal taxamount = finalamount;
                                            fareIndividual.taxamount = taxamount;
                                            fareIndividual.faretotal = fareAmount;
                                            fareIndividual.discountamount = discountamount;
                                            fareIndividual.passengertype = passengertype;
                                            fareIndividual.fareKey = fareAvailabilityKey;
                                            fareIndividual.procuctclass = procuctclass;
                                            fareIndividualsList.Add(fareIndividual);
                                        }
                                        else
                                            continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }

                        fareIndividualsconnectedList = new List<FareIndividual>();
                        if (segmentscount > 1)
                        {
                            fareIndividualsconnectedList = new List<FareIndividual>();
                            string SCfarekey = string.Empty;
                            decimal SCAmount = decimal.Zero;
                            string MXfarekey = string.Empty;
                            decimal MXAmount = decimal.Zero;
                            for (int i3 = 0; i3 < fareIndividualsList.Count; i3++)
                            {

                                if (fareIndividualsList[i3].procuctclass == "SC")
                                {
                                    if (string.IsNullOrEmpty(SCfarekey))
                                    {
                                        SCfarekey += fareIndividualsList[i3].fareKey;
                                    }
                                    else
                                    {
                                        SCfarekey += "^" + fareIndividualsList[i3].fareKey;
                                    }

                                    SCAmount += fareIndividualsList[i3].faretotal;
                                }
                                else if (fareIndividualsList[i3].procuctclass == "RS")
                                {
                                    if (string.IsNullOrEmpty(MXfarekey))
                                    {
                                        MXfarekey += fareIndividualsList[i3].fareKey;
                                    }
                                    else
                                    {
                                        MXfarekey += "^" + fareIndividualsList[i3].fareKey;
                                    }

                                    MXAmount += fareIndividualsList[i3].faretotal;
                                }

                            }
                            FareIndividual SCfare = new FareIndividual();
                            SCfare.fareKey = SCfarekey;
                            SCfare.faretotal = SCAmount;
                            SCfare.procuctclass = "SC";

                            FareIndividual MXfare = new FareIndividual();
                            MXfare.fareKey = MXfarekey;
                            MXfare.faretotal = MXAmount;
                            SCfare.procuctclass = "RS";


                            fareIndividualsconnectedList.Add(SCfare);
                            fareIndividualsconnectedList.Add(MXfare);

                        }
                        else
                        {
                            fareIndividualsconnectedList = fareIndividualsList;
                        }
                        //fareIndividualsList.Add(fareIndividual);

                        //}
                        #endregion

                    }

                    //Legobj.designator = Designatorobj;

                    _SimpleAvailibilityaAddResponceobj.designator = Designatorobj;
                    _SimpleAvailibilityaAddResponceobj.segments = Segmentobjlist;
                    _SimpleAvailibilityaAddResponceobj.stops = Segmentobjlist.Count - 1;


                    var bookingdate = "2023-12-10T00:00:00";
                    _SimpleAvailibilityaAddResponceobj.bookingdate = Convert.ToDateTime(bookingdate).ToString("dddd, dd MMM yyyy");
                    _SimpleAvailibilityaAddResponceobj.fareTotalsum = fareTotalsum;

                    _SimpleAvailibilityaAddResponceobj.journeyKey = journeyKey;
                    _SimpleAvailibilityaAddResponceobj.faresIndividual = fareIndividualsconnectedList;// fareIndividualsList;
                    _SimpleAvailibilityaAddResponceobj.uniqueId = uniqueidx;
                    _SimpleAvailibilityaAddResponceobj.Airline = Airlines.Spicejet;
                    uniqueidx++;
                    SpiceJetAvailibilityaAddResponcelist.Add(_SimpleAvailibilityaAddResponceobj);
                    SimpleAvailibilityaAddResponcelist.Add(_SimpleAvailibilityaAddResponceobj);
                }
                string str1 = JsonConvert.SerializeObject(_getAvailabilityVer2Response);

                #endregion

                #endregion
                if (_SimpleAvailabilityobj.beginDate != null && _SimpleAvailabilityobj.endDate != null)
                {


                    uniqueidx = 0;
                    ////Roundtripcode for AirAsia
                    SimpleAvailibilityaAddResponcelistR = new List<SimpleAvailibilityaAddResponce>();
                    _SimpleAvailibilityaAddResponceobjR = new SimpleAvailibilityaAddResponce();

                    DomainLayer.Model.SimpleAvailabilityRequestModel _SimpleAvailabilityobjR = new DomainLayer.Model.SimpleAvailabilityRequestModel();
                    _SimpleAvailabilityobjR.origin = _GetfligthModel.destination;
                    _SimpleAvailabilityobjR.destination = _GetfligthModel.origin;
                    _SimpleAvailabilityobjR.beginDate = _GetfligthModel.endDate;
                    //_SimpleAvailabilityobj.endDate = "2023-12-20";//_GetfligthModel.endDate;
                    Codessimple _codesR = new Codessimple();


                    List<Typesimple> _typeslistR = new List<Typesimple>();
                    var AdtTypeR = _GetfligthModel.passengercount.adulttype;
                    var AdtCountR = _GetfligthModel.passengercount.adultcount;
                    var chdtypeR = _GetfligthModel.passengercount.childtype;
                    var chdcountR = _GetfligthModel.passengercount.childcount;
                    var infanttypeR = _GetfligthModel.passengercount.infanttype;
                    var infantcountR = _GetfligthModel.passengercount.infantcount;

                    if (AdtTypeR == "ADT" && AdtCountR != 0)
                    {
                        Typesimple TypesR = new Typesimple();
                        TypesR.type = AdtType;
                        TypesR.count = AdtCount;

                        _typeslistR.Add(TypesR);
                    }
                    if (chdtypeR == "CHD" && chdcountR != 0)
                    {
                        Typesimple TypesR = new Typesimple();
                        TypesR.type = chdtype;
                        TypesR.count = chdcount;

                        _typeslistR.Add(TypesR);
                    }
                    if (infanttypeR == "INFT" && infantcountR != 0)
                    {
                        Typesimple TypesR = new Typesimple();
                        TypesR.type = infanttype;
                        TypesR.count = infantcount;

                        _typeslistR.Add(TypesR);
                    }


                    Passengerssimple _PassengerssimpleR = new Passengerssimple();
                    _PassengerssimpleR.types = _typeslist;
                    _SimpleAvailabilityobjR.passengers = _Passengerssimple;

                    //_codes.currencyCode = "INR";
                    _SimpleAvailabilityobjR.codes = _codes;
                    _SimpleAvailabilityobjR.sourceOrganization = "";
                    _SimpleAvailabilityobjR.currentSourceOrganization = "";
                    _SimpleAvailabilityobjR.promotionCode = "";
                    string[] sortOptionsR = new string[1];
                    sortOptionsR[0] = "ServiceType";
                    string[] fareTypesR = new string[2];
                    fareTypesR[0] = "R";
                    fareTypesR[1] = "M";

                    string[] productClassesR = new string[3];
                    productClassesR[0] = "EC";
                    productClassesR[1] = "HF";
                    productClassesR[2] = "EP";
                    Filters FiltersR = new Filters();
                    FiltersR.exclusionType = "Default";
                    FiltersR.loyalty = "MonetaryOnly";
                    FiltersR.includeAllotments = true;
                    FiltersR.connectionType = "Both";
                    FiltersR.compressionType = "CompressByProductClass";
                    FiltersR.sortOptions = sortOptions;
                    FiltersR.maxConnections = 10;
                    FiltersR.fareTypes = fareTypes;
                    FiltersR.productClasses = productClasses;
                    _SimpleAvailabilityobjR.filters = Filters;
                    _SimpleAvailabilityobjR.taxesAndFees = "Taxes";
                    _SimpleAvailabilityobjR.ssrCollectionsMode = "Leg";
                    _SimpleAvailabilityobjR.numberOfFaresPerJourney = 10;
                    var jsonR = JsonConvert.SerializeObject(_SimpleAvailabilityobjR, Formatting.Indented);

                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AirasiaTokan.token);
                    HttpResponseMessage responceR = await client.PostAsJsonAsync(AppUrlConstant.AirasiasearchsimpleR, _SimpleAvailabilityobjR);
                    if (responceR.IsSuccessStatusCode)
                    {
                        var resultsR = responceR.Content.ReadAsStringAsync().Result;
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SimpleAvailabilityobjR) + "Url: " + AppUrlConstant.AirasiasearchsimpleR + "\n Response: " + JsonConvert.SerializeObject(resultsR), "GetAvailability", "AirAsiaRT");
                        var JsonObjR = JsonConvert.DeserializeObject<dynamic>(resultsR);
                        // var value = JsonObj.data.token;
                        //var value = JsonObj.data.results[0].trips[0].date;
                        var oriDes = _GetfligthModel.destination + "|" + _GetfligthModel.origin;
                        TempData["originR"] = _SimpleAvailabilityobj.origin;
                        TempData["destinationR"] = _SimpleAvailabilityobj.destination;



                        var finddate = JsonObjR.data.results[0].trips[0].date;
                        var bookingdate = finddate.ToString("dddd, dd MMMM yyyy");



                        int count = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count;
                        TempData["countr"] = count;



                        for (int i = 0; i < JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes].Count; i++)
                        {

                            string journeyKey = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].journeyKey;
                            //journeyKey1 = ((Newtonsoft.Json.Linq.JValue)journeyKey).Value.ToString();
                            var destination = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i];
                            Designator Designatorobj = new Designator();
                            Designatorobj.origin = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.origin;
                            Designatorobj.destination = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.destination;
                            Designatorobj.departure = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.departure;
                            Designatorobj.arrival = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].designator.arrival;

                            var segmentscount = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments.Count;
                            List<DomainLayer.Model.Segment> Segmentobjlist = new List<DomainLayer.Model.Segment>();

                            for (int l = 0; l < segmentscount; l++)
                            {
                                DomainLayer.Model.Segment Segmentobj = new DomainLayer.Model.Segment();
                                Designator SegmentDesignatorobj = new Designator();
                                SegmentDesignatorobj.origin = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.origin;
                                SegmentDesignatorobj.destination = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.destination;
                                SegmentDesignatorobj.departure = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.departure;
                                SegmentDesignatorobj.arrival = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].designator.arrival;
                                Segmentobj.designator = SegmentDesignatorobj;
                                Identifier Identifier = new Identifier();
                                Identifier.identifier = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.identifier;
                                Identifier.carrierCode = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].identifier.carrierCode;
                                Segmentobj.identifier = Identifier;

                                int legscount = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs.Count;
                                List<DomainLayer.Model.Leg> Leglist = new List<DomainLayer.Model.Leg>();
                                for (int m = 0; m < legscount; m++)
                                {
                                    DomainLayer.Model.Leg Legobj = new DomainLayer.Model.Leg();
                                    Designator legdesignatorobj = new Designator();
                                    legdesignatorobj.origin = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.origin;
                                    legdesignatorobj.destination = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.destination;
                                    legdesignatorobj.departure = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.departure;
                                    legdesignatorobj.arrival = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].designator.arrival;
                                    Legobj.designator = legdesignatorobj;
                                    Legobj.legKey = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
                                    Legobj.flightReference = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
                                    Leglist.Add(Legobj);
                                    DomainLayer.Model.LegInfo LegInfo = new DomainLayer.Model.LegInfo();
                                    LegInfo.arrivalTerminal = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTerminal;
                                    LegInfo.departureTerminal = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTerminal;
                                    LegInfo.arrivalTime = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.arrivalTime;
                                    LegInfo.departureTime = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legInfo.departureTime;
                                    Legobj.legInfo = LegInfo;

                                }
                                //  Leglist.Add(Legobj);
                                Segmentobj.legs = Leglist;
                                Segmentobjlist.Add(Segmentobj);

                            }


                            var arrivalTerminal = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.arrivalTerminal;
                            var departureTerminal = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[0].legs[0].legInfo.departureTerminal;
                            int FareCount = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares.Count;


                            if (FareCount > 0)
                            {
                                List<FareIndividual> fareIndividualsList = new List<FareIndividual>();

                                for (int j = 0; j < FareCount; j++)
                                {
                                    //x.data.results[0].trips[0].journeysAvailableByMarket["DEL|BLR"][0].fares[0].fareAvailabilityKey


                                    FareIndividual fareIndividual = new FareIndividual();

                                    string fareAvailabilityKey = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[j].fareAvailabilityKey;
                                    string fareAvailabilityKeyhead = JsonObjR.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].fares[0].fareAvailabilityKey;
                                    var fareAvilableCount = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares.Count;
                                    var isGoverning = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].isGoverning;
                                    var procuctclass = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].productClass;
                                    var passengertype = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].passengerType;
                                    var fareAmount = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].fareAmount;
                                    fareTotalsum = JsonObjR.data.faresAvailable[fareAvailabilityKeyhead].fares[0].passengerFares[0].fareAmount;



                                    decimal discountamount = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].discountedFare;

                                    int servicecharge = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges.Count;
                                    decimal finalamount = 0;
                                    for (int k = 1; k < servicecharge; k++)
                                    {

                                        decimal amount = JsonObjR.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].serviceCharges[k].amount;
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
                                dynamic objR = JsonConvert.DeserializeObject<ExpandoObject>(destination.ToString(), expandoconverter);
                                string jsonresultR = JsonConvert.SerializeObject(objR);
                                _SimpleAvailibilityaAddResponceobjR = JsonConvert.DeserializeObject<SimpleAvailibilityaAddResponce>(jsonresultR);
                                _SimpleAvailibilityaAddResponceobjR.designator = Designatorobj;
                                _SimpleAvailibilityaAddResponceobjR.segments = Segmentobjlist;

                                _SimpleAvailibilityaAddResponceobjR.arrivalTerminal = arrivalTerminal;
                                _SimpleAvailibilityaAddResponceobjR.departureTerminal = departureTerminal;
                                _SimpleAvailibilityaAddResponceobjR.bookingdate = bookingdate;
                                _SimpleAvailibilityaAddResponceobjR.fareTotalsum = fareTotalsum;
                                _SimpleAvailibilityaAddResponceobjR.journeyKey = journeyKey;
                                _SimpleAvailibilityaAddResponceobjR.uniqueId = i;
                                _SimpleAvailibilityaAddResponceobjR.faresIndividual = fareIndividualsList;
                                _SimpleAvailibilityaAddResponceobjR.uniqueId = uniqueidx;
                                _SimpleAvailibilityaAddResponceobjR.Airline = Airlines.Airasia;
                                uniqueidx++;
                                SimpleAvailibilityaAddResponcelistR.Add(_SimpleAvailibilityaAddResponceobjR);
                            }
                        }


                    }
                    //Roundtripcode for SpiceJet
                    #region spicejet
                    List<SimpleAvailibilityaAddResponce> SpiceJetAvailibilityaAddResponcelistR = new List<SimpleAvailibilityaAddResponce>();
                    //Logon 
                    #region Logon
                    LogonRequest _logonRequestobjR = new LogonRequest();
                    _logonRequestobjR.ContractVersion = 420;
                    LogonRequestData LogonRequestDataobjR = new LogonRequestData();
                    LogonRequestDataobjR.AgentName = "APITESTID";
                    LogonRequestDataobjR.DomainCode = "WWW";
                    LogonRequestDataobjR.Password = "Spice@123";
                    _logonRequestobjR.logonRequestData = LogonRequestDataobjR;

                    SpiceJetApiController objSpiceJetR = new SpiceJetApiController();
                    LogonResponse _logonResponseobjR = await objSpiceJet.Signature(_logonRequestobjR);

                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_logonRequestobjR) + "\n Response: " + JsonConvert.SerializeObject(_logonResponseobjR), "Logon", "SpiceJetRT");


                    #endregion
                    //GetAvailability
                    #region GetAvailability
                    GetAvailabilityVer2Response _getAvailabilityReturnRS = null;
                    GetAvailabilityRequest _getAvailabilityReturnRQ = null;
                    _getAvailabilityReturnRQ = new GetAvailabilityRequest();
                    _getAvailabilityReturnRQ.Signature = _logonResponseobjR.Signature;
                    _getAvailabilityReturnRQ.ContractVersion = _logonRequestobjR.ContractVersion;


                    //_GetfligthModel.origin = "BOM";
                    //_GetfligthModel.destination = "IXJ";
                    _getAvailabilityReturnRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.destination; //return_origin

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.origin; //return_depart

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                    //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime("2024-01-18");
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
                    //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime("2024-01-18");
                    //_getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = default;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.


                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "SG";

                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

                    string[] faretypesreturn = { "R", "MX", "IO", "SF" };
                    // string[] faretypes = {"R"};
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypesreturn;

                    string[] productclassesreturn = new string[1];
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
                    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

                    HttpContext.Session.SetString("SpicejetReturnSignature", JsonConvert.SerializeObject(_logonResponseobjR.Signature));
                    HttpContext.Session.SetString("SpicejetAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));

                    GetAvailabilityVer2Response _getAvailabilityVer2ReturnResponse = await objSpiceJet.GetAvailabilityVer2Async(_getAvailabilityReturnRQ);

                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "SpiceJetRT");


                    int count2 = 0;
                    if (_getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0].Length > 0)
                    {

                        count2 = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys.Length;
                    }
                    for (int i = 0; i < count2; i++)
                    {
                        string _journeysellkey = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey;

                        _SimpleAvailibilityaAddResponceobjR = new SimpleAvailibilityaAddResponce();
                        string journeyKey = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey;
                        Designator Designatorobj = new Designator();
                        Designatorobj.origin = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].DepartureStation;
                        Designatorobj.destination = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].ArrivalStation;

                        string journeykey = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].JourneySellKey.ToString();
                        string departureTime = Regex.Match(journeykey, @Designatorobj.origin + @"[\s\S]*?\s(?<STD>[\s\S]*?)~").Groups["STD"].Value.Trim();
                        string arrivalTime = Regex.Match(journeykey, @Designatorobj.destination + @"[\s\S]*?\s(?<STA>[\s\S]*?)~").Groups["STA"].Value.Trim();

                        Designatorobj.departure = Convert.ToDateTime(departureTime);


                        Designatorobj.arrival = Convert.ToDateTime(arrivalTime);


                        var segmentscount = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment.Length;
                        List<DomainLayer.Model.Segment> Segmentobjlist = new List<DomainLayer.Model.Segment>();
                        List<FareIndividual> fareIndividualsList = new List<FareIndividual>();
                        List<FareIndividual> fareIndividualsconnectedList = new List<FareIndividual>();
                        for (int l = 0; l < segmentscount; l++)
                        {
                            //Designatorobj.departure = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STD;
                            //Designatorobj.arrival = _getAvailabilityVer2Response.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STA;


                            DomainLayer.Model.Segment Segmentobj = new DomainLayer.Model.Segment();
                            Designator SegmentDesignatorobj = new Designator();
                            SegmentDesignatorobj.origin = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].DepartureStation;
                            SegmentDesignatorobj.destination = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].ArrivalStation; ;
                            SegmentDesignatorobj.departure = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STD;
                            SegmentDesignatorobj.arrival = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].STA;
                            Segmentobj.designator = SegmentDesignatorobj;
                            Identifier Identifier = new Identifier();
                            Identifier.identifier = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].FlightDesignator.FlightNumber; ;
                            Identifier.carrierCode = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].FlightDesignator.CarrierCode;
                            Segmentobj.identifier = Identifier;

                            int legscount = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs.Length;
                            List<DomainLayer.Model.Leg> Leglist = new List<DomainLayer.Model.Leg>();

                            for (int m = 0; m < legscount; m++)
                            {
                                DomainLayer.Model.Leg Legobj = new DomainLayer.Model.Leg();
                                Designator legdesignatorobj = new Designator();
                                legdesignatorobj.origin = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].DepartureStation; ;
                                legdesignatorobj.destination = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].ArrivalStation;
                                legdesignatorobj.departure = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].STD;
                                legdesignatorobj.arrival = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].STA;
                                Legobj.designator = legdesignatorobj;
                                //Legobj.legKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].legKey;
                                //Legobj.flightReference = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][i].segments[l].legs[m].flightReference;
                                Leglist.Add(Legobj);

                                DomainLayer.Model.LegInfo LegInfo = new DomainLayer.Model.LegInfo();
                                LegInfo.arrivalTerminal = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.ArrivalTerminal;
                                LegInfo.departureTerminal = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.DepartureTerminal;
                                LegInfo.arrivalTime = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.PaxSTA;
                                LegInfo.departureTime = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.PaxSTD;
                                var arrivalTerminal = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.ArrivalTerminal;
                                var departureTerminal = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].Legs[m].LegInfo.DepartureTerminal;

                                Legobj.legInfo = LegInfo;


                                _SimpleAvailibilityaAddResponceobjR.arrivalTerminal = arrivalTerminal;
                                _SimpleAvailibilityaAddResponceobjR.departureTerminal = departureTerminal;

                            }
                            Segmentobj.legs = Leglist;
                            Segmentobjlist.Add(Segmentobj);
                            FareIndividual fareIndividual = new FareIndividual();
                            for (int k2 = 0; k2 < _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].AvailableFares.Length; k2++)
                            {

                                string fareindex = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Schedules[0][0].AvailableJourneys[i].AvailableSegment[l].AvailableFares[k2].FareIndex.ToString();

                                #region fare
                                int FareCount = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares.Length;

                                if (FareCount > 0)
                                {
                                    //fareIndividualsList = new List<FareIndividual>();

                                    try
                                    {
                                        for (int j = 0; j < FareCount; j++)
                                        {
                                            if (fareindex == j.ToString())
                                            {

                                                fareIndividual = new FareIndividual();
                                                string _fareSellkey = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                                string fareAvailabilityKey = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                                string fareAvailabilityKeyhead = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].FareSellKey;
                                                //var fareAvilableCount = JsonObj.data.faresAvailable[fareAvailabilityKey].fares.Count;
                                                //var isGoverning = JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].isGoverning;
                                                var procuctclass = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].ProductClass;
                                                var passengertype = "";
                                                decimal fareAmount = 0.0M;
                                                int servicecharge = 0;
                                                servicecharge = 0;
                                                if (_getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares.Length > 0)
                                                {
                                                    passengertype = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].PaxType;
                                                    fareAmount = Math.Round(_getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[0].Amount);
                                                    fareTotalsum = Math.Round(_getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[0].Amount);
                                                    servicecharge = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges.Length;

                                                }
                                                else
                                                {

                                                }

                                                decimal discountamount = 0M;// JsonObj.data.faresAvailable[fareAvailabilityKey].fares[0].passengerFares[0].discountedFare;

                                                decimal finalamount = 0;
                                                //for (int k = 1; k < servicecharge; k++) // one way
                                                for (int k = 0; k < servicecharge; k++)
                                                {

                                                    decimal amount = _getAvailabilityVer2ReturnResponse.GetTripAvailabilityVer2Response.Fares[j].PaxFares[0].ServiceCharges[k].Amount;
                                                    finalamount += amount;

                                                }
                                                decimal taxamount = finalamount;
                                                fareIndividual.taxamount = taxamount;
                                                fareIndividual.faretotal = fareAmount;
                                                fareIndividual.discountamount = discountamount;
                                                fareIndividual.passengertype = passengertype;
                                                fareIndividual.fareKey = fareAvailabilityKey;
                                                fareIndividual.procuctclass = procuctclass;
                                                fareIndividualsList.Add(fareIndividual);
                                            }
                                            else
                                                continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }

                            fareIndividualsconnectedList = new List<FareIndividual>();
                            if (segmentscount > 1)
                            {
                                fareIndividualsconnectedList = new List<FareIndividual>();
                                string SCfarekey = string.Empty;
                                decimal SCAmount = decimal.Zero;
                                string MXfarekey = string.Empty;
                                decimal MXAmount = decimal.Zero;
                                for (int i3 = 0; i3 < fareIndividualsList.Count; i3++)
                                {

                                    if (fareIndividualsList[i3].procuctclass == "SC")
                                    {
                                        if (string.IsNullOrEmpty(SCfarekey))
                                        {
                                            SCfarekey += fareIndividualsList[i3].fareKey;
                                        }
                                        else
                                        {
                                            SCfarekey += "^" + fareIndividualsList[i3].fareKey;
                                        }

                                        SCAmount += fareIndividualsList[i3].faretotal;
                                    }
                                    else if (fareIndividualsList[i3].procuctclass == "RS")
                                    {
                                        if (string.IsNullOrEmpty(MXfarekey))
                                        {
                                            MXfarekey += fareIndividualsList[i3].fareKey;
                                        }
                                        else
                                        {
                                            MXfarekey += "^" + fareIndividualsList[i3].fareKey;
                                        }

                                        MXAmount += fareIndividualsList[i3].faretotal;
                                    }

                                }
                                FareIndividual SCfare = new FareIndividual();
                                SCfare.fareKey = SCfarekey;
                                SCfare.faretotal = SCAmount;
                                SCfare.procuctclass = "SC";

                                FareIndividual MXfare = new FareIndividual();
                                MXfare.fareKey = MXfarekey;
                                MXfare.faretotal = MXAmount;
                                SCfare.procuctclass = "RS";


                                fareIndividualsconnectedList.Add(SCfare);
                                fareIndividualsconnectedList.Add(MXfare);

                            }
                            else
                            {
                                fareIndividualsconnectedList = fareIndividualsList;
                            }
                            //fareIndividualsList.Add(fareIndividual);

                            //}
                            #endregion

                        }

                        //Legobj.designator = Designatorobj;

                        _SimpleAvailibilityaAddResponceobjR.designator = Designatorobj;
                        _SimpleAvailibilityaAddResponceobjR.segments = Segmentobjlist;
                        _SimpleAvailibilityaAddResponceobjR.stops = Segmentobjlist.Count - 1;

                        DateTime currentDate = DateTime.Now;
                        var bookingdate = currentDate; //"2023-12-10T00:00:00";
                        _SimpleAvailibilityaAddResponceobjR.bookingdate = Convert.ToDateTime(bookingdate).ToString("dddd, dd MMM yyyy");
                        _SimpleAvailibilityaAddResponceobjR.fareTotalsum = fareTotalsum;

                        _SimpleAvailibilityaAddResponceobjR.journeyKey = journeyKey;
                        _SimpleAvailibilityaAddResponceobjR.faresIndividual = fareIndividualsconnectedList;// fareIndividualsList;
                        _SimpleAvailibilityaAddResponceobjR.uniqueId = uniqueidx;
                        _SimpleAvailibilityaAddResponceobjR.Airline = Airlines.Spicejet;
                        uniqueidx++;
                        SpiceJetAvailibilityaAddResponcelistR.Add(_SimpleAvailibilityaAddResponceobjR);
                        SimpleAvailibilityaAddResponcelistR.Add(_SimpleAvailibilityaAddResponceobjR);
                    }


                    string str1Return = JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse);

                    #endregion

                    #endregion
                    //end
                    HttpContext.Session.SetString("LeftReturnViewFlightView", JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist));
                    HttpContext.Session.SetString("PassengerModel", JsonConvert.SerializeObject(_SimpleAvailabilityobj));
                    //TempData["PassengerModel"] = JsonConvert.SerializeObject(_SimpleAvailabilityobj);

                    HttpContext.Session.SetString("RightReturnFlightView", JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelistR));
                    //AirAsia
                    TempData["Mymodel"] = JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist);
                    TempData["PassengerModel"] = JsonConvert.SerializeObject(_SimpleAvailabilityobj);


                    //RoundTrip
                    TempData["MymodelR"] = JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelistR);
                    TempData["PassengerModelR"] = JsonConvert.SerializeObject(_SimpleAvailabilityobjR);
                    HttpContext.Session.SetString("PassengerModelR", JsonConvert.SerializeObject(_SimpleAvailabilityobj));
                    HttpContext.Session.SetString("SpicejetSignautre", JsonConvert.SerializeObject(_logonResponseobjR.Signature));

                    ////SpiceJet
                    TempData["SpiceJetmodel"] = JsonConvert.SerializeObject(SpiceJetAvailibilityaAddResponcelist);
                    TempData["SpiceJetPassengerModel"] = JsonConvert.SerializeObject(_getAvailabilityRQ);

                    return RedirectToAction("RTFlightView", "RoundTrip");
                }
                else
                {
                    HttpContext.Session.SetString("OneWayFlightView", JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist));
                    HttpContext.Session.SetString("OneWayPassengerModel", JsonConvert.SerializeObject(_SimpleAvailabilityobj));
                    TempData["Mymodel"] = JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist);
                    TempData["PassengerModel"] = JsonConvert.SerializeObject(_SimpleAvailabilityobj);
                    ////SpiceJet
                    TempData["SpiceJetmodel"] = JsonConvert.SerializeObject(SpiceJetAvailibilityaAddResponcelist);
                    TempData["SpiceJetPassengerModel"] = JsonConvert.SerializeObject(_getAvailabilityRQ);
                    return RedirectToAction("FlightView", "ResultFlightView");


                }


            }

        }
        public IActionResult PassengeDetails(Passengers passengers)
        {
            Passengers passengers1 = new Passengers();
            List<_Types> types = new List<_Types>();
            passengers1.types = passengers.types;
            return View();
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

