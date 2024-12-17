using DomainLayer.Model;
using Spicejet;
using SpicejetSessionManager_;
using SpicejetBookingManager_;
using Newtonsoft.Json;
using Utility;
using Microsoft.AspNetCore.Mvc;

namespace OnionArchitectureAPI.Services.Spicejet
{
    public class _GetAvailability : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _GetAvailability(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSessionValue(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
        Logs logs = new Logs();
        public async Task<GetAvailabilityVer2Response> GetTripAvailability(SimpleAvailabilityRequestModel _GetfligthModel, LogonResponse _SpicejetlogonResponseobj, int TotalCount, int adultcount, int childcount, int infantcount, string flightclass, string JourneyType, string _AirlineWay = "")
        {
            #region Availability
            
            
            GetAvailabilityRequest _getAvailabilityRQ = new GetAvailabilityRequest();
            _getAvailabilityRQ.Signature = _SpicejetlogonResponseobj.Signature;
            _getAvailabilityRQ.ContractVersion = 420;// _SpicejetlogonResponseobj.ContractVersion;
            _getAvailabilityRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();

            if (_AirlineWay.ToLower() == "spicejetoneway")
            {
                //TempData["origin"] = _GetfligthModel.origin;
                //TempData["destination"] = _GetfligthModel.destination;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.origin;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.destination;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            }
            else
            {
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.destination;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.origin;
                //TempData["originR"] = _GetfligthModel.origin;
                //TempData["destinationR"] = _GetfligthModel.destination;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);

                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
                _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.endDate);
            }
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = default;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "SG";
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatusSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatus = BookingStatus.Default;
            // Different Product Class
            //string[] faretypes = { "R", "MX", "IO", "SF" };
            string[] faretypes = { "R", "MX", "SF" };
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypes;

            string[] productclasses = new string[1];
            //string[] productclasses = {"R"};
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclasses;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = FareRuleFilter.Default;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControlSpecified = true;
            _getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControl = ServiceBundleControl.Disabled;

            _getapi objSpicejet = new _getapi();
            GetAvailabilityVer2Response _getAvailabilityVer2Response = await objSpicejet.GetTripAvailability(_getAvailabilityRQ);
            if (_AirlineWay.ToLower() == "spicejetoneway")
            {
                SetSessionValue("SpicejetAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityRQ));
                SetSessionValue("SpicejetSignature", JsonConvert.SerializeObject(_getAvailabilityRQ.Signature));

                logs.WriteLogs(JsonConvert.SerializeObject(_getAvailabilityRQ), "2-GetAvailabilityReq", "SpicejetOneWay", JourneyType);
                logs.WriteLogs(JsonConvert.SerializeObject(_getAvailabilityVer2Response), "2-GetAvailabilityRes", "SpicejetOneWay", JourneyType);
            }
            else
            {
                SetSessionValue("SpicejetAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityRQ));
                SetSessionValue("SpicejetReturnSignature", JsonConvert.SerializeObject(_getAvailabilityRQ.Signature));

                logs.WriteLogsR(JsonConvert.SerializeObject(_getAvailabilityRQ), "2-GetAvailabilityReq", "SpicejetRT");
                logs.WriteLogsR(JsonConvert.SerializeObject(_getAvailabilityVer2Response), "2-GetAvailabilityRes", "SpicejetRT");
                //logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getAvailabilityRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2Response), "GetAvailability", "SpicejetRT");
            }
            return (GetAvailabilityVer2Response)_getAvailabilityVer2Response;
            #endregion

        }

        //Corporate Get Availibilty
        //public async Task<GetAvailabilityVer2Response> GetCorporateTripAvailability(SimpleAvailabilityRequestModel _GetfligthModel, LogonResponse _IndigologonResponseobj, int TotalCount, int adultcount, int childcount, int infantcount, string flightclass, string _AirlineWay = "")
        //{
        //    #region Logon

        //    GetAvailabilityVer2Response _getAvailabilityReturnRS = null;
        //    GetAvailabilityRequest _getAvailabilityReturnRQ = null;
        //    _getAvailabilityReturnRQ = new GetAvailabilityRequest();
        //    _getAvailabilityReturnRQ.Signature = _IndigologonResponseobj.Signature;
        //    _getAvailabilityReturnRQ.ContractVersion = 452;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.origin; //return_origin
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.destination; //return_depart
        //    if (_AirlineWay.ToLower() == "indigooneway")
        //    {
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.beginDate);

        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
        //    }
        //    else
        //    {
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
        //    }
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = AvailabilityFilter.ExcludeUnavailable;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.


        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "6E";

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatusSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatus = BookingStatus.Default;

        //    // Different Product Class
        //    string[] faretypesreturn = { "R", "Z", "F" };
        //    //string[] faretypesreturn = { "R" };
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypesreturn;
        //    if (flightclass == "F") // Corporate Class
        //    {
        //        string[] productclassesreturn = { "F", "M", "C" };
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
        //    }
        //    else if (flightclass == "B") // Business Class
        //    {
        //        string[] productclassesreturn = { "BR", "BC" };
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
        //    }
        //    else //Corporate Fare and Retail Fare 
        //    {
        //        string[] productclassesreturn = { "R", "J", "A", "O", "S", "N", "B", "T", "F", "M", "C" };
        //        _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
        //    }

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = FareRuleFilter.Default;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControlSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControl = ServiceBundleControl.Disabled;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilterSpecified = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = true;
        //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = true;
        //    SetSessionValue("IndigoAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));
        //    SetSessionValue("IndigoPassengerModel", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));

        //    _getapi objIndigo = new _getapi();
        //    GetAvailabilityVer2Response _getAvailabilityVer2ReturnResponse = await objIndigo.GetTripAvailability(_getAvailabilityReturnRQ);
        //    if (_AirlineWay.ToLower() == "indigooneway")
        //    {
        //        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoOneWay");
        //    }
        //    else
        //    {
        //        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoRT");
        //    }
        //    return (GetAvailabilityVer2Response)_getAvailabilityVer2ReturnResponse;
        //    #endregion

        //}


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
