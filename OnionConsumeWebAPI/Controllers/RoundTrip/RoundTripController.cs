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
        public IActionResult RTFlightView(string sortOrderName, string sortOrderNameR, List<int> selectedIds, List<int> selectedIdsRight, List<string> RTFilterIdAirLine, List<string> departure, List<string> arrival, List<string> departureRight, List<string> arrivalRight)
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

            if (departure.Count > 0)
            {
                if (departure[0] == null)
                {
                    departure = departure.Where(d => d != null).ToList();
                    //departure = new List<string>();
                }
            }
            if (arrival.Count > 0)
            {
                if (arrival[0] == null)
                {
                    //arrival = new List<string>();
                    arrival = arrival.Where(d => d != null).ToList();
                }
            }
            if (departureRight.Count > 0)
            {
                if (departureRight[0] == null)
                {
                    departureRight = departureRight.Where(d => d != null).ToList();
                }
            }
            if (arrivalRight.Count > 0)
            {
                if (arrivalRight[0] == null)
                {
                    arrivalRight = arrivalRight.Where(d => d != null).ToList();
                }
            }

            string Leftshowpopupdata = HttpContext.Session.GetString("LeftReturnViewFlightView");
            string Rightshowpopupdata = HttpContext.Session.GetString("RightReturnFlightView");
            if (string.IsNullOrEmpty(Leftshowpopupdata))
            {
                return View("Error");
            }

            List<SimpleAvailibilityaAddResponce> filteredFlightsReturn = LeftdeserializedStops;
            List<SimpleAvailibilityaAddResponce> filteredFlightsRight = RightdeserializedStops;
            if (departure != null && departure.Count > 0)
            {
                filteredFlightsReturn = filteredFlightsReturn.Where(flight =>
                    departure.Any(d =>
                        (d.ToLower() == "before_6am" && flight.designator.departure.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (d.ToLower() == "6am_to_12pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (d.ToLower() == "12pm_to_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (d.ToLower() == "after_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();

                LeftdeserializedStops = filteredFlightsReturn.ToList();
                vmobj.SimpleAvailibilityaAddResponcelist = LeftdeserializedStops;
            }
            if (arrival != null && arrival.Count > 0)
            {
                filteredFlightsReturn = filteredFlightsReturn.Where(flight =>
                    arrival.Any(a =>
                        (a.ToLower() == "before_6am" && flight.designator.arrival.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (a.ToLower() == "6am_to_12pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (a.ToLower() == "12pm_to_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (a.ToLower() == "after_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();

                LeftdeserializedStops = filteredFlightsReturn.ToList();
                vmobj.SimpleAvailibilityaAddResponcelist = LeftdeserializedStops;
            }
            //Return Timing Filter
            if (departureRight != null && departureRight.Count > 0)
            {
                filteredFlightsRight = filteredFlightsRight.Where(flight =>
                    departureRight.Any(a =>
                        (a.ToLower() == "before_6am" && flight.designator.departure.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (a.ToLower() == "6am_to_12pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (a.ToLower() == "12pm_to_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (a.ToLower() == "after_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();
                RightdeserializedStops = filteredFlightsRight.ToList();
                vmobj.SimpleAvailibilityaAddResponcelist = RightdeserializedStops;
            }
            if (arrivalRight != null && arrivalRight.Count > 0)
            {
                filteredFlightsRight = filteredFlightsRight.Where(flight =>
                    arrivalRight.Any(a =>
                        (a.ToLower() == "before_6am" && flight.designator.arrival.TimeOfDay < new TimeSpan(6, 0, 0)) ||
                        (a.ToLower() == "6am_to_12pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(12, 0, 0)) ||
                        (a.ToLower() == "12pm_to_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(18, 0, 0)) ||
                        (a.ToLower() == "after_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(18, 0, 0))
                    )).ToList();
                RightdeserializedStops = filteredFlightsRight.ToList();
                vmobj.SimpleAvailibilityaAddResponcelist = RightdeserializedStops;
            }

            ViewBag.NameSortParam = sortOrderName == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.PriceSortParam = sortOrderName == "price_desc" ? "price_asc" : "price_desc";
            ViewBag.DepartSortParam = sortOrderName == "deprt_desc" ? "deprt_asc" : "deprt_desc";
            ViewBag.arriveSortParam = sortOrderName == "arrive_desc" ? "arrive_asc" : "arrive_desc";
            ViewBag.durationSortParam = sortOrderName == "duration_desc" ? "duration_asc" : "duration_desc";

            switch (sortOrderName)
            {
                case "name_desc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderByDescending(f => f.Airline.ToString()).ToList();
                    break;
                case "name_asc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(f => f.Airline.ToString()).ToList();
                    break;
                case "price_desc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderByDescending(p => p.fareTotalsum).ToList();
                    break;
                case "price_asc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(p => p.fareTotalsum).ToList();
                    break;
                case "deprt_desc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderByDescending(d => d.designator.departure).ToList();
                    break;
                case "deprt_asc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(d => d.designator.departure).ToList();
                    break;
                case "arrive_desc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderByDescending(d => d.designator.arrival).ToList();
                    break;
                case "arrive_asc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(d => d.designator.arrival).ToList();
                    break;
                case "duration_desc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderByDescending(d => d.designator.formatTime).ToList();
                    break;
                case "duration_asc":
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(d => d.designator.formatTime).ToList();
                    break;
                default:
                    LeftdeserializedStops = LeftdeserializedStops.OrderBy(p => p.fareTotalsum).ToList();
                    break;

            }


            ViewBag.NameSortParam = sortOrderNameR == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.PriceSortParam = sortOrderNameR == "price_desc" ? "price_asc" : "price_desc";
            ViewBag.DepartSortParam = sortOrderNameR == "deprt_desc" ? "deprt_asc" : "deprt_desc";
            ViewBag.arriveSortParam = sortOrderNameR == "arrive_desc" ? "arrive_asc" : "arrive_desc";
            ViewBag.durationSortParam = sortOrderNameR == "duration_desc" ? "duration_asc" : "duration_desc";

            switch (sortOrderNameR)
            {
                case "name_desc":
                    RightdeserializedStops = RightdeserializedStops.OrderByDescending(f => f.Airline.ToString()).ToList();
                    break;
                case "name_asc":
                    RightdeserializedStops = RightdeserializedStops.OrderBy(f => f.Airline.ToString()).ToList();
                    break;
                case "price_desc":
                    RightdeserializedStops = RightdeserializedStops.OrderByDescending(p => p.fareTotalsum).ToList();
                    break;
                case "price_asc":
                    RightdeserializedStops = RightdeserializedStops.OrderBy(p => p.fareTotalsum).ToList();
                    break;
                case "deprt_desc":
                    RightdeserializedStops = RightdeserializedStops.OrderByDescending(d => d.designator.departure).ToList();
                    break;
                case "deprt_asc":
                    RightdeserializedStops = RightdeserializedStops.OrderBy(d => d.designator.departure).ToList();
                    break;
                case "arrive_desc":
                    RightdeserializedStops = RightdeserializedStops.OrderByDescending(d => d.designator.arrival).ToList();
                    break;
                case "arrive_asc":
                    RightdeserializedStops = RightdeserializedStops.OrderBy(d => d.designator.arrival).ToList();
                    break;
                case "duration_desc":
                    RightdeserializedStops = RightdeserializedStops.OrderByDescending(d => d.designator.formatTime).ToList();
                    break;
                case "duration_asc":
                    RightdeserializedStops = RightdeserializedStops.OrderBy(d => d.designator.formatTime).ToList();
                    break;
                default:
                    RightdeserializedStops = RightdeserializedStops.OrderBy(p => p.fareTotalsum).ToList();
                    break;

            }


            // vmobj.SimpleAvailibilityaAddResponcelist = filteredFlightsReturn;
            // vmobj.SimpleAvailibilityaAddResponcelistR = filteredFlightsRight;
            //return PartialView("_RTFlightResultsSortingPartialView", viewModelobject);

            List<SimpleAvailibilityaAddResponce> FilterStopData = LeftdeserializedStops;
            List<SimpleAvailibilityaAddResponce> FilterStopDataRight = RightdeserializedStops;
            //Onward Stops
            if (selectedIds != null && selectedIds.Count > 0)
            {
                FilterStopData = LeftdeserializedStops?.Where(x => selectedIds.Contains(x.stops)).ToList();
               
                foreach (int value in selectedIds)
                {
                    switch (value)
                    {
                        case 0:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();

                            break;
                        case 1:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();

                            break;
                        case 2:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();

                            break;
                        default:
                            FilterStopData = FilterStopData?.Where(x => selectedIds.Contains(x.stops)).ToList();
                            break;
                    }
                }
            }
            // Returns Stop
            if (selectedIdsRight != null && selectedIdsRight.Count > 0)
            {
                FilterStopDataRight = RightdeserializedStops?.Where(x => selectedIdsRight.Contains(x.stops)).ToList();
                foreach (int value in selectedIdsRight)
                {
                    switch (value)
                    {
                        case 0:

                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIdsRight.Contains(x.stops)).ToList();
                            break;
                        case 1:

                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIdsRight.Contains(x.stops)).ToList();
                            break;
                        case 2:

                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIdsRight.Contains(x.stops)).ToList();
                            break;
                        default:

                            FilterStopDataRight = FilterStopDataRight?.Where(x => selectedIdsRight.Contains(x.stops)).ToList();
                            break;
                    }
                }
            }

            if (RTFilterIdAirLine.Count > 0 && RTFilterIdAirLine.Count >= 0)
            {
                FilterStopData = FilterStopData?.Where(x => RTFilterIdAirLine.Contains(x.Airline.ToString())).ToList();
                FilterStopDataRight = FilterStopDataRight?.Where(x => RTFilterIdAirLine.Contains(x.Airline.ToString())).ToList();
                vmobj.SimpleAvailibilityaAddResponcelist = FilterStopData;
                vmobj.SimpleAvailibilityaAddResponcelistR = FilterStopDataRight;
                //return PartialView("_RTFlightResultsSortingPartialView", vmobj);
            }
            vmobj.SimpleAvailibilityaAddResponcelist = FilterStopData;
            vmobj.SimpleAvailibilityaAddResponcelistR = FilterStopDataRight;
            return PartialView("_RTFlightResultsSortingPartialView", vmobj);

        }
        //public IActionResult ReturnFlightViewFilter(List<string> departure, List<string> arrival, List<string> departureRight, List<string> arrivalRight)
        //{
        //    if (departure.Count > 0)
        //    {
        //        if (departure[0] == null)
        //        {
        //            departure = departure.Where(d => d != null).ToList();
        //            //departure = new List<string>();
        //        }
        //    }
        //    if (arrival.Count > 0)
        //    {
        //        if (arrival[0] == null)
        //        {
        //            //arrival = new List<string>();
        //            arrival = arrival.Where(d => d != null).ToList();
        //        }
        //    }
        //    if (departureRight.Count > 0)
        //    {
        //        if (departureRight[0] == null)
        //        {
        //            departureRight = departureRight.Where(d => d != null).ToList();
        //        }
        //    }
        //    if (arrivalRight.Count > 0)
        //    {
        //        if (arrivalRight[0] == null)
        //        {
        //            arrivalRight = arrivalRight.Where(d => d != null).ToList();
        //        }
        //    }

        //    string Leftshowpopupdata = HttpContext.Session.GetString("LeftReturnViewFlightView");
        //    string Rightshowpopupdata = HttpContext.Session.GetString("RightReturnFlightView");
        //    if (string.IsNullOrEmpty(Leftshowpopupdata))
        //    {
        //        return View("Error");
        //    }
        //    List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
        //    List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;

        //    ViewModel viewModelobject = new ViewModel();
        //    if (!string.IsNullOrEmpty(Leftshowpopupdata))
        //    {
        //        LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
        //    }

        //    if (!string.IsNullOrEmpty(Rightshowpopupdata))
        //    {
        //        RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
        //    }
        //    List<SimpleAvailibilityaAddResponce> filteredFlightsReturn = LeftdeserializedObjects;
        //    List<SimpleAvailibilityaAddResponce> filteredFlightsRight = RightdeserializedObjects;
        //    if (departure != null && departure.Count > 0)
        //    {
        //        filteredFlightsReturn = filteredFlightsReturn.Where(flight =>
        //            departure.Any(d =>
        //                (d.ToLower() == "before_6am" && flight.designator.departure.TimeOfDay < new TimeSpan(6, 0, 0)) ||
        //                (d.ToLower() == "6am_to_12pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(12, 0, 0)) ||
        //                (d.ToLower() == "12pm_to_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(18, 0, 0)) ||
        //                (d.ToLower() == "after_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(18, 0, 0))
        //            )).ToList();
        //    }
        //    else if (arrival != null && arrival.Count > 0)
        //    {
        //        filteredFlightsReturn = filteredFlightsReturn.Where(flight =>
        //            arrival.Any(a =>
        //                (a.ToLower() == "before_6am" && flight.designator.arrival.TimeOfDay < new TimeSpan(6, 0, 0)) ||
        //                (a.ToLower() == "6am_to_12pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(12, 0, 0)) ||
        //                (a.ToLower() == "12pm_to_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(18, 0, 0)) ||
        //                (a.ToLower() == "after_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(18, 0, 0))
        //            )).ToList();
        //    }
        //    else if (departureRight != null && departureRight.Count > 0)
        //    {
        //        filteredFlightsRight = filteredFlightsRight.Where(flight =>
        //            departureRight.Any(a =>
        //                (a.ToLower() == "before_6am" && flight.designator.departure.TimeOfDay < new TimeSpan(6, 0, 0)) ||
        //                (a.ToLower() == "6am_to_12pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(12, 0, 0)) ||
        //                (a.ToLower() == "12pm_to_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.departure.TimeOfDay < new TimeSpan(18, 0, 0)) ||
        //                (a.ToLower() == "after_6pm" && flight.designator.departure.TimeOfDay >= new TimeSpan(18, 0, 0))
        //            )).ToList();
        //    }
        //    else if (arrivalRight != null && arrivalRight.Count > 0)
        //    {
        //        filteredFlightsRight = filteredFlightsRight.Where(flight =>
        //            arrivalRight.Any(a =>
        //                (a.ToLower() == "before_6am" && flight.designator.arrival.TimeOfDay < new TimeSpan(6, 0, 0)) ||
        //                (a.ToLower() == "6am_to_12pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(6, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(12, 0, 0)) ||
        //                (a.ToLower() == "12pm_to_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(12, 0, 0) && flight.designator.arrival.TimeOfDay < new TimeSpan(18, 0, 0)) ||
        //                (a.ToLower() == "after_6pm" && flight.designator.arrival.TimeOfDay >= new TimeSpan(18, 0, 0))
        //            )).ToList();
        //    }
        //    else
        //    {

        //        if (!string.IsNullOrEmpty(Leftshowpopupdata))
        //        {
        //            LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
        //        }

        //        if (!string.IsNullOrEmpty(Rightshowpopupdata))
        //        {
        //            RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
        //        }

        //        viewModelobject.SimpleAvailibilityaAddResponcelist = LeftdeserializedObjects;
        //        viewModelobject.SimpleAvailibilityaAddResponcelistR = RightdeserializedObjects;

        //        string RTFlightEditData = HttpContext.Session.GetString("PassengerModelR");
        //        SimpleAvailabilityRequestModel simpleAvailabilityRequestModel = null;
        //        if (!string.IsNullOrEmpty(RTFlightEditData))
        //        {
        //            simpleAvailabilityRequestModel = JsonConvert.DeserializeObject<SimpleAvailabilityRequestModel>(RTFlightEditData);
        //        }
        //        viewModelobject.simpleAvailabilityRequestModelEdit = simpleAvailabilityRequestModel;
        //        return PartialView("_RTFlightResultsSortingPartialView", viewModelobject);
        //    }

        //    viewModelobject.SimpleAvailibilityaAddResponcelist = filteredFlightsReturn;
        //    viewModelobject.SimpleAvailibilityaAddResponcelistR = filteredFlightsRight;

        //    return PartialView("_RTFlightResultsSortingPartialView", viewModelobject);
        //}



    }
}
