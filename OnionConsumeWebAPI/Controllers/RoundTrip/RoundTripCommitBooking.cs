using Bookingmanager_;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Indigo;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using NuGet.Common;
using OnionArchitectureAPI.Services.Indigo;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.Models;
using Sessionmanager;
using System.Collections;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using Utility;
using static DomainLayer.Model.ReturnAirLineTicketBooking;
using static DomainLayer.Model.ReturnTicketBooking;
using OnionArchitectureAPI.Services.Barcode;

namespace OnionConsumeWebAPI.Controllers.RoundTrip
{
    public class RoundTripCommitBooking : Controller
    {
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        string AirLinePNR = string.Empty;
        string BarcodeString = string.Empty;
        string BarcodeInfantString = string.Empty;
        String BarcodePNR = string.Empty;
        string orides = string.Empty;
        string carriercode = string.Empty;
        string flightnumber = string.Empty;
        string seatnumber = string.Empty;
        string sequencenumber = string.Empty;
        string bookingKey = string.Empty;
        ApiResponseModel responseModel;
        double totalAmount = 0;
        double totalAmountBaggage = 0;
        double totalAmounttax = 0;
        double totalAmounttaxSGST = 0;
        double totalAmounttaxBag = 0;
        double totalAmounttaxSGSTBag = 0;
        double totalMealTax = 0;
        double totalBaggageTax = 0;
        double taxMinusMeal = 0;
        double taxMinusBaggage = 0;

        public async Task<IActionResult> RoundTripBookingView()
        {


            AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
            _AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();
            Logs logs = new Logs();

            bool flagAirAsia = true;
            bool flagSpicejet = true;
            bool flagIndigo = true;
            string json = HttpContext.Session.GetString("AirlineSelectedRT");
            Airlinenameforcommit data = JsonConvert.DeserializeObject<Airlinenameforcommit>(json);

            using (HttpClient client = new HttpClient())
            {


                for (int k1 = 0; k1 < data.Airline.Count; k1++)
                {

                    string tokenview = string.Empty;
                    //AirAsia
                    string token = string.Empty;
                    tokenview = HttpContext.Session.GetString("AirasiaTokan");


                    if (!string.IsNullOrEmpty(tokenview) && flagAirAsia == true && data.Airline[k1].ToLower().Contains("airasia"))
                    {
                        //flagAirAsia = false;
                        if (k1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("AirasiaTokan");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("AirasiaTokanR");
                        }
                        token = tokenview.Replace(@"""", string.Empty);
                        #region Commit Booking
                        string[] NotifyContacts = new string[1];
                        NotifyContacts[0] = "P";
                        Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();

                        _Commit_BookingModel.notifyContacts = true;
                        _Commit_BookingModel.contactTypesToNotify = NotifyContacts;
                        var jsonCommitBookingRequest = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

                        //ApiRequests apiRequests = new ApiRequests();
                        //   responseModel = await apiRequests.OnPostAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);


                        if (responceCommit_Booking.IsSuccessStatusCode)
                        {
                            var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
                            logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_Commit_BookingModel) + "Url: " + AppUrlConstant.AirasiaCommitBooking + "\n Response: " + _responceCommit_Booking, "Commit", "AirAsiaRT");

                            var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
                        }
                        #endregion

                        #region Booking GET
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.AirasiaGetBoking);

                        if (responceGetBooking.IsSuccessStatusCode)
                        {
                            // string BaseURL1 = "http://localhost:5225/";
                            var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                            logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetFinalRequest") + "Url: " + AppUrlConstant.AirasiaGetBoking + "\n Response: " + _responceGetBooking, "FinalgetBooking", "AirAsiaRT");
                            var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                            AirLinePNR = JsonObjGetBooking.data.recordLocator;

                        }



                        #endregion
                        #region AirLinePNR
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responcepnrBooking = await client.GetAsync(AppUrlConstant.AirasiaPNRBooking + AirLinePNR);
                        if (responcepnrBooking.IsSuccessStatusCode)
                        {
                            // string BaseURL1 = "http://localhost:5225/";
                            Hashtable htseatdata = new Hashtable();
                            Hashtable htmealdata = new Hashtable();
                            Hashtable htBagdata = new Hashtable();
                            var _responcePNRBooking = responcepnrBooking.Content.ReadAsStringAsync().Result;
                            var JsonObjPNRBooking = JsonConvert.DeserializeObject<dynamic>(_responcePNRBooking);
                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            string PassengerData = HttpContext.Session.GetString("PassengerNameDetails");
                            List<passkeytype> PassengerDataDetailsList = JsonConvert.DeserializeObject<List<passkeytype>>(PassengerData);
                            returnTicketBooking.recordLocator = JsonObjPNRBooking.data.recordLocator;
                            BarcodePNR = JsonObjPNRBooking.data.recordLocator;
                            Info info = new Info();
                            info.bookedDate = JsonObjPNRBooking.data.info.bookedDate;
                            returnTicketBooking.info = info;
                            if (BarcodePNR.Length < 7)
                            {
                                BarcodePNR = BarcodePNR.PadRight(7);
                            }
                            returnTicketBooking.airLines = "AirAsia";
                            returnTicketBooking.bookingKey = JsonObjPNRBooking.data.bookingKey;
                            // var zxvx= JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                            Breakdown breakdown = new Breakdown();
                            breakdown.balanceDue = JsonObjPNRBooking.data.breakdown.balanceDue;
                            JourneyTotals journeyTotalsobj = new JourneyTotals();
                            journeyTotalsobj.totalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                            journeyTotalsobj.totalTax = JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;

                            var baseTotalAmount = journeyTotalsobj.totalAmount;
                            var BaseTotalTax = journeyTotalsobj.totalTax;

                            var ToatalBasePrice = journeyTotalsobj.totalAmount + journeyTotalsobj.totalTax;

                            InfantReturn infantReturnobj = new InfantReturn();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.infant != null)
                            {
                                infantReturnobj.total = JsonObjPNRBooking.data.breakdown.passengerTotals.infant.total;
                                infantReturnobj.taxes = JsonObjPNRBooking.data.breakdown.passengerTotals.infant.taxes;

                                double totalAmountSum = journeyTotalsobj.totalAmount + infantReturnobj.total;
                                double totaltax = journeyTotalsobj.totalTax + infantReturnobj.taxes;

                                double totalplusAmountSumtax = totalAmountSum + totaltax;
                                breakdown.totalAmountSum = totalAmountSum;
                                breakdown.totaltax = totaltax;
                                breakdown.totalplusAmountSumtax = totalplusAmountSumtax;
                            }

                            PassengerTotals passengerTotals = new PassengerTotals();
                            SpecialServices serviceChargeReturn = new SpecialServices();
                            List<ReturnCharge> returnChargeList = new List<ReturnCharge>();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices != null)
                            {
                                int chargesCount = JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.charges.Count;

                                for (int ch = 0; ch < chargesCount; ch++)
                                {
                                    ReturnCharge returnChargeobj = new ReturnCharge();
                                    returnChargeobj.amount = JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.charges[ch].amount;
                                    returnChargeobj.code = JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.charges[ch].code;
                                    if (returnChargeobj.code.StartsWith("V"))
                                    {
                                        totalAmount += returnChargeobj.amount;

                                    }
                                    if (returnChargeobj.code.StartsWith("C"))
                                    {
                                        totalAmounttax += returnChargeobj.amount;
                                    }
                                    if (returnChargeobj.code.StartsWith("S"))
                                    {
                                        totalAmounttaxSGST += returnChargeobj.amount;
                                    }
                                    totalMealTax = totalAmounttax + totalAmounttaxSGST;
                                    taxMinusMeal = totalAmount - totalMealTax;

                                    if (returnChargeobj.code.StartsWith("P"))
                                    {
                                        totalAmountBaggage += returnChargeobj.amount;

                                    }
                                    if (returnChargeobj.code.StartsWith("C"))
                                    {
                                        totalAmounttaxBag += returnChargeobj.amount;
                                    }

                                    if (returnChargeobj.code.StartsWith("S"))
                                    {
                                        totalAmounttaxSGSTBag += returnChargeobj.amount;
                                    }
                                    totalBaggageTax = totalAmounttaxBag + totalAmounttaxSGSTBag;
                                    taxMinusBaggage = totalAmountBaggage - totalBaggageTax;


                                    returnChargeList.Add(returnChargeobj);
                                }
                                serviceChargeReturn.charges = returnChargeList;

                            }

                            ReturnSeats returnSeats = new ReturnSeats();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.seats != null)
                            {
                                if (JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total > 0 || JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total != null)
                                {
                                    returnSeats.total = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total;
                                    returnSeats.taxes = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.taxes;
                                }
                            }
                            SpecialServices specialServices = new SpecialServices();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices != null)
                            {
                                specialServices.total = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                                specialServices.taxes = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                            }
                            //breakdown.journeyTotals = (int)ToatalBasePrice;

                            breakdown.journeyTotals = journeyTotalsobj;
                            breakdown.passengerTotals = passengerTotals;
                            breakdown.baseTotalAmount = baseTotalAmount;
                            breakdown.ToatalBasePrice = ToatalBasePrice;
                            breakdown.BaseTotalTax = BaseTotalTax;
                            //breakdown.totalAmountSum = totalAmountSum;
                            //breakdown.totaltax = totaltax;
                            //breakdown.totalplusAmountSumtax = totalplusAmountSumtax;
                            passengerTotals.seats = returnSeats;
                            passengerTotals.infant = infantReturnobj;
                            passengerTotals.specialServices = specialServices;
                            passengerTotals.specialServices = serviceChargeReturn;

                            if (JsonObjPNRBooking.data.contacts.G != null)
                            {
                                returnTicketBooking.customerNumber = JsonObjPNRBooking.data.contacts.G.customerNumber;
                                returnTicketBooking.companyName = JsonObjPNRBooking.data.contacts.G.companyName;
                                returnTicketBooking.emailAddress = JsonObjPNRBooking.data.contacts.G.emailAddress;
                            }
                            Contacts _contactobj = new Contacts();
                            int PhoneNumberCount = JsonObjPNRBooking.data.contacts.P.phoneNumbers.Count;
                            List<PhoneNumber> phoneNumberList = new List<PhoneNumber>();
                            for (int p = 0; p < PhoneNumberCount; p++)
                            {
                                PhoneNumber phoneobject = new PhoneNumber();
                                phoneobject.number = JsonObjPNRBooking.data.contacts.P.phoneNumbers[p].number;
                                phoneNumberList.Add(phoneobject);
                            }
                            int JourneysReturnCount = JsonObjPNRBooking.data.journeys.Count;
                            List<JourneysReturn> journeysreturnList = new List<JourneysReturn>();
                            for (int i = 0; i < JourneysReturnCount; i++)
                            {
                                JourneysReturn journeysReturnObj = new JourneysReturn();
                                journeysReturnObj.stops = JsonObjPNRBooking.data.journeys[i].stops;

                                DesignatorReturn ReturnDesignatorobject = new DesignatorReturn();
                                ReturnDesignatorobject.origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
                                ReturnDesignatorobject.destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
                                orides = JsonObjPNRBooking.data.journeys[0].designator.origin + JsonObjPNRBooking.data.journeys[0].designator.destination;
                                ReturnDesignatorobject.departure = JsonObjPNRBooking.data.journeys[0].designator.departure;
                                ReturnDesignatorobject.arrival = JsonObjPNRBooking.data.journeys[0].designator.arrival;

                                journeysReturnObj.designator = ReturnDesignatorobject;
                                int SegmentReturnCount = JsonObjPNRBooking.data.journeys[i].segments.Count;
                                List<SegmentReturn> segmentReturnsList = new List<SegmentReturn>();
                                for (int j = 0; j < SegmentReturnCount; j++)
                                {
                                    returnSeats.unitDesignator = string.Empty;
                                    returnSeats.SSRCode = string.Empty;
                                    SegmentReturn segmentReturnobj = new SegmentReturn();
                                    segmentReturnobj.isStandby = JsonObjPNRBooking.data.journeys[i].segments[j].isStandby;
                                    segmentReturnobj.isHosted = JsonObjPNRBooking.data.journeys[i].segments[j].isHosted;
                                    DesignatorReturn designatorReturn = new DesignatorReturn();
                                    //var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
                                    designatorReturn.origin = JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin;
                                    designatorReturn.destination = JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination;
                                    designatorReturn.departure = JsonObjPNRBooking.data.journeys[i].segments[j].designator.departure;
                                    designatorReturn.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].designator.arrival;
                                    segmentReturnobj.designator = designatorReturn;

                                    var passengersegmentCount = JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment;
                                    int passengerReturnCount = ((Newtonsoft.Json.Linq.JContainer)passengersegmentCount).Count;
                                    List<PassengerSegment> passengerSegmentsList = new List<PassengerSegment>();
                                    foreach (var item in JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment)
                                    {
                                        PassengerSegment passengerSegmentobj = new PassengerSegment();
                                        passengerSegmentobj.passengerKey = item.Value.passengerKey;
                                        passengerSegmentsList.Add(passengerSegmentobj);
                                        int seatCount = item.Value.seats.Count;
                                        int ssrCodeCount = item.Value.ssrs.Count;
                                        List<ReturnSeats> returnSeatsList = new List<ReturnSeats>();
                                        for (int q = 0; q < seatCount; q++)
                                        {
                                            ReturnSeats returnSeatsObj = new ReturnSeats();
                                            returnSeatsObj.unitDesignator = item.Value.seats[q].unitDesignator;
                                            seatnumber = item.Value.seats[q].unitDesignator;
                                            if (string.IsNullOrEmpty(seatnumber))
                                            {
                                                seatnumber = "0000"; // Set to "0000" if not available
                                            }
                                            else
                                            {
                                                seatnumber = seatnumber.PadRight(4, '0'); // Right-pad with zeros if less than 4 characters
                                            }
                                            returnSeatsList.Add(returnSeatsObj);
                                            htseatdata.Add(passengerSegmentobj.passengerKey.ToString() + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination, returnSeatsObj.unitDesignator);
                                            returnSeats.unitDesignator += returnSeatsObj.unitDesignator + ",";
                                        }
                                        List<SsrReturn> SrrcodereturnsList = new List<SsrReturn>();
                                        for (int t = 0; t < ssrCodeCount; t++)
                                        {
                                            SsrReturn ssrReturn = new SsrReturn();
                                            ssrReturn.ssrCode = item.Value.ssrs[t].ssrCode;
                                            if (ssrReturn.ssrCode.StartsWith("P"))
                                            {
                                                continue;
                                            }
                                            else
                                            {

                                                htmealdata.Add(passengerSegmentobj.passengerKey.ToString() + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination, ssrReturn.ssrCode);
                                                returnSeats.SSRCode += ssrReturn.ssrCode + ",";
                                            }


                                        }
                                        for (int t = 0; t < ssrCodeCount; t++)
                                        {
                                            SsrReturn ssrReturn = new SsrReturn();
                                            ssrReturn.ssrCode = item.Value.ssrs[t].ssrCode;
                                            if (ssrReturn.ssrCode.StartsWith("V"))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                htBagdata.Add(passengerSegmentobj.passengerKey.ToString() + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin + "_" + JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination, ssrReturn.ssrCode);
                                                returnSeats.SSRCode += ssrReturn.ssrCode + ",";
                                            }


                                        }

                                    }
                                    segmentReturnobj.passengerSegment = passengerSegmentsList;

                                    int ReturmFareCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares.Count;
                                    List<FareReturn> fareList = new List<FareReturn>();
                                    for (int k = 0; k < ReturmFareCount; k++)
                                    {
                                        FareReturn fareReturnobj = new FareReturn();
                                        fareReturnobj.productClass = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].productClass;

                                        int PassengerFareReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares.Count;
                                        List<PassengerFareReturn> passengerFareReturnList = new List<PassengerFareReturn>();
                                        for (int l = 0; l < PassengerFareReturnCount; l++)
                                        {
                                            PassengerFareReturn passengerFareReturnobj = new PassengerFareReturn();

                                            int ServiceChargeReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges.Count;

                                            List<ServiceChargeReturn> serviceChargeReturnList = new List<ServiceChargeReturn>();
                                            for (int m = 0; m < ServiceChargeReturnCount; m++)
                                            {
                                                ServiceChargeReturn serviceChargeReturnobj = new ServiceChargeReturn();

                                                serviceChargeReturnobj.amount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges[m].amount;
                                                serviceChargeReturnList.Add(serviceChargeReturnobj);


                                            }
                                            passengerFareReturnobj.serviceCharges = serviceChargeReturnList;
                                            passengerFareReturnList.Add(passengerFareReturnobj);

                                        }
                                        fareReturnobj.passengerFares = passengerFareReturnList;
                                        fareList.Add(fareReturnobj);

                                    }
                                    segmentReturnobj.fares = fareList;

                                    IdentifierReturn identifierReturn = new IdentifierReturn();
                                    identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                                    flightnumber = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                                    if (flightnumber.Length < 5)
                                    {
                                        flightnumber = flightnumber.PadRight(5);
                                    }
                                    carriercode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                                    if (carriercode.Length < 3)
                                    {
                                        carriercode = carriercode.PadRight(3);
                                    }
                                    identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                                    segmentReturnobj.identifier = identifierReturn;

                                    var LegReturn = JsonObjPNRBooking.data.journeys[i].segments[j].legs;
                                    int Legcount = ((Newtonsoft.Json.Linq.JContainer)LegReturn).Count;
                                    List<LegReturn> legReturnsList = new List<LegReturn>();
                                    for (int n = 0; n < Legcount; n++)
                                    {
                                        LegReturn LegReturnobj = new LegReturn();
                                        LegReturnobj.legKey = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legKey;

                                        DesignatorReturn ReturnlegDesignatorobj = new DesignatorReturn();
                                        ReturnlegDesignatorobj.origin = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.origin;
                                        ReturnlegDesignatorobj.destination = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.destination;
                                        ReturnlegDesignatorobj.departure = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.departure;
                                        ReturnlegDesignatorobj.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.arrival;
                                        LegReturnobj.designator = ReturnlegDesignatorobj;

                                        LegInfoReturn legInfoReturn = new LegInfoReturn();
                                        legInfoReturn.arrivalTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                        legInfoReturn.arrivalTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                        legInfoReturn.departureTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                        legInfoReturn.departureTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                        LegReturnobj.legInfo = legInfoReturn;
                                        legReturnsList.Add(LegReturnobj);

                                    }
                                    segmentReturnobj.unitdesignator = returnSeats.unitDesignator;
                                    segmentReturnobj.SSRCode = returnSeats.SSRCode;
                                    segmentReturnobj.legs = legReturnsList;
                                    segmentReturnsList.Add(segmentReturnobj);

                                }
                                journeysReturnObj.segments = segmentReturnsList;
                                journeysreturnList.Add(journeysReturnObj);
                            }

                            var Returnpassanger = JsonObjPNRBooking.data.passengers;
                            int Returnpassengercount = ((Newtonsoft.Json.Linq.JContainer)Returnpassanger).Count;
                            List<ReturnPassengers> ReturnpassengersList = new List<ReturnPassengers>();
                            foreach (var items in JsonObjPNRBooking.data.passengers)
                            {
                                ReturnPassengers returnPassengersobj = new ReturnPassengers();
                                returnPassengersobj.passengerKey = items.Value.passengerKey;
                                returnPassengersobj.passengerTypeCode = items.Value.passengerTypeCode;
                                returnPassengersobj.name = new Name();
                                returnPassengersobj.name.first = items.Value.name.first + " " + items.Value.name.last;
                                //returnPassengersobj.name.last = items.Value.name.last;
                                for (int i = 0; i < PassengerDataDetailsList.Count; i++)
                                {
                                    if (returnPassengersobj.passengerTypeCode == PassengerDataDetailsList[i].passengertypecode && returnPassengersobj.name.first.ToLower() == PassengerDataDetailsList[i].first.ToLower() + " " + PassengerDataDetailsList[i].last.ToLower())
                                    {
                                        returnPassengersobj.MobNumber = PassengerDataDetailsList[i].mobile;
                                        returnPassengersobj.passengerKey = PassengerDataDetailsList[i].passengerkey;

                                        break;
                                    }

                                }
                                ReturnpassengersList.Add(returnPassengersobj);

                                string dateString = JsonObjPNRBooking.data.journeys[0].designator.departure;
                                DateTime date = DateTime.ParseExact(dateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                //julian date
                                int year = date.Year;
                                int month = date.Month;
                                int day = date.Day;

                                // Calculate the number of days from January 1st to the given date
                                DateTime currentDate = new DateTime(year, month, day);
                                DateTime startOfYear = new DateTime(year, 1, 1);
                                int julianDate = (currentDate - startOfYear).Days + 1;
                                if (string.IsNullOrEmpty(sequencenumber))
                                {
                                    sequencenumber = "0000";
                                }
                                else
                                {
                                    sequencenumber = sequencenumber.PadRight(5, '0');
                                }

                                BarcodeString = "M" + "1" + items.Value.name.last + "/" + items.Value.name.first + " " + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDate + "Y" + seatnumber + " " + sequencenumber + "1" + "00";
                                BarcodeUtility BarcodeUtility = new BarcodeUtility();
                                var barcodeImage = BarcodeUtility.BarcodereadUtility(BarcodeString);
                                returnPassengersobj.barcodestring = barcodeImage;
                                //InfantReturn infantsObject = new InfantReturn();
                                if (items.Value.infant != null)
                                {
                                    returnPassengersobj = new ReturnPassengers();
                                    returnPassengersobj.name = new Name();
                                    returnPassengersobj.passengerTypeCode = "INFT";
                                    returnPassengersobj.name.first = items.Value.infant.name.first + " " + items.Value.infant.name.last;
                                    //passkeytypeobj.MobNumber = "";
                                    for (int i = 0; i < PassengerDataDetailsList.Count; i++)
                                    {
                                        if (returnPassengersobj.passengerTypeCode == PassengerDataDetailsList[i].passengertypecode && returnPassengersobj.name.first.ToLower() == PassengerDataDetailsList[i].first.ToLower() + " " + PassengerDataDetailsList[i].last.ToLower())
                                        {
                                            returnPassengersobj.passengerKey = PassengerDataDetailsList[i].passengerkey;
                                            break;
                                        }

                                    }
                                    ReturnpassengersList.Add(returnPassengersobj);

                                }

                            }

                            returnTicketBooking.breakdown = breakdown;
                            returnTicketBooking.journeys = journeysreturnList;
                            returnTicketBooking.passengers = ReturnpassengersList;
                            returnTicketBooking.passengerscount = Returnpassengercount;
                            returnTicketBooking.PhoneNumbers = phoneNumberList;
                            returnTicketBooking.totalAmount = totalAmount;
                            returnTicketBooking.taxMinusMeal = taxMinusMeal;
                            returnTicketBooking.taxMinusBaggage = taxMinusBaggage;
                            returnTicketBooking.totalMealTax = totalMealTax;
                            returnTicketBooking.totalAmountBaggage = totalAmountBaggage;
                            returnTicketBooking.Seatdata = htseatdata;
                            returnTicketBooking.Mealdata = htmealdata;
                            returnTicketBooking.Bagdata = htBagdata;
                            #region
                            //var _responcePNRBooking = responcepnrBooking.Content.ReadAsStringAsync().Result;
                            //var JsonObjPNRBooking = JsonConvert.DeserializeObject<dynamic>(_responcePNRBooking);

                            //ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            //returnTicketBooking.recordLocator = JsonObjPNRBooking.data.recordLocator;
                            //returnTicketBooking.airLines = "AirAsia";
                            //returnTicketBooking.bookingKey = JsonObjPNRBooking.data.bookingKey;
                            //int JourneysReturnCount = JsonObjPNRBooking.data.journeys.Count;

                            //Breakdown breakdown = new Breakdown();
                            //JourneyTotals journeyTotalsobj = new JourneyTotals();
                            //journeyTotalsobj.totalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                            //journeyTotalsobj.totalTax = JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
                            //var ToatalBasePrice = journeyTotalsobj.totalAmount + journeyTotalsobj.totalTax;

                            //PassengerTotals passengerTotals = new PassengerTotals();
                            //ReturnSeats returnSeats = new ReturnSeats();
                            //if (JsonObjPNRBooking.data.breakdown.passengerTotals.seats != null)
                            //{
                            //    returnSeats.total = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total;
                            //    returnSeats.taxes = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.taxes;
                            //}
                            //SpecialServices specialServices = new SpecialServices();
                            //if (JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices != null)
                            //{
                            //    specialServices.total = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                            //    specialServices.taxes = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                            //}
                            ////breakdown.journeyTotals = (int)ToatalBasePrice;
                            //breakdown.journeyTotals = journeyTotalsobj;
                            //breakdown.passengerTotals = passengerTotals;
                            //passengerTotals.seats = returnSeats;
                            //passengerTotals.specialServices = specialServices;
                            //if (JsonObjPNRBooking.data.contacts.G != null)
                            //{
                            //    returnTicketBooking.customerNumber = JsonObjPNRBooking.data.contacts.G.customerNumber;
                            //    returnTicketBooking.companyName = JsonObjPNRBooking.data.contacts.G.companyName;
                            //    returnTicketBooking.emailAddress = JsonObjPNRBooking.data.contacts.G.emailAddress;
                            //}
                            //Contacts _contactobj = new Contacts();
                            //int PhoneNumberCount = JsonObjPNRBooking.data.contacts.P.phoneNumbers.Count;
                            //List<PhoneNumber> phoneNumberList = new List<PhoneNumber>();
                            //for (int p = 0; p < PhoneNumberCount; p++)
                            //{
                            //    PhoneNumber phoneobject = new PhoneNumber();
                            //    phoneobject.number = JsonObjPNRBooking.data.contacts.P.phoneNumbers[p].number;
                            //    phoneNumberList.Add(phoneobject);
                            //}

                            //List<JourneysReturn> journeysreturnList = new List<JourneysReturn>();
                            //for (int i = 0; i < JourneysReturnCount; i++)
                            //{
                            //    JourneysReturn journeysReturnObj = new JourneysReturn();
                            //    journeysReturnObj.stops = JsonObjPNRBooking.data.journeys[i].stops;

                            //    DesignatorReturn ReturnDesignatorobject = new DesignatorReturn();
                            //    ReturnDesignatorobject.origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
                            //    ReturnDesignatorobject.destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
                            //    ReturnDesignatorobject.departure = JsonObjPNRBooking.data.journeys[0].designator.departure;
                            //    ReturnDesignatorobject.arrival = JsonObjPNRBooking.data.journeys[0].designator.arrival;
                            //    journeysReturnObj.designator = ReturnDesignatorobject;


                            //    int SegmentReturnCount = JsonObjPNRBooking.data.journeys[i].segments.Count;
                            //    List<SegmentReturn> segmentReturnsList = new List<SegmentReturn>();
                            //    for (int j = 0; j < SegmentReturnCount; j++)
                            //    {
                            //        SegmentReturn segmentReturnobj = new SegmentReturn();
                            //        segmentReturnobj.isStandby = JsonObjPNRBooking.data.journeys[i].segments[j].isStandby;
                            //        segmentReturnobj.isHosted = JsonObjPNRBooking.data.journeys[i].segments[j].isHosted;


                            //        DesignatorReturn designatorReturn = new DesignatorReturn();
                            //        //var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
                            //        designatorReturn.origin = JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin;
                            //        designatorReturn.destination = JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination;
                            //        designatorReturn.departure = JsonObjPNRBooking.data.journeys[i].segments[j].designator.departure;
                            //        designatorReturn.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].designator.arrival;
                            //        segmentReturnobj.designator = designatorReturn;

                            //        var passengersegmentCount = JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment;
                            //        int passengerReturnCount = ((Newtonsoft.Json.Linq.JContainer)passengersegmentCount).Count;
                            //        List<PassengerSegment> passengerSegmentsList = new List<PassengerSegment>();
                            //        foreach (var item in JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment)
                            //        {
                            //            PassengerSegment passengerSegmentobj = new PassengerSegment();
                            //            passengerSegmentobj.passengerKey = item.Value.passengerKey;

                            //            passengerSegmentsList.Add(passengerSegmentobj);
                            //            int seatCount = item.Value.seats.Count;
                            //            List<ReturnSeats> returnSeatsList = new List<ReturnSeats>();
                            //            for (int q = 0; q < seatCount; q++)
                            //            {
                            //                ReturnSeats returnSeatsObj = new ReturnSeats();

                            //                returnSeatsObj.unitDesignator = item.Value.seats[q].unitDesignator;
                            //                //seatnumber = item.Value.seats[q].unitDesignator;
                            //                //if (string.IsNullOrEmpty(seatnumber))
                            //                //{
                            //                //    seatnumber = "0000"; // Set to "0000" if not available
                            //                //}
                            //                //else
                            //                //{
                            //                //    seatnumber = seatnumber.PadRight(4, '0'); // Right-pad with zeros if less than 4 characters
                            //                //}

                            //                returnSeatsList.Add(returnSeatsObj);
                            //            }
                            //            passengerSegmentobj.seats = returnSeatsList;
                            //            //passengerSegmentsList.Add(passengerSegmentobj);
                            //        }
                            //        segmentReturnobj.passengerSegment = passengerSegmentsList;


                            //        //IdentifierReturn identifierReturn = new IdentifierReturn();
                            //        //identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                            //        //identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                            //        //segmentReturnobj.identifier = identifierReturn;

                            //        int ReturmFareCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares.Count;
                            //        List<FareReturn> fareList = new List<FareReturn>();
                            //        for (int k = 0; k < ReturmFareCount; k++)
                            //        {
                            //            FareReturn fareReturnobj = new FareReturn();
                            //            fareReturnobj.productClass = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].productClass;

                            //            int PassengerFareReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares.Count;
                            //            List<PassengerFareReturn> passengerFareReturnList = new List<PassengerFareReturn>();
                            //            for (int l = 0; l < PassengerFareReturnCount; l++)
                            //            {
                            //                PassengerFareReturn passengerFareReturnobj = new PassengerFareReturn();

                            //                int ServiceChargeReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges.Count;

                            //                List<ServiceChargeReturn> serviceChargeReturnList = new List<ServiceChargeReturn>();
                            //                for (int m = 0; m < ServiceChargeReturnCount; m++)
                            //                {
                            //                    try
                            //                    {
                            //                        ServiceChargeReturn serviceChargeReturnobj = new ServiceChargeReturn();

                            //                        serviceChargeReturnobj.amount = JsonObjPNRBooking.data.journeys[i].segments[0].fares[j].passengerFares[k].serviceCharges[l].amount;
                            //                        serviceChargeReturnList.Add(serviceChargeReturnobj);
                            //                    }
                            //                    catch (Exception ex)
                            //                    {

                            //                    }

                            //                }
                            //                passengerFareReturnobj.serviceCharges = serviceChargeReturnList;
                            //                passengerFareReturnList.Add(passengerFareReturnobj);

                            //            }
                            //            fareReturnobj.passengerFares = passengerFareReturnList;
                            //            fareList.Add(fareReturnobj);

                            //        }
                            //        segmentReturnobj.fares = fareList;

                            //        IdentifierReturn identifierReturn = new IdentifierReturn();
                            //        identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                            //        identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                            //        segmentReturnobj.identifier = identifierReturn;

                            //        var LegReturn = JsonObjPNRBooking.data.journeys[i].segments[j].legs;
                            //        int Legcount = ((Newtonsoft.Json.Linq.JContainer)LegReturn).Count;
                            //        List<LegReturn> legReturnsList = new List<LegReturn>();
                            //        for (int n = 0; n < Legcount; n++)
                            //        {
                            //            LegReturn LegReturnobj = new LegReturn();
                            //            LegReturnobj.legKey = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legKey;

                            //            DesignatorReturn ReturnlegDesignatorobj = new DesignatorReturn();
                            //            ReturnlegDesignatorobj.origin = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.origin;
                            //            ReturnlegDesignatorobj.destination = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.destination;
                            //            ReturnlegDesignatorobj.departure = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.departure;
                            //            ReturnlegDesignatorobj.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.arrival;
                            //            LegReturnobj.designator = ReturnlegDesignatorobj;

                            //            LegInfoReturn legInfoReturn = new LegInfoReturn();
                            //            legInfoReturn.arrivalTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                            //            legInfoReturn.arrivalTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                            //            legInfoReturn.departureTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                            //            legInfoReturn.departureTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                            //            LegReturnobj.legInfo = legInfoReturn;
                            //            legReturnsList.Add(LegReturnobj);

                            //        }
                            //        segmentReturnobj.legs = legReturnsList;
                            //        segmentReturnsList.Add(segmentReturnobj);

                            //    }
                            //    journeysReturnObj.segments = segmentReturnsList;
                            //    journeysreturnList.Add(journeysReturnObj);
                            //}
                            //var Returnpassanger = JsonObjPNRBooking.data.passengers;
                            //int Returnpassengercount = ((Newtonsoft.Json.Linq.JContainer)Returnpassanger).Count;

                            //List<ReturnPassengers> ReturnpassengersList = new List<ReturnPassengers>();
                            //foreach (var items in JsonObjPNRBooking.data.passengers)
                            //{
                            //    ReturnPassengers returnPassengersobj = new ReturnPassengers();
                            //    returnPassengersobj.passengerKey = items.Value.passengerKey;
                            //    returnPassengersobj.passengerTypeCode = items.Value.passengerTypeCode;
                            //    //  passkeytypeobj.passengertypecount = items.Count;
                            //    returnPassengersobj.name = new Name();
                            //    returnPassengersobj.name.first = items.Value.name.first;
                            //    ReturnpassengersList.Add(returnPassengersobj);


                            //}
                            //returnTicketBooking.breakdown = breakdown;
                            //returnTicketBooking.journeys = journeysreturnList;
                            //returnTicketBooking.passengers = ReturnpassengersList;
                            //returnTicketBooking.passengerscount = Returnpassengercount;
                            //returnTicketBooking.PhoneNumbers = phoneNumberList;
                            ////returnTicketBooking.breakdown = breakdown;
                            ////returnTicketBooking.journeys = journeysreturnList;
                            ////returnTicketBooking.passengers = ReturnpassengersList;
                            ////returnTicketBooking.passengerscount = Returnpassengercount;

                            //// HttpContext.Session.SetString("PNRTicketBooking", JsonConvert.SerializeObject(returnTicketBooking));
                            #endregion
                            _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                        }
                        //}


                        #endregion
                    }

                    //Akasa Air Line Commit Booking
                    var BookingKeyAkasa=string.Empty;
                    tokenview = string.Empty;

                    token = string.Empty;
                    tokenview = HttpContext.Session.GetString("AkasaTokan");
                    if (!string.IsNullOrEmpty(tokenview) && flagAirAsia == true && data.Airline[k1].ToLower().Contains("akasaair"))
                    {
                        if (k1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("AkasaTokan");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("AkasaTokanR");
                        }
                        token = tokenview.Replace(@"""", string.Empty);
                        #region Get Booking
                        Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();
                        _Commit_BookingModel.receivedBy = null;
                        _Commit_BookingModel.restrictionOverride = false;
                        _Commit_BookingModel.hold = null;
                        _Commit_BookingModel.notifyContacts = false;
                        _Commit_BookingModel.comments = null;
                        _Commit_BookingModel.contactTypesToNotify = null;
                        var jsonCommitBookingRequest = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(AppUrlConstant.AkasaGetBoking, _Commit_BookingModel);


                        if (responceCommit_Booking.IsSuccessStatusCode)
                        {
                            var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
                            //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_Commit_BookingModel) + "Url: " + AppUrlConstant.AkasaCommitBooking + "\n Response: " + _responceCommit_Booking, "Commit", "AkasaAirRT");
                         
                            var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);

                        }
                        #endregion

                        #region Commit Booking
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.AkasaCommitBooking);

                        if (responceGetBooking.IsSuccessStatusCode)
                        {
                            
                            var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                            //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetFinalRequest") + "Url: " + AppUrlConstant.AkasaGetBoking + "\n Response: " + _responceGetBooking, "FinalgetBooking", "AkasaRT");
                            var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                            AirLinePNR = JsonObjGetBooking.data.recordLocator;
                            BookingKeyAkasa = JsonObjGetBooking.data.bookingkey;

                        }


                        #endregion
                        #region AirLinePNR
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responcepnrBooking = await client.GetAsync(AppUrlConstant.AkasaPNRBooking + BookingKeyAkasa);
                        if (responcepnrBooking.IsSuccessStatusCode)
                        {
                            // string BaseURL1 = "http://localhost:5225/";
                            var _responcePNRBooking = responcepnrBooking.Content.ReadAsStringAsync().Result; //_responceGetBooking;//
                            var JsonObjPNRBooking = JsonConvert.DeserializeObject<dynamic>(_responcePNRBooking);

                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            returnTicketBooking.recordLocator = JsonObjPNRBooking.data.recordLocator;
                            returnTicketBooking.airLines = "AkasaAir";
                            returnTicketBooking.bookingKey = JsonObjPNRBooking.data.bookingKey;
                            int JourneysReturnCount = JsonObjPNRBooking.data.journeys.Count;

                            Breakdown breakdown = new Breakdown();
                            JourneyTotals journeyTotalsobj = new JourneyTotals();
                            journeyTotalsobj.totalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                            journeyTotalsobj.totalTax = JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
                            var ToatalBasePrice = journeyTotalsobj.totalAmount + journeyTotalsobj.totalTax;

                            PassengerTotals passengerTotals = new PassengerTotals();
                            ReturnSeats returnSeats = new ReturnSeats();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.seats != null)
                            {
                                returnSeats.total = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total;
                                returnSeats.taxes = JsonObjPNRBooking.data.breakdown.passengerTotals.seats.taxes;
                            }
                            SpecialServices specialServices = new SpecialServices();
                            if (JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices != null)
                            {
                                specialServices.total = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                                specialServices.taxes = (decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                            }
                            //breakdown.journeyTotals = (int)ToatalBasePrice;
                            breakdown.journeyTotals = journeyTotalsobj;
                            breakdown.passengerTotals = passengerTotals;
                            passengerTotals.seats = returnSeats;
                            passengerTotals.specialServices = specialServices;
                            if (JsonObjPNRBooking.data.contacts.G != null)
                            {
                                returnTicketBooking.customerNumber = JsonObjPNRBooking.data.contacts.G.customerNumber;
                                returnTicketBooking.companyName = JsonObjPNRBooking.data.contacts.G.companyName;
                                returnTicketBooking.emailAddress = JsonObjPNRBooking.data.contacts.G.emailAddress;
                            }
                            Contacts _contactobj = new Contacts();
                            int PhoneNumberCount = JsonObjPNRBooking.data.contacts.P.phoneNumbers.Count;
                            List<PhoneNumber> phoneNumberList = new List<PhoneNumber>();
                            for (int p = 0; p < PhoneNumberCount; p++)
                            {
                                PhoneNumber phoneobject = new PhoneNumber();
                                phoneobject.number = JsonObjPNRBooking.data.contacts.P.phoneNumbers[p].number;
                                phoneNumberList.Add(phoneobject);
                            }

                            List<JourneysReturn> journeysreturnList = new List<JourneysReturn>();
                            for (int i = 0; i < JourneysReturnCount; i++)
                            {
                                JourneysReturn journeysReturnObj = new JourneysReturn();
                                journeysReturnObj.stops = JsonObjPNRBooking.data.journeys[i].stops;

                                DesignatorReturn ReturnDesignatorobject = new DesignatorReturn();
                                ReturnDesignatorobject.origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
                                ReturnDesignatorobject.destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
                                ReturnDesignatorobject.departure = JsonObjPNRBooking.data.journeys[0].designator.departure;
                                ReturnDesignatorobject.arrival = JsonObjPNRBooking.data.journeys[0].designator.arrival;
                                journeysReturnObj.designator = ReturnDesignatorobject;


                                int SegmentReturnCount = JsonObjPNRBooking.data.journeys[i].segments.Count;
                                List<SegmentReturn> segmentReturnsList = new List<SegmentReturn>();
                                for (int j = 0; j < SegmentReturnCount; j++)
                                {
                                    SegmentReturn segmentReturnobj = new SegmentReturn();
                                    segmentReturnobj.isStandby = JsonObjPNRBooking.data.journeys[i].segments[j].isStandby;
                                    segmentReturnobj.isHosted = JsonObjPNRBooking.data.journeys[i].segments[j].isHosted;


                                    DesignatorReturn designatorReturn = new DesignatorReturn();
                                    //var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
                                    designatorReturn.origin = JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin;
                                    designatorReturn.destination = JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination;
                                    designatorReturn.departure = JsonObjPNRBooking.data.journeys[i].segments[j].designator.departure;
                                    designatorReturn.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].designator.arrival;
                                    segmentReturnobj.designator = designatorReturn;

                                    var passengersegmentCount = JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment;
                                    int passengerReturnCount = ((Newtonsoft.Json.Linq.JContainer)passengersegmentCount).Count;
                                    List<PassengerSegment> passengerSegmentsList = new List<PassengerSegment>();
                                    foreach (var item in JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment)
                                    {
                                        PassengerSegment passengerSegmentobj = new PassengerSegment();
                                        passengerSegmentobj.passengerKey = item.Value.passengerKey;

                                        passengerSegmentsList.Add(passengerSegmentobj);
                                        int seatCount = item.Value.seats.Count;
                                        List<ReturnSeats> returnSeatsList = new List<ReturnSeats>();
                                        for (int q = 0; q < seatCount; q++)
                                        {
                                            ReturnSeats returnSeatsObj = new ReturnSeats();

                                            returnSeatsObj.unitDesignator = item.Value.seats[q].unitDesignator;
                                            //seatnumber = item.Value.seats[q].unitDesignator;
                                            //if (string.IsNullOrEmpty(seatnumber))
                                            //{
                                            //    seatnumber = "0000"; // Set to "0000" if not available
                                            //}
                                            //else
                                            //{
                                            //    seatnumber = seatnumber.PadRight(4, '0'); // Right-pad with zeros if less than 4 characters
                                            //}

                                            returnSeatsList.Add(returnSeatsObj);
                                        }
                                        passengerSegmentobj.seats = returnSeatsList;
                                        //passengerSegmentsList.Add(passengerSegmentobj);
                                    }
                                    segmentReturnobj.passengerSegment = passengerSegmentsList;


                                    //IdentifierReturn identifierReturn = new IdentifierReturn();
                                    //identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                                    //identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                                    //segmentReturnobj.identifier = identifierReturn;

                                    int ReturmFareCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares.Count;
                                    List<FareReturn> fareList = new List<FareReturn>();
                                    for (int k = 0; k < ReturmFareCount; k++)
                                    {
                                        FareReturn fareReturnobj = new FareReturn();
                                        fareReturnobj.productClass = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].productClass;

                                        int PassengerFareReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares.Count;
                                        List<PassengerFareReturn> passengerFareReturnList = new List<PassengerFareReturn>();
                                        for (int l = 0; l < PassengerFareReturnCount; l++)
                                        {
                                            PassengerFareReturn passengerFareReturnobj = new PassengerFareReturn();

                                            int ServiceChargeReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges.Count;

                                            List<ServiceChargeReturn> serviceChargeReturnList = new List<ServiceChargeReturn>();
                                            for (int m = 0; m < ServiceChargeReturnCount; m++)
                                            {
                                                try
                                                {
                                                    ServiceChargeReturn serviceChargeReturnobj = new ServiceChargeReturn();

                                                    serviceChargeReturnobj.amount = JsonObjPNRBooking.data.journeys[i].segments[0].fares[j].passengerFares[k].serviceCharges[l].amount;
                                                    serviceChargeReturnList.Add(serviceChargeReturnobj);
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                            passengerFareReturnobj.serviceCharges = serviceChargeReturnList;
                                            passengerFareReturnList.Add(passengerFareReturnobj);

                                        }
                                        fareReturnobj.passengerFares = passengerFareReturnList;
                                        fareList.Add(fareReturnobj);

                                    }
                                    segmentReturnobj.fares = fareList;

                                    IdentifierReturn identifierReturn = new IdentifierReturn();
                                    identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                                    identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                                    segmentReturnobj.identifier = identifierReturn;

                                    var LegReturn = JsonObjPNRBooking.data.journeys[i].segments[j].legs;
                                    int Legcount = ((Newtonsoft.Json.Linq.JContainer)LegReturn).Count;
                                    List<LegReturn> legReturnsList = new List<LegReturn>();
                                    for (int n = 0; n < Legcount; n++)
                                    {
                                        LegReturn LegReturnobj = new LegReturn();
                                        LegReturnobj.legKey = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legKey;

                                        DesignatorReturn ReturnlegDesignatorobj = new DesignatorReturn();
                                        ReturnlegDesignatorobj.origin = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.origin;
                                        ReturnlegDesignatorobj.destination = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.destination;
                                        ReturnlegDesignatorobj.departure = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.departure;
                                        ReturnlegDesignatorobj.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.arrival;
                                        LegReturnobj.designator = ReturnlegDesignatorobj;

                                        LegInfoReturn legInfoReturn = new LegInfoReturn();
                                        legInfoReturn.arrivalTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
                                        legInfoReturn.arrivalTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
                                        legInfoReturn.departureTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
                                        legInfoReturn.departureTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
                                        LegReturnobj.legInfo = legInfoReturn;
                                        legReturnsList.Add(LegReturnobj);

                                    }
                                    segmentReturnobj.legs = legReturnsList;
                                    segmentReturnsList.Add(segmentReturnobj);

                                }
                                journeysReturnObj.segments = segmentReturnsList;
                                journeysreturnList.Add(journeysReturnObj);
                            }
                            var Returnpassanger = JsonObjPNRBooking.data.passengers;
                            int Returnpassengercount = ((Newtonsoft.Json.Linq.JContainer)Returnpassanger).Count;

                            List<ReturnPassengers> ReturnpassengersList = new List<ReturnPassengers>();
                            foreach (var items in JsonObjPNRBooking.data.passengers)
                            {
                                ReturnPassengers returnPassengersobj = new ReturnPassengers();
                                returnPassengersobj.passengerKey = items.Value.passengerKey;
                                returnPassengersobj.passengerTypeCode = items.Value.passengerTypeCode;
                                //  passkeytypeobj.passengertypecount = items.Count;
                                returnPassengersobj.name = new Name();
                                returnPassengersobj.name.first = items.Value.name.first;
                                ReturnpassengersList.Add(returnPassengersobj);


                            }
                            returnTicketBooking.breakdown = breakdown;
                            returnTicketBooking.journeys = journeysreturnList;
                            returnTicketBooking.passengers = ReturnpassengersList;
                            returnTicketBooking.passengerscount = Returnpassengercount;
                            returnTicketBooking.PhoneNumbers = phoneNumberList;
                            //returnTicketBooking.breakdown = breakdown;
                            //returnTicketBooking.journeys = journeysreturnList;
                            //returnTicketBooking.passengers = ReturnpassengersList;
                            //returnTicketBooking.passengerscount = Returnpassengercount;

                            // HttpContext.Session.SetString("PNRTicketBooking", JsonConvert.SerializeObject(returnTicketBooking));
                            _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                        }
                    }


                    #endregion
                    //}


                    // Spice Jet
                    else if (flagSpicejet == true && data.Airline[k1].ToLower().Contains("spicejet"))
                    {
                        //flagSpicejet = false;
                        #region Spicejet Commit
                        //Spicejet
                        token = string.Empty;
                        if (k1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("SpicejetSignature");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("SpicejetSignatureR");
                        }

                        if (!string.IsNullOrEmpty(tokenview))
                        {
                            _commit objcommit = new _commit();
                            #region GetState
                            GetBookingFromStateResponse _GetBookingFromStateRS1 = null;
                            GetBookingFromStateRequest _GetBookingFromStateRQ1 = null;
                            _GetBookingFromStateRQ1 = new GetBookingFromStateRequest();
                            _GetBookingFromStateRQ1.Signature = tokenview;
                            _GetBookingFromStateRQ1.ContractVersion = 420;


                            SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                            _GetBookingFromStateRS1 = await objSpiceJet.GetBookingFromState(_GetBookingFromStateRQ1);

                            string strdata = JsonConvert.SerializeObject(_GetBookingFromStateRS1);
                            decimal Totalpayment = 0M;
                            if (_GetBookingFromStateRS1 != null)
                            {
                                Totalpayment = _GetBookingFromStateRS1.BookingData.BookingSum.TotalCost;
                            }
                            #endregion
                            #region Addpayment For Api payment deduction
                            //IndigoBookingManager_.AddPaymentToBookingResponse _BookingPaymentResponse = await objcommit.AddpaymenttoBook(token, Totalpayment);

                            #endregion
                            if (tokenview == null) { tokenview = ""; }
                            token = tokenview.Replace(@"""", string.Empty);
                            string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                            List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));
                            string contactdata = HttpContext.Session.GetString("ContactDetails");
                            UpdateContactsRequest contactList = (UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(UpdateContactsRequest));
                            using (HttpClient client1 = new HttpClient())
                            {
                                #region Commit Booking
                                BookingCommitRequest _bookingCommitRequest = new BookingCommitRequest();
                                BookingCommitResponse _BookingCommitResponse = new BookingCommitResponse();
                                _bookingCommitRequest.Signature = token;
                                _bookingCommitRequest.ContractVersion = 420;
                                _bookingCommitRequest.BookingCommitRequestData = new BookingCommitRequestData();
                                _bookingCommitRequest.BookingCommitRequestData.SourcePOS = GetPointOfSale();
                                _bookingCommitRequest.BookingCommitRequestData.CurrencyCode = "INR";
                                _bookingCommitRequest.BookingCommitRequestData.PaxCount = Convert.ToInt16(passeengerlist.Count);
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts = new BookingContact[1];
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0] = new BookingContact();
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].TypeCode = "P";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names = new BookingName[1];
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0] = new BookingName();
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].State = MessageState.New;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].FirstName = passeengerlist[0].first;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].MiddleName = passeengerlist[0].middle;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].LastName = passeengerlist[0].last;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].Title = passeengerlist[0].title;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].EmailAddress = contactList.updateContactsRequestData.BookingContactList[0].EmailAddress; //"vinay.ks@gmail.com"; //passeengerlist.Email;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].HomePhone = "9457000000"; //contactList.updateContactsRequestData.BookingContactList[0].HomePhone; //"9457000000"; //passeengerlist.mobile;
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine1 = "A";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine2 = "B";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].City = "Delhi";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CountryCode = "IN";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CultureCode = "en-GB";
                                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].DistributionOption = DistributionOption.Email;

                                objSpiceJet = new SpiceJetApiController();
                                _BookingCommitResponse = await objSpiceJet.BookingCommit(_bookingCommitRequest);
                                if (_BookingCommitResponse != null && _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator != null)
                                {
                                    string Str3 = JsonConvert.SerializeObject(_BookingCommitResponse);
                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit", "SpiceJetRT");

                                    GetBookingRequest getBookingRequest = new GetBookingRequest();
                                    GetBookingResponse _getBookingResponse = new GetBookingResponse();
                                    getBookingRequest.Signature = token;
                                    getBookingRequest.ContractVersion = 420;
                                    getBookingRequest.GetBookingReqData = new GetBookingRequestData();
                                    getBookingRequest.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                                    getBookingRequest.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                                    getBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator;

                                    _getBookingResponse = await objSpiceJet.GetBookingdetails(getBookingRequest);
                                    string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);

                                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails", "SpiceJetRT");

                                    if (_getBookingResponse != null)
                                    {
                                        Hashtable htseatdata = new Hashtable();
                                        Hashtable htmealdata = new Hashtable();
                                        Hashtable htbagdata = new Hashtable();
                                        int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                                        int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                                        int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                                        int TotalCount = adultcount + childcount;
                                        //string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);
                                        ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                                        var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                                        returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                                        Contacts _contact = new Contacts();
                                        _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[1].HomePhone.ToString();
                                        ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                                        if (_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats != null && _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                        {
                                            _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                                            _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();
                                        }
                                        returnTicketBooking.airLines = "Spicejet";
                                        returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;

                                        Breakdown breakdown = new Breakdown();
                                        List<JourneyTotals> journeyBaseFareobj = new List<JourneyTotals>();
                                        JourneyTotals journeyTotalsobj = new JourneyTotals();

                                        PassengerTotals passengerTotals = new PassengerTotals();
                                        ReturnSeats returnSeats = new ReturnSeats();
                                        passengerTotals.specialServices = new SpecialServices();
                                        var totalTax = "";

                                        #region Itenary segment and legs

                                        int journeyscount = _getBookingResponse.Booking.Journeys.Length;
                                        List<JourneysReturn> AAJourneyList = new List<JourneysReturn>();
                                        for (int i = 0; i < journeyscount; i++)
                                        {

                                            JourneysReturn AAJourneyobj = new JourneysReturn();

                                            //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                                            //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                                            AAJourneyobj.journeyKey = _getBookingResponse.Booking.Journeys[i].JourneySellKey;

                                            int segmentscount = _getBookingResponse.Booking.Journeys[i].Segments.Length;
                                            List<SegmentReturn> AASegmentlist = new List<SegmentReturn>();
                                            for (int j = 0; j < segmentscount; j++)
                                            {
                                                returnSeats.unitDesignator = string.Empty;
                                                returnSeats.SSRCode = string.Empty;
                                                DesignatorReturn AADesignatorobj = new DesignatorReturn();
                                                AADesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[0].DepartureStation;
                                                AADesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].ArrivalStation;
                                                AADesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[0].STD;
                                                AADesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].STA;
                                                AAJourneyobj.designator = AADesignatorobj;


                                                SegmentReturn AASegmentobj = new SegmentReturn();
                                                //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                                                //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                                                DesignatorReturn AASegmentDesignatorobj = new DesignatorReturn();

                                                AASegmentDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                                AASegmentDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                                AASegmentDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                                AASegmentDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                                AASegmentobj.designator = AASegmentDesignatorobj;

                                                int fareCount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares.Length;
                                                List<FareReturn> AAFarelist = new List<FareReturn>();
                                                for (int k = 0; k < fareCount; k++)
                                                {
                                                    FareReturn AAFareobj = new FareReturn();
                                                    AAFareobj.fareKey = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                                    AAFareobj.productClass = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                                    var passengerFares = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                                    int passengerFarescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                                    List<PassengerFareReturn> PassengerfarelistRT = new List<PassengerFareReturn>();
                                                    double AdtAmount = 0.0;
                                                    double AdttaxAmount = 0.0;
                                                    double chdAmount = 0.0;
                                                    double chdtaxAmount = 0.0;
                                                    for (int l = 0; l < passengerFarescount; l++)
                                                    {
                                                        journeyTotalsobj = new JourneyTotals();
                                                        PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                                        AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                                        var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                                        int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                                        List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                                        for (int m = 0; m < serviceChargescount; m++)
                                                        {
                                                            ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                                            AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            string _data = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeType.ToString().ToLower().Trim();
                                                            if (_data == "fareprice")
                                                            {
                                                                journeyTotalsobj.totalAmount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            }
                                                            else
                                                            {
                                                                journeyTotalsobj.totalTax += Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            }
                                                        }
                                                        if (AAPassengerfareobject.passengerType.Equals("ADT"))
                                                        {
                                                            AdtAmount += journeyTotalsobj.totalAmount * adultcount;
                                                            AdttaxAmount += journeyTotalsobj.totalTax * adultcount;
                                                        }

                                                        if (AAPassengerfareobject.passengerType.Equals("CHD"))
                                                        {
                                                            chdAmount += journeyTotalsobj.totalAmount * childcount;
                                                            chdtaxAmount += journeyTotalsobj.totalTax * childcount;
                                                        }
                                                    }
                                                    journeyTotalsobj.totalAmount = AdtAmount + chdAmount;
                                                    journeyTotalsobj.totalTax = AdttaxAmount + chdtaxAmount;
                                                    journeyBaseFareobj.Add(journeyTotalsobj);
                                                    AAFareobj.passengerFares = PassengerfarelistRT;

                                                    AAFarelist.Add(AAFareobj);
                                                }
                                                //breakdown.journeyTotals = journeyTotalsobj;
                                                breakdown.passengerTotals = passengerTotals;
                                                AASegmentobj.fares = AAFarelist;
                                                IdentifierReturn AAIdentifierobj = new IdentifierReturn();

                                                AAIdentifierobj.identifier = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                                AAIdentifierobj.carrierCode = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                                AASegmentobj.identifier = AAIdentifierobj;

                                                var leg = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs;
                                                int legcount = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs.Length;
                                                List<LegReturn> AALeglist = new List<LegReturn>();
                                                for (int n = 0; n < legcount; n++)
                                                {
                                                    LegReturn AALeg = new LegReturn();
                                                    //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                                    DesignatorReturn AAlegDesignatorobj = new DesignatorReturn();
                                                    AAlegDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                                    AAlegDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                                    AAlegDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                                    AAlegDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                                    AALeg.designator = AAlegDesignatorobj;

                                                    LegInfoReturn AALeginfoobj = new LegInfoReturn();
                                                    AALeginfoobj.arrivalTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                                    AALeginfoobj.arrivalTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                                    AALeginfoobj.departureTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                                    AALeginfoobj.departureTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
                                                    AALeg.legInfo = AALeginfoobj;
                                                    AALeglist.Add(AALeg);

                                                }
                                                //vivek
                                                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                                {
                                                    try
                                                    {
                                                        if (!htseatdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            htseatdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.UnitDesignator);
                                                            returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                //SSR
                                                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                                {
                                                    try
                                                    {
                                                        //if (!htmealdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        //{
                                                        //    htmealdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                                        //    returnSeats.SSRCode += item1.SSRCode + ",";
                                                        //}
                                                        if (!htmealdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            if (item1.SSRCode != "INFT" && !item1.SSRCode.StartsWith("E", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                htmealdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                                            }
                                                            returnSeats.SSRCode += item1.SSRCode + ",";
                                                        }

                                                        if (!htbagdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            if (item1.SSRCode != "INFT" && item1.SSRCode.StartsWith("E", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                htbagdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                                            }
                                                            returnSeats.SSRCode += item1.SSRCode + ",";
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                //vivek
                                                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                                //{
                                                //    returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                                //}

                                                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                                //{
                                                //    returnSeats.SSRCode += item1.SSRCode + ",";
                                                //}
                                                //
                                                AASegmentobj.unitdesignator = returnSeats.unitDesignator;
                                                AASegmentobj.SSRCode = returnSeats.SSRCode;
                                                AASegmentobj.legs = AALeglist;
                                                AASegmentlist.Add(AASegmentobj);
                                                breakdown.journeyfareTotals = journeyBaseFareobj;
                                            }
                                            AAJourneyobj.segments = AASegmentlist;
                                            AAJourneyList.Add(AAJourneyobj);

                                        }

                                        #endregion
                                        string stravailibitilityrequest = HttpContext.Session.GetString("SpicejetAvailibilityRequest");
                                        GetAvailabilityRequest availibiltyRQ = JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);
                                        var passanger = _getBookingResponse.Booking.Passengers;
                                        int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;
                                        ReturnPassengers passkeytypeobj = new ReturnPassengers();
                                        List<ReturnPassengers> passkeylist = new List<ReturnPassengers>();


                                        foreach (var item in _getBookingResponse.Booking.Passengers)
                                        {
                                            foreach (var item1 in item.PassengerFees)
                                            {
                                                if (item1.FeeCode.Equals("SEAT"))
                                                {
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if (item2.ChargeCode.Equals("SEAT"))
                                                        {
                                                            returnSeats.total += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            returnSeats.taxes += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                                        }
                                                    }

                                                }
                                                else if (item1.FeeCode.Equals("INFT"))
                                                {
                                                    JourneyTotals InfantfareTotals = new JourneyTotals();
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if (item2.ChargeCode.Equals("INFT"))
                                                        {
                                                            InfantfareTotals.totalAmount = Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            InfantfareTotals.totalTax += 0;// Convert.ToInt32(item2.Amount);
                                                        }
                                                    }
                                                    journeyBaseFareobj.Add(InfantfareTotals);
                                                    breakdown.journeyfareTotals = journeyBaseFareobj;
                                                }
                                                else
                                                {
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if ((!item2.ChargeCode.Equals("SEAT") || !item2.ChargeCode.Equals("INFT")) && !item2.ChargeType.ToString().ToLower().Contains("tax"))
                                                        {
                                                            passengerTotals.specialServices.total += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            passengerTotals.specialServices.taxes += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                                        }
                                                    }
                                                }

                                            }
                                            passkeytypeobj = new ReturnPassengers();
                                            passkeytypeobj.name = new Name();
                                            passkeytypeobj.passengerTypeCode = item.PassengerTypeInfo.PaxType;
                                            passkeytypeobj.name.first = item.Names[0].FirstName + " " + item.Names[0].LastName;
                                            for (int i = 0; i < passeengerlist.Count; i++)
                                            {
                                                if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                                {
                                                    passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                                    passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                                    //passkeytypeobj.seats.unitDesignator = htseatdata[passeengerlist[i].passengerkey].ToString();
                                                    break;
                                                }

                                            }
                                            passkeylist.Add(passkeytypeobj);
                                            if (item.Infant != null)
                                            {
                                                passkeytypeobj = new ReturnPassengers();
                                                passkeytypeobj.name = new Name();
                                                passkeytypeobj.passengerTypeCode = "INFT";
                                                passkeytypeobj.name.first = item.Infant.Names[0].FirstName + " " + item.Infant.Names[0].LastName;
                                                for (int i = 0; i < passeengerlist.Count; i++)
                                                {
                                                    if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                                    {
                                                        passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                                        passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                                        break;
                                                    }

                                                }
                                                //passkeytypeobj.MobNumber = "";
                                                passkeylist.Add(passkeytypeobj);

                                            }
                                        }
                                        double BasefareAmt = 0.0;
                                        double BasefareTax = 0.0;
                                        for (int i = 0; i < breakdown.journeyfareTotals.Count; i++)
                                        {
                                            BasefareAmt += breakdown.journeyfareTotals[i].totalAmount;
                                            BasefareTax += breakdown.journeyfareTotals[i].totalTax;
                                        }
                                        breakdown.journeyTotals = new JourneyTotals();
                                        breakdown.journeyTotals.totalAmount = Convert.ToDouble(BasefareAmt);
                                        breakdown.passengerTotals.seats = new ReturnSeats();
                                        //breakdown.passengerTotals.specialServices = new SpecialServices();
                                        breakdown.passengerTotals.specialServices.total = passengerTotals.specialServices.total;
                                        breakdown.passengerTotals.seats.total = returnSeats.total;
                                        breakdown.passengerTotals.seats.taxes = returnSeats.taxes;
                                        breakdown.journeyTotals.totalTax = Convert.ToDouble(BasefareTax);
                                        breakdown.totalAmount = breakdown.journeyTotals.totalAmount + breakdown.journeyTotals.totalTax;
                                        if (totalAmount != 0M)
                                        {
                                            breakdown.totalToCollect = Convert.ToDouble(totalAmount);
                                        }
                                        returnTicketBooking.breakdown = breakdown;
                                        returnTicketBooking.journeys = AAJourneyList;
                                        returnTicketBooking.passengers = passkeylist;
                                        returnTicketBooking.passengerscount = passengercount;
                                        returnTicketBooking.contacts = _contact;
                                        returnTicketBooking.Seatdata = htseatdata;
                                        returnTicketBooking.Mealdata = htmealdata;
                                        returnTicketBooking.Bagdata = htbagdata;
                                        returnTicketBooking.bookingdate = _getBookingResponse.Booking.BookingInfo.BookingDate;
                                        _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else if (flagIndigo == true && data.Airline[k1].ToLower().Contains("indigo"))
                    {
                        //flagIndigo = false;
                        #region Indigo Commit
                        //Spicejet
                        token = string.Empty;
                        if (k1 == 0)
                        {
                            tokenview = HttpContext.Session.GetString("IndigoSignature");
                        }
                        else
                        {
                            tokenview = HttpContext.Session.GetString("IndigoSignatureR");
                        }
                        if (!string.IsNullOrEmpty(tokenview))
                        {
                            if (tokenview == null) { tokenview = ""; }
                            token = tokenview.Replace(@"""", string.Empty);
                            string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                            List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));
                            string contactdata = HttpContext.Session.GetString("ContactDetails");
                            IndigoBookingManager_.UpdateContactsRequest contactList = (IndigoBookingManager_.UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(IndigoBookingManager_.UpdateContactsRequest));
                            using (HttpClient client1 = new HttpClient())
                            {
                                _commit objcommit = new _commit();
                                #region GetState
                                _sell objsell = new _sell();
                                IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS1 = await objsell.GetBookingFromState(token, "");

                                string strdata = JsonConvert.SerializeObject(_GetBookingFromStateRS1);
                                decimal Totalpayment = 0M;
                                if (_GetBookingFromStateRS1 != null)
                                {
                                    Totalpayment = _GetBookingFromStateRS1.BookingData.BookingSum.TotalCost;
                                }
                                #endregion
                                #region Addpayment For Api payment deduction
                                //IndigoBookingManager_.AddPaymentToBookingResponse _BookingPaymentResponse = await objcommit.AddpaymenttoBook(token, Totalpayment);

                                #endregion
                                #region Commit Booking


                                IndigoBookingManager_.BookingCommitResponse _BookingCommitResponse = await objcommit.commit(token, contactList, passeengerlist);

                                #region old code
                                //if (_BookingCommitResponse != null && _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator != null)
                                //{
                                //    IndigoBookingManager_.GetBookingResponse _getBookingResponse = await objcommit.GetBookingdetails(token, _BookingCommitResponse);

                                //    if (_getBookingResponse != null)
                                //    {
                                //        Hashtable htseatdata = new Hashtable();
                                //        Hashtable htmealdata = new Hashtable();
                                //        int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                                //        int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                                //        int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                                //        int TotalCount = adultcount + childcount;
                                //        string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);
                                //        //string fileName = @"D:\a.txt";
                                //        //try
                                //        //{
                                //        //    using (StreamReader reader = new StreamReader(fileName))
                                //        //    {
                                //        //        _responceGetBooking = reader.ReadToEnd();
                                //        //    }
                                //        //}
                                //        //catch (Exception exp)
                                //        //{
                                //        //    //Console.WriteLine(exp.Message);
                                //        //}
                                //        //_getBookingResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<IndigoBookingManager_.GetBookingResponse>(_responceGetBooking);
                                //        ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                                //        var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                                //        returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                                //        Contacts _contact = new Contacts();
                                //        _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[1].HomePhone.ToString();
                                //        ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                                //        if (_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats != null && _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                //        {
                                //            _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                                //            _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();
                                //        }

                                //        returnTicketBooking.airLines = "Indigo";
                                //        returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;

                                //        Breakdown breakdown = new Breakdown();
                                //        List<JourneyTotals> journeyBaseFareobj = new List<JourneyTotals>();
                                //        JourneyTotals journeyTotalsobj = new JourneyTotals();

                                //        PassengerTotals passengerTotals = new PassengerTotals();
                                //        ReturnSeats returnSeats = new ReturnSeats();
                                //        passengerTotals.specialServices = new SpecialServices();
                                //        var totalTax = "";// _getPri

                                //        #region Itenary segment and legs

                                //        int journeyscount = _getBookingResponse.Booking.Journeys.Length;
                                //        List<JourneysReturn> AAJourneyList = new List<JourneysReturn>();
                                //        for (int i = 0; i < journeyscount; i++)
                                //        {

                                //            JourneysReturn AAJourneyobj = new JourneysReturn();

                                //            //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                                //            //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                                //            AAJourneyobj.journeyKey = _getBookingResponse.Booking.Journeys[i].JourneySellKey;

                                //            int segmentscount = _getBookingResponse.Booking.Journeys[i].Segments.Length;
                                //            List<SegmentReturn> AASegmentlist = new List<SegmentReturn>();
                                //            for (int j = 0; j < segmentscount; j++)
                                //            {
                                //                returnSeats.unitDesignator = string.Empty;
                                //                returnSeats.SSRCode = string.Empty;
                                //                DesignatorReturn AADesignatorobj = new DesignatorReturn();
                                //                AADesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[0].DepartureStation;
                                //                AADesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].ArrivalStation;
                                //                AADesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[0].STD;
                                //                AADesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].STA;
                                //                AAJourneyobj.designator = AADesignatorobj;


                                //                SegmentReturn AASegmentobj = new SegmentReturn();
                                //                //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                                //                //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                                //                DesignatorReturn AASegmentDesignatorobj = new DesignatorReturn();

                                //                AASegmentDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                //                AASegmentDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                //                AASegmentDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                //                AASegmentDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                //                AASegmentobj.designator = AASegmentDesignatorobj;

                                //                int fareCount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares.Length;
                                //                List<FareReturn> AAFarelist = new List<FareReturn>();
                                //                for (int k = 0; k < fareCount; k++)
                                //                {
                                //                    FareReturn AAFareobj = new FareReturn();
                                //                    AAFareobj.fareKey = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                //                    AAFareobj.productClass = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                //                    var passengerFares = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                //                    int passengerFarescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                //                    List<PassengerFareReturn> PassengerfarelistRT = new List<PassengerFareReturn>();
                                //                    double AdtAmount = 0.0;
                                //                    double AdttaxAmount = 0.0;
                                //                    double chdAmount = 0.0;
                                //                    double chdtaxAmount = 0.0;
                                //                    for (int l = 0; l < passengerFarescount; l++)
                                //                    {
                                //                        journeyTotalsobj = new JourneyTotals();
                                //                        PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                //                        AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                //                        var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                //                        int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                //                        List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                //                        for (int m = 0; m < serviceChargescount; m++)
                                //                        {
                                //                            ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                //                            AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                //                            string _data = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeType.ToString().ToLower().Trim();
                                //                            if (_data == "fareprice")
                                //                            {
                                //                                journeyTotalsobj.totalAmount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                //                            }
                                //                            else
                                //                            {
                                //                                journeyTotalsobj.totalTax += Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                //                            }
                                //                        }

                                //                        if (AAPassengerfareobject.passengerType.Equals("ADT"))
                                //                        {
                                //                            AdtAmount += journeyTotalsobj.totalAmount * adultcount;
                                //                            AdttaxAmount += journeyTotalsobj.totalTax * adultcount;
                                //                        }

                                //                        if (AAPassengerfareobject.passengerType.Equals("CHD"))
                                //                        {
                                //                            chdAmount += journeyTotalsobj.totalAmount * childcount;
                                //                            chdtaxAmount += journeyTotalsobj.totalTax * childcount;
                                //                        }

                                //                    }
                                //                    journeyTotalsobj.totalAmount = AdtAmount + chdAmount;
                                //                    journeyTotalsobj.totalTax = AdttaxAmount + chdtaxAmount;
                                //                    journeyBaseFareobj.Add(journeyTotalsobj);
                                //                    AAFareobj.passengerFares = PassengerfarelistRT;

                                //                    AAFarelist.Add(AAFareobj);
                                //                }
                                //                //breakdown.journeyTotals = journeyTotalsobj;
                                //                breakdown.passengerTotals = passengerTotals;
                                //                AASegmentobj.fares = AAFarelist;
                                //                IdentifierReturn AAIdentifierobj = new IdentifierReturn();

                                //                AAIdentifierobj.identifier = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                //                AAIdentifierobj.carrierCode = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                //                AASegmentobj.identifier = AAIdentifierobj;

                                //                var leg = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs;
                                //                int legcount = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs.Length;
                                //                List<LegReturn> AALeglist = new List<LegReturn>();
                                //                for (int n = 0; n < legcount; n++)
                                //                {
                                //                    LegReturn AALeg = new LegReturn();
                                //                    //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                //                    DesignatorReturn AAlegDesignatorobj = new DesignatorReturn();
                                //                    AAlegDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                //                    AAlegDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                //                    AAlegDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                //                    AAlegDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                //                    AALeg.designator = AAlegDesignatorobj;

                                //                    LegInfoReturn AALeginfoobj = new LegInfoReturn();
                                //                    AALeginfoobj.arrivalTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                //                    AALeginfoobj.arrivalTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                //                    AALeginfoobj.departureTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                //                    AALeginfoobj.departureTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
                                //                    AALeg.legInfo = AALeginfoobj;
                                //                    AALeglist.Add(AALeg);



                                //                }
                                //                //vivek
                                //                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                //                {
                                //                    try
                                //                    {
                                //                        if (!htseatdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                //                        {
                                //                            htseatdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.UnitDesignator);
                                //                            returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                //                        }
                                //                    }
                                //                    catch (Exception ex)
                                //                    {

                                //                    }
                                //                }
                                //                //SSR
                                //                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                //                {
                                //                    try
                                //                    {
                                //                        if (!htmealdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                //                        {
                                //                            htmealdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                //                            returnSeats.SSRCode += item1.SSRCode + ",";
                                //                        }
                                //                    }
                                //                    catch (Exception ex)
                                //                    {

                                //                    }
                                //                }

                                //                ////vivek
                                //                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                //                //{
                                //                //    htseatdata.Add(item1.PassengerNumber.ToString()+ _getBookingResponse.Booking.Journeys[i].Segments[j], item1.UnitDesignator);
                                //                //    returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                //                //}

                                //                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                //                //{
                                //                //    returnSeats.SSRCode += item1.SSRCode + ",";
                                //                //}
                                //                //
                                //                AASegmentobj.unitdesignator = returnSeats.unitDesignator;
                                //                AASegmentobj.SSRCode = returnSeats.SSRCode;
                                //                AASegmentobj.legs = AALeglist;
                                //                AASegmentlist.Add(AASegmentobj);
                                //                breakdown.journeyfareTotals = journeyBaseFareobj;
                                //            }

                                //            AAJourneyobj.segments = AASegmentlist;
                                //            AAJourneyList.Add(AAJourneyobj);

                                //        }

                                //        #endregion

                                //        string stravailibitilityrequest = HttpContext.Session.GetString("IndigoAvailibilityRequest");
                                //        GetAvailabilityRequest availibiltyRQ = JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                                //        var passanger = _getBookingResponse.Booking.Passengers;
                                //        int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;
                                //        ReturnPassengers passkeytypeobj = new ReturnPassengers();
                                //        List<ReturnPassengers> passkeylist = new List<ReturnPassengers>();
                                //        foreach (var item in _getBookingResponse.Booking.Passengers)
                                //        {
                                //            foreach (var item1 in item.PassengerFees)
                                //            {
                                //                if (item1.FeeCode.Equals("SEAT"))
                                //                {
                                //                    foreach (var item2 in item1.ServiceCharges)
                                //                    {
                                //                        if (item2.ChargeCode.Equals("SEAT"))
                                //                        {
                                //                            returnSeats.total += Convert.ToInt32(item2.Amount);
                                //                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                //                        }
                                //                        else
                                //                        {
                                //                            returnSeats.taxes += Convert.ToInt32(item2.Amount);
                                //                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                //                        }
                                //                    }
                                //                }
                                //                else if (item1.FeeCode.Equals("INFT"))
                                //                {
                                //                    JourneyTotals InfantfareTotals = new JourneyTotals();
                                //                    foreach (var item2 in item1.ServiceCharges)
                                //                    {
                                //                        if (item2.ChargeCode.Equals("INFT"))
                                //                        {
                                //                            InfantfareTotals.totalAmount = Convert.ToInt32(item2.Amount);
                                //                        }
                                //                        else
                                //                        {
                                //                            InfantfareTotals.totalTax += 0;// Convert.ToInt32(item2.Amount);
                                //                        }
                                //                    }
                                //                    journeyBaseFareobj.Add(InfantfareTotals);
                                //                    breakdown.journeyfareTotals = journeyBaseFareobj;
                                //                }
                                //                else
                                //                {
                                //                    foreach (var item2 in item1.ServiceCharges)
                                //                    {
                                //                        if ((!item2.ChargeCode.Equals("SEAT") || !item2.ChargeCode.Equals("INFT")) && !item2.ChargeType.ToString().ToLower().Contains("tax"))
                                //                        {
                                //                            passengerTotals.specialServices.total += Convert.ToInt32(item2.Amount);
                                //                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                //                        }
                                //                        else
                                //                        {
                                //                            passengerTotals.specialServices.taxes += Convert.ToInt32(item2.Amount);
                                //                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //            passkeytypeobj = new ReturnPassengers();
                                //            passkeytypeobj.name = new Name();
                                //            passkeytypeobj.passengerTypeCode = item.PassengerTypeInfo.PaxType;
                                //            passkeytypeobj.name.first = item.Names[0].FirstName + " " + item.Names[0].LastName;
                                //            for (int i = 0; i < passeengerlist.Count; i++)
                                //            {
                                //                if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                //                {
                                //                    passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                //                    passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                //                    //passkeytypeobj.seats.unitDesignator = htseatdata[passeengerlist[i].passengerkey].ToString();
                                //                    break;
                                //                }

                                //            }



                                //            passkeylist.Add(passkeytypeobj);
                                //            if (item.Infant != null)
                                //            {
                                //                passkeytypeobj = new ReturnPassengers();
                                //                passkeytypeobj.name = new Name();
                                //                passkeytypeobj.passengerTypeCode = "INFT";
                                //                passkeytypeobj.name.first = item.Infant.Names[0].FirstName + " " + item.Infant.Names[0].LastName;
                                //                for (int i = 0; i < passeengerlist.Count; i++)
                                //                {
                                //                    if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                //                    {
                                //                        passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                //                        passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                //                        break;
                                //                    }

                                //                }
                                //                //passkeytypeobj.MobNumber = "";
                                //                passkeylist.Add(passkeytypeobj);

                                //            }
                                //        }

                                //        double BasefareAmt = 0.0;
                                //        double BasefareTax = 0.0;
                                //        for (int i = 0; i < breakdown.journeyfareTotals.Count; i++)
                                //        {
                                //            BasefareAmt += breakdown.journeyfareTotals[i].totalAmount;
                                //            BasefareTax += breakdown.journeyfareTotals[i].totalTax;
                                //        }
                                //        breakdown.journeyTotals = new JourneyTotals();
                                //        breakdown.journeyTotals.totalAmount = Convert.ToDouble(BasefareAmt);
                                //        breakdown.passengerTotals.seats = new ReturnSeats();
                                //        //breakdown.passengerTotals.specialServices = new SpecialServices();
                                //        breakdown.passengerTotals.specialServices.total = passengerTotals.specialServices.total;
                                //        breakdown.passengerTotals.seats.total = returnSeats.total;
                                //        breakdown.passengerTotals.seats.taxes = returnSeats.taxes;
                                //        breakdown.journeyTotals.totalTax = Convert.ToDouble(BasefareTax);
                                //        breakdown.totalAmount = breakdown.journeyTotals.totalAmount + breakdown.journeyTotals.totalTax;
                                //        if (totalAmount != 0M)
                                //        {
                                //            breakdown.totalToCollect = Convert.ToDouble(totalAmount);
                                //        }
                                //        returnTicketBooking.breakdown = breakdown;
                                //        returnTicketBooking.journeys = AAJourneyList;
                                //        returnTicketBooking.passengers = passkeylist;
                                //        returnTicketBooking.passengerscount = passengercount;
                                //        returnTicketBooking.contacts = _contact;
                                //        returnTicketBooking.Seatdata = htseatdata;
                                //        returnTicketBooking.Mealdata = htmealdata;
                                //        _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                                //    }
                                //}
                                #endregion
                                if (_BookingCommitResponse != null && _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator != null)
                                {
                                    IndigoBookingManager_.GetBookingResponse _getBookingResponse = await objcommit.GetBookingdetails(token, _BookingCommitResponse);

                                    if (_getBookingResponse != null)
                                    {
                                        Hashtable htseatdata = new Hashtable();
                                        Hashtable htmealdata = new Hashtable();
                                        Hashtable htbagdata = new Hashtable();
                                        int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                                        int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                                        int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                                        int TotalCount = adultcount + childcount;
                                        string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);
                                        //string fileName = @"D:\a.txt";
                                        //try
                                        //{
                                        //    using (StreamReader reader = new StreamReader(fileName))
                                        //    {
                                        //        _responceGetBooking = reader.ReadToEnd();
                                        //    }
                                        //}
                                        //catch (Exception exp)
                                        //{
                                        //    //Console.WriteLine(exp.Message);
                                        //}
                                        //_getBookingResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<IndigoBookingManager_.GetBookingResponse>(_responceGetBooking);
                                        ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                                        var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                                        returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();

                                        ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                                        if (_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats != null && _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                            _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                                        //GST Number
                                        if (_getBookingResponse.Booking.BookingContacts[0].TypeCode == "I")
                                        {
                                            returnTicketBooking.customerNumber = _getBookingResponse.Booking.BookingContacts[0].CustomerNumber;
                                            returnTicketBooking.companyName = _getBookingResponse.Booking.BookingContacts[0].CompanyName;
                                        }

                                        Contacts _contact = new Contacts();
                                        _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[0].HomePhone.ToString();
                                        if (_unitdesinator.unitDesignatorPax != null)
                                            _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();


                                        returnTicketBooking.airLines = "Indigo";
                                        returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;

                                        Breakdown breakdown = new Breakdown();
                                        List<JourneyTotals> journeyBaseFareobj = new List<JourneyTotals>();
                                        JourneyTotals journeyTotalsobj = new JourneyTotals();

                                        PassengerTotals passengerTotals = new PassengerTotals();
                                        ReturnSeats returnSeats = new ReturnSeats();
                                        passengerTotals.specialServices = new SpecialServices();
                                        var totalTax = "";// _getPri

                                        #region Itenary segment and legs

                                        int journeyscount = _getBookingResponse.Booking.Journeys.Length;
                                        List<JourneysReturn> AAJourneyList = new List<JourneysReturn>();
                                        for (int i = 0; i < journeyscount; i++)
                                        {

                                            JourneysReturn AAJourneyobj = new JourneysReturn();

                                            //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                                            //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                                            AAJourneyobj.journeyKey = _getBookingResponse.Booking.Journeys[i].JourneySellKey;

                                            int segmentscount = _getBookingResponse.Booking.Journeys[i].Segments.Length;
                                            List<SegmentReturn> AASegmentlist = new List<SegmentReturn>();
                                            for (int j = 0; j < segmentscount; j++)
                                            {
                                                returnSeats.unitDesignator = string.Empty;
                                                returnSeats.SSRCode = string.Empty;
                                                DesignatorReturn AADesignatorobj = new DesignatorReturn();
                                                AADesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                                AADesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                                AADesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                                AADesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                                AAJourneyobj.designator = AADesignatorobj;


                                                SegmentReturn AASegmentobj = new SegmentReturn();
                                                //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                                                //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                                                DesignatorReturn AASegmentDesignatorobj = new DesignatorReturn();

                                                AASegmentDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                                AASegmentDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                                AASegmentDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                                AASegmentDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                                AASegmentobj.designator = AASegmentDesignatorobj;

                                                int fareCount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares.Length;
                                                List<FareReturn> AAFarelist = new List<FareReturn>();
                                                for (int k = 0; k < fareCount; k++)
                                                {
                                                    FareReturn AAFareobj = new FareReturn();
                                                    AAFareobj.fareKey = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                                    AAFareobj.productClass = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                                    var passengerFares = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                                    int passengerFarescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                                    List<PassengerFareReturn> PassengerfarelistRT = new List<PassengerFareReturn>();
                                                    double AdtAmount = 0.0;
                                                    double AdttaxAmount = 0.0;
                                                    double chdAmount = 0.0;
                                                    double chdtaxAmount = 0.0;
                                                    for (int l = 0; l < passengerFarescount; l++)
                                                    {
                                                        journeyTotalsobj = new JourneyTotals();
                                                        PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                                        AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                                        var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                                        int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                                        List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                                        for (int m = 0; m < serviceChargescount; m++)
                                                        {
                                                            ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                                            AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            string _data = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeType.ToString().ToLower().Trim();
                                                            if (_data == "fareprice")
                                                            {
                                                                journeyTotalsobj.totalAmount += Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            }
                                                            else
                                                            {
                                                                journeyTotalsobj.totalTax += Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                            }


                                                            AAServicechargelist.Add(AAServicechargeobj);
                                                        }

                                                        if (AAPassengerfareobject.passengerType.Equals("ADT"))
                                                        {
                                                            AdtAmount += journeyTotalsobj.totalAmount * adultcount;
                                                            AdttaxAmount += journeyTotalsobj.totalTax * adultcount;
                                                        }

                                                        if (AAPassengerfareobject.passengerType.Equals("CHD"))
                                                        {
                                                            chdAmount += journeyTotalsobj.totalAmount * childcount;
                                                            chdtaxAmount += journeyTotalsobj.totalTax * childcount;
                                                        }


                                                        AAPassengerfareobject.serviceCharges = AAServicechargelist;
                                                        PassengerfarelistRT.Add(AAPassengerfareobject);

                                                    }
                                                    journeyTotalsobj.totalAmount = AdtAmount + chdAmount;
                                                    journeyTotalsobj.totalTax = AdttaxAmount + chdtaxAmount;
                                                    journeyBaseFareobj.Add(journeyTotalsobj);
                                                    AAFareobj.passengerFares = PassengerfarelistRT;

                                                    AAFarelist.Add(AAFareobj);
                                                }
                                                //breakdown.journeyTotals = journeyTotalsobj;
                                                breakdown.passengerTotals = passengerTotals;
                                                AASegmentobj.fares = AAFarelist;
                                                IdentifierReturn AAIdentifierobj = new IdentifierReturn();

                                                AAIdentifierobj.identifier = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                                AAIdentifierobj.carrierCode = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                                AASegmentobj.identifier = AAIdentifierobj;

                                                var leg = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs;
                                                int legcount = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs.Length;
                                                List<LegReturn> AALeglist = new List<LegReturn>();
                                                for (int n = 0; n < legcount; n++)
                                                {
                                                    LegReturn AALeg = new LegReturn();
                                                    //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                                    DesignatorReturn AAlegDesignatorobj = new DesignatorReturn();
                                                    AAlegDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                                    AAlegDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                                    AAlegDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                                    AAlegDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                                    AALeg.designator = AAlegDesignatorobj;

                                                    LegInfoReturn AALeginfoobj = new LegInfoReturn();
                                                    AALeginfoobj.arrivalTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                                    AALeginfoobj.arrivalTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                                    AALeginfoobj.departureTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                                    AALeginfoobj.departureTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
                                                    AALeg.legInfo = AALeginfoobj;
                                                    AALeglist.Add(AALeg);

                                                }


                                                //vivek
                                                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                                {
                                                    try
                                                    {
                                                        if (!htseatdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            htseatdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.UnitDesignator);
                                                            returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                //SSR
                                                foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                                {
                                                    try
                                                    {
                                                        if (!htmealdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            if (item1.SSRCode != "INFT" && item1.SSRCode != "FFWD")
                                                            {
                                                                htmealdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                                            }
                                                            returnSeats.SSRCode += item1.SSRCode + ",";
                                                        }

                                                        else if (!htbagdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                                        {
                                                            if (item1.SSRCode != "INFT" && item1.SSRCode != "FFWD")
                                                            {
                                                                htbagdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.SSRCode);
                                                            }
                                                            returnSeats.SSRCode += item1.SSRCode + ",";


                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }

                                                ////vivek
                                                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                                //{
                                                //    htseatdata.Add(item1.PassengerNumber.ToString()+ _getBookingResponse.Booking.Journeys[i].Segments[j], item1.UnitDesignator);
                                                //    returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                                //}

                                                //foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                                //{
                                                //    returnSeats.SSRCode += item1.SSRCode + ",";
                                                //}
                                                //
                                                AASegmentobj.unitdesignator = returnSeats.unitDesignator;
                                                AASegmentobj.SSRCode = returnSeats.SSRCode;
                                                AASegmentobj.legs = AALeglist;
                                                AASegmentlist.Add(AASegmentobj);
                                                breakdown.journeyfareTotals = journeyBaseFareobj;
                                            }

                                            AAJourneyobj.segments = AASegmentlist;
                                            AAJourneyList.Add(AAJourneyobj);

                                        }

                                        #endregion

                                        string stravailibitilityrequest = HttpContext.Session.GetString("IndigoAvailibilityRequest");
                                        GetAvailabilityRequest availibiltyRQ = JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                                        var passanger = _getBookingResponse.Booking.Passengers;
                                        int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;
                                        ReturnPassengers passkeytypeobj = new ReturnPassengers();
                                        List<ReturnPassengers> passkeylist = new List<ReturnPassengers>();
                                        foreach (var item in _getBookingResponse.Booking.Passengers)
                                        {
                                            foreach (var item1 in item.PassengerFees)
                                            {
                                                if (item1.FeeCode.Equals("SEAT"))
                                                {
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if (item2.ChargeCode.Equals("SEAT"))
                                                        {
                                                            returnSeats.total += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            returnSeats.taxes += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                                        }
                                                    }
                                                }
                                                else if (item1.FeeCode.Equals("INFT"))
                                                {
                                                    JourneyTotals InfantfareTotals = new JourneyTotals();
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if (item2.ChargeCode.Equals("INFT"))
                                                        {
                                                            InfantfareTotals.totalAmount = Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            InfantfareTotals.totalTax += 0;// Convert.ToInt32(item2.Amount);
                                                        }
                                                    }
                                                    journeyBaseFareobj.Add(InfantfareTotals);
                                                    breakdown.journeyfareTotals = journeyBaseFareobj;
                                                }
                                                else
                                                {
                                                    foreach (var item2 in item1.ServiceCharges)
                                                    {
                                                        if ((!item2.ChargeCode.Equals("SEAT") || !item2.ChargeCode.Equals("INFT")) && !item2.ChargeType.ToString().ToLower().Contains("tax"))
                                                        {
                                                            passengerTotals.specialServices.total += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                        }
                                                        else
                                                        {
                                                            passengerTotals.specialServices.taxes += Convert.ToInt32(item2.Amount);
                                                            //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                                        }
                                                    }
                                                }
                                            }
                                            passkeytypeobj = new ReturnPassengers();
                                            passkeytypeobj.name = new Name();
                                            passkeytypeobj.passengerTypeCode = item.PassengerTypeInfo.PaxType;
                                            passkeytypeobj.name.first = item.Names[0].FirstName + " " + item.Names[0].LastName;
                                            for (int i = 0; i < passeengerlist.Count; i++)
                                            {
                                                if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                                {
                                                    passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                                    passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                                    //passkeytypeobj.seats.unitDesignator = htseatdata[passeengerlist[i].passengerkey].ToString();
                                                    break;
                                                }

                                            }



                                            passkeylist.Add(passkeytypeobj);
                                            if (item.Infant != null)
                                            {
                                                passkeytypeobj = new ReturnPassengers();
                                                passkeytypeobj.name = new Name();
                                                passkeytypeobj.passengerTypeCode = "INFT";
                                                passkeytypeobj.name.first = item.Infant.Names[0].FirstName + " " + item.Infant.Names[0].LastName;
                                                for (int i = 0; i < passeengerlist.Count; i++)
                                                {
                                                    if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() + " " + passeengerlist[i].last.ToLower())
                                                    {
                                                        passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                                        passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                                        break;
                                                    }

                                                }
                                                //passkeytypeobj.MobNumber = "";
                                                passkeylist.Add(passkeytypeobj);

                                            }
                                        }

                                        double BasefareAmt = 0.0;
                                        double BasefareTax = 0.0;
                                        for (int i = 0; i < breakdown.journeyfareTotals.Count; i++)
                                        {
                                            BasefareAmt += breakdown.journeyfareTotals[i].totalAmount;
                                            BasefareTax += breakdown.journeyfareTotals[i].totalTax;
                                        }
                                        breakdown.journeyTotals = new JourneyTotals();
                                        breakdown.journeyTotals.totalAmount = Convert.ToDouble(BasefareAmt);
                                        breakdown.passengerTotals.seats = new ReturnSeats();
                                        //breakdown.passengerTotals.specialServices = new SpecialServices();
                                        breakdown.passengerTotals.specialServices.total = passengerTotals.specialServices.total;
                                        breakdown.passengerTotals.seats.total = returnSeats.total;
                                        breakdown.passengerTotals.seats.taxes = returnSeats.taxes;
                                        breakdown.journeyTotals.totalTax = Convert.ToDouble(BasefareTax);
                                        breakdown.totalAmount = breakdown.journeyTotals.totalAmount + breakdown.journeyTotals.totalTax;
                                        if (totalAmount != 0M)
                                        {
                                            breakdown.totalToCollect = Convert.ToDouble(totalAmount);
                                        }
                                        returnTicketBooking.breakdown = breakdown;
                                        returnTicketBooking.journeys = AAJourneyList;
                                        returnTicketBooking.passengers = passkeylist;
                                        returnTicketBooking.passengerscount = passengercount;
                                        returnTicketBooking.contacts = _contact;
                                        returnTicketBooking.Seatdata = htseatdata;
                                        returnTicketBooking.Mealdata = htmealdata;
                                        returnTicketBooking.Bagdata = htbagdata;
                                        returnTicketBooking.bookingdate = _getBookingResponse.Booking.BookingInfo.BookingDate;
                                        _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                                    }
                                }
                                #endregion
                                //LogOut 
                                IndigoSessionmanager_.LogoutRequest _logoutRequestobj = new IndigoSessionmanager_.LogoutRequest();
                                IndigoSessionmanager_.LogoutResponse _logoutResponse = new IndigoSessionmanager_.LogoutResponse();
                                _logoutRequestobj.ContractVersion = 456;
                                _logoutRequestobj.Signature = token;
                                _getapi objIndigo = new _getapi();
                                _logoutResponse = await objIndigo.Logout(_logoutRequestobj);

                                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logoutRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logoutResponse), "Logout", "SpicejetOneWay");

                            }
                        }
                        #endregion
                    }
                }
                return View(_AirLinePNRTicket);
                //return RedirectToAction("AirLineTicketBooking", "AirLinesTicket");

            }
        }
        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = MessageState.New;
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
