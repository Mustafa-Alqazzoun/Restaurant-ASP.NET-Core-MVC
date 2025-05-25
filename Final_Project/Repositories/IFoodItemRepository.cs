using Final_Project.Models;

namespace Final_Project.Repositories
{
    public interface IFoodItemRepository : IRepository<FoodItem>
    {
        // Additional methods specific to food items
        object GetFoodItemsByCategory(int categoryId);
    }
}