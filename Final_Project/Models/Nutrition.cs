using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Nutrition
    {
        [Key]
        public int NutritionID { get; set; }

        [Required]
        public int FoodItemID { get; set; }

        public int Calories { get; set; }

        public float Fat { get; set; } // in grams

        public float Carbohydrates { get; set; } // in grams

        public float Fiber { get; set; } // in grams

        public float Sugar { get; set; } // in grams

        public float Protein { get; set; } // in grams

        // Navigation property
        [ForeignKey("FoodItemID")]
        public virtual FoodItem FoodItem { get; set; }
    }
}
