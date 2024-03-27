using System;
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

        public static List<SpicejetMealImageList> GetAllmeal()
        {
            List<SpicejetMealImageList> mealdatalist = new List<SpicejetMealImageList>();

            var mealDataCollection = new[]
            {
 
            new { MealCode = "VGSW", MealImage = "Cucumber, tomato & cheese in multigrain bread (180 gms)"},
            new { MealCode = "NVSW", MealImage = "Chicken Junglee in Marble Bread  (180 gms)"},
            
              
            
        };

            foreach (var data in mealDataCollection)
            {
                SpicejetMealImageList mealItem = new SpicejetMealImageList
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
