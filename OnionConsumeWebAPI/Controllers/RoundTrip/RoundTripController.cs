using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using static DomainLayer.Model.SeatMapResponceModel;

namespace OnionConsumeWebAPI.Controllers.RoundTrip
{
    public class RoundTripController : Controller
    {

        int stopFilter = 0;
        public IActionResult RTFlightView()
        {

            var searchcount = TempData["count"];
            ViewData["count"] = searchcount;

            var origindata = TempData["origin"];
            ViewData["origin"] = origindata;

            var destinationdata = TempData["destination"];
            ViewData["destination"] = destinationdata;

            var searchcountR = TempData["countR"];
            ViewData["countR"] = searchcountR;

            var origindataR = TempData["originR"];
            ViewData["originR"] = origindataR;

            var destinationdataR = TempData["destinationR"];
            ViewData["destinationR"] = destinationdataR;

            ViewModel vmobj = new ViewModel();
            string Leftshowpopupdata = HttpContext.Session.GetString("LeftReturnViewFlightView");
            string Rightshowpopupdata = HttpContext.Session.GetString("RightReturnFlightView");

            List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
            List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;
            if (!string.IsNullOrEmpty(Leftshowpopupdata))
            {
                LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
            }

            if (!string.IsNullOrEmpty(Rightshowpopupdata))
            {
                RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
            }

            vmobj.SimpleAvailibilityaAddResponcelist = LeftdeserializedObjects;
            vmobj.SimpleAvailibilityaAddResponcelistR = RightdeserializedObjects;

            string RTFlightEditData = HttpContext.Session.GetString("PassengerModelR");
            SimpleAvailabilityRequestModel simpleAvailabilityRequestModel = null;
            if (!string.IsNullOrEmpty(RTFlightEditData))
            {
                simpleAvailabilityRequestModel = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(RTFlightEditData);
            }
            vmobj.simpleAvailabilityRequestModelEdit = simpleAvailabilityRequestModel;
            // HttpContext.Session.SetString("FlightDetail", JsonConvert.SerializeObject(vmobj));
            return View(vmobj);
        }
        public IActionResult PostReturnAATripsellView(int uniqueId, int uniqueIdR)
        {
            string Leftshowpopupdata = HttpContext.Session.GetString("LeftReturnViewFlightView");
            string Rightshowpopupdata = HttpContext.Session.GetString("RightReturnFlightView");
            List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
            List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;
            LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
            RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
            var filteredDataLeft = LeftdeserializedObjects.Where(x => x.uniqueId == uniqueId).ToList();
            var filteredDataRight = RightdeserializedObjects.Where(m => m.uniqueId == uniqueIdR).ToList();
            ViewModel vmobject = new ViewModel();
            vmobject.SimpleAvailibilityaAddResponcelist = filteredDataLeft;
            vmobject.SimpleAvailibilityaAddResponcelistR = filteredDataRight;
            return View(vmobject);

        }
        [HttpPost]
        public IActionResult FlightView0111(string sortOrderName, List<string> FilterIdAirLine, List<int> FilterId)
        {
            ViewModel viewModelobject = new ViewModel();
            string OnewayFlightData = HttpContext.Session.GetString("OneWayFlightView");
            List<SimpleAvailibilityaAddResponce> OnewaydeserializedObjects = null;
            OnewaydeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(OnewayFlightData);
            ViewBag.NameSortParam = sortOrderName == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.PriceSortParam = sortOrderName == "price_desc" ? "price_asc" : "price_desc";
            ViewBag.DepartSortParam = sortOrderName == "deprt_desc" ? "deprt_asc" : "deprt_desc";
            ViewBag.arriveSortParam = sortOrderName == "arrive_desc" ? "arrive_asc" : "arrive_desc";
            ViewBag.durationSortParam = sortOrderName == "duration_desc" ? "duration_asc" : "duration_desc";
            switch (sortOrderName)
            {
                case "name_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(f => f.Airline.ToString()).ToList();
                    break;
                case "name_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(f => f.Airline.ToString()).ToList();
                    break;
                case "price_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(p => p.fareTotalsum).ToList();
                    break;
                case "price_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(p => p.fareTotalsum).ToList();
                    break;
                case "deprt_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.departure).ToList();
                    break;
                case "deprt_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.departure).ToList();
                    break;
                case "arrive_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.ArrivalTime).ToList();
                    break;
                case "arrive_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.ArrivalTime).ToList();
                    break;
                case "duration_desc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderByDescending(d => d.designator.formatTime).ToList();
                    break;
                case "duration_asc":
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(d => d.designator.formatTime).ToList();
                    break;
                default:
                    OnewaydeserializedObjects = OnewaydeserializedObjects.OrderBy(p => p.fareTotalsum).ToList();
                    break;

            }
            if (FilterId != null && FilterId.Count > 0)
            {
                foreach (int value in FilterId)
                {
                    switch (value)
                    {
                        case 0:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        case 1:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        case 2:

                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                        default:
                            OnewaydeserializedObjects = OnewaydeserializedObjects.Where(x => FilterId.Contains(x.stops)).ToList();
                            break;
                    }
                }

            }
            else if (FilterIdAirLine.Count > 0 && FilterIdAirLine.Count >= 0)
            {
                var FilterAirLineData = OnewaydeserializedObjects.Where(x => FilterIdAirLine.Contains(x.Airline.ToString())).ToList();
                viewModelobject.SimpleAvailibilityaAddResponcelist = FilterAirLineData;
                return PartialView("_FlightResultsSortingPartialView", viewModelobject);
            }
            viewModelobject.SimpleAvailibilityaAddResponcelist = OnewaydeserializedObjects;
            return PartialView("_FlightResultsSortingPartialView", viewModelobject);
        }
        [HttpPost]
        public IActionResult RTFlightView(List<int> selectedIds, List<string> RTFilterIdAirLine)
        {

            string LeftshowpopupdataStops = HttpContext.Session.GetString("LeftReturnViewFlightView");
            string RightshowpopupdataStops = HttpContext.Session.GetString("RightReturnFlightView");

            ViewModel vmobj = new ViewModel();
            List<SimpleAvailibilityaAddResponce> RightdeserializedStops = null;
            List<SimpleAvailibilityaAddResponce> LeftdeserializedStops = null;

            if (!string.IsNullOrEmpty(LeftshowpopupdataStops))
            {
                LeftdeserializedStops = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(LeftshowpopupdataStops);
            }

            if (!string.IsNullOrEmpty(RightshowpopupdataStops))
            {
                RightdeserializedStops = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(RightshowpopupdataStops);
            }

            List<SimpleAvailibilityaAddResponce> FilterStopData = new List<SimpleAvailibilityaAddResponce>();
            List<SimpleAvailibilityaAddResponce> FilterStopDataRight = new List<SimpleAvailibilityaAddResponce>();

            if (selectedIds != null && selectedIds.Count > 0)
            {
                FilterStopData = LeftdeserializedStops?.Where(x => selectedIds.Contains(x.stops)).ToList();
                FilterStopDataRight = RightdeserializedStops?.Where(x => selectedIds.Contains(x.stops)).ToList();

                // Apply filtering based on selected stop counts
                foreach (int value in selectedIds)
                {
                    switch (value)
                    {
                        case 0:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            break;
                        case 1:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            break;
                        case 2:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            break;
                        default:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            break;
                    }
                }
            }
            else if (RTFilterIdAirLine.Count > 0 && RTFilterIdAirLine.Count >= 0)
            {

                FilterStopData = LeftdeserializedStops?.Where(x => RTFilterIdAirLine.Contains(x.Airline.ToString())).ToList();
                FilterStopDataRight = RightdeserializedStops?.Where(x => RTFilterIdAirLine.Contains(x.Airline.ToString())).ToList();


                vmobj.SimpleAvailibilityaAddResponcelist = FilterStopData;
                vmobj.SimpleAvailibilityaAddResponcelistR = FilterStopDataRight;
                return PartialView("_RTFlightResultsSortingPartialView", vmobj);
            }

            vmobj.SimpleAvailibilityaAddResponcelist = FilterStopData ?? new List<SimpleAvailibilityaAddResponce>();
            vmobj.SimpleAvailibilityaAddResponcelistR = FilterStopDataRight ?? new List<SimpleAvailibilityaAddResponce>();

            return PartialView("_RTFlightResultsSortingPartialView", vmobj);

        }


    }
}
