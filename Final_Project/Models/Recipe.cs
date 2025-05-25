using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }

        [Required]
        public int FoodItemID { get; set; }

        [Required]
        public int IngredientID { get; set; }

        [Required]
        public float Quantity { get; set; }

        // Navigation properties
        [ForeignKey("FoodItemID")]
        public virtual FoodItem FoodItem { get; set; }

        [ForeignKey("IngredientID")]
        public virtual Ingredient Ingredient { get; set; }
    }


}
