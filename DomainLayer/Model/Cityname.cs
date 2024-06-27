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
            new { citycode = "AYJ", cityname = "Ayodhya", airportname = "Maharishi Valmiki International Airport "},
            new { citycode = "DEL", cityname = "New Delhi", airportname = "Indira Gandhi International Airport "},
            new { citycode = "BOM", cityname = "Mumbai", airportname = "Chhatrapati Shivaji Maharaj International Airport "},
            new { citycode = "BLR", cityname = "Bangalore", airportname = "Kempegowda International Airport "},
            new { citycode = "CCU", cityname = "Kolkata", airportname = "Netaji Subhas Chandra Bose International Airport "},
            new { citycode = "PNQ", cityname = "Pune", airportname = "Pune International Airport "},
            new { citycode = "UDR", cityname = "Udaipur", airportname = "Maharana Pratap Airport "},
            new { citycode = "LUH", cityname = "Ludhiana", airportname = "Sahnewal Airport "},
            new { citycode = "PYB", cityname = "Jeypore", airportname = "Jeypore Airport "},
            new { citycode = "DMU", cityname = "Dimapur", airportname = "Dimapur Airport "},
            new { citycode = "AJL", cityname = "Aizawl", airportname = "Lengpui Airport "},
            new { citycode = "SHL", cityname = "Shillong", airportname = "Shillong Airport "},
            new { citycode = "AKD", cityname = "Akola", airportname = "Akola Airport "},
            new { citycode = "JLG", cityname = "Jalgaon", airportname = "Jalgaon Airport "},
            new { citycode = "SSE", cityname = "Sholapur", airportname = "Sholapur Airport "},
            new { citycode = "IXU", cityname = "Aurangabad", airportname = "Aurangabad Airport "},
            new { citycode = "KLH", cityname = "Kolhapur", airportname = "Kolhapur Airport "},
            new { citycode = "NDC", cityname = "Nanded", airportname = "Shri Guru Gobind Singh Ji Airport "},
            new { citycode = "ISK", cityname = "Nashik", airportname = "Ozar Airport "},
            new { citycode = "SAG", cityname = "Shirdi", airportname = "Shirdi Airport "},
            new { citycode = "IDR", cityname = "Indore", airportname = "Devi Ahilyabai Holkar International Airport "},
            new { citycode = "BHO", cityname = "Bhopal", airportname = "Raja Bhoj Airport "},
            new { citycode = "JLR", cityname = "Jabalpur", airportname = "Jabalpur Airport "},
            new { citycode = "TNI", cityname = "Satna", airportname = "Satna Airport "},
            new { citycode = "HJR", cityname = "Khajuraho", airportname = "Khajurah Airport "},
            new { citycode = "JAI", cityname = "Jaipur", airportname = "Jaipur International Airport"},
            new { citycode = "LKO", cityname = "Lucknow", airportname = "Amausi Airport "},
            new { citycode = "STV", cityname = "Surat", airportname = " Surat International Airport "},
            new { citycode = "GWL", cityname = "Gwalior", airportname = "Gwalior Airport "},
            new { citycode = "GOI", cityname = "Goa", airportname = "Goa Dabolim International Airport "},
            new { citycode = "GOX", cityname = "Goa AirPort", airportname = "North Goa -MOPA Airport "},
            new { citycode = "IXR", cityname = "Ranchi", airportname = "Birsa Munda Airport "},
            new { citycode = "BBI", cityname = "Bhubaneswar", airportname = " Birsa Munda Airport"},
            new { citycode = "HYD", cityname = "Hyderabad", airportname = "Rajiv Gandhi International Airport"},
            new { citycode = "COK", cityname = "Kochi", airportname = "Cochin International Airport Limited"},
            new { citycode = "BDQ", cityname = "Vadodara", airportname = "Vadodara Airport"},
            new { citycode = "NAG", cityname = "Nagpur", airportname = "Dr. Babasaheb Ambedkar International Airport"},
            new { citycode = "IXC", cityname = "Chandigarh", airportname = "Chandigarh Airport"},
            new { citycode = "SXR", cityname = "Srinagar", airportname = "Srinagar International Airport"},
            new { citycode = "DED", cityname = "Dehradun", airportname = "Jolly Grant Airport"},
            new { citycode = "MAA", cityname = "Chennai", airportname = "Chennai International Airport"},
            new { citycode = "CJB", cityname = "Coimbatore", airportname = "Coimbatore Airport"},
            new { citycode = "KNU", cityname = "Kanpur", airportname = "Kanpur Airport"},
            new { citycode = "IXG", cityname = "Belgaum", airportname = "Belgaum Airport"},
            new { citycode = "GOP", cityname = "Gorakhpur", airportname = "Gorakhpur Airport"},
            new { citycode = "VNS", cityname = "Varanasi", airportname = "Varanasi Airport"},
            new { citycode = "VTZ", cityname = "Visakhapatnam", airportname = "Visakhapatnam International Airport"},
            new { citycode = "GAU", cityname = "Guwahati", airportname = "Lokpriya Gopinath Bordoloi International Airport"},
            new { citycode = "GAY", cityname = "Gaya", airportname = "Gaya International Airport"},
            new { citycode = "TRV", cityname = "Thiruvananthapuram", airportname = "Trivandrum International Airport"},
            new { citycode = "IXB", cityname = "Bagdogra", airportname = "Bagdogra Airport"},
            new { citycode = "AMD", cityname = "Ahmedabad", airportname = "Sardar Vallabhbhai Patel International Airport India"},
            new { citycode = "RPR", cityname = "Raipur", airportname = "Raipur Airport"},
            new { citycode = "IXD", cityname = "Prayagraj", airportname = "Allahabad Airport"},
            new { citycode = "IMF", cityname = "Imphal", airportname = "Tulihal International Airport"},
            new { citycode = "IXJ", cityname = "Jammu", airportname = "Jammu Airport"},
            new { citycode = "CCJ", cityname = "Kozhikode", airportname = "Calicut Airport"},
            new { citycode = "IXE", cityname = "Mangalore", airportname = "Mangalore International Airport"},
            new { citycode = "PAT", cityname = "Patna", airportname = "Lok Nayak Jayaprakash Airport"},
            new { citycode = "CNN", cityname = "Kannur", airportname = "Kannur International Airport"}
    
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
