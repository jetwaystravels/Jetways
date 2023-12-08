using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using static DomainLayer.Model.SeatMapResponceModel;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{
    public class RoundTripController : Controller
    {
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
        string passengerkey12 = string.Empty;
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
            string Leftshowpopupdata = HttpContext.Session.GetString("ReturnViewFlightView");
            string Rightshowpopupdata = HttpContext.Session.GetString("LeftReturnFlightView");
            List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
            List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;
            LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
            RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
            ViewModel vmobj = new ViewModel();

            vmobj.SimpleAvailibilityaAddResponcelistR = LeftdeserializedObjects;
            vmobj.SimpleAvailibilityaAddResponcelist = RightdeserializedObjects;
            HttpContext.Session.SetString("FlightDetail", JsonConvert.SerializeObject(vmobj));

            return View(vmobj);
        }

        public IActionResult TestingDataView(int i, int j)
        {
            string Leftshowpopupdata = HttpContext.Session.GetString("ReturnViewFlightView");
            string Rightshowpopupdata = HttpContext.Session.GetString("LeftReturnFlightView");
            List<SimpleAvailibilityaAddResponce> LeftdeserializedObjects = null;
            List<SimpleAvailibilityaAddResponce> RightdeserializedObjects = null;
            LeftdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Leftshowpopupdata);
            RightdeserializedObjects = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(Rightshowpopupdata);
            var filteredData = LeftdeserializedObjects.Where(x => x.uniqueId == j).ToList();
            ViewModel vmobject = new ViewModel();
            vmobject.SimpleAvailibilityaAddResponcelist = filteredData;
            return View(vmobject);
        }


    }
}
