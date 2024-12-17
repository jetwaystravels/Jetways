using DomainLayer.Model;
using Indigo;
using IndigoBookingManager_;
using IndigoSessionmanager_;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Utility;

namespace OnionArchitectureAPI.Services.Spicejet
{
    public class _sell
    {
        Logs logs = new Logs();
        _getapi _obj = new _getapi();
        public async Task<SellResponse> Sell(string Signature, string _JourneykeyData, string _FareKeyData, string _Jparts, string fareKey, int TotalCount, int adultcount, int childcount, int infantcount, string _AirlineWay = "")
        {
            SellResponse _getSellRS = null;
            SellRequest _getSellRQ = null;
            _getSellRQ = new SellRequest();
            _getSellRQ.SellRequestData = new SellRequestData(); ;
            _getSellRQ.Signature = Signature;
            _getSellRQ.ContractVersion = 456;
            _getSellRQ.SellRequestData.SellBy = SellBy.JourneyBySellKey;
            if (!string.IsNullOrEmpty(_Jparts))
            {
                _JourneykeyData = _Jparts;
            }

            if (!string.IsNullOrEmpty(fareKey))
            {
                string fareKeyRTway = fareKey;
                string[] FRTparts = fareKeyRTway.Split('@');
                _FareKeyData = FRTparts[0];
            }
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest = new SellJourneyByKeyRequest();
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new SellJourneyByKeyRequestData();
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.CurrencyCode = "INR";
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new SellKeyList[1];
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new SellKeyList();
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = _JourneykeyData;
            _getSellRQ.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = _FareKeyData;
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
            _getSellRS = await _obj.sell(_getSellRQ);

            string str = JsonConvert.SerializeObject(_getSellRS);
            if (_AirlineWay.ToLower() == "oneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest", "IndigoOneWay");
            }
            else
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest", "IndigoRT");


            return (SellResponse)_getSellRS;
        }

        public async Task<GetBookingFromStateResponse> GetBookingFromState(string Signature, string _AirlineWay = "")
        {
            GetBookingFromStateResponse _GetBookingFromStateRS1 = null;
            GetBookingFromStateRequest _GetBookingFromStateRQ1 = null;
            _GetBookingFromStateRQ1 = new GetBookingFromStateRequest();
            _GetBookingFromStateRQ1.Signature = Signature;
            _GetBookingFromStateRQ1.ContractVersion = 420;

            IBookingManager bookingManager = null;
            GetBookingFromStateResponse _getBookingFromStateResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getBookingFromStateResponse = await bookingManager.GetBookingFromStateAsync(_GetBookingFromStateRQ1);
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            if (_AirlineWay.ToLower() == "oneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_GetBookingFromStateRQ1) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingFromStateResponse), "GetBookingFromStateAftersellInfantrequest", "IndigoOneWay");

            }
            else
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_GetBookingFromStateRQ1) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingFromStateResponse), "GetBookingFromStateAftersellInfantrequest", "IndigoRT");

            return _getBookingFromStateResponse;
        }

        public async Task<PriceItineraryResponse> GetItineraryPrice(string Signature, string _JourneykeyData, string _FareKeyData, string _Jparts, string fareKey, int TotalCount, int adultcount, int childcount, int infantcount, string _AirlineWay = "")
        {
            PriceItineraryResponse _getPriceItineraryRS = null;
            PriceItineraryRequest _getPriceItineraryRQ = null;
            _getPriceItineraryRQ = new PriceItineraryRequest();
            _getPriceItineraryRQ.ItineraryPriceRequest = new ItineraryPriceRequest();
            _getPriceItineraryRQ.Signature = Signature;
            _getPriceItineraryRQ.ContractVersion = 456;
            _getPriceItineraryRQ.ItineraryPriceRequest.PriceItineraryBy = PriceItineraryBy.JourneyBySellKey;

            _getPriceItineraryRQ.ItineraryPriceRequest.BookingStatus = default;
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest = new SellJourneyByKeyRequestData();
            SellKeyList _getSellKeyList = new SellKeyList();
            _getSellKeyList.JourneySellKey = _JourneykeyData;
            _getSellKeyList.FareSellKey = _FareKeyData;
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys = new SellKeyList[1];
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0] = new SellKeyList();
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].JourneySellKey = _getSellKeyList.JourneySellKey;
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.JourneySellKeys[0].FareSellKey = _getSellKeyList.FareSellKey;
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxCount = Convert.ToInt16(TotalCount);
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.CurrencyCode = "INR";
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.PaxPriceType = getPaxdetails(adultcount, childcount, 0);
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.SourcePOS = GetPointOfSale();
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
            _getPriceItineraryRQ.ItineraryPriceRequest.SellByKeyRequest.IsAllotmentMarketFare = false;
            _getPriceItineraryRQ.ItineraryPriceRequest.SSRRequest = new SSRRequest();

            _getPriceItineraryRS = await _obj.GetItineraryPrice(_getPriceItineraryRQ);
            string str = JsonConvert.SerializeObject(_getPriceItineraryRS);
            if (_AirlineWay.ToLower() == "oneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getPriceItineraryRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getPriceItineraryRS), "PriceIteniry", "IndigoOneWay");
            }
            else
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getPriceItineraryRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getPriceItineraryRS), "PriceIteniry", "IndigoRT");


            return (PriceItineraryResponse)_getPriceItineraryRS;
        }

        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = MessageState.New;
                SourcePOS.OrganizationCode = "";
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

        PaxPriceType[] getPaxdetails(int adult_, int child_, int infant_)
        {
            PaxPriceType[] paxPriceTypes = null;
            try
            {
                //int tcount = adult_ + child_ + infant_;
                int i = 0;
                if (adult_ > 0) i++;
                if (child_ > 0) i++;
                if (infant_ > 0) i++;

                paxPriceTypes = new PaxPriceType[i];
                int j = 0;
                if (adult_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "ADT";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(adult_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }

                if (child_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "CHD";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(child_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }

                if (infant_ > 0)
                {
                    paxPriceTypes[j] = new PaxPriceType();
                    paxPriceTypes[j].PaxType = "INFT";
                    paxPriceTypes[j].PaxCountSpecified = true;
                    paxPriceTypes[j].PaxCount = Convert.ToInt16(infant_);
                    //paxPriceTypes[j].PaxCount = Convert.ToInt16(0);
                    j++;
                }
            }
            catch (Exception e)
            {
            }

            return paxPriceTypes;
        }

        public async Task<SellResponse> sellssrInft(string Signature, PriceItineraryResponse _getPriceItineraryRS, int infantcount, int _a, string _Airline = "")
        {
            var passanger = _getPriceItineraryRS.Booking.Passengers;
            string passenger = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                #region SellSSrInfant
                SellResponse sellSsrResponse = null;
                SellRequest sellSsrRequest = new SellRequest();
                SellRequestData sellreqd = new SellRequestData();
                sellSsrRequest.Signature = Signature;
                sellSsrRequest.ContractVersion = 456;
                sellreqd.SellBy = SellBy.SSR;
                sellreqd.SellBySpecified = true;
                sellreqd.SellSSR = new SellSSR();
                sellreqd.SellSSR.SSRRequest = new SSRRequest();
                int journeyscount = _getPriceItineraryRS.Booking.Journeys.Length;
                for (int i = 0; i < journeyscount; i++)
                {
                    int segmentscount = _getPriceItineraryRS.Booking.Journeys[i].Segments.Length;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[segmentscount];
                    for (int j = 0; j < segmentscount; j++)
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j] = new SegmentSSRRequest();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].DepartureStation;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].ArrivalStation;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STD = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].STD;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new FlightDesignator();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode; ;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                        //GetPassenger(passengerdetails);
                        int numinfant = 0;
                        if (infantcount > 0)
                        {
                            numinfant = infantcount;
                        }
                        //infantcount = Convert.ToInt32(HttpContext.Session.GetString("infantCount"));
                        //Paxes PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));
                        bool infant = false;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[numinfant];

                        for (int j1 = 0; j1 < numinfant; j1++)
                        {

                            if (j1 < numinfant)
                            {
                                for (int i1 = 0; i1 < numinfant; i1++)
                                {
                                    infantcount = numinfant;
                                    if (infantcount > 0 && i1 + 1 <= infantcount)
                                    {
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1] = new PaxSSR();
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ActionStatusCode = "NN";
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRCode = "INFT";
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumberSpecified = true;
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].PassengerNumber = Convert.ToInt16(i1);
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumberSpecified = true;
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].SSRNumber = Convert.ToInt16(0);
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].DepartureStation = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].DepartureStation;
                                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i1].ArrivalStation = _getPriceItineraryRS.Booking.Journeys[i].Segments[j].ArrivalStation;
                                        j1 = numinfant - 1;
                                    }
                                }
                            }
                        }
                        sellSsrRequest.SellRequestData = sellreqd;
                    }
                    _getapi _objIndigo = new _getapi();
                    sellSsrResponse = await _objIndigo._sellssR(sellSsrRequest);

                    
                    #endregion
                    //}
                }
                string str3 = JsonConvert.SerializeObject(sellSsrResponse);
                if (_Airline.ToLower() == "oneway")
                {
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSRInft", "IndigoOneWay");
                }
                else
                {
                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSRInft", "IndigoRT");
                }
                if (sellSsrResponse != null)
                {
                    var JsonObjSeatAssignment = sellSsrResponse;
                }
                return (SellResponse)sellSsrResponse;
            }

        }
    }
}
