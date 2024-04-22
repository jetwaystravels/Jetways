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
using OnionArchitectureAPI.Services.Indigo;
using static DomainLayer.Model.ReturnTicketBooking;
using IndigoBookingManager_;
using OnionConsumeWebAPI.Extensions;

namespace OnionConsumeWebAPI.Controllers.Indigo
{

    public class IndigoCommitBookingController : Controller
    {

        Logs logs = new Logs();
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";


        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        string bookingKey = string.Empty;

        public async Task<IActionResult> booking()
        {
            AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
            _AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();
            string tokenview = HttpContext.Session.GetString("IndigoSignature");
            token = string.Empty;
            if (!string.IsNullOrEmpty(tokenview))
            {

                token = tokenview.Replace(@"""", string.Empty);
                string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));
                string contactdata = HttpContext.Session.GetString("ContactDetails");
                IndigoBookingManager_.UpdateContactsRequest contactList = (IndigoBookingManager_.UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(IndigoBookingManager_.UpdateContactsRequest));
                using (HttpClient client1 = new HttpClient())
                {
                    #region Commit Booking

                    _commit objcommit = new _commit();
                    IndigoBookingManager_.BookingCommitResponse _BookingCommitResponse = await objcommit.commit(token, contactList, passeengerlist, "OneWay");

                    if (_BookingCommitResponse != null && _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator != null)
                    {
                        IndigoBookingManager_.GetBookingResponse _getBookingResponse = await objcommit.GetBookingdetails(token, _BookingCommitResponse, "OneWay");

                        if (_getBookingResponse != null)
                        {
                            int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                            int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                            int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                            int TotalCount = adultcount + childcount;
                            string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);
                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                            //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                            var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                            returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                            ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                            if (_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
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
                            var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

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
                                                string data = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeType.ToString();
                                                if (data.ToLower() == "fareprice")
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
                                        returnSeats.unitDesignator += item1.UnitDesignator + ",";
                                    }

                                    foreach (var item1 in _getBookingResponse.Booking.Journeys[i].Segments[j].PaxSSRs)
                                    {
                                        returnSeats.SSRCode += item1.SSRCode + ",";
                                    }
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
                                passkeytypeobj.MobNumber = "";
                                passkeylist.Add(passkeytypeobj);
                                if (item.Infant != null)
                                {
                                    passkeytypeobj = new ReturnPassengers();
                                    passkeytypeobj.name = new Name();
                                    passkeytypeobj.passengerTypeCode = "INFT";
                                    passkeytypeobj.name.first = item.Infant.Names[0].FirstName + " " + item.Infant.Names[0].LastName;
                                    passkeytypeobj.MobNumber = "";
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
                            tb_Booking.TotalAmount = _getBookingResponse.Booking.BookingSum.BalanceDue;
                            tb_Booking.SpecialServicesTotal = (decimal)1000.00;//(decimal)JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.total;
                            tb_Booking.SpecialServicesTotal_Tax = (decimal)100.0;//JsonObjPNRBooking.data.breakdown.passengerTotals.specialServices.taxes;
                            tb_Booking.SeatTotalAmount = (decimal)2000.00;//JsonObjPNRBooking.data.breakdown.passengerTotalsls.seats.total;
                            tb_Booking.SeatTotalAmount_Tax = (decimal)200.00;//JsonObjPNRBooking.data.breakdown.passengerTotalsls.seats.taxes;
                            tb_Booking.ExpirationDate = DateTime.Now;//JsonObjPNRBooking.data.hold.expiration;
                            tb_Booking.ArrivalDate = Convert.ToString(_getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].STA);//DateTime.Now;
                            tb_Booking.DepartureDate = Convert.ToString(_getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].STD);//DateTime.Now;
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
                                tb_Passengerobj.TotalAmount = 10000; //JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                                tb_Passengerobj.TotalAmount_tax = 200; //JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
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
                            tb_PassengerTotalobj.TotalMealsAmount = _getBookingResponse.Booking.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount;
                            tb_PassengerTotalobj.TotalMealsAmount_Tax = _getBookingResponse.Booking.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount;
                            tb_PassengerTotalobj.TotalSeatAmount = _getBookingResponse.Booking.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount;
                            tb_PassengerTotalobj.TotalSeatAmount_Tax = _getBookingResponse.Booking.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount;
                            tb_PassengerTotalobj.TotalBookingAmount = (decimal)1000.00;//JsonObjPNRBooking.data.breakdown.journeyTotals.totalAmount;
                            tb_PassengerTotalobj.totalBookingAmount_Tax = (decimal)100.00;// JsonObjPNRBooking.data.breakdown.journeyTotals.totalTax;
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
                            client1.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage responsePassengers = await client1.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/AirLineTicketBooking/PostairlineTicketData", airLineFlightTicketBooking);
                            if (responsePassengers.IsSuccessStatusCode)
                            {
                                var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            }

                        }
                    }
                    #endregion

                }
            }
            return View(_AirLinePNRTicket);
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
