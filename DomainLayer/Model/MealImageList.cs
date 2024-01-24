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
            new { MealCode = "VSDB", MealImage = "shôndeshtiramisùvsdf.png" },
            new { MealCode = "VPMB", MealImage = "paneermakhaniwith jeera aloovpmb.png" },
            new { MealCode = "VPBB", MealImage = "matar-paneer-bhurji-with-aloo-paratha vpbb.png" }
           

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
