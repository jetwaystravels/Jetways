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
using Bookingmanager_;
using Utility;
using Sessionmanager;
using OnionArchitectureAPI.Services.Barcode;
using static DomainLayer.Model.ReturnTicketBooking;
using IndigoBookingManager_;
using OnionConsumeWebAPI.Extensions;
using System.Collections;
using static DomainLayer.Model.SeatMapResponceModel;
using static DomainLayer.Model.ReturnAirLineTicketBooking;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{

    public class SGCommitBookingController : Controller
    {

        Logs logs = new Logs();
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";


        string token = string.Empty;
        String BarcodePNR = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        string BarcodeString = string.Empty;
        string BarcodeInfantString = string.Empty;
        string orides = string.Empty;
        string carriercode = string.Empty;
        string flightnumber = string.Empty;
        string seatnumber = string.Empty;
        string sequencenumber = string.Empty;
        decimal TotalMeal = 0;
        decimal TotalBag = 0;
        decimal TotalBagtax = 0;
        decimal Totatamountmb = 0;
        DateTime Journeydatetime = new DateTime();
        string bookingKey = string.Empty;

        public async Task<IActionResult> booking()
        {
            AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
            _AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();

            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            token = string.Empty;
            if (!string.IsNullOrEmpty(tokenview))
            {
                if (tokenview == null) { tokenview = ""; }
                token = tokenview.Replace(@"""", string.Empty);
                string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));
                string contactdata = HttpContext.Session.GetString("ContactDetails");
                UpdateContactsRequest contactList = (UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(UpdateContactsRequest));
                using (HttpClient client = new HttpClient())
                {

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

                    SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                    _BookingCommitResponse = await objSpiceJet.BookingCommit(_bookingCommitRequest);

                    string Str3 = JsonConvert.SerializeObject(_BookingCommitResponse);
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit", "SpicejetOneWay");
                    if (_BookingCommitResponse != null)
                    {
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

                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails", "SpicejetOneWay");
                        if (_getBookingResponse != null)
                        {
                            Hashtable htname = new Hashtable();
                            Hashtable htnameempty = new Hashtable();
                            Hashtable htpax = new Hashtable();

                            Hashtable htseatdata = new Hashtable();
                            Hashtable htmealdata = new Hashtable();
                            Hashtable htbagdata = new Hashtable();

                            int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                            int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                            int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                            int TotalCount = adultcount + childcount;
                            string _responceGetBooking1 = JsonConvert.SerializeObject(_getBookingResponse);
                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                            returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                            ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                            if (_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                            //GST Number
                            if (_getBookingResponse.Booking.BookingContacts[0].TypeCode == "G")
                            {
                                returnTicketBooking.customerNumber = _getBookingResponse.Booking.BookingContacts[0].CustomerNumber;
                                returnTicketBooking.companyName = _getBookingResponse.Booking.BookingContacts[0].CompanyName;
                                returnTicketBooking.emailAddressgst = _getBookingResponse.Booking.BookingContacts[0].EmailAddress;
                            }
                            Contacts _contact = new Contacts();
                            _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[0].HomePhone.ToString();
                            if (_unitdesinator.unitDesignatorPax != null)
                                _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();
                            returnTicketBooking.airLines = "SpiceJet";
                            returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;
                            Breakdown breakdown = new Breakdown();
                            List<JourneyTotals> journeyBaseFareobj = new List<JourneyTotals>();
                            JourneyTotals journeyTotalsobj = new JourneyTotals();

                            PassengerTotals passengerTotals = new PassengerTotals();
                            ReturnSeats returnSeats = new ReturnSeats();
                            passengerTotals.specialServices = new SpecialServices();
                            passengerTotals.baggage = new SpecialServices();
                            var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

                            //changes for Passeneger name:

                            foreach (var item in _getBookingResponse.Booking.Passengers)
                            {
                                htname.Add(item.PassengerNumber, item.Names[0].LastName + "/" + item.Names[0].FirstName);
                            }

                            //barcode
                            BarcodePNR = _getBookingResponse.Booking.RecordLocator;
                            if (BarcodePNR != null && BarcodePNR.Length < 7)
                            {
                                BarcodePNR = BarcodePNR.PadRight(7);
                            }
                            List<string> barcodeImage = new List<string>();
                            #region Itenary segment and legs
                            int journeyscount = _getBookingResponse.Booking.Journeys.Length;
                            List<JourneysReturn> AAJourneyList = new List<JourneysReturn>();
                            for (int i = 0; i < journeyscount; i++)
                            {

                                JourneysReturn AAJourneyobj = new JourneysReturn();
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
                                    DesignatorReturn AASegmentDesignatorobj = new DesignatorReturn();

                                    AASegmentDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                    AASegmentDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                    AASegmentDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                    AASegmentDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                    AASegmentobj.designator = AASegmentDesignatorobj;
                                    orides = AASegmentDesignatorobj.origin + AASegmentDesignatorobj.destination;
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
                                        if (passengerFarescount > 0)
                                        {
                                            for (int l = 0; l < passengerFarescount; l++)
                                            {
                                                journeyTotalsobj = new JourneyTotals();
                                                PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                                AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                                double percentagechd = 0.0;
                                                var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                                int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                                List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                                for (int m = 0; m < serviceChargescount; m++)
                                                {
                                                    ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                                    AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                    string data = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeType.ToString();
                                                    if (data.ToLower() == "fareprice")
                                                    {
                                                        journeyTotalsobj.totalAmount += Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                    }
                                                    else
                                                    {
                                                        if (_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType.Equals("CHD") && _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeCode.Contains("PRCT"))
                                                            percentagechd = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                                        else
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
                                                    if (percentagechd > 0)
                                                        journeyTotalsobj.totalAmount = journeyTotalsobj.totalAmount - percentagechd;
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
                                    }
                                    //breakdown.journeyTotals = journeyTotalsobj;
                                    breakdown.passengerTotals = passengerTotals;
                                    AASegmentobj.fares = AAFarelist;
                                    IdentifierReturn AAIdentifierobj = new IdentifierReturn();

                                    AAIdentifierobj.identifier = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                    AAIdentifierobj.carrierCode = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                    AASegmentobj.identifier = AAIdentifierobj;
                                    //barCode
                                    //julian date
                                    Journeydatetime = DateTime.Parse(_getBookingResponse.Booking.Journeys[i].Segments[j].STD.ToString());
                                    carriercode = AAIdentifierobj.carrierCode;
                                    flightnumber = AAIdentifierobj.identifier;
                                    int year = Journeydatetime.Year;
                                    int month = Journeydatetime.Month;
                                    int day = Journeydatetime.Day;
                                    // Calculate the number of days from January 1st to the given date
                                    DateTime currentDate = new DateTime(year, month, day);
                                    DateTime startOfYear = new DateTime(year, 1, 1);
                                    int julianDate = (currentDate - startOfYear).Days + 1;
                                    if (string.IsNullOrEmpty(sequencenumber))
                                    {
                                        sequencenumber = "00000";
                                    }
                                    else
                                    {
                                        sequencenumber = sequencenumber.PadRight(5, '0');
                                    }
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
                                    foreach (var item in _getBookingResponse.Booking.Passengers)
                                    {
                                        if (!htnameempty.Contains(item.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                        {
                                            if (carriercode.Length < 3)
                                                carriercode = carriercode.PadRight(3);
                                            if (flightnumber.Length < 5)
                                            {
                                                flightnumber = flightnumber.PadRight(5);
                                            }
                                            if (sequencenumber.Length < 5)
                                                sequencenumber = sequencenumber.PadRight(5, '0');
                                            seatnumber = "0000";
                                            if (seatnumber.Length < 4)
                                                seatnumber = seatnumber.PadLeft(4, '0');
                                            BarcodeString = "M" + "1" + htname[item.PassengerNumber] + " " + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDate + "Y" + seatnumber + "" + sequencenumber + "1" + "00";
                                            htnameempty.Add(item.PassengerNumber.ToString() + "_" + htname[item.PassengerNumber] + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, BarcodeString);
                                        }
                                    }




                                    // Vinay For Seat 
                                    foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSeats)
                                    {
                                        barcodeImage = new List<string>();
                                        try
                                        {
                                            if (!htseatdata.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                            {
                                                htseatdata.Add(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, item1.UnitDesignator);
                                                returnSeats.unitDesignator += item1.PassengerNumber + "_" + item1.UnitDesignator + ",";
                                            }
                                            if (!htpax.Contains(item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation))
                                            {
                                                if (carriercode.Length < 3)
                                                    carriercode = carriercode.PadRight(3);
                                                if (flightnumber.Length < 5)
                                                {
                                                    flightnumber = flightnumber.PadRight(5);
                                                }
                                                if (sequencenumber.Length < 5)
                                                    sequencenumber = sequencenumber.PadRight(5, '0');
                                                seatnumber = htseatdata[item1.PassengerNumber.ToString() + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation].ToString();
                                                if (seatnumber.Length < 4)
                                                    seatnumber = seatnumber.PadLeft(4, '0');
                                                BarcodeString = "M" + "1" + htname[item1.PassengerNumber] + " " + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDate + "Y" + seatnumber + "" + sequencenumber + "1" + "00";
                                                htpax.Add(item1.PassengerNumber.ToString() + "_" + htname[item1.PassengerNumber] + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation + "_" + _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation, BarcodeString);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }

                                    // Vinay SSR Meal 

                                    foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                    {
                                        try
                                        {
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
                            string flightreference = string.Empty;
                            foreach (var item in _getBookingResponse.Booking.Passengers)
                            {
                                barcodeImage = new List<string>();
                                passkeytypeobj = new ReturnPassengers();
                                passkeytypeobj.name = new Name();
                                foreach (var item1 in item.PassengerFees)
                                {
                                    if (item1.FeeCode.Equals("SeatFee") || item1.FeeType.ToString().ToLower().Equals("seatfee"))
                                    {
                                        flightreference = item1.FlightReference;
                                        string[] parts = flightreference.Split(' ');

                                        if (parts.Length > 3)
                                        {
                                            carriercode = parts[1]; // "6E" + "774"
                                            flightnumber = parts[2];
                                            orides = parts[3];
                                        }
                                        else
                                        {
                                            // Combine parts for the flight code
                                            carriercode = parts[1].Substring(0, 2); // "6E" + "774"
                                            flightnumber = parts[1].Substring(2);
                                            orides = parts[2];
                                        }
                                        if (flightnumber.Length < 5)
                                        {
                                            flightnumber = flightnumber.PadRight(5);
                                        }
                                        if (carriercode.Length < 3)
                                        {
                                            carriercode = carriercode.PadRight(3);
                                        }

                                        //barCode
                                        //julian date
                                        Journeydatetime = DateTime.Parse(_getBookingResponse.Booking.Journeys[0].Segments[0].STD.ToString());
                                        int year = Journeydatetime.Year;
                                        int month = Journeydatetime.Month;
                                        int day = Journeydatetime.Day;

                                        // Calculate the number of days from January 1st to the given date
                                        DateTime currentDate = new DateTime(year, month, day);
                                        DateTime startOfYear = new DateTime(year, 1, 1);
                                        int julianDate = (currentDate - startOfYear).Days + 1;
                                        if (string.IsNullOrEmpty(sequencenumber))
                                        {
                                            sequencenumber = "00000";
                                        }
                                        else
                                        {
                                            sequencenumber = sequencenumber.PadRight(5, '0');
                                        }

                                        Hashtable seatassignhashtable = new Hashtable();
                                        string[] entries = returnSeats.unitDesignator.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string entry in entries)
                                        {
                                            // Split each entry by underscore
                                            string[] keyValue = entry.Split('_');
                                            if (keyValue.Length == 2)
                                            {
                                                string key = keyValue[0];
                                                string value = keyValue[1];
                                                // Add to the hashtable
                                                seatassignhashtable.Add(key, value);
                                            }
                                        }
                                        if (htseatdata.ContainsKey(item.PassengerNumber.ToString() + "_" + orides.Substring(0, 3) + "_" + orides.Substring(3)))
                                        {
                                            seatnumber = htseatdata[item.PassengerNumber.ToString() + "_" + orides.Substring(0, 3) + "_" + orides.Substring(3)].ToString();
                                            if (string.IsNullOrEmpty(seatnumber))
                                            {
                                                seatnumber = "0000"; // Set to "0000" if not available
                                            }
                                            else
                                            {
                                                seatnumber = seatnumber.PadLeft(4, '0'); // Right-pad with zeros if less than 4 characters
                                            }


                                        }
                                        //BarcodeString = "M" + "1" + item.Names[0].LastName + "/" + item.Names[0].FirstName + " " + BarcodePNR + "" + orides + carriercode + "" + flightnumber + "" + julianDate + "Y" + seatnumber + " " + sequencenumber + "1" + "00";
                                        //BarcodeUtility BarcodeUtility = new BarcodeUtility();
                                        //barcodeImage.Add(BarcodeUtility.BarcodereadUtility(BarcodeString));
                                        foreach (var item2 in item1.ServiceCharges)
                                        {
                                            if (item2.ChargeCode.Equals("SeatFee") || item2.ChargeType.ToString().ToLower().Equals("servicecharge"))
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
                                                InfantfareTotals.totalTax += Convert.ToInt32(item2.Amount);
                                            }
                                        }
                                        journeyBaseFareobj.Add(InfantfareTotals);
                                        breakdown.journeyfareTotals = journeyBaseFareobj;
                                    }
                                    else
                                    {
                                        foreach (var item2 in item1.ServiceCharges)
                                        {
                                            if ((!item2.ChargeCode.Equals("SeatFee") || !item2.ChargeCode.Equals("INFT")) && !item2.ChargeType.ToString().ToLower().Contains("tax") && item2.ChargeCode.StartsWith("E", StringComparison.OrdinalIgnoreCase) == false)
                                            //if ((!item2.ChargeCode.Equals("SEAT") || !item2.ChargeCode.Equals("INFT")) && !item2.ChargeType.ToString().ToLower().Contains("tax"))
                                            {
                                                passengerTotals.specialServices.total += Convert.ToInt32(item2.Amount);
                                                //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                TotalMeal = passengerTotals.specialServices.total;
                                            }
                                            if (item2.ChargeCode.StartsWith("E", StringComparison.OrdinalIgnoreCase) == true)
                                            {
                                                passengerTotals.baggage.total += Convert.ToInt32(item2.Amount);
                                                //breakdown.passengerTotals.seats.total += Convert.ToInt32(item2.Amount);
                                                TotalBag = passengerTotals.baggage.total;
                                            }
                                            else
                                            {
                                                passengerTotals.specialServices.taxes += Convert.ToInt32(item2.Amount);
                                                //breakdown.passengerTotals.seats.taxes += Convert.ToInt32(item2.Amount);
                                                TotalBagtax = passengerTotals.specialServices.taxes;
                                            }
                                            Totatamountmb = TotalMeal + TotalBag;
                                        }
                                    }
                                }
                                passkeytypeobj.barcodestringlst = barcodeImage;

                                passkeytypeobj.passengerTypeCode = item.PassengerTypeInfo.PaxType;
                                //passkeytypeobj.name.first = item.Names[0].FirstName + " " + item.Names[0].LastName;
                                passkeytypeobj.name.first = item.Names[0].FirstName;
                                passkeytypeobj.name.last = item.Names[0].LastName;
                                //passkeytypeobj.MobNumber = "";
                                for (int i = 0; i < passeengerlist.Count; i++)
                                {
                                    if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() && passkeytypeobj.name.last.ToLower() == passeengerlist[i].last.ToLower())
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
                                    //passkeytypeobj.name.first = item.Infant.Names[0].FirstName + " " + item.Infant.Names[0].LastName;
                                    passkeytypeobj.name.first = item.Infant.Names[0].FirstName;
                                    passkeytypeobj.name.last = item.Infant.Names[0].LastName;
                                    //passkeytypeobj.MobNumber = "";
                                    for (int i = 0; i < passeengerlist.Count; i++)
                                    {
                                        if (passkeytypeobj.passengerTypeCode == passeengerlist[i].passengertypecode && passkeytypeobj.name.first.ToLower() == passeengerlist[i].first.ToLower() && passkeytypeobj.name.last.ToLower() == passeengerlist[i].last.ToLower())
                                        {
                                            passkeytypeobj.MobNumber = passeengerlist[i].mobile;
                                            passkeytypeobj.passengerKey = passeengerlist[i].passengerkey;
                                            //passkeytypeobj.seats.unitDesignator = htseatdata[passeengerlist[i].passengerkey].ToString();
                                            break;
                                        }

                                    }
                                    passkeylist.Add(passkeytypeobj);

                                }
                                returnTicketBooking.passengers = passkeylist;
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
                            breakdown.passengerTotals.baggage.total = passengerTotals.baggage.total;
                            breakdown.TotalMealBagAmount = breakdown.passengerTotals.specialServices.total + breakdown.passengerTotals.baggage.total;

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
                            //returnTicketBooking.passengers = passkeylist;
                            returnTicketBooking.passengerscount = passengercount;
                            returnTicketBooking.contacts = _contact;
                            returnTicketBooking.Seatdata = htseatdata;
                            returnTicketBooking.Mealdata = htmealdata;
                            returnTicketBooking.Bagdata = htbagdata;

                            returnTicketBooking.htname = htname;
                            returnTicketBooking.htnameempty = htnameempty;
                            returnTicketBooking.htpax = htpax;

                            returnTicketBooking.bookingdate = _getBookingResponse.Booking.BookingInfo.BookingDate;
                            _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);

                            AirLineFlightTicketBooking airLineFlightTicketBooking = new AirLineFlightTicketBooking();
                            airLineFlightTicketBooking.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                            tb_Booking tb_Booking = new tb_Booking();
                            tb_Booking.AirLineID = 1;
                            tb_Booking.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                            tb_Booking.RecordLocator = _getBookingResponse.Booking.RecordLocator;
                            tb_Booking.CurrencyCode = _getBookingResponse.Booking.CurrencyCode;
                            tb_Booking.Origin = _getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].DepartureStation;
                            tb_Booking.Destination = _getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].ArrivalStation;
                            tb_Booking.BookedDate = DateTime.Now;//JsonObjPNRBooking.data.journeys[0].designator.departure;                    
                            tb_Booking.TotalAmount = (double)_getBookingResponse.Booking.BookingSum.BalanceDue;
                            tb_Booking.SpecialServicesTotal = (double)Totatamountmb;
                            tb_Booking.SpecialServicesTotal_Tax = (double)TotalBagtax;
                            tb_Booking.SeatTotalAmount = returnSeats.total;
                            tb_Booking.SeatTotalAmount_Tax = returnSeats.taxes;
                            tb_Booking.ExpirationDate = DateTime.Now;
                            tb_Booking.ArrivalDate = Convert.ToString(_getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].STA);
                            tb_Booking.DepartureDate = Convert.ToString(_getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].STD);
                            tb_Booking.CreatedDate = DateTime.Now;
                            tb_Booking.Createdby = "Online";
                            tb_Booking.ModifiedDate = DateTime.Now;
                            tb_Booking.ModifyBy = "Online";
                            tb_Booking.BookingDoc = Convert.ToString(_getBookingResponse);
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
                            ContactDetail contactDetail = new ContactDetail();
                            contactDetail.FirstName = _getBookingResponse.Booking.BookingContacts[0].Names[0].FirstName;
                            contactDetail.LastName = _getBookingResponse.Booking.BookingContacts[0].Names[0].LastName;
                            contactDetail.EmailID = _getBookingResponse.Booking.BookingContacts[0].EmailAddress;
                            contactDetail.MobileNumber = 789456123;/*Convert.ToInt64(_getBookingResponse.Booking.BookingContacts[0].HomePhone)*/
                            contactDetail.CreateDate = DateTime.Now;
                            contactDetail.CreateBy = "Admin";
                            contactDetail.ModifyDate = DateTime.Now;
                            contactDetail.ModifyBy = "Admin";
                            contactDetail.Status = 0;

                            var passangerCount = _getBookingResponse.Booking.Passengers;
                            int PassengerDataCount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;
                            List<tb_PassengerDetails> tb_PassengerDetailsList = new List<tb_PassengerDetails>();
                            foreach (var items in _getBookingResponse.Booking.Passengers)
                            {
                                tb_PassengerDetails tb_Passengerobj = new tb_PassengerDetails();
                                tb_Passengerobj.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                                tb_Passengerobj.PassengerKey = "MCGWRH";
                                tb_Passengerobj.TypeCode = items.PassengerTypeInfo.PaxType;
                                tb_Passengerobj.FirstName = items.Names[0].FirstName;
                                tb_Passengerobj.Title = "Mr";
                                tb_Passengerobj.LastName = items.Names[0].LastName;
                                tb_Passengerobj.TotalAmount = (decimal)breakdown.journeyTotals.totalAmount;
                                tb_Passengerobj.TotalAmount_tax = (decimal)breakdown.journeyTotals.totalTax;
                                tb_Passengerobj.CreatedDate = DateTime.Now;
                                tb_Passengerobj.Createdby = "Online";
                                tb_Passengerobj.ModifiedDate = DateTime.Now;
                                tb_Passengerobj.ModifyBy = "Online";
                                tb_Passengerobj.Status = "0";
                                tb_PassengerDetailsList.Add(tb_Passengerobj);
                            }

                            tb_PassengerTotal tb_PassengerTotalobj = new tb_PassengerTotal();
                            bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                            tb_PassengerTotalobj.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                            if (_getBookingResponse.Booking.Passengers.Length > 0 && _getBookingResponse.Booking.Passengers[0].PassengerFees.Length > 0)
                            {
                                tb_PassengerTotalobj.TotalMealsAmount = (double)Totatamountmb;
                                tb_PassengerTotalobj.TotalMealsAmount_Tax = (double)TotalBagtax;
                                tb_PassengerTotalobj.TotalSeatAmount = returnSeats.total;
                                tb_PassengerTotalobj.TotalSeatAmount_Tax = returnSeats.taxes;
                            }
                            tb_PassengerTotalobj.TotalBookingAmount = (double)breakdown.journeyTotals.totalAmount;
                            tb_PassengerTotalobj.totalBookingAmount_Tax = (double)breakdown.journeyTotals.totalTax;
                            tb_PassengerTotalobj.Modifyby = "Online";
                            tb_PassengerTotalobj.Createdby = "Online";
                            tb_PassengerTotalobj.Status = "0";
                            tb_PassengerTotalobj.CreatedDate = DateTime.Now;
                            tb_PassengerTotalobj.ModifiedDate = DateTime.Now;

                            int JourneysCount = _getBookingResponse.Booking.Journeys.Length;
                            List<tb_journeys> tb_JourneysList = new List<tb_journeys>();
                            for (int i = 0; i < JourneysCount; i++)
                            {
                                tb_journeys tb_JourneysObj = new tb_journeys();
                                tb_JourneysObj.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                                tb_JourneysObj.JourneyKey = _getBookingResponse.Booking.Journeys[i].JourneySellKey;
                                tb_JourneysObj.Stops = 1;
                                tb_JourneysObj.JourneyKeyCount = i;
                                tb_JourneysObj.FlightType = "1";
                                tb_JourneysObj.Origin = _getBookingResponse.Booking.Journeys[i].Segments[0].DepartureStation;
                                tb_JourneysObj.Destination = _getBookingResponse.Booking.Journeys[i].Segments[0].ArrivalStation;
                                tb_JourneysObj.DepartureDate = _getBookingResponse.Booking.Journeys[i].Segments[0].STD;
                                tb_JourneysObj.ArrivalDate = _getBookingResponse.Booking.Journeys[i].Segments[0].STA;
                                tb_JourneysObj.CreatedDate = DateTime.Now;
                                tb_JourneysObj.Createdby = "Online";
                                tb_JourneysObj.ModifiedDate = DateTime.Now;
                                tb_JourneysObj.Modifyby = "Online";
                                tb_JourneysObj.Status = "0";
                                tb_JourneysList.Add(tb_JourneysObj);
                            }
                            int SegmentReturnCountt = _getBookingResponse.Booking.Journeys[0].Segments.Length;
                            List<tb_Segments> segmentReturnsListt = new List<tb_Segments>();
                            for (int j = 0; j < SegmentReturnCountt; j++)
                            {
                                tb_Segments segmentReturnobj = new tb_Segments();
                                segmentReturnobj.BookingID = _getBookingResponse.Booking.BookingID.ToString();
                                segmentReturnobj.journeyKey = _getBookingResponse.Booking.Journeys[0].JourneySellKey;
                                segmentReturnobj.SegmentKey = _getBookingResponse.Booking.Journeys[0].Segments[j].SegmentSellKey;
                                segmentReturnobj.SegmentCount = j;
                                segmentReturnobj.Origin = _getBookingResponse.Booking.Journeys[0].Segments[j].DepartureStation;
                                segmentReturnobj.Destination = _getBookingResponse.Booking.Journeys[0].Segments[j].ArrivalStation;
                                segmentReturnobj.DepartureDate = (_getBookingResponse.Booking.Journeys[0].Segments[j].STD.ToString());
                                segmentReturnobj.ArrivalDate = (_getBookingResponse.Booking.Journeys[0].Segments[j].STA.ToString());
                                segmentReturnobj.Identifier = _getBookingResponse.Booking.Journeys[0].Segments[j].FlightDesignator.FlightNumber;
                                segmentReturnobj.CarrierCode = _getBookingResponse.Booking.Journeys[0].Segments[j].FlightDesignator.CarrierCode;
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
                            airLineFlightTicketBooking.ContactDetail = contactDetail;
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/AirLineTicketBooking/PostairlineTicketData", airLineFlightTicketBooking);
                            if (responsePassengers.IsSuccessStatusCode)
                            {
                                var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            }
                            //LogOut 

                            LogoutRequest _logoutRequestobj = new LogoutRequest();
                            LogoutResponse _logoutResponse = new LogoutResponse();
                            _logoutRequestobj.ContractVersion = 420;
                            _logoutRequestobj.Signature = token;
                            objSpiceJet = new SpiceJetApiController();
                            _logoutResponse = await objSpiceJet.Logout(_logoutRequestobj);

                            logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logoutRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logoutResponse), "Logout", "SpicejetOneWay");
                        }
                    }
                }
                //}


            }

            return View(_AirLinePNRTicket);
            //}
            //return RedirectToAction("GetTicketBooking", "AirLinesTicket");
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
    }
}
