using DomainLayer.Model;
using Indigo;
using IndigoBookingManager_;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Globalization;
using Utility;
using static DomainLayer.Model.ReturnTicketBooking;
using static OnionArchitectureAPI.Services.Indigo._updateContact;

namespace OnionArchitectureAPI.Services.Indigo
{
    public class _SellSSR : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public _SellSSR(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetString(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }
        Logs logs = new Logs();
        _getapi _obj = new _getapi();
        int passengerscount = 0;
        int journeyscount = 0;
        public async Task<IndigoBookingManager_.SellResponse> sellssr(string Signature, AirAsiaTripResponceModel passeengerKeyList, List<string> ssrKey, List<string> BaggageSSrkey, int _a,string _Airline="")
        {
            passengerscount = passeengerKeyList.passengerscount;
            SellResponse sellSsrResponse = null;
            using (HttpClient client = new HttpClient())
            {
                if (ssrKey.Count >= 0)
                {

                    #region SellSSr
                    SellRequest sellSsrRequest = new SellRequest();
                    SellRequestData sellreqd = new SellRequestData();
                    sellSsrRequest.Signature = Signature;
                    sellSsrRequest.ContractVersion = 456;
                    sellreqd.SellBy = SellBy.SSR;
                    sellreqd.SellBySpecified = true;
                    sellreqd.SellSSR = new SellSSR();
                    sellreqd.SellSSR.SSRRequest = new SSRRequest();

                    journeyscount = passeengerKeyList.journeys.Count;
                    for (int i = 0; i < journeyscount; i++)
                    {
                        int segmentscount = passeengerKeyList.journeys[i].segments.Count;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[segmentscount];
                        for (int j = 0; j < segmentscount; j++)
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j] = new SegmentSSRRequest();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STD = passeengerKeyList.journeys[i].segments[j].designator.departure;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new FlightDesignator();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = passeengerKeyList.journeys[i].segments[j].identifier.carrierCode;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = passeengerKeyList.journeys[i].segments[j].identifier.identifier;
                            string numinfant = GetString("PaxArray");
                            Paxes PaxNum = null;
                            if (!string.IsNullOrEmpty(numinfant))
                            {
                                PaxNum = (Paxes)JsonConvert.DeserializeObject(numinfant, typeof(Paxes));
                            }
                            PaxNum.Infant_ = new List<passkeytype>();
                            bool infant = false;
                            ssrsegmentwise _obj = new ssrsegmentwise();
                            _obj.SSRcodeOneWayI = new List<ssrsKey>();
                            _obj.SSRcodeOneWayII = new List<ssrsKey>();
                            _obj.SSRcodeRTI = new List<ssrsKey>();
                            _obj.SSRcodeRTII = new List<ssrsKey>();
                            _obj.SSRbaggagecode0 = new List<ssrsKey>();
                            _obj.SSRbaggagecode1 = new List<ssrsKey>();
                            for (int k = 0; k < ssrKey.Count; k++)
                            {
                                if (ssrKey[k].ToLower().Contains("airasia"))
                                    continue;
                                if (ssrKey[k].Contains("_OneWay0"))
                                {
                                    //split
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        ssrsKey _obj0 = new ssrsKey();
                                        _obj0.key = ssrKey[k];
                                        _obj.SSRcodeOneWayI.Add(_obj0);
                                    }
                                }
                                else if (ssrKey[k].Contains("_OneWay1"))
                                {
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        ssrsKey _obj1 = new ssrsKey();
                                        _obj1.key = ssrKey[k];
                                        _obj.SSRcodeOneWayII.Add(_obj1);
                                    }
                                }
                                else if (ssrKey[k].Contains("_RT0"))
                                {
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        ssrsKey _obj2 = new ssrsKey();
                                        _obj2.key = ssrKey[k];
                                        _obj.SSRcodeRTI.Add(_obj2);
                                    }
                                }
                                else if (ssrKey[k].Contains("_RT1"))
                                {
                                    string[] wordsArray = ssrKey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        ssrsKey _obj3 = new ssrsKey();
                                        _obj3.key = ssrKey[k];
                                        _obj.SSRcodeRTII.Add(_obj3);
                                    }
                                }

                            }
                            for (int k = 0; k < BaggageSSrkey.Count; k++)
                            {
                                string[] sskeydata = new string[2];
                                if (BaggageSSrkey[k].Contains("_0"))
                                {
                                    string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        sskeydata = BaggageSSrkey[k].Split("_");
                                        ssrsKey _objBag0 = new ssrsKey();
                                        _objBag0.key = sskeydata[0];
                                        _obj.SSRbaggagecode0.Add(_objBag0);
                                    }
                                }
                                else if (BaggageSSrkey[k].Contains("_1"))
                                {
                                    string[] wordsArray = BaggageSSrkey[k].ToString().Split('_');
                                    if (wordsArray.Length > 1 && !string.IsNullOrEmpty(wordsArray[0]))
                                    {
                                        sskeydata = BaggageSSrkey[k].Split("_");
                                        ssrsKey _objBag1 = new ssrsKey();
                                        _objBag1.key = sskeydata[0];
                                        _obj.SSRbaggagecode1.Add(_objBag1);
                                    }
                                }

                            }
                            if (j == 0 && _a == 0)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeOneWayI.Count];
                            }
                            else if (j == 1 && _a == 0)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeOneWayII.Count];
                            }
                            else if (j == 0 && _a == 1)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeRTI.Count];
                            }
                            else if (j == 1 && _a == 1)
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new PaxSSR[PaxNum.Infant_.Count + _obj.SSRcodeRTII.Count];
                            }


                            int TotalPaxcount = PaxNum.Adults_.Count + PaxNum.Childs_.Count;

                            if (j == 0 && _a == 0)
                            {
                                //int k = 0;

                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeOneWayI.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i2 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcodeOneWayI.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i2 = 0; i2 < _obj.SSRcodeOneWayI.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcodeOneWayI[i2].key.ToString().Split('_');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcodeOneWayI[i2].key.ToString();
                                                    idx = j1 + i2;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;

                                                }

                                            }
                                            j1 = idx;
                                        }
                                    }

                                }


                            }
                            else if (j == 1 && _a == 0)
                            {
                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeOneWayII.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i2 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcodeOneWayII.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i2 = 0; i2 < _obj.SSRcodeOneWayII.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcodeOneWayII[i2].key.ToString().Split('_');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcodeOneWayII[i2].key.ToString();
                                                    idx = j1 + i2;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;

                                                }

                                            }
                                            j1 = idx;
                                        }
                                    }

                                }
                            }
                            else if (j == 0 && _a == 1)
                            {
                                //int k = 0;

                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeRTI.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i2 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcodeRTI.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i2 = 0; i2 < _obj.SSRcodeRTI.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcodeRTI[i2].key.ToString().Split('_');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcodeRTI[i2].key.ToString();
                                                    idx = j1 + i2;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;

                                                }

                                            }
                                            j1 = idx;
                                        }
                                    }

                                }


                            }
                            else if (j == 1 && _a == 1)
                            {
                                if (TotalPaxcount > 0)
                                {
                                    for (int j1 = 0; j1 < PaxNum.Infant_.Count + _obj.SSRcodeRTII.Count; j1++)
                                    {

                                        if (j1 < PaxNum.Infant_.Count)
                                        {
                                            for (int i2 = 0; i2 < PaxNum.Adults_.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                            {
                                                int infantcount = PaxNum.Infant_.Count;
                                                if (infantcount > 0 && i2 + 1 <= infantcount)
                                                {
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRCode = "INFT";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i2].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    j1 = PaxNum.Infant_.Count - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int idx = 0;
                                            if (_obj.SSRcodeRTII.Count > 0)//&& i1 + 1 <= ssrKey.Count
                                            {
                                                for (int i2 = 0; i2 < _obj.SSRcodeRTII.Count; i2++)//Paxnum 1 adult,1 child,1 infant 2 meal
                                                {
                                                    string[] wordsArray = _obj.SSRcodeRTII[i2].key.ToString().Split('_');
                                                    //alert(wordsArray);
                                                    //var meal = null;
                                                    string ssrCodeKey = "";
                                                    if (wordsArray.Length > 1)
                                                    {
                                                        ssrCodeKey = wordsArray[0];
                                                        ssrCodeKey = ssrCodeKey.Replace(@"""", "");
                                                    }
                                                    else
                                                        ssrCodeKey = _obj.SSRcodeRTII[i2].key.ToString();
                                                    idx = j1 + i2;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx] = new PaxSSR();
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ActionStatusCode = "NN";
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRCode = ssrCodeKey;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumberSpecified = true;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].PassengerNumber = Convert.ToInt16(i2);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].SSRNumber = Convert.ToInt16(0);
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].DepartureStation = passeengerKeyList.journeys[i].segments[j].designator.origin;
                                                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[idx].ArrivalStation = passeengerKeyList.journeys[i].segments[j].designator.destination;
                                                    //j1 = j1 + i1;

                                                }

                                            }
                                            j1 = idx;
                                        }
                                    }

                                }
                            }

                            //to do 
                            //sellSsrRequest.SellRequestData = sellreqd;
                            //SellResponse sellSsrResponse = null;
                            //sellreqd.SellSSR.SSRRequest.SellSSRMode = SellSSRMode.NonBundle;
                            //sellreqd.SellSSR.SSRRequest.SellSSRModeSpecified = true;
                            //SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                            //sellSsrResponse = await objSpiceJet.sellssR(sellSsrRequest);

                            //string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                        }
                    }

                    sellSsrRequest.SellRequestData = sellreqd;
                    sellreqd.SellSSR.SSRRequest.SellSSRMode = SellSSRMode.NonBundle;
                    sellreqd.SellSSR.SSRRequest.SellSSRModeSpecified = true;

                    _getapi _objIndigo = new _getapi();
                    sellSsrResponse = await _objIndigo._sellssR(sellSsrRequest);

                    string Str3 = JsonConvert.SerializeObject(sellSsrResponse);
                    if (_Airline.ToLower() == "oneway")
                    {
                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR", "IndigoOneWay");
                    }
                    else
                    {
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(sellSsrRequest) + "\n\n Response: " + JsonConvert.SerializeObject(sellSsrResponse), "SellSSR", "IndigoRT");
                    }
                    if (sellSsrResponse != null)
                    {
                        var JsonObjSeatAssignment = sellSsrResponse;
                    }
                    #endregion
                }
            }
            return (SellResponse)sellSsrResponse;

        }


        public async Task<IndigoBookingManager_.AssignSeatsResponse> AssignSeat(string Signature, AirAsiaTripResponceModel passeengerKeyList, List<string> unitKey, int p,int keycount0,int keycount1,string _Airlineway="")
        {
            journeyscount = passeengerKeyList.journeys.Count;
            AssignSeatsResponse _AssignseatRes = new AssignSeatsResponse();
            AssignSeatsRequest _AssignSeatReq = new AssignSeatsRequest();
            if (!string.IsNullOrEmpty(Signature))
            {
                string[] unitKey2 = null;
                string[] unitsubKey2 = null;
                string pas_unitKey = string.Empty;
                int seatid = 0;
                int _index = 0;

                _AssignSeatReq.Signature = Signature;
                _AssignSeatReq.ContractVersion = 456;
                _AssignSeatReq.SellSeatRequest = new SeatSellRequest();
                _AssignSeatReq.SellSeatRequest.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                _AssignSeatReq.SellSeatRequest.SeatAssignmentModeSpecified = true;
                int keycount = 0;
                if (p == 0)
                    keycount = keycount0;
                else
                    keycount = keycount1;
                _AssignSeatReq.SellSeatRequest.SegmentSeatRequests = new SegmentSeatRequest[keycount];// [unitKey.Count];//to do

                for (int i2 = 0; i2 < journeyscount; i2++)
                {
                    int l = 0;
                    int m = 0;
                    for (int k = 0; k < unitKey.Count; k++)
                    {
                        int idx = 0;
                        int paxnum = 0;
                        if (seatid < unitKey.Count)
                        {
                            if (unitKey[seatid].Length > 1)
                            {
                                if ((unitKey[seatid].ToString().Contains("OneWay0") || unitKey[seatid].ToString().Contains("_0") || unitKey[seatid].ToString().Contains("OneWay1") || unitKey[seatid].ToString().Contains("_1")) && p == 0)
                                {
                                    unitsubKey2 = unitKey[seatid].Split('_');
                                    pas_unitKey = unitsubKey2[1];
                                    if (unitsubKey2.Length > 3)
                                    {
                                        idx = int.Parse(unitsubKey2[3]);
                                    }
                                    else
                                    {
                                        idx = int.Parse(unitsubKey2[2]);
                                    }
                                    if (idx == 0)
                                    {
                                        paxnum = l++;
                                    }
                                    else
                                    {
                                        paxnum = m++;
                                    }
                                    //keycount++;

                                }
                                else if ((unitKey[seatid].ToString().Contains("RT0") || unitKey[seatid].ToString().Contains("RT1")) && p == 1)
                                {
                                    unitsubKey2 = unitKey[seatid].Split('_');
                                    pas_unitKey = unitsubKey2[1];
                                    if (unitsubKey2.Length > 3)
                                    {
                                        idx = int.Parse(unitsubKey2[3]);
                                    }
                                    else
                                    {
                                        idx = int.Parse(unitsubKey2[2]);
                                    }
                                    if (idx == 0)
                                    {
                                        paxnum = l++;
                                    }
                                    else
                                    {
                                        paxnum = m++;
                                    }
                                    //keycount++;

                                }
                                else
                                {
                                    seatid++;
                                    continue;
                                }
                            }

                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index] = new SegmentSeatRequest();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator = new FlightDesignator();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator.CarrierCode = passeengerKeyList.journeys[i2].segments[idx].identifier.carrierCode;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].FlightDesignator.FlightNumber = passeengerKeyList.journeys[i2].segments[idx].identifier.identifier;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].STD = passeengerKeyList.journeys[i2].segments[idx].designator.departure;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].STDSpecified = true;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].DepartureStation = passeengerKeyList.journeys[i2].segments[idx].designator.origin;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].ArrivalStation = passeengerKeyList.journeys[i2].segments[idx].designator.destination;
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].UnitDesignator = pas_unitKey.Trim();
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].PassengerNumbers = new short[1];
                            _AssignSeatReq.SellSeatRequest.SegmentSeatRequests[_index].PassengerNumbers[0] = Convert.ToInt16(paxnum);
                            seatid++;
                            _index++;
                        }
                    }

                }

                _AssignSeatReq.SellSeatRequest.IncludeSeatData = true;
                _AssignSeatReq.SellSeatRequest.IncludeSeatDataSpecified = true;

                _getapi _obj = new _getapi();
                _AssignseatRes = await _obj._Assignseat(_AssignSeatReq);

                string Str2 = JsonConvert.SerializeObject(_AssignseatRes);
                if (_Airlineway.ToLower() == "oneway")
                {
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat", "IndigoOneWay");
                }
                else
                {
                    logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_AssignSeatReq) + "\n\n Response: " + JsonConvert.SerializeObject(_AssignseatRes), "AssignSeat", "IndigoRT");
                }
            }
            return (AssignSeatsResponse)_AssignseatRes;

        }


        public class ssrsegmentwise
        {
            public List<ssrsKey> SSRcode0 { get; set; }
            public List<ssrsKey> SSRcode1 { get; set; }
            public List<ssrsKey> SSRbaggagecode0 { get; set; }
            public List<ssrsKey> SSRbaggagecode1 { get; set; }
            public List<ssrsKey> SSRcodeOneWayI { get; set; }
            public List<ssrsKey> SSRcodeOneWayII { get; set; }
            public List<ssrsKey> SSRcodeRTI { get; set; }
            public List<ssrsKey> SSRcodeRTII { get; set; }
        }

        public class ssrsKey
        {
            public string key { get; set; }
        }
    }
}
