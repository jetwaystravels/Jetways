using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static DomainLayer.Model.SeatMapResponceModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System;
using Utility;
using IndigoBookingManager_;
using static DomainLayer.Model.ReturnTicketBooking;
using OnionArchitectureAPI.Services.Indigo;
using System.Collections;
namespace OnionConsumeWebAPI.Controllers
{
    public class IndigoResultFlightViewController : Controller
    {
        Logs logs = new Logs();
        PaxPriceType[] getPaxdetails(int adult_, int child_, int infant_)
        {
            PaxPriceType[] paxPriceTypes = null;
            try
            {
                int idx = 0;
                if (adult_ > 0) idx++;
                if (child_ > 0) idx++;
                if (infant_ > 0) idx++;

                paxPriceTypes = new PaxPriceType[idx];

                int arrCount = 0;
                for (int cntAdt = 0; cntAdt < adult_; cntAdt++)
                {
                    if (cntAdt > 0) continue;
                    paxPriceTypes[arrCount] = new PaxPriceType();
                    paxPriceTypes[arrCount].PaxType = "ADT";
                    paxPriceTypes[arrCount].PaxCountSpecified = true;
                    paxPriceTypes[arrCount].PaxCount = Convert.ToInt16(adult_);
                    // paxPriceTypes[arrCount].PaxDiscountCode = "true";
                    arrCount++;
                }
                for (int cntChd = 0; cntChd < child_; cntChd++)
                {
                    if (cntChd > 0) continue;
                    paxPriceTypes[arrCount] = new PaxPriceType();
                    paxPriceTypes[arrCount].PaxType = "CHD";
                    paxPriceTypes[arrCount].PaxCountSpecified = true;
                    paxPriceTypes[arrCount].PaxCount = Convert.ToInt16(child_);
                    // paxPriceTypes[arrCount].PaxDiscountCode = "true";
                    arrCount++;
                }
                for (int cntInf = 0; cntInf < infant_; cntInf++)
                {
                    paxPriceTypes[arrCount] = new PaxPriceType();
                    paxPriceTypes[arrCount].PaxType = "INFT";
                    arrCount++;
                }
            }
            catch (Exception e)
            {
            }

            return paxPriceTypes;
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


        //public PointOfSale GetPointOfSale()
        //{
        //    PointOfSale SourcePOS = null;
        //    try
        //    {
        //        SourcePOS = new PointOfSale();
        //        SourcePOS.State = IndigoBookingManager_.MessageState.New;
        //        SourcePOS.OrganizationCode = "";
        //        SourcePOS.AgentCode = "AG";
        //        SourcePOS.LocationCode = "";
        //        SourcePOS.DomainCode = "WWW";
        //    }
        //    catch (Exception e)
        //    {
        //        string exp = e.Message;
        //        exp = null;
        //    }

        //    return SourcePOS;
        //}


        [HttpPost] // this APi is used to map trip data Amount
        public async Task<ActionResult> IndigoTripsell(string fareKey, string journeyKey)
        {
            AAIdentifier AAIdentifierobj = null;
            TempData["farekey"] = fareKey;
            TempData["journeyKey"] = journeyKey;

            //List<_credentials> credentialslist = new List<_credentials>();
            using (HttpClient client = new HttpClient())
            {
                int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                int TotalCount = adultcount + childcount;
                string str3 = string.Empty;
                string tokenview = HttpContext.Session.GetString("IndigoSignature");
                string Signature = tokenview.Replace(@"""", string.Empty);
                if (Signature == "" || Signature == null)
                {
                    return RedirectToAction("Index");
                }
                //var Signature = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(token);
                string stravailibitilityrequest = HttpContext.Session.GetString("IndigoAvailibilityRequest");
                GetAvailabilityRequest availibiltyRQ = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                #region IndigoSellRequest
                _sell objsell = new _sell();
                IndigoBookingManager_.SellResponse _getSellRS = await objsell.Sell(Signature, journeyKey, fareKey, "", "", TotalCount, adultcount, childcount, infantcount, "OneWay");
                string str = JsonConvert.SerializeObject(_getSellRS);
                #endregion

                #region GetState

                IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS1 = await objsell.GetBookingFromState(Signature, "OneWay");

                str3 = JsonConvert.SerializeObject(_GetBookingFromStateRS1);

                if (_GetBookingFromStateRS1 != null)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                    //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                    //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                    var totalAmount = _GetBookingFromStateRS1.BookingData.BookingSum.TotalCost;

                    var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

                    #region Itenary segment and legs
                    int journeyscount = _GetBookingFromStateRS1.BookingData.Journeys.Length;
                    List<AAJourney> AAJourneyList = new List<AAJourney>();
                    AASegment AASegmentobj = null;
                    List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                    for (int i = 0; i < journeyscount; i++)
                    {

                        AAJourney AAJourneyobj = new AAJourney();

                        //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                        //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                        AAJourneyobj.journeyKey = _GetBookingFromStateRS1.BookingData.Journeys[i].JourneySellKey;

                        int segmentscount = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments.Length;
                        List<AASegment> AASegmentlist = new List<AASegment>();
                        for (int j = 0; j < segmentscount; j++)
                        {
                            AAPassengerfarelist = new List<AAPassengerfare>();
                            AADesignator AADesignatorobj = new AADesignator();

                            if (j == 1)
                            {
                                AADesignatorobj.origin = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].DepartureStation;
                                AADesignatorobj.destination = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].ArrivalStation;
                                AADesignatorobj.departure = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].STD;
                                AADesignatorobj.arrival = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].STA;

                            }
                            else
                            {

                                AADesignatorobj.origin = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].DepartureStation;
                                AADesignatorobj.destination = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].ArrivalStation;
                                AADesignatorobj.departure = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].STD;
                                AADesignatorobj.arrival = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[0].STA;

                            }
                            AAJourneyobj.designator = AADesignatorobj;


                            AASegmentobj = new AASegment();
                            //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                            //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                            AADesignator AASegmentDesignatorobj = new AADesignator();

                            AASegmentDesignatorobj.origin = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].DepartureStation;
                            AASegmentDesignatorobj.destination = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].ArrivalStation;
                            AASegmentDesignatorobj.departure = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].STD;
                            AASegmentDesignatorobj.arrival = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].STA;
                            AASegmentobj.designator = AASegmentDesignatorobj;

                            int fareCount = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares.Length;
                            List<AAFare> AAFarelist = new List<AAFare>();
                            for (int k = 0; k < fareCount; k++)
                            {
                                AAFare AAFareobj = new AAFare();
                                AAFareobj.fareKey = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                AAFareobj.productClass = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].ProductClass;

                                var passengerFares = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares;

                                int passengerFarescount = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                //List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                                for (int l = 0; l < passengerFarescount; l++)
                                {
                                    AAPassengerfare AAPassengerfareobj = new AAPassengerfare();
                                    AAPassengerfareobj.passengerType = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                    var serviceCharges1 = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                    int serviceChargescount = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                    List<AAServicecharge> AAServicechargelist = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescount; m++)
                                    {
                                        AAServicecharge AAServicechargeobj = new AAServicecharge();
                                        AAServicechargeobj.amount = Convert.ToInt32(_GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);
                                        AAServicechargeobj.code = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].ChargeCode;
                                        if (AAPassengerfareobj.passengerType.Equals("CHD") && AAServicechargeobj.code.Equals("20PRCT"))
                                        {
                                            if (AAServicechargelist[0].amount != null && AAServicechargeobj.amount != null)
                                            {
                                                AAServicechargelist[0].amount = AAServicechargelist[0].amount - AAServicechargeobj.amount;
                                            }
                                            continue;
                                        }


                                        AAServicechargelist.Add(AAServicechargeobj);
                                    }



                                    AAPassengerfareobj.serviceCharges = AAServicechargelist;

                                    AAPassengerfarelist.Add(AAPassengerfareobj);

                                }
                                AAFareobj.passengerFares = AAPassengerfarelist;

                                AAFarelist.Add(AAFareobj);




                            }
                            AASegmentobj.fares = AAFarelist;
                            AAIdentifierobj = new AAIdentifier();

                            AAIdentifierobj.identifier = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                            AAIdentifierobj.carrierCode = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                            AASegmentobj.identifier = AAIdentifierobj;

                            var leg = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs;
                            int legcount = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs.Length;
                            List<AALeg> AALeglist = new List<AALeg>();
                            for (int n = 0; n < legcount; n++)
                            {
                                AALeg AALeg = new AALeg();
                                //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                AADesignator AAlegDesignatorobj = new AADesignator();

                                // queryorigin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                // querydestination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                //if (Citydata.GetAllcity().Where(x => x.cityCode == queryorigin).SingleOrDefault() != null)
                                //{
                                //    string origin = Citydata.GetAllcity().Where(x => x.cityCode == queryorigin).SingleOrDefault().cityName;
                                //    AAlegDesignatorobj.origin = origin;
                                //}
                                //else
                                //{
                                //    AAlegDesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                //}
                                //if (Citydata.GetAllcity().Where(x => x.cityCode == querydestination).SingleOrDefault() != null)
                                //{
                                //    string destination1 = Citydata.GetAllcity().Where(x => x.cityCode == querydestination).SingleOrDefault().cityName;
                                //    AAlegDesignatorobj.destination = destination1;
                                //}
                                //else
                                //    AAlegDesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;

                                AAlegDesignatorobj.origin = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                AAlegDesignatorobj.destination = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                AAlegDesignatorobj.departure = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].STD;
                                AAlegDesignatorobj.arrival = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].STA;
                                AALeg.designator = AAlegDesignatorobj;

                                AALeginfo AALeginfoobj = new AALeginfo();
                                AALeginfoobj.arrivalTerminal = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                AALeginfoobj.arrivalTime = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                AALeginfoobj.departureTerminal = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                AALeginfoobj.departureTime = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
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
                    //var passanger = _getPriceItineraryRS.Booking.Passengers;
                    //int passengercount = _getPriceItineraryRS.Booking.Passengers.Length;

                    //List<AAPassengers> passkeylist = new List<AAPassengers>();

                    //foreach (var items in _getPriceItineraryRS.Booking.Passengers)
                    //{
                    //    AAPassengers passkeytypeobj = new AAPassengers();
                    //    passkeytypeobj.passengerKey = items.PassengerNumber.ToString();
                    //    passkeytypeobj.passengerTypeCode = items.PassengerTypeInfo.PaxType;
                    //    //  passkeytypeobj.passengertypecount = items.Count;

                    //    passkeylist.Add(passkeytypeobj);


                    //    //passengerkey12 = passkeytypeobj.passengerKey;


                    //}


                    //var passanger = _getPriceItineraryRS.Booking.Passengers;
                    int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;

                    List<AAPassengers> passkeylist = new List<AAPassengers>();
                    int a = 0;
                    foreach (var items in availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes)
                    {
                        for (int i = 0; i < items.PaxCount; i++)
                        {

                            AAPassengers passkeytypeobj = new AAPassengers();
                            passkeytypeobj.passengerKey = a.ToString();
                            passkeytypeobj.passengerTypeCode = items.PaxType;
                            passkeylist.Add(passkeytypeobj);
                            a++;
                        }

                    }
                    //To do for basefare and taxes

                    int Adulttax = 0;
                    int childtax = 0;
                    if (AAJourneyList.Count > 0)
                    {
                        for (int i = 0; i < AAJourneyList[0].segments.Count; i++)
                        {
                            for (int k = 0; k < AAJourneyList[0].segments[i].fares.Count; k++)
                            {
                                for (int l = 0; l < AAJourneyList[0].segments[i].fares[k].passengerFares.Count; l++)
                                {
                                    if (AAJourneyList[0].segments[i].fares[k].passengerFares[l].passengerType == "ADT")
                                    {
                                        for (int i2 = 0; i2 < AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges.Count; i2++)
                                        {
                                            if (i2 == 0)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                Adulttax += AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges[i2].amount;
                                            }
                                        }
                                    }
                                    if (AAJourneyList[0].segments[i].fares[k].passengerFares[l].passengerType == "CHD")
                                    {
                                        for (int i2 = 0; i2 < AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges.Count; i2++)
                                        {
                                            if (i2 == 0)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                childtax += AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges[i2].amount;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    int basefaretax = 0;
                    if (Adulttax > 0)
                    {
                        basefaretax = Adulttax * adultcount;
                    }
                    if (childtax > 0)
                    {
                        basefaretax += childtax * childcount;
                    }
                    AirAsiaTripResponceobj.basefaretax = basefaretax;
                    AirAsiaTripResponceobj.journeys = AAJourneyList;
                    AirAsiaTripResponceobj.passengers = passkeylist;
                    AirAsiaTripResponceobj.passengerscount = passengercount;
                    //}
                    #endregion

                    #region  GetItineraryPrice

                    IndigoBookingManager_.PriceItineraryResponse _getPriceItineraryRS = await objsell.GetItineraryPrice(Signature, journeyKey, fareKey, "", "", TotalCount, adultcount, childcount, infantcount, "OneWay");
                    str = JsonConvert.SerializeObject(_getPriceItineraryRS);
                    #endregion

                    HttpContext.Session.SetString("journeySellKey", JsonConvert.SerializeObject(journeyKey));
                    SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();

                    var jsonData = HttpContext.Session.GetString("IndigoPassengerModel");
                    _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());
                    if (_getPriceItineraryRS != null)
                    {

                        #region SellSSrInfant
                        if (infantcount > 0)
                        {
                            SellResponse sellSsrResponse = null;

                            sellSsrResponse = await objsell.sellssrInft(Signature, _getPriceItineraryRS, infantcount, 0, "OneWay");

                            str3 = JsonConvert.SerializeObject(sellSsrResponse);

                            if (sellSsrResponse != null)
                            {
                                //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                                var JsonsellSsrResponse = sellSsrResponse;
                            }
                        }
                        #endregion
                    }


                    #region GetBookingFromState
                    IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS = await objsell.GetBookingFromState(Signature, "OneWay");

                    str3 = JsonConvert.SerializeObject(_GetBookingFromStateRS);

                    if (_GetBookingFromStateRS != null)
                    {
                        //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                        var JsonSellSSrInfant = _GetBookingFromStateRS;
                        int Inftbasefare = 0;
                        int Inftcount = 0;
                        int infttax = 0;
                        if (_GetBookingFromStateRS.BookingData.Passengers.Length > 0 && _GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees.Length > 0)
                        {
                            for (int i = 0; i < _GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges.Length; i++)
                            {
                                if (i == 0)
                                {
                                    Inftbasefare = Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount);
                                    Inftcount += Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers.Length);
                                    AirAsiaTripResponceobj.inftcount = Inftcount;
                                    AirAsiaTripResponceobj.inftbasefare = Inftbasefare;
                                }
                                else
                                {
                                    infttax += Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges[i].Amount);
                                }

                            }
                            AirAsiaTripResponceobj.infttax = infttax * infantcount;
                        }
                    }

                    #endregion

                    HttpContext.Session.SetString("SGkeypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));

                    #region ssravailability
                    AirAsiaTripResponceModel passeengerlist = null;
                    string passenger = HttpContext.Session.GetString("SGkeypassenger");
                    passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));

                    _GetSSR objssr = new _GetSSR();
                    IndigoBookingManager_.GetSSRAvailabilityForBookingResponse _res = await objssr.GetSSRAvailabilityForBooking(Signature, passeengerlist, TotalCount, "OneWay");
                    string Str2 = JsonConvert.SerializeObject(_res);

                    //******Vinay***********//
                    if (_res != null)
                    {
                        Hashtable htSSr = new Hashtable();
                        SpicejetMealImageList.GetAllmeal(htSSr);
                        // var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);


                        //  var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                        // ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                        //var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
                        //journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();
                        List<legSsrs> SSRAvailabiltyLegssrlist = new List<legSsrs>();

                        SSRAvailabiltyResponceModel SSRAvailabiltyResponceobj = new SSRAvailabiltyResponceModel();
                        int PaxssrListcount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[0].AvailablePaxSSRList.Length;
                        try
                        {
                            legSsrs SSRAvailabiltyLegssrobj = new legSsrs();
                            legDetails legDetailsobj = null;
                            List<childlegssrs> legssrslist = new List<childlegssrs>();
                            for (int i1 = 0; i1 < _res.SSRAvailabilityForBookingResponse.SSRSegmentList.Length; i1++)
                            {
                                legssrslist = new List<childlegssrs>();
                                for (int j = 0; j < _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList.Length; j++)
                                {
                                    //if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].InventoryControlled == true)
                                    //{
                                    //int legSsrscount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList.Length;

                                    //SSRAvailabiltyLegssrlist = new List<legSsrs>();
                                    try
                                    {
                                        //for (int i = 0; i < legSsrscount; i++)
                                        //{

                                        SSRAvailabiltyLegssrobj = new legSsrs();
                                        SSRAvailabiltyLegssrobj.legKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.ToString();
                                        legDetailsobj = new legDetails();
                                        legDetailsobj.destination = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.ArrivalStation;
                                        legDetailsobj.origin = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.DepartureStation;
                                        legDetailsobj.departureDate = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.DepartureDate.ToString();
                                        legidentifier legidentifierobj = new legidentifier();
                                        legidentifierobj.identifier = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.FlightNumber;
                                        legidentifierobj.carrierCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.CarrierCode;
                                        legDetailsobj.legidentifier = legidentifierobj;

                                        //var ssrscount = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs.Count;

                                        //for (int j = 0; j < ssrscount; j++)
                                        //{
                                        childlegssrs legssrs = new childlegssrs();
                                        legssrs.ssrCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode.ToString();
                                        if (htSSr[legssrs.ssrCode] != null)
                                        {
                                            legssrs.name = htSSr[legssrs.ssrCode].ToString();
                                        }
                                        else
                                            continue;
                                        //legssrs.ssrType = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[0].AvailablePaxSSRList[j].ssrType;
                                        //legssrs.name = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].name;
                                        //legssrs.limitPerPassenger = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].limitPerPassenger;
                                        legssrs.available = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].Available;
                                        if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0)
                                        {
                                            legssrs.feeCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.FeeCode;
                                            List<legpassengers> legpassengerslist = new List<legpassengers>();
                                            Decimal Amount = decimal.Zero;
                                            legpassengers passengersdetail = new legpassengers();
                                            int i2 = 0;
                                            foreach (var items in _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges)
                                            {
                                                if (i2 > 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Amount += items.Amount;
                                                    passengersdetail.price = Math.Round(Amount).ToString(); //Ammount
                                                }
                                                i2++;
                                            }
                                            passengersdetail.passengerKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PassengerNumberList.ToString();
                                            passengersdetail.ssrKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode;
                                            passengersdetail.Airline = Airlines.Indigo;
                                            legpassengerslist.Add(passengersdetail);
                                            legssrs.legpassengers = legpassengerslist;
                                            legssrslist.Add(legssrs);
                                        }
                                        //}
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                    //}
                                }
                                SSRAvailabiltyLegssrobj.legDetails = legDetailsobj;
                                SSRAvailabiltyLegssrobj.legssrs = legssrslist;
                                SSRAvailabiltyLegssrlist.Add(SSRAvailabiltyLegssrobj);
                            }

                        }
                        catch (Exception ex)
                        {

                        }


                        SSRAvailabiltyResponceobj.legSsrs = SSRAvailabiltyLegssrlist;
                        HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));

                    }

                    #endregion

                    #region SeatMap

                    objssr = new _GetSSR();
                    List<IndigoBookingManager_.GetSeatAvailabilityResponse> SeatGroup = await objssr.GetseatAvailability(Signature, AirAsiaTripResponceobj, "OneWay");
                    if (SeatGroup != null)
                    {
                        string columncount0 = string.Empty;
                        //string columncount1 = string.Empty;
                        var data = SeatGroup.Count;// _getSeatAvailabilityResponse.SeatAvailabilityResponse.EquipmentInfos.Length;

                        List<data> datalist = new List<data>();
                        SeatMapResponceModel SeatMapResponceModel = new SeatMapResponceModel();
                        List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();
                        for (int x = 0; x < data; x++)
                        {
                            data dataobj = new data();

                            SeatMapResponceModel = new SeatMapResponceModel();
                            SeatMapResponceModellist = new List<SeatMapResponceModel>();
                            Fees Fees = new Fees();
                            Seatmap Seatmapobj = new Seatmap();
                            Seatmapobj.name = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Name;
                            TempData["AirCraftName"] = Seatmapobj.name;
                            Seatmapobj.arrivalStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].ArrivalStation;
                            Seatmapobj.departureStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].DepartureStation;
                            Seatmapobj.marketingCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].MarketingCode;
                            Seatmapobj.equipmentType = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentType;
                            Seatmapobj.equipmentTypeSuffix = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentTypeSuffix;
                            //doubt
                            //Seatmapobj.categorySG = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].EquipmentCategory.ToString();
                            //doubt
                            //Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;
                           
                            

                            int compartmentsunitCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments.Length;
                            List<Unit> compartmentsunitlist = new List<Unit>();
                            //List<Decks> Decksobjarray = new List<Decks>();
                            Seatmapobj.decksindigo = new List<Decks>();
                            Decks Decksobj = null;
                            for (int i = 0; i < compartmentsunitCount; i++) // 2 times 
                            {
                                compartmentsunitlist = new List<Unit>();
                                Decksobj = new Decks();
                                Decksobj.availableUnits = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].AvailableUnits;
                                Decksobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].CompartmentDesignator;
                                Decksobj.length = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Length;
                                Decksobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Width;
                                Decksobj.sequence = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Sequence;
                                Decksobj.orientation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Orientation;

                                for (int i1 = 0; i1 < SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats.Length; i1++)
                                {
                                    try
                                    {
                                        Unit compartmentsunitobj = new Unit();
                                        //doubt
                                        compartmentsunitobj.Airline = Airlines.Indigo;
                                        compartmentsunitobj.assignable = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Assignable;
                                        compartmentsunitobj.availability = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatAvailability);
                                        compartmentsunitobj.compartmentDesignator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].CompartmentDesignator;
                                        compartmentsunitobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatDesignator;
                                        compartmentsunitobj.type = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatGroup);
                                        compartmentsunitobj.travelClassCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].TravelClassCode;
                                        compartmentsunitobj.set = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatSet;
                                        compartmentsunitobj.group = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatGroup;
                                        compartmentsunitobj.priority = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Priority;
                                        compartmentsunitobj.text = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Text;
                                        //compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                                        compartmentsunitobj.angle = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatAngle;
                                        compartmentsunitobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Width;
                                        compartmentsunitobj.height = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Height;
                                        compartmentsunitobj.zone = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Zone;
                                        compartmentsunitobj.x = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].X;
                                        compartmentsunitobj.y = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Y;

                                        for (int k = 0; k < SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length; k++)
                                        {
                                            if (compartmentsunitobj.group == Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].SeatGroup))
                                            {
                                                var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges.Length;

                                                List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                                                for (int l = 0; l < feesgroupserviceChargescount; l++)
                                                {
                                                    //Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                                    if (l > 0)
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        compartmentsunitobj.servicechargefeeAmount += Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges[l].Amount);
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        //if (compartmentsunitobj.assignable == true)
                                        //{
                                        compartmentsunitobj.unitKey = compartmentsunitobj.designator;

                                       
                                        //}

                                        int compartmentypropertiesCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList.Length;
                                        List<Properties> Propertieslist = new List<Properties>();
                                        for (int j = 0; j < compartmentypropertiesCount; j++)
                                        {
                                            Properties compartmentyproperties = new Properties();
                                            compartmentyproperties.code = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList[j].TypeCode;
                                            compartmentyproperties.value = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList[j].Value;
                                            Propertieslist.Add(compartmentyproperties);
                                        }

                                        compartmentsunitobj.properties = Propertieslist;
                                        if(compartmentsunitobj.designator.Contains('$'))
                                        {
                                            columncount0 = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1-1].SeatDesignator;
                                            break;
                                        }

                                        compartmentsunitlist.Add(compartmentsunitobj);


                                        
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    
                                }
                                Decksobj.units = compartmentsunitlist;
                                Seatmapobj.SeatColumnCount= Regex.Replace(columncount0, "[^0-9]", "");
                                //Decksobjarray.Add(Decksobj);
                                //Decksobj.units = compartmentsunitlist;
                                Seatmapobj.decksindigo.Add(Decksobj);
                            }
                            

                            //var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                            //var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                            //string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                            // RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;

                            //string seatgroup = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].Compartments[i].Seats[i].SeatGroup.ToString();

                            List<Groups> GroupsFeelist = new List<Groups>();

                            int testcount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length;
                            for (int i = 0; i < testcount; i++)
                            {
                                Groups Groupsobj = new Groups();
                                GroupsFee GroupsFeeobj = new GroupsFee();
                                string feeseatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                                //if (seatgroup == feeseatGroup)
                                //{
                                //doubt
                                GroupsFeeobj.SeatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                                GroupsFeeobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeNumber;
                                GroupsFeeobj.ssrCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRCode;
                                GroupsFeeobj.ssrNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRNumber;
                                GroupsFeeobj.paymentNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.PaymentNumber;
                                GroupsFeeobj.isConfirmed = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                GroupsFeeobj.isConfirming = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                GroupsFeeobj.isConfirmingExternal = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                                GroupsFeeobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeCode;
                                GroupsFeeobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeDetail;
                                //Dout
                                // GroupsFeeobj.passengerFeeKey = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].passengerFeeKey;
                                GroupsFeeobj.flightReference = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FlightReference;
                                GroupsFeeobj.note = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.Note;
                                GroupsFeeobj.createdDate = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.CreatedDate;
                                GroupsFeeobj.isProtected = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.IsProtected;

                                var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges.Length;

                                List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                                for (int l = 0; l < feesgroupserviceChargescount; l++)
                                {
                                    Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                    feesgroupserviceChargesobj.amount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].Amount);
                                    feesgroupserviceChargesobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeCode; ;
                                    feesgroupserviceChargesobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeDetail;
                                    // feesgroupserviceChargesobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeType;
                                    // feesgroupserviceChargesobj.collectType = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CollectType;
                                    feesgroupserviceChargesobj.currencyCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CurrencyCode;

                                    feesgroupserviceChargesobj.foreignAmount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ForeignAmount);
                                    feesgroupserviceChargesobj.ticketCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].TicketCode;
                                    feesgroupserviceChargeslist.Add(feesgroupserviceChargesobj);

                                }
                                GroupsFeeobj.serviceCharges = feesgroupserviceChargeslist;

                                Groupsobj.groupsFee = GroupsFeeobj;
                                GroupsFeelist.Add(Groupsobj);

                                Fees.groups = GroupsFeelist;
                                //break;

                                //}
                                //else
                                //{
                                //    continue;

                                //}
                            }
                            dataobj.seatMap = Seatmapobj;
                            dataobj.seatMapfees = Fees;
                            datalist.Add(dataobj);
                            SeatMapResponceModel.datalist = datalist;
                        }
                        string strseat = JsonConvert.SerializeObject(SeatMapResponceModel);
                        HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));

                    }
                }
                return RedirectToAction("IndigoSaverTripsell", "IndigoTripsell");
            }
        }
        #endregion
        //return View();
    }
}
