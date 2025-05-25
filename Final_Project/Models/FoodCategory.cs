using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class FoodCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        // Navigation properties
        public virtual ICollection<FoodItem> FoodItems { get; set; }

    }
}
