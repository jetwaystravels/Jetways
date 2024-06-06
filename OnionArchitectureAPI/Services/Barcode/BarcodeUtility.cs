using System.Drawing;
using ZXing.Common;
using ZXing;
using ZXing.Windows.Compatibility;
using ZXing.QrCode.Internal;

namespace OnionArchitectureAPI.Services.Barcode
{
    public class BarcodeUtility
    {
        //private readonly ILogger<BarcodeUtility> _logger;

        public string BarcodereadUtility(string BarcodeString)
        {
            string barcodeImageString = string.Empty; // Declare the variable outside the using block

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.PDF_417,
                Options = new EncodingOptions
                {
                    Width = 240,
                    Height = 60,
                    Margin = 0
                    
                }
            };

            Bitmap barcodeBitmap = barcodeWriter.Write(BarcodeString);
            using (MemoryStream stream = new MemoryStream())
            {
                barcodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = stream.ToArray();
                Convert.ToBase64String(bytes);
                barcodedata barcodedata = new barcodedata();
                barcodedata.barcodeimage = "data:image/png;base64," + Convert.ToBase64String(bytes);
              //  barcodedata.barcodestring = BarcodeString;

                barcodeImageString = barcodedata.barcodeimage; // Assign the barcode image string to the variable
            }

            return barcodeImageString; // Return the barcode image string
        }


        public class barcodedata
        {
            public string barcodeimage { get; set; }
            public string barcodestring { get; set; }
           


        }


    }
}
