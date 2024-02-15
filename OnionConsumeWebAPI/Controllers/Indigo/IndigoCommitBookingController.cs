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
                    IndigoBookingManager_.BookingCommitResponse _BookingCommitResponse = await objcommit.commit(token, contactList, passeengerlist,"OneWay");

                    if (_BookingCommitResponse != null && _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator != null)
                    {
                        IndigoBookingManager_.GetBookingResponse _getBookingResponse = await objcommit.GetBookingdetails(token, _BookingCommitResponse,"OneWay");

                        if (_getBookingResponse != null)
                        {
                            string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);

                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                            //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                            var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                            returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                            ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                            _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                            Contacts _contact = new Contacts();
                            _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[1].HomePhone.ToString();
                            _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();
                            returnTicketBooking.airLines = "Indigo";
                            returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;

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
                                        for (int l = 0; l < passengerFarescount; l++)
                                        {
                                            PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                            AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                            var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                            int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                            List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                            for (int m = 0; m < serviceChargescount; m++)
                                            {
                                                ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                                AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);



                                                AAServicechargelist.Add(AAServicechargeobj);
                                            }



                                            AAPassengerfareobject.serviceCharges = AAServicechargelist;

                                            PassengerfarelistRT.Add(AAPassengerfareobject);

                                        }
                                        AAFareobj.passengerFares = PassengerfarelistRT;

                                        AAFarelist.Add(AAFareobj);




                                    }
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

                                    AASegmentobj.legs = AALeglist;
                                    AASegmentlist.Add(AASegmentobj);
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
                            passkeytypeobj.name = new Name();
                            passkeytypeobj.name.first = _getBookingResponse.Booking.Passengers[0].Names[0].FirstName;




                            List<ReturnPassengers> passkeylist = new List<ReturnPassengers>();
                            int a = 0;
                            foreach (var items in availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes)
                            {
                                for (int i = 0; i < items.PaxCount; i++)
                                {


                                    passkeytypeobj.passengerKey = a.ToString();
                                    //if (items.PaxType == "ADT")
                                    //{
                                    passkeytypeobj.passengerTypeCode = items.PaxType;
                                    //}

                                    passkeylist.Add(passkeytypeobj);
                                    a++;
                                }
                                //passengerkey12 = passkeytypeobj.passengerKey;


                            }

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
