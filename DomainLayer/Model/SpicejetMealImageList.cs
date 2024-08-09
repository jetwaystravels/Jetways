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

            //htSSr.Add("VGSW", "Cucumber, tomato & cheese in multigrain bread (180 gms)");
            //htSSr.Add("NVSW", "Chicken Junglee in Marble Bread  (180 gms)");
            htSSr.Add("EB03", "3 kg");
            htSSr.Add("EB05", "5 kg");
            htSSr.Add("EB10", "10 kg");
            htSSr.Add("EB15", "15 kg");
            htSSr.Add("EB20", "20 kg");
            htSSr.Add("EB25", "25 kg");
            htSSr.Add("EB30", "30 kg");
            htSSr.Add("XBPA", "5 Kg");
            htSSr.Add("XBPJ", "20 Kg");
            htSSr.Add("XBPB", "10 Kg");
            htSSr.Add("XBPC", "15 Kg");
            htSSr.Add("XBPD", "30 Kg");
            htSSr.Add("XBPE", "3 Kg");
            //htSSr.Add("VGPI", "Awadhi Chicken Biryani with Mirch Salan(Free meal code");
            //htSSr.Add("VGNI", "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)(Chargable meal code");
            //htSSr.Add("VCSW", "Chicken Junglee Sandwich(Chargable meal code");
            //htSSr.Add("VCNX", "Chicken Junglee Sandwich(Free meal code");
            //htSSr.Add("VCN2", "Chicken Nuggets with Fried Potatoes(Chargable meal code");
            //htSSr.Add("VCN1", "Chicken Nuggets with Fried Potatoes(Free meal code");
            //htSSr.Add("UMNR", "Herb Grilled Fish Fillet(Chargable meal code");
            //htSSr.Add("STUD", "Herb Roast Vegetable Roll(Chargable meal code");
            //htSSr.Add("SRCT", "Herb Roast Vegetable Roll(Free meal code");
            //htSSr.Add("SPEQ", "Hyderabadi Mutton Biryani with Mirch Salan(Chargable meal code");
            //htSSr.Add("PROT", "Hyderabadi Mutton Biryani with Mirch Salan (Free meal code");
            //htSSr.Add("PRI5", "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(Chargable meal code");
            //htSSr.Add("PRI4", "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(FREE meal code");
            //htSSr.Add("PRI3", "Matar-Paneer-Bhurji-with-Aloo-Paratha(Chargable meal code");
            //htSSr.Add("PRI2", "Matar-Paneer-Bhurji-with-Aloo-Paratha(Free meal code");
            //htSSr.Add("PRI1", "Mini Idlis, Medu Vada and Upma(Chargable meal code");
            //htSSr.Add("NUSW", "Mini Idlis, Medu Vada and Upma(Free meal code");
            //htSSr.Add("LBG", "Murg Tikka Masala with Lachha Paratha(Free  meal code");
            //htSSr.Add("IXBC", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao (Free meal code");
            //htSSr.Add("IXBB", "Seasonal Fresh Fruits Platter (Chargable meal code");
            //htSSr.Add("IXBA", "Seasonal Fresh Fruits Platter (Free meal code");
            //htSSr.Add("IGPR", "Shôndesh Tiramisù(Chargable meal code");
            //htSSr.Add("IFNR", "Shôndesh Tiramisù(Free meal code");
            //htSSr.Add("FFWD", "Vegan Moilee Curry with Coconut Rice (1)(Chargable meal code");
            //htSSr.Add("CPML", "Mini Idlis, Medu Vada and Upma(Free meal code");
            //htSSr.Add("CHTI", "Paneer Makhani with Jeera Aloo _ Vegetable Pulao(Chargable meal code");
            //htSSr.Add("CHKI", "Seasonal Fresh Fruits Platter (Chargable meal code");
            //htSSr.Add("AGSW", "Seasonal Fresh Fruits Platter (Free meal code");
            //htSSr.Add("ABHF", "Shôndesh Tiramisù(Chargable meal code");

            //Indigo

            htSSr.Add("CJSW", "Chicken Junglee Sandwich");
            htSSr.Add("FFWD", "Fast Forward");
            htSSr.Add("JNML", "Jain Meal / Tomato Cucumber");
            htSSr.Add("VLML", "Veg Lactose Meal /Paneer Bhatti");
            htSSr.Add("VGAN", "Vegan Meal  / 2 Dips with Baked Pita");
            htSSr.Add("CHVM", "KIDDIE DELIGHT -Veg");
            htSSr.Add("CHNM", "KIDDIE DELIGHT  Non -Veg");
            htSSr.Add("CHNS", "Sesame Chicken");
            htSSr.Add("TCSW", "Tomato Cucumber Cheese Lettuce Sandwich");
            htSSr.Add("BEER", "Beer - 330 ML (2 Cans)");
            htSSr.Add("WHSK", "Whiskey – 50 ML (2 Miniatures)");
            htSSr.Add("CNWT", "CASHEW (SALTED) 50 GMS");
            htSSr.Add("CCWT", "Unibic Chocolate Chips Cookies-75");
            htSSr.Add("CTSW", "Chicken Tikka Sandwich");
            htSSr.Add("PTSW", "Paneer Tikka Sandwich");
            htSSr.Add("VGTR", "Veg Trio Sandwich");
            htSSr.Add("NVTR", "Non veg trio sandwich");
            htSSr.Add("CKKU", "Chicken Keema with Kulcha");
            htSSr.Add("PITA", "2 Dips with Baked Pita");
            htSSr.Add("CHCT", "Chicken cucumber tomato sandwich");
            htSSr.Add("BHPS", "Bhatti Paneer Salad");
            htSSr.Add("POHA", "Poha Combo");
            htSSr.Add("UPMA", "Rava Upma Combo");
            htSSr.Add("ZCHK", "Nissin Zesty Chicken Keema Noodle");
            htSSr.Add("COMI", "Cornflakes with Milk");
            htSSr.Add("MUYO", "Muesli with yogurt ");
            htSSr.Add("VBIR", "Veg Biryani Combo");
            htSSr.Add("PBMR", "Paneer Butter masala");
            htSSr.Add("AOAT", "Apple Oat Meal");
            htSSr.Add("SAKD", "Sabudana Khichdi ");
            htSSr.Add("CHCR", "Chicken Curry Rice");
            htSSr.Add("NLVG", "Non Lactose meal / 2 Dips with Baked Pita");
            htSSr.Add("LCVG", "Low Calorie Veg /Paneer Bhatti");
            htSSr.Add("VPAV", "Vada Pav");
            htSSr.Add("SAVA", "Sambar Vada");
            htSSr.Add("CHBR", "Chicken BiryanI");
            htSSr.Add("DBNV", "Diabitic N.Veg / Chicken Supreme Salad");
            htSSr.Add("DBVG", "Diabetic Veg Meal / Paneer Bhatt");
            htSSr.Add("GFNV", "Gluten free / Chicken Supreme Salad");

            return htSSr;

        }
        public static Hashtable GetAllmealSG(Hashtable htSSr)
        {
            //List<SpicejetMealImageList> mealdatalist = new List<SpicejetMealImageList>();

            //var mealDataCollection = new[]
            //{

            //new { MealCode = "VGSW", MealImage = "Cucumber, tomato & cheese in multigrain bread (180 gms)"},
            //new { MealCode = "NVSW", MealImage = "Chicken Junglee in Marble Bread  (180 gms)"},
            //new { MealCode = "NVML", MealImage = "Masala Omelette with grilled chicken sausages & Hash Brown (190 gms)"},
            //new { MealCode = "VGML", MealImage = "Vegetable Upma with Sambar, Medu vada & Steamed Idli along with Coconut Chutney (205 gms)"},

            //htSSr.Add("VGSW", "Cucumber, tomato & cheese in multigrain bread (180 gms)");
            //htSSr.Add("NVSW", "Chicken Junglee in Marble Bread  (180 gms)");
            htSSr.Add("EB03", "3 kg");
            htSSr.Add("EB05", "5 kg");
            htSSr.Add("EB10", "10 kg");
            htSSr.Add("EB15", "15 kg");
            htSSr.Add("EB20", "20 kg");
            htSSr.Add("EB25", "25 kg");
            htSSr.Add("EB30", "30 kg");
            htSSr.Add("XBPA", "5 Kg");
            htSSr.Add("XBPJ", "20 Kg");
            htSSr.Add("XBPB", "10 Kg");
            htSSr.Add("XBPC", "15 Kg");
            htSSr.Add("XBPD", "30 Kg");
            htSSr.Add("XBPE", "3 Kg");

            //SpiceJet
            htSSr.Add("NVML", "Murg lababdar on bed of palak pulao & Dal panchrattni (260 gms)");
            htSSr.Add("VGML", "Masala Dosa with Tomato onion uttapam and kanjivaram mini idli in sambar along with Coconut Chutney (220 gms)");
            htSSr.Add("VGSW", "Cucumber, tomato & cheese in multigrain bread (180 gms)");
            htSSr.Add("NVSW", "Non Veg Sandwich");
            htSSr.Add("NCC1", "Grilled Chicken Breast with Mushroom Sauce, Yellow Rice, Sautéed Carrots & Beans Baton (270 gms)");
            htSSr.Add("NCC2", "Chicken in Red Thai Curry with Steamed Rice (200 gms)");
            htSSr.Add("NCC6", "Chicken schezwan on bed of fried rice (210 gms)");
            htSSr.Add("NCC4", "Tandoori Chicken tangri with chicken haryali tikka & vegetable shami kebab. (225 gms)");
            htSSr.Add("NCC5", "Tawa Fish masala on bed of  Steamed rice with tadka masoor dal (260  gms)");
            htSSr.Add("VCC5", "Vegetable Pasta in Neapolitan sauce (280 gms)");
            htSSr.Add("VCC2", "Vegtable in Red Thai Curry with Steamed Rice (200 gms)");
            htSSr.Add("VCC6", "Vegetable Daliya (280 gms)");
            htSSr.Add("JNML", "Jain Hot Meal");
            htSSr.Add("JNSW", "Jain Cold Sandwich (current Cucumber and Tomato sandwich) ");
            htSSr.Add("DNVL", "Non - Vegetarian Diabetic Hot Meal");
            htSSr.Add("DBML", "Vegetarian Diabetic Hot Meal");
            htSSr.Add("GFNV", "Non - Vegetarian Gluten-free Hot Meal");
            htSSr.Add("GFVG", "Vegetarian Gluten-free Hot Meal");
            htSSr.Add("GFCM", "Vegetarian Gluten-free Cold Meal (Dhokla)");
            htSSr.Add("NVRT", "Navratra Hot Meal");
            htSSr.Add("FPML", "Fruit Platter");
            htSSr.Add("LCVS", "Low cal salad Vegetarian");
            htSSr.Add("LCNS", "Low cal salad Non - Vegetarian");
            htSSr.Add("CHML", "Kid's meal");
            htSSr.Add("NACH", "CHIPS");
            htSSr.Add("COOK", "COOKIES");

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
