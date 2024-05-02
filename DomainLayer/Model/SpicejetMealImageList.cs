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
            htSSr.Add("XBPA", "+5 Kg Xcess Baggage");
            htSSr.Add("XBPJ", "+20 Kg Xcess Baggage");
            htSSr.Add("XBPB", "+10 Kg Xcess Baggage");
            htSSr.Add("XBPC", "+15 Kg Xcess Baggage");
            htSSr.Add("XBPD", "+30 Kg Xcess Baggage");
            htSSr.Add("XBPE", "XBG");
            htSSr.Add("VLML", "Awadhi Chicken Biryani with Mirch Salan(Chargable meal code");
            htSSr.Add("VGPI", "Awadhi Chicken Biryani with Mirch Salan(Free meal code");
            htSSr.Add("VGNI", "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)(Chargable meal code");
            htSSr.Add("VGAN", "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)(Free meal code");
            htSSr.Add("VCSW", "Chicken Junglee Sandwich(Chargable meal code");
            htSSr.Add("VCNX", "Chicken Junglee Sandwich(Free meal code");
            htSSr.Add("VCN2", "Chicken Nuggets with Fried Potatoes(Chargable meal code");
            htSSr.Add("VCN1", "Chicken Nuggets with Fried Potatoes(Free meal code");
            htSSr.Add("UMNR", "Herb Grilled Fish Fillet(Chargable meal code");
            htSSr.Add("TCSW", "Herb Grilled Fish Fillet(Free  meal code");
            htSSr.Add("STUD", "Herb Roast Vegetable Roll(Chargable meal code");
            htSSr.Add("SRCT", "Herb Roast Vegetable Roll(Free meal code");
            htSSr.Add("SPEQ", "Hyderabadi Mutton Biryani with Mirch Salan(Chargable meal code");
            htSSr.Add("PROT", "Hyderabadi Mutton Biryani with Mirch Salan (Free meal code");
            htSSr.Add("PRI5", "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(Chargable meal code");
            htSSr.Add("PRI4", "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(FREE meal code");
            htSSr.Add("PRI3", "Matar-Paneer-Bhurji-with-Aloo-Paratha(Chargable meal code");
            htSSr.Add("PRI2", "Matar-Paneer-Bhurji-with-Aloo-Paratha(Free meal code");
            htSSr.Add("PRI1", "Mini Idlis, Medu Vada and Upma(Chargable meal code");
            htSSr.Add("NUSW", "Mini Idlis, Medu Vada and Upma(Free meal code");
            htSSr.Add("LCVG", "Murg Tikka Masala with Lachha Paratha(Chargable meal code");
            htSSr.Add("LBG", "Murg Tikka Masala with Lachha Paratha(Free  meal code");
            htSSr.Add("JNML", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao(Chargable meal code");
            htSSr.Add("IXBC", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao (Free meal code");
            htSSr.Add("IXBB", "Seasonal Fresh Fruits Platter (Chargable meal code");
            htSSr.Add("IXBA", "Seasonal Fresh Fruits Platter (Free meal code");
            htSSr.Add("IGPR", "Shôndesh Tiramisù(Chargable meal code");
            htSSr.Add("IFNR", "Shôndesh Tiramisù(Free meal code");
            htSSr.Add("GFNV", "Vegan Moilee Curry with Coconut Rice (1)(Chargable meal code");
            htSSr.Add("FFWD", "Vegan Moilee Curry with Coconut Rice (1)(Chargable meal code");
            htSSr.Add("DBVG", "Vegetable Manchurian with Fried Rice (Chargable meal code");
            htSSr.Add("DBNV", "Vegetable Manchurian with Fried Rice (Free meal code");

            htSSr.Add("CPML", "Mini Idlis, Medu Vada and Upma(Free meal code");
            htSSr.Add("CJSW", "Murg Tikka Masala with Lachha Paratha(Chargable meal code");
            htSSr.Add("CHVM", "Murg Tikka Masala with Lachha Paratha(Free  meal code");
            htSSr.Add("CHTI", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao(Chargable meal code");
            htSSr.Add("CHNM", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao (Free meal code");
            htSSr.Add("CHKI", "Seasonal Fresh Fruits Platter (Chargable meal code");
            htSSr.Add("AGSW", "Seasonal Fresh Fruits Platter (Free meal code");
            htSSr.Add("ABHF", "Shôndesh Tiramisù(Chargable meal code");
            
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
