﻿using Bookingmanager_;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static DomainLayer.Model.SeatMapResponceModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System;
using Utility;

namespace OnionConsumeWebAPI.Controllers
{
    public class SpiceJetResultFlightViewController : Controller
    {
        Logs logs = new Logs();
        PaxPriceType[] getPaxdetails(int adult_, int child_, int infant_)
        {
            PaxPriceType[] paxPriceTypes = null;
            try
            {
                int idx = 0;
                if (adult_>0) idx++;
                if(child_>0) idx++; 
                if(infant_>0) idx++;

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

        //PaxPriceType[] getPaxdetails(int adult_, int child_, int infant_)
        //{
        //    PaxPriceType[] paxPriceTypes = null;
        //    try
        //    {
        //        int i = 0;
        //        if (adult_ > 0) i++;
        //        if (child_ > 0) i++;

        //        paxPriceTypes = new PaxPriceType[i];

        //        if (adult_ > 0)
        //        {
        //            paxPriceTypes[0] = new PaxPriceType();
        //            paxPriceTypes[0].PaxType = "ADT";
        //            paxPriceTypes[0].PaxCountSpecified = true;
        //            paxPriceTypes[0].PaxCount = Convert.ToInt16(adult_);
        //        }
        //        if (child_ > 0)
        //        {
        //            paxPriceTypes[1] = new PaxPriceType();
        //            paxPriceTypes[1].PaxType = "CHD";
        //            paxPriceTypes[1].PaxCountSpecified = true;
        //            paxPriceTypes[1].PaxCount = Convert.ToInt16(child_);

        //        }


        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return paxPriceTypes;
        //}


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


        [HttpPost] // this APi is used to map trip data Amount
        public async Task<ActionResult> SpicejetTripsell(string fareKey, string journeyKey)
        {
            AAIdentifier AAIdentifierobj = null;
            TempData["farekey"] = fareKey;
            TempData["journeyKey"] = journeyKey;

            //List<_credentials> credentialslist = new List<_credentials>();
            using (HttpClient client = new HttpClient())
            {
                string tokenview = HttpContext.Session.GetString("SpicejetSignature");
                string token = tokenview.Replace(@"""", string.Empty);
                if (token == "" || token == null)
                {
                    return RedirectToAction("Index");
                }
                var Signature = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(tokenview);
                string stravailibitilityrequest = HttpContext.Session.GetString("SpicejetAvailibilityRequest");
                GetAvailabilityRequest availibiltyRQ = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);
                //GetItineraryPrice
                #region 
                PriceItineraryResponse _getPriceItineraryRS = null;
                PriceItineraryRequest _getPriceItineraryRQ = null;
                _getPriceItineraryRQ = new PriceItineraryRequest();
                _getPriceItineraryRQ.ItineraryPriceRequest = new ItineraryPriceRequest();
                _getPriceItineraryRQ.Signature = Signature;
                _getPriceItineraryRQ.ContractVersion = 420;
                _getPriceItineraryRQ.ItineraryPriceRequest.PriceItineraryBy = PriceItineraryBy.JourneyBySellKey;

                _getPriceItineraryRQ.ItineraryPriceRequest.BookingStatus = default;
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest = new SellJourneyByKeyRequestData();
                SellKeyList _getSellKeyList = new SellKeyList();
                _getSellKeyList.JourneySellKey = journeyKey;
                _getSellKeyList.FareSellKey = fareKey;
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys = new SellKeyList[1];
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0] = new SellKeyList();
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].JourneySellKey = _getSellKeyList.JourneySellKey;
                //"SG~8169~ ~~DEL~12/10/2023 20:00~BOM~12/10/2023 22:05~~";
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].FareSellKey = _getSellKeyList.FareSellKey;
                //"0~V~ ~SG~VSAV~5511~~0~6~~X";
                // Changes for Adult child infant
                int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                int TotalCount = adultcount + childcount;
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxCount = Convert.ToInt16(TotalCount);
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.CurrencyCode = "INR";

                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxPriceType = getPaxdetails(adultcount, childcount, 0);
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.SourcePOS = GetPointOfSale();
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
                _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.IsAllotmentMarketFare = false;
                _getPriceItineraryRQ.ItineraryPriceRequest.SSRRequest = new SSRRequest();
                SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                _getPriceItineraryRS = await objSpiceJet.GetItineraryPriceAsync(_getPriceItineraryRQ);

                string str = JsonConvert.SerializeObject(_getPriceItineraryRS);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getPriceItineraryRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getPriceItineraryRS), "PriceIteniry");


                #endregion

                HttpContext.Session.SetString("journeySellKey", JsonConvert.SerializeObject(journeyKey));
                SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();

                var jsonData = TempData["SpiceJetPassengerModel"];
                _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());

                if (_getPriceItineraryRS != null)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                    //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                    //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                    var totalAmount = _getPriceItineraryRS.Booking.BookingSum.TotalCost;

                    var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

                    #region Itenary segment and legs
                    int journeyscount = _getPriceItineraryRS.Booking.Journeys.Length;
                    List<AAJourney> AAJourneyList = new List<AAJourney>();
                    for (int i = 0; i < journeyscount; i++)
                    {

                        AAJourney AAJourneyobj = new AAJourney();

                        //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                        //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                        AAJourneyobj.journeyKey = _getPriceItineraryRS.Booking.Journeys[i].JourneySellKey;

                        int segmentscount = _getPriceItineraryRS.Booking.Journeys[i].Segments.Length;
                        List<AASegment> AASegmentlist = new List<AASegment>();
                        for (int j = 0; j < segmentscount; j++)
                        {
                            AADesignator AADesignatorobj = new AADesignator();
                            AADesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[0].DepartureStation;
                            AADesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[segmentscount-1].ArrivalStation;
                            AADesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[0].STD;
                            AADesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[segmentscount-1].STA;
                            AAJourneyobj.designator = AADesignatorobj;


                            AASegment AASegmentobj = new AASegment();
                            //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                            //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                            AADesignator AASegmentDesignatorobj = new AADesignator();

                            AASegmentDesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].DepartureStation;
                            AASegmentDesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].ArrivalStation;
                            AASegmentDesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].STD;
                            AASegmentDesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].STA;
                            AASegmentobj.designator = AASegmentDesignatorobj;

                            int fareCount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares.Length;
                            List<AAFare> AAFarelist = new List<AAFare>();
                            for (int k = 0; k < fareCount; k++)
                            {
                                AAFare AAFareobj = new AAFare();
                                AAFareobj.fareKey = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                AAFareobj.productClass = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                var passengerFares = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                int passengerFarescount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                                for (int l = 0; l < passengerFarescount; l++)
                                {
                                    AAPassengerfare AAPassengerfareobj = new AAPassengerfare();
                                    AAPassengerfareobj.passengerType = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                    var serviceCharges1 = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                    int serviceChargescount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                    List<AAServicecharge> AAServicechargelist = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescount; m++)
                                    {
                                        AAServicecharge AAServicechargeobj = new AAServicecharge();

                                        AAServicechargeobj.amount = Convert.ToInt32(_getPriceItineraryRS.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);



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

                            AAIdentifierobj.identifier = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                            AAIdentifierobj.carrierCode = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                            AASegmentobj.identifier = AAIdentifierobj;

                            var leg = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs;
                            int legcount = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs.Length;
                            List<AALeg> AALeglist = new List<AALeg>();
                            for (int n = 0; n < legcount; n++)
                            {
                                AALeg AALeg = new AALeg();
                                //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                AADesignator AAlegDesignatorobj = new AADesignator();
                                AAlegDesignatorobj.origin = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                AAlegDesignatorobj.destination = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                AAlegDesignatorobj.departure = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                AAlegDesignatorobj.arrival = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                AALeg.designator = AAlegDesignatorobj;

                                AALeginfo AALeginfoobj = new AALeginfo();
                                AALeginfoobj.arrivalTerminal = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                AALeginfoobj.arrivalTime = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                AALeginfoobj.departureTerminal = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                AALeginfoobj.departureTime = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
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
                    var passanger = _getPriceItineraryRS.Booking.Passengers;
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
                            //if (items.PaxType == "ADT")
                            //{
                                passkeytypeobj.passengerTypeCode = items.PaxType;
                            //}

                            passkeylist.Add(passkeytypeobj);
                            a++;
                        }
                        //passengerkey12 = passkeytypeobj.passengerKey;

                        
                    }

                    AirAsiaTripResponceobj.journeys = AAJourneyList;
                    AirAsiaTripResponceobj.passengers = passkeylist;
                    AirAsiaTripResponceobj.passengerscount = passengercount;

                    HttpContext.Session.SetString("SGkeypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));

                    //}

                    #region SpiceJetSellRequest
                    SellResponse _getSellRS = null;
                    SellRequest _getSellRQ = null;
                    _getSellRQ = new SellRequest();
                    _getSellRQ.SellRequestData = new SellRequestData(); ;
                    _getSellRQ.Signature = Signature;
                    _getSellRQ.ContractVersion = 420;
                    _getSellRQ.SellRequestData.SellBy = SellBy.JourneyBySellKey;


                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest = new SellJourneyByKeyRequest();
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new SellJourneyByKeyRequestData();

                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.CurrencyCode = "INR";

                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new SellKeyList[1];
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new SellKeyList();

                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = _getSellKeyList.JourneySellKey;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = _getSellKeyList.FareSellKey;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType = getPaxdetails(adultcount, childcount, 0);
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS = GetPointOfSale();
                    
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCountSpecified = true;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCount = Convert.ToInt16(TotalCount);
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFare = false;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PreventOverLap = false;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ReplaceAllPassengersOnUpdate = false;
                    _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ApplyServiceBundle = ApplyServiceBundle.No;
                    _getSellRQ.SellRequestData.SellSSR = new SellSSR();

                    _getSellRS = await objSpiceJet.GetSellAsync(_getSellRQ);

                    str = JsonConvert.SerializeObject(_getSellRS);

                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest");


                    #endregion

                    #region ssravailability
                    AirAsiaTripResponceModel passeengerlist = null;
                    string passenger = HttpContext.Session.GetString("SGkeypassenger");
                    passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));

                    //availibiltyRQ

                    GetSSRAvailabilityForBookingRequest _req = new GetSSRAvailabilityForBookingRequest();
                    GetSSRAvailabilityForBookingResponse _res = new GetSSRAvailabilityForBookingResponse();
                    try
                    {
                        int segmentcount = 0;
                        journeyscount = passeengerlist.journeys.Count;
                        _req.Signature = token;
                        _req.ContractVersion = 420;
                        SSRAvailabilityForBookingRequest _SSRAvailabilityForBookingRequest = new SSRAvailabilityForBookingRequest();

                        

                        //List<AAJourney> AAJourneyList = new List<AAJourney>();
                        for (int i = 0; i < journeyscount; i++)
                        {
                            int segmentscount = passeengerlist.journeys[i].segments.Count;
                            _SSRAvailabilityForBookingRequest.SegmentKeyList = new LegKey[segmentscount];
                            for (int j = 0; j < segmentscount; j++)
                            {
                                int legcount = passeengerlist.journeys[i].segments[j].legs.Count;
                                for (int n = 0; n < legcount; n++)
                                {
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j] = new LegKey();
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].CarrierCode = passeengerlist.journeys[i].segments[j].identifier.carrierCode;
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].FlightNumber = passeengerlist.journeys[i].segments[j].identifier.identifier;
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureDateSpecified = true;
                                    //string strdate = Convert.ToDateTime(passengerdetails.departure).ToString("yyyy-MM-dd");
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureDate = Convert.ToDateTime(AirAsiaTripResponceobj.journeys[i].segments[j].designator.departure);//DateTime.ParseExact(strdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].ArrivalStation = AirAsiaTripResponceobj.journeys[i].segments[j].designator.destination;
                                    _SSRAvailabilityForBookingRequest.SegmentKeyList[j].DepartureStation = AirAsiaTripResponceobj.journeys[i].segments[j].designator.origin;
                                    segmentcount++;
                                }
                            }
                        }
                        //// for(int k=0;k< passeengerlist)
                        //LegKey[] _legkey =new LegKey[1];
                        //_legkey[0].CarrierCode = passengerdetails.carrierCode;
                        //_legkey[0].FlightNumber = passengerdetails.identifier;
                        //_legkey[0].DepartureDate = Convert.ToDateTime(passengerdetails.departure);
                        //_legkey[0].DepartureStation = passengerdetails.origin;
                        //_legkey[0].ArrivalStation = passengerdetails.destination;

                        //_SSRAvailabilityForBookingRequest.SegmentKeyList = new LegKey[journeyscount];




                        _SSRAvailabilityForBookingRequest.PassengerNumberList = new short[Convert.ToInt16(TotalCount)];//new short[1];
                        int paxCount = _SSRAvailabilityForBookingRequest.PassengerNumberList.Length;//passeengerlist.passengerscount;
                        for (int i = 0; i < paxCount; i++)
                        {
                            if (i > 0)
                                continue;
                            _SSRAvailabilityForBookingRequest.PassengerNumberList[i] = Convert.ToInt16(i);
                        }
                        _SSRAvailabilityForBookingRequest.InventoryControlled = true;
                        _SSRAvailabilityForBookingRequest.InventoryControlledSpecified = true;
                        _SSRAvailabilityForBookingRequest.NonInventoryControlled = true;
                        _SSRAvailabilityForBookingRequest.NonInventoryControlledSpecified = true;
                        _SSRAvailabilityForBookingRequest.SeatDependent = true;
                        _SSRAvailabilityForBookingRequest.SeatDependentSpecified = true;
                        _SSRAvailabilityForBookingRequest.NonSeatDependent = true;
                        _SSRAvailabilityForBookingRequest.NonSeatDependentSpecified = true;
                        _SSRAvailabilityForBookingRequest.CurrencyCode = "INR";
                        _SSRAvailabilityForBookingRequest.SSRAvailabilityMode = SSRAvailabilityMode.NonBundledSSRs;
                        _SSRAvailabilityForBookingRequest.SSRAvailabilityModeSpecified = true;
                        _req.SSRAvailabilityForBookingRequest = _SSRAvailabilityForBookingRequest;
                        objSpiceJet = new SpiceJetApiController();
                        _res = await objSpiceJet.GetSSRAvailabilityForBooking(_req);

                        string Str2 = JsonConvert.SerializeObject(_res);  //GetSSRAvailibility Response
                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_req) + "\n\n Response: " + JsonConvert.SerializeObject(_res), "GetSSRAvailabilityForBooking");


                        //******Vinay***********//
                        if (_res != null)
                        {


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
                                        if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].InventoryControlled == true)
                                        {
                                            int legSsrscount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList.Length;

                                            //SSRAvailabiltyLegssrlist = new List<legSsrs>();
                                            try
                                            {
                                                for (int i = 0; i < legSsrscount; i++)
                                                {

                                                    SSRAvailabiltyLegssrobj = new legSsrs();
                                                    SSRAvailabiltyLegssrobj.legKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.ToString();
                                                    legDetailsobj = new legDetails();
                                                    legDetailsobj.destination = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.ArrivalStation;
                                                    legDetailsobj.origin = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.DepartureStation;
                                                    legDetailsobj.departureDate = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.DepartureDate.ToString();
                                                    legidentifier legidentifierobj = new legidentifier();
                                                    legidentifierobj.identifier = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.FlightNumber;
                                                    legidentifierobj.carrierCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList[i].LegKey.CarrierCode;
                                                    legDetailsobj.legidentifier = legidentifierobj;

                                                    //var ssrscount = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs.Count;

                                                    //for (int j = 0; j < ssrscount; j++)
                                                    //{
                                                    childlegssrs legssrs = new childlegssrs();
                                                    legssrs.ssrCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode.ToString();
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
                                                        foreach (var items in _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges)
                                                        {
                                                            Amount += items.Amount;
                                                            passengersdetail.price = Math.Round(Amount).ToString(); //Ammount

                                                        }
                                                        passengersdetail.passengerKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PassengerNumberList.ToString();
                                                        passengersdetail.ssrKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode;
                                                        legpassengerslist.Add(passengersdetail);
                                                        legssrs.legpassengers = legpassengerslist;
                                                        legssrslist.Add(legssrs);
                                                    }


                                                }


                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                        }
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

                        //*********Vinay***********//
                    }
                    catch
                    {

                    }


                    #endregion

                    #region SeatMap

                    GetSeatAvailabilityRequest _getseatAvailabilityRequest = new GetSeatAvailabilityRequest();
                    GetSeatAvailabilityResponse _getSeatAvailabilityResponse = new GetSeatAvailabilityResponse();

                    _getseatAvailabilityRequest.Signature = Signature;
                    _getseatAvailabilityRequest.ContractVersion = 420;

                    SeatAvailabilityRequest _seatRequest = new SeatAvailabilityRequest();
                    List<GetSeatAvailabilityResponse> SeatGroup = new List<GetSeatAvailabilityResponse>();
                    for (int i = 0; i < AirAsiaTripResponceobj.journeys[0].segments.Count; i++)
                    {
                        _seatRequest = new SeatAvailabilityRequest();
                        _seatRequest.STDSpecified = true;
                        _seatRequest.STD = AirAsiaTripResponceobj.journeys[0].segments[i].designator.departure;
                        _seatRequest.DepartureStation = AirAsiaTripResponceobj.journeys[0].segments[i].designator.origin;
                        _seatRequest.ArrivalStation = AirAsiaTripResponceobj.journeys[0].segments[i].designator.destination;
                        _seatRequest.IncludeSeatFees = true;
                        _seatRequest.IncludeSeatFeesSpecified = true;
                        _seatRequest.SeatAssignmentModeSpecified = true;
                        _seatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                        _seatRequest.FlightNumber = AirAsiaTripResponceobj.journeys[0].segments[i].identifier.identifier;
                        _seatRequest.OverrideSTDSpecified = true;
                        _seatRequest.OverrideSTD = AirAsiaTripResponceobj.journeys[0].segments[i].designator.departure;
                        _seatRequest.CarrierCode = AirAsiaTripResponceobj.journeys[0].segments[i].identifier.carrierCode;
                        _getseatAvailabilityRequest.SeatAvailabilityRequest = _seatRequest;
                        _getSeatAvailabilityResponse = await objSpiceJet.GetseatAvaialbility(_getseatAvailabilityRequest);
                        SeatGroup.Add(_getSeatAvailabilityResponse);

                    }


                    str = JsonConvert.SerializeObject(SeatGroup);
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getseatAvailabilityRequest) + "\n\n Response: " + JsonConvert.SerializeObject(SeatGroup), "GetSeatAvailability");



                    // data[0].seatMap.decks['1'].compartments.Y.units[0].unitKey


                    if (SeatGroup != null)
                    {

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
                            //Seatmapobj.name = _getSeatAvailabilityResponse.SeatAvailabilityResponse.EquipmentInfos[x].Compartments[0].Seats[x].SeatDesignator;
                            Seatmapobj.arrivalStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].ArrivalStation;
                            Seatmapobj.departureStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].DepartureStation;
                            Seatmapobj.marketingCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].MarketingCode;
                            Seatmapobj.equipmentType = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentType;
                            Seatmapobj.equipmentTypeSuffix = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentTypeSuffix;
                            //doubt
                            //Seatmapobj.categorySG = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].EquipmentCategory.ToString();
                            //doubt
                            //Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;
                            Decks Decksobj = new Decks();
                            Decksobj.availableUnits = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].AvailableUnits;
                            Decksobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].CompartmentDesignator;
                            Decksobj.length = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Length;
                            Decksobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Width;
                            Decksobj.sequence = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Sequence;
                            Decksobj.orientation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Orientation;
                            Seatmapobj.decks = Decksobj;

                            int compartmentsunitCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats.Length;
                            List<Unit> compartmentsunitlist = new List<Unit>();
                            for (int i = 0; i < compartmentsunitCount; i++)
                            {
                                Unit compartmentsunitobj = new Unit();
                                //doubt
                                compartmentsunitobj.assignable = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Assignable;
                                compartmentsunitobj.availability = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatAvailability);
                                compartmentsunitobj.compartmentDesignator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].CompartmentDesignator;
                                compartmentsunitobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatDesignator;
                                compartmentsunitobj.type = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatGroup);
                                compartmentsunitobj.travelClassCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].TravelClassCode;
                                compartmentsunitobj.set = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatSet;
                                compartmentsunitobj.group = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatGroup;
                                compartmentsunitobj.priority = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Priority;
                                compartmentsunitobj.text = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Text;
                                //compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                                compartmentsunitobj.angle = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].SeatAngle;
                                compartmentsunitobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Width;
                                compartmentsunitobj.height = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Height;
                                compartmentsunitobj.zone = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Zone;
                                compartmentsunitobj.x = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].X;
                                compartmentsunitobj.y = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].Y;

                                for (int k = 0; k < SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length; k++)
                                {
                                    if(compartmentsunitobj.group == Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].SeatGroup))
                                    {
                                        var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges.Length;

                                        List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                                        for (int l = 0; l < feesgroupserviceChargescount; l++)
                                        {
                                            Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                                            compartmentsunitobj.servicechargefeeAmount += Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges[l].Amount);
                                        }
                                    }
                                }
                                //if (compartmentsunitobj.assignable == true)
                                //{
                                compartmentsunitobj.unitKey = compartmentsunitobj.designator;

                                compartmentsunitlist.Add(compartmentsunitobj);
                                //}

                                int compartmentypropertiesCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList.Length;
                                List<Properties> Propertieslist = new List<Properties>();
                                for (int j = 0; j < compartmentypropertiesCount; j++)
                                {
                                    Properties compartmentyproperties = new Properties();
                                    compartmentyproperties.code = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList[j].TypeCode;
                                    compartmentyproperties.value = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats[i].PropertyList[j].Value;
                                    Propertieslist.Add(compartmentyproperties);
                                }

                                compartmentsunitobj.properties = Propertieslist;
                                Decksobj.units = compartmentsunitlist;
                            }

                            //var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                            //var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                            //string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                            // RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;

                            //string seatgroup = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].Compartments[0].Seats[i].SeatGroup.ToString();

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
                #endregion

                return RedirectToAction("SpiceJetSaverTripsell", "SGTripsell");

                //return View();
            }
        }
    }
}
