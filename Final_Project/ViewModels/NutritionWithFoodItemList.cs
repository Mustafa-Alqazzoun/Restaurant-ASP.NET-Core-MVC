using Final_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.ViewModels
{
    public class NutritionWithFoodItemList
    {

        public int? id { get; set; }
        public int Calories { get; set; }

        public float Fat { get; set; }

        public float Carbohydrates { get; set; }

        public float Fiber { get; set; }
        public float Sugar { get; set; }

        public float Protein { get; set; }
        public int FoodItemID { get; set; }
        public virtual List<FoodItem> FoodItems { get; set; }
    }
}