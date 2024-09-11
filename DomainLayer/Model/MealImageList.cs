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
            new { MealCode = "NCBB", MealImage = "Awadhi Chicken Biryani with Mirch Salan" },
            new { MealCode = "NCBF", MealImage = "Awadhi Chicken Biryani with Mirch Salan" },
            new { MealCode = "VBKB", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)" },
            new { MealCode = "VBKF", MealImage = "Bajra Khichdi with Matar Bhaji (Diabetic-friendly)" },
            new { MealCode = "NCJB", MealImage = "Chicken Junglee Sandwich" },
            new { MealCode = "NCJF", MealImage = "Chicken Junglee Sandwich(Free meal code).png" },
            new { MealCode = "NCNB", MealImage = "Chicken Nuggets with Fried Potatoes" },
            new { MealCode = "NCNF", MealImage = "Chicken Nuggets with Fried Potatoes" },
            new { MealCode = "NFFB", MealImage = "Herb Grilled Fish Fillet" },
            new { MealCode = "NFFF", MealImage = "Herb Grilled Fish Fillet" },
            new { MealCode = "VHRB", MealImage = "Herb Roast Vegetable Roll" },
            new { MealCode = "VHRF", MealImage = "Herb Roast Vegetable Roll" },
            new { MealCode = "NMBB", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan" },
            new { MealCode = "NMBF", MealImage = "Hyderabadi Mutton Biryani with Mirch Salan " },
            new { MealCode = "NOSB", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown" },
            new { MealCode = "NOSF", MealImage = "Masala-Omelette-with-Chicken-Sausages-_-Hash-Brown" },
            new { MealCode = "VPBB", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha" },
            new { MealCode = "VPBF", MealImage = "Matar-Paneer-Bhurji-with-Aloo-Paratha" },
            new { MealCode = "VIVB", MealImage = "Mini Idlis, Medu Vada and Upma" },
            new { MealCode = "VIVF", MealImage = "Mini Idlis, Medu Vada and Upma" },
            new { MealCode = "NMTB", MealImage = "Murg Tikka Masala with Lachha Paratha" },
            new { MealCode = "NMTF", MealImage = "Murg Tikka Masala with Lachha Paratha" },
            new { MealCode = "VPMB", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao" },
            new { MealCode = "VPMF", MealImage = "Paneer Makhani with Jeera Aloo _ Vegetable Pulao " },
            new { MealCode = "VFPB", MealImage = "Seasonal Fresh Fruits Platter" },
            new { MealCode = "VFPF", MealImage = "Seasonal Fresh Fruits Platter" },
            new { MealCode = "VSDB", MealImage = "Shôndesh Tiramisù" },
            new { MealCode = "VSDF", MealImage = "Shôndesh Tiramisù" },
            new { MealCode = "VMCB", MealImage = "Vegan Moilee Curry with Coconut Rice (1)" },
            new { MealCode = "VMCF", MealImage = "Vegan Moilee Curry with Coconut Rice (1)" },
            new { MealCode = "VMFB", MealImage = "Vegetable Manchurian with Fried Rice " },
            new { MealCode = "VMFF", MealImage = "Vegetable Manchurian with Fried Rice " },
            new { MealCode = "VMGB", MealImage = "Mushroom Ghee Roast with Sriracha Fried Rice" },
            new { MealCode = "VKDB", MealImage = "Kaju Katli white chocolate Mousse" },
            new { MealCode = "MMFD", MealImage = "MOJO BAR ORANGE DARK CHOCOLATE + VITAMIN C" },
            new { MealCode = "LSCB", MealImage = "Lemon Samiya ( Vakulaa)" },
            new { MealCode = "CXCB", MealImage = "TOMATO CUCUMBER CHEESE SANDWICH" },
            new { MealCode = "CPMD", MealImage = "CHOWPATTY PHUDINA BHEL" },
            new { MealCode = "CCCB", MealImage = "Chicken Curry Rice" },
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


           new { MealCode = "PBA3", MealImage = "+3 kgs Xcess Baggage" },
           new { MealCode = "PBAB", MealImage = "+ 5 kg Xcess Baggage" },
           new { MealCode = "PBAC", MealImage = "+ 10 kg Xcess Baggage" },
           new { MealCode = "PBAD", MealImage = "+ 15 kg Xcess Baggage" },
           new { MealCode = "PBAF", MealImage = "+ 25 Kg Xcess Baggage" },
           
          
          
           
           //Indigo Bag Code
           new { MealCode = "XBPD", MealImage = "+30 kgs Xcess Baggage" },
           new { MealCode = "XBPJ", MealImage = "+ 20 kg Xcess Baggage" },
           new { MealCode = "XBPC", MealImage = "+ 15 kg Xcess Baggage" },
           new { MealCode = "XBPB", MealImage = "+ 10 kg Xcess Baggage" },
           new { MealCode = "XBPA", MealImage = "+ 5 kg Xcess Baggage" },
      

              
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
