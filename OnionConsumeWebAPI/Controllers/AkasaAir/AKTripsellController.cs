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
using IndigoBookingManager_;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Nancy;
using Nancy.Json;
using Nancy.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.Models;
using ServiceLayer.Service.Interface;
using Utility;
using ZXing.Aztec.Internal;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.ReturnAirLineTicketBooking;
using static DomainLayer.Model.SeatMapResponceModel;
using static DomainLayer.Model.SSRAvailabiltyResponceModel;

namespace OnionConsumeWebAPI.Controllers.AkasaAir
{
    public class AKTripsellController : Controller
    {
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        public IActionResult AkTripsellView()
        {
            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };
            ViewBag.Title = Title;
            ViewModel vm = new ViewModel();
            var AKpassenger = HttpContext.Session.GetString("ResultFlightPassenger");
            var AkMeals = HttpContext.Session.GetString("AKMealsBaggage");
            var Akbaggage = HttpContext.Session.GetString("AKBaggageDetails");
            var AkSeatMap = HttpContext.Session.GetString("AKSeatmap");
            var AkItanary = HttpContext.Session.GetString("AkasaAirItanary");
            if (AkItanary != null)
            {

                AirAsiaTripResponceModel AkPassenger = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AKpassenger, typeof(AirAsiaTripResponceModel));
                SSRAvailabiltyResponceModel AkMealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(AkMeals, typeof(SSRAvailabiltyResponceModel));
                SSRAvailabiltyResponceModel AkBaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Akbaggage, typeof(SSRAvailabiltyResponceModel));
                SeatMapResponceModel AkSeatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(AkSeatMap, typeof(SeatMapResponceModel));
                AirAsiaTripResponceModel AkpasseengerItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AkItanary, typeof(AirAsiaTripResponceModel));
                vm.AkPassenger = AkPassenger;
                vm.AkMealslist = AkMealslist;
                vm.AkBaggageDetails = AkBaggageDetails;
                vm.AkSeatmaplist = AkSeatmaplist;
                vm.AkpasseengerItanary = AkpasseengerItanary;
                return View(vm);
            }
            else
            {
                if (!string.IsNullOrEmpty(AKpassenger))
                {
                    AirAsiaTripResponceModel AkPassenger = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AKpassenger, typeof(AirAsiaTripResponceModel));
                    vm.AkPassenger = AkPassenger;
                }
                if (!string.IsNullOrEmpty(AkMeals))
                {
                    SSRAvailabiltyResponceModel AkMealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(AkMeals, typeof(SSRAvailabiltyResponceModel));
                    vm.AkMealslist = AkMealslist;
                }

                if (!string.IsNullOrEmpty(Akbaggage))
                {
                    SSRAvailabiltyResponceModel AkBaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Akbaggage, typeof(SSRAvailabiltyResponceModel));
                    vm.AkBaggageDetails = AkBaggageDetails;

                }
                if (!string.IsNullOrEmpty(AkSeatMap))
                {
                    SeatMapResponceModel AkSeatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(AkSeatMap, typeof(SeatMapResponceModel));
                    vm.AkSeatmaplist = AkSeatmaplist;
                }
                return View(vm);

            }
        }
        public IActionResult AkPostSeatMapdataView()
        {
            ViewModel vm = new ViewModel();
            var AKpassenger = HttpContext.Session.GetString("ResultFlightPassenger");
            var AkSeatMap = HttpContext.Session.GetString("AKSeatmap");
            var AkpassengerDetails = HttpContext.Session.GetString("AKPassengerName");

            AirAsiaTripResponceModel AkPassenger = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AKpassenger, typeof(AirAsiaTripResponceModel));
            SeatMapResponceModel AkSeatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(AkSeatMap, typeof(SeatMapResponceModel));
            List<passkeytype> passkeytypesDetails = JsonConvert.DeserializeObject<List<passkeytype>>(AkpassengerDetails);
            vm.AkPassenger = AkPassenger;
            vm.AkSeatmaplist = AkSeatmaplist;
            vm.passkeytype = passkeytypesDetails;
            return View(vm);
        }
        public async Task<IActionResult> AKContactDetails(ContactModel contactobject)
        {
            string tokenview = HttpContext.Session.GetString("AkasaTokan");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);
            using (HttpClient client = new HttpClient())
            {
                ContactModel _AkContactModel = new ContactModel();
                _AkContactModel.emailAddress = contactobject.emailAddress;
                _Phonenumber AkPhonenumber = new _Phonenumber();
                List<_Phonenumber> AkPhonenumberlist = new List<_Phonenumber>();
                AkPhonenumber.type = "Home";
                AkPhonenumber.number = contactobject.number;
                AkPhonenumberlist.Add(AkPhonenumber);
                _Phonenumber AkPhonenumber1 = new _Phonenumber();
                AkPhonenumber1.type = "Other";
                AkPhonenumber1.number = contactobject.number;
                AkPhonenumberlist.Add(AkPhonenumber1);
                foreach (var item in AkPhonenumberlist)
                {
                    _AkContactModel.phoneNumbers = AkPhonenumberlist;
                }
                _AkContactModel.contactTypeCode = "P";
                _Address AkAddress = new _Address();
                _AkContactModel.address = AkAddress;
                _Name AkName = new _Name();
                AkName.first = contactobject.first;
                AkName.last = contactobject.last;
                AkName.title = "MR";
                _AkContactModel.name = AkName;
                var jsonAkContactRequest = JsonConvert.SerializeObject(_AkContactModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAkAddContact = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirContactDetails, _AkContactModel);
                if (responseAkAddContact.IsSuccessStatusCode)
                {
                    Logs logs = new Logs();
                    var _responseAkAddContact = responseAkAddContact.Content.ReadAsStringAsync().Result;
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_AkContactModel) + "Url: " + "\n\n Response: " + JsonConvert.SerializeObject(_responseAkAddContact), "Contact", "AkasaOneWay");
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAkAddContact);
                }

            }

            //return RedirectToAction("AkTripsellView", "AKTripsell");
            return RedirectToAction("_RGetGstDetails", "AKTripsell", contactobject);

        }

        // Code For GST 
        public async Task<IActionResult> _RGetGstDetails(ContactModel contactobject)
        {
            string tokenview = string.Empty;
            tokenview = HttpContext.Session.GetString("AkasaTokan");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);
            using (HttpClient client = new HttpClient())
            {
                AddGSTInformation addinformation = new AddGSTInformation();
                addinformation.contactTypeCode = "G";
                GSTPhonenumber Phonenumber = new GSTPhonenumber();
                List<GSTPhonenumber> Phonenumberlist = new List<GSTPhonenumber>();
                Phonenumber.type = "Other";
                Phonenumber.number = contactobject.number;
                Phonenumberlist.Add(Phonenumber);

                foreach (var item in Phonenumberlist)
                {
                    addinformation.phoneNumbers = Phonenumberlist;
                }
                addinformation.cultureCode = "";
                GSTAddress Address = new GSTAddress();
                Address.lineOne = "Ashokenagar,bharathi cross str";
                Address.countryCode = "IN";
                Address.provinceState = "TN";
                Address.city = "Ashokenagar";
                Address.postalCode = "400006";
                addinformation.Address = Address;

                addinformation.emailAddress = contactobject.emailAddressgst;
                addinformation.customerNumber = contactobject.customerNumber;
                //addinformation.sourceOrganization = "QPCCJ5003C";
                addinformation.distributionOption = null;
                addinformation.notificationPreference = null;
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
                    HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v1/booking/contacts", addinformation);
                    if (responseAddContact.IsSuccessStatusCode)
                    {
                        var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;

                        var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                    }
                }

            }

            return RedirectToAction("AkTripsellView", "AKTripsell");
        }

        public async Task<IActionResult> AKTravellerInfo(List<passkeytype> passengerdetails, string formattedDates)
        {
            string tokenview = HttpContext.Session.GetString("AkasaTokan");
            string[] dateStrings = JsonConvert.DeserializeObject<string[]>(formattedDates);
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(tokenview))
                {
                    if (tokenview == null) { tokenview = ""; }
                    token = tokenview.Replace(@"""", string.Empty);
                    PassengersModel _AkPassengersModel = new PassengersModel();
                    for (int i = 0; i < passengerdetails.Count; i++)
                    {
                        if (passengerdetails[i].passengertypecode == "INFT")
                            continue;
                        if (passengerdetails[i].passengertypecode != null)
                        {

                            Name Akname = new Name();
                            _Info AkInfo = new _Info();
                            if (passengerdetails[i].title == "Mr")
                            {
                                AkInfo.gender = "Male";
                            }
                            else
                            {
                                AkInfo.gender = "Female";
                            }
                            Akname.title = passengerdetails[i].title;
                            Akname.first = passengerdetails[i].first;
                            Akname.last = passengerdetails[i].last;
                            //Akname.mobile = passengerdetails[i].mobile;
                            Akname.middle = "";
                            AkInfo.dateOfBirth = "";
                            AkInfo.nationality = "IN";
                            AkInfo.residentCountry = "IN";
                            _AkPassengersModel.name = Akname;
                            _AkPassengersModel.info = AkInfo;
                            HttpContext.Session.SetString("AKPassengerName", JsonConvert.SerializeObject(passengerdetails));
                            var jsonPassengers = JsonConvert.SerializeObject(_AkPassengersModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage AkresponsePassengers = await client.PutAsJsonAsync(AppUrlConstant.AkasaAirPassengerDetails + passengerdetails[i].passengerkey, _AkPassengersModel);
                            if (AkresponsePassengers.IsSuccessStatusCode)
                            {
                                var _responsePassengers = AkresponsePassengers.Content.ReadAsStringAsync().Result;
                                Logs logs = new Logs();
                                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_AkPassengersModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger", "AkasaOneWay");

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
                    AddInFantModel _AkPassengersModel1 = new AddInFantModel();
                    for (int i = 0; i < passengerdetails.Count; i++)
                    {
                        if (passengerdetails[i].passengertypecode == "ADT" || passengerdetails[i].passengertypecode == "CHD")
                            continue;
                        if (passengerdetails[i].passengertypecode == "INFT")
                        {
                            for (int k = 0; k < infantcount; k++)
                            {
                                _AkPassengersModel1.nationality = "IN";
                                //_PassengersModel1.dateOfBirth = "2023-10-01";
                                _AkPassengersModel1.dateOfBirth = dateStrings[k];
                                _AkPassengersModel1.residentCountry = "IN";
                                _AkPassengersModel1.gender = "Male";

                                InfantName AknameINF = new InfantName();
                                AknameINF.first = passengerdetails[i].first;
                                AknameINF.middle = "";
                                AknameINF.last = passengerdetails[i].last;
                                AknameINF.title = "Mr";
                                AknameINF.suffix = "";
                                _AkPassengersModel1.name = AknameINF;


                                var jsonPassengers = JsonConvert.SerializeObject(_AkPassengersModel1, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirInfantDetails + passengerdetails[k].passengerkey + "/infant", _AkPassengersModel1);
                                //HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirInfantDetails , _AkPassengersModel1);
                                if (responsePassengers.IsSuccessStatusCode)
                                {
                                    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                                    Logs logs = new Logs();
                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_AkPassengersModel1) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[k].passengerkey + "/infant" + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger_Infant", "AkasaOneWay");

                                    var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                                }
                                i++;
                            }

                        }
                    }
                }

                #region post data 
                ViewModel vm = new ViewModel();
                var AKpassenger = HttpContext.Session.GetString("ResultFlightPassenger");
                var AkMeals = HttpContext.Session.GetString("AKMealsBaggage");
                var Akbaggage = HttpContext.Session.GetString("AKBaggageDetails");
                var AkSeatMap = HttpContext.Session.GetString("AKSeatmap");
                var AkpassengerDetails = HttpContext.Session.GetString("AKPassengerName");
                if (!string.IsNullOrEmpty(AKpassenger))
                {
                    AirAsiaTripResponceModel AkPassenger = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AKpassenger, typeof(AirAsiaTripResponceModel));
                    vm.AkPassenger = AkPassenger;
                }
                if (!string.IsNullOrEmpty(AkMeals))
                {
                    SSRAvailabiltyResponceModel AkMealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(AkMeals, typeof(SSRAvailabiltyResponceModel));
                    vm.AkMealslist = AkMealslist;
                }
                if (!string.IsNullOrEmpty(Akbaggage))
                {
                    SSRAvailabiltyResponceModel AkBaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Akbaggage, typeof(SSRAvailabiltyResponceModel));
                    vm.AkBaggageDetails = AkBaggageDetails;
                }
                if (!string.IsNullOrEmpty(AkSeatMap))
                {
                    SeatMapResponceModel AkSeatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(AkSeatMap, typeof(SeatMapResponceModel));
                    vm.AkSeatmaplist = AkSeatmaplist;
                }
                if (!string.IsNullOrEmpty(AkpassengerDetails))
                {
                    List<passkeytype> passkeytypesDetails = JsonConvert.DeserializeObject<List<passkeytype>>(AkpassengerDetails);
                    vm.passkeytype = passkeytypesDetails;
                }


                #endregion

                return PartialView("_AkServiceRequestsPartialView", vm);

            }

        }
        public async Task<IActionResult> PostSeatmapMealdata(List<string> unitKey, List<string> mealssrKey, List<string> BaggageSSrkey)
        {
            List<string> _unitkey = new List<string>();
            for (int i = 0; i < unitKey.Count; i++)
            {
                if (unitKey[i] == null)
                    continue;
                _unitkey.Add(unitKey[i].Trim());
            }
            unitKey = new List<string>();
            unitKey = _unitkey;

            //if (unitKey.Count > 0)
            //{
            //    if (unitKey[0] == null)
            //    {
            //        unitKey = new List<string>();
            //    }
            //}

            if (mealssrKey.Count > 0)
            {
                if (mealssrKey[0] == null)
                {
                    mealssrKey = new List<string>();
                }
            }
            if (BaggageSSrkey.Count > 0)
            {
                if (BaggageSSrkey[0] == null)
                {
                    BaggageSSrkey = new List<string>();
                }
            }


            string tokenview = HttpContext.Session.GetString("AkasaTokan");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }
            using (HttpClient client = new HttpClient())
            {
                var AKpassenger = HttpContext.Session.GetString("ResultFlightPassenger");
                var AkMeals = HttpContext.Session.GetString("AKMealsBaggage");
                var Akbaggage = HttpContext.Session.GetString("AKBaggageDetails");
                var AkSeatMap = HttpContext.Session.GetString("AKSeatmap");
                var AkpassengerDetails = HttpContext.Session.GetString("AKPassengerName");

                AirAsiaTripResponceModel AkPassenger = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(AKpassenger, typeof(AirAsiaTripResponceModel));

                SSRAvailabiltyResponceModel AkMealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(AkMeals, typeof(SSRAvailabiltyResponceModel));

                SSRAvailabiltyResponceModel AkBaggageDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Akbaggage, typeof(SSRAvailabiltyResponceModel));

                SeatMapResponceModel AkSeatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(AkSeatMap, typeof(SeatMapResponceModel));

                int passengerscount = AkPassenger.passengerscount;
                var data = AkSeatmaplist.datalist.Count;
                string legkey = AkPassenger.journeys[0].segments[0].legs[0].legKey;
                int Seatcount = unitKey.Count;
                if (Seatcount <= 0)
                {
                    for (int i = 0; i < data; i++)
                    {
                        for (int j = 0; j < passengerscount; j++)
                        {
                            string unitKey1 = string.Empty;
                            string passengerkey = AkPassenger.passengers[j].passengerKey;
                            string journeyKey = AkPassenger.journeys[0].journeyKey;
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
                    var mealcount = mealssrKey.Count;
                    if (mealcount > 0)
                    {
                        int mealid = 0;
                        int mealssr = AkMealslist.legSsrs.Count;

                        for (int k = 0; k < mealssr; k++)
                        {
                            for (int l = 0; l < passengerscount; l++)
                            {
                                if (mealid < mealssrKey.Count) // Check if mealid is within bounds
                                {
                                    string mealskey = string.Empty;
                                    mealskey = mealssrKey[mealid];
                                    string[] MealSSrKeyData = mealskey.Split('_');
                                    string pas_SsrKey = MealSSrKeyData[0];
                                    SellSSRModel _sellSSRModel = new SellSSRModel();
                                    _sellSSRModel.count = 1;
                                    _sellSSRModel.note = "PYOG";
                                    _sellSSRModel.forceWaveOnSell = false;
                                    _sellSSRModel.currencyCode = "INR";
                                    _sellSSRModel.ssrSellMode = 2;

                                    var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirMealBaggagePost + pas_SsrKey, _sellSSRModel);
                                    if (responseSellSSR.IsSuccessStatusCode)
                                    {
                                        Logs logs = new Logs();
                                        var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.AkasaAirMealBaggagePost + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AkasaOneWay");

                                        var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                    }
                                    mealid++;
                                }
                                else
                                {

                                    break;
                                }
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
                            if (seatid < unitKey.Count) // Check if mealid is within bounds
                            {
                                string unitKey1 = string.Empty;
                                string passengerkey = AkPassenger.passengers[j].passengerKey;
                                string journeyKey = AkPassenger.journeys[0].journeyKey;
                                string pas_unitKeyS = unitKey[seatid];
                                string[] partsPas_unitKey = pas_unitKeyS.Split('_');
                                string pas_unitKey = partsPas_unitKey[1];
                                Logs logs = new Logs();
                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                _SeatAssignmentModel.journeyKey = journeyKey;
                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirMealSeatAssign + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                if (responceSeatAssignment.IsSuccessStatusCode)
                                {
                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.AkasaAirMealSeatAssign + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "SeatAssign", "AkasaOneWay");
                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                }
                                seatid++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    var mealcount = mealssrKey.Count;
                    if (mealcount > 0)
                    {
                        int mealid = 0;
                        int mealssr = AkMealslist.legSsrs.Count;
                        Logs logs = new Logs();
                        for (int k = 0; k < mealssr; k++)
                        {
                            for (int l = 0; l < passengerscount; l++)
                            {
                                if (mealid < mealssrKey.Count) // Check if mealid is within bounds
                                {
                                    string mealskey = string.Empty;
                                    mealskey = mealssrKey[mealid];
                                    string[] MealSSrKeyData = mealskey.Split('_');
                                    string pas_SsrKey = MealSSrKeyData[0];
                                    SellSSRModel _sellSSRModel = new SellSSRModel();
                                    _sellSSRModel.count = 1;
                                    _sellSSRModel.note = "PYOG";
                                    _sellSSRModel.forceWaveOnSell = false;
                                    _sellSSRModel.currencyCode = "INR";
                                    _sellSSRModel.ssrSellMode = 2;

                                    var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.AkasaAirMealBaggagePost + pas_SsrKey, _sellSSRModel);
                                    if (responseSellSSR.IsSuccessStatusCode)
                                    {
                                        var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.AkasaAirMealBaggagePost + pas_SsrKey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSr", "AkasaOneWay");
                                        var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                    }
                                    mealid++;
                                }
                                else
                                {

                                    break;
                                }
                            }
                        }

                    }



                }
            }
            return RedirectToAction("AkasaAirPaymentView", "AkasaAirPayment");

        }
    }

}
