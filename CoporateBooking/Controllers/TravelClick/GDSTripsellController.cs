using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
//using OnionArchitectureAPI.Services.Indigo;
//using OnionArchitectureAPI.Services.Indigo;
using Utility;
using static DomainLayer.Model.ReturnTicketBooking;

namespace OnionConsumeWebAPI.Controllers.TravelClick
{
    public class GDSTripsellController : Controller
    {
        Logs logs = new Logs();
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        AirAsiaTripResponceModel passeengerlist = null;
        IHttpContextAccessor httpContextAccessorInstance = new HttpContextAccessor();
        public IActionResult GDSSaverTripsell()
        {

            List<SelectListItem> Title = new()
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Ms" ,Value = "Ms" },
                new SelectListItem { Text = "Mrs", Value = "Mrs"},

            };

            ViewBag.Title = Title;
            var AirCraftName = TempData["AirCraftName"];
            ViewData["name"] = AirCraftName;
            //spicejet
            string passenger = HttpContext.Session.GetString("SGkeypassenger"); //From Itenary Response
            string passengerInfant = HttpContext.Session.GetString("SGkeypassenger");
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");
            ViewModel vm = new ViewModel();
            if (passengerInfant != null)
            {
                AirAsiaTripResponceModel passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
                passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = null;// (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = null;// (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.passeengerlistItanary = passeengerlistItanary;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
            }
            else
            {
                passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
               // SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                //SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = null;// Seatmaplist;
                vm.Meals = null;// Mealslist;
            }
            if (!string.IsNullOrEmpty(passengerNamedetails))
            {
                List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                vm.passengerNamedetails = passengerNamedetailsdata;
            }
            return View(vm);

        }

        //Seat map meal Pip Up bind Code 
        public IActionResult PostSeatMapModaldataView()
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

            string passenger = HttpContext.Session.GetString("SGkeypassenger"); //From Itenary Response
            string passengerInfant = HttpContext.Session.GetString("SGkeypassenger");
            string Seatmap = HttpContext.Session.GetString("Seatmap");
            string Meals = HttpContext.Session.GetString("Meals");
            string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");
            ViewModel vm = new ViewModel();
            if (passengerInfant != null)
            {
                AirAsiaTripResponceModel passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
                passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                //SeatMapResponceModel Seatmaplist = new SeatMapResponceModel();
                //SSRAvailabiltyResponceModel Mealslist = new SSRAvailabiltyResponceModel();
                if (!string.IsNullOrEmpty(passengerNamedetails))
                {
                    List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                    vm.passengerNamedetails = passengerNamedetailsdata;
                }
                vm.passeengerlist = passeengerlist;
                vm.passeengerlistItanary = passeengerlistItanary;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
            }
            else
            {
                passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                //SeatMapResponceModel Seatmaplist = new SeatMapResponceModel();
                //SSRAvailabiltyResponceModel Mealslist = new SSRAvailabiltyResponceModel();
                if (!string.IsNullOrEmpty(passengerNamedetails))
                {
                    List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                    vm.passengerNamedetails = passengerNamedetailsdata;
                }
                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
            }
            return View(vm);
        }


        public async Task<IActionResult> GDSContactDetails(ContactModel contactobject)
        {

            //string Signature = HttpContext.Session.GetString("GDSSignature");
            //if (Signature == null) { Signature = ""; }
            //if (!string.IsNullOrEmpty(Signature))
            //{
            //    Signature = Signature.Replace(@"""", string.Empty);
            //    //_updateContact obj = new _updateContact(httpContextAccessorInstance);
            //    IndigoBookingManager_.UpdateContactsResponse _responseAddContact6E = null;// await obj.GetUpdateContacts(Signature, contactobject.emailAddress, contactobject.number, contactobject.companyName, contactobject.customerNumber, "OneWay");
            //    string Str1 = JsonConvert.SerializeObject(_responseAddContact6E);
            //}
            HttpContext.Session.SetString("GDSContactdetails", JsonConvert.SerializeObject(contactobject));
            return RedirectToAction("GDSSaverTripsell", "GDSTripsell");
        }

        //Passenger Data on Trip Page

        public async Task<PartialViewResult> GDSTravllerDetails(List<passkeytype> passengerdetails)
        {
            HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));

            //string Signature = HttpContext.Session.GetString("PassengerNameDetails");
            //if (Signature == null) { Signature = ""; }
            //if (!string.IsNullOrEmpty(Signature))
            //{
            //    Signature = Signature.Replace(@"""", string.Empty);
            //    //_updateContact obj = new _updateContact(httpContextAccessorInstance);
            //    IndigoBookingManager_.UpdatePassengersResponse updatePaxResp = null;// await obj.UpdatePassengers(Signature, passengerdetails, "OneWay");
            //    string Str2 = JsonConvert.SerializeObject(updatePaxResp);
            //}
            string passenger = HttpContext.Session.GetString("SGkeypassenger"); //From Itenary Response
            string passengerInfant = HttpContext.Session.GetString("SGkeypassenger");
            //string Seatmap = HttpContext.Session.GetString("Seatmap");
            //string Meals = HttpContext.Session.GetString("Meals");
            string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");
            ViewModel vm = new ViewModel();
            passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            SeatMapResponceModel Seatmaplist = new SeatMapResponceModel(); //(SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
            SSRAvailabiltyResponceModel Mealslist = new SSRAvailabiltyResponceModel();// (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
            if (!string.IsNullOrEmpty(passengerNamedetails))
            {
                List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                vm.passengerNamedetails = passengerNamedetailsdata;
            }

            vm.passeengerlist = passeengerlist;
            vm.Seatmaplist = Seatmaplist;
            vm.Meals = Mealslist;

            return PartialView("_GDSServiceRequestsPartialView", vm);

            //return RedirectToAction("IndigoSaverTripsell", "IndigoTripsell", passengerdetails);
        }
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

            string tokenview = HttpContext.Session.GetString("IndigoSignature");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }
            string passenger = HttpContext.Session.GetString("SGkeypassenger");
            AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            int passengerscount = passeengerKeyList.passengerscount;
            using (HttpClient client = new HttpClient())
            {
                if (ssrKey.Count > 0)
                {
                    #region SellSSr
                    //_SellSSR obj_ = new _SellSSR(httpContextAccessorInstance);
                    //IndigoBookingManager_.SellResponse sellSsrResponse = null;// await obj_.sellssr(token, passeengerKeyList, ssrKey, BaggageSSrkey, FastfarwardAddon, PPBGAddon, 0, "OneWay");
                    #endregion

                }
                if (unitKey.Count > 0)
                {
                    //try
                    //{
                    //    var unitKey_1 = unitKey;// selectedIds;
                    //    string[] unitKey2 = null;
                    //    string[] unitsubKey2 = null;
                    //    string pas_unitKey = string.Empty;

                    //    int journeyscount = passeengerKeyList.journeys.Count;

                    //    //_SellSSR obj_ = new _SellSSR(httpContextAccessorInstance);
                    //    IndigoBookingManager_.AssignSeatsResponse _AssignseatRes = null; //await obj_.AssignSeat(token, passeengerKeyList, unitKey, 0, unitKey.Count, 0, "OneWay");

                    //    string Str2 = JsonConvert.SerializeObject(_AssignseatRes);

                    //    if (_AssignseatRes != null)
                    //    {
                    //        var JsonObjSeatAssignment = _AssignseatRes;
                    //        #region GetBookingFromState
                    //        //_sell objsell = new _sell();
                    //        IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS = null;// await objsell.GetBookingFromState(token, "OneWay");

                    //        string str3 = JsonConvert.SerializeObject(_GetBookingFromStateRS);

                    //        if (_GetBookingFromStateRS != null)
                    //        {
                    //            //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                    //        }

                    //        #endregion
                    //    }

                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
            }

            return RedirectToAction("GDSPayment", "GDSPaymentGateway");
        }

        public async Task<IActionResult> PostMeal(legpassengers legpassengers)
        {
            using (HttpClient client = new HttpClient())
            {
                #region SellSSR
                SellSSRModel _sellSSRModel = new SellSSRModel();
                _sellSSRModel.count = 1;
                _sellSSRModel.note = "DevTest";
                _sellSSRModel.forceWaveOnSell = false;
                _sellSSRModel.currencyCode = "INR";


                var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/" + legpassengers.ssrKey, _sellSSRModel);
                if (responseSellSSR.IsSuccessStatusCode)
                {
                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                }
                #endregion
            }
            return View();
        }

        public class ssrsegmentwise
        {
            public List<ssrsKey> SSRcode0 { get; set; }
            public List<ssrsKey> SSRcode1 { get; set; }
            public List<ssrsKey> SSRcodeOneWayI { get; set; }
            public List<ssrsKey> SSRcodeOneWayII { get; set; }
            public List<ssrsKey> SSRcodeRTI { get; set; }
            public List<ssrsKey> SSRcodeRTII { get; set; }
            public List<ssrsKey> SSRbaggagecodeOneWayI { get; set; }
            public List<ssrsKey> SSRbaggagecodeOneWayII { get; set; }
            public List<ssrsKey> SSRbaggagecodeRTI { get; set; }
            public List<ssrsKey> SSRbaggagecodeRTII { get; set; }
            public List<ssrsKey> SSRffwOneWayI { get; set; }
            public List<ssrsKey> SSRffwcodeRTI { get; set; }
            public List<ssrsKey> PPBGOneWayI { get; set; }
            public List<ssrsKey> PPBGcodeRTI { get; set; }
        }

        public class ssrsKey
        {
            public string key { get; set; }
        }

        public class Paxes
        {
            public List<passkeytype> Adults_ { get; set; }
            public List<passkeytype> Childs_ { get; set; }

            public List<passkeytype> Infant_ { get; set; }
        }
        Paxes _paxes = new Paxes();
    }
}
