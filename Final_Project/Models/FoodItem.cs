using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemID { get; set; }


        [Required]
        public int FoodCategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

 

        [StringLength(255)]
        public string Description { get; set; }
        public string ItemImage { get; set; }

        // Navigation properties
        [ForeignKey("FoodCategoryID")]
        public virtual FoodCategory FoodCategory { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

}

