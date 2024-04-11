using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class SpicejetMealImageList
    {

        public string MealCode { get; set; }
        public string MealImage { get; set; }

        public static Hashtable GetAllmeal(Hashtable htSSr)
        {
            //List<SpicejetMealImageList> mealdatalist = new List<SpicejetMealImageList>();

            //var mealDataCollection = new[]
            //{

            //new { MealCode = "VGSW", MealImage = "Cucumber, tomato & cheese in multigrain bread (180 gms)"},
            //new { MealCode = "NVSW", MealImage = "Chicken Junglee in Marble Bread  (180 gms)"},
            //new { MealCode = "NVML", MealImage = "Masala Omelette with grilled chicken sausages & Hash Brown (190 gms)"},
            //new { MealCode = "VGML", MealImage = "Vegetable Upma with Sambar, Medu vada & Steamed Idli along with Coconut Chutney (205 gms)"},

            htSSr.Add("VGSW", "Cucumber, tomato & cheese in multigrain bread (180 gms)");
            htSSr.Add("NVSW", "Chicken Junglee in Marble Bread  (180 gms)");
            htSSr.Add("EB03", "3 kg");
            htSSr.Add("EB05", "5 kg");
            htSSr.Add("EB10", "10 kg");
            htSSr.Add("EB15", "15 kg");
            htSSr.Add("EB20", "20 kg");
            htSSr.Add("EB25", "25 kg");
            htSSr.Add("EB30", "30 kg");
            htSSr.Add("XBPA", "5 kg");
            htSSr.Add("XBPJ", "20 kg");
            htSSr.Add("XBPB", "10 kg");
            htSSr.Add("XBPC", "15 kg");
            htSSr.Add("XBPD", "30 kg");
            htSSr.Add("XBPE", "XBG");
            return htSSr;

        }

            //foreach (var data in mealDataCollection)
            //{
            //    SpicejetMealImageList mealItem = new SpicejetMealImageList
            //    {
            //        MealCode = data.MealCode,
            //        MealImage = data.MealImage
            //    };

            //    mealdatalist.Add(mealItem);
            //}

         
        
    }
}
