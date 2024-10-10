using System;
using System.Text;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;
using NuGet.DependencyResolver;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Reflection.Metadata;
using System.Data.Common;
//using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Net.Http.Json;
using System.Globalization;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.ContactModel;
using NuGet.Common;

namespace OnionConsumeWebAPI.Controllers
{
    public class SearchdataController : Controller
    {
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string PnrStaus = string.Empty;
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

       
       public async Task <ActionResult> Index(_credentials credentials, GetfligthModel _GetfligthModel, _Types _Types1)
        {

            #region Token
            credentials.username = "otaapi";
            credentials.alternateIdentifier = "";
            credentials.password = "AirAsia@123";
            credentials.domain = "EXT";
            credentials.location = "";
            credentials.channelType = "";
            credentials.loginRole = "";
            airlineLogin login = new airlineLogin();
          //  login.credentials= credentials;
            login.applicationName = "";
            string token = string.Empty;
            string destination1 = string.Empty;
            string origin1 = string.Empty;
            string arrival1 = string.Empty;
            string departure1 = string.Empty;
            string identifier1 = string.Empty;
            string carrierCode1 = string.Empty;
            string totalfare1= string.Empty;
            string journeyKey1 = string.Empty;
            string fareAvailabilityKey1 = string.Empty;
            string inventoryControl1= string.Empty;
            string ssrKey = string.Empty;
            string passengerkey= string.Empty;
            string uniquekey= string.Empty;
          
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var jsonlogin = JsonConvert.SerializeObject(login, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responce = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/token", login);
                if (responce.IsSuccessStatusCode)
                {
                    var results = responce.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    var value = JsonObj.data.token;
                    token = ((Newtonsoft.Json.Linq.JValue)value).Value.ToString();
                }

                #endregion
                   #region Simple Avaibility

                // GetfligthModel _GetfligthModel = new GetfligthModel();
                //_codes _codes = new _codes();
                _Types _Types= new _Types();
                List<_Types> _typeslist = new List<_Types>();
                _Types.count = 1;
                _Types.type = "ADT";
                _typeslist.Add(_Types);
                Passengers passengers =new Passengers();
                passengers.types = _typeslist;
               // _codes.currencyCode= "INR";
                //_GetfligthModel.origin = "DEL";
                //_GetfligthModel.origin = _GetfligthModel.frmCity;
                //_GetfligthModel.destination = "BOM";
                _GetfligthModel.searchDestinationMacs =true;
                _GetfligthModel.searchOriginMacs = true;
                _GetfligthModel.beginDate = "2023-05-08";
                _GetfligthModel.passengers = passengers;           
               // _GetfligthModel.codes = _codes;

                var json=JsonConvert.SerializeObject(_GetfligthModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responce1 = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v4/availability/search/simple", _GetfligthModel);
                if (responce1.IsSuccessStatusCode)
                {
                    var results = responce1.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    // var value = JsonObj.data.token;
                    //var value = JsonObj.data.results[0].trips[0].date;
                    var oriDes = _GetfligthModel.origin + "|" + _GetfligthModel.destination;
                    var destination = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].designator.destination;
                    destination1 = ((Newtonsoft.Json.Linq.JValue)destination).Value.ToString();

                    var origin = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].designator.origin;
                    origin1 = ((Newtonsoft.Json.Linq.JValue)origin).Value.ToString();

                    var departure = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].designator.departure;
                    departure1 = ((Newtonsoft.Json.Linq.JValue)departure).Value.ToString();

                    var arrival = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].designator.arrival;
                    arrival1 = ((Newtonsoft.Json.Linq.JValue)arrival).Value.ToString();


                    var identifier = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].segments[0].identifier.identifier;
                    identifier1 = ((Newtonsoft.Json.Linq.JValue)identifier).Value.ToString();

                    var carrierCode = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].segments[0].identifier.carrierCode;
                    carrierCode1 = ((Newtonsoft.Json.Linq.JValue)carrierCode).Value.ToString();

                    var fareAvailabilityKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].fares[0].fareAvailabilityKey;
                    fareAvailabilityKey1 = ((Newtonsoft.Json.Linq.JValue)fareAvailabilityKey).Value.ToString();
                    //var totalfare = JsonObj.data.faresAvailable.MH5LfiB_STV_S05SQTAwfk5SMDB_fjB_M35_WCEw.totals.fareTotal;
                    var totalfare = JsonObj.data.faresAvailable[fareAvailabilityKey1].totals.fareTotal;
                   // var fare = JsonObj.data.faresAvailable.Value;
                    //var totalfare= fare.totals.fareTotal;
                    totalfare1 = ((Newtonsoft.Json.Linq.JValue)totalfare).Value.ToString();

                    var journeyKey = JsonObj.data.results[0].trips[0].journeysAvailableByMarket[oriDes][0].journeyKey;
                    journeyKey1 = ((Newtonsoft.Json.Linq.JValue)journeyKey).Value.ToString();

                    
                    var inventoryControl = "HoldSpace";

                    var suppressPassengerAgeValidation = true;
                    AirAsiaTripSellRequest _AirAsiaTripSell = new AirAsiaTripSellRequest();
                    _AirAsiaTripSell.preventOverlap = true;
                    _AirAsiaTripSell.suppressPassengerAgeValidation = true;
                   // _AirAsiaTripSell.infantCount = 0;
                    _AirAsiaTripSell.passengers = passengers;
                    _AirAsiaTripSell.currencyCode = "INR";
                    _Key _Key = new _Key();
                    List<_Key> _keylist = new List<_Key>();
                    _Key.journeyKey = journeyKey1;
                    _Key.fareAvailabilityKey = fareAvailabilityKey1;
                    _Key.inventoryControl = inventoryControl;
                    _keylist.Add(_Key);
                    _AirAsiaTripSell.keys = _keylist;
                    #endregion
                    #region tripSell
                    // _AirAsiaTripSell.keys = _Key;
                    var json1 = JsonConvert.SerializeObject(_AirAsiaTripSell, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responseTripsell = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v4/trip/sell", _AirAsiaTripSell);
                    List<passkeytype> listpasskeytypes = new List<passkeytype>();
                    if (responseTripsell.IsSuccessStatusCode)
                    {
                        var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                        var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                        // var passCount= JsonObjTripsell.data.passengers.
                        


                        foreach (var items in JsonObjTripsell.data.passengers)
                        {
                            passkeytype _passkeytype = new passkeytype();
                            _passkeytype.passengerkey = items.Value.passengerKey;
                            _passkeytype.passengertypecode = items.Value.passengerTypeCode;
                            listpasskeytypes.Add(_passkeytype);
                        }

                    }
                    #endregion
                    #region Add Contact
                    //Add Contact
                    ContactModel _ContactModel = new ContactModel();
                    _ContactModel.emailAddress = "kanpur.ashok@gmail.com";
                    _ContactModel.distributionOption = "Email";
                                       
                    _Phonenumber Phonenumber = new _Phonenumber();
                   
                    List<_Phonenumber> Phonenumberlist = new List<_Phonenumber>();
                    Phonenumber.type = "Home";
                    Phonenumber.number = "8427549192";
                    Phonenumberlist.Add(Phonenumber);
                    _Phonenumber Phonenumber1 = new _Phonenumber();
                    Phonenumber1.type = "Other";
                    Phonenumber1.number = "8427549192";                   
                    Phonenumberlist.Add(Phonenumber1);
                    foreach (var item in Phonenumberlist)
                    {
                        
                        _ContactModel.phoneNumbers = Phonenumberlist;
                        

                    }
                   
                                       
                    _ContactModel.contactTypeCode = "P";

                    _Address Address = new _Address();
                    Address.lineOne = "123 Main Street";
                    Address.countryCode = "IN";
                    Address.provinceState = "TN";
                    Address.city = "Chennai";
                    Address.postalCode = "600028";
                    _ContactModel.address = Address;

                    _Name Name = new _Name();
                    Name.first = "Vadivel";
                    Name.middle = "raja";
                    Name.last = "VR";
                    Name.title= "MR";
                    _ContactModel.name = Name;


                    var jsonContact = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/contacts", _ContactModel);
                    if (responseAddContact.IsSuccessStatusCode)
                    {
                        var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                        var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                    }

                    #endregion
                    #region Add Passenger

                    PassengersModel _PassengersModel  = new PassengersModel();
                    Name name = new Name();
                    _Info Info = new _Info();
                    for (int i = 0; i < listpasskeytypes.Count; i++)
                    {
                        name.first = "Ashok";
                        name.last = "Kushwaha";
                        name.middle = "kumar";
                        name.title = "MR";
                        Info.dateOfBirth = "1986-01-01";
                        Info.gender = "Male";
                        Info.nationality = "IN";
                        Info.residentCountry = "IN";

                    }
                    _PassengersModel.name = name;
                    _PassengersModel.info = Info;
                    var passengerKey = listpasskeytypes[0].passengerkey.ToString();
                    var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(BaseURL + "/api/nsk/v3/booking/passengers/"+ passengerKey, _PassengersModel);
                    if (responsePassengers.IsSuccessStatusCode)
                    {
                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                        var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                    }

                    #endregion

                    //#region SSRAvailabilty
                    //SSRAvailabiltyModel _SSRAvailabilty = new SSRAvailabiltyModel();
                    //_SSRAvailabilty.passengerKeys = new string[listpasskeytypes.Count];
                    //for(int i=0;i< listpasskeytypes.Count;i++)
                    //{
                    //    _SSRAvailabilty.passengerKeys[i] = listpasskeytypes[i].passengerkey;                            
                    //}
                    //_SSRAvailabilty.currencyCode = _AirAsiaTripSell.currencyCode;
                    //List<_Trip> _Trips= new List<_Trip>();
                    //_Trip _Trip= new _Trip();
                    //_Trip.origin = origin1;
                    //// _Trip.departureDate = departure1;
                    // _Trip.departureDate = "2023-05-08";
                    //_Identifier _Identifier = new _Identifier();
                    //_Identifier.carrierCode = carrierCode1;
                    //_Identifier.identifier = identifier1;
                    //_Trip.identifier = _Identifier;
                    //_Trip.destination = destination1;
                    //_Trips.Add(_Trip);
                    //_SSRAvailabilty.trips= _Trips;


                    
                    //var jsonSSRAvailabilty = JsonConvert.SerializeObject(_SSRAvailabilty, Formatting.Indented);
                    //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //HttpResponseMessage responseSSRAvailabilty = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/availability", _SSRAvailabilty);
                    //if (responseSSRAvailabilty.IsSuccessStatusCode)
                    //{
                    //    var _responseSSRAvailabilty = responseSSRAvailabilty.Content.ReadAsStringAsync().Result;
                    //    var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);
                    //    var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability["MCFBRFQ-"].ssrKey;
                    //    ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                    //    var passengerkey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability["MCFBRFQ-"].passengerKey;
                    //    passengerkey = ((Newtonsoft.Json.Linq.JValue)passengerkey1).Value.ToString();
                    //}

                    //#endregion

                    #region SellSSR
                    


                    SellSSRModel _sellSSRModel = new SellSSRModel();

                    _sellSSRModel.count = 1;
                    _sellSSRModel.note = "DevTest";
                    _sellSSRModel.forceWaveOnSell= false;
                    _sellSSRModel.currencyCode = "INR";


                    var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                   

                    HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/"+ssrKey, _sellSSRModel);
                    if (responseSellSSR.IsSuccessStatusCode)
                    {
                        var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                        var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                    }

                    #endregion

                    #region SeatMap
                    

                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //HttpResponseMessage responseSeatmap = await client.PostAsJsonAsync(BaseURL + "api/nsk/v3/booking/seatmaps/journey/ {{journeyKey}}", _SSRAvailabilty);
                  //  var REQUEST1= (BaseURL + "/api/nsk/v3/booking/seatmaps/journey/{{" +journeyKey1+ "}}?IncludePropertyLookup=true");
                    HttpResponseMessage responseSeatmap = await client.GetAsync(BaseURL + "/api/nsk/v3/booking/seatmaps/journey/"+journeyKey1+"?IncludePropertyLookup=true");

                   // data[0].seatMap.decks['1'].compartments.Y.units[0].unitKey


                    if (responseSeatmap.IsSuccessStatusCode)
                    {
                        var _responseSeatmap = responseSeatmap.Content.ReadAsStringAsync().Result;
                        var JsonObjSeatmap = JsonConvert.DeserializeObject<dynamic>(_responseSeatmap);
                       // var decks1 = "1";
                      //  x.data[0].seatMap.decks.1.compartments.Y.units[0].unitKey
                        var uniquekey1= JsonObjSeatmap.data[0].seatMap.decks["1"].compartments.Y.units[0].unitKey;
                        uniquekey = ((Newtonsoft.Json.Linq.JValue)uniquekey1).Value.ToString();

                    }

                    #endregion
                    
                    #region SeatAssignment

                    SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                    _SeatAssignmentModel.journeyKey= journeyKey1;
                    _SeatAssignmentModel.collectedCurrencyCode = "INR";
                    // var unitkey = journeyKey1;
                    var jsonSeatAssignmentModel = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/"+passengerkey+"/seats/"+ uniquekey, _SeatAssignmentModel);


                    if (responceSeatAssignment.IsSuccessStatusCode)
                    {
                        var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                        var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                    }


                    #endregion


                    #region Commit Booking
                    string[]NotifyContacts=new string[1];
                    NotifyContacts[0] = "P";
                    Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();

                    _Commit_BookingModel.notifyContacts = true;
                    _Commit_BookingModel.contactTypesToNotify = NotifyContacts;
                    var jsonCommit_Booking = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v3/booking", _Commit_BookingModel);


                    if (responceCommit_Booking.IsSuccessStatusCode)
                    {
                        var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
                        var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
                    }
                    #endregion
                    #region Booking GET
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceGetBooking = await client.GetAsync(BaseURL + "/api/nsk/v1/booking");
                    
                    if (responceGetBooking.IsSuccessStatusCode)
                    {
                        var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                        var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                        var PnrStaus1 = JsonObjGetBooking.data.recordLocator;
                        PnrStaus = ((Newtonsoft.Json.Linq.JValue)PnrStaus1).Value.ToString();
                    

                       
                    }

                    #endregion
                    var arrivalDate = DateTime.Parse(arrival1);
                    var departureDate = DateTime.Parse(departure1);
                    var diff = (arrivalDate- departureDate);
                    ViewBag.destination = destination1;
                    ViewBag.origin = origin1;
                    ViewBag.arrival = arrival1;
                    ViewBag.departure = departure1;
                    ViewBag.identifier = identifier1;
                    ViewBag.carrierCode = carrierCode1;
                    ViewBag.totalfare = totalfare1;
                    ViewBag.Duration = diff;
                    ViewBag.pnr = PnrStaus;

                }
            }

            return View();

            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> cancelBooking( _credentials credentials)
        {
            credentials.username = "otaapi";
            credentials.alternateIdentifier = "";
            credentials.password = "AirAsia@123";
            credentials.domain = "EXT";
            credentials.location = "";
            credentials.channelType = "";
            credentials.loginRole = "";
            airlineLogin login  = new airlineLogin();
          //  login.credentials=credentials;
            login.applicationName = "";
            string cancletokan = string.Empty;
            string pnrhdn= Request.Form["pnrhdn"].ToString();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responce = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/token", login);
                if (responce.IsSuccessStatusCode)
                {
                    var results = responce.Content.ReadAsStringAsync().Result;
                    var JsonObj = JsonConvert.DeserializeObject<dynamic>(results);
                    var value = JsonObj.data.token;
                    cancletokan = ((Newtonsoft.Json.Linq.JValue)value).Value.ToString();
                }

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cancletokan);
                HttpResponseMessage responceGetbooking = await client.GetAsync(BaseURL + "/api/nsk/v1/booking/retrieve/byRecordLocator/"+pnrhdn);
                if (responceGetbooking.IsSuccessStatusCode)
                {
                    var _resultsGetbooking = responceGetbooking.Content.ReadAsStringAsync().Result;
                    var JsonObjGetbooking = JsonConvert.DeserializeObject<dynamic>(_resultsGetbooking);
                    
                }
                #region Cancel all journeys
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
               // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cancletokan);
                HttpResponseMessage responceCanceljourneys = await client.DeleteAsync(BaseURL + "/api/nsk/v1/booking/journeys");
                if (responceCanceljourneys.IsSuccessStatusCode)
                {
                    var _resultsCanceljourneys = responceCanceljourneys.Content.ReadAsStringAsync().Result;
                    var JsonObjCanceljourneys = JsonConvert.DeserializeObject<dynamic>(_resultsCanceljourneys);

                }
                #endregion

                #region Cancle Commit Booking

                string[] NotifyContacts = new string[1];
                NotifyContacts[0] = "P";
                Commit_BookingModel _Commit_Booking_Cancel = new Commit_BookingModel();

                _Commit_Booking_Cancel.notifyContacts = true;
                _Commit_Booking_Cancel.contactTypesToNotify = NotifyContacts;
                var jsonCommit_Booking = JsonConvert.SerializeObject(_Commit_Booking_Cancel, Formatting.Indented);

                HttpResponseMessage responceCancelCommit = await client.PutAsJsonAsync(BaseURL + "/api/nsk/v3/booking", _Commit_Booking_Cancel);
                if (responceCancelCommit.IsSuccessStatusCode)
                {
                    var _resultsCancelCommit = responceCancelCommit.Content.ReadAsStringAsync().Result;
                    var JsonObjCancelCommit = JsonConvert.DeserializeObject<dynamic>(_resultsCancelCommit);

                }
                #endregion

                #region Get Booking Cancel

                HttpResponseMessage responceBookingCancel = await client.GetAsync(BaseURL + "/api/nsk/v1/booking");
                if (responceBookingCancel.IsSuccessStatusCode)
                {
                    var _responceBookingCancel = responceBookingCancel.Content.ReadAsStringAsync().Result;
                    var JsonObj_responceBookingCancel = JsonConvert.DeserializeObject<dynamic>(_responceBookingCancel);

                }

                #endregion


            }

            ViewBag.canclepnr = pnrhdn;


                return View();
        }

       


    }
}
