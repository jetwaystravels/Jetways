using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Bookingmanager_;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Nancy.Session;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using Utility;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.SeatMapResponceModel;
using static OnionConsumeWebAPI.Controllers.SGTripsellController;

namespace OnionConsumeWebAPI.Controllers
{
    public class RoundAATripsellController : Controller
    {
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        public IActionResult RoundAATripsellView()
        {

            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };
            ViewBag.Title = Title;

            string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
            //
            ViewModel vm = new ViewModel();

            //AirAsia Passeneger
            string test = string.Empty;
            AirAsiaTripResponceModel passeengerlist = null;
            string Meals = string.Empty;// HttpContext.Session.GetString("Meals");
            SSRAvailabiltyResponceModel Mealslist = null;
            SeatMapResponceModel Seatmaplist = null;
            string Seatmap = string.Empty;
            string passenger = HttpContext.Session.GetString("keypassenger");
            vm.SeatmaplistRT = new List<SeatMapResponceModel>();
            vm.passeengerlistRT = new List<AirAsiaTripResponceModel>();
            vm.MealslistRT = new List<SSRAvailabiltyResponceModel>();


            string Passenegrtext = HttpContext.Session.GetString("Mainpassengervm");
            string Seattext = HttpContext.Session.GetString("Mainseatmapvm");
            string Mealtext = HttpContext.Session.GetString("Mainmealvm");


            #region 2


            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainpassengervm")))
            {
                test = HttpContext.Session.GetString("Mainpassengervm");

                foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
                {
                    passenger = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"").Replace("\\\\", "");
                    if (passenger != null)
                    {
                        passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                        vm.passeengerlistRT.Add(passeengerlist);
                    }
                }
                //HttpContext.Session.Remove("_keypassengerdata");
            }


            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainseatmapvm")))
            {
                //vm.SeatmaplistRT = new List<SeatMapResponceModel>();
                test = HttpContext.Session.GetString("Mainseatmapvm");
                Seatmap = string.Empty;
                foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
                {
                    Seatmap = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"").Replace("\\\\", "");
                    if (Seatmap != null)
                    {
                        Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                        vm.SeatmaplistRT.Add(Seatmaplist);
                    }
                }
                //HttpContext.Session.Remove("_SeatmapData");
            }

            Meals = string.Empty;// HttpContext.Session.GetString("Meals");
            Mealslist = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainmealvm")))
            {
                test = HttpContext.Session.GetString("Mainmealvm");
                foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
                {
                    Meals = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"").Replace("\\\\", "");
                    if (Meals != null)
                    {
                        Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                        vm.MealslistRT.Add(Mealslist);
                    }
                }
                // HttpContext.Session.Remove("_MealsData");
            }


            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_keypassengerdata")))
            //{
            //    test = HttpContext.Session.GetString("_keypassengerdata");

            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        passenger = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (passenger != null)
            //        {
            //            passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            //            vm.passeengerlistRT.Add(passeengerlist);
            //        }
            //    }
            //    //HttpContext.Session.Remove("_keypassengerdata");
            //}

            //AirAsia Seatmap
            //Seatmaplist = null;
            //string Seatmap = HttpContext.Session.GetString("Seatmap");

            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_SeatmapData")))
            //{
            //    //vm.SeatmaplistRT = new List<SeatMapResponceModel>();
            //    test = HttpContext.Session.GetString("_SeatmapData");
            //    Seatmap = string.Empty;
            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        Seatmap = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (Seatmap != null)
            //        {
            //            Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
            //            vm.SeatmaplistRT.Add(Seatmaplist);
            //        }
            //    }
            //    //HttpContext.Session.Remove("_SeatmapData");
            //}


            //AirAsia SSR MealsData
            //vm.MealslistRT = new List<SSRAvailabiltyResponceModel>();
            //Meals = string.Empty;// HttpContext.Session.GetString("Meals");
            //Mealslist = null;
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_MealsData")))
            //{
            //    test = HttpContext.Session.GetString("_MealsData");
            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        Meals = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (Meals != null)
            //        {
            //            Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
            //            vm.MealslistRT.Add(Mealslist);
            //        }
            //    }
            //    // HttpContext.Session.Remove("_MealsData");
            //}
            #endregion
            #region 1
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("keypassengerdata")))
            //{
            //    test = HttpContext.Session.GetString("keypassengerdata");

            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        passenger = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (passenger != null)
            //        {
            //            passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            //            vm.passeengerlistRT.Add(passeengerlist);
            //        }
            //    }
            //    //HttpContext.Session.Remove("keypassengerdata");
            //}

            //AirAsia Seatmap
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SeatmapData")))
            //{
            //    //string Seatmap = HttpContext.Session.GetString("Seatmap");
            //    test = HttpContext.Session.GetString("SeatmapData");

            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        Seatmap = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (Seatmap != null)
            //        {
            //            Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
            //            vm.SeatmaplistRT.Add(Seatmaplist);
            //        }
            //    }
            //    //HttpContext.Session.Remove("SeatmapData");
            //}


            //AirAsia SSR MealsData
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MealsData")))
            //{
            //    test = HttpContext.Session.GetString("MealsData");
            //    foreach (Match item in Regex.Matches(test, @"<Start>(?<test>[\s\S]*?)<End>"))
            //    {
            //        Meals = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"");
            //        if (Meals != null)
            //        {
            //            Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
            //            vm.MealslistRT.Add(Mealslist);
            //        }
            //    }
            //    //HttpContext.Session.Remove("MealsData");
            //}
            #endregion


            return View(vm);


        }

        public async Task<IActionResult> PostReturnContactData(ContactModel contactobject)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");

            if (!string.IsNullOrEmpty(tokenview))
            {
                token = tokenview.Replace(@"""", string.Empty);
                using (HttpClient client = new HttpClient())
                {
                    ContactModel _ContactModel = new ContactModel();
                    //  _ContactModel.emailAddress = passengerdetails.Email;
                    _ContactModel.emailAddress = contactobject.emailAddress;
                    _Phonenumber Phonenumber = new _Phonenumber();
                    List<_Phonenumber> Phonenumberlist = new List<_Phonenumber>();
                    Phonenumber.type = "Home";
                    Phonenumber.number = contactobject.number;
                    //Phonenumber.number = passengerdetails.mobile;
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
                    Name.title = "MR";
                    _ContactModel.name = Name;

                    var jsonContactRequest = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking/contacts", _ContactModel);
                    if (responseAddContact.IsSuccessStatusCode)
                    {
                        var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                        var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                    }

                }
            }

            //SPICE JEt Return Contact APi Request
            Logs logs = new Logs();
            string Signature = HttpContext.Session.GetString("SpicejetSignautre");

            if (!string.IsNullOrEmpty(Signature))
            {
                Signature = Signature.Replace(@"""", string.Empty);
                UpdateContactsRequest _ContactModelSG = new UpdateContactsRequest();
                //  _ContactModel.emailAddress = passengerdetails.Email;
                _ContactModelSG.updateContactsRequestData = new UpdateContactsRequestData();
                _ContactModelSG.Signature = Signature;
                _ContactModelSG.ContractVersion = 420;
                _ContactModelSG.updateContactsRequestData.BookingContactList = new BookingContact[1];
                _ContactModelSG.updateContactsRequestData.BookingContactList[0] = new BookingContact();
                _ContactModelSG.updateContactsRequestData.BookingContactList[0].EmailAddress = contactobject.emailAddress;
                _ContactModelSG.updateContactsRequestData.BookingContactList[0].TypeCode = "G";
                _ContactModelSG.updateContactsRequestData.BookingContactList[0].CompanyName = "SpiceJet";
                _ContactModelSG.updateContactsRequestData.BookingContactList[0].CustomerNumber = "22AAAAA0000A1Z5"; //GSTNumber Re_ Assistance required for SG API Integration\GST Logs.zip\GST Logs
                SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                UpdateContactsResponse responseAddContactSG = await objSpiceJet.GetUpdateContactsAsync(_ContactModelSG);
                HttpContext.Session.SetString("ContactDetails", JsonConvert.SerializeObject(_ContactModelSG));
                String Str1 = JsonConvert.SerializeObject(responseAddContactSG);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_ContactModelSG) + "\n\n Response: " + JsonConvert.SerializeObject(responseAddContactSG), "UpdateContact");
            }
            return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
        }

        public async Task<IActionResult> PostReturnTravllerData(List<passkeytype> passengerdetails, List<Infanttype> infanttype)
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
                                var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                            }
                            //END
                        }
                    }


                    //SpiceJet Passenger DEtails Round Trip Request API
                    HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                }

                string Signature = HttpContext.Session.GetString("SpicejetSignautre");

                if (!string.IsNullOrEmpty(Signature))
                {
                    Signature = Signature.Replace(@"""", string.Empty);
                    UpdatePassengersResponse updatePaxResp = null;
                    UpdatePassengersRequest updatePaxReq = null;

                    try
                    {
                        updatePaxReq = new UpdatePassengersRequest(); //Assign Signature generated from Session
                        updatePaxReq.Signature = Signature;
                        updatePaxReq.ContractVersion = 420;
                        updatePaxReq.updatePassengersRequestData = new UpdatePassengersRequestData();
                        updatePaxReq.updatePassengersRequestData.Passengers = GetPassenger(passengerdetails);

                        try
                        {
                            SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                            updatePaxResp = await objSpiceJet.UpdatePassengers(updatePaxReq);

                            string Str2 = JsonConvert.SerializeObject(updatePaxResp);
                            Logs logs = new Logs();
                            logs.WriteLogs("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(updatePaxResp), "UpdatePassenger");

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                return RedirectToAction("RoundAATripsellView", "RoundAATripsell");

            }
        }


        //Post Unit Key
        public async Task<IActionResult> PostUnitkey(List<string> unitKey, List<string> ssrKey)
        {
            //#region AirAsia

            //Seat
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            if (!string.IsNullOrEmpty(tokenview))
            {
                token = tokenview.Replace(@"""", string.Empty);
                if (token == "" || token == null)
                {
                    return RedirectToAction("Index");
                }
            }

            string Meals = string.Empty;
            List<AirAsiaTripResponceModel> passeengerKeyListRT = new List<AirAsiaTripResponceModel>();
            string passenger = HttpContext.Session.GetString("keypassenger");
            AirAsiaTripResponceModel passeengerKeyList = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("keypassenger")))
            {
                passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));

                passeengerKeyListRT.Add(passeengerKeyList);
            }
            passenger = HttpContext.Session.GetString("SGkeypassengerRT");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SGkeypassengerRT")))
            {
                passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                passeengerKeyListRT.Add(passeengerKeyList);
            }


            List<SeatMapResponceModel> SeatmapListRT = new List<SeatMapResponceModel>();
            SeatMapResponceModel Seatmaplist = null;
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Seatmap")))
            {
                Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SeatmapListRT.Add(Seatmaplist);
            }
            Seatmap = HttpContext.Session.GetString("SeatmapRT");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SeatmapRT")))
            {
                Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SeatmapListRT.Add(Seatmaplist);
            }

            List<SSRAvailabiltyResponceModel> mealListRT = new List<SSRAvailabiltyResponceModel>();
            SSRAvailabiltyResponceModel Mealslist = null;
            Meals = HttpContext.Session.GetString("Meals");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Meals")))
            {
                Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                mealListRT.Add(Mealslist);
            }

            Meals = HttpContext.Session.GetString("SGMealsRT");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SGMealsRT")))
            {
                Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                mealListRT.Add(Mealslist);
            }


            int passengerscount = passeengerKeyList.passengerscount;
            var data = Seatmaplist.datalist.Count;
            //string legkey = passeengerKeyList.journeys[0].segments[0].legs[0].legKey;
            int Seatcount = unitKey.Count;




            #region RoundTripSSR


            if (ssrKey.Count > 0)
            {

                try
                {
                    var ssrKey_1 = ssrKey;// selectedIds;
                    string[] ssrKey2 = null;
                    string[] ssrsubKey2 = null;
                    string pas_ssrKey = string.Empty;

                    int journeyscount = 0;// passeengerKeyList.journeys.Count;
                    int mealid = 0;
                    for (int i1 = 0; i1 < passeengerKeyListRT.Count; i1++)
                    {
                        if (i1 == 0)
                        {
                            journeyscount = passeengerKeyListRT[0].journeys.Count;

                            for (int i = 0; i < journeyscount; i++)
                            {
                                //int segmentscount = passeengerKeyListRT[0].journeys[i].segments.Count;
                                //for (int k1 = 0; k1 < segmentscount; k1++)
                                //{
                                for (int l1 = 0; l1 < ssrKey.Count; l1++)
                                {
                                    int l = 0;
                                    int m = 0;
                                    int idx = 0;
                                    int paxnum = 0;
                                    //if (passeengerKeyListRT[0].passengers[l1].passengerTypeCode == "INFT")
                                    //    continue;
                                    if (ssrKey[l1].ToLower().Contains("airasia"))
                                    {
                                        if (ssrKey[l1].Length > 1)
                                        {

                                            ssrsubKey2 = ssrKey[l1].Split('_');
                                            pas_ssrKey = ssrsubKey2[0].Trim();
                                            //idx = int.Parse(ssrsubKey2[3]);
                                            //if (idx == 0) paxnum = l++;
                                            //else
                                            //paxnum = m++;

                                        }

                                        //string passengerkey = string.Empty;
                                        // passengerkey = passeengerKeyListRT[0].passengers[l1].passengerKey;
                                        string mealskey = pas_ssrKey;
                                        mealskey = mealskey.Replace(@"""", string.Empty);
                                        if (!string.IsNullOrEmpty(token))
                                        {
                                            using (HttpClient client = new HttpClient())
                                            {
                                                SellSSRModel _sellSSRModel = new SellSSRModel();
                                                _sellSSRModel.count = 1;
                                                _sellSSRModel.note = "DevTest";
                                                _sellSSRModel.forceWaveOnSell = false;
                                                _sellSSRModel.currencyCode = "INR";
                                                var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                                                if (responseSellSSR.IsSuccessStatusCode)
                                                {
                                                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    //continue;
                                    mealid++;
                                }
                                //seatid++;
                                //}
                            }
                        }
                        else
                        {
                            //
                            tokenview = HttpContext.Session.GetString("SpicejetSignautre");//spelling 
                            if (!string.IsNullOrEmpty(tokenview))
                            {
                                token = tokenview.Replace(@"""", string.Empty);
                                if (token == "" || token == null)
                                {
                                    return RedirectToAction("Index");
                                }

                                //int journeyscount = passeengerlist.journeys.Count;
                                passenger = HttpContext.Session.GetString("SGkeypassengerRT");
                                passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                                passengerscount = passeengerKeyList.passengerscount;
                                Logs logs = new Logs();
                                //string journeyKey = passeengerKeyList.journeys[0].journeyKey;
                                using (HttpClient client = new HttpClient())
                                {
                                    if (ssrKey.Count >= 0)
                                    {

                                        #region SellSSr
                                        SellRequest sellSsrRequest = new SellRequest();
                                        SellRequestData sellreqd = new SellRequestData();
                                        sellSsrRequest.Signature = token;
                                        sellSsrRequest.ContractVersion = 420;
                                        sellreqd.SellBy = SellBy.SSR;
                                        sellreqd.SellBySpecified = true;
                                        sellreqd.SellSSR = new SellSSR();
                                        sellreqd.SellSSR.SSRRequest = new SSRRequest();

                                        journeyscount = passeengerKeyList.journeys.Count;
                                        for (int i = 0; i < journeyscount; i++)
                                        {
                                            int segmentscount = passeengerKeyList.journeys[i].segments.Count;
                                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[segmentscount];
                                            for (int j = 0; j < segmentscount; j++)
                                            {
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j] = new SegmentSSRRequest();
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STD = passeengerKeyList.journeys[i].segments[j].designator.departure;
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new FlightDesignator();
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = passeengerKeyList.journeys[i].segments[j].identifier.carrierCode;
                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = passeengerKeyList.journeys[i].segments[j].identifier.identifier;
                                                string numinfant = HttpContext.Session.GetString("PaxArray");
                                                Paxes PaxNum = null;
                                                if (!string.IsNullOrEmpty(numinfant))
                                                {
                                                    PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));
                                                }
                                                bool infant = false;
                                                ssrsegmentwise _obj = new ssrsegmentwise();
                                                _obj.SSRcode0 = new List<ssrsKey>();
                                                _obj.SSRcode1 = new List<ssrsKey>();
                                                for (int k = 0; k < ssrKey.Count; k++)
                                                {
                                                    if (ssrKey[k].ToLower().Contains("airasia"))
                                                        continue;

                                                    if (ssrKey[k].Contains("_0"))
                                                    {

                                                        ssrsKey _obj0 = new ssrsKey();
                                                        _obj0.key = ssrKey[k];
                                                        _obj.SSRcode0.Add(_obj0);
                                                    }
                                                    else if (ssrKey[k].Contains("_1"))
                                                    {
                                                        ssrsKey _obj1 = new ssrsKey();
                                                        _obj1.key = ssrKey[k];
                                                        _obj.SSRcode1.Add(_obj1);
                                                    }

                                                }
                                                if (j == 0)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcode0.Count];
                                                }
                                                else if (j == 1)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcode1.Count];
                                                }


                                                int TotalPaxcount = PaxNum.Adults_.Count + PaxNum.Childs_.Count;

                                                if (j == 0)
                                                {
                                                    //int k = 0;

                                                    if (TotalPaxcount > 0)
                                                    {
                                                        for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcode0.Count; j1++)
                                                        {

                                                            if (j1 < PaxNum.Infant_.Count)
                                                            {
                                                                for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                {
                                                                    int infantcount = PaxNum.Infant_.Count;
                                                                    if (infantcount > 0 && i2 + 1 <= infantcount)
                                                                    {
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                        j1 = PaxNum.Infant_.Count - 1;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                int idx = 0;
                                                                if (_obj.SSRcode0.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                {
                                                                    for (int i2 = 0; i2 < _obj.SSRcode0.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                    {
                                                                        string[] wordsArray = _obj.SSRcode0[i2].key.ToString().Split(' ');
                                                                        //alert(wordsArray);
                                                                        //var meal = null;
                                                                        string ssrCodeKey = "";
                                                                        if (wordsArray.Length > 1)
                                                                        {
                                                                            ssrCodeKey = wordsArray[0];
                                                                            ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                        }
                                                                        else
                                                                            ssrCodeKey = _obj.SSRcode0[i2].key.ToString();
                                                                        idx = j1 + i2;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                        //j1 = j1 + i1;

                                                                    }

                                                                }
                                                                j1 = idx;
                                                            }
                                                        }

                                                    }


                                                }
                                                else
                                                {
                                                    if (TotalPaxcount > 0)
                                                    {
                                                        for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcode1.Count; j1++)
                                                        {

                                                            if (j1 < PaxNum.Infant_.Count)
                                                            {
                                                                for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                {
                                                                    int infantcount = PaxNum.Infant_.Count;
                                                                    if (infantcount > 0 && i2 + 1 <= infantcount)
                                                                    {
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                        j1 = PaxNum.Infant_.Count - 1;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                int idx = 0;
                                                                if (_obj.SSRcode1.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                {
                                                                    for (int i2 = 0; i2 < _obj.SSRcode1.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                    {
                                                                        string[] wordsArray = _obj.SSRcode1[i2].key.ToString().Split(' ');
                                                                        //alert(wordsArray);
                                                                        //var meal = null;
                                                                        string ssrCodeKey = "";
                                                                        if (wordsArray.Length > 1)
                                                                        {
                                                                            ssrCodeKey = wordsArray[0];
                                                                            ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                        }
                                                                        else
                                                                            ssrCodeKey = _obj.SSRcode1[i2].key.ToString();
                                                                        idx = j1 + i2;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                        //j1 = j1 + i1;

                                                                    }

                                                                }
                                                                j1 = idx;
                                                            }
                                                        }

                                                    }
                                                }

                                            }
                                        }


                                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs=

                                        sellSsrRequest.SellRequestData = sellreqd;
                                        SellResponse sellSsrResponse = null;
                                        sellreqd.SellSSR.SSRRequest.SellSSRMode = SellSSRMode.NonBundle;
                                        sellreqd.SellSSR.SSRRequest.SellSSRModeSpecified = true;
                                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                                        sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                                        string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR");


                                        if (sellSsrResponse != null)
                                        {
                                            //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                            var JsonObjSeatAssignment = sellSsrResponse;
                                        }
                                        #endregion

                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }



            }

            #endregion


            #region SeatMap RoundTrip Both
            //here to do 
            if (unitKey.Count > 0)
            {
                try
                {
                    tokenview = HttpContext.Session.GetString("AirasiaTokan");
                    if (!string.IsNullOrEmpty(tokenview))
                    {
                        token = tokenview.Replace(@"""", string.Empty);
                        if (token == "" || token == null)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    var unitKey_1 = unitKey;// selectedIds;
                    string[] unitKey2 = null;
                    string[] unitsubKey2 = null;
                    string pas_unitKey = string.Empty;

                    int journeyscount = 0;// passeengerKeyList.journeys.Count;
                    int seatid = 0;
                    for (int i1 = 0; i1 < passeengerKeyListRT.Count; i1++)
                    {
                        if (i1 == 0)
                        {
                            journeyscount = passeengerKeyListRT[0].journeys.Count;

                            for (int i = 0; i < journeyscount; i++)
                            {
                                int segmentscount = passeengerKeyListRT[0].journeys[i].segments.Count;
                                for (int k1 = 0; k1 < segmentscount; k1++)
                                {
                                    for (int l1 = 0; l1 < passeengerKeyListRT[0].passengerscount; l1++)
                                    {
                                        int l = 0;
                                        int m = 0;
                                        int idx = 0;
                                        int paxnum = 0;
                                        if (passeengerKeyListRT[0].passengers[l1].passengerTypeCode == "INFT")
                                            continue;
                                        if (unitKey[seatid].ToLower().Contains("airasia"))
                                        {
                                            if (unitKey[seatid].Length > 1)
                                            {

                                                unitsubKey2 = unitKey[seatid].Split('_');
                                                pas_unitKey = unitsubKey2[1].Trim();
                                                idx = int.Parse(unitsubKey2[3]);
                                                if (idx == 0) paxnum = l++;
                                                else
                                                    paxnum = m++;

                                            }

                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyListRT[0].passengers[l1].passengerKey;
                                            string unitkey = pas_unitKey;
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyListRT[0].journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaSeatSelect + unitkey, _SeatAssignmentModel);

                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        //continue;
                                        seatid++;
                                    }
                                    //seatid++;
                                }
                            }
                        }
                        else
                        {
                            if (unitKey.Count > 0)
                            {

                                try
                                {
                                    int keycount = 0;
                                    for (int k = 0; k < unitKey.Count; k++)
                                    {
                                        if (unitKey[k].ToLower().Contains("spicejet"))
                                            keycount++;
                                    }
                                    Logs _logs = new Logs();
                                    journeyscount = passeengerKeyListRT[1].journeys.Count;
                                    AssignSeatsResponse _AssignseatRes = new AssignSeatsResponse();
                                    AssignSeatsRequest _AssignSeatReq = new AssignSeatsRequest();
                                    string Signature = HttpContext.Session.GetString("SpicejetSignautre");
                                    if (!string.IsNullOrEmpty(Signature))
                                    {
                                        Signature = Signature.Replace(@"""", string.Empty);
                                        _AssignSeatReq.Signature = Signature;
                                        _AssignSeatReq.ContractVersion = 420;
                                        _AssignSeatReq.SellSeatRequest = new SeatSellRequest();
                                        _AssignSeatReq.SellSeatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                                        _AssignSeatReq.SellSeatRequest.SeatAssignmentModeSpecified = true;
                                        _AssignSeatReq.SellSeatRequest.SegmentSeatRequests = new SegmentSeatRequest[keycount];// [unitKey.Count];//to do

                                        for (int i2 = 0; i2 < journeyscount; i2++)
                                        {
                                            //    int segmentscount = passeengerKeyList.journeys[i].segments.Count;
                                            //    //for (int j = 0; j < segmentscount; j++)
                                            int l = 0;
                                            int m = 0;
                                            for (int k = 0; k < keycount; k++)
                                            {
                                                int idx = 0;
                                                int paxnum = 0;
                                                if (unitKey[seatid].Length > 1)
                                                {
                                                    unitsubKey2 = unitKey[seatid].Split('_');
                                                    pas_unitKey = unitsubKey2[1];
                                                    idx = int.Parse(unitsubKey2[3]);
                                                    if (idx == 0) paxnum = l++;
                                                    else
                                                        paxnum = m++;

                                                }

                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k] = new SegmentSeatRequest();
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator = new FlightDesignator();
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator.CarrierCode = passeengerKeyListRT[1].journeys[i2].segments[idx].identifier.carrierCode;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator.FlightNumber = passeengerKeyListRT[1].journeys[i2].segments[idx].identifier.identifier;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].STD = passeengerKeyListRT[1].journeys[i2].segments[idx].designator.departure;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].STDSpecified = true;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].DepartureStation = passeengerKeyListRT[1].journeys[i2].segments[idx].designator.origin;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].ArrivalStation = passeengerKeyListRT[1].journeys[i2].segments[idx].designator.destination;
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].UnitDesignator = pas_unitKey.Trim();
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].PassengerNumbers = new short[1];
                                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].PassengerNumbers[0] = Convert.ToInt16(paxnum);
                                                seatid++;
                                            }

                                        }

                                        _AssignSeatReq.SellSeatRequest.IncludeSeatData = true;
                                        _AssignSeatReq.SellSeatRequest.IncludeSeatDataSpecified = true;

                                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                                        _AssignseatRes = await objSpiceJet.Assignseat(_AssignSeatReq);

                                        string Str2 = JsonConvert.SerializeObject(_AssignseatRes);

                                        _logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat");
                                    }
                                    //if (_AssignseatRes != null)
                                    //{
                                    //    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    //    var JsonObjSeatAssignment = _AssignseatRes;
                                    //}
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            #endregion

            return RedirectToAction("RoundTripPaymentView", "RoundTripPaymentGateway");
        }
        public class Paxes
        {
            public List<passkeytype> Adults_ { get; set; }
            public List<passkeytype> Childs_ { get; set; }

            public List<passkeytype> Infant_ { get; set; }
        }
        Paxes _paxes = new Paxes();

        public Passenger[] GetPassenger(List<passkeytype> travellers_)
        {

            _paxes.Adults_ = new List<passkeytype>();
            _paxes.Childs_ = new List<passkeytype>();
            _paxes.Infant_ = new List<passkeytype>();
            for (int i = 0; i < travellers_.Count; i++)
            {
                if (travellers_[i].passengertypecode == "ADT")
                    _paxes.Adults_.Add(travellers_[i]);
                else if (travellers_[i].passengertypecode == "CHD")
                    _paxes.Childs_.Add(travellers_[i]);
                else if (travellers_[i].passengertypecode == "INFT")
                    _paxes.Infant_.Add(travellers_[i]);

            }

            HttpContext.Session.SetString("PaxArray", JsonConvert.SerializeObject(_paxes));

            Passenger[] passengers = null;
            try
            {



                int chdPax = 0;
                int infFax = 0;
                if (_paxes.Childs_ != null)
                {
                    chdPax = _paxes.Childs_.Count;
                }
                if (_paxes.Infant_ != null)
                {
                    infFax = _paxes.Infant_.Count;
                }
                passengers = new Passenger[_paxes.Adults_.Count + chdPax]; //Assign Passenger Information 
                Passenger p1 = null;
                int PassCnt = 0;
                for (int cntAdt = 0; cntAdt < _paxes.Adults_.Count; cntAdt++)
                {
                    p1 = new Passenger();
                    p1.PassengerNumberSpecified = true;
                    p1.PassengerNumber = Convert.ToInt16(PassCnt);
                    p1.Names = new BookingName[1];
                    p1.Names[0] = new BookingName();
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].first))
                    {
                        p1.Names[0].FirstName = Convert.ToString(_paxes.Adults_[cntAdt].first.Trim()).ToUpper();
                    }
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].middle))
                    {
                        p1.Names[0].MiddleName = Convert.ToString(_paxes.Adults_[cntAdt].middle.Trim()).ToUpper();
                    }
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].last))
                    {
                        p1.Names[0].LastName = Convert.ToString(_paxes.Adults_[cntAdt].last.Trim()).ToUpper();
                    }
                    p1.Names[0].Title = _paxes.Adults_[cntAdt].title.ToUpper().Replace(".", "");
                    p1.PassengerInfo = new PassengerInfo();
                    if (_paxes.Adults_[cntAdt].title.ToUpper().Replace(".", "") == "MR")
                    {
                        p1.PassengerInfo.Gender = Gender.Male;
                        p1.PassengerInfo.WeightCategory = WeightCategory.Male;
                    }
                    else
                    {
                        p1.PassengerInfo.Gender = Gender.Female;
                        p1.PassengerInfo.WeightCategory = WeightCategory.Female;
                    }
                    p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                    p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                    p1.PassengerTypeInfos[0].DOBSpecified = true;
                    p1.PassengerTypeInfos[0].PaxType = _paxes.Adults_[cntAdt].passengertypecode.ToString().ToUpper();
                    if (_paxes.Infant_ != null && _paxes.Infant_.Count > 0)
                    {
                        if (cntAdt < _paxes.Infant_.Count)
                        {
                            p1.Infant = new PassengerInfant();
                            p1.Infant.DOBSpecified = true;
                            p1.Infant.DOB = Convert.ToDateTime("2023-08-01");//Convert.ToDateTime(_paxes.Infant_[cntAdt].dateOfBirth);
                                                                             //p1.Infant.Gender = Gender.Male;
                            if (_paxes.Infant_[cntAdt].title.ToUpper().Replace(".", "") == "MR")
                            {
                                p1.Infant.Gender = Gender.Male;
                            }
                            else
                            {
                                p1.Infant.Gender = Gender.Female;
                            }
                            p1.Infant.Names = new BookingName[1];
                            p1.Infant.Names[0] = new BookingName();
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].first))
                            {
                                p1.Infant.Names[0].FirstName = Convert.ToString(_paxes.Infant_[cntAdt].first.Trim());
                            }
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].middle))
                            {
                                p1.Infant.Names[0].MiddleName = Convert.ToString(_paxes.Infant_[cntAdt].middle.Trim());
                            }
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].last))
                            {
                                p1.Infant.Names[0].LastName = Convert.ToString(_paxes.Infant_[cntAdt].last.Trim());
                            }
                            p1.Infant.Names[0].Title = _paxes.Infant_[cntAdt].title.Replace(".", "");
                            p1.Infant.Nationality = _paxes.Infant_[cntAdt].nationality;
                            p1.Infant.ResidentCountry = _paxes.Infant_[cntAdt].residentCountry;
                            p1.State = MessageState.New;
                        }

                    }

                    passengers[PassCnt] = p1;
                    PassCnt++;
                }
                if (_paxes.Childs_ != null)
                {
                    for (int cntChd = 0; cntChd < _paxes.Childs_.Count; cntChd++)
                    {
                        p1 = new Passenger();

                        p1.PassengerNumberSpecified = true;
                        p1.PassengerNumber = Convert.ToInt16(PassCnt);
                        p1.Names = new BookingName[1];
                        p1.Names[0] = new BookingName();

                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].first))
                        {
                            p1.Names[0].FirstName = Convert.ToString(_paxes.Childs_[cntChd].first).ToUpper();
                        }
                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].middle))
                        {
                            p1.Names[0].MiddleName = Convert.ToString(_paxes.Childs_[cntChd].middle).ToUpper();
                        }
                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].last))
                        {
                            p1.Names[0].LastName = Convert.ToString(_paxes.Childs_[cntChd].last).ToUpper();
                        }
                        p1.Names[0].Title = _paxes.Childs_[cntChd].title.ToUpper().Replace(".", "");
                        p1.PassengerInfo = new PassengerInfo();
                        if (_paxes.Childs_[cntChd].title.ToUpper().Replace(".", "") == "Mr")
                        {
                            p1.PassengerInfo.Gender = Gender.Male;
                            p1.PassengerInfo.WeightCategory = WeightCategory.Male;
                        }
                        else
                        {
                            p1.PassengerInfo.Gender = Gender.Female;
                            p1.PassengerInfo.WeightCategory = WeightCategory.Female;
                        }
                        p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                        p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                        p1.PassengerTypeInfos[0].DOBSpecified = true;
                        p1.PassengerTypeInfos[0].PaxType = _paxes.Childs_[cntChd].passengertypecode.ToString().ToUpper();
                        passengers[PassCnt] = p1;
                        PassCnt++;
                    }
                }
            }
            catch (SystemException sex_)
            {
            }
            return passengers;
        }
    }
}