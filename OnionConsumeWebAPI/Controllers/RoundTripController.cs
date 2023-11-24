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
            List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist = new List<SimpleAvailibilityaAddResponce>();
            SimpleAvailibilityaAddResponce SimpleAvailibilityaAddResponceobj = new SimpleAvailibilityaAddResponce();
            SimpleAvailibilityaAddResponcelist = JsonConvert.DeserializeObject<List<SimpleAvailibilityaAddResponce>>(TempData["Mymodel"].ToString());       
            HttpContext.Session.SetString("FlightDetail", JsonConvert.SerializeObject(SimpleAvailibilityaAddResponcelist));

            return View(SimpleAvailibilityaAddResponcelist.AsEnumerable());
        }
    }
}
