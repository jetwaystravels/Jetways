using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.Models;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.SeatMapResponceModel;
using static DomainLayer.Model.SSRAvailabiltyResponceModel;
using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers
{
    public class AATripsellController : Controller
    {
       // string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;

        public IActionResult Tripsell()
        {

            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };

            ViewBag.Title = Title;

            string passenger = HttpContext.Session.GetString("keypassenger");

            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            ViewModel vm = new ViewModel();

            AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
            SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));

            vm.passeengerlist = passeengerlist;
            vm.Seatmaplist = Seatmaplist;
            vm.Meals = Mealslist;
            return View(vm);
           
        }
        
        public async Task<IActionResult> ContectDetails(ContactModel obj)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                ContactModel _ContactModel = new ContactModel();
                //  _ContactModel.emailAddress = passengerdetails.Email;
                _ContactModel.emailAddress = obj.emailAddress;
                _Phonenumber Phonenumber = new _Phonenumber();
                List<_Phonenumber> Phonenumberlist = new List<_Phonenumber>();
                Phonenumber.type = "Home";
                Phonenumber.number = obj.number;
                //Phonenumber.number = passengerdetails.mobile;
                Phonenumberlist.Add(Phonenumber);
                _Phonenumber Phonenumber1 = new _Phonenumber();
                Phonenumber1.type = "Other";
                Phonenumber1.number = obj.number;
                Phonenumberlist.Add(Phonenumber1);
                foreach (var item in Phonenumberlist)
                {
                    _ContactModel.phoneNumbers = Phonenumberlist;
                }
                _ContactModel.contactTypeCode = "P";

                _Address Address = new _Address();
                Address.lineOne = "123 CP";
                Address.countryCode = "IN";
                Address.provinceState = "TN";
                Address.city = "Delhi";
                Address.postalCode = "110011";
                _ContactModel.address = Address;

                _Name Name = new _Name();
                Name.first = "Raj";
                Name.middle = "kumar";
                Name.last = "sing";
                Name.title = "MR";
                _ContactModel.name = Name;

                var jsonContactRequest = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking/contacts", _ContactModel);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }

            }
            return RedirectToAction("Tripsell", "AATripsell");
        }
        public async Task<IActionResult> TravllerDetails(passkeytype passengerdetails)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                PassengersModel _PassengersModel = new PassengersModel();
                Name name = new Name();
                _Info Info = new _Info();
                if (passengerdetails.title == "Mr")
                {
                    Info.gender = "Male";
                }
                else
                {
                    Info.gender = "Female";
                }

                name.title = passengerdetails.title;
                name.first = passengerdetails.first;
                name.last = passengerdetails.last;
                name.middle = "";
                Info.dateOfBirth = "";
                Info.nationality = "IN";
                Info.residentCountry = "IN";


                _PassengersModel.name = name;
                _PassengersModel.info = Info;
                var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(AppUrlConstant.URLAirasia+ "/api/nsk/v3/booking/passengers/" + passengerdetails.passengerkey, _PassengersModel);
                if (responsePassengers.IsSuccessStatusCode)
                {
                    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                    var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                }
            }

            return RedirectToAction("GetSSRAvailabilty", "AATripsell", passengerdetails);
        }
        public async Task<IActionResult> GetSSRAvailabilty(passkeytype passengerdetails, DateTime departure)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            using (HttpClient client = new HttpClient())
            {
                string departuredate = string.Empty;
                List<passkeytype> passkeytypeslist = new List<passkeytype>();
                passkeytypeslist.Add(passengerdetails);
                SSRAvailabiltyModel _SSRAvailabilty = new SSRAvailabiltyModel();
                _SSRAvailabilty.passengerKeys = new string[passkeytypeslist.Count];
                for (int i = 0; i < passkeytypeslist.Count; i++)
                {
                    _SSRAvailabilty.passengerKeys[i] = passkeytypeslist[i].passengerkey;
                }
                _SSRAvailabilty.currencyCode = _SSRAvailabilty.currencyCode;

                List<Trip> Tripslist = new List<Trip>();
                Trip Tripobj = new Trip();
                Tripobj.origin = passengerdetails.origin;



                DateTime dateOnly = departure.Date;

                string departuredaterequest = (dateOnly.ToString("yyyy-MM-dd"));
                Tripobj.departureDate = departuredaterequest;
                //  _Trip.departureDate = "2023-07-02";
                List<TripIdentifier> TripIdentifierlist = new List<TripIdentifier>();
                TripIdentifier TripIdentifierobj = new TripIdentifier();
                TripIdentifierobj.carrierCode = passengerdetails.carrierCode;
                //_Identifier.carrierCode = "I5";
                TripIdentifierobj.identifier = passengerdetails.identifier;
                TripIdentifierlist.Add(TripIdentifierobj);
                Tripobj.identifier = TripIdentifierlist;
                Tripobj.destination = passengerdetails.destination;
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


                    var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                    ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
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
                            legssrslist.Add(legssrs);


                        }



                        SSRAvailabiltyLegssrobj.legDetails = legDetailsobj;
                        SSRAvailabiltyLegssrobj.legssrs = legssrslist;
                        SSRAvailabiltyLegssrlist.Add(SSRAvailabiltyLegssrobj);

                    }
                    SSRAvailabiltyResponceobj.legSsrs = SSRAvailabiltyLegssrlist;

                }

            }

            return RedirectToAction("Tripsell", "AATripsell");
        }


        public async Task<IActionResult> GetGstDetails(AddGSTInformation addGSTInformation)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                AddGSTInformation addinformation = new AddGSTInformation();

                
                addinformation.contactTypeCode = "G";

                GSTPhonenumber Phonenumber = new GSTPhonenumber();
                List<GSTPhonenumber> Phonenumberlist = new List<GSTPhonenumber>();
                Phonenumber.type = "Other";
                Phonenumber.number ="9898989898";               
                Phonenumberlist.Add(Phonenumber);               

                foreach (var item in Phonenumberlist)
                {
                    addinformation.phoneNumbers = Phonenumberlist;
                }
                addinformation.cultureCode = "";
                GSTAddress Address = new GSTAddress();
                Address.lineOne = "123 Main Street";
                Address.lineTwo = "Noida";
                Address.lineThree = "";
                Address.countryCode = "IN";
                Address.provinceState = "TN";
                Address.city = "Chennai";
                Address.postalCode = "600028";
                addinformation.Address = Address;

                addinformation.emailAddress = "vadivel@test.com";
                addinformation.customerNumber = "18AABCT3518Q1ZV";
                addinformation.sourceOrganization = "";
                addinformation.distributionOption = "None";
                addinformation.notificationPreference = "None";
                addinformation.companyName = "test company";

                GSTName Name = new GSTName();
                Name.first = "Vadivel";
                Name.middle = "raja";
                Name.last = "VR";
                Name.title = "MR";
                Name.suffix = "";
                addinformation.Name = Name;

                var jsonContactRequest = JsonConvert.SerializeObject(addinformation, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking/contacts", addinformation);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }

            }

            return RedirectToAction("Tripsell", "AATripsell");
        }
        public async Task<IActionResult> PostUnitkey(string unitKey, string passengerkey)
        {

            
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }

            string journeyKey = HttpContext.Session.GetString("journeyKey");
            string passenger = HttpContext.Session.GetString("keypassenger");
            AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            int passengerscount = passeengerKeyList.passengerscount;          

                using (HttpClient client = new HttpClient())
                {
                    var unitKey_1 = unitKey;

                    #region SeatAssignment
                    string[] unitKey2 = unitKey_1.Split('_');
                    string pas_unitKey = unitKey2[1];
                    SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                    _SeatAssignmentModel.journeyKey = journeyKey;
                    var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                    if (responceSeatAssignment.IsSuccessStatusCode)
                    {
                        var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                        var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                    }
                    #endregion

                }          
           
               

            
            return RedirectToAction("Tripsell", "AATripsell");
        }
       
        public async Task<IActionResult> PostMeal(legpassengers legpassengers)
        {


            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }


            using (HttpClient client = new HttpClient())
            {
                //legpassengers obj = new legpassengers();
                //obj.passengerKey = legpassengers.passengerKey;
                //obj.ssrKey = legpassengers.ssrKey;

                #region SellSSR
                SellSSRModel _sellSSRModel = new SellSSRModel();
                _sellSSRModel.count = 1;
                _sellSSRModel.note = "DevTest";
                _sellSSRModel.forceWaveOnSell = false;
                _sellSSRModel.currencyCode = "INR";


                var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + legpassengers.ssrKey, _sellSSRModel);
                if (responseSellSSR.IsSuccessStatusCode)
                {
                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                }
            }

            #endregion
            return View();
        }
        
    }
}

    


