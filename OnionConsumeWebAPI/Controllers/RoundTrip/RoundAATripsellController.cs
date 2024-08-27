using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
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
using OnionArchitectureAPI.Services.Indigo;
using OnionConsumeWebAPI.Extensions;
using Utility;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.SeatMapResponceModel;
using static OnionConsumeWebAPI.Controllers.IndigoTripsellController;

namespace OnionConsumeWebAPI.Controllers.RoundTrip
{
    public class RoundAATripsellController : Controller
    {
        IHttpContextAccessor httpContextAccessorInstance = new HttpContextAccessor();
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        AirAsiaTripResponceModel passeengerlist = null;
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
            AirAsiaTripResponceModel passeengerlistItanary = null;
            if (passengerInfant != null)
            {
                passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
            }
            //
            ViewModel vm = new ViewModel();

            //AirAsia Passeneger
            string test = string.Empty;
            string Meals = string.Empty;// HttpContext.Session.GetString("Meals");
            SSRAvailabiltyResponceModel Mealslist = null;
            SeatMapResponceModel Seatmaplist = null;
            string Seatmap = string.Empty;
            string passenger = HttpContext.Session.GetString("keypassenger");
            vm.SeatmaplistRT = new List<SeatMapResponceModel>();
            vm.passeengerlistRT = new List<AirAsiaTripResponceModel>();
            vm.MealslistRT = new List<SSRAvailabiltyResponceModel>();
            vm.passeengerlistItanary = passeengerlistItanary;
            string Passenegrtext = HttpContext.Session.GetString("Mainpassengervm");
            string Seattext = HttpContext.Session.GetString("Mainseatmapvm");
            string Mealtext = HttpContext.Session.GetString("Mainmealvm");
            string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");

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
            if (!string.IsNullOrEmpty(passengerNamedetails))
            {
                List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                vm.passengerNamedetails = passengerNamedetailsdata;
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
            }

            #endregion


            return View(vm);


        }
        public IActionResult RTPostSeatMapModaldataView()
        {
            ViewModel vm = new ViewModel();
            string test = string.Empty;
            string Meals = string.Empty;
            SSRAvailabiltyResponceModel Mealslist = null;
            SeatMapResponceModel Seatmaplist = null;
            string Seatmap = string.Empty;
            vm.SeatmaplistRT = new List<SeatMapResponceModel>();
            vm.passeengerlistRT = new List<AirAsiaTripResponceModel>();
            vm.MealslistRT = new List<SSRAvailabiltyResponceModel>();

            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };
            ViewBag.Title = Title;
            var AirlineName = TempData["AirLineName"];
            ViewData["name"] = AirlineName;

            string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
            string passenger = HttpContext.Session.GetString("keypassenger");
            string Passenegrtext = HttpContext.Session.GetString("Mainpassengervm");
            string Seattext = HttpContext.Session.GetString("Mainseatmapvm");
            string Mealtext = HttpContext.Session.GetString("Mainmealvm");
            string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");
            string BaggageDataR = HttpContext.Session.GetString("BaggageDetails");

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
            }
            if (!string.IsNullOrEmpty(BaggageDataR))
            {
                SSRAvailabiltyResponceModel RBaggageDataDetails = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(BaggageDataR, typeof(SSRAvailabiltyResponceModel));
                vm.Baggage = RBaggageDataDetails;
            }
            if (!string.IsNullOrEmpty(passengerNamedetails))
            {
                List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                vm.passengerNamedetails = passengerNamedetailsdata;
            }


            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainseatmapvm")))
            {
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
            }

            Meals = string.Empty;
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
            }

            #endregion


            return View(vm);
        }

        public async Task<IActionResult> PostReturnContactData(ContactModel contactobject, AddGSTInformation addGSTInformation)
        {
            HttpContext.Session.SetString("GDSContactdetails", JsonConvert.SerializeObject(contactobject));
            string SelectedAirlinedata = HttpContext.Session.GetString("SelectedAirlineName");
            string[] dataArray = JsonConvert.DeserializeObject<string[]>(SelectedAirlinedata);
            for (int i = 0; i < dataArray.Length; i++)
            {

                string tokenview = string.Empty;
                if (i == 0)
                {
                    tokenview = HttpContext.Session.GetString("AirasiaTokan");
                }
                else
                {
                    tokenview = HttpContext.Session.GetString("AirasiaTokanR");
                }

                if (!string.IsNullOrEmpty(tokenview) && dataArray[i].ToLower() == "airasia")
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
                            Logs logs1 = new Logs();
                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_ContactModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v1/booking/contacts" + "\n Response: " + JsonConvert.SerializeObject(_responseAddContact), "Update Contacts", "AirAsiaRT");

                            var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                        }

                    }
                }

                // Akasa Air return Contact Api 
                tokenview = string.Empty;
                if (i == 0)
                {
                    tokenview = HttpContext.Session.GetString("AkasaTokan");
                }
                else
                {
                    tokenview = HttpContext.Session.GetString("AkasaTokanR");
                }

                if (!string.IsNullOrEmpty(tokenview) && dataArray[i].ToLower() == "akasaair")
                {
                    if (tokenview == null) { tokenview = ""; }
                    token = tokenview.Replace(@"""", string.Empty);
                    using (HttpClient client = new HttpClient())
                    {
                        ContactModel _ContactModel = new ContactModel();
                        //  _ContactModel.emailAddress = passengerdetails.Email;
                        _ContactModel.emailAddress = contactobject.emailAddress;
                        _ContactModel.customerNumber = null; //contactobject.customerNumber;
                        _ContactModel.sourceOrganization = "QPCCJ5003C";
                        _ContactModel.distributionOption = null;
                        _ContactModel.notificationPreference = null;
                        _ContactModel.companyName = contactobject.companyName;
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
                        Address.lineOne = "Ashokenagar,bharathi cross str";
                        Address.countryCode = "IN";
                        Address.provinceState = "TN";
                        Address.city = "Ashokenagar";
                        Address.postalCode = "400006";
                        _ContactModel.address = Address;

                        _Name Name = new _Name();
                        Name.first = contactobject.first; //"VIGNESHWARAN";
                        Name.middle = "";
                        Name.last = contactobject.last; //"VIGNESHWARAN";
                        Name.title = "MR";
                        _ContactModel.name = Name;

                        var jsonContactRequest = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v1/booking/contacts", _ContactModel);
                        if (responseAddContact.IsSuccessStatusCode)
                        {
                            var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
                            Logs logs1 = new Logs();
                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_ContactModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v1/booking/contacts" + "\n Response: " + JsonConvert.SerializeObject(_responseAddContact), "Update Contacts", "AkasaRT");

                            var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
                        }

                    }
                }




                //SPICE JEt Return Contact APi Request
                Logs logs = new Logs();
                string Signature = string.Empty;
                if (i == 0)
                {
                    Signature = HttpContext.Session.GetString("SpicejetSignature");
                }
                else
                {
                    Signature = HttpContext.Session.GetString("SpicejetSignatureR");
                }

                if (!string.IsNullOrEmpty(Signature) && dataArray[i].ToLower() == "spicejet")
                {
                    if (Signature == null) { Signature = ""; }
                    Signature = Signature.Replace(@"""", string.Empty);
                    UpdateContactsRequest _ContactModelSG = new UpdateContactsRequest();
                    //  _ContactModel.emailAddress = passengerdetails.Email;
                    _ContactModelSG.updateContactsRequestData = new UpdateContactsRequestData();
                    _ContactModelSG.Signature = Signature;
                    _ContactModelSG.ContractVersion = 420;
                    _ContactModelSG.updateContactsRequestData.BookingContactList = new BookingContact[1];
                    _ContactModelSG.updateContactsRequestData.BookingContactList[0] = new BookingContact();
                   
                    if (contactobject.customerNumber != null && contactobject.customerNumber != "")
                    {
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].TypeCode = "G";
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].CompanyName = contactobject.companyName;
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].CustomerNumber = contactobject.customerNumber; //"22AAAAA0000A1Z5"; //GSTNumber Re_ Assistance required for SG API Integration\GST Logs.zip\GST Logs
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].EmailAddress = contactobject.emailAddressgst;
                    }
                    else
                    {
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].TypeCode = "P";
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].CountryCode = "IN";
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].HomePhone = contactobject.number;
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].EmailAddress = contactobject.emailAddress;
                        BookingName[] Name = new BookingName[1];
                        Name[0] = new BookingName();
                        Name[0].FirstName = contactobject.first;
                        Name[0].LastName = contactobject.last;
                        Name[0].Title = "MR";
                        _ContactModelSG.updateContactsRequestData.BookingContactList[0].Names = Name;
                    }
                    SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                    UpdateContactsResponse responseAddContactSG = await objSpiceJet.GetUpdateContactsAsync(_ContactModelSG);
                    HttpContext.Session.SetString("ContactDetails", JsonConvert.SerializeObject(_ContactModelSG));
                    string Str1 = JsonConvert.SerializeObject(responseAddContactSG);

                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_ContactModelSG) + "\n\n Response: " + JsonConvert.SerializeObject(responseAddContactSG), "UpdateContact", "SpiceJetRT");
                }

                if (i == 0)
                {
                    Signature = HttpContext.Session.GetString("IndigoSignature");

                }
                else
                {
                    Signature = HttpContext.Session.GetString("IndigoSignatureR");
                }
                if (!string.IsNullOrEmpty(Signature) && dataArray[i].ToLower() == "indigo")
                {
                    if (Signature == null) { Signature = ""; }
                    Signature = Signature.Replace(@"""", string.Empty);
                    _updateContact obj = new _updateContact(httpContextAccessorInstance);
                    IndigoBookingManager_.UpdateContactsResponse _responseAddContact6E = await obj.GetUpdateContacts(Signature, contactobject.emailAddress, contactobject.emailAddressgst, contactobject.number, contactobject.companyName, contactobject.customerNumber, "");
                    string Str1 = JsonConvert.SerializeObject(_responseAddContact6E);
                }

            }

            return RedirectToAction("_RGetGstDetails", "RoundAATripsell", contactobject);
        }

        //GST Code For Akasa and Airasia
        public async Task<IActionResult> _RGetGstDetails(ContactModel contactobject, AddGSTInformation addGSTInformation)
        {

            string SelectedAirlinedata = HttpContext.Session.GetString("SelectedAirlineName");
            string[] dataArray = JsonConvert.DeserializeObject<string[]>(SelectedAirlinedata);
            for (int i = 0; i < dataArray.Length; i++)
            {
                string tokenview = string.Empty;
                if (i == 0)
                {
                    tokenview = HttpContext.Session.GetString("AirasiaTokan");
                }
                else
                {
                    tokenview = HttpContext.Session.GetString("AirasiaTokanR");
                }


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
                    addinformation.emailAddress = contactobject.emailAddressgst;
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

                tokenview = string.Empty;
                if (i == 0)
                {
                    tokenview = HttpContext.Session.GetString("AkasaTokan");
                }
                else
                {
                    tokenview = HttpContext.Session.GetString("AkasaTokanR");
                }


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


            }
            return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
        }
        [HttpPost]
        public async Task<IActionResult> PostReturnTravllerData(List<passkeytype> passengerdetails, List<Infanttype> infanttype)
        {
            SSRAvailabiltyResponceModel Mealslist = null;
            SeatMapResponceModel Seatmaplist = null;
            ViewModel vm = new ViewModel();
            string SelectedAirlinedata = HttpContext.Session.GetString("SelectedAirlineName");
            string[] dataArray = JsonConvert.DeserializeObject<string[]>(SelectedAirlinedata);
            for (int i1 = 0; i1 < dataArray.Length; i1++)
            {
                string tokenview = HttpContext.Session.GetString("AirasiaTokan");


                using (HttpClient client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(tokenview) && dataArray[i1].ToLower() == "airasia")
                    {

                        tokenview = string.Empty;
                        if (i1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("AirasiaTokan");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("AirasiaTokanR");
                        }
                        token = tokenview.Replace(@"""", string.Empty);
                        PassengersModel _PassengersModel = new PassengersModel();
                        for (int i = 0; i < passengerdetails.Count; i++)
                        {
                            string _airlinename = string.Empty;
                            string[] arraypaxkey = passengerdetails[i].passengerkey.Split('@');
                            if (arraypaxkey.Length > 1)
                            {
                                string[] arraypaxdata = arraypaxkey[0].Split('^');
                                if (arraypaxdata.Length > 1)
                                {
                                    _airlinename = arraypaxdata[1];
                                }
                                if (dataArray[i1].ToLower() == _airlinename.ToLower())
                                {
                                    passengerdetails[i].passengerkey = arraypaxdata[0];
                                }
                                else
                                {
                                    arraypaxdata = arraypaxkey[1].Split('^');
                                    _airlinename = arraypaxdata[1];
                                    if (dataArray[i1].ToLower() == _airlinename.ToLower())
                                    {
                                        passengerdetails[i].passengerkey = arraypaxdata[0];
                                    }

                                }
                            }
                            if (passengerdetails[i].passengertypecode == "INFT" || passengerdetails[i].passengertypecode == "INF")
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
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                            if (passengerdetails[k].passengertypecode == "INFT" || passengerdetails[k].passengertypecode == "INF")
                                infantcount++;

                        }

                        AddInFantModel _PassengersModel1 = new AddInFantModel();
                        for (int i = 0; i < passengerdetails.Count; i++)
                        {
                            if (passengerdetails[i].passengertypecode == "ADT" || passengerdetails[i].passengertypecode == "CHD")
                                continue;
                            if (passengerdetails[i].passengertypecode == "INFT" || passengerdetails[i].passengertypecode == "INF")
                            {
                                for (int k = 0; k < infantcount; k++)
                                {


                                    _PassengersModel1.nationality = "IN";
                                    _PassengersModel1.dateOfBirth = "2023-01-01";
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
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking");
                                if (responceGetBooking.IsSuccessStatusCode)
                                {
                                    var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                                    Logs logs = new Logs();
                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetBookingRequest") + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v1/booking" + "\n Response: " + JsonConvert.SerializeObject(_responceGetBooking), "GetBooking", "AirAsiaRT");

                                    var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                                }
                                //END
                            }
                        }


                        //SpiceJet Passenger DEtails Round Trip Request API
                        HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                    }

                    // Akasa trevvel Details Request ********
                    if (!string.IsNullOrEmpty(tokenview) && dataArray[i1].ToLower() == "akasaair")
                    {

                        tokenview = string.Empty;
                        if (i1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("AkasaTokan");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("AkasaTokanR");
                        }
                        if (tokenview == null) { tokenview = ""; }
                        token = tokenview.Replace(@"""", string.Empty);
                        PassengersModel _PassengersModel = new PassengersModel();
                        for (int i = 0; i < passengerdetails.Count; i++)
                        {
                            string _airlinename = string.Empty;
                            string[] arraypaxkey = passengerdetails[i].passengerkey.Split('@');
                            if (arraypaxkey.Length > 1)
                            {
                                string[] arraypaxdata = arraypaxkey[0].Split('^');
                                if (arraypaxdata.Length > 1)
                                {
                                    _airlinename = arraypaxdata[1];
                                }
                                if (dataArray[i1].ToLower() == _airlinename.ToLower())
                                {
                                    passengerdetails[i].passengerkey = arraypaxdata[0];
                                }
                                else
                                {
                                    arraypaxdata = arraypaxkey[1].Split('^');
                                    _airlinename = arraypaxdata[1];
                                    if (dataArray[i1].ToLower() == _airlinename.ToLower())
                                    {
                                        passengerdetails[i].passengerkey = arraypaxdata[0];
                                    }

                                }
                            }
                            if (passengerdetails[i].passengertypecode == "INFT" || passengerdetails[i].passengertypecode == "INF")
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
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey, _PassengersModel);
                                if (responsePassengers.IsSuccessStatusCode)
                                {
                                    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                                    Logs logs = new Logs();
                                    //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_PassengersModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger", "AkasaAirRT");

                                    var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                                }
                            }
                        }

                        int infantcount = 0;
                        for (int k = 0; k < passengerdetails.Count; k++)
                        {
                            if (passengerdetails[k].passengertypecode == "INFT" || passengerdetails[k].passengertypecode == "INF")
                                infantcount++;

                        }

                        AddInFantModel _PassengersModel1 = new AddInFantModel();
                        for (int i = 0; i < passengerdetails.Count; i++)
                        {
                            if (passengerdetails[i].passengertypecode == "ADT" || passengerdetails[i].passengertypecode == "CHD")
                                continue;
                            if (passengerdetails[i].passengertypecode == "INFT" || passengerdetails[i].passengertypecode == "INF")
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
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v3/booking/passengers/" + passengerdetails[k].passengerkey + "/infant", _PassengersModel1);
                                    if (responsePassengers.IsSuccessStatusCode)
                                    {
                                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                                        Logs logs = new Logs();
                                        //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_PassengersModel1) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v3/booking/passengers/" + passengerdetails[k].passengerkey + "/infant" + "\n Response: " + JsonConvert.SerializeObject(_responsePassengers), "Update passenger_Infant", "AkasaAirRT");

                                        var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
                                    }
                                    i++;
                                }

                                // STRAT Get INFO
                                // var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v1/booking");
                                if (responceGetBooking.IsSuccessStatusCode)
                                {
                                    var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                                    Logs logs = new Logs();
                                    //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetBookingRequest") + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v1/booking" + "\n Response: " + JsonConvert.SerializeObject(_responceGetBooking), "GetBooking", "AkasaAirRT");

                                    var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                                }
                                //END
                            }
                        }


                        //SpiceJet Passenger DEtails Round Trip Request API
                        HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                    }

                    // Spice Jet **********
                    string Signature = string.Empty;
                    if (i1 == 0)
                    {
                        Signature = HttpContext.Session.GetString("SpicejetSignature");
                    }
                    else
                    {
                        Signature = HttpContext.Session.GetString("SpicejetSignatureR");
                    }
                    if (!string.IsNullOrEmpty(Signature) && dataArray[i1].ToLower() == "spicejet")
                    {
                        if (Signature == null) { Signature = ""; }
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
                                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(updatePaxResp), "UpdatePassenger", "SpiceJetRT");

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                    }

                    if (i1 == 0)
                    {
                        Signature = HttpContext.Session.GetString("IndigoSignature");

                    }
                    else
                    {
                        Signature = HttpContext.Session.GetString("IndigoSignatureR");
                    }
                    if (!string.IsNullOrEmpty(Signature) && dataArray[i1].ToLower() == "indigo")
                    {
                        if (Signature == null) { Signature = ""; }
                        Signature = Signature.Replace(@"""", string.Empty);
                        _updateContact obj = new _updateContact(httpContextAccessorInstance);
                        IndigoBookingManager_.UpdatePassengersResponse updatePaxResp = await obj.UpdatePassengers(Signature, passengerdetails);
                        string Str2 = JsonConvert.SerializeObject(updatePaxResp);

                        #region GetState
                        //_sell objsell = new _sell();
                        //IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS1 = await objsell.GetBookingFromState(Signature, "");

                        //string strdata = JsonConvert.SerializeObject(_GetBookingFromStateRS1);
                        #endregion
                    }
                    if (dataArray[i1].ToLower() == "vistara" || dataArray[i1].ToLower() == "airindia")
                    {
                        HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                    }
                    vm.SeatmaplistRT = new List<SeatMapResponceModel>();
                    vm.passeengerlistRT = new List<AirAsiaTripResponceModel>();
                    vm.MealslistRT = new List<SSRAvailabiltyResponceModel>();
                    string test = string.Empty;
                    string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
                    string passenger = HttpContext.Session.GetString("keypassenger");
                    string Passenegrtext = HttpContext.Session.GetString("Mainpassengervm");
                    string Seatmap = HttpContext.Session.GetString("Mainseatmapvm");
                    string Meals = HttpContext.Session.GetString("Mainmealvm");
                    string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");

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
                    }
                    if (!string.IsNullOrEmpty(passengerNamedetails))
                    {
                        List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                        vm.passengerNamedetails = passengerNamedetailsdata;
                    }
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainseatmapvm")))
                    {
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
                    }

                    Meals = string.Empty;
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
                    }
                    //HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
                    //return PartialView("_ServiceRequestsPartialView", vm);

                    //return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
                }
            }
            HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
            return PartialView("_ServiceRequestsPartialView", vm);
        }


        //Post Unit Key
        public async Task<IActionResult> PostUnitkey(List<string> unitKey, List<string> ssrKey, List<string> BaggageSSrkey, List<string> FastfarwardAddon, List<string> PPBGAddon)
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
            if (BaggageSSrkey.Count > 0 && BaggageSSrkey[0] == null)
            {
                BaggageSSrkey = new List<string>();
            }
            if (ssrKey.Count > 0 && ssrKey[0] == null)
            {
                ssrKey = new List<string>();
            }
            if (unitKey.Count > 0 && unitKey[0] == null)
            {
                unitKey = new List<string>();
            }
            if (FastfarwardAddon.Count > 0 && FastfarwardAddon[0] == null)
            {
                FastfarwardAddon = new List<string>();
            }
            if (PPBGAddon.Count > 0 && PPBGAddon[0] == null)
            {
                PPBGAddon = new List<string>();
            }
            //List<string> ConnetedBaggageSSrkey = new List<string>();
            //for (int i = 0; i < BaggageSSrkey.Count; i++)
            //{
            //    ConnetedBaggageSSrkey.Add(BaggageSSrkey[i].Replace("_OneWay0", "_OneWay1").Replace("_RT0", "_RT1"));
            //}
            ////ConnetedBaggageSSrkey = BaggageSSrkey;
            //BaggageSSrkey.AddRange(ConnetedBaggageSSrkey);
            //#region AirAsia

            //Seat

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
            //var data = Seatmaplist.datalist.Count;
            //string legkey = passeengerKeyList.journeys[0].segments[0].legs[0].legKey;
            int Seatcount = unitKey.Count;
            #region RoundTripSSR

            Logs logs1 = new Logs();
            if (ssrKey.Count > 0 || BaggageSSrkey.Count > 0)
            {

                try
                {
                    var ssrKey_1 = ssrKey;// selectedIds;
                    string[] ssrKey2 = null;
                    string[] ssrsubKey2 = null;
                    string pas_ssrKey = string.Empty;

                    int journeyscount = 0;// passeengerKeyList.journeys.Count;
                    int mealid = 0;
                    int bagid = 0;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainpassengervm")))
                    {
                        passenger = HttpContext.Session.GetString("Mainpassengervm");
                        int _a = 0;
                        foreach (Match item in Regex.Matches(passenger, @"<Start>(?<test>[\s\S]*?)<End>"))
                        {
                            passenger = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"").Replace("\\\\", "");
                            if (passenger != null)
                            {
                                passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                                if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "spicejet")
                                {
                                    string tokenview = string.Empty;//spelling 
                                    if (_a == 0)
                                    {
                                        tokenview = HttpContext.Session.GetString("SpicejetSignature");//spelling 
                                    }
                                    else
                                    {
                                        tokenview = HttpContext.Session.GetString("SpicejetSignatureR");//spelling 
                                    }
                                    if (tokenview == null) { tokenview = ""; }
                                    token = tokenview.Replace(@"""", string.Empty);
                                    passengerscount = passeengerKeyList.passengerscount;
                                    Logs logs = new Logs();
                                    using (HttpClient client = new HttpClient())
                                    {
                                        if (ssrKey.Count >= 0)
                                        {
                                            #region SellSSr
                                            for (int i = 0; i < passeengerKeyList.passengers.Count; i++)
                                            {
                                                if (passeengerKeyList.passengers[i].passengerTypeCode == "INFT")
                                                    continue;
                                                if (_a == 0)
                                                {
                                                    FastfarwardAddon.Add("FFWD_OneWay0");
                                                }
                                                else
                                                {
                                                    FastfarwardAddon.Add("FFWD__RT0");
                                                }

                                            }
                                            FastfarwardAddon = new List<string>();
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
                                                    PaxNum.Infant_ = new List<passkeytype>();
                                                    bool infant = false;
                                                    ssrsegmentwise _obj = new ssrsegmentwise();
                                                    _obj.SSRcodeOneWayI = new List<ssrsKey>();
                                                    _obj.SSRcodeOneWayII = new List<ssrsKey>();
                                                    _obj.SSRcodeRTI = new List<ssrsKey>();
                                                    _obj.SSRcodeRTII = new List<ssrsKey>();
                                                    _obj.SSRbaggagecodeOneWayI = new List<ssrsKey>();
                                                    _obj.SSRbaggagecodeOneWayII = new List<ssrsKey>();
                                                    _obj.SSRbaggagecodeRTI = new List<ssrsKey>();
                                                    _obj.SSRbaggagecodeRTII = new List<ssrsKey>();
                                                    _obj.SSRffwcodeRTI = new List<ssrsKey>();
                                                    _obj.PPBGcodeRTI = new List<ssrsKey>();
                                                    _obj.SSRffwOneWayI = new List<ssrsKey>();
                                                    _obj.PPBGOneWayI = new List<ssrsKey>();
                                                    for (int k = 0; k < ssrKey.Count; k++)
                                                    {
                                                        if (ssrKey[k] == null)
                                                        {
                                                            continue;
                                                        }
                                                        if (ssrKey[k].ToLower().Contains("airasia"))
                                                            continue;

                                                        if (ssrKey[k].Contains("_OneWay0"))
                                                        {
                                                            string[] wordsArray = ssrKey[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                ssrsKey _obj0 = new ssrsKey();
                                                                _obj0.key = ssrKey[k];
                                                                _obj.SSRcodeOneWayI.Add(_obj0);
                                                            }

                                                        }
                                                        else if (ssrKey[k].Contains("_OneWay1"))
                                                        {
                                                            string[] wordsArray = ssrKey[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                ssrsKey _obj1 = new ssrsKey();
                                                                _obj1.key = ssrKey[k];
                                                                _obj.SSRcodeOneWayII.Add(_obj1);
                                                            }
                                                        }
                                                        else if (ssrKey[k].Contains("_RT0"))
                                                        {
                                                            string[] wordsArray = ssrKey[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                ssrsKey _obj2 = new ssrsKey();
                                                                _obj2.key = ssrKey[k];
                                                                _obj.SSRcodeRTI.Add(_obj2);
                                                            }
                                                        }
                                                        else if (ssrKey[k].Contains("_RT1"))
                                                        {
                                                            string[] wordsArray = ssrKey[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                ssrsKey _obj3 = new ssrsKey();
                                                                _obj3.key = ssrKey[k];
                                                                _obj.SSRcodeRTII.Add(_obj3);
                                                            }
                                                        }

                                                    }
                                                    if (BaggageSSrkey.Count > 0)
                                                    {
                                                        for (int k = 0; k < BaggageSSrkey.Count; k++)
                                                        {
                                                            string[] sskeydata = new string[2];
                                                            if (BaggageSSrkey[k].Contains("_OneWay0"))
                                                            {
                                                                //split
                                                                string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                                                if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                                {
                                                                    sskeydata = BaggageSSrkey[k].Split("_");
                                                                    ssrsKey _objBag0 = new ssrsKey();
                                                                    _objBag0.key = sskeydata[0];
                                                                    _obj.SSRbaggagecodeOneWayI.Add(_objBag0);
                                                                }
                                                            }
                                                            else if (BaggageSSrkey[k].Contains("_OneWay1"))
                                                            {
                                                                string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                                                if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                                {
                                                                    sskeydata = BaggageSSrkey[k].Split("_");
                                                                    ssrsKey _objBag1 = new ssrsKey();
                                                                    _objBag1.key = sskeydata[0];
                                                                    _obj.SSRbaggagecodeOneWayII.Add(_objBag1);
                                                                }
                                                            }
                                                            else if (BaggageSSrkey[k].Contains("_RT0"))
                                                            {
                                                                string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                                                if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                                {
                                                                    sskeydata = BaggageSSrkey[k].Split("_");
                                                                    ssrsKey _objBagRT1 = new ssrsKey();
                                                                    _objBagRT1.key = sskeydata[0];
                                                                    _obj.SSRbaggagecodeRTI.Add(_objBagRT1);
                                                                }
                                                            }
                                                            else if (BaggageSSrkey[k].Contains("_RT1"))
                                                            {
                                                                string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                                                if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                                {
                                                                    sskeydata = BaggageSSrkey[k].Split("_");
                                                                    ssrsKey _objBagRT2 = new ssrsKey();
                                                                    _objBagRT2.key = sskeydata[0];
                                                                    _obj.SSRbaggagecodeRTII.Add(_objBagRT2);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    for (int k = 0; k < FastfarwardAddon.Count; k++)
                                                    {
                                                        string[] sskeydata = new string[2];
                                                        if (FastfarwardAddon[k].Contains("_OneWay0"))
                                                        {
                                                            //split
                                                            string[] wordsArray = FastfarwardAddon[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                sskeydata = FastfarwardAddon[k].Split("_");
                                                                ssrsKey _objffw0 = new ssrsKey();
                                                                _objffw0.key = sskeydata[0];
                                                                _obj.SSRffwOneWayI.Add(_objffw0);
                                                            }
                                                        }
                                                        else if (FastfarwardAddon[k].Contains("_RT0"))
                                                        {
                                                            string[] wordsArray = FastfarwardAddon[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                sskeydata = FastfarwardAddon[k].Split("_");
                                                                ssrsKey _objffwRT1 = new ssrsKey();
                                                                _objffwRT1.key = sskeydata[0];
                                                                _obj.SSRffwcodeRTI.Add(_objffwRT1);
                                                            }
                                                        }

                                                    }
                                                    //Priority Boarding
                                                    for (int k = 0; k < PPBGAddon.Count; k++)
                                                    {
                                                        string[] sskeydata = new string[2];
                                                        if (PPBGAddon[k].Contains("_OneWay0"))
                                                        {
                                                            //split
                                                            string[] wordsArray = PPBGAddon[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                sskeydata = PPBGAddon[k].Split("_");
                                                                ssrsKey _objppbg0 = new ssrsKey();
                                                                _objppbg0.key = sskeydata[0];
                                                                _obj.PPBGOneWayI.Add(_objppbg0);
                                                            }
                                                        }
                                                        else if (PPBGAddon[k].Contains("_RT0"))
                                                        {
                                                            string[] wordsArray = PPBGAddon[k].ToString().Split('_');
                                                            if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                                            {
                                                                sskeydata = PPBGAddon[k].Split("_");
                                                                ssrsKey _objppbg1 = new ssrsKey();
                                                                _objppbg1.key = sskeydata[0];
                                                                _obj.PPBGcodeRTI.Add(_objppbg1);
                                                            }
                                                        }

                                                    }
                                                    if (j == 0 && _a == 0)
                                                    {
                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeOneWayI.Count + _obj.SSRbaggagecodeOneWayI.Count + _obj.SSRffwOneWayI.Count + _obj.PPBGOneWayI.Count];
                                                    }
                                                    else if (j == 1 && _a == 0)
                                                    {
                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeOneWayII.Count + _obj.SSRbaggagecodeOneWayII.Count];
                                                    }
                                                    else if (j == 0 && _a == 1)
                                                    {
                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeRTI.Count + _obj.SSRbaggagecodeRTI.Count + _obj.SSRffwcodeRTI.Count + _obj.PPBGcodeRTI.Count];
                                                    }
                                                    else if (j == 1 && _a == 1)
                                                    {
                                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeRTII.Count + _obj.SSRbaggagecodeRTII.Count];
                                                    }


                                                    int TotalPaxcount = PaxNum.Adults_.Count + PaxNum.Childs_.Count;

                                                    if (j == 0 && _a == 0)
                                                    {
                                                        //int k = 0;

                                                        if (TotalPaxcount > 0)
                                                        {
                                                            for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeOneWayI.Count + _obj.SSRbaggagecodeOneWayI.Count + _obj.SSRffwOneWayI.Count + _obj.PPBGOneWayI.Count; j1++)
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
                                                                    if (_obj.SSRcodeOneWayI.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                    {
                                                                        for (int i2 = 0; i2 < _obj.SSRcodeOneWayI.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                        {
                                                                            string[] wordsArray = _obj.SSRcodeOneWayI[i2].key.ToString().Split('_');
                                                                            //alert(wordsArray);
                                                                            //var meal = null;
                                                                            string ssrCodeKey = "";
                                                                            if (wordsArray.Length > 1)
                                                                            {
                                                                                ssrCodeKey = wordsArray[0];
                                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                            }
                                                                            else
                                                                                ssrCodeKey = _obj.SSRcodeOneWayI[i2].key.ToString();
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
                                                                        if (_obj.SSRbaggagecodeOneWayI.Count > 0)
                                                                        {
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.SSRbaggagecodeOneWayI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {
                                                                                    idx++;
                                                                                    string[] wordsArray = _obj.SSRbaggagecodeOneWayI[k].key.ToString().Split('_');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.SSRbaggagecodeOneWayI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;

                                                                                }
                                                                            }
                                                                        }
                                                                        if (_obj.SSRffwOneWayI.Count > 0)
                                                                        {
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.SSRffwOneWayI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {
                                                                                    idx++;
                                                                                    string[] wordsArray = _obj.SSRffwOneWayI[k].key.ToString().Split(' ');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.SSRffwOneWayI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;

                                                                                }
                                                                            }
                                                                        }
                                                                        if (_obj.PPBGOneWayI.Count > 0)
                                                                        {
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.PPBGOneWayI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {
                                                                                    idx++;
                                                                                    string[] wordsArray = _obj.PPBGOneWayI[k].key.ToString().Split(' ');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.PPBGOneWayI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;

                                                                                }
                                                                            }
                                                                        }

                                                                    }
                                                                    j1 = idx;
                                                                }
                                                            }

                                                        }


                                                    }
                                                    else if (j == 1 && _a == 0)
                                                    {
                                                        if (TotalPaxcount > 0)
                                                        {
                                                            for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeOneWayII.Count + _obj.SSRbaggagecodeOneWayII.Count; j1++)
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
                                                                    if (_obj.SSRcodeOneWayII.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                    {
                                                                        for (int i2 = 0; i2 < _obj.SSRcodeOneWayII.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                        {
                                                                            string[] wordsArray = _obj.SSRcodeOneWayII[i2].key.ToString().Split('_');
                                                                            //alert(wordsArray);
                                                                            //var meal = null;
                                                                            string ssrCodeKey = "";
                                                                            if (wordsArray.Length > 1)
                                                                            {
                                                                                ssrCodeKey = wordsArray[0];
                                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                            }
                                                                            else
                                                                                ssrCodeKey = _obj.SSRcodeOneWayII[i2].key.ToString();
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
                                                                    if (_obj.SSRbaggagecodeOneWayII.Count > 0)
                                                                    {
                                                                        if (idx > 0)
                                                                            idx++;
                                                                        for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                        {
                                                                            int baggagecount = _obj.SSRbaggagecodeOneWayII.Count;
                                                                            if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                            {

                                                                                string[] wordsArray = _obj.SSRbaggagecodeOneWayII[k].key.ToString().Split('_');
                                                                                //alert(wordsArray);
                                                                                //var meal = null;
                                                                                string ssrCodeKey = "";
                                                                                if (wordsArray.Length > 1)
                                                                                {
                                                                                    ssrCodeKey = wordsArray[0];
                                                                                    ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                }
                                                                                else
                                                                                    ssrCodeKey = _obj.SSRbaggagecodeOneWayII[k].key.ToString();
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                                idx++;
                                                                            }
                                                                        }
                                                                    }
                                                                    j1 = idx;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    else if (j == 0 && _a == 1)
                                                    {
                                                        //int k = 0;

                                                        if (TotalPaxcount > 0)
                                                        {
                                                            for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeRTI.Count + _obj.SSRbaggagecodeRTI.Count + _obj.SSRffwcodeRTI.Count + _obj.PPBGcodeRTI.Count; j1++)
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
                                                                    if (_obj.SSRcodeRTI.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                    {
                                                                        for (int i2 = 0; i2 < _obj.SSRcodeRTI.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                        {
                                                                            string[] wordsArray = _obj.SSRcodeRTI[i2].key.ToString().Split('_');
                                                                            //alert(wordsArray);
                                                                            //var meal = null;
                                                                            string ssrCodeKey = "";
                                                                            if (wordsArray.Length > 1)
                                                                            {
                                                                                ssrCodeKey = wordsArray[0];
                                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                            }
                                                                            else
                                                                                ssrCodeKey = _obj.SSRcodeRTI[i2].key.ToString();
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
                                                                        if (_obj.SSRbaggagecodeRTI.Count > 0)
                                                                        {
                                                                            idx++;
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.SSRbaggagecodeRTI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {

                                                                                    string[] wordsArray = _obj.SSRbaggagecodeRTI[k].key.ToString().Split('_');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.SSRbaggagecodeRTI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                                    idx++;
                                                                                }
                                                                            }
                                                                        }
                                                                        //
                                                                        if (_obj.SSRffwcodeRTI.Count > 0)
                                                                        {
                                                                            idx++;
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.SSRffwcodeRTI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {

                                                                                    string[] wordsArray = _obj.SSRffwcodeRTI[k].key.ToString().Split(' ');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.SSRffwcodeRTI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                                    idx++;
                                                                                }
                                                                            }
                                                                        }
                                                                        if (_obj.PPBGcodeRTI.Count > 0)
                                                                        {
                                                                            idx++;
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.PPBGcodeRTI.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {

                                                                                    string[] wordsArray = _obj.PPBGcodeRTI[k].key.ToString().Split(' ');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.PPBGcodeRTI[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                                    idx++;
                                                                                }
                                                                            }
                                                                        }



                                                                    }
                                                                    j1 = idx;
                                                                }
                                                            }

                                                        }


                                                    }
                                                    else if (j == 1 && _a == 1)
                                                    {
                                                        if (TotalPaxcount > 0)
                                                        {
                                                            for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeRTII.Count + _obj.SSRbaggagecodeRTII.Count; j1++)
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
                                                                    if (_obj.SSRcodeRTII.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                                                    {
                                                                        for (int i2 = 0; i2 < _obj.SSRcodeRTII.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                        {
                                                                            string[] wordsArray = _obj.SSRcodeRTII[i2].key.ToString().Split('_');
                                                                            //alert(wordsArray);
                                                                            //var meal = null;
                                                                            string ssrCodeKey = "";
                                                                            if (wordsArray.Length > 1)
                                                                            {
                                                                                ssrCodeKey = wordsArray[0];
                                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                            }
                                                                            else
                                                                                ssrCodeKey = _obj.SSRcodeRTII[i2].key.ToString();
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
                                                                        if (_obj.SSRbaggagecodeRTII.Count > 0)
                                                                        {
                                                                            idx++;
                                                                            for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                                            {
                                                                                int baggagecount = _obj.SSRbaggagecodeRTII.Count;
                                                                                if (baggagecount > 0 && k + 1 <= baggagecount)
                                                                                {

                                                                                    string[] wordsArray = _obj.SSRbaggagecodeRTII[k].key.ToString().Split('_');
                                                                                    //alert(wordsArray);
                                                                                    //var meal = null;
                                                                                    string ssrCodeKey = "";
                                                                                    if (wordsArray.Length > 1)
                                                                                    {
                                                                                        ssrCodeKey = wordsArray[0];
                                                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                                                    }
                                                                                    else
                                                                                        ssrCodeKey = _obj.SSRbaggagecodeRTII[k].key.ToString();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(k);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                                                    idx++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    j1 = idx;
                                                                }
                                                            }

                                                        }
                                                    }

                                                    //to do 
                                                    //sellSsrRequest.SellRequestData = sellreqd;
                                                    //SellResponse sellSsrResponse = null;
                                                    //sellreqd.SellSSR.SSRRequest.SellSSRMode = SellSSRMode.NonBundle;
                                                    //sellreqd.SellSSR.SSRRequest.SellSSRModeSpecified = true;
                                                    //SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                                                    //sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                                                    //string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                                                }
                                            }

                                            sellSsrRequest.SellRequestData = sellreqd;
                                            SellResponse sellSsrResponse = null;
                                            sellreqd.SellSSR.SSRRequest.SellSSRMode = SellSSRMode.NonBundle;
                                            sellreqd.SellSSR.SSRRequest.SellSSRModeSpecified = true;
                                            SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                                            sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                                            string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                                            logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR", "SpiceJetRT");


                                            if (sellSsrResponse != null)
                                            {
                                                var JsonObjSeatAssignment = sellSsrResponse;
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "airasia")
                                {
                                    mealid = 0;

                                    string tokenview = string.Empty;
                                    if (string.IsNullOrEmpty(tokenview))
                                    {
                                        if (_a == 0)
                                        {
                                            tokenview = HttpContext.Session.GetString("AirasiaTokan");//spelling 
                                        }
                                        else
                                        {
                                            tokenview = HttpContext.Session.GetString("AirasiaTokanR");//spelling 
                                        }
                                        token = tokenview.Replace(@"""", string.Empty);
                                        if (token == "" || token == null)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                    for (int l1 = 0; l1 < ssrKey.Count; l1++)
                                    {
                                        int l = 0;
                                        int m = 0;
                                        int idx = 0;
                                        int paxnum = 0;
                                        if (mealid < ssrKey.Count)
                                        {
                                            if (ssrKey[mealid] == null)
                                            {
                                                continue;
                                            }
                                            if (ssrKey[mealid].ToLower().Contains("airasia") && _a == 0 && (ssrKey[mealid].ToLower().Contains("oneway0") || ssrKey[mealid].ToLower().Contains("oneway1")))
                                            {
                                                if (ssrKey[mealid].Length > 1)
                                                {
                                                    ssrsubKey2 = ssrKey[mealid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
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
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {

                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AirAsiaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else if (ssrKey[mealid].ToLower().Contains("airasia") && _a == 1 && (ssrKey[mealid].ToLower().Contains("rt0") || ssrKey[mealid].ToLower().Contains("rt1")))
                                            {
                                                if (ssrKey[mealid].Length > 1)
                                                {
                                                    ssrsubKey2 = ssrKey[mealid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
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
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {
                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AirAsiaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                mealid++;
                                                continue;
                                            }
                                            mealid++;
                                        }
                                        //continue;

                                    }
                                    for (int b = 0; b < BaggageSSrkey.Count; b++)
                                    {
                                        int l = 0;
                                        int m = 0;
                                        int idx = 0;
                                        int paxnum = 0;
                                        if (bagid < BaggageSSrkey.Count)
                                        {
                                            if (BaggageSSrkey[bagid] == null)
                                            {
                                                continue;
                                            }
                                            if (BaggageSSrkey[bagid].ToLower().Contains("airasia") && _a == 0 && (BaggageSSrkey[bagid].ToLower().Contains("oneway0") || BaggageSSrkey[bagid].ToLower().Contains("oneway1")))
                                            {
                                                if (BaggageSSrkey[bagid].Length > 1)
                                                {
                                                    ssrsubKey2 = BaggageSSrkey[bagid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string bagskey = pas_ssrKey;
                                                bagskey = bagskey.Replace(@"""", string.Empty);
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
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + bagskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {

                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + bagskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AirAsiaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else if (BaggageSSrkey[bagid].ToLower().Contains("airasia") && _a == 1 && (BaggageSSrkey[bagid].ToLower().Contains("rt0") || BaggageSSrkey[bagid].ToLower().Contains("rt1")))
                                            {
                                                if (BaggageSSrkey[bagid].Length > 1)
                                                {
                                                    ssrsubKey2 = BaggageSSrkey[bagid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string bagskeyCon = pas_ssrKey;
                                                bagskeyCon = bagskeyCon.Replace(@"""", string.Empty);
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
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + bagskeyCon, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {
                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + bagskeyCon + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AirAsiaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bagid++;
                                                continue;
                                            }
                                            bagid++;
                                        }
                                        //continue;

                                    }


                                }

                                //Vinay Akasa SellSSR//

                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "akasaair")
                                {
                                    mealid = 0;
                                    string tokenview = string.Empty;
                                    if (string.IsNullOrEmpty(tokenview))
                                    {
                                        if (_a == 0)
                                        {
                                            tokenview = HttpContext.Session.GetString("AkasaTokan");//spelling 
                                        }
                                        else
                                        {
                                            tokenview = HttpContext.Session.GetString("AkasaTokanR");//spelling 
                                        }
                                        if (tokenview == null) { tokenview = ""; }
                                        token = tokenview.Replace(@"""", string.Empty);
                                        if (token == "" || token == null)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                    for (int l1 = 0; l1 < ssrKey.Count; l1++)
                                    {
                                        int l = 0;
                                        int m = 0;
                                        int idx = 0;
                                        int paxnum = 0;

                                        if (mealid < ssrKey.Count)
                                        {
                                            if (ssrKey[mealid] == null)
                                            {
                                                continue;
                                            }
                                            if (ssrKey[mealid].ToLower().Contains("akasaair") && _a == 0 && (ssrKey[mealid].ToLower().Contains("oneway0") || ssrKey[mealid].ToLower().Contains("oneway1")))
                                            {
                                                if (ssrKey[mealid].Length > 1)
                                                {
                                                    ssrsubKey2 = ssrKey[mealid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string mealskey = pas_ssrKey;
                                                mealskey = mealskey.Replace(@"""", string.Empty);
                                                if (!string.IsNullOrEmpty(token))
                                                {
                                                    using (HttpClient client = new HttpClient())
                                                    {
                                                        SellSSRModel _sellSSRModel = new SellSSRModel();
                                                        _sellSSRModel.count = 1;
                                                        _sellSSRModel.note = "PYOG";
                                                        _sellSSRModel.forceWaveOnSell = false;
                                                        _sellSSRModel.currencyCode = "INR";
                                                        _sellSSRModel.ssrSellMode = 2;
                                                        var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {

                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + mealskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AkasaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }

                                            else if (ssrKey[mealid].ToLower().Contains("akasaair") && _a == 1 && (ssrKey[mealid].ToLower().Contains("rt0") || ssrKey[mealid].ToLower().Contains("rt1")))
                                            {
                                                if (ssrKey[mealid].Length > 1)
                                                {
                                                    ssrsubKey2 = ssrKey[mealid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string mealskey = pas_ssrKey;
                                                mealskey = mealskey.Replace(@"""", string.Empty);
                                                if (!string.IsNullOrEmpty(token))
                                                {
                                                    using (HttpClient client = new HttpClient())
                                                    {
                                                        SellSSRModel _sellSSRModel = new SellSSRModel();
                                                        _sellSSRModel.count = 1;
                                                        _sellSSRModel.note = "PYOG";
                                                        _sellSSRModel.forceWaveOnSell = false;
                                                        _sellSSRModel.currencyCode = "INR";
                                                        _sellSSRModel.ssrSellMode = 2;
                                                        var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {
                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + mealskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AkasaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                mealid++;
                                                continue;
                                            }
                                            mealid++;
                                        }
                                        //continue;

                                    }
                                    //SellBaggage
                                    for (int b = 0; b < BaggageSSrkey.Count; b++)
                                    {
                                        int l = 0;
                                        int m = 0;
                                        int idx = 0;
                                        int paxnum = 0;
                                        if (bagid < BaggageSSrkey.Count)
                                        {
                                            if (BaggageSSrkey[bagid] == null)
                                            {
                                                continue;
                                            }
                                            if (BaggageSSrkey[bagid].ToLower().Contains("akasaair") && _a == 0 && (BaggageSSrkey[bagid].ToLower().Contains("oneway0") || BaggageSSrkey[bagid].ToLower().Contains("oneway1")))
                                            {
                                                if (BaggageSSrkey[bagid].Length > 1)
                                                {
                                                    ssrsubKey2 = BaggageSSrkey[bagid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string bagskey = pas_ssrKey;
                                                bagskey = bagskey.Replace(@"""", string.Empty);
                                                if (!string.IsNullOrEmpty(token))
                                                {
                                                    using (HttpClient client = new HttpClient())
                                                    {
                                                        SellSSRModel _sellSSRModel = new SellSSRModel();
                                                        _sellSSRModel.count = 1;
                                                        _sellSSRModel.note = "PYOG";
                                                        _sellSSRModel.forceWaveOnSell = false;
                                                        _sellSSRModel.currencyCode = "INR";
                                                        _sellSSRModel.ssrSellMode = 2;
                                                        var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + bagskey, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {

                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + bagskey + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AkasaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else if (BaggageSSrkey[bagid].ToLower().Contains("akasaair") && _a == 1 && (BaggageSSrkey[bagid].ToLower().Contains("rt0") || BaggageSSrkey[bagid].ToLower().Contains("rt1")))
                                            {
                                                if (BaggageSSrkey[bagid].Length > 1)
                                                {
                                                    ssrsubKey2 = BaggageSSrkey[bagid].Split('_');
                                                    pas_ssrKey = ssrsubKey2[0].Trim();
                                                }
                                                string bagskeyCon = pas_ssrKey;
                                                bagskeyCon = bagskeyCon.Replace(@"""", string.Empty);
                                                if (!string.IsNullOrEmpty(token))
                                                {
                                                    using (HttpClient client = new HttpClient())
                                                    {
                                                        SellSSRModel _sellSSRModel = new SellSSRModel();
                                                        _sellSSRModel.count = 1;
                                                        _sellSSRModel.note = "PYOG";
                                                        _sellSSRModel.forceWaveOnSell = false;
                                                        _sellSSRModel.currencyCode = "INR";
                                                        _sellSSRModel.ssrSellMode = 2;
                                                        var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                        HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + bagskeyCon, _sellSSRModel);
                                                        if (responseSellSSR.IsSuccessStatusCode)
                                                        {
                                                            var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                                                            logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_sellSSRModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/ssrs/" + bagskeyCon + "\n Response: " + JsonConvert.SerializeObject(_responseresponseSellSSR), "SellSSR", "AkasaRT");
                                                            var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bagid++;
                                                continue;
                                            }
                                            bagid++;
                                        }
                                        //continue;

                                    }

                                }





                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "indigo")
                                {
                                    bool Boolfastforward = false;
                                    string tokenview = string.Empty;
                                    if (_a == 0)
                                    {
                                        tokenview = HttpContext.Session.GetString("IndigoSignature");//spelling 
                                    }
                                    else
                                    {
                                        tokenview = HttpContext.Session.GetString("IndigoSignatureR");//spelling 
                                    }
                                    if (tokenview == null) { tokenview = ""; }
                                    token = tokenview.Replace(@"""", string.Empty);
                                    _SellSSR obj_ = new _SellSSR(httpContextAccessorInstance);
                                    //List<string> BaggageSSrkey = new List<string>();
                                    //VinayFast
                                    IndigoBookingManager_.SellResponse sellSsrResponse = await obj_.sellssr(token, passeengerKeyList, ssrKey, BaggageSSrkey, FastfarwardAddon, PPBGAddon, Boolfastforward, _a);
                                }
                                _a++;
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
            //if (unitKey.Count > 0)
            //{
            //try
            //{

            //var unitKey_1 = unitKey;// selectedIds;
            //string[] unitKey2 = null;
            //string[] unitsubKey2 = null;
            //string pas_unitKey = string.Empty;

            //int journeyscount = 0;// passeengerKeyList.journeys.Count;
            //int seatid = 0;
            //for (int i1 = 0; i1 < passeengerKeyListRT.Count; i1++)
            //{
            //if (i1 == 0)
            //{
            //journeyscount = passeengerKeyListRT[0].journeys.Count;

            //for (int i = 0; i < journeyscount; i++)
            //{
            //    int segmentscount = passeengerKeyListRT[0].journeys[i].segments.Count;
            //    for (int k1 = 0; k1 < segmentscount; k1++)
            //    {
            //        for (int l1 = 0; l1 < passeengerKeyListRT[0].passengerscount; l1++)
            //        {
            //            int l = 0;
            //            int m = 0;
            //            int idx = 0;
            //            int paxnum = 0;
            //            if (passeengerKeyListRT[0].passengers[l1].passengerTypeCode == "INFT")
            //                continue;
            //            if (unitKey[seatid].ToLower().Contains("airasia"))
            //            {
            //                if (unitKey[seatid].Length > 1)
            //                {

            //                    unitsubKey2 = unitKey[seatid].Split('_');
            //                    pas_unitKey = unitsubKey2[1].Trim();
            //                    idx = int.Parse(unitsubKey2[3]);
            //                    if (idx == 0) paxnum = l++;
            //                    else
            //                        paxnum = m++;

            //                }

            //                string passengerkey = string.Empty;
            //                passengerkey = passeengerKeyListRT[0].passengers[l1].passengerKey;
            //                string unitkey = pas_unitKey;
            //                using (HttpClient client = new HttpClient())
            //                {
            //                    string journeyKey = passeengerKeyListRT[0].journeys[i].journeyKey;
            //                    SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
            //                    _SeatAssignmentModel.journeyKey = journeyKey;
            //                    var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
            //                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //                    HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
            //                    //HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaSeatSelect + unitkey, _SeatAssignmentModel);

            //                    if (responceSeatAssignment.IsSuccessStatusCode)
            //                    {
            //                        var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
            //                        var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
            //                    }

            //                }
            //            }
            //            else
            //            {
            //                continue;
            //            }
            //            //continue;
            //            seatid++;
            //        }
            //        //seatid++;
            //    }
            //}
            //}
            //else
            //{
            if (unitKey.Count > 0)
            {

                try
                {
                    int journeyscount = 0;
                    int keycount1 = 0;
                    int keycount0 = 0;
                    int seatid = 0;
                    string[] unitKey2 = null;
                    string[] unitsubKey2 = null;
                    string pas_unitKey = string.Empty;
                    for (int k = 0; k < unitKey.Count; k++)
                    {
                        if ((unitKey[k].ToLower().Contains("spicejet") || unitKey[k].ToLower().Contains("indigo")) && (unitKey[k].ToString().Contains("OneWay0") || unitKey[k].ToString().Contains("OneWay1")))
                            keycount0++;
                        if ((unitKey[k].ToLower().Contains("spicejet") || unitKey[k].ToLower().Contains("indigo")) && (unitKey[k].ToString().Contains("RT0") || unitKey[k].ToString().Contains("RT1")))
                            keycount1++;

                    }
                    Logs _logs = new Logs();
                    int _index = 0;
                    int p = 0;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Mainpassengervm")))
                    {
                        passenger = HttpContext.Session.GetString("Mainpassengervm");
                        foreach (Match item in Regex.Matches(passenger, @"<Start>(?<test>[\s\S]*?)<End>"))
                        {
                            passenger = item.Groups["test"].Value.ToString().Replace("/\"", "\"").Replace("\\\"", "\"").Replace("\\\\", "");
                            if (passenger != null)
                            {
                                passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                                if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "spicejet")
                                {
                                    seatid = 0;
                                    _index = 0;
                                    journeyscount = passeengerKeyList.journeys.Count;
                                    AssignSeatsResponse _AssignseatRes = new AssignSeatsResponse();
                                    AssignSeatsRequest _AssignSeatReq = new AssignSeatsRequest();
                                    string Signature = string.Empty;
                                    if (p == 0)
                                    {
                                        Signature = HttpContext.Session.GetString("SpicejetSignature");
                                    }
                                    else
                                    {
                                        Signature = HttpContext.Session.GetString("SpicejetSignatureR");
                                    }
                                    if (Signature == null) { Signature = ""; }
                                    Signature = Signature.Replace(@"""", string.Empty);
                                    if (!string.IsNullOrEmpty(Signature))
                                    {
                                        Signature = Signature.Replace(@"""", string.Empty);
                                        _AssignSeatReq.Signature = Signature;
                                        _AssignSeatReq.ContractVersion = 420;
                                        _AssignSeatReq.SellSeatRequest = new SeatSellRequest();
                                        _AssignSeatReq.SellSeatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                                        _AssignSeatReq.SellSeatRequest.SeatAssignmentModeSpecified = true;
                                        int keycount = 0;
                                        if (p == 0)
                                            keycount = keycount0;
                                        else
                                            keycount = keycount1;
                                        _AssignSeatReq.SellSeatRequest.SegmentSeatRequests = new SegmentSeatRequest[keycount];// [unitKey.Count];//to do

                                        for (int i2 = 0; i2 < journeyscount; i2++)
                                        {
                                            int l = 0;
                                            int m = 0;
                                            for (int k = 0; k < unitKey.Count; k++)
                                            {
                                                int idx = 0;
                                                int paxnum = 0;
                                                if (seatid < unitKey.Count)
                                                {
                                                    if (unitKey[seatid].Length > 1)
                                                    {
                                                        if ((unitKey[seatid].ToString().Contains("OneWay0") || unitKey[seatid].ToString().Contains("OneWay1")) && p == 0)
                                                        {
                                                            unitsubKey2 = unitKey[seatid].Split('_');
                                                            pas_unitKey = unitsubKey2[1];
                                                            idx = int.Parse(unitsubKey2[3]);
                                                            if (idx == 0)
                                                            {
                                                                paxnum = l++;
                                                            }
                                                            else
                                                            {
                                                                paxnum = m++;
                                                            }
                                                            //keycount++;

                                                        }
                                                        else if ((unitKey[seatid].ToString().Contains("RT0") || unitKey[seatid].ToString().Contains("RT1")) && p == 1)
                                                        {
                                                            unitsubKey2 = unitKey[seatid].Split('_');
                                                            pas_unitKey = unitsubKey2[1];
                                                            idx = int.Parse(unitsubKey2[3]);
                                                            if (idx == 0)
                                                            {
                                                                paxnum = l++;
                                                            }
                                                            else
                                                            {
                                                                paxnum = m++;
                                                            }
                                                            //keycount++;

                                                        }
                                                        else
                                                        {
                                                            seatid++;
                                                            continue;
                                                        }
                                                    }

                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index] = new SegmentSeatRequest();
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator = new FlightDesignator();
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator.CarrierCode = passeengerKeyList.journeys[i2].segments[idx].identifier.carrierCode;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator.FlightNumber = passeengerKeyList.journeys[i2].segments[idx].identifier.identifier;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].STD = passeengerKeyList.journeys[i2].segments[idx].designator.departure;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].STDSpecified = true;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].DepartureStation = passeengerKeyList.journeys[i2].segments[idx].designator.origin;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].ArrivalStation = passeengerKeyList.journeys[i2].segments[idx].designator.destination;
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].UnitDesignator = pas_unitKey.Trim();
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].PassengerNumbers = new short[1];
                                                    _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].PassengerNumbers[0] = Convert.ToInt16(paxnum);
                                                    seatid++;
                                                    _index++;
                                                }
                                            }

                                        }

                                        _AssignSeatReq.SellSeatRequest.IncludeSeatData = true;
                                        _AssignSeatReq.SellSeatRequest.IncludeSeatDataSpecified = true;

                                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                                        _AssignseatRes = await objSpiceJet.Assignseat(_AssignSeatReq);

                                        string Str2 = JsonConvert.SerializeObject(_AssignseatRes);

                                        _logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat", "SpiceJetRT");
                                    }
                                }

                                //*************AirAsia AssignSeat API*************
                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "airasia")
                                {
                                    //seatid = 0;
                                    string tokenview = string.Empty;
                                    if (p == 0)
                                    {
                                        tokenview = HttpContext.Session.GetString("AirasiaTokan");//spelling 
                                    }
                                    else
                                    {
                                        tokenview = HttpContext.Session.GetString("AirasiaTokanR");//spelling 
                                    }
                                    if (!string.IsNullOrEmpty(tokenview))
                                    {

                                        token = tokenview.Replace(@"""", string.Empty);
                                        if (token == "" || token == null)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                    journeyscount = passeengerKeyList.journeys.Count;
                                    //To do

                                    ssrsegmentwise _obj = new ssrsegmentwise();
                                    _obj.SSRcodeOneWayI = new List<ssrsKey>();
                                    _obj.SSRcodeOneWayII = new List<ssrsKey>();
                                    _obj.SSRcodeRTI = new List<ssrsKey>();
                                    _obj.SSRcodeRTII = new List<ssrsKey>();
                                    for (int k = 0; k < unitKey.Count; k++)
                                    {
                                        if (unitKey[k].ToLower().Contains("spicejet"))
                                            continue;

                                        if (unitKey[k].Contains("_OneWay0") && p == 0)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj0 = new ssrsKey();
                                            _obj0.key = pas_unitKey;
                                            _obj.SSRcodeOneWayI.Add(_obj0);
                                        }
                                        else if (unitKey[k].Contains("_OneWay1") && p == 0)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj1 = new ssrsKey();
                                            _obj1.key = pas_unitKey;
                                            _obj.SSRcodeOneWayII.Add(_obj1);
                                        }
                                        else if (unitKey[k].Contains("_RT0") && p == 1)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj2 = new ssrsKey();
                                            _obj2.key = pas_unitKey;
                                            _obj.SSRcodeRTI.Add(_obj2);
                                        }
                                        else if (unitKey[k].Contains("_RT1") && p == 1)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj3 = new ssrsKey();
                                            _obj3.key = pas_unitKey;
                                            _obj.SSRcodeRTII.Add(_obj3);
                                        }

                                    }




                                    for (int i = 0; i < journeyscount; i++)
                                    {
                                        int segmentscount = passeengerKeyList.journeys[i].segments.Count;

                                        for (int l2 = 0; l2 < _obj.SSRcodeOneWayI.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeOneWayI[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }

                                        for (int l2 = 0; l2 < _obj.SSRcodeOneWayII.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeOneWayII[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }

                                        for (int l2 = 0; l2 < _obj.SSRcodeRTI.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeRTI[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                        for (int l2 = 0; l2 < _obj.SSRcodeRTII.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeRTII[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                    }







                                    //for (int k1 = 0; k1 < segmentscount; k1++)
                                    //{
                                    //seatid = 0;
                                    //for (int l1 = 0; l1 < passeengerKeyList.passengerscount; l1++)
                                    //{
                                    int l = 0;
                                    //int m = 0;
                                    //int idx = 0;
                                    //int paxnum = 0;

                                    //for (int k = 0; k < unitKey.Count; k++)
                                    //{
                                    //for (int l1 = 0; l1 < passeengerKeyList.passengerscount; l1++)
                                    //{
                                    //if (passeengerKeyList.passengers[l1].passengerTypeCode == "INFT")
                                    // continue;
                                    //if (seatid < unitKey.Count)
                                    //{
                                    //    if ((unitKey[seatid].ToString().Contains("OneWay0") || unitKey[seatid].ToString().Contains("OneWay1")) && p == 0)
                                    //    {
                                    //        if (unitKey[seatid].Length > 1)
                                    //        {

                                    //            unitsubKey2 = unitKey[seatid].Split('_');
                                    //            pas_unitKey = unitsubKey2[1].Trim();
                                    //            idx = int.Parse(unitsubKey2[3]);
                                    //            if (idx == 0) paxnum = l++;
                                    //            else
                                    //                paxnum = m++;

                                    //        }

                                    //        string passengerkey = string.Empty;
                                    //        passengerkey = passeengerKeyList.passengers[l1].passengerKey;
                                    //        string unitkey = pas_unitKey;
                                    //        using (HttpClient client = new HttpClient())
                                    //        {
                                    //            string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                    //            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                    //            _SeatAssignmentModel.journeyKey = journeyKey;
                                    //            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                    //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    //            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                    //            if (responceSeatAssignment.IsSuccessStatusCode)
                                    //            {
                                    //                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    //                logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                    //                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                    //            }

                                    //        }
                                    //    }
                                    //    else if ((unitKey[seatid].ToString().Contains("RT0") || unitKey[seatid].ToString().Contains("RT1")) && p == 1)
                                    //    {
                                    //        if (unitKey[seatid].Length > 1)
                                    //        {

                                    //            unitsubKey2 = unitKey[seatid].Split('_');
                                    //            pas_unitKey = unitsubKey2[1].Trim();
                                    //            idx = int.Parse(unitsubKey2[3]);
                                    //            if (idx == 0) paxnum = l++;
                                    //            else
                                    //                paxnum = m++;

                                    //        }

                                    //        string passengerkey = string.Empty;
                                    //        passengerkey = passeengerKeyList.passengers[l1].passengerKey;
                                    //        string unitkey = pas_unitKey;
                                    //        using (HttpClient client = new HttpClient())
                                    //        {
                                    //            string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                    //            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                    //            _SeatAssignmentModel.journeyKey = journeyKey;
                                    //            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                    //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    //            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                    //            if (responceSeatAssignment.IsSuccessStatusCode)
                                    //            {
                                    //                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    //                logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                    //                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                    //            }

                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        seatid++;
                                    //        continue;
                                    //    }
                                    //}
                                    //seatid++;
                                    //}
                                    //}
                                    //}
                                    //continue;

                                    //}
                                    //}


                                }
                                //*************Akasa AssignSeat API************
                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "akasaair")
                                {
                                    string tokenview = string.Empty;
                                    if (p == 0)
                                    {
                                        tokenview = HttpContext.Session.GetString("AkasaTokan");//
                                    }
                                    else
                                    {
                                        tokenview = HttpContext.Session.GetString("AkasaTokanR");//
                                    }
                                    if (!string.IsNullOrEmpty(tokenview))
                                    {
                                        if (tokenview == null) { tokenview = ""; }
                                        token = tokenview.Replace(@"""", string.Empty);
                                        if (token == "" || token == null)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                    journeyscount = passeengerKeyList.journeys.Count;
                                    //To do

                                    ssrsegmentwise _obj = new ssrsegmentwise();
                                    _obj.SSRcodeOneWayI = new List<ssrsKey>();
                                    _obj.SSRcodeOneWayII = new List<ssrsKey>();
                                    _obj.SSRcodeRTI = new List<ssrsKey>();
                                    _obj.SSRcodeRTII = new List<ssrsKey>();
                                    for (int k = 0; k < unitKey.Count; k++)
                                    {    // use this line for reference code...
                                        if (unitKey[k].ToLower().Contains("spicejet") || unitKey[k].ToLower().Contains("airasia"))
                                            continue;

                                        if (unitKey[k].Contains("_OneWay0") && p == 0)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj0 = new ssrsKey();
                                            _obj0.key = pas_unitKey;
                                            _obj.SSRcodeOneWayI.Add(_obj0);
                                        }
                                        else if (unitKey[k].Contains("_OneWay1") && p == 0)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj1 = new ssrsKey();
                                            _obj1.key = pas_unitKey;
                                            _obj.SSRcodeOneWayII.Add(_obj1);
                                        }
                                        else if (unitKey[k].Contains("_RT0") && p == 1)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj2 = new ssrsKey();
                                            _obj2.key = pas_unitKey;
                                            _obj.SSRcodeRTI.Add(_obj2);
                                        }
                                        else if (unitKey[k].Contains("_RT1") && p == 1)
                                        {
                                            unitsubKey2 = unitKey[k].Split('_');
                                            pas_unitKey = unitsubKey2[1].Trim();
                                            ssrsKey _obj3 = new ssrsKey();
                                            _obj3.key = pas_unitKey;
                                            _obj.SSRcodeRTII.Add(_obj3);
                                        }

                                    }




                                    for (int i = 0; i < journeyscount; i++)
                                    {
                                        int segmentscount = passeengerKeyList.journeys[i].segments.Count;

                                        for (int l2 = 0; l2 < _obj.SSRcodeOneWayI.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeOneWayI[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                _SeatAssignmentModel.collectedCurrencyCode = "INR";
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    //logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AkasaAirRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }

                                        for (int l2 = 0; l2 < _obj.SSRcodeOneWayII.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeOneWayII[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    //logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AkasaAirRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                        // SSr CODE API For Infant AkasaAir
                                        for (int l2 = 0; l2 < _obj.SSRcodeRTI.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeRTI[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    //logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AkasaAirRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                        for (int l2 = 0; l2 < _obj.SSRcodeRTII.Count; l2++)
                                        {
                                            if (passeengerKeyList.passengers[l2].passengerTypeCode == "INFT")
                                                continue;
                                            string passengerkey = string.Empty;
                                            passengerkey = passeengerKeyList.passengers[l2].passengerKey;
                                            pas_unitKey = _obj.SSRcodeRTII[l2].key.Trim();
                                            using (HttpClient client = new HttpClient())
                                            {
                                                string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                                SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                                _SeatAssignmentModel.journeyKey = journeyKey;
                                                var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                                HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                                if (responceSeatAssignment.IsSuccessStatusCode)
                                                {
                                                    var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                                    //logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAkasaAir + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AkasaAirRT");
                                                    var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                                }

                                            }
                                        }
                                    }







                                    //for (int k1 = 0; k1 < segmentscount; k1++)
                                    //{
                                    //seatid = 0;
                                    //for (int l1 = 0; l1 < passeengerKeyList.passengerscount; l1++)
                                    //{
                                    int l = 0;
                                    //int m = 0;
                                    //int idx = 0;
                                    //int paxnum = 0;

                                    //for (int k = 0; k < unitKey.Count; k++)
                                    //{
                                    //for (int l1 = 0; l1 < passeengerKeyList.passengerscount; l1++)
                                    //{
                                    //if (passeengerKeyList.passengers[l1].passengerTypeCode == "INFT")
                                    // continue;
                                    //if (seatid < unitKey.Count)
                                    //{
                                    //    if ((unitKey[seatid].ToString().Contains("OneWay0") || unitKey[seatid].ToString().Contains("OneWay1")) && p == 0)
                                    //    {
                                    //        if (unitKey[seatid].Length > 1)
                                    //        {

                                    //            unitsubKey2 = unitKey[seatid].Split('_');
                                    //            pas_unitKey = unitsubKey2[1].Trim();
                                    //            idx = int.Parse(unitsubKey2[3]);
                                    //            if (idx == 0) paxnum = l++;
                                    //            else
                                    //                paxnum = m++;

                                    //        }

                                    //        string passengerkey = string.Empty;
                                    //        passengerkey = passeengerKeyList.passengers[l1].passengerKey;
                                    //        string unitkey = pas_unitKey;
                                    //        using (HttpClient client = new HttpClient())
                                    //        {
                                    //            string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                    //            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                    //            _SeatAssignmentModel.journeyKey = journeyKey;
                                    //            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                    //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    //            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                    //            if (responceSeatAssignment.IsSuccessStatusCode)
                                    //            {
                                    //                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    //                logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                    //                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                    //            }

                                    //        }
                                    //    }
                                    //    else if ((unitKey[seatid].ToString().Contains("RT0") || unitKey[seatid].ToString().Contains("RT1")) && p == 1)
                                    //    {
                                    //        if (unitKey[seatid].Length > 1)
                                    //        {

                                    //            unitsubKey2 = unitKey[seatid].Split('_');
                                    //            pas_unitKey = unitsubKey2[1].Trim();
                                    //            idx = int.Parse(unitsubKey2[3]);
                                    //            if (idx == 0) paxnum = l++;
                                    //            else
                                    //                paxnum = m++;

                                    //        }

                                    //        string passengerkey = string.Empty;
                                    //        passengerkey = passeengerKeyList.passengers[l1].passengerKey;
                                    //        string unitkey = pas_unitKey;
                                    //        using (HttpClient client = new HttpClient())
                                    //        {
                                    //            string journeyKey = passeengerKeyList.journeys[i].journeyKey;
                                    //            SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
                                    //            _SeatAssignmentModel.journeyKey = journeyKey;
                                    //            var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
                                    //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    //            HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
                                    //            if (responceSeatAssignment.IsSuccessStatusCode)
                                    //            {
                                    //                var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                    //                logs1.WriteLogsR("Request: " + JsonConvert.SerializeObject(_SeatAssignmentModel) + "Url: " + AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey + "\n Response: " + JsonConvert.SerializeObject(_responseSeatAssignment), "AssignSeat", "AirAsiaRT");
                                    //                var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
                                    //            }

                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        seatid++;
                                    //        continue;
                                    //    }
                                    //}
                                    //seatid++;
                                    //}
                                    //}
                                    //}
                                    //continue;

                                    //}
                                    //}


                                }

                                //*************Indigo AssignSeat API*************
                                else if (passeengerKeyList.journeys[0].Airlinename.ToLower() == "indigo")
                                {
                                    string Signature = string.Empty;
                                    seatid = 0;
                                    _index = 0;
                                    if (p == 0)
                                    {
                                        Signature = HttpContext.Session.GetString("IndigoSignature");
                                    }
                                    else
                                    {
                                        Signature = HttpContext.Session.GetString("IndigoSignatureR");
                                    }
                                    if (Signature == null) { Signature = ""; }
                                    Signature = Signature.Replace(@"""", string.Empty);
                                    _SellSSR obj_ = new _SellSSR(httpContextAccessorInstance);
                                    IndigoBookingManager_.AssignSeatsResponse _AssignseatRes = await obj_.AssignSeat(Signature, passeengerKeyList, unitKey, p, keycount0, keycount1);

                                }
                                p++;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                //}
                // }
                //}
            }
            //catch (Exception ex)
            //{

            //}
            //}

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