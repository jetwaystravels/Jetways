using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiceLayer.Service.Interface;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using Utility;
using ZXing.QrCode.Internal;
using static DomainLayer.Model.GDSResModel;

namespace OnionArchitectureAPI.Services.Travelport
{
    public class TravelPort : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TravelPort(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSessionValue(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
        Logs logs = new Logs();
        //string _targetBranch = string.Empty;
        //string _userName = string.Empty;
        //string _password = string.Empty;
        //public TravelPort(string tragetBranch_, string userName_, string password_)
        //{
        //    _targetBranch = tragetBranch_;
        //    _userName = userName_;
        //    _password = password_;
        //}

        public string GetAvailabilty(string _testURL, StringBuilder sbReq, TravelPort _objAvail, SimpleAvailabilityRequestModel _GetfligthModel, string newGuid, string _targetBranch, string _userName, string _password, string _AirlineWay)
        {

            sbReq = new StringBuilder();
            sbReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sbReq.Append("<soap:Body>");
            sbReq.Append("<LowFareSearchReq xmlns=\"http://www.travelport.com/schema/air_v52_0\" SolutionResult=\"true\" TraceId=\"" + newGuid + "\" TargetBranch=\"" + _targetBranch + "\" ReturnUpsellFare =\"true\">");
            sbReq.Append("<BillingPointOfSaleInfo xmlns=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
            sbReq.Append("<SearchAirLeg>");
            sbReq.Append("<SearchOrigin>");
            sbReq.Append("<CityOrAirport xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"" + _GetfligthModel.origin + "\" PreferCity=\"true\" />");
            sbReq.Append("</SearchOrigin>");
            sbReq.Append("<SearchDestination>");
            sbReq.Append("<CityOrAirport xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"" + _GetfligthModel.destination + "\" PreferCity=\"true\" />");
            sbReq.Append("</SearchDestination>");

            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                sbReq.Append("<SearchDepTime PreferredTime=\"" + _GetfligthModel.beginDate + "\"/>");
            }
            else
            {
                sbReq.Append("<SearchDepTime PreferredTime=\"" + _GetfligthModel.endDate + "\"/>");

            }
            sbReq.Append("</SearchAirLeg>");
            sbReq.Append("<AirSearchModifiers>");
            sbReq.Append("<PreferredProviders>");
            sbReq.Append("<Provider xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"1G\" />");
            sbReq.Append("</PreferredProviders>");

            // Start for prohibited carrier
            sbReq.Append("<ProhibitedCarriers>");
            sbReq.Append("<Carrier Code='H1' xmlns=\"http://www.travelport.com/schema/common_v52_0\"/>");
            sbReq.Append("</ProhibitedCarriers>");
            //End  for prohibited carrier

            sbReq.Append("</AirSearchModifiers>");
            if (_GetfligthModel.passengercount != null)
            {
                if (_GetfligthModel.passengercount.adultcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.adultcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"ADT\"/>");
                    }
                }

                if (_GetfligthModel.passengercount.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.infantcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"INF\" Age=\"01\"/>");
                    }
                }

                if (_GetfligthModel.passengercount.childcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.childcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"CNN\" Age=\"10\"/>");
                    }
                }
            }
            else
            {

                if (_GetfligthModel.adultcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.adultcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"ADT\" />");
                    }
                }

                if (_GetfligthModel.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.infantcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"INF\" Age=\"01\"/>");
                    }
                }

                if (_GetfligthModel.childcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.childcount; i++)
                    {
                        sbReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"CNN\" Age=\"10\"/>");
                    }
                }
            }
            sbReq.Append("<AirPricingModifiers>");
            sbReq.Append("<AccountCodes>");
            sbReq.Append("<AccountCode xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"-\" />");
            sbReq.Append("</AccountCodes>");
            sbReq.Append("</AirPricingModifiers>");
            sbReq.Append("</LowFareSearchReq></soap:Body></soap:Envelope>");





            //sbReq.Append("<air:LowFareSearchReq xmlns:com=\"http://www.travelport.com/schema/common_v52_0\" xmlns:air=\"http://www.travelport.com/schema/air_v52_0\" AuthorizedBy=\"ENDFARE\" ");
            //sbReq.Append("SolutionResult=\"true\" TraceId=\"" + newGuid + "\" TargetBranch=\"" + _objAvail._targetBranch + "\">");
            //sbReq.Append("<BillingPointOfSaleInfo xmlns=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
            //sbReq.Append("<air:SearchAirLeg>");
            //sbReq.Append("<air:SearchOrigin><com:CityOrAirport Code=\"" + _GetfligthModel.origin + "\"/></air:SearchOrigin>");
            //sbReq.Append("<air:SearchDestination><com:CityOrAirport Code=\"" + _GetfligthModel.destination + "\"/></air:SearchDestination>");
            //sbReq.Append("<air:SearchDepTime PreferredTime=\"" + _GetfligthModel.beginDate + "\"/>");
            //sbReq.Append("<air:AirLegModifiers><air:PreferredCabins><com:CabinClass Type=\"Economy\"/></air:PreferredCabins></air:AirLegModifiers>");
            //sbReq.Append("</air:SearchAirLeg><air:AirSearchModifiers OrderBy=\"DepartureTime\">");
            //sbReq.Append("<air:PreferredProviders><com:Provider Code=\"1G\"/></air:PreferredProviders>");

            ////sbReq.Append("<air:PermittedCarriers xmlns=\"http://www.travelport.com/schema/common_v52_0\">");
            ////sbReq.Append("<Carrier Code='9W' xmlns=\"http://www.travelport.com/schema/common_v52_0\"/>");
            ////sbReq.Append("<Carrier Code='AI' xmlns=\"http://www.travelport.com/schema/common_v52_0\"/>");
            ////sbReq.Append("<Carrier Code='UK' xmlns=\"http://www.travelport.com/schema/common_v52_0\"/>");
            ////sbReq.Append("</air:PermittedCarriers>");
            //sbReq.Append("</air:AirSearchModifiers>");

            //if (_GetfligthModel.passengercount != null)
            //{
            //    if (_GetfligthModel.passengercount.adultcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.passengercount.adultcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"ADT\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUA01\"/>");
            //        }
            //    }

            //    if (_GetfligthModel.passengercount.infantcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.passengercount.infantcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"INF\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUB02\" PricePTCOnly=\"true\" Age=\"01\"/>");
            //        }
            //    }

            //    if (_GetfligthModel.passengercount.childcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.passengercount.childcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"CNN\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUC03\" Age=\"10\"/>");
            //        }
            //    }
            //}
            //else
            //{

            //    if (_GetfligthModel.adultcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.adultcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"ADT\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUA01\"/>");
            //        }
            //    }

            //    if (_GetfligthModel.infantcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.infantcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"INF\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUB02\" PricePTCOnly=\"true\" Age=\"01\"/>");
            //        }
            //    }

            //    if (_GetfligthModel.childcount != 0)
            //    {
            //        for (int i = 0; i < _GetfligthModel.childcount; i++)
            //        {
            //            sbReq.Append("<com:SearchPassenger Code=\"CNN\" BookingTravelerRef=\"ilay2SzXTkSUYRO+0owUC03\" Age=\"10\"/>");
            //        }
            //    }
            //}
            //sbReq.Append("<air:AirPricingModifiers FaresIndicator=\"AllFares\" ETicketability=\"Yes\" CurrencyType=\"INR\">");
            //sbReq.Append("<air:FlightType RequireSingleCarrier=\"true\" MaxConnections=\"1\" MaxStops=\"1\" NonStopDirects=\"true\" StopDirects=\"true\" SingleOnlineCon=\"true \"/></air:AirPricingModifiers>");
            //sbReq.Append("</air:LowFareSearchReq></soap:Body></soap:Envelope>");

            string res = Methodshit.HttpPost(_testURL, sbReq.ToString(), _userName, _password);
            SetSessionValue("GDSAvailibilityRequest", JsonConvert.SerializeObject(_GetfligthModel));
            SetSessionValue("GDSPassengerModel", JsonConvert.SerializeObject(_GetfligthModel));


            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                logs.WriteLogs("URL: " + _testURL + "\n\n Request: " + sbReq + "\n\n Response: " + res, "GetAvailability", "GDSOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(sbReq) + "\n\n Response: " + JsonConvert.SerializeObject(res), "GetAvailability", "GDSRT");
            }
            return res;
        }

        public string AirPriceGet(string _testURL, StringBuilder fareRepriceReq, SimpleAvailabilityRequestModel _GetfligthModel, string newGuid, string _targetBranch, string _userName, string _password, dynamic Airfaredata, string _AirlineWay)
        {

            int count = 0;
            int paxCount = 0;
            int legcount = 0;
            string origin = string.Empty;
            int legKeyCounter = 0;

            fareRepriceReq = new StringBuilder();
            fareRepriceReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            fareRepriceReq.Append("<soap:Body>");

            fareRepriceReq.Append("<AirPriceReq xmlns=\"http://www.travelport.com/schema/air_v52_0\" TraceId=\"" + newGuid + "\" AuthorizedBy = \"Travelport\" TargetBranch=\"" + _targetBranch + "\">");
            fareRepriceReq.Append("<BillingPointOfSaleInfo xmlns=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
            fareRepriceReq.Append("<AirItinerary>");
            //< AirSegment Key = "nX2BdBWDuDKAf9mT8SBAAA==" AvailabilitySource = "P" Equipment = "32A" AvailabilityDisplayType = "Fare Shop/Optimal Shop" Group = "0" Carrier = "AI" FlightNumber = "860" Origin = "DEL" Destination = "BOM" DepartureTime = "2024-07-25T02:15:00.000+05:30" ArrivalTime = "2024-07-25T04:30:00.000+05:30" FlightTime = "135" Distance = "708" ProviderCode = "1G" ClassOfService = "T" />


            // to do
            string segmentIdData = Airfaredata.Segmentiddata;
            string[] segmentIds = segmentIdData.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            string segmentIdAtIndex0 = string.Empty;
            string segmentIdAtIndex1 = string.Empty;
            string segmentIdAtIndex2 = string.Empty;
            // Checking if the array has at least two elements
            if (segmentIds.Length == 3)
            {
                segmentIdAtIndex0 = segmentIds[0];
                segmentIdAtIndex1 = segmentIds[1];
                segmentIdAtIndex2 = segmentIds[2];

            }
            else if (segmentIds.Length == 2)
            {
                // Accessing elements by index
                segmentIdAtIndex0 = segmentIds[0];
                segmentIdAtIndex1 = segmentIds[1];
            }
            else
            {
                segmentIdAtIndex0 = segmentIds[0];
            }


            foreach (var segment in Airfaredata.segments)
            {
                if (count == 0)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex0;
                }
                else if (count == 1)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex1;
                }
                else
                {
                    segmentIdAtIndex0 = segmentIdAtIndex2;
                }
                fareRepriceReq.Append("<AirSegment Key=\"" + segmentIdAtIndex0 + "\" AvailabilitySource = \"" + segment.designator._AvailabilitySource + "\" Equipment = \"" + segment.designator._Equipment + "\" AvailabilityDisplayType = \"" + segment.designator._AvailabilityDisplayType + "\" ");
                fareRepriceReq.Append("Group = \"" + segment.designator._Group + "\" Carrier = \"" + segment.identifier.carrierCode + "\" FlightNumber = \"" + segment.identifier.identifier + "\" ");
                fareRepriceReq.Append("Origin = \"" + segment.designator.origin + "\" Destination = \"" + segment.designator.destination + "\" ");
                fareRepriceReq.Append("DepartureTime = \"" + Convert.ToDateTime(segment.designator._DepartureDate).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz") + "\" ArrivalTime = \"" + Convert.ToDateTime(segment.designator._ArrivalDate).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz") + "\" ");
                fareRepriceReq.Append("FlightTime = \"" + segment.designator._FlightTime + "\" Distance = \"" + segment.designator._Distance + "\" ProviderCode = \"" + segment.designator._ProviderCode + "\" ClassOfService = \"" + segment.designator._ClassOfService + "\" />");
                count++;
            }

            fareRepriceReq.Append("</AirItinerary>");
            fareRepriceReq.Append("<AirPricingModifiers InventoryRequestType=\"DirectAccess\">");
            fareRepriceReq.Append("<BrandModifiers>");
            fareRepriceReq.Append("<FareFamilyDisplay ModifierType=\"FareFamily\"/>");
            fareRepriceReq.Append("</BrandModifiers>");
            fareRepriceReq.Append("</AirPricingModifiers>");
            if (_GetfligthModel.passengercount != null)
            {
                if (_GetfligthModel.passengercount.adultcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.adultcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"ADT\" Key=\"" + paxCount + "\"/>");
                        paxCount++;
                    }
                }
                if (_GetfligthModel.passengercount.childcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.childcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"CNN\" Key=\"" + paxCount + "\" Age=\"10\"/>");
                        paxCount++;
                    }
                }

                if (_GetfligthModel.passengercount.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.infantcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"INF\"  Key=\"" + paxCount + "\" Age=\"01\"/>");
                        paxCount++;
                    }
                }

            }
            else
            {

                if (_GetfligthModel.adultcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.adultcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\"  Key=\"" + paxCount + "\" Code=\"ADT\" />");
                        paxCount++;
                    }
                }

                if (_GetfligthModel.childcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.childcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + paxCount + "\" Code=\"CNN\" Age=\"10\"/>");
                        paxCount++;
                    }
                }

                if (_GetfligthModel.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.infantcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + paxCount + "\" Code=\"INF\" Age=\"01\"/>");
                        paxCount++;
                    }
                }


            }
            fareRepriceReq.Append("<AirPricingCommand>");
            if (segmentIds.Length == 3)
            {
                segmentIdAtIndex0 = segmentIds[0];
                segmentIdAtIndex1 = segmentIds[1];
                segmentIdAtIndex2 = segmentIds[2];
            }
            else if (segmentIds.Length == 2)
            {
                // Accessing elements by index
                segmentIdAtIndex0 = segmentIds[0];
                segmentIdAtIndex1 = segmentIds[1];
            }
            else
            {
                segmentIdAtIndex0 = segmentIds[0];
            }
            foreach (var segment in Airfaredata.segments)
            {
                if (legKeyCounter == 0)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex0;
                }
                else if (legKeyCounter == 1)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex1;
                }
                else
                {
                    segmentIdAtIndex0 = segmentIdAtIndex2;
                }
                fareRepriceReq.Append("<AirSegmentPricingModifiers AirSegmentRef = \"" + segmentIdAtIndex0 + "\">");
                fareRepriceReq.Append("<PermittedBookingCodes>");
                fareRepriceReq.Append("<BookingCode Code = \"" + segment.designator._ClassOfService + "\"/>");
                fareRepriceReq.Append("</PermittedBookingCodes>");
                fareRepriceReq.Append("</AirSegmentPricingModifiers>");
                legKeyCounter++;
            }
            fareRepriceReq.Append("</AirPricingCommand>");
            fareRepriceReq.Append("<FormOfPayment xmlns = \"http://www.travelport.com/schema/common_v52_0\" Type = \"Credit\" />");
            fareRepriceReq.Append("</AirPriceReq></soap:Body></soap:Envelope>");



            string res = Methodshit.HttpPost(_testURL, fareRepriceReq.ToString(), _userName, _password);
            SetSessionValue("GDSAvailibilityRequest", JsonConvert.SerializeObject(_GetfligthModel));
            SetSessionValue("GDSPassengerModel", JsonConvert.SerializeObject(_GetfligthModel));


            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                logs.WriteLogs("URL: " + _testURL + "\n\n Request: " + fareRepriceReq + "\n\n Response: " + res, "GetAirPrice", "GDSOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(fareRepriceReq) + "\n\n Response: " + JsonConvert.SerializeObject(res), "GetAirprice", "GDSRT");
            }
            return res;
        }


        public string CreatePNR(string _testURL, StringBuilder createPNRReq, string newGuid, string _targetBranch, string _userName, string _password, string AdultTraveller, string _data, string _Total, string _AirlineWay, string? _pricesolution = null)
        {

            int count = 0;
            //int paxCount = 0;
            //int legcount = 0;
            //string origin = string.Empty;
            //int legKeyCounter = 0;

            createPNRReq = new StringBuilder();
            createPNRReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            createPNRReq.Append("<soap:Body>");
            createPNRReq.Append("<AirCreateReservationReq xmlns=\"http://www.travelport.com/schema/universal_v52_0\" TraceId=\"" + newGuid + "\" AuthorizedBy = \"Travelport\" TargetBranch=\"" + _targetBranch + "\" ProviderCode=\"1G\" RetainReservation=\"Both\">");
            createPNRReq.Append("<BillingPointOfSaleInfo xmlns=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
            List<passkeytype> passengerdetails = (List<passkeytype>)JsonConvert.DeserializeObject(AdultTraveller, typeof(List<passkeytype>));



            AirAsiaTripResponceModel Getdetails = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(_data, typeof(AirAsiaTripResponceModel));
            Getdetails.PriceSolution = _pricesolution.Replace("\\", "");

            if (passengerdetails.Count > 0)
            {
                for (int i = 0; i < passengerdetails.Count; i++)
                {
                    if (passengerdetails[i].passengertypecode == "ADT")
                    {
                        createPNRReq.Append("<BookingTraveler xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + count + "\"  TravelerType=\"ADT\" Age=\"40\" DOB=\"1984-07-25\">");
                    }
                    else if (passengerdetails[i].passengertypecode == "CHD")
                    {
                        createPNRReq.Append("<BookingTraveler xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + count + "\"  TravelerType=\"CNN\" Age=\"10\" DOB=\"2014-07-25\" >");
                    }
                    else if (passengerdetails[i].passengertypecode == "INF" || passengerdetails[i].passengertypecode == "INFT")
                    {
                        createPNRReq.Append("<BookingTraveler xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + count + "\" TravelerType=\"INF\" Age=\"1\" DOB=\"2023-08-25\" >");
                    }
                    else
                    {
                        createPNRReq.Append("<BookingTraveler xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + count + "\"  TravelerType=\"ADT\" Age=\"40\" DOB=\"1984-07-25\">");
                    }
                    if (!string.IsNullOrEmpty(passengerdetails[i].middle))
                    {
                        createPNRReq.Append("<BookingTravelerName  First=\"" + passengerdetails[i].first.ToUpper() + "\" Last=\"" + passengerdetails[i].last.ToUpper() + "\" Middle=\"" + passengerdetails[i].middle.ToUpper() + "\" Prefix=\"" + passengerdetails[i].title.ToUpper().Replace(".", "") + "\" />");
                    }
                    else
                    {
                        createPNRReq.Append("<BookingTravelerName  First=\"" + passengerdetails[i].first.ToUpper() + "\" Last=\"" + passengerdetails[i].last.ToUpper() + "\" Prefix=\"" + passengerdetails[i].title.ToUpper().Replace(".", "") + "\" />");
                    }
                    if (passengerdetails[i].passengertypecode == "ADT" || passengerdetails[i].passengertypecode == "CHD" || passengerdetails[i].passengertypecode == "CNN")
                    {
                        createPNRReq.Append("<PhoneNumber Number=\"" + passengerdetails[i].mobile + "\"  />");
                        createPNRReq.Append("<Email EmailID=\"" + passengerdetails[i].Email + "\" />");
                    }
                    else
                    {
                        createPNRReq.Append("<PhoneNumber Number=\"" + passengerdetails[0].mobile + "\"  />");
                        createPNRReq.Append("<Email EmailID=\"" + passengerdetails[0].Email + "\" />");
                    }

                    //if (!String.IsNullOrEmpty(paxDetail.FrequentFlierNumber) && paxDetail.FrequentFlierNumber.Length > 5)
                    //{
                    //if (segment_.Bonds[0].Legs[0].AirlineName.Equals("UK"))
                    //{
                    //createPNRReq.Append("<SSR  Key='" + count + "' Type='FQTV' Status='HK' Carrier='UK' FreeText='" + paxDetail.FrequentFlierNumber + "-" + paxDetail.LastName + "/" + paxDetail.FirstName + "" + paxDetail.Title.ToUpper() + "'/>");
                    //}
                    //else
                    //{
                    //  createPNRReq.Append("<com:LoyaltyCard SupplierCode='" + segment_.Bonds[0].Legs[0].AirlineName + "' CardNumber='" + paxDetail.FrequentFlierNumber + "'/>");
                    //}
                    //}
                    //if (!IsDomestic)
                    //{
                    //    if (IsSSR)
                    //    {
                    //        pnrreq.Append("<com:SSR Type='DOCS'  Key='" + count + "' FreeText='P/" + paxDetail.Nationality + "/" + paxDetail.PassportNo + "/" + paxDetail.Nationality + "/" + paxDetail.DOB.ToString("ddMMMyy") + "/" + PaxGender(paxDetail.Gender) + "/" + paxDetail.PassportExpiryDate.ToString("ddMMMyy") + "/" + paxDetail.FirstName + "/" + paxDetail.LastName + "' Carrier='" + segment_.Bonds[0].Legs[0].AirlineName + "'/>");
                    //    }
                    //    else if (ISSSR(segment_.Bonds))
                    //    {
                    //        pnrreq.Append("<com:SSR Type='DOCS'  Key='" + count + "' FreeText='P/" + paxDetail.Nationality + "/" + paxDetail.PassportNo + "/" + paxDetail.Nationality + "/" + paxDetail.DOB.ToString("ddMMMyy") + "/" + PaxGender(paxDetail.Gender) + "/" + paxDetail.PassportExpiryDate.ToString("ddMMMyy") + "/" + paxDetail.FirstName + "/" + paxDetail.LastName + "' Carrier='" + segment_.Bonds[0].Legs[0].AirlineName + "'/>");
                    //    }
                    //}
                    createPNRReq.Append("<SSR  Key=\"" + count + "\" Type=\"DOCS\"  FreeText=\"P/GB/S12345678/GB/20JUL76/M/01JAN16/" + passengerdetails[i].last.ToUpper() + "/" + passengerdetails[i].first.ToUpper() + "\" Carrier=\"" + Getdetails.journeys[0].segments[0].identifier.carrierCode + "\"/>");
                    string contractNo = string.Empty;
                    if (string.IsNullOrEmpty(contractNo))
                    {
                        contractNo = "CTCM " + passengerdetails[i].mobile + " PAX";
                    }
                    //if (!IsDomestic)
                    //{
                    //    if (IsSSR)
                    //    {
                    //        pnrreq.Append("<com:SSR Type='DOCS'  Key='" + count + "' FreeText='P/" + paxDetail.Nationality + "/" + paxDetail.PassportNo + "/" + paxDetail.Nationality + "/" + paxDetail.DOB.ToString("ddMMMyy") + "/" + PaxGender(paxDetail.Gender) + "/" + paxDetail.PassportExpiryDate.ToString("ddMMMyy") + "/" + paxDetail.FirstName + "/" + paxDetail.LastName + "' Carrier='" + segment_.Bonds[0].Legs[0].AirlineName + "'/>");
                    //    }
                    //    else if (ISSSR(segment_.Bonds))
                    //    {
                    //        pnrreq.Append("<com:SSR Type='DOCS'  Key='" + count + "' FreeText='P/" + paxDetail.Nationality + "/" + paxDetail.PassportNo + "/" + paxDetail.Nationality + "/" + paxDetail.DOB.ToString("ddMMMyy") + "/" + PaxGender(paxDetail.Gender) + "/" + paxDetail.PassportExpiryDate.ToString("ddMMMyy") + "/" + paxDetail.FirstName + "/" + paxDetail.LastName + "' Carrier='" + segment_.Bonds[0].Legs[0].AirlineName + "'/>");
                    //    }
                    //}
                    createPNRReq.Append("</BookingTraveler>");
                    count++;
                }
                createPNRReq.Append("<FormOfPayment xmlns=\"http://www.travelport.com/schema/common_v52_0\" Type=\"Cash\" Key=\"1\" />");
                createPNRReq.Append(Getdetails.PriceSolution);
                createPNRReq.Append("<ActionStatus xmlns=\"http://www.travelport.com/schema/common_v52_0\" Type=\"ACTIVE\" TicketDate=\"T*\" ProviderCode=\"1G\" />");
                createPNRReq.Append("<Payment xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"2\" Type=\"Itinerary\" FormOfPaymentRef=\"1\" Amount=\"INR" + _Total + "\" />");
                createPNRReq.Append("</AirCreateReservationReq></soap:Body></soap:Envelope>");
            }

            string res = Methodshit.HttpPost(_testURL, createPNRReq.ToString(), _userName, _password);
            //SetSessionValue("GDSAvailibilityRequest", JsonConvert.SerializeObject(_GetfligthModel));
            //SetSessionValue("GDSPassengerModel", JsonConvert.SerializeObject(_GetfligthModel));
            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                logs.WriteLogs("URL: " + _testURL + "\n\n Request: " + createPNRReq + "\n\n Response: " + res, "GetPNR", "GDSOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(createPNRReq) + "\n\n Response: " + JsonConvert.SerializeObject(res), "GetPNR", "GDSRT");
            }
            return res;
        }

        public string RetrivePnr(string universalRlcode_, string _testURL, string newGuid, string _targetBranch, string _userName, string _password, string _AirlineWay)
        {
            StringBuilder retrivePnrReq = null;
            string pnrretriveRes = string.Empty;
            try
            {
                retrivePnrReq = new StringBuilder();
                //retrivePnrReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                //retrivePnrReq.Append("<soap:Body>");
                //retrivePnrReq.Append("<univ:UniversalRecordRetrieveReq xmlns:univ=\"http://www.travelport.com/schema/universal_v52_0\" AuthorizedBy=\"ENDFARE\" TargetBranch=\"" + _targetBranch + "\" TraceId=\"" + newGuid + "\">");
                //retrivePnrReq.Append("<com:BillingPointOfSaleInfo xmlns:com=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
                //retrivePnrReq.Append("<univ:ProviderReservationInfo ProviderLocatorCode=\"" + universalRlcode_ + "\" ProviderCode=\"1G\"/>");
                //retrivePnrReq.Append("</univ:UniversalRecordRetrieveReq>");
                //retrivePnrReq.Append("</soap:Body>");
                //retrivePnrReq.Append("</soap:Envelope>");

                retrivePnrReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                retrivePnrReq.Append("<soap:Body>");
                retrivePnrReq.Append("<UniversalRecordRetrieveReq xmlns=\"http://www.travelport.com/schema/universal_v52_0\" TraceId=\"" + newGuid + "\" AuthorizedBy=\"Travelport\" TargetBranch=\"" + _targetBranch + "\">");
                retrivePnrReq.Append("<BillingPointOfSaleInfo xmlns=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"uAPI\" />");
                retrivePnrReq.Append("<UniversalRecordLocatorCode>" + universalRlcode_ + "</UniversalRecordLocatorCode>");
                retrivePnrReq.Append("</UniversalRecordRetrieveReq>");
                retrivePnrReq.Append("</soap:Body>");
                retrivePnrReq.Append("</soap:Envelope>");


                //retrivePnrReq.Append("<univ:UniversalRecordRetrieveReq xmlns:univ=\"http://www.travelport.com/schema/universal_v52_0\" AuthorizedBy=\"ENDFARE\" TargetBranch=\"" + _targetBranch + "\" TraceId=\"" + newGuid + "\">");
                //retrivePnrReq.Append("<com:BillingPointOfSaleInfo xmlns:com=\"http://www.travelport.com/schema/common_v52_0\" OriginApplication=\"UAPI\"/>");
                //retrivePnrReq.Append("<univ:ProviderReservationInfo ProviderLocatorCode=\"" + universalRlcode_ + "\" ProviderCode=\"1G\"/>");
                //retrivePnrReq.Append("</univ:UniversalRecordRetrieveReq>");




                //retrivePnrReq.Append("<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>");
                //retrivePnrReq.Append("<s:Header>");
                //retrivePnrReq.Append("<Action s:mustUnderstand='1' xmlns='http://schemas.microsoft.com/ws/2005/05/addressing/none'/>");
                //retrivePnrReq.Append("</s:Header>");
                //retrivePnrReq.Append("<s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>");
                ////if (IsDomestic)
                ////{
                ////retrivePnrReq.Append("<univ:UniversalRecordRetrieveReq TraceId='" + GetTId(5) + "' TargetBranch='" + _ticketingCredential.Split('|')[0] + "' AuthorizedBy='user'  xmlns:univ='http://www.travelport.com/schema/universal_v46_0'>");
                //retrivePnrReq.Append("<univ:UniversalRecordRetrieveReq xmlns:univ=\"http://www.travelport.com/schema/universal_v52_0\" TraceId=\"" + newGuid + "\" TargetBranch=\"" + _targetBranch + "\" AuthorizedBy=\"ENDFARE\">");
                ////}
                ////else
                ////{
                ////retrivePnrReq.Append("<univ:UniversalRecordRetrieveReq TraceId='" + GetTId(5) + "' TargetBranch='" + _ticketingCredential.Split('|')[0] + "' AuthorizedBy='user'  xmlns:univ='http://www.travelport.com/schema/universal_v46_0'>");
                ////}
                //retrivePnrReq.Append("<com:BillingPointOfSaleInfo OriginApplication=\"UAPI\" xmlns:com=\"http://www.travelport.com/schema/common_v52_0\" />");
                //retrivePnrReq.Append("<univ:ProviderReservationInfo ProviderCode=\"1G\" ProviderLocatorCode=\"" + universalRlcode_ + "\" />");
                //retrivePnrReq.Append("</univ:UniversalRecordRetrieveReq>");
                //retrivePnrReq.Append("</s:Body>");
                //retrivePnrReq.Append("</s:Envelope>");
                pnrretriveRes = Methodshit.HttpPost(_testURL, retrivePnrReq.ToString(), _userName, _password);
            }
            catch (SystemException ex_)
            {
                //Utility.BookingTracker.LogTrackBooking(TransactionId, "[Cloud][TravelPortAPI][PnrRetriveResErr]", pnrretriveRes + "_" + sex_.Message + "_" + sex_.StackTrace, false, "", "");
            }

            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                logs.WriteLogs("URL: " + _testURL + "\n\n Request: " + retrivePnrReq + "\n\n Response: " + pnrretriveRes, "RetrivePnr", "GDSOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(retrivePnrReq) + "\n\n Response: " + JsonConvert.SerializeObject(pnrretriveRes), "RetrivePnr", "GDSRT");
            }
            return pnrretriveRes;
        }

        public string GetTicketdata(string universalRlcode_, string _testURL, string newGuid, string _targetBranch, string _userName, string _password, string _AirlineWay)
        {
            StringBuilder retriveTicketPnrReq = null;
            string pnrticketretriveRes = string.Empty;
            try
            {
                retriveTicketPnrReq = new StringBuilder();
                retriveTicketPnrReq.Append("<soap:Envelope xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
                retriveTicketPnrReq.Append("<soap:Body>");
                //ReturnInfoOnFail =\"true\" BulkTicket=\"false\"
                retriveTicketPnrReq.Append("<AirTicketingReq TargetBranch=\"" + _targetBranch + "\" TraceId=\"" + newGuid + "\" AuthorizedBy=\"test\"  xmlns=\"http://www.travelport.com/schema/air_v52_0\">");
                retriveTicketPnrReq.Append("<BillingPointOfSaleInfo OriginApplication=\"UAPI\" xmlns=\"http://www.travelport.com/schema/common_v52_0\"/>");
                retriveTicketPnrReq.Append("<AirReservationLocatorCode>" + universalRlcode_ + "</AirReservationLocatorCode>");
                retriveTicketPnrReq.Append("</AirTicketingReq>");
                retriveTicketPnrReq.Append("</soap:Body>");
                retriveTicketPnrReq.Append("</soap:Envelope>");

                pnrticketretriveRes = Methodshit.HttpPost(_testURL, retriveTicketPnrReq.ToString(), _userName, _password);
            }
            catch (SystemException ex_)
            {
                //Utility.BookingTracker.LogTrackBooking(TransactionId, "[Cloud][TravelPortAPI][PnrRetriveResErr]", pnrretriveRes + "_" + sex_.Message + "_" + sex_.StackTrace, false, "", "");
            }

            if (_AirlineWay.ToLower() == "gdsoneway")
            {
                logs.WriteLogs("URL: " + _testURL + "\n\n Request: " + retriveTicketPnrReq + "\n\n Response: " + pnrticketretriveRes, "RetriveTicketPnr", "GDSOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(retriveTicketPnrReq) + "\n\n Response: " + JsonConvert.SerializeObject(pnrticketretriveRes), "RetriveTicketPnr", "GDSRT");
            }
            return pnrticketretriveRes;
        }


    }
}
