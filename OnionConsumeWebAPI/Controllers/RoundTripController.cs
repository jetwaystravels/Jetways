using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
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

            List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist = new List<SimpleAvailibilityaAddResponce>();
            SimpleAvailibilityaAddResponce SimpleAvailibilityaAddResponceobj = new SimpleAvailibilityaAddResponce();

            List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelistR = new List<SimpleAvailibilityaAddResponce>();
            SimpleAvailibilityaAddResponce _SimpleAvailibilityaAddResponceobjR = new SimpleAvailibilityaAddResponce();
            ViewModel vmobj = new ViewModel();

            SimpleAvailibilityaAddResponcelist = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(TempData["Mymodel"].ToString());
            SimpleAvailibilityaAddResponcelistR = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(TempData["MymodelR"].ToString());
            vmobj.SimpleAvailibilityaAddResponcelistR = SimpleAvailibilityaAddResponcelistR;
            vmobj.SimpleAvailibilityaAddResponcelist = SimpleAvailibilityaAddResponcelist;


            HttpContext.Session.SetString("FlightDetail", JsonConvert.SerializeObject(vmobj));

            return View(vmobj);
        }
    }
}
