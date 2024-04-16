using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Citynamelist
    {
        public string citycode { get; set; }
        public string cityname { get; set; }
        public string airportname { get; set; } // New property

        public static List<Citynamelist> GetAllCityData()
        {
            List<Citynamelist> cityDataList = new List<Citynamelist>();

            var cityDataCollection = new[]
            {
            new { citycode = "DEL", cityname = "New Delhi", airportname = "Indira Gandhi International Airport India"},
            new { citycode = "BOM", cityname = "Mumbai", airportname = "Chhatrapati Shivaji Maharaj International Airport India"},
            new { citycode = "BLR", cityname = "Bangalore", airportname = "Kempegowda International Airport India"},
            new { citycode = "CCU", cityname = "Kolkata", airportname = "Netaji Subhas Chandra Bose International Airport India"},
            new { citycode = "PNQ", cityname = "Pune", airportname = "Pune International Airport India"},
            new { citycode = "LUH", cityname = "Ludhiana", airportname = "Sahnewal Airport India"},
            new { citycode = "PYB", cityname = "Jeypore", airportname = "Jeypore Airport India"},
            new { citycode = "DMU", cityname = "Dimapur", airportname = "Dimapur Airport India"},
            new { citycode = "AJL", cityname = "Aizawl", airportname = "Lengpui Airport India"},
            new { citycode = "SHL", cityname = "Shillong", airportname = "Shillong Airport India"},
            new { citycode = "AKD", cityname = "Akola", airportname = "Akola Airport India"},
            new { citycode = "JLG", cityname = "Jalgaon", airportname = "Jalgaon Airport India"},
            new { citycode = "SSE", cityname = "Sholapur", airportname = "Sholapur Airport India"},
            new { citycode = "IXU", cityname = "Aurangabad", airportname = "Aurangabad Airport India"},
            new { citycode = "KLH", cityname = "Kolhapur", airportname = "Kolhapur Airport India"},
            new { citycode = "NDC", cityname = "Nanded", airportname = "Shri Guru Gobind Singh Ji Airport India"},
            new { citycode = "ISK", cityname = "Nashik", airportname = "Ozar Airport India"},
            new { citycode = "SAG", cityname = "Shirdi", airportname = "Shirdi Airport India"},
            new { citycode = "IDR", cityname = "Indore", airportname = "Devi Ahilyabai Holkar International Airport India"},
            new { citycode = "GWL", cityname = "Gwalior", airportname = "Gwalior Airport India"},
            new { citycode = "BHO", cityname = "Bhopal", airportname = "Raja Bhoj Airport India"},
            new { citycode = "JLR", cityname = "Jabalpur", airportname = "Jabalpur Airport India"},
            new { citycode = "TNI", cityname = "Satna", airportname = "Satna Airport India"},
            new { citycode = "HJR", cityname = "Khajuraho", airportname = "Khajurah Airport India"},
            new { citycode = "JAI", cityname = "Jaipur", airportname = "Jaipur International Airport"},
            new { citycode = "LKO", cityname = "Lucknow", airportname = "Amausi Airport India"},
            new { citycode = "STV", cityname = "Surat", airportname = " Surat International Airport India"},
            new { citycode = "GWL", cityname = "Gwalior", airportname = "Gwalior Airport India"},
            new { citycode = "GOI", cityname = "Goa", airportname = "Goa Dabolim International Airport India"},
            new { citycode = "IXR", cityname = "Ranchi", airportname = "Surat International Airport India"},
            new { citycode = "BBI", cityname = "Bhubaneswar", airportname = " Birsa Munda Airport India"},
            new { citycode = "HYD", cityname = "Hyderabad", airportname = "Rajiv Gandhi International Airport India"}
            //new { citycode = "STV", cityname = "Surat", airportname = "Surat International Airport India"},
          
    
            // Add more data as needed...
        };

            foreach (var data in cityDataCollection)
            {
                Citynamelist cityItem = new Citynamelist
                {
                    citycode = data.citycode,
                    cityname = data.cityname,
                    airportname = data.airportname
                };

                cityDataList.Add(cityItem);
            }

            return cityDataList;
        }
    }
}
