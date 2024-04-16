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
                citycityLKO.cityName = "Lucknow";
            };
            Citydatalist.Add(citycityLKO);

            Citydata citycitySTV = new Citydata();
            {
                citycitySTV.cityCode = "STV";
                citycitySTV.cityName = "Surat";
            };
            Citydatalist.Add(citycitySTV);
            Citydata citycityGWL = new Citydata();
            {
                citycityGWL.cityCode = "GWL";
                citycityGWL.cityName = "Gwalior";
            };
            Citydatalist.Add(citycityGWL);
            Citydata citycityGOI = new Citydata();
            {
                citycityGOI.cityCode = "GOI";
                citycityGOI.cityName = "Goa";
            };
            Citydatalist.Add(citycityGOI);
            Citydata citycityJAI = new Citydata();
            {
                citycityJAI.cityCode = "JAI";
                citycityJAI.cityName = "Jaipur";
            };
            Citydatalist.Add(citycityJAI);
            Citydata citycityIXR = new Citydata();
            {
                citycityIXR.cityCode = "IXR";
                citycityIXR.cityName = "Ranchi";
            };
            Citydatalist.Add(citycityIXR);
            Citydata citycityBBI = new Citydata();
            {
                citycityBBI.cityCode = "BBI";
                citycityBBI.cityName = "Bhubaneswar";
            };
            Citydatalist.Add(citycityBBI);
            Citydata citycityHYD = new Citydata();
            {
                citycityHYD.cityCode = "HYD";
                citycityHYD.cityName = "Hyderabad";
            };
            Citydatalist.Add(citycityHYD);
            Citydata citycityPNQ = new Citydata();
            {
                citycityPNQ.cityCode = "PNQ";
                citycityPNQ.cityName = "Pune";
            };
            Citydatalist.Add(citycityPNQ);
            return Citydatalist;


        }
    }
}
