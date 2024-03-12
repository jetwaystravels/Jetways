using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Bookingmanager_;
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
using ServiceLayer.Service.Interface;
using Utility;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.SeatMapResponceModel;
using static DomainLayer.Model.SSRAvailabiltyResponceModel;
//using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers
{
    public class AATripsellController : Controller
    {
        // string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        string MealsData = string.Empty;
        string Baggagesdata = string.Empty;

        public IActionResult Tripsell()
        {

            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };

            ViewBag.Title = Title;
            var AirlineName = TempData["AirLineName"];
            ViewData["name"] = AirlineName;

            string passenger = HttpContext.Session.GetString("keypassenger");
            string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            string BaggageData = HttpContext.Session.GetString("BaggageDetails");
            ViewModel vm = new ViewModel();
            if (passengerInfant != null)
            {
                AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                AirAsiaTripResponceModel passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                SSRAvailabiltyResponceModel BaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(BaggageData, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.passeengerlistItanary = passeengerlistItanary;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
                vm.Baggage = BaggageDetails;
            }
            else
            {
                AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                SSRAvailabiltyResponceModel BaggageDataDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(BaggageData, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;                
                vm.Baggage = BaggageDataDetails;
            }
            return View(vm);
        }     
    
        public async Task<IActionResult> ContectDetails(ContactModel contactobject)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            using (HttpClient client = new HttpClient())
            {
                ContactModel _ContactModel = new ContactModel();
                _ContactModel.emailAddress = contactobject.emailAddress;
                _Phonenumber Phonenumber = new _Phonenumber();
                List<_Phonenumber> Phonenumberlist = new List<_Phonenumber>();
                Phonenumber.type = "Home";
                Phonenumber.number = contactobject.number;
                Phonenumberlist.Add(Phonenumber);
                _Phonenumber Phonenumber1 = new _Phonenumber();
                Phonenumber1.type = "Other";
                Phonenumber1.number = contactobject.number;
                Phonenumberlist.Add(Phonenumber1);
                foreach (var item in Phonenumberlist)
                {
                    _ContactModel.phoneNumbers = Phonenumberlist;
                }
                _ContactModel.contactTypeCode = "P";
                _Address Address = new _Address();
                _ContactModel.address = Address;
                _Name Name = new _Name();
                Name.first = contactobject.first;
                Name.last = contactobject.last;
                Name.title = "MR";
                _ContactModel.name = Name;
                var jsonContactRequest = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.AirasiaContactDetail, _ContactModel);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }

            }
            return RedirectToAction("GetGstDetails", "AATripsell", contactobject);
        }
        public async Task<IActionResult> GetGstDetails(ContactModel contactobject, AddGSTInformation addGSTInformation)
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
                Phonenumber.number = contactobject.number; ;
                Phonenumberlist.Add(Phonenumber);

                foreach (var item in Phonenumberlist)
                {
                    addinformation.phoneNumbers = Phonenumberlist;
                }
                addinformation.cultureCode = "";
                GSTAddress Address = new GSTAddress();
                addinformation.Address = Address;
                addinformation.emailAddress = contactobject.emailAddress;
                addinformation.customerNumber = contactobject.customerNumber;
                addinformation.sourceOrganization = "";
                addinformation.distributionOption = "None";
                addinformation.notificationPreference = "None";
                addinformation.companyName = contactobject.companyName;
                GSTName Name = new GSTName();
                Name.first = contactobject.first;
                Name.last = contactobject.last;
                Name.title = "MR";
                Name.suffix = "";
                addinformation.Name = Name;
                if (contactobject.companyName != null)
                {
                    var jsonContactRequest = JsonConvert.SerializeObject(addinformation, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.AirasiaGstDetail, addinformation);
                    if (responseAddContact.IsSuccessStatusCode)
                    {
                        var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                        var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                    }
                }

            }

            return RedirectToAction("Tripsell", "AATripsell");
        }



        public async Task<IActionResult> TravllerDetails(List<passkeytype> passengerdetails, List<Infanttype> infanttype)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");


            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(tokenview))
                {
                    token = tokenview.Replace(@"""", string.Empty);
                    PassengersModel _PassengersModel = new PassengersModel();
                    for (int i = 0; i < passengerdetails.Count; i++)
                    {
                        if (passengerdetails[i].passengertypecode == "INFT")
                            continue;
                        if (passengerdetails[i].passengertypecode != null)
                        {

                            Name name = new Name();
                            _Info Info = new _Info();
                            if (passengerdetails[i].title == "Mr")
                            {
                                Info.gender = "Male";
                            }
                            else
                            {
                                Info.gender = "Female";
                            }

                            name.title = passengerdetails[i].title;
                            name.first = passengerdetails[i].first;
                            name.last = passengerdetails[i].last;
                            name.middle = "";
                            Info.dateOfBirth = "";
                            Info.nationality = "IN";
                            Info.residentCountry = "IN";
                            _PassengersModel.name = name;
                            _PassengersModel.info = Info;
                            var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey, _PassengersModel);
                            if (responsePassengers.IsSuccessStatusCode)
                            {
                                var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                                Logs logs = new Logs();
                                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_PassengersModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger", "AirAsiaRT");

                                var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                            }
                        }
                    }

                    int infantcount = 0;
                    for (int k = 0; k < passengerdetails.Count; k++)
                    {
                        if (passengerdetails[k].passengertypecode == "INFT")
                            infantcount++;

                    }
                    AddInFantModel _PassengersModel1 = new AddInFantModel();
                    for (int i = 0; i < passengerdetails.Count; i++)
                    {
                        if (passengerdetails[i].passengertypecode == "ADT" || passengerdetails[i].passengertypecode == "CHD")
                            continue;
                        if (passengerdetails[i].passengertypecode == "INFT")
                        {
                            for (int k = 0; k < infantcount; k++)
                            {
                                _PassengersModel1.nationality = "IN";
                                _PassengersModel1.dateOfBirth = "2023-10-01";
                                _PassengersModel1.residentCountry = "IN";
                                _PassengersModel1.gender = "Male";

                                InfantName nameINF = new InfantName();
                                nameINF.first = passengerdetails[i].first;
                                nameINF.middle = "";
                                nameINF.last = passengerdetails[i].last;
                                nameINF.title = "Mr";
                                nameINF.suffix = "";
                                _PassengersModel1.name = nameINF;


                                var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[k].passengerkey + "/infant", _PassengersModel1);
                                if (responsePassengers.IsSuccessStatusCode)
                                {
                                    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                                    Logs logs = new Logs();
                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_PassengersModel1) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[k].passengerkey + "/infant" + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger_Infant", "AirAsiaRT");

                                    var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                                }
                                i++;
                            }
                            // STRAT Get INFO
                            // var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking");
                            if (responceGetBooking.IsSuccessStatusCode)
                            {
                                var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                                Logs logs = new Logs();
                                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetBookingRequest") + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v1/booking" + "\n Response: " + JsonConvert.SerializeObject(_responceGetBooking), "GetBooking", "AirAsiaRT");

                                var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                            }
                        }
                    }
                    HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                }

                return RedirectToAction("Tripsell", "AATripsell");
            }
        }
       
        public async Task<IActionResult> GetGstDetails01(AddGSTInformation addGSTInformation, string lineOne, string lineTwo, string city, string number, string postalCode)
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
                Phonenumber.number = number;
                Phonenumberlist.Add(Phonenumber);

                foreach (var item in Phonenumberlist)
                {
                    addinformation.phoneNumbers = Phonenumberlist;
                }
                addinformation.cultureCode = "";
                GSTAddress Address = new GSTAddress();
                Address.lineOne = lineOne;
                Address.lineTwo = lineTwo;
                Address.lineThree = "";
                Address.countryCode = "IN";
                Address.provinceState = "TN";
                Address.city = city;
                Address.postalCode = postalCode;
                addinformation.Address = Address;

                addinformation.emailAddress = addGSTInformation.emailAddress;
                addinformation.customerNumber = addGSTInformation.customerNumber;
                addinformation.sourceOrganization = "";
                addinformation.distributionOption = "None";
                addinformation.notificationPreference = "None";
                addinformation.companyName = addGSTInformation.companyName;

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
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.AirasiaGstDetail, addinformation);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }
            }
            return RedirectToAction("Tripsell", "AATripsell");
        }
        public async Task<IActionResult> PostUnitkey(List<string> unitKey, List<string> ssrKey,List<string> BaggageSSrkey)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }
            using (HttpClient client = new HttpClient())
            {
                string passenger = HttpContext.Session.GetString("keypassenger");
                string Seatmap = HttpContext.Session.GetString("Seatmap");
                string Meals = HttpContext.Session.GetString("Meals");
                string BaggageData = HttpContext.Session.GetString("BaggageDetails");
                AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                SSRAvailabiltyResponceModel BaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(BaggageData, typeof(SSRAvailabiltyResponceModel));
                int passengerscount = passeengerKeyList.passengerscount;
                var data = Seatmaplist.datalist.Count;
                string legkey = passeengerKeyList.journeys[0].segments[0].legs[0].legKey;
                int Seatcount = unitKey.Count;
                if (Seatcount <= 0)
                {
                    for (int i = 0; i < data; i++)
                    {
                        for (int j = 0; j < passengerscount; j++)
                        {
                            string unitKey1 = string.Empty;                            
                            string passengerkey = passeengerKeyList.passengers[j].passengerKey;                           
                            string journeyKey = passeengerKeyList.journeys[0].journeyKey;
                            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                            _SeatAssignmentModel.journeyKey = journeyKey;
                            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaAutoSeat + passengerkey, _SeatAssignmentModel);

                            if (responceSeatAssignment.IsSuccessStatusCode)
                            {
                                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                            }
                        }
                    }
                }
                else
                {
                    int seatid = 0;
                    for (int i = 0; i < data; i++)
                    {
                        for (int j = 0; j < passengerscount; j++)
                        {
                            string unitKey1 = string.Empty;                            
                            string passengerkey = passeengerKeyList.passengers[j].passengerKey;
                            string journeyKey = passeengerKeyList.journeys[0].journeyKey;
                            unitKey1 = unitKey[seatid];
                            string[] unitKey2 = unitKey1.Split('_');
                            string pas_unitKey = unitKey2[1];
                            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                            _SeatAssignmentModel.journeyKey = journeyKey;
                            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaSeatSelect + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                            //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/seats/auto/" + passengerkey, _SeatAssignmentModel);
                            if (responceSeatAssignment.IsSuccessStatusCode)
                            {
                                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                            }
                            seatid++;
                        }
                    }
                    var mealcount = ssrKey.Count;                   
                    if (mealcount > 0)
                    {
                        int mealid = 0;
                        int mealssr = Mealslist.legSsrs.Count;                   
                                               
                        for (int k = 0; k < mealssr; k++)
                        {
                            for (int l = 0; l< passengerscount; l++)
                            {
                                string mealskey = string.Empty;
                                mealskey = ssrKey[mealid];
                                string[] MealSSrKeyData = mealskey.Split('_');
                                string pas_SsrKey = MealSSrKeyData[0];                               
                                SellSSRModel _sellSSRModel = new SellSSRModel();
                                _sellSSRModel.count = 1;
                                _sellSSRModel.note = "DevTest";
                                _sellSSRModel.forceWaveOnSell = false;
                                _sellSSRModel.currencyCode = "INR";

                                var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + pas_SsrKey, _sellSSRModel);
                                if (responseSellSSR.IsSuccessStatusCode)
                                {
                                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                }
                                mealid++;
                            }
                        }                      
                        
                    }
                    #region Baggage
                    var baggagecount = BaggageSSrkey.Count;
                    int baggageSsr = BaggageDetails.journeySsrsBaggage.Count;
                    if (baggagecount > 0)
                    {
                        int baggageid = 0;
                        for (int k = 0; k < baggageSsr; k++)
                        {
                            for (int i = 0; i < passengerscount; i++)
                            {                          


                                string BaggageKey = string.Empty;
                                BaggageKey = BaggageSSrkey[baggageid];
                                string[] BaggageSSrKeyData = BaggageKey.Split('_');
                                string pas_BaggageSsrKey = BaggageSSrKeyData[0];

                                SellSSRModel _sellSSRModel = new SellSSRModel();
                                _sellSSRModel.count = 1;
                                _sellSSRModel.note = "DevTest";
                                _sellSSRModel.forceWaveOnSell = false;
                                _sellSSRModel.currencyCode = "INR";
                                var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + pas_BaggageSsrKey, _sellSSRModel);
                                if (responseSellSSR.IsSuccessStatusCode)
                                {
                                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                }
                                baggageid++;

                            }
                        }
                    }
                    #endregion


                }

                #region Booking GET
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responceGetBooking01 = await client.GetAsync(AppUrlConstant.AirasiaGetBoking);
                if (responceGetBooking01.IsSuccessStatusCode)
                {
                    // string BaseURL1 = "http://localhost:5225/";
                    var _responceGetBooking = responceGetBooking01.Content.ReadAsStringAsync().Result;

                    var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);

                }
                #endregion
            }
            //return RedirectToAction("booking", "CommitBooking");
            return RedirectToAction("AirAsiaOneWayPaymentView", "AirAsiaOneWayPayment");
        }

    }
}







