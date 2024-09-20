using static DomainLayer.Model.SeatMapResponceModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System;
using Utility;
using IndigoBookingManager_;
using static DomainLayer.Model.ReturnTicketBooking;
//using OnionArchitectureAPI.Services.Indigo;
using System.Collections;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using OnionArchitectureAPI.Services.Travelport;
using OnionConsumeWebAPI.Extensions;
using System.Text;
using static DomainLayer.Model.GDSResModel;

namespace OnionConsumeWebAPI.Controllers.TravelClick
{
    public class GDSResultFlightViewController : Controller
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

        [HttpPost] // this APi is used to map trip data Amount
        public async Task<ActionResult> GDSTripsell(string fareKey, string journeyKey)
        {
            AAIdentifier AAIdentifierobj = null;
            string _targetBranch = string.Empty;
            string _userName = string.Empty;
            string _password = string.Empty;
            TempData["farekey"] = fareKey;
            TempData["journeyKey"] = journeyKey;
            dynamic Airfaredata = null;
            if (!string.IsNullOrEmpty(fareKey))
            {
                Airfaredata = JsonConvert.DeserializeObject<dynamic>(fareKey);
            }
            //List<_credentials> credentialslist = new List<_credentials>();
            using (HttpClient client = new HttpClient())
            {
                int adultcount = Convert.ToInt32(HttpContext.Session.GetString("adultCount"));
                int childcount = Convert.ToInt32(HttpContext.Session.GetString("childCount"));
                int infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                int TotalCount = adultcount + childcount;
                string str3 = string.Empty;
                string tokenview = HttpContext.Session.GetString("GDSTraceid");
                if (tokenview == null) { tokenview = ""; }
                string newGuid = tokenview.Replace(@"""", string.Empty);
                if (newGuid == "" || newGuid == null)
                {
                    return RedirectToAction("Index");
                }
                //var Signature = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(token);
                string stravailibitilityrequest = HttpContext.Session.GetString("GDSAvailibilityRequest");
                SimpleAvailabilityRequestModel availibiltyRQGDS = Newtonsoft.Json.JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(stravailibitilityrequest);

                GetAvailabilityRequest availibiltyRQ = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                #region GDSAirPricelRequest

                TravelPort _objAvail = null;

                HttpContextAccessor httpContextAccessorInstance = new HttpContextAccessor();
                _objAvail = new TravelPort(httpContextAccessorInstance);
                string _testURL = AppUrlConstant.GDSURL;
                //string _targetBranch = string.Empty;
                //string _userName = string.Empty;
                //string _password = string.Empty;
                _targetBranch = "P7027135";
                _userName = "Universal API/uAPI5098257106-beb65aec";
                _password = "Q!f5-d7A3D";
                StringBuilder fareRepriceReq = new StringBuilder();
                string res = _objAvail.AirPriceGet(_testURL, fareRepriceReq, availibiltyRQGDS, newGuid.ToString(), _targetBranch, _userName, _password, Airfaredata, "GDSOneWay");

                TravelPortParsing _objP = new TravelPortParsing();
                List<GDSResModel.Segment> getAirPriceRes = new List<GDSResModel.Segment>();
                if (res != null && !res.Contains("Bad Request") && !res.Contains("Internal Server Error"))
                {
                    getAirPriceRes = _objP.ParseAirFareRsp(res, "OneWay", availibiltyRQGDS);
                }
                if(getAirPriceRes.Count>0)
                {
                    HttpContext.Session.SetString("Total", getAirPriceRes[0].Fare.TotalFareWithOutMarkUp.ToString());
                }
                //_sell objsell = new _sell();
                //IndigoBookingManager_.SellResponse _getSellRS = null;// await objsell.Sell(Signature, journeyKey, fareKey, "", "", TotalCount, adultcount, childcount, infantcount, "OneWay");
                string str = JsonConvert.SerializeObject(getAirPriceRes);
                #endregion

                #region GetState
                //IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS1 = null;// await objsell.GetBookingFromState(Signature, "OneWay");
                //str3 = JsonConvert.SerializeObject(_GetBookingFromStateRS1);
                if (getAirPriceRes != null && getAirPriceRes.Count > 0)
                {
                    AirAsiaTripResponceModel AirAsiaTripResponceobj = new AirAsiaTripResponceModel();
                    #region Itenary segment and legs
                    int journeyscount = getAirPriceRes.Count;// _GetBookingFromStateRS1.BookingData.Journeys.Length;
                    List<AAJourney> AAJourneyList = new List<AAJourney>();
                    AASegment AASegmentobj = null;
                    string paxType = String.Empty;
                    List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                    for (int i = 0; i < journeyscount; i++)
                    {
                        AAJourney AAJourneyobj = new AAJourney();
                        AAJourneyobj.journeyKey = ""; // _GetBookingFromStateRS1.BookingData.Journeys[i].JourneySellKey;
                        int segmentscount = getAirPriceRes[0].Bonds[0].Legs.Count;//.BookingData.Journeys[i].Segments.Length;
                        List<AASegment> AASegmentlist = new List<AASegment>();
                        for (int j = 0; j < segmentscount; j++)
                        {
                            AAPassengerfarelist = new List<AAPassengerfare>();
                            AADesignator AADesignatorobj = new AADesignator();
                            AADesignatorobj.origin = getAirPriceRes[0].Bonds[0].Legs[0].Origin;
                            AADesignatorobj.destination = getAirPriceRes[0].Bonds[0].Legs[j].Destination;
                            AADesignatorobj.departure = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[0].DepartureTime);
                            if (segmentscount>1)
                            {
                                AADesignatorobj.arrival = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[segmentscount - 1].ArrivalTime);
                            }
                            else
                            {
                                AADesignatorobj.arrival = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].ArrivalTime);
                            }

                            AAJourneyobj.designator = AADesignatorobj;
                            AASegmentobj = new AASegment();
                            AADesignator AASegmentDesignatorobj = new AADesignator();
                            AASegmentDesignatorobj.origin = getAirPriceRes[0].Bonds[0].Legs[j].Origin;
                            AASegmentDesignatorobj.destination = getAirPriceRes[0].Bonds[0].Legs[j].Destination;
                            AASegmentDesignatorobj.departure = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].DepartureTime);
                            AASegmentDesignatorobj.arrival = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].ArrivalTime);
                            AASegmentobj.designator = AASegmentDesignatorobj;

                            //int fareCount = getAirPriceRes[0].Fare.cou
                            List<AAFare> AAFarelist = new List<AAFare>();
                            //for (int k = 0; k < fareCount; k++)
                            //{
                            AAFare AAFareobj = new AAFare();
                            AAFareobj.fareKey = ""; //_GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].FareSellKey;
                            AAFareobj.productClass = journeyKey;// _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].ProductClass;
                                                                //var passengerFares = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares;
                            int passengerFarescount = getAirPriceRes[0].Fare.PaxFares.Count;// _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                            if (j == 0)                                                           //List<AAPassengerfare> AAPassengerfarelist = new List<AAPassengerfare>();
                            {

                                for (int l = 0; l < passengerFarescount; l++)
                                {
                                    if (getAirPriceRes[0].Fare.PaxFares[l].PaxType == PAXTYPE.ADT)
                                    {
                                        paxType = "ADT";
                                    }
                                    else if (getAirPriceRes[0].Fare.PaxFares[l].PaxType == PAXTYPE.CHD)
                                    {
                                        paxType = "CHD";
                                    }
                                    else if (getAirPriceRes[0].Fare.PaxFares[l].PaxType == PAXTYPE.INF)
                                    {
                                        paxType = "INF";
                                    }
                                    AAPassengerfare AAPassengerfareobj = new AAPassengerfare();
                                    AAPassengerfareobj.passengerType = paxType;
                                    //var serviceCharges1 = _GetBookingFromStateRS1.BookingData.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                    int serviceChargescount = getAirPriceRes[0].Fare.PaxFares[l].Fare.Count;
                                    List<AAServicecharge> AAServicechargelist = new List<AAServicecharge>();
                                    for (int m = 0; m < serviceChargescount; m++)
                                    {
                                        AAServicecharge AAServicechargeobj = new AAServicecharge();
                                        AAServicechargeobj.amount = Convert.ToInt32(getAirPriceRes[0].Fare.PaxFares[l].Fare[m].Amount);
                                        AAServicechargeobj.code = getAirPriceRes[0].Fare.PaxFares[l].Fare[m].ChargeCode;
                                        AAServicechargelist.Add(AAServicechargeobj);
                                    }
                                    AAPassengerfareobj.basicAmount = Convert.ToInt32(getAirPriceRes[0].Fare.PaxFares[l].BasicFare);
                                    AAPassengerfareobj.serviceCharges = AAServicechargelist;
                                    AAPassengerfarelist.Add(AAPassengerfareobj);
                                }
                            }
                            AAFareobj.passengerFares = AAPassengerfarelist;
                            AAFarelist.Add(AAFareobj);
                            //}
                            AASegmentobj.fares = AAFarelist;
                            AAIdentifierobj = new AAIdentifier();

                            AAIdentifierobj.identifier = getAirPriceRes[0].Bonds[0].Legs[j].FlightNumber;
                            AAIdentifierobj.carrierCode = getAirPriceRes[0].Bonds[0].Legs[j].CarrierCode;
                            if (AAIdentifierobj.carrierCode == "AI")
                            {
                                AAJourneyobj.Airlinename = "AirIndia";
                            }
                            if (AAIdentifierobj.carrierCode == "UK")
                            {
                                AAJourneyobj.Airlinename = "Vistara";
                            }
                            AASegmentobj.identifier = AAIdentifierobj;

                            var leg = getAirPriceRes[0].Bonds[0].Legs;
                            int legcount = getAirPriceRes[0].Bonds[0].Legs.Count;
                            List<AALeg> AALeglist = new List<AALeg>();
                            //for (int n = 0; n < legcount; n++)
                            //{
                            AALeg AALeg = new AALeg();
                            AADesignator AAlegDesignatorobj = new AADesignator();
                            AAlegDesignatorobj.origin = getAirPriceRes[0].Bonds[0].Legs[j].Origin;
                            AAlegDesignatorobj.destination = getAirPriceRes[0].Bonds[0].Legs[j].Destination;
                            AAlegDesignatorobj.departure = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].DepartureTime);
                            AAlegDesignatorobj.arrival = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].ArrivalTime);
                            AALeg.designator = AAlegDesignatorobj;

                            AALeginfo AALeginfoobj = new AALeginfo();
                            AALeginfoobj.arrivalTerminal = getAirPriceRes[0].Bonds[0].Legs[j].ArrivalTerminal;
                            AALeginfoobj.arrivalTime = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].ArrivalTime);
                            AALeginfoobj.departureTerminal = getAirPriceRes[0].Bonds[0].Legs[j].DepartureTerminal;
                            AALeginfoobj.departureTime = Convert.ToDateTime(getAirPriceRes[0].Bonds[0].Legs[j].DepartureTime);
                            AALeginfoobj.equipmentType = getAirPriceRes[0].Bonds[0].Legs[j]._Equipment;
                            AALeg.legInfo = AALeginfoobj;
                            AALeglist.Add(AALeg);

                            //}
                            AASegmentobj.legs = AALeglist;
                            AASegmentlist.Add(AASegmentobj);
                        }

                        AAJourneyobj.segments = AASegmentlist;
                        AAJourneyList.Add(AAJourneyobj);

                    }

                    #endregion
                    int passengercount = 0;
                    if (availibiltyRQGDS.passengercount != null)
                    {
                        passengercount = availibiltyRQGDS.passengercount.adultcount + availibiltyRQGDS.passengercount.childcount + availibiltyRQGDS.passengercount.infantcount;
                    }
                    else
                    {
                        passengercount = availibiltyRQGDS.adultcount + availibiltyRQGDS.childcount + availibiltyRQGDS.infantcount;
                    }
                    List<AAPassengers> passkeylist = new List<AAPassengers>();
                    int a = 0;
                    //foreach (var items in availibiltyRQGDS.passengers)
                    //{
                    int paxcount = 0;
                    for (int i = 0; i < getAirPriceRes[0].Fare.PaxFares.Count; i++)
                    {
                        if (getAirPriceRes[0].Fare.PaxFares[i].PaxType == PAXTYPE.ADT)
                        {
                            paxType = "ADT";
                            if(availibiltyRQGDS.passengercount!=null)
                            {
                                paxcount = availibiltyRQGDS.passengercount.adultcount;
                            }
                            else
                            {
                                paxcount = availibiltyRQGDS.adultcount;
                            }
                            
                        }
                        else if (getAirPriceRes[0].Fare.PaxFares[i].PaxType == PAXTYPE.CHD)
                        {
                            paxType = "CHD";
                            if (availibiltyRQGDS.passengercount != null)
                            {
                                paxcount = availibiltyRQGDS.passengercount.childcount;
                            }
                            else
                            {
                                paxcount = availibiltyRQGDS.childcount;
                            }
                        }
                        else if (getAirPriceRes[0].Fare.PaxFares[i].PaxType == PAXTYPE.INF)
                        {
                            paxType = "INF";
                            if (availibiltyRQGDS.passengercount != null)
                            {
                                paxcount = availibiltyRQGDS.passengercount.infantcount;
                            }
                            else
                            {
                                paxcount = availibiltyRQGDS.infantcount;
                            }
                        }
                        for (int k = 0; k < paxcount; k++)
                        {

                            AAPassengers passkeytypeobj = new AAPassengers();
                            passkeytypeobj.passengerKey = a.ToString();
                            passkeytypeobj.passengerTypeCode = paxType; ;
                            passkeylist.Add(passkeytypeobj);
                            a++;
                        }
                    }
                    //}

                    //}
                    //To do for basefare and taxes

                    int Adulttax = 0;
                    int childtax = 0;
                    int Inftbasefare = 0;
                    int Inftcount = 0;
                    int infttax = 0;
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
                                            Adulttax += AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges[i2].amount;

                                        }
                                    }
                                    if (AAJourneyList[0].segments[i].fares[k].passengerFares[l].passengerType == "CHD")
                                    {
                                        for (int i2 = 0; i2 < AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges.Count; i2++)
                                        {

                                            childtax += AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges[i2].amount;
                                        }
                                    }

                                    if (AAJourneyList[0].segments[i].fares[k].passengerFares[l].passengerType == "INF")
                                    {
                                        if (availibiltyRQGDS.passengercount != null)
                                        {
                                            Inftcount = availibiltyRQGDS.passengercount.infantcount;
                                        }
                                        else
                                        {
                                            Inftcount = availibiltyRQGDS.infantcount;
                                        }
                                        for (int i2 = 0; i2 < AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges.Count; i2++)
                                        {

                                            infttax += AAJourneyList[0].segments[i].fares[k].passengerFares[l].serviceCharges[i2].amount;
                                        }
                                        Inftbasefare = Convert.ToInt32(AAJourneyList[0].segments[i].fares[k].passengerFares[l].basicAmount);
                                        //Inftcount += Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers.Length);
                                        AirAsiaTripResponceobj.inftcount = Inftcount;
                                        AirAsiaTripResponceobj.inftbasefare = Inftbasefare;
                                    }
                                }
                            }
                        }
                    }

                    int basefaretax = 0;
                    int basefareInfttax = 0;
                    if (Adulttax > 0)
                    {
                        basefaretax = Adulttax * adultcount;
                    }
                    if (childtax > 0)
                    {
                        basefaretax += childtax * childcount;
                    }
                    if (infttax > 0)
                    {
                        basefareInfttax = infttax * infantcount;
                        basefaretax+= infttax * infantcount;
                    }
                    AirAsiaTripResponceobj.basefaretax = basefaretax;
                    AirAsiaTripResponceobj.journeys = AAJourneyList;
                    AirAsiaTripResponceobj.passengers = passkeylist;
                    AirAsiaTripResponceobj.passengerscount = passengercount;
                    AirAsiaTripResponceobj.infttax = basefareInfttax;
                    HttpContext.Session.SetString("PricingSolutionValue_0", JsonConvert.SerializeObject(getAirPriceRes[0].PricingSolutionValue));
                    //AirAsiaTripResponceobj.PriceSolution = getAirPriceRes[0].PricingSolutionValue;
                    #endregion

                    HttpContext.Session.SetString("SGkeypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));

                    // HttpContext.Session.SetString("journeySellKey", JsonConvert.SerializeObject(journeyKey));
                    // SimpleAvailabilityRequestModel _SimpleAvailabilityobj = new SimpleAvailabilityRequestModel();

                    //var jsonData = HttpContext.Session.GetString("IndigoPassengerModel");
                    // _SimpleAvailabilityobj = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(jsonData.ToString());
                    //if (_getPriceItineraryRS != null)
                    //{

                    #region SellSSrInfant
                    //if (infantcount > 0)
                    //{
                    //    SellResponse sellSsrResponse = null;

                    //    //sellSsrResponse = await objsell.sellssrInft(Signature, _getPriceItineraryRS, infantcount, 0, "OneWay");

                    //    str3 = JsonConvert.SerializeObject(sellSsrResponse);

                    //    if (sellSsrResponse != null)
                    //    {
                    //        //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                    //        var JsonsellSsrResponse = sellSsrResponse;
                    //    }
                    //}
                    #endregion
                    //}


                    #region GetBookingFromState
                    //IndigoBookingManager_.GetBookingFromStateResponse _GetBookingFromStateRS = null;// await objsell.GetBookingFromState(Signature, "OneWay");

                    //str3 = JsonConvert.SerializeObject(_GetBookingFromStateRS);

                    //if (_GetBookingFromStateRS != null)
                    //{
                    //var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
                    //var JsonSellSSrInfant = _GetBookingFromStateRS;
                    //int Inftbasefare = 0;
                    //int Inftcount = 0;
                    //int infttax = 0;
                    //if (_GetBookingFromStateRS.BookingData.Passengers.Length > 0 && _GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees.Length > 0)
                    //{
                    //for (int i = 0; i < _GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges.Length; i++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                //Inftbasefare = Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges[0].Amount);
                    //                //Inftcount += Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers.Length);
                    //                //AirAsiaTripResponceobj.inftcount = Inftcount;
                    //                //AirAsiaTripResponceobj.inftbasefare = Inftbasefare;
                    //            }
                    //            else
                    //            {
                    //                infttax += Convert.ToInt32(_GetBookingFromStateRS.BookingData.Passengers[0].PassengerFees[0].ServiceCharges[i].Amount);
                    //            }

                    //        }
                    //        AirAsiaTripResponceobj.infttax = infttax * infantcount;
                    //}
                    //}

                    #endregion

                    //HttpContext.Session.SetString("SGkeypassenger", JsonConvert.SerializeObject(AirAsiaTripResponceobj));

                    #region ssravailability
                    //AirAsiaTripResponceModel passeengerlist = null;
                    //string passenger = HttpContext.Session.GetString("SGkeypassenger");
                    //passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));

                    ////_GetSSR objssr = new _GetSSR();
                    //IndigoBookingManager_.GetSSRAvailabilityForBookingResponse _res = null;// await objssr.GetSSRAvailabilityForBooking(Signature, passeengerlist, TotalCount, "OneWay");
                    //string Str2 = JsonConvert.SerializeObject(_res);

                    ////******Vinay***********//
                    //if (_res != null)
                    //{
                    //    Hashtable htSSr = new Hashtable();
                    //    SpicejetMealImageList.GetAllmeal(htSSr);
                    //    // var JsonObjresponseSSRAvailabilty = JsonConvert.DeserializeObject<dynamic>(_responseSSRAvailabilty);


                    //    //  var ssrKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].ssrs[0].passengersAvailability[passengerdetails.passengerkey].ssrKey;
                    //    // ssrKey = ((Newtonsoft.Json.Linq.JValue)ssrKey1).Value.ToString();
                    //    //var journeyKey1 = JsonObjresponseSSRAvailabilty.data.journeySsrs[0].journeyKey;
                    //    //journeyKey = ((Newtonsoft.Json.Linq.JValue)journeyKey1).Value.ToString();
                    //    List<legSsrs> SSRAvailabiltyLegssrlist = new List<legSsrs>();

                    //    SSRAvailabiltyResponceModel SSRAvailabiltyResponceobj = new SSRAvailabiltyResponceModel();
                    //    int PaxssrListcount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[0].AvailablePaxSSRList.Length;
                    //    try
                    //    {
                    //        legSsrs SSRAvailabiltyLegssrobj = new legSsrs();
                    //        legDetails legDetailsobj = null;
                    //        List<childlegssrs> legssrslist = new List<childlegssrs>();
                    //        for (int i1 = 0; i1 < _res.SSRAvailabilityForBookingResponse.SSRSegmentList.Length; i1++)
                    //        {
                    //            legssrslist = new List<childlegssrs>();
                    //            for (int j = 0; j < _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList.Length; j++)
                    //            {
                    //                //if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].InventoryControlled == true)
                    //                //{
                    //                //int legSsrscount = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRLegList.Length;

                    //                //SSRAvailabiltyLegssrlist = new List<legSsrs>();
                    //                try
                    //                {
                    //                    //for (int i = 0; i < legSsrscount; i++)
                    //                    //{

                    //                    SSRAvailabiltyLegssrobj = new legSsrs();
                    //                    SSRAvailabiltyLegssrobj.legKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.ToString();
                    //                    legDetailsobj = new legDetails();
                    //                    legDetailsobj.destination = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.ArrivalStation;
                    //                    legDetailsobj.origin = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.DepartureStation;
                    //                    legDetailsobj.departureDate = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.DepartureDate.ToString();
                    //                    legidentifier legidentifierobj = new legidentifier();
                    //                    legidentifierobj.identifier = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.FlightNumber;
                    //                    legidentifierobj.carrierCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].LegKey.CarrierCode;
                    //                    legDetailsobj.legidentifier = legidentifierobj;

                    //                    //var ssrscount = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs.Count;

                    //                    //for (int j = 0; j < ssrscount; j++)
                    //                    //{
                    //                    childlegssrs legssrs = new childlegssrs();
                    //                    legssrs.ssrCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode.ToString();
                    //                    if (htSSr[legssrs.ssrCode] != null)
                    //                    {
                    //                        legssrs.name = htSSr[legssrs.ssrCode].ToString();
                    //                    }
                    //                    else
                    //                        continue;
                    //                    //legssrs.ssrType = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[0].AvailablePaxSSRList[j].ssrType;
                    //                    //legssrs.name = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].name;
                    //                    //legssrs.limitPerPassenger = JsonObjresponseSSRAvailabilty.data.legSsrs[i].ssrs[j].limitPerPassenger;
                    //                    legssrs.available = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].Available;
                    //                    if (_res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0)
                    //                    {
                    //                        legssrs.feeCode = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.FeeCode;
                    //                        List<legpassengers> legpassengerslist = new List<legpassengers>();
                    //                        Decimal Amount = decimal.Zero;
                    //                        legpassengers passengersdetail = new legpassengers();
                    //                        int i2 = 0;
                    //                        foreach (var items in _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges)
                    //                        {
                    //                            if (i2 > 0)
                    //                            {
                    //                                break;
                    //                            }
                    //                            else
                    //                            {
                    //                                Amount += items.Amount;
                    //                                passengersdetail.price = Math.Round(Amount).ToString(); //Ammount
                    //                            }
                    //                            i2++;
                    //                        }
                    //                        passengersdetail.passengerKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PassengerNumberList.ToString();
                    //                        passengersdetail.ssrKey = _res.SSRAvailabilityForBookingResponse.SSRSegmentList[i1].AvailablePaxSSRList[j].SSRCode;
                    //                        passengersdetail.Airline = Airlines.Indigo;
                    //                        legpassengerslist.Add(passengersdetail);
                    //                        legssrs.legpassengers = legpassengerslist;
                    //                        legssrslist.Add(legssrs);
                    //                    }
                    //                    //}
                    //                }
                    //                catch (Exception ex)
                    //                {

                    //                }

                    //                //}
                    //            }
                    //            SSRAvailabiltyLegssrobj.legDetails = legDetailsobj;
                    //            SSRAvailabiltyLegssrobj.legssrs = legssrslist;
                    //            SSRAvailabiltyLegssrlist.Add(SSRAvailabiltyLegssrobj);
                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }


                    //    SSRAvailabiltyResponceobj.legSsrs = SSRAvailabiltyLegssrlist;
                    //    HttpContext.Session.SetString("Meals", JsonConvert.SerializeObject(SSRAvailabiltyResponceobj));

                    //}

                    #endregion

                    #region SeatMap

                    //List<IndigoBookingManager_.GetSeatAvailabilityResponse> SeatGroup = null;// await objssr.GetseatAvailability(Signature, AirAsiaTripResponceobj, "OneWay");
                    //if (SeatGroup != null)
                    //{
                    //    string columncount0 = string.Empty;
                    //    //string columncount1 = string.Empty;
                    //    var data = SeatGroup.Count;// _getSeatAvailabilityResponse.SeatAvailabilityResponse.EquipmentInfos.Length;

                    //    List<data> datalist = new List<data>();
                    //    SeatMapResponceModel SeatMapResponceModel = new SeatMapResponceModel();
                    //    List<SeatMapResponceModel> SeatMapResponceModellist = new List<SeatMapResponceModel>();
                    //    for (int x = 0; x < data; x++)
                    //    {
                    //        data dataobj = new data();

                    //        SeatMapResponceModel = new SeatMapResponceModel();
                    //        SeatMapResponceModellist = new List<SeatMapResponceModel>();
                    //        Fees Fees = new Fees();
                    //        Seatmap Seatmapobj = new Seatmap();
                    //        Seatmapobj.name = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Name;
                    //        TempData["AirCraftName"] = Seatmapobj.name;
                    //        Seatmapobj.arrivalStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].ArrivalStation;
                    //        Seatmapobj.departureStation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].DepartureStation;
                    //        Seatmapobj.marketingCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].MarketingCode;
                    //        Seatmapobj.equipmentType = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentType;
                    //        Seatmapobj.equipmentTypeSuffix = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].EquipmentTypeSuffix;
                    //        //doubt
                    //        //Seatmapobj.categorySG = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].EquipmentCategory.ToString();
                    //        //doubt
                    //        //Seatmapobj.seatmapReference = JsonObjSeatmap.data[x].seatMap.seatmapReference;



                    //        int compartmentsunitCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments.Length;
                    //        List<Unit> compartmentsunitlist = new List<Unit>();
                    //        //List<Decks> Decksobjarray = new List<Decks>();
                    //        Seatmapobj.decksindigo = new List<Decks>();
                    //        Decks Decksobj = null;
                    //        for (int i = 0; i < compartmentsunitCount; i++) // 2 times 
                    //        {
                    //            compartmentsunitlist = new List<Unit>();
                    //            Decksobj = new Decks();
                    //            Decksobj.availableUnits = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].AvailableUnits;
                    //            Decksobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].CompartmentDesignator;
                    //            Decksobj.length = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Length;
                    //            Decksobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Width;
                    //            Decksobj.sequence = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Sequence;
                    //            Decksobj.orientation = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Orientation;

                    //            for (int i1 = 0; i1 < SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats.Length; i1++)
                    //            {
                    //                try
                    //                {
                    //                    Unit compartmentsunitobj = new Unit();
                    //                    //doubt
                    //                    compartmentsunitobj.Airline = Airlines.Indigo;
                    //                    compartmentsunitobj.assignable = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Assignable;
                    //                    compartmentsunitobj.availability = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatAvailability);
                    //                    compartmentsunitobj.compartmentDesignator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].CompartmentDesignator;
                    //                    compartmentsunitobj.designator = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatDesignator;
                    //                    compartmentsunitobj.type = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatGroup);
                    //                    compartmentsunitobj.travelClassCode = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].TravelClassCode;
                    //                    compartmentsunitobj.set = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatSet;
                    //                    compartmentsunitobj.group = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatGroup;
                    //                    compartmentsunitobj.priority = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Priority;
                    //                    compartmentsunitobj.text = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Text;
                    //                    //compartmentsunitobj.setVacancy = JsonObjSeatmap.data[x].seatMap.decks["1"].compartments.Y.units[i].setVacancy;
                    //                    compartmentsunitobj.angle = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].SeatAngle;
                    //                    compartmentsunitobj.width = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Width;
                    //                    compartmentsunitobj.height = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Height;
                    //                    compartmentsunitobj.zone = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Zone;
                    //                    compartmentsunitobj.x = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].X;
                    //                    compartmentsunitobj.y = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].Y;

                    //                    for (int k = 0; k < SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length; k++)
                    //                    {
                    //                        if (compartmentsunitobj.group == Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].SeatGroup))
                    //                        {
                    //                            var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges.Length;

                    //                            List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                    //                            for (int l = 0; l < feesgroupserviceChargescount; l++)
                    //                            {
                    //                                //Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                    //                                if (l > 0)
                    //                                {
                    //                                    break;
                    //                                }
                    //                                else
                    //                                {
                    //                                    compartmentsunitobj.servicechargefeeAmount += Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[k].PassengerFee.ServiceCharges[l].Amount);
                    //                                }
                    //                            }
                    //                            break;
                    //                        }
                    //                    }
                    //                    //if (compartmentsunitobj.assignable == true)
                    //                    //{
                    //                    compartmentsunitobj.unitKey = compartmentsunitobj.designator;


                    //                    //}

                    //                    int compartmentypropertiesCount = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList.Length;
                    //                    List<Properties> Propertieslist = new List<Properties>();
                    //                    for (int j = 0; j < compartmentypropertiesCount; j++)
                    //                    {
                    //                        Properties compartmentyproperties = new Properties();
                    //                        compartmentyproperties.code = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList[j].TypeCode;
                    //                        compartmentyproperties.value = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1].PropertyList[j].Value;
                    //                        Propertieslist.Add(compartmentyproperties);
                    //                    }

                    //                    compartmentsunitobj.properties = Propertieslist;
                    //                    if (compartmentsunitobj.designator.Contains('$'))
                    //                    {
                    //                        columncount0 = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[0].Compartments[i].Seats[i1 - 1].SeatDesignator;
                    //                        break;
                    //                    }

                    //                    compartmentsunitlist.Add(compartmentsunitobj);



                    //                }
                    //                catch (Exception ex)
                    //                {

                    //                }

                    //            }
                    //            Decksobj.units = compartmentsunitlist;
                    //            Seatmapobj.SeatColumnCount = Regex.Replace(columncount0, "[^0-9]", "");
                    //            //Decksobjarray.Add(Decksobj);
                    //            //Decksobj.units = compartmentsunitlist;
                    //            Seatmapobj.decksindigo.Add(Decksobj);
                    //        }


                    //        //var groupscount = JsonObjSeatmap.data[x].fees[passengerkey12].groups;
                    //        //var feesgroupcount = ((Newtonsoft.Json.Linq.JContainer)groupscount).Count;
                    //        //string strText = Regex.Match(_responseSeatmap, @"data""[\s\S]*?fees[\s\S]*?groups""(?<data>[\s\S]*?)ssrLookup",
                    //        // RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups["data"].Value;

                    //        //string seatgroup = SeatGroup[x].SeatAvailabilityResponse.EquipmentInfos[x].Compartments[i].Seats[i].SeatGroup.ToString();

                    //        List<Groups> GroupsFeelist = new List<Groups>();

                    //        int testcount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees.Length;
                    //        for (int i = 0; i < testcount; i++)
                    //        {
                    //            Groups Groupsobj = new Groups();
                    //            GroupsFee GroupsFeeobj = new GroupsFee();
                    //            string feeseatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                    //            //if (seatgroup == feeseatGroup)
                    //            //{
                    //            //doubt
                    //            GroupsFeeobj.SeatGroup = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].SeatGroup.ToString();
                    //            GroupsFeeobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeNumber;
                    //            GroupsFeeobj.ssrCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRCode;
                    //            GroupsFeeobj.ssrNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.SSRNumber;
                    //            GroupsFeeobj.paymentNumber = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.PaymentNumber;
                    //            GroupsFeeobj.isConfirmed = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                    //            GroupsFeeobj.isConfirming = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                    //            GroupsFeeobj.isConfirmingExternal = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeOverride;
                    //            GroupsFeeobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeCode;
                    //            GroupsFeeobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FeeDetail;
                    //            //Dout
                    //            // GroupsFeeobj.passengerFeeKey = JsonObjSeatmap.data[x].fees[passengerkey12].groups[myString].fees[0].passengerFeeKey;
                    //            GroupsFeeobj.flightReference = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.FlightReference;
                    //            GroupsFeeobj.note = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.Note;
                    //            GroupsFeeobj.createdDate = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.CreatedDate;
                    //            GroupsFeeobj.isProtected = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.IsProtected;

                    //            var feesgroupserviceChargescount = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges.Length;

                    //            List<Servicecharge> feesgroupserviceChargeslist = new List<Servicecharge>();
                    //            for (int l = 0; l < feesgroupserviceChargescount; l++)
                    //            {
                    //                Servicecharge feesgroupserviceChargesobj = new Servicecharge();
                    //                feesgroupserviceChargesobj.amount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].Amount);
                    //                feesgroupserviceChargesobj.code = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeCode; ;
                    //                feesgroupserviceChargesobj.detail = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeDetail;
                    //                // feesgroupserviceChargesobj.type = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ChargeType;
                    //                // feesgroupserviceChargesobj.collectType = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CollectType;
                    //                feesgroupserviceChargesobj.currencyCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].CurrencyCode;

                    //                feesgroupserviceChargesobj.foreignAmount = Convert.ToInt32(SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].ForeignAmount);
                    //                feesgroupserviceChargesobj.ticketCode = SeatGroup[x].SeatAvailabilityResponse.SeatGroupPassengerFees[i].PassengerFee.ServiceCharges[l].TicketCode;
                    //                feesgroupserviceChargeslist.Add(feesgroupserviceChargesobj);

                    //            }
                    //            GroupsFeeobj.serviceCharges = feesgroupserviceChargeslist;

                    //            Groupsobj.groupsFee = GroupsFeeobj;
                    //            GroupsFeelist.Add(Groupsobj);

                    //            Fees.groups = GroupsFeelist;
                    //            //break;

                    //            //}
                    //            //else
                    //            //{
                    //            //    continue;

                    //            //}
                    //        }
                    //        dataobj.seatMap = Seatmapobj;
                    //        dataobj.seatMapfees = Fees;
                    //        datalist.Add(dataobj);
                    //        SeatMapResponceModel.datalist = datalist;
                    //    }
                    //    string strseat = JsonConvert.SerializeObject(SeatMapResponceModel);
                    //    HttpContext.Session.SetString("Seatmap", JsonConvert.SerializeObject(SeatMapResponceModel));

                    //}
                    #endregion
                }
                return RedirectToAction("GDSSaverTripsell", "GDSTripsell");
            }
        }

        //return View();
    }
}
