using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class MealImageList
    {

        public string MealCode { get; set; }
        public string MealImage { get; set; }

        public static List<MealImageList> GetAllmeal()
        {
            List<MealImageList> mealdatalist = new List<MealImageList>();

            var mealDataCollection = new[]
            {
            new { MealCode = "NCBB", MealImage = "Awadhi Chicken Biryani with Mirch Salan NCBB (Chargable meal code).png" },
            new { MealCode = "NCBF", MealImage = "Awadhi Chicken Biryani with Mirch Salan NCBF(Free meal code).png" },
            new { MealCode = "VBKB", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly) VBKB (Chargable meal code).png" },
            new { MealCode = "VBKF", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly) VBKF (Free meal code).png" },
            new { MealCode = "NCJB", MealImage = "Chicken Junglee Sandwich NCJB (Chargable meal code).png" },
            new { MealCode = "NCJF", MealImage = "Chicken Junglee Sandwich NCJF (Free meal code).png" },
            new { MealCode = "NCNB", MealImage = "Chicken Nuggets with Fried Potatoes NCNB (Chargable meal code).png" },
            new { MealCode = "NCNF", MealImage = "Chicken Nuggets with Fried Potatoes NCNF (Free meal code).png" },
            new { MealCode = "NFFB", MealImage = "Herb Grilled Fish Fillet NFFB (Chargable meal code).png" },
            new { MealCode = "NFFF", MealImage = "Herb Grilled Fish Fillet NFFF (Free  meal code).png" },
            new { MealCode = "VHRB", MealImage = "Herb Roast Vegetable Roll VHRB (Chargable meal code).png" },
            new { MealCode = "VHRF", MealImage = "Herb Roast Vegetable Roll VHRF (Free meal code).png" },
            new { MealCode = "NMBB", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan NMBB (Chargable meal code).png" },
            new { MealCode = "NMBF", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan NMBF (Free meal code).png" },
            new { MealCode = "NOSB", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown NOSB (Chargable meal code).png" },
            new { MealCode = "NOSF", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown NOSF (FREE meal code).png" },
            new { MealCode = "VPBB", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha VPBB (Chargable meal code).png" },
            new { MealCode = "VPBF", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha VPBF (Free meal code).png" },
            new { MealCode = "VIVB", MealImage = "Mini Idlis, Medu Vada and Upma VIVB(Chargable meal code).png" },
            new { MealCode = "VIVF", MealImage = "Mini Idlis, Medu Vada and Upma VIVF (Free meal code).png" },
            new { MealCode = "NMTB", MealImage = "Murg Tikka Masala with Lachha Paratha NMTB (Chargable meal code).png" },
            new { MealCode = "NMTF", MealImage = "Murg Tikka Masala with Lachha Paratha NMTF(Free  meal code).png" },
            new { MealCode = "VPMB", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao VPMB (Chargable meal code).png" },
            new { MealCode = "VPMF", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao VPMF (Free meal code).png" },
            new { MealCode = "VFPB", MealImage = "Seasonal Fresh Fruits Platter VFPB (Chargable meal code).png" },
            new { MealCode = "VFPF", MealImage = "Seasonal Fresh Fruits Platter VFPF (Free meal code).png" },
            new { MealCode = "VSDB", MealImage = "Shôndesh Tiramisù VSDB (Chargable meal code).png" },
            new { MealCode = "VSDF", MealImage = "Shôndesh Tiramisù VSDF (Free meal code).png" },
            new { MealCode = "VMCB", MealImage = "Vegan Moilee Curry with Coconut Rice (1) VMCB (Chargable meal code).png" },
            new { MealCode = "VMCF", MealImage = "Vegan Moilee Curry with Coconut Rice (1) VMCF (Chargable meal code).png" },
            new { MealCode = "VMFB", MealImage = "Vegetable Manchurian with Fried Rice VMFB (Chargable meal code).png" },
            new { MealCode = "VMFF", MealImage = "Vegetable Manchurian with Fried Rice VMFF (Free meal code).png" },
          
              
            // Add more data as needed...
        };

            foreach (var data in mealDataCollection)
            {
                MealImageList mealItem = new MealImageList
                {
                    MealCode = data.MealCode,
                    MealImage = data.MealImage
                };

                mealdatalist.Add(mealItem);
            }

            return mealdatalist;
        }

    }
}
