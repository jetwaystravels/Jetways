using IndigoBookingManager_;
using DomainLayer.Model;
using Indigo;
using IndigoSessionmanager_;
using Newtonsoft.Json;
using Utility;
using Microsoft.AspNetCore.Mvc;

namespace OnionArchitectureAPI.Services.Indigo
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
        public async Task<GetAvailabilityVer2Response> GetTripAvailability(SimpleAvailabilityRequestModel _GetfligthModel, LogonResponse _IndigologonResponseobj, int TotalCount, int adultcount, int childcount, int infantcount, string flightclass, string _AirlineWay = "")
        {
            #region Logon

            GetAvailabilityVer2Response _getAvailabilityReturnRS = null;
            GetAvailabilityRequest _getAvailabilityReturnRQ = null;
            _getAvailabilityReturnRQ = new GetAvailabilityRequest();
            _getAvailabilityReturnRQ.Signature = _IndigologonResponseobj.Signature;
            _getAvailabilityReturnRQ.ContractVersion = 452;
            //_GetfligthModel.origin = "BOM";
            //_GetfligthModel.destination = "IXJ";
            _getAvailabilityReturnRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();

            //if (_GetfligthModel.endDate != null)
            //{
            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.destination; //return_origin

            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.origin; //return_depart

            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
            //    //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime("2024-01-18");
            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);

            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
            //    //_getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime("2024-01-18");
            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            //}
            //else
            //{
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.origin; //return_origin

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.destination; //return_depart
            if (_AirlineWay.ToLower() == "indigooneway")
            {
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime("2024-01-18");
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            }
            else
            {
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                //_getAvailabilityRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime("2024-01-18");
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            }
            //_getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime();
            //}
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = AvailabilityFilter.ExcludeUnavailable;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.


            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "6E";

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatusSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatus = BookingStatus.Default;
            //string[] faretypesreturn = { "R" };
            //_getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypesreturn;
            //if (flightclass == "B") // Business Class
            //{
            //    string[] productclassesreturn = { "BR","BC" };
            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            //}
            //else
            //{
            //    string[] productclassesreturn = { "R", "J", "A", "O" };
            //    _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            //}


            // Different Product Class
            string[] faretypesreturn = { "R", "Z", "F" };
            //string[] faretypesreturn = { "R" };
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypesreturn;
            if (flightclass == "F") // Corporate Class
            {
                string[] productclassesreturn = { "F", "M", "C" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }
            else if (flightclass == "B") // Business Class
            {
                string[] productclassesreturn = { "BR", "BC" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }
            else //Retails
            {
                string[] productclassesreturn = { "R", "J", "A", "O", "S", "N", "B", "T" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = FareRuleFilter.Default;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControlSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControl = ServiceBundleControl.Disabled;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = true;


            //SetSessionValue("IndigoReturnSignature", JsonConvert.SerializeObject(_IndigologonResponseobj.Signature));
            SetSessionValue("IndigoAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));
            SetSessionValue("IndigoPassengerModel", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));
            //httpContext.Session.SetString("IndigoReturnSignature", JsonConvert.SerializeObject(_IndigologonResponseobj.Signature));
            //HttpContext.Session.SetString("IndigoAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));
            //HttpContext.Session.SetString("IndigoPassengerModel", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));

            _getapi objIndigo = new _getapi();
            GetAvailabilityVer2Response _getAvailabilityVer2ReturnResponse = await objIndigo.GetTripAvailability(_getAvailabilityReturnRQ);
            if (_AirlineWay.ToLower() == "indigooneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoRT");
            }
            return (GetAvailabilityVer2Response)_getAvailabilityVer2ReturnResponse;
            #endregion

        }
        
        //Corporate Get Availibilty
        public async Task<GetAvailabilityVer2Response> GetCorporateTripAvailability(SimpleAvailabilityRequestModel _GetfligthModel, LogonResponse _IndigologonResponseobj, int TotalCount, int adultcount, int childcount, int infantcount, string flightclass, string _AirlineWay = "")
        {
            #region Logon

            GetAvailabilityVer2Response _getAvailabilityReturnRS = null;
            GetAvailabilityRequest _getAvailabilityReturnRQ = null;
            _getAvailabilityReturnRQ = new GetAvailabilityRequest();
            _getAvailabilityReturnRQ.Signature = _IndigologonResponseobj.Signature;
            _getAvailabilityReturnRQ.ContractVersion = 452;
            _getAvailabilityReturnRQ.TripAvailabilityRequest = new TripAvailabilityRequest();
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0] = new AvailabilityRequest();
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = _GetfligthModel.origin; //return_origin
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = _GetfligthModel.destination; //return_depart
            if (_AirlineWay.ToLower() == "indigooneway")
            {
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.beginDate);

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            }
            else
            {
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = Convert.ToDateTime(_GetfligthModel.endDate);
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = Convert.ToDateTime(_GetfligthModel.beginDate);
            }
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = FlightType.All;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(TotalCount); //Total Travell Count

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].Dow = DOW.Daily;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = AvailabilityFilter.ExcludeUnavailable;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;


            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = new PaxPriceType[0];
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = getPaxdetails(adultcount, childcount, infantcount); //Pax Count 1 always Default Set.


            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "6E";

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = FareClassControl.CompressByProductClass;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatusSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].BookingStatus = BookingStatus.Default;

            // Different Product Class
            string[] faretypesreturn = { "R", "Z", "F" };
            //string[] faretypesreturn = { "R" };
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = faretypesreturn;
            if (flightclass == "F") // Corporate Class
            {
                string[] productclassesreturn = { "F", "M", "C" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }
            else if (flightclass == "B") // Business Class
            {
                string[] productclassesreturn = { "BR", "BC" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }
            else //Corporate Fare and Retail Fare 
            {
                string[] productclassesreturn = { "R", "J", "A", "O", "S", "N", "B", "T", "F", "M", "C" };
                _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ProductClasses = productclassesreturn;
            }

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 20;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = FareRuleFilter.Default;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControlSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].ServiceBundleControl = ServiceBundleControl.Disabled;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilterSpecified = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.LoyaltyFilter = LoyaltyFilter.MonetaryOnly;

            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = true;
            _getAvailabilityReturnRQ.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = true;
            SetSessionValue("IndigoAvailibilityRequest", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));
            SetSessionValue("IndigoPassengerModel", JsonConvert.SerializeObject(_getAvailabilityReturnRQ));

            _getapi objIndigo = new _getapi();
            GetAvailabilityVer2Response _getAvailabilityVer2ReturnResponse = await objIndigo.GetTripAvailability(_getAvailabilityReturnRQ);
            if (_AirlineWay.ToLower() == "indigooneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getAvailabilityReturnRQ) + "\n\n Response: " + JsonConvert.SerializeObject(_getAvailabilityVer2ReturnResponse), "GetAvailability", "IndigoRT");
            }
            return (GetAvailabilityVer2Response)_getAvailabilityVer2ReturnResponse;
            #endregion

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
