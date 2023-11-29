using System;
using System.Collections.Generic;
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
//using static DomainLayer.Model.testseat;

namespace OnionConsumeWebAPI.Controllers
{
    public class AATripsellController : Controller
    {
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
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
            string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            ViewModel vm = new ViewModel();

            if (passengerInfant != null)
            {
                AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                AirAsiaTripResponceModel passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.passeengerlistItanary = passeengerlistItanary;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
            }
            else
            {
                AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;


            }



            //vm.passeengerlist = passeengerlistItanary;

            return View(vm);




        }
        public async Task<IActionResult> ContectDetails(ContactModel contactobject)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
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
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/contacts", _ContactModel);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }

            }
            return RedirectToAction("Tripsell", "AATripsell");
        }
        public async Task<IActionResult> TravllerDetails(List<passkeytype> passengerdetails, List<Infanttype> infanttype)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                PassengersModel _PassengersModel = new PassengersModel();
                for (int i = 0; i < passengerdetails.Count; i++)
                {
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
                        HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(BaseURL + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey, _PassengersModel);
                        if (responsePassengers.IsSuccessStatusCode)
                        {
                            var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                        }
                    }
                }
                AddInFantModel _PassengersModel1 = new AddInFantModel();
                for (int i = 0; i < infanttype.Count; i++)
                {
                    if (infanttype[i].code != null)
                    {
                        _PassengersModel1.nationality = "IN";
                        _PassengersModel1.dateOfBirth = "2023-10-01";
                        _PassengersModel1.residentCountry = "IN";
                        _PassengersModel1.gender = "Male";
                       
                        InfantName nameINF = new InfantName();
                        nameINF.first = infanttype[i].First;
                        nameINF.middle = "";
                        nameINF.last = infanttype[i].Last;
                        nameINF.title = "Mr";
                        nameINF.suffix = "";
                        _PassengersModel1.name = nameINF;


                        var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v3/booking/passengers/" + infanttype[i].passengerkey + "/infant", _PassengersModel1);
                        if (responsePassengers.IsSuccessStatusCode)
                        {
                            var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                        }

                        // STRAT Get INFO
                        // var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responceGetBooking = await client.GetAsync(BaseURL + "/api/nsk/v1/booking");
                        if (responceGetBooking.IsSuccessStatusCode)
                        {
                            var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                            var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                        }
                        //END



                    }
                }



            }

            return RedirectToAction("GetSSRAvailabilty", "AATripsell", passengerdetails);
            //return RedirectToAction("Tripsell", "AATripsell");


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
                HttpResponseMessage responseSSRAvailabilty = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/availability", _SSRAvailabilty);
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


        public async Task<IActionResult> GetGstDetails(AddGSTInformation addGSTInformation, string lineOne, string lineTwo, string city, string number, string postalCode)
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
                HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/contacts", addinformation);
                if (responseAddContact.IsSuccessStatusCode)
                {
                    var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                    var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                }

            }

            return RedirectToAction("Tripsell", "AATripsell");
        }

        public async Task<IActionResult> PostUnitkey(List<string> unitKey, List<string> ssrKey)
        {
            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }


            string passenger = HttpContext.Session.GetString("keypassenger");
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
            SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
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
                        //mealskey = ssrKey[i];
                        string passengerkey = passeengerKeyList.passengers[j].passengerKey;
                        using (HttpClient client = new HttpClient())
                        {
                            string journeyKey = passeengerKeyList.journeys[0].journeyKey;
                            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                            _SeatAssignmentModel.journeyKey = journeyKey;
                            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/seats/auto/" + passengerkey, _SeatAssignmentModel);

                            if (responceSeatAssignment.IsSuccessStatusCode)
                            {
                                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                            }

                        }
                    }
                }
                //int mealssr = Mealslist.legSsrs.Count;
                //for (int j = 0; j < mealssr; j++)
                //{

                //    for (int i = 0; i < passengerscount; i++)
                //    {
                //        string mealskey = string.Empty;
                //        mealskey = ssrKey[i];
                //        //string ssrkey = Meals[0].SSRAvailabiltyResponceModel[0].legSsrs[0].legssrs[0].legpassengers.ssrKey;
                //        using (HttpClient client = new HttpClient())
                //        {

                //            SellSSRModel _sellSSRModel = new SellSSRModel();
                //            _sellSSRModel.count = 1;
                //            _sellSSRModel.note = "DevTest";
                //            _sellSSRModel.forceWaveOnSell = false;
                //            _sellSSRModel.currencyCode = "INR";


                //            var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //            HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                //            if (responseSellSSR.IsSuccessStatusCode)
                //            {
                //                var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                //                var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                //            }
                //        }
                //    }

                //}


            }
            else
            {
                int seatid = 0;
                for (int i = 0; i < data; i++)
                {
                    for (int j = 0; j < passengerscount; j++)
                    {

                        string unitKey1 = string.Empty;
                        //mealskey = ssrKey[i];
                        string passengerkey = passeengerKeyList.passengers[j].passengerKey;
                        using (HttpClient client = new HttpClient())
                        {

                            string journeyKey = passeengerKeyList.journeys[0].journeyKey;

                            unitKey1 = unitKey[seatid];
                            string[] unitKey2 = unitKey1.Split('_');
                            string pas_unitKey = unitKey2[1];
                            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                            _SeatAssignmentModel.journeyKey = journeyKey;

                            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                            //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/seats/auto/" + passengerkey, _SeatAssignmentModel);

                            if (responceSeatAssignment.IsSuccessStatusCode)
                            {
                                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                            }

                        }
                        seatid++;


                    }



                }

                int mealssr = Mealslist.legSsrs.Count;

                for (int j = 0; j < mealssr; j++)
                {

                    for (int i = 0; i < passengerscount; i++)
                    {
                        string mealskey = string.Empty;
                        mealskey = ssrKey[i];
                        //string ssrkey = Meals[0].SSRAvailabiltyResponceModel[0].legSsrs[0].legssrs[0].legpassengers.ssrKey;
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
                            HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                            if (responseSellSSR.IsSuccessStatusCode)
                            {
                                var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                            }
                        }
                    }

                }
            }




            return RedirectToAction("Payment", "PaymentGateway");

        }



    }
}







