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

            LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
            RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);

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
        public IActionResult RTFlightView(List<int> selectedIds)
        {
            if (selectedIds.Count > 0 && selectedIds.Count >= 0)
            {

                string LeftshowpopupdataStops = HttpContext.Session.GetString("LeftReturnViewFlightView");
                string RightshowpopupdataStops = HttpContext.Session.GetString("RightReturnFlightView");

                ViewModel vmobj = new ViewModel();
                List<SimpleAvailibilityaAddResponce> RightdeserializedStops = null;
                List<SimpleAvailibilityaAddResponce> LeftdeserializedStops = null;


                LeftdeserializedStops = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(LeftshowpopupdataStops);
                RightdeserializedStops = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(RightshowpopupdataStops);


                var FilterStopData = LeftdeserializedStops.Where(x => selectedIds.Contains(x.stops)).ToList();

                var FilterStopDataRight = RightdeserializedStops.Where(x => selectedIds.Contains(x.stops)).ToList();

                //var FilterStopData = LeftdeserializedStops.Where(x=>x.stops== selectedIds).ToList();
                //var FilterStopDataRight = RightdeserializedStops.Where(x => x.stops == selectedIds).ToList();

                vmobj.SimpleAvailibilityaAddResponcelist = FilterStopData;
                vmobj.SimpleAvailibilityaAddResponcelistR = FilterStopDataRight;
                return View(vmobj);
            }
            else
            {
                ViewModel vmobj = new ViewModel();
                string Leftshowpopupdata = HttpContext.Session.GetString("LeftReturnViewFlightView");
                string Rightshowpopupdata = HttpContext.Session.GetString("RightReturnFlightView");

                List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
                List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;

                LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
                RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);

                vmobj.SimpleAvailibilityaAddResponcelist = LeftdeserializedObjects;
                vmobj.SimpleAvailibilityaAddResponcelistR = RightdeserializedObjects;

                // HttpContext.Session.SetString("FlightDetail", JsonConvert.SerializeObject(vmobj));
                return View(vmobj);

            }
        }


    }
}
