using Final_Project.Models;

namespace Final_Project.ViewModels
{
    public class FoodItemDetailsViewModel
    {
        public FoodItem FoodItem { get; set; }
        public Nutrition Nutrition { get; set; }
        public FoodCategory FoodCategory { get; set; }
        public List<FoodItem> RelatedItems { get; set; } = new List<FoodItem>();

        // Additional properties for better display
        public bool HasNutrition => Nutrition != null;
        public string FormattedPrice => FoodItem?.Price.ToString("C") ?? "$0.00";
        public string CategoryName => FoodCategory?.CategoryName ?? "Unknown Category";
    }
}