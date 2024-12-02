using DomainLayer.Model;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using OnionConsumeWebAPI.ApiService;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace OnionConsumeWebAPI.Models
{
    public class MongoHelper
    {

         public string GetRequestCacheKey(SimpleAvailabilityRequestModel FlightSCriteria)
        {
            StringBuilder key = new StringBuilder();
            //string key = string.Empty;

            //Adult
            if(FlightSCriteria.passengercount!=null)
            {
                key.Append(FlightSCriteria.passengercount.adultcount);
                key.Append(FlightSCriteria.passengercount.childcount);
                key.Append(FlightSCriteria.passengercount.infantcount);
            }
            else
            {
                key.Append(FlightSCriteria.adultcount);
                key.Append(FlightSCriteria.childcount);
                key.Append(FlightSCriteria.infantcount);
            }

 
          
            //if (FlightSCriteria.ChildrenAges != null)
            //{
            //    if (FlightSCriteria.Children > 0)
            //        foreach (string str in FlightSCriteria.ChildrenAges)
            //            key.Append(str);
            //}

            key.Append(FlightSCriteria.trip.ToString());
          
                 key.Append(Convert.ToDateTime(FlightSCriteria.beginDate).ToString("ddMMyyyy"));  //
            key.Append(Convert.ToDateTime(FlightSCriteria.endDate).ToString("ddMMyyyy"));  //
            key.Append(FlightSCriteria.origin.ToString().Split("-")[1].Trim());
                key.Append(FlightSCriteria.destination.ToString().Split("-")[1].Trim());
               // key.Append(FlightSCriteria.DirectFlights.ToString());
            //    if (!string.IsNullOrEmpty(FlightSCriteria.Carrier))
            //        key.Append(FlightSCriteria.Carrier.ToString());
            
            //if (!string.IsNullOrEmpty(FlightSCriteria.CabinClass.ToString()))
            //    key.Append(FlightSCriteria.CabinClass.ToString());
            //key.Append(currcode);
          

            //key = key.TrimEnd('~');
            return key.ToString();
        }

        public string Zip(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return text;
                else
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
                    MemoryStream ms = new MemoryStream();
                    using (System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true))
                    {
                        zip.Write(buffer, 0, buffer.Length);
                    }

                    ms.Position = 0;
                    MemoryStream outStream = new MemoryStream();

                    byte[] compressed = new byte[ms.Length];
                    ms.Read(compressed, 0, compressed.Length);

                    byte[] gzBuffer = new byte[compressed.Length + 4];
                    System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                    System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                    return Convert.ToBase64String(gzBuffer);
                }
            }
            catch
            {
                return text;
            }
        }

        public string UnZip(string compressedText)
        {
            try
            {
                if (string.IsNullOrEmpty(compressedText)) return compressedText;
                else
                {
                    byte[] gzBuffer = Convert.FromBase64String(compressedText);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                        ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
                        byte[] buffer = new byte[msgLength];
                        ms.Position = 0;
                        using (System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress))
                        {
                            zip.Read(buffer, 0, buffer.Length);
                        }
                        return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
                return compressedText;
            }
        }

         public string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 1000000000;
            return String.Format("{0:D8}", random);
        }

        public string GetIp()
        {
            string ip = "192.168.1.99";
            return ip;
        }




    }
}
