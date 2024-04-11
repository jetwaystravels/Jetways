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
                cityDEL.cityName = "New Delhi";
            };
            Citydatalist.Add(cityDEL);

            Citydata citycityBOM = new Citydata();
            {
                citycityBOM.cityCode = "BOM";
                citycityBOM.cityName = "Mumbai";
            };
            Citydatalist.Add(citycityBOM);

            Citydata citycityBLR = new Citydata();
            {
                citycityBLR.cityCode = "BLR";
                citycityBLR.cityName = "Bangalore";
            };
            Citydatalist.Add(citycityBLR);

            Citydata citycityLKO = new Citydata();
            {
                citycityLKO.cityCode = "LKO";
                citycityLKO.cityName = "LKO";
            };
            Citydatalist.Add(citycityLKO);

            Citydata citycitySTV = new Citydata();
            {
                citycitySTV.cityCode = "STV";
                citycitySTV.cityName = "STV";
            };
            Citydatalist.Add(citycitySTV);
            return Citydatalist;
        }
    }
}
