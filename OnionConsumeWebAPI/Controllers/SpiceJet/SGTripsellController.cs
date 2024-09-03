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
using Bookingmanager_;
using Utility;

namespace OnionConsumeWebAPI.Controllers
{
    public class SGTripsellController : Controller
    {
        Logs logs = new Logs();
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        AirAsiaTripResponceModel passeengerlist = null;
        public IActionResult SpiceJetSaverTripsell()
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
            //string passenger = HttpContext.Session.GetString("keypassenger");
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
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                //SeatMapResponceModel Seatmaplist = new SeatMapResponceModel();
                //SSRAvailabiltyResponceModel Mealslist = new SSRAvailabiltyResponceModel();
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
                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;
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
        public async Task<IActionResult> SGContactDetails(ContactModel obj)
        {
            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                UpdateContactsRequest _ContactModel = new UpdateContactsRequest();
                //  _ContactModel.emailAddress = passengerdetails.Email;
                _ContactModel.updateContactsRequestData = new UpdateContactsRequestData();
                _ContactModel.Signature = token;
                _ContactModel.ContractVersion = 420;
                _ContactModel.updateContactsRequestData.BookingContactList = new BookingContact[1];
                _ContactModel.updateContactsRequestData.BookingContactList[0] = new BookingContact();
              
                if (obj.customerNumber != null && obj.customerNumber != "")
                {
                    _ContactModel.updateContactsRequestData.BookingContactList[0].TypeCode = "G";
                    _ContactModel.updateContactsRequestData.BookingContactList[0].CompanyName = obj.companyName;
                    _ContactModel.updateContactsRequestData.BookingContactList[0].CustomerNumber = obj.customerNumber; //"22AAAAA0000A1Z5"; //GSTNumber Re_ Assistance required for SG API Integration\GST Logs.zip\GST Logs
                    _ContactModel.updateContactsRequestData.BookingContactList[0].EmailAddress = obj.emailAddressgst;
                    //if(obj.emailAddressgst == null)
                    //{
                    //    _ContactModel.updateContactsRequestData.BookingContactList[0].EmailAddress = obj.emailAddress;
                    //}


                }
                else
                {
                    _ContactModel.updateContactsRequestData.BookingContactList[0].EmailAddress = obj.emailAddress;
                    _ContactModel.updateContactsRequestData.BookingContactList[0].TypeCode = "P";
                    _ContactModel.updateContactsRequestData.BookingContactList[0].CountryCode = "IN";
                    _ContactModel.updateContactsRequestData.BookingContactList[0].HomePhone = obj.number;
                    BookingName[] Name = new BookingName[1];
                    Name[0] = new BookingName();
                    Name[0].FirstName = obj.first;
                    Name[0].LastName = obj.last;
                    Name[0].Title = "MR";
                    _ContactModel.updateContactsRequestData.BookingContactList[0].Names = Name;
                }
                SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                UpdateContactsResponse responseAddContact = await objSpiceJet.GetUpdateContactsAsync(_ContactModel);
                HttpContext.Session.SetString("ContactDetails", JsonConvert.SerializeObject(_ContactModel));
                String Str1 = JsonConvert.SerializeObject(responseAddContact);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_ContactModel) + "\n\n Response: " + JsonConvert.SerializeObject(responseAddContact), "UpdateContact", "SpicejetOneWay");

            }
            return RedirectToAction("SpiceJetSaverTripsell", "SGTripsell");
        }

        //Passenger Data on Trip Page
        //[HttpPost]
        public async Task<PartialViewResult> SGTravllerDetails(List<passkeytype> passengerdetails)
        {
            HttpContext.Session.SetString("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));

            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            if (tokenview == null) { tokenview = ""; }
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                UpdatePassengersResponse updatePaxResp = null;
                UpdatePassengersRequest updatePaxReq = null;

                try
                {
                    updatePaxReq = new UpdatePassengersRequest(); //Assign Signature generated from Session
                    updatePaxReq.Signature = token;
                    updatePaxReq.ContractVersion = 420;
                    updatePaxReq.updatePassengersRequestData = new UpdatePassengersRequestData();
                    updatePaxReq.updatePassengersRequestData.Passengers = GetPassenger(passengerdetails);

                    try
                    {
                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                        updatePaxResp = await objSpiceJet.UpdatePassengers(updatePaxReq);

                        string Str2 = JsonConvert.SerializeObject(updatePaxResp);

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(updatePaxResp), "UpdatePassenger", "SpicejetOneWay");

                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {

                }
                string passenger = HttpContext.Session.GetString("SGkeypassenger"); //From Itenary Response
                string passengerInfant = HttpContext.Session.GetString("SGkeypassenger");
                string Seatmap = HttpContext.Session.GetString("Seatmap");
                string Meals = HttpContext.Session.GetString("Meals");
                string passengerNamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                ViewModel vm = new ViewModel();
                passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
                SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
                SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
                if (!string.IsNullOrEmpty(passengerNamedetails))
                {
                    List<passkeytype> passengerNamedetailsdata = (List<passkeytype>)JsonConvert.DeserializeObject(passengerNamedetails, typeof(List<passkeytype>));
                    vm.passengerNamedetails = passengerNamedetailsdata;
                }

                vm.passeengerlist = passeengerlist;
                vm.Seatmaplist = Seatmaplist;
                vm.Meals = Mealslist;

                return PartialView("_ServiceRequestsPartialView", vm);

                //return RedirectToAction("SpiceJetSaverTripsell", "SGTripsell", passengerdetails);
            }
        }

        public PaxPriceType[] getPaxdetails()
        {
            PaxPriceType[] paxPriceTypes = null;
            try
            {
                paxPriceTypes = new PaxPriceType[1];
                paxPriceTypes[0] = new PaxPriceType();
                //int arrcount = 0;
                paxPriceTypes[0].PaxType = "ADT";

            }
            catch { }
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


        //public async Task<IActionResult> GetGstDetails(AddGSTInformation addGSTInformation, string lineOne, string lineTwo, string city, string number, string postalCode)
        //{
        //    string tokenview = HttpContext.Session.GetString("AirasiaTokan");
        //    token = tokenview.Replace(@"""", string.Empty);

        //    using (HttpClient client = new HttpClient())
        //    {
        //        AddGSTInformation addinformation = new AddGSTInformation();


        //        addinformation.contactTypeCode = "G";

        //        GSTPhonenumber Phonenumber = new GSTPhonenumber();
        //        List<GSTPhonenumber> Phonenumberlist = new List<GSTPhonenumber>();
        //        Phonenumber.type = "Other";
        //        Phonenumber.number = number;
        //        Phonenumberlist.Add(Phonenumber);

        //        foreach (var item in Phonenumberlist)
        //        {
        //            addinformation.phoneNumbers = Phonenumberlist;
        //        }
        //        addinformation.cultureCode = "";
        //        GSTAddress Address = new GSTAddress();
        //        Address.lineOne = lineOne;
        //        Address.lineTwo = lineTwo;
        //        Address.lineThree = "";
        //        Address.countryCode = "IN";
        //        Address.provinceState = "TN";
        //        Address.city = city;
        //        Address.postalCode = postalCode;
        //        addinformation.Address = Address;

        //        addinformation.emailAddress = addGSTInformation.emailAddress;
        //        addinformation.customerNumber = addGSTInformation.customerNumber;
        //        addinformation.sourceOrganization = "";
        //        addinformation.distributionOption = "None";
        //        addinformation.notificationPreference = "None";
        //        addinformation.companyName = addGSTInformation.companyName;

        //        GSTName Name = new GSTName();
        //        Name.first = "Vadivel";
        //        Name.middle = "raja";
        //        Name.last = "VR";
        //        Name.title = "MR";
        //        Name.suffix = "";
        //        addinformation.Name = Name;

        //        var jsonContactRequest = JsonConvert.SerializeObject(addinformation, Formatting.Indented);
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/contacts", addinformation);
        //        if (responseAddContact.IsSuccessStatusCode)
        //        {
        //            var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
        //            var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
        //        }

        //    }

        //    return RedirectToAction("Tripsell", "AATripsell");
        //}
        ////public async Task<IActionResult> PostUnitkey(string selectedIds, List<string> ssrKey)
        public async Task<IActionResult> PostUnitkey(List<string> unitKey, List<string> ssrKey, List<string> BaggageSSrkey)
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
            //List<string> ConnetedBaggageSSrkey = new List<string>();
            //for (int i = 0; i < BaggageSSrkey.Count; i++)
            //{
            //    ConnetedBaggageSSrkey.Add(BaggageSSrkey[i].Replace("_0", "_1"));
            //}
            ////ConnetedBaggageSSrkey = BaggageSSrkey;
            //BaggageSSrkey.AddRange(ConnetedBaggageSSrkey);

            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }
            //int journeyscount = passeengerlist.journeys.Count;
            string passenger = HttpContext.Session.GetString("SGkeypassenger");
            AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            int passengerscount = passeengerKeyList.passengerscount;

            //string journeyKey = passeengerKeyList.journeys[0].journeyKey;
            using (HttpClient client = new HttpClient())
            {
                if (ssrKey.Count > 0 || BaggageSSrkey.Count>0)
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

                    int journeyscount = passeengerKeyList.journeys.Count;
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
                            Paxes PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));
                            PaxNum.Infant_ = new List<passkeytype>();
                            bool infant = false;

                            ssrsegmentwise _obj = new ssrsegmentwise();
                            _obj.SSRcode0 = new List<ssrsKey>();
                            _obj.SSRcode1 = new List<ssrsKey>();
                            _obj.SSRbaggagecode0 = new List<ssrsKey>();
                            _obj.SSRbaggagecode1 = new List<ssrsKey>();
                            for (int k = 0; k < ssrKey.Count; k++)
                            {
                                string[] sskeydata = new string[2];
                                if (ssrKey[k].Contains("_0") && !ssrKey[k].Contains("_0_1"))
                                {
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        sskeydata = ssrKey[k].Split("_");
                                        ssrsKey _obj0 = new ssrsKey();
                                        _obj0.key = sskeydata[0];
                                        _obj.SSRcode0.Add(_obj0);
                                    }
                                }
                                else if (ssrKey[k].Contains("_1"))
                                {
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        sskeydata = ssrKey[k].Split("_");
                                        ssrsKey _obj1 = new ssrsKey();
                                        _obj1.key = ssrKey[k];
                                        _obj1.key = sskeydata[0];
                                        _obj.SSRcode1.Add(_obj1);
                                    }
                                }

                            }
                            for (int k = 0; k < BaggageSSrkey.Count; k++)
                            {
                                string[] sskeydata = new string[2];
                                if (BaggageSSrkey[k].Contains("_OneWay0"))
                                {
                                    string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        sskeydata = BaggageSSrkey[k].Split("_");
                                        ssrsKey _objBag0 = new ssrsKey();
                                        _objBag0.key = sskeydata[0];
                                        _obj.SSRbaggagecode0.Add(_objBag0);
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
                                        _obj.SSRbaggagecode1.Add(_objBag1);
                                    }
                                }

                            }
                            if (j == 0)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcode0.Count + _obj.SSRbaggagecode0.Count];
                            }
                            else if (j == 1)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcode1.Count + _obj.SSRbaggagecode1.Count];
                            }


                            int TotalPaxcount = PaxNum.Adults_.Count + PaxNum.Childs_.Count;

                            //bool infant = false;// to do if infant is pass in sellssr then need to updatepassenger Api for infant otherwise exception in commit
                            //if (PaxNum.Infant_.Count > 0)
                            //{
                            //    for (int i1 = 0; i1 < PaxNum.Infant_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                            //    {
                            //        ssrKey.Insert(i1, "INFT_"+i1);
                            //    }
                            //}


                            if (j == 0)
                            {
                                //int k = 0;

                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcode0.Count + _obj.SSRbaggagecode0.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i1 = 0; i1 < PaxNum.Adults_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i1 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcode0.Count > 0 || _obj.SSRbaggagecode0.Count>0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i1 = 0; i1 < _obj.SSRcode0.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcode0[i1].key.ToString().Split(' ');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcode0[i1].key.ToString();
                                                    idx = j1 + i1;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;

                                                }
                                                if (_obj.SSRbaggagecode0.Count > 0)
                                                {
                                                    for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                    {
                                                        int baggagecount = _obj.SSRbaggagecode0.Count;
                                                        if (baggagecount > 0 && k + 1 <= baggagecount)
                                                        {
                                                            if (sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] != null)
                                                            {
                                                                idx++;
                                                            }
                                                            else
                                                                idx = k;
                                                            //idx++;
                                                            string[] wordsArray = _obj.SSRbaggagecode0[k].key.ToString().Split(' ');
                                                            //alert(wordsArray);
                                                            //var meal = null;
                                                            string ssrCodeKey = "";
                                                            if (wordsArray.Length > 1)
                                                            {
                                                                ssrCodeKey = wordsArray[0];
                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                            }
                                                            else
                                                                ssrCodeKey = _obj.SSRbaggagecode0[k].key.ToString();
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
                            else
                            {
                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcode1.Count + _obj.SSRbaggagecode1.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i1 = 0; i1 < PaxNum.Adults_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i1 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcode1.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i1 = 0; i1 < _obj.SSRcode1.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcode1[i1].key.ToString().Split(' ');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcode1[i1].key.ToString();
                                                    idx = j1 + i1;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;
                                                }
                                                if (_obj.SSRbaggagecode1.Count > 0)
                                                {
                                                    idx++;
                                                    for (int k = 0; k < PaxNum.Adults_.Count + PaxNum.Childs_.Count; k++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                    {
                                                        int baggagecount = _obj.SSRbaggagecode1.Count;
                                                        if (baggagecount > 0 && k + 1 <= baggagecount)
                                                        {

                                                            string[] wordsArray = _obj.SSRbaggagecode1[k].key.ToString().Split(' ');
                                                            //alert(wordsArray);
                                                            //var meal = null;
                                                            string ssrCodeKey = "";
                                                            if (wordsArray.Length > 1)
                                                            {
                                                                ssrCodeKey = wordsArray[0];
                                                                ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                            }
                                                            else
                                                                ssrCodeKey = _obj.SSRbaggagecode1[k].key.ToString();
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

                        }
                    }


                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs=

                    sellSsrRequest.SellRequestData = sellreqd;
                    SellResponse sellSsrResponse = null;

                    SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                    sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                    string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR", "SpicejetOneWay");


                    if (sellSsrResponse != null)
                    {
                        //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                        var JsonObjSeatAssignment = sellSsrResponse;
                    }
                    #endregion

                }

                if (unitKey.Count > 0)
                {
                    try
                    {
                        var unitKey_1 = unitKey;// selectedIds;
                        string[] unitKey2 = null;
                        string[] unitsubKey2 = null;
                        string pas_unitKey = string.Empty;

                        int journeyscount = passeengerKeyList.journeys.Count;

                        AssignSeatsResponse _AssignseatRes = new AssignSeatsResponse();
                        AssignSeatsRequest _AssignSeatReq = new AssignSeatsRequest();
                        _AssignSeatReq.Signature = token;
                        _AssignSeatReq.ContractVersion = 420;
                        _AssignSeatReq.SellSeatRequest = new SeatSellRequest();
                        _AssignSeatReq.SellSeatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                        _AssignSeatReq.SellSeatRequest.SeatAssignmentModeSpecified = true;
                        _AssignSeatReq.SellSeatRequest.SegmentSeatRequests = new SegmentSeatRequest[unitKey.Count];

                        for (int i = 0; i < journeyscount; i++)
                        {
                            int segmentscount = passeengerKeyList.journeys[i].segments.Count;
                            //for (int j = 0; j < segmentscount; j++)
                            int l = 0;
                            int m = 0;
                            for (int k = 0; k < unitKey.Count; k++)
                            {
                                int idx = 0;
                                int paxnum = 0;
                                if (unitKey[k].Length > 1)
                                {
                                    unitsubKey2 = unitKey[k].Split('_');
                                    pas_unitKey = unitsubKey2[1];
                                    if (unitsubKey2.Length > 3)
                                    {
                                        idx = int.Parse(unitsubKey2[3]);
                                    }
                                    else
                                    {
                                        idx = int.Parse(unitsubKey2[2]);
                                    }
                                    if (idx == 0) paxnum = l++;
                                    else
                                        paxnum = m++;

                                }

                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k] = new SegmentSeatRequest();
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator = new FlightDesignator();
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator.CarrierCode = passeengerKeyList.journeys[i].segments[idx].identifier.carrierCode;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].FlightDesignator.FlightNumber = passeengerKeyList.journeys[i].segments[idx].identifier.identifier;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].STD = passeengerKeyList.journeys[i].segments[idx].designator.departure;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].STDSpecified = true;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].DepartureStation = passeengerKeyList.journeys[i].segments[idx].designator.origin;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].ArrivalStation = passeengerKeyList.journeys[i].segments[idx].designator.destination;
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].UnitDesignator = pas_unitKey.Trim();
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].PassengerNumbers = new short[1];
                                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[k].PassengerNumbers[0] = Convert.ToInt16(paxnum);
                            }

                        }

                        _AssignSeatReq.SellSeatRequest.IncludeSeatData = true;
                        _AssignSeatReq.SellSeatRequest.IncludeSeatDataSpecified = true;

                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                        _AssignseatRes = await objSpiceJet.Assignseat(_AssignSeatReq);

                        string Str2 = JsonConvert.SerializeObject(_AssignseatRes);

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat", "SpicejetOneWay");

                        if (_AssignseatRes != null)
                        {
                            //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                            var JsonObjSeatAssignment = _AssignseatRes;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return RedirectToAction("SpiceJetPayment", "SGPaymentGateway");
        }

        public async Task<IActionResult> PostUnitkey_OLD(List<string> unitKey, List<string> ssrKey)
        {
            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            token = tokenview.Replace(@"""", string.Empty);
            if (token == "" || token == null)
            {
                return RedirectToAction("Index");
            }
            //int journeyscount = passeengerlist.journeys.Count;
            string passenger = HttpContext.Session.GetString("SGkeypassenger");
            AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
            int passengerscount = passeengerKeyList.passengerscount;

            //string journeyKey = passeengerKeyList.journeys[0].journeyKey;
            using (HttpClient client = new HttpClient())
            {
                //if (ssrKey.Count>=0)
                //{

                //    #region SellSSr
                //    SellRequest sellSsrRequest = new SellRequest();
                //    SellRequestData sellreqd = new SellRequestData();
                //    sellSsrRequest.Signature = token;
                //    sellSsrRequest.ContractVersion = 420;
                //    sellreqd.SellBy = SellBy.SSR;
                //    sellreqd.SellBySpecified = true;
                //    sellreqd.SellSSR = new SellSSR();
                //    sellreqd.SellSSR.SSRRequest = new SSRRequest();
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[1];

                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0] = new SegmentSSRRequest();
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = passeengerKeyList.journeys[0].designator.origin;
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = passeengerKeyList.journeys[0].designator.destination;
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].STD = passeengerKeyList.journeys[0].designator.departure;
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = passeengerKeyList.journeys[0].segments[0].identifier.carrierCode;
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = passeengerKeyList.journeys[0].segments[0].identifier.identifier;

                //    string numinfant = HttpContext.Session.GetString("PaxArray");
                //    Paxes PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));

                //    bool infant = false;// to do if infant is pass in sellssr then need to updatepassenger Api for infant otherwise exception in commit
                //    //if(PaxNum.Infant_.Count>0)
                //    //{
                //    //    for (int i = 0; i < PaxNum.Infant_.Count; i++)//Paxnum 1 adult,1 child,1 infant 2 meal
                //    //    {
                //    //        ssrKey.Insert(i,"INFT");
                //    //    }

                //    //}
                //    //2 adult 1 meal 1 infant
                //    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[ssrKey.Count];
                //    int k = 0;
                //    for (int i = 0; i < ssrKey.Count; i++)
                //    {
                //        string [] wordsArray = ssrKey[i].Split(' ');
                //        //alert(wordsArray);
                //        //var meal = null;
                //        string ssrCodeKey = "";
                //        if (wordsArray.Length > 1)
                //        {
                //            ssrCodeKey = wordsArray[0];
                //            ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                //            //var ssrKey1 = ssrCodeKey.Split('"');
                //            //var sskeydata = ssrKey1[1];
                //            //ssrKey = sskeydata;
                //            //alert(ssrKey);
                //            //meal = wordsArray[1];
                //        }
                //        else
                //            ssrCodeKey = ssrKey[i];

                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = ssrCodeKey;// ssrKey[i];
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = Convert.ToInt16(0);
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = passeengerKeyList.journeys[0].designator.origin;
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = passeengerKeyList.journeys[0].designator.destination;
                //        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
                //        if (ssrCodeKey != "INFT")
                //        {
                //            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(k);
                //            k++;
                //        }
                //        else
                //        {
                //            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(i);
                //        }
                //        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].
                //    }
                //    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs=

                //    sellSsrRequest.SellRequestData = sellreqd;
                //    SellResponse sellSsrResponse = null;

                //    SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                //    sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                //    string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                //    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR");


                //    if (sellSsrResponse != null)
                //    {
                //        //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                //        var JsonObjSeatAssignment = sellSsrResponse;
                //    }
                //    #endregion

                //}
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

                    int journeyscount = passeengerKeyList.journeys.Count;
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
                            Paxes PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));
                            bool infant = false;
                            ssrsegmentwise _obj = new ssrsegmentwise();
                            _obj.SSRcode0 = new List<ssrsKey>();
                            _obj.SSRcode1 = new List<ssrsKey>();
                            for (int k = 0; k < ssrKey.Count; k++)
                            {

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

                            //bool infant = false;// to do if infant is pass in sellssr then need to updatepassenger Api for infant otherwise exception in commit
                            //if (PaxNum.Infant_.Count > 0)
                            //{
                            //    for (int i1 = 0; i1 < PaxNum.Infant_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                            //    {
                            //        ssrKey.Insert(i1, "INFT_"+i1);
                            //    }
                            //}


                            if (j == 0)
                            {
                                //int k = 0;

                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcode0.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i1 = 0; i1 < PaxNum.Adults_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i1 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcode0.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i1 = 0; i1 < _obj.SSRcode0.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcode0[i1].key.ToString().Split(' ');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcode0[i1].key.ToString();
                                                    idx = j1 + i1;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i1);
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
                                            for (int i1 = 0; i1 < PaxNum.Adults_.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i1 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumber = Convert.ToInt16(i1);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcode0.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i1 = 0; i1 < _obj.SSRcode1.Count; i1++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcode1[i1].key.ToString().Split(' ');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcode1[i1].key.ToString();
                                                    idx = j1 + i1;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i1);
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

                    SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                    sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                    string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR", "SpicejetOneWay");


                    if (sellSsrResponse != null)
                    {
                        //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                        var JsonObjSeatAssignment = sellSsrResponse;
                    }
                    #endregion

                }
                if (unitKey.Count > 0)
                {
                    try
                    {

                        var unitKey_1 = unitKey;// selectedIds;

                        #region SeatAssignment
                        string[] unitKey2 = null;
                        string[] unitsubKey2 = null;
                        string pas_unitKey = string.Empty;

                        AssignSeatsResponse _AssignseatRes = new AssignSeatsResponse();
                        AssignSeatsRequest _AssignSeatReq = new AssignSeatsRequest();
                        _AssignSeatReq.Signature = token;
                        _AssignSeatReq.ContractVersion = 420;
                        _AssignSeatReq.SellSeatRequest = new SeatSellRequest();
                        _AssignSeatReq.SellSeatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                        _AssignSeatReq.SellSeatRequest.SeatAssignmentModeSpecified = true;
                        _AssignSeatReq.SellSeatRequest.SegmentSeatRequests = new SegmentSeatRequest[unitKey.Count];
                        for (int i = 0; i < unitKey.Count; i++)
                        {
                            if (unitKey[i].Length > 1)
                            {
                                unitsubKey2 = unitKey[i].Split('_');
                                pas_unitKey = unitsubKey2[1];
                            }
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i] = new SegmentSeatRequest();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].FlightDesignator = new FlightDesignator();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].FlightDesignator.CarrierCode = passeengerKeyList.journeys[0].segments[0].identifier.carrierCode;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].FlightDesignator.FlightNumber = passeengerKeyList.journeys[0].segments[0].identifier.identifier;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].STD = passeengerKeyList.journeys[0].designator.departure;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].STDSpecified = true;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].DepartureStation = passeengerKeyList.journeys[0].designator.origin;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].ArrivalStation = passeengerKeyList.journeys[0].designator.destination;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].UnitDesignator = pas_unitKey.Trim();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].PassengerNumbers = new short[1];
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[i].PassengerNumbers[0] = Convert.ToInt16(i);
                        }
                        //_AssignSeatReq.SellSeatRequest.IncludeSeatDataSpecified = true;
                        //_AssignSeatReq.SellSeatRequest.IncludeSeatData = true;

                        SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                        _AssignseatRes = await objSpiceJet.Assignseat(_AssignSeatReq);

                        string Str2 = JsonConvert.SerializeObject(_AssignseatRes);

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat", "SpicejetOneWay");

                        if (_AssignseatRes != null)
                        {
                            //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                            var JsonObjSeatAssignment = _AssignseatRes;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }

            }

            return RedirectToAction("SpiceJetPayment", "SGPaymentGateway");
        }

        public async Task<IActionResult> PostMeal(legpassengers legpassengers)
        {
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



                HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/ssrs/" + legpassengers.ssrKey, _sellSSRModel);
                if (responseSellSSR.IsSuccessStatusCode)
                {
                    var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
                    var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
                }
            }

            #endregion
            return View();
        }

        public class ssrsegmentwise
        {
            public List<ssrsKey> SSRcode0 { get; set; }
            public List<ssrsKey> SSRcode1 { get; set; }
            public List<ssrsKey> SSRbaggagecode0 { get; set; }
            public List<ssrsKey> SSRbaggagecode1 { get; set; }
            public List<ssrsKey> SSRcodeOneWayI { get; set; }
            public List<ssrsKey> SSRcodeOneWayII { get; set; }
            public List<ssrsKey> SSRcodeRTI { get; set; }
            public List<ssrsKey> SSRcodeRTII { get; set; }
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
                #region
                //int chdPax = 0;
                //int infFax = 0;
                //passengers = new Passenger[travellers_.Count]; //Assign Passenger Information 
                //Passenger p1 = null;
                //int PassCnt = 0;
                //for (int cntAdt = 0; cntAdt < travellers_.Count; cntAdt++)
                //{
                //    if (travellers_[cntAdt].passengertypecode.ToString().ToUpper() == "ADT")
                //    {

                //        p1 = new Passenger();
                //        p1.PassengerNumberSpecified = true;
                //        p1.PassengerNumber = Convert.ToInt16(PassCnt);
                //        p1.Names = new BookingName[1];
                //        p1.Names[0] = new BookingName();
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].first))
                //        {
                //            p1.Names[0].FirstName = Convert.ToString(travellers_[cntAdt].first.Trim()).ToUpper();
                //        }
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].middle))
                //        {
                //            p1.Names[0].MiddleName = Convert.ToString(travellers_[cntAdt].middle.Trim()).ToUpper();
                //        }
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].last))
                //        {
                //            p1.Names[0].LastName = Convert.ToString(travellers_[cntAdt].last.Trim()).ToUpper();
                //        }
                //        p1.Names[0].Title = travellers_[cntAdt].title.ToUpper().Replace(".", "");
                //        p1.PassengerInfo = new PassengerInfo();
                //        p1.PassengerInfo.Gender = Gender.Male;
                //        p1.PassengerInfo.WeightCategory = WeightCategory.Male;

                //        p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                //        p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                //        p1.PassengerTypeInfos[0].PaxType = travellers_[cntAdt].passengertypecode.ToString().ToUpper();
                //        p1.PassengerTypeInfos[0].DOBSpecified = true;
                //        p1.PassengerTypeInfos[0].DOB = Convert.ToDateTime("0001-01-01T00:00:00");
                //    }

                //    if(travellers_[cntAdt].passengertypecode.ToString().ToUpper() == "CHD")
                //    {

                //        p1 = new Passenger();
                //        p1.PassengerNumberSpecified = true;
                //        p1.PassengerNumber = Convert.ToInt16(PassCnt);
                //        p1.Names = new BookingName[1];
                //        p1.Names[0] = new BookingName();
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].first))
                //        {
                //            p1.Names[0].FirstName = Convert.ToString(travellers_[cntAdt].first.Trim()).ToUpper();
                //        }
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].middle))
                //        {
                //            p1.Names[0].MiddleName = Convert.ToString(travellers_[cntAdt].middle.Trim()).ToUpper();
                //        }
                //        if (!string.IsNullOrEmpty(travellers_[cntAdt].last))
                //        {
                //            p1.Names[0].LastName = Convert.ToString(travellers_[cntAdt].last.Trim()).ToUpper();
                //        }
                //        p1.Names[0].Title = travellers_[cntAdt].title.ToUpper().Replace(".", "");
                //        p1.PassengerInfo = new PassengerInfo();
                //        p1.PassengerInfo.Gender = Gender.Male;
                //        p1.PassengerInfo.WeightCategory = WeightCategory.Male;

                //        p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                //        p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                //        p1.PassengerTypeInfos[0].PaxType = travellers_[cntAdt].passengertypecode.ToString().ToUpper();
                //        p1.PassengerTypeInfos[0].DOBSpecified = true;
                //        p1.PassengerTypeInfos[0].DOB = Convert.ToDateTime("0001-01-01T00:00:00");


                //        //if (cntAdt < travellers_.InfantTraveller.Count)
                //        //{
                //        //    p1.Infant = new PassengerInfant();
                //        //    p1.Infant.DOB = travellers_.InfantTraveller[cntAdt].DOB;
                //        //    //p1.Infant.Gender = Gender.Male;
                //        //    if (GetGender(travellers_.InfantTraveller[cntAdt].Title.Replace(".", "")) == AirService.GENDER.MALE)
                //        //    {
                //        //        p1.Infant.Gender = Gender.Male;
                //        //    }
                //        //    else
                //        //    {
                //        //        p1.Infant.Gender = Gender.Female;
                //        //    }
                //        //    p1.Infant.Names = new BookingName[1];
                //        //    p1.Infant.Names[0] = new BookingName();
                //        //    if (!string.IsNullOrEmpty(travellers_.InfantTraveller[cntAdt].FirstName))
                //        //    {
                //        //        p1.Infant.Names[0].FirstName = Convert.ToString(travellers_.InfantTraveller[cntAdt].FirstName.Trim());
                //        //    }
                //        //    if (!string.IsNullOrEmpty(travellers_.InfantTraveller[cntAdt].MiddleName))
                //        //    {
                //        //        p1.Infant.Names[0].MiddleName = Convert.ToString(travellers_.InfantTraveller[cntAdt].MiddleName.Trim());
                //        //    }
                //        //    if (!string.IsNullOrEmpty(travellers_.InfantTraveller[cntAdt].LastName))
                //        //    {
                //        //        p1.Infant.Names[0].LastName = Convert.ToString(travellers_.InfantTraveller[cntAdt].LastName.Trim());
                //        //    }
                //        //    p1.Infant.Names[0].Title = travellers_.InfantTraveller[cntAdt].Title.Replace(".", "");
                //        //    p1.Infant.Nationality = travellers_.InfantTraveller[cntAdt].Nationality;
                //        //    p1.Infant.ResidentCountry = travellers_.InfantTraveller[cntAdt].ResidentCountry;
                //        //    p1.State = MessageState.New;
                //        //}

                //    }



                //    passengers[PassCnt] = p1;
                //    PassCnt++;
                //}
                #endregion

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
        //public Passenger[] GetPassenger(List<passkeytype> travellers_)
        //{
        //    Passenger[] passengers = new Passenger[travellers_.Count];

        //    try
        //    {
        //        int chdPax = 0;
        //        int infFax = 0;
        //        Passenger p1 = null;
        //        int PassCnt = 0;
        //        for (int i = 0; i < travellers_.Count; i++)
        //        {
        //            p1 = new Passenger();
        //            p1.Names = new BookingName[1];
        //            p1.PassengerNumber = Convert.ToInt16(i);
        //            p1.Names[0] = new BookingName();
        //            p1.Names[0].FirstName = travellers_[i].first;
        //            p1.Names[0].MiddleName = travellers_[i].middle;
        //            p1.Names[0].LastName = travellers_[i].last;
        //            p1.Names[0].Title = travellers_[i].title;
        //            //our svc model
        //            p1.PassengerInfo = new PassengerInfo();

        //            // our view model
        //            travellers_[i]._passengerInfo = new _PassengerInfo();
        //            travellers_[i]._passengerInfo._gender = _Gender.Male;
        //            p1.PassengerInfo.Gender = (Gender)travellers_[i]._passengerInfo._gender;

        //            travellers_[i]._passengerInfo._weightCategory = _WeightCategory.Male;

        //            p1.PassengerInfo.WeightCategory = (WeightCategory)travellers_[i]._passengerInfo._weightCategory;
        //            p1.PassengerTypeInfos = new PassengerTypeInfo[1];
        //            p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
        //            travellers_[i]._passengerInfo.paxType = travellers_[i].passengertypecode;
        //            p1.PassengerTypeInfos[0].DOBSpecified = true;
        //            travellers_[i]._passengerInfo._DOB = Convert.ToDateTime("0001-01-01T00:00:00");
        //            travellers_[i]._passengerInfo.State = "New";
        //            p1.PassengerTypeInfos[0].PaxType = travellers_[i].passengertypecode;
        //            passengers[i] = p1;

        //        }

        //    }
        //    catch (Exception ex_)
        //    {
        //    }
        //    return passengers;
        //}

    }



}





