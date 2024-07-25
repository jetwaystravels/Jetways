using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Diagnostics;
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
            sbReq.Append("<SearchDepTime PreferredTime=\"" + _GetfligthModel.beginDate + "\"/>");
            sbReq.Append("</SearchAirLeg>");
            sbReq.Append("<AirSearchModifiers>");
            sbReq.Append("<PreferredProviders>");
            sbReq.Append("<Provider xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"1G\" />");
            sbReq.Append("</PreferredProviders>");
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
            // Checking if the array has at least two elements
            if (segmentIds.Length >= 2)
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
                if(count==0)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex0;
                }
                else
                {
                    segmentIdAtIndex0 = segmentIdAtIndex1;
                }
                fareRepriceReq.Append("<AirSegment Key=\"" + segmentIdAtIndex0 + "\" AvailabilitySource = \""+ segment.designator._AvailabilitySource + "\" Equipment = \""+ segment.designator._Equipment + "\" AvailabilityDisplayType = \""+ segment.designator._AvailabilityDisplayType + "\" ");
                fareRepriceReq.Append("Group = \""+ segment.designator._Group + "\" Carrier = \""+ segment.identifier.carrierCode + "\" FlightNumber = \""+ segment.identifier.identifier+ "\" ");
                fareRepriceReq.Append("Origin = \""+ segment.designator.origin + "\" Destination = \""+ segment.designator.destination+ "\" ");
                fareRepriceReq.Append("DepartureTime = \""+ Convert.ToDateTime(segment.designator._DepartureDate).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz") + "\" ArrivalTime = \""+ Convert.ToDateTime(segment.designator._ArrivalDate).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz") + "\" ");
                fareRepriceReq.Append("FlightTime = \""+ segment.designator._FlightTime + "\" Distance = \""+ segment.designator._Distance + "\" ProviderCode = \""+ segment.designator._ProviderCode + "\" ClassOfService = \""+ segment.designator._ClassOfService + "\" />");
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

                if (_GetfligthModel.passengercount.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.passengercount.infantcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Code=\"INF\"  Key=\"" + paxCount + "\" Age=\"01\"/>");
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

                if (_GetfligthModel.infantcount != 0)
                {
                    for (int i = 0; i < _GetfligthModel.infantcount; i++)
                    {
                        fareRepriceReq.Append("<SearchPassenger xmlns=\"http://www.travelport.com/schema/common_v52_0\" Key=\"" + paxCount + "\" Code=\"INF\" Age=\"01\"/>");
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
            }
            fareRepriceReq.Append("<AirPricingCommand>");
            if (segmentIds.Length >= 2)
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
                if(legKeyCounter==0)
                {
                    segmentIdAtIndex0 = segmentIdAtIndex0;
                }
                else
                {
                    segmentIdAtIndex0 = segmentIdAtIndex1;
                }
                fareRepriceReq.Append("<AirSegmentPricingModifiers AirSegmentRef = \""+ segmentIdAtIndex0 + "\">");
                fareRepriceReq.Append("<PermittedBookingCodes>");
                fareRepriceReq.Append("<BookingCode Code = \""+ segment.designator._ClassOfService + "\"/>");
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

    }
}
