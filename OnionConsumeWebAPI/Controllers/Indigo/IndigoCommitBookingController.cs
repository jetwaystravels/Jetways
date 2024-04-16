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
                            //List<ReturnSeats> unitdesignatorlst = new List<ReturnSeats>();
                            //foreach (var item in _getBookingResponse.Booking.Journeys[0].Segments)
                            //{
                            //    foreach (var item1 in item.PaxSeats)
                            //    {
                            //        returnSeats.unitDesignator = item1.UnitDesignator + ",";
                            //    }
                            //    unitdesignatorlst.Add(returnSeats);
                            //}

                            //passengerTotals.unitseats = unitdesignatorlst;

                            //int a = 0;
                            //foreach (var items in availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes)
                            //{
                            //    for (int i = 0; i < items.PaxCount; i++)
                            //    {


                            //        passkeytypeobj.passengerKey = a.ToString();
                            //        //if (items.PaxType == "ADT")
                            //        //{
                            //        passkeytypeobj.passengerTypeCode = items.PaxType;
                            //        //}

                            //        //passkeylist.Add(passkeytypeobj);
                            //        a++;
                            //    }
                            //    //passengerkey12 = passkeytypeobj.passengerKey;


                            //}
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
                            //HttpContext.Session.SetString("SGkeypassengerRT", JsonConvert.SerializeObject(returnTicketBooking));
                            //_AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
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
