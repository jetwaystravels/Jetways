using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Methodshit
    {
        public static string HttpPost(string uri, string parameters, string _userName,string _password)
        {

            WebRequest mywebrequest = WebRequest.Create(uri);
            mywebrequest.Method = "POST";
            mywebrequest.ContentType = "text/xml;charset=UTF-8";
            // Credentials
            //byte[] authBytes = Encoding.UTF8.GetBytes(("Universal API/uAPI5098257106-beb65aec" + ":" + "Q!f5-d7A3D").ToCharArray());
            byte[] authBytes = Encoding.UTF8.GetBytes((_userName + ":" + _password).ToCharArray());
            mywebrequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(authBytes);
            mywebrequest.Headers["Accept-Encoding"] = "gzip";
            mywebrequest.Headers["Connection"] = "keep-alive";
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            byte[] mybytes = Encoding.ASCII.GetBytes(parameters);
            Stream os = null;
            try
            { // send the Post
                mywebrequest.ContentLength = mybytes.Length;    //Count bytes to send
                os = mywebrequest.GetRequestStream();
                os.Write(mybytes, 0, mybytes.Length); //Send it
            }
            catch (WebException ex)
            {
                //Utility.Utility.CreateExceptionLogFile("TravelPort_HttpPost()1_" + RQ.SearchDetails[0].Origin + "_TO_" + RQ.SearchDetails[0].Destination + "_" + CompanyName + "_", ex.Message +"--"+ex.StackTrace+ "--------" + parameters);
                return "Oops What happened: " + ex.Message;
            }
            finally
            {
                if (os != null)
                {
                    os.Close();
                }
            }
            try
            { // get the response
                WebResponse mywebResponse = mywebrequest.GetResponse();
                if (mywebResponse == null)
                { return null; }
                if (mywebResponse.Headers["Content-Encoding"] != null && mywebResponse.Headers["Content-Encoding"].Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    Stream dataStream = mywebResponse.GetResponseStream();
                    GZipStream compressor = new GZipStream(dataStream, CompressionMode.Decompress);
                    StreamReader sr = new StreamReader(compressor);
                    return sr.ReadToEnd().Trim();
                }
                else
                {
                    return new StreamReader(mywebResponse.GetResponseStream()).ReadToEnd();
                }

            }
            catch (ThreadAbortException thAbortEx_)
            {
                return "Oops What happened: " + thAbortEx_.Message;
            }
            catch (WebException ex)
            {
                return "Oops What happened: " + ex.Message;
            }
        }
    }
}
