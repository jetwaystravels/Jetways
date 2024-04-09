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
            new { MealCode = "NCBB", MealImage = "Awadhi Chicken Biryani with Mirch Salan(Chargable meal code).png" },
            new { MealCode = "NCBF", MealImage = "Awadhi Chicken Biryani with Mirch Salan(Free meal code).png" },
            new { MealCode = "VBKB", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)(Chargable meal code).png" },
            new { MealCode = "VBKF", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)(Free meal code).png" },
            new { MealCode = "NCJB", MealImage = "Chicken Junglee Sandwich(Chargable meal code).png" },
            new { MealCode = "NCJF", MealImage = "Chicken Junglee Sandwich(Free meal code).png" },
            new { MealCode = "NCNB", MealImage = "Chicken Nuggets with Fried Potatoes(Chargable meal code).png" },
            new { MealCode = "NCNF", MealImage = "Chicken Nuggets with Fried Potatoes(Free meal code).png" },
            new { MealCode = "NFFB", MealImage = "Herb Grilled Fish Fillet(Chargable meal code).png" },
            new { MealCode = "NFFF", MealImage = "Herb Grilled Fish Fillet(Free  meal code).png" },
            new { MealCode = "VHRB", MealImage = "Herb Roast Vegetable Roll(Chargable meal code).png" },
            new { MealCode = "VHRF", MealImage = "Herb Roast Vegetable Roll(Free meal code).png" },
            new { MealCode = "NMBB", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan(Chargable meal code).png" },
            new { MealCode = "NMBF", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan (Free meal code).png" },
            new { MealCode = "NOSB", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(Chargable meal code).png" },
            new { MealCode = "NOSF", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown(FREE meal code).png" },
            new { MealCode = "VPBB", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha(Chargable meal code).png" },
            new { MealCode = "VPBF", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha(Free meal code).png" },
            new { MealCode = "VIVB", MealImage = "Mini Idlis, Medu Vada and Upma(Chargable meal code).png" },
            new { MealCode = "VIVF", MealImage = "Mini Idlis, Medu Vada and Upma(Free meal code).png" },
            new { MealCode = "NMTB", MealImage = "Murg Tikka Masala with Lachha Paratha(Chargable meal code).png" },
            new { MealCode = "NMTF", MealImage = "Murg Tikka Masala with Lachha Paratha(Free  meal code).png" },
            new { MealCode = "VPMB", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao(Chargable meal code).png" },
            new { MealCode = "VPMF", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao (Free meal code).png" },
            new { MealCode = "VFPB", MealImage = "Seasonal Fresh Fruits Platter (Chargable meal code).png" },
            new { MealCode = "VFPF", MealImage = "Seasonal Fresh Fruits Platter (Free meal code).png" },
            new { MealCode = "VSDB", MealImage = "Shôndesh Tiramisù(Chargable meal code).png" },
            new { MealCode = "VSDF", MealImage = "Shôndesh Tiramisù(Free meal code).png" },
            new { MealCode = "VMCB", MealImage = "Vegan Moilee Curry with Coconut Rice (1)(Chargable meal code).png" },
            new { MealCode = "VMCF", MealImage = "Vegan Moilee Curry with Coconut Rice (1)(Chargable meal code).png" },
            new { MealCode = "VMFB", MealImage = "Vegetable Manchurian with Fried Rice (Chargable meal code).png" },
            new { MealCode = "VMFF", MealImage = "Vegetable Manchurian with Fried Rice (Free meal code).png" },
            //wheelchair
             new { MealCode = "WCHS", MealImage = "Wheelchair Unable to ascend and descend step" },
             new { MealCode = "WCHR", MealImage = "Wheelchair Unable to walk long distance"},
             new { MealCode = "WCHQ", MealImage = "Wheelchair Quadriplegic" },
             new { MealCode = "WCHC", MealImage = "Wheelchair Paraplegic" },
             new { MealCode = "WCHA", MealImage = "Arrival wheelchair request" },
             new { MealCode = "WCAS", MealImage = "Airport Unable to ascend and descend steps" },
             new { MealCode = "WCAR", MealImage = "Airport Unable to walk long distance" },
          //baggage
           new { MealCode = "PVIP", MealImage = "Xpress Ahead- Prebook" },
           new { MealCode = "PBCB", MealImage = "+ 5Kgs Xtra-Carry-On" },
           new { MealCode = "PBCA", MealImage = "+3Kgs Xtra-Carry-On" },
           new { MealCode = "PBAF", MealImage = "+ 25 Kg Xcess Baggage" },
           new { MealCode = "PBAD", MealImage = "+ 15 kg Xcess Baggage" },
           new { MealCode = "PBAC", MealImage = "+ 10 kg Xcess Baggage" },
           new { MealCode = "PBAB", MealImage = "+ 5 kg Xcess Baggage" },
           new { MealCode = "PBA3", MealImage = "+3 kgs Xcess Baggage" },
              
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
