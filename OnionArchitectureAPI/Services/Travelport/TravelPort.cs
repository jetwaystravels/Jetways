using DomainLayer.Model;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;
using Utility;

namespace OnionArchitectureAPI.Services.Travelport
{
    public class TravelPort
    {
        Logs logs = new Logs();
        string _targetBranch = string.Empty;
        string _userName = string.Empty;
        string _password = string.Empty;
        public TravelPort(string tragetBranch_, string userName_, string password_)
        {
            _targetBranch = tragetBranch_;
            _userName = userName_;
            _password = password_;
        }

        public string GetAvailabilty(string _testURL, StringBuilder sbReq, TravelPort _objAvail, SimpleAvailabilityRequestModel _GetfligthModel, string newGuid, string _AirlineWay)
        {
            sbReq = new StringBuilder();
            sbReq.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sbReq.Append("<soap:Body>");
            sbReq.Append("<LowFareSearchReq xmlns=\"http://www.travelport.com/schema/air_v52_0\" SolutionResult=\"true\" TraceId=\"" + newGuid + "\" TargetBranch=\"" + _objAvail._targetBranch + "\" ReturnUpsellFare =\"true\">");
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

            string res = Methodshit.HttpPost(_testURL, sbReq.ToString(), _objAvail._userName, _objAvail._password);

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
    }
}
