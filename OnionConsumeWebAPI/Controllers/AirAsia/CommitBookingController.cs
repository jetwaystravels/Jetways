using DomainLayer.Model;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.ApiService;
using OnionConsumeWebAPI.Models;
using static DomainLayer.Model.ReturnTicketBooking;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing.Common;
using ZXing;
using ZXing.Windows.Compatibility;
using OnionArchitectureAPI.Services.Barcode;

namespace OnionConsumeWebAPI.Controllers
{

    public class CommitBookingController : Controller
    {
        // string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
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
        public async Task<IActionResult> booking()
        {


            AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
            _AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();


            string tokenview = HttpContext.Session.GetString("AirasiaTokan");
            token = tokenview.Replace(@"""", string.Empty);

            using (HttpClient client = new HttpClient())
            {
                #region Commit Booking
                string[] NotifyContacts = new string[1];
                NotifyContacts[0] = "P";
                Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();

                _Commit_BookingModel.notifyContacts = true;
                _Commit_BookingModel.contactTypesToNotify = NotifyContacts;
                var jsonCommitBookingRequest = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

                ApiRequests apiRequests = new ApiRequests();
                //   responseModel = await apiRequests.OnPostAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);


                if (responceCommit_Booking.IsSuccessStatusCode)
                {
                    var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
                    var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
                }
                #endregion
                #region Booking GET
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.AirasiaGetBoking);
                if (responceGetBooking.IsSuccessStatusCode)
                {
                    // string BaseURL1 = "http://localhost:5225/";
                    var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                    var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                    AirLinePNR = JsonObjGetBooking.data.recordLocator;
                }
                #endregion
                #region AirLinePNR
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responcepnrBooking = await client.GetAsync(AppUrlConstant.AirasiaPNRBooking + AirLinePNR);
                if (responcepnrBooking.IsSuccessStatusCode)
                {
                    // string BaseURL1 = "http://localhost:5225/";
                    var _responcePNRBooking = responcepnrBooking.Content.ReadAsStringAsync().Result;
                    var JsonObjPNRBooking = JsonConvert.DeserializeObject<dynamic>(_responcePNRBooking);
                    ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                    returnTicketBooking.recordLocator = JsonObjPNRBooking.data.recordLocator;
                    BarcodePNR = JsonObjPNRBooking.data.recordLocator;
                    if (BarcodePNR.Length < 7)
                    {
                        BarcodePNR = BarcodePNR.PadRight(7);
                    }
                    returnTicketBooking.bookingKey = JsonObjPNRBooking.data.bookingKey;
                    // var zxvx= JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
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
                                }
                                passengerSegmentobj.seats = returnSeatsList;
                                //passengerSegmentsList.Add(passengerSegmentobj);
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
                            carriercode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
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
                        returnPassengersobj.name.last = items.Value.name.last;
                        //julian date
                        int year = 2024;
                        int month = 2;
                        int day = 20;

                        // Calculate the number of days from January 1st to the given date
                        DateTime currentDate = new DateTime(year, month, day);
                        DateTime startOfYear = new DateTime(year, 1, 1);
                        int julianDate = (currentDate - startOfYear).Days + 1;
                        if (string.IsNullOrEmpty(sequencenumber))
                        {
                            sequencenumber = "0000"; // Set to "0000" if not available
                        }
                        else
                        {
                            sequencenumber = sequencenumber.PadRight(5, '0'); // Right-pad with zeros if less than 4 characters
                        }

                        BarcodeString = "M" + "1" + items.Value.name.last + "/" + items.Value.name.first + "" + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDate + "Y" + sequencenumber + "1" + "00";
                        BarcodeUtility BarcodeUtility = new BarcodeUtility();
                        var barcodeImage = BarcodeUtility.BarcodereadUtility(BarcodeString);
                        returnPassengersobj.barcodestring = barcodeImage;
                        InfantReturn infantsObject = new InfantReturn();
                        Name name = new Name();
                        if (items.Value.infant != null)
                        {
                            name.first = items.Value.infant.name.first;
                            name.last = items.Value.infant.name.last;
                            //x.data.passengers["MCFBRFQ-"].infant.fees[0].code
                            infantsObject.name = name;
                            //julian date
                            int  Infantyear = 2024;
                            int  Infantmonth = 2;
                            int Infantday = 20;
                            // Calculate the number of days from January 1st to the given date
                            DateTime currentDatee = new DateTime(year, month, day);
                            DateTime startOfYeare = new DateTime(year, 1, 1);
                            int julianDatee = (currentDate - startOfYear).Days + 1;
                            if (string.IsNullOrEmpty(sequencenumber))
                            {
                                sequencenumber = "0000"; // Set to "0000" if not available
                            }
                            else
                            {
                                sequencenumber = sequencenumber.PadRight(5, '0'); // Right-pad with zeros if less than 4 characters
                            }
                            BarcodeInfantString = "M" + "1" + items.Value.infant.name.last + "/" + items.Value.infant.name.first + "" + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDatee + "Y" + sequencenumber + "1" + "00";
                            var barcodeInfantImage = BarcodeUtility.BarcodereadUtility(BarcodeInfantString);
                            returnPassengersobj.barcodestring = barcodeInfantImage;

                        }
                        returnPassengersobj.infant = infantsObject;
                        ReturnpassengersList.Add(returnPassengersobj);
                    }

                  



                    returnTicketBooking.breakdown = breakdown;
                    returnTicketBooking.journeys = journeysreturnList;
                    returnTicketBooking.passengers = ReturnpassengersList;
                    returnTicketBooking.passengerscount = Returnpassengercount;
                    returnTicketBooking.PhoneNumbers = phoneNumberList;
                    _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);

                    AirLineFlightTicketBooking airLineFlightTicketBooking = new AirLineFlightTicketBooking();
                    airLineFlightTicketBooking.BookingID = JsonObjPNRBooking.data.bookingKey;
                    tb_Booking tb_Booking = new tb_Booking();
                    tb_Booking.AirLineID = 1;
                    tb_Booking.BookingID = JsonObjPNRBooking.data.bookingKey;
                    tb_Booking.RecordLocator = JsonObjPNRBooking.data.recordLocator;
                    tb_Booking.CurrencyCode = JsonObjPNRBooking.data.currencyCode;
                    tb_Booking.Origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
                    tb_Booking.Destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
                    tb_Booking.BookedDate = DateTime.Now;//JsonObjPNRBooking.data.journeys[0].designator.departure;                    
                    tb_Booking.TotalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                    tb_Booking.SpecialServicesTotal = (decimal)1000.00;//(decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                    tb_Booking.SpecialServicesTotal_Tax = (decimal)100.0;//JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                    tb_Booking.SeatTotalAmount = (decimal)2000.00;//JsonObjPNRBooking.data.breakdown.passengerTotalsls.seats.total;
                    tb_Booking.SeatTotalAmount_Tax = (decimal)200.00;//JsonObjPNRBooking.data.breakdown.passengerTotalsls.seats.taxes;
                    tb_Booking.ExpirationDate = DateTime.Now;//JsonObjPNRBooking.data.hold.expiration;
                    tb_Booking.ArrivalDate = JsonObjPNRBooking.data.journeys[0].designator.arrival;//DateTime.Now;
                    tb_Booking.DepartureDate = JsonObjPNRBooking.data.journeys[0].designator.departure;//DateTime.Now;
                    tb_Booking.CreatedDate = DateTime.Now;
                    tb_Booking.Createdby = "Online";
                    tb_Booking.ModifiedDate = DateTime.Now;
                    tb_Booking.ModifyBy = "Online";
                    tb_Booking.BookingDoc = _responcePNRBooking;
                    tb_Booking.Status = "0";
                    tb_Airlines tb_Airlines = new tb_Airlines();
                    tb_Airlines.AirlineID = 1;
                    tb_Airlines.AirlneName = "Boing";
                    tb_Airlines.AirlineDescription = "Indra Gandhi airport";
                    tb_Airlines.CreatedDate = DateTime.Now;
                    tb_Airlines.Createdby = "Online";
                    tb_Airlines.Modifieddate = DateTime.Now;
                    tb_Airlines.Modifyby = "Online";
                    tb_Airlines.Status = "0";
                    tb_AirCraft tb_AirCraft = new tb_AirCraft();
                    tb_AirCraft.Id = 1;
                    tb_AirCraft.AirlineID = 1;
                    tb_AirCraft.AirCraftName = "Airbus";
                    tb_AirCraft.AirCraftDescription = " City Squares Worldwide";
                    tb_AirCraft.CreatedDate = DateTime.Now;
                    tb_AirCraft.Modifieddate = DateTime.Now;
                    tb_AirCraft.Createdby = "Online";
                    tb_AirCraft.Modifyby = "Online";
                    tb_AirCraft.Status = "0";



                    tb_PassengerTotal tb_PassengerTotalobj = new tb_PassengerTotal();
                    bookingKey = JsonObjPNRBooking.data.bookingKey;
                    tb_PassengerTotalobj.BookingID = JsonObjPNRBooking.data.bookingKey;
                    tb_PassengerTotalobj.TotalMealsAmount = (decimal)1000.00;//JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                    tb_PassengerTotalobj.TotalMealsAmount_Tax = (decimal)100.00; //JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                    tb_PassengerTotalobj.TotalSeatAmount = (decimal)2000.00;//JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total;
                    tb_PassengerTotalobj.TotalSeatAmount_Tax = (decimal)200.00;//JsonObjPNRBooking.data.breakdown.passengerTotals.seats.taxes;
                    tb_PassengerTotalobj.TotalBookingAmount = (decimal)1000.00;//JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                    tb_PassengerTotalobj.totalBookingAmount_Tax = (decimal)100.00;// JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
                    tb_PassengerTotalobj.Modifyby = "Online";
                    tb_PassengerTotalobj.Createdby = "Online";
                    tb_PassengerTotalobj.Status = "0";
                    tb_PassengerTotalobj.CreatedDate = DateTime.Now;
                    tb_PassengerTotalobj.ModifiedDate = DateTime.Now;


                    var passangerCount = JsonObjPNRBooking.data.passengers;
                    int PassengerDataCount = ((Newtonsoft.Json.Linq.JContainer)passangerCount).Count;
                    List<tb_PassengerDetails> tb_PassengerDetailsList = new List<tb_PassengerDetails>();
                    foreach (var items in JsonObjPNRBooking.data.passengers)
                    {
                        tb_PassengerDetails tb_Passengerobj = new tb_PassengerDetails();
                        tb_Passengerobj.BookingID = bookingKey;
                        tb_Passengerobj.PassengerKey = items.Value.passengerKey;
                        tb_Passengerobj.TypeCode = items.Value.passengerTypeCode;
                        tb_Passengerobj.FirstName = items.Value.name.first;
                        tb_Passengerobj.Title = items.Value.name.title;
                        tb_Passengerobj.LastName = items.Value.name.last;
                        tb_Passengerobj.TotalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                        tb_Passengerobj.TotalAmount_tax = JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
                        tb_Passengerobj.CreatedDate = DateTime.Now;
                        tb_Passengerobj.Createdby = "Online";
                        tb_Passengerobj.ModifiedDate = DateTime.Now;
                        tb_Passengerobj.ModifyBy = "Online";
                        tb_Passengerobj.Status = "0";
                        tb_PassengerDetailsList.Add(tb_Passengerobj);
                    }

                  

                    int JourneysCount = JsonObjPNRBooking.data.journeys.Count;
                    List<tb_journeys> tb_JourneysList = new List<tb_journeys>();
                    for (int i = 0; i < JourneysCount; i++)
                    {
                        tb_journeys tb_JourneysObj = new tb_journeys();
                        tb_JourneysObj.BookingID = JsonObjPNRBooking.data.bookingKey;
                        tb_JourneysObj.JourneyKey = JsonObjPNRBooking.data.journeys[i].journeyKey;
                        tb_JourneysObj.Stops = JsonObjPNRBooking.data.journeys[i].stops;
                        tb_JourneysObj.JourneyKeyCount = i;
                        tb_JourneysObj.FlightType = JsonObjPNRBooking.data.journeys[i].flightType;
                        tb_JourneysObj.Origin = JsonObjPNRBooking.data.journeys[i].designator.origin;
                        tb_JourneysObj.Destination = JsonObjPNRBooking.data.journeys[i].designator.destination;
                        tb_JourneysObj.DepartureDate = JsonObjPNRBooking.data.journeys[i].designator.departure;
                        tb_JourneysObj.ArrivalDate = JsonObjPNRBooking.data.journeys[i].designator.arrival;
                        tb_JourneysObj.CreatedDate = DateTime.Now;
                        tb_JourneysObj.Createdby = "Online";
                        tb_JourneysObj.ModifiedDate = DateTime.Now;
                        tb_JourneysObj.Modifyby = "Online";
                        tb_JourneysObj.Status = "0";
                        tb_JourneysList.Add(tb_JourneysObj);
                    }

                    int SegmentReturnCountt = JsonObjPNRBooking.data.journeys[0].segments.Count;
                    List<tb_Segments> segmentReturnsListt = new List<tb_Segments>();
                    for (int j = 0; j < SegmentReturnCountt; j++)
                    {
                        tb_Segments segmentReturnobj = new tb_Segments();
                        segmentReturnobj.BookingID = JsonObjPNRBooking.data.bookingKey;
                        segmentReturnobj.journeyKey = JsonObjPNRBooking.data.journeys[0].journeyKey;
                        segmentReturnobj.SegmentKey = JsonObjPNRBooking.data.journeys[0].segments[j].segmentKey;
                        segmentReturnobj.SegmentCount = j;
                        segmentReturnobj.Origin = JsonObjPNRBooking.data.journeys[0].segments[j].designator.origin;
                        segmentReturnobj.Destination = JsonObjPNRBooking.data.journeys[0].segments[j].designator.destination;
                        segmentReturnobj.DepartureDate = JsonObjPNRBooking.data.journeys[0].segments[j].designator.departure;
                        segmentReturnobj.ArrivalDate = JsonObjPNRBooking.data.journeys[0].segments[j].designator.arrival;
                        segmentReturnobj.Identifier = JsonObjPNRBooking.data.journeys[0].segments[j].identifier.identifier;
                        segmentReturnobj.CarrierCode = JsonObjPNRBooking.data.journeys[0].segments[j].identifier.carrierCode;
                        segmentReturnobj.Seatnumber = "2";
                        segmentReturnobj.MealCode = "VScODE";
                        segmentReturnobj.MealDiscription = "it is a coffe";
                        segmentReturnobj.DepartureTerminal = 2;
                        segmentReturnobj.ArrivalTerminal = 1;
                        segmentReturnobj.CreatedDate = DateTime.Now;
                        segmentReturnobj.ModifiedDate = DateTime.Now;
                        segmentReturnobj.Createdby = "Online";
                        segmentReturnobj.Modifyby = "Online";
                        segmentReturnobj.Status = "0";
                        segmentReturnsListt.Add(segmentReturnobj);
                    }

                    airLineFlightTicketBooking.tb_Booking = tb_Booking;
                    airLineFlightTicketBooking.tb_Segments = segmentReturnsListt;
                    airLineFlightTicketBooking.tb_AirCraft = tb_AirCraft;
                    airLineFlightTicketBooking.tb_journeys = tb_JourneysList;
                    airLineFlightTicketBooking.tb_PassengerTotal = tb_PassengerTotalobj;
                    airLineFlightTicketBooking.tb_PassengerDetails = tb_PassengerDetailsList;
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/AirLineTicketBooking/PostairlineTicketData", airLineFlightTicketBooking);
                    if (responsePassengers.IsSuccessStatusCode)
                    {
                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                    }
                }
                #endregion
            }
            return View(_AirLinePNRTicket);

        }
    }
}



//TicketBooking ticketBooking01 = new TicketBooking();
//ticketBooking01.guestBooking = "Travel Mode";
//ticketBooking01.Title = JsonObjGetBooking.data.contacts.P.name.title;
//ticketBooking01.phoneNumber = JsonObjGetBooking.data.contacts.P.phoneNumbers[0].number;
//ticketBooking01.bookingDateTime = JsonObjGetBooking.data.info.bookedDate;
//ticketBooking01.Class = "Economics";
//ticketBooking01.airLines = "Air Asia";
//ticketBooking01.passengerName = JsonObjGetBooking.data.contacts.P.name.first;
//ticketBooking01.emailId = JsonObjGetBooking.data.contacts.P.emailAddress;
//ticketBooking01.seatNumber = "1A";
//ticketBooking01.desination = JsonObjGetBooking.data.journeys[0].segments[0].designator.destination;
//ticketBooking01.arrival = JsonObjGetBooking.data.journeys[0].segments[0].designator.arrival;
//ticketBooking01.departure = JsonObjGetBooking.data.journeys[0].segments[0].designator.departure;
//ticketBooking01.identifier = JsonObjGetBooking.data.journeys[0].segments[0].identifier.identifier;
//ticketBooking01.carrierCode = JsonObjGetBooking.data.journeys[0].segments[0].identifier.carrierCode;
//ticketBooking01.origin = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].designator.origin;
//ticketBooking01.taxex = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[1].amount;
//ticketBooking01.price = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[0].amount;
//ticketBooking01.desinationTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.departureTerminal;
//ticketBooking01.sourceTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.arrivalTerminal;
//ticketBooking01.airlinePNR = JsonObjGetBooking.data.recordLocator;
//ticketBooking01.bookingStatus = "Confirmed";
//ticketBooking01.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
//ticketBooking01.response = _responceGetBooking;

//client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/TicketBooking/GetTicketBooking", ticketBooking01);
//if (responsePassengers.IsSuccessStatusCode)
//{
//    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
//}
//GSTDetails details = new GSTDetails();
//if (details.Id != null)
//{
//    details.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
//    details.airLinePNR = JsonObjGetBooking.data.recordLocator;
//    details.GSTNumber = JsonObjGetBooking.data.contacts.G.customerNumber;
//    details.GSTName = JsonObjGetBooking.data.contacts.G.companyName;
//    details.GSTEmail = JsonObjGetBooking.data.contacts.G.emailAddress;
//    details.status = 1;
//    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//    HttpResponseMessage responsePassengers1 = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/GSTDetails/GetGstDetails", details);
//    if (responsePassengers1.IsSuccessStatusCode)
//    {
//        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
//    }
//}




//AirLineFlightTicketBooking airLineFlightTicketBooking = new AirLineFlightTicketBooking();
//tb_Booking tb_Booking = new tb_Booking();
//tb_Booking.AirLineID = 1;
//tb_Booking.BookingID = JsonObjPNRBooking.data.bookingKey;
//tb_Booking.RecordLocator = JsonObjPNRBooking.data.recordLocator;
//tb_Booking.CurrencyCode = JsonObjPNRBooking.data.currencyCode;
//tb_Booking.Origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
//tb_Booking.Destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
//tb_Booking.BookedDate = JsonObjPNRBooking.data.journeys[0].designator.departure;
//tb_Booking.Destination = JsonObjPNRBooking.data.journeys[0].designator.arrival;
//tb_Booking.TotalAmount = JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
//tb_Booking.SpecialServicesTotal = 1000;//JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
//tb_Booking.SpecialServicesTotal_Tax = 10;//JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
//tb_Booking.SeatTotalAmount = 20000;//JsonObjPNRBooking.data.breakdown.passengerTotals.seats.total;
//tb_Booking.SeatTotalAmount_Tax = 200;//JsonObjPNRBooking.data.breakdown.passengerTotals.seats.taxes;
//tb_Booking.CreatedDate = DateTime.Now;
//tb_Booking.Createdby = "xyz";
//tb_Booking.ModifiedDate = DateTime.Now;
//tb_Booking.ModifyBy = "abc";
//tb_Booking.BookingDoc = "BookingDoc";
//tb_Booking.Status = "0";
//tb_Airlines tb_Airlines = new tb_Airlines();
//tb_Airlines.Id = 1;
//tb_Airlines.AirlneName = "Boing";
//tb_Airlines.AirlineDescription = "Indra Gandhi airport";
//tb_Airlines.CreatedDate = DateTime.Now;
//tb_Airlines.Createdby = "xyz";
//tb_Airlines.Modifieddate = DateTime.Now;
//tb_Airlines.Modifyby = "abc";
//tb_Airlines.Status = "0";
//airLineFlightTicketBooking.tb_Booking = tb_Booking;
//airLineFlightTicketBooking.tb_Airlines = tb_Airlines;                   

//client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/AirLineTicketBooking/PostairlineTicketData", airLineFlightTicketBooking);
//if (responsePassengers.IsSuccessStatusCode)
//{
//    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
//}
