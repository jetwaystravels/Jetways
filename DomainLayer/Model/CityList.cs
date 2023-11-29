using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Citydata
    {
        public string cityCode { get; set; }
        public string cityName { get; set; }
       
        public static List<Citydata> GetAllcity()
        {
            List<Citydata> Citydatalist = new List<Citydata>();

            Citydata cityDEL = new Citydata();
            {
                cityDEL.cityCode = "DEL";
                cityDEL.cityName = "New DELHI";
            };
            Citydatalist.Add(cityDEL);

            Citydata citycityBOM = new Citydata();
            {
                citycityBOM.cityCode = "BOM";
                citycityBOM.cityName = "Mumbai";
            };
            Citydatalist.Add(citycityBOM);
           

            return Citydatalist;
        }
    }
}
