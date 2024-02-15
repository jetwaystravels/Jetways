using DomainLayer.Model;
using Indigo;
using IndigoBookingManager_;
using IndigoSessionmanager_;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Utility;

namespace OnionArchitectureAPI.Services.Indigo
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
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest", "IndigoOneWay");
            }
            else
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getSellRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getSellRS), "SellRequest", "IndigoRT");


            return (SellResponse)_getSellRS;
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
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getPriceItineraryRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getPriceItineraryRS), "PriceIteniry", "IndigoOneWay");
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
    }
}
