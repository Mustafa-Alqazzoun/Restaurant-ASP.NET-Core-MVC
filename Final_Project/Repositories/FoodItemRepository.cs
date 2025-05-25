using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Repositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly RestaurantContext context;

        public FoodItemRepository()
        {
            context = new RestaurantContext();
        }

        public List<FoodItem> GetAll(string includes = "")
        {
            IQueryable<FoodItem> query = context.FoodItems;

            if (!string.IsNullOrEmpty(includes))
            {
                if (includes.Contains("FoodCategory"))
                {
                    query = query.Include(f => f.FoodCategory);
                }
            }

            return query.ToList();
        }

        public FoodItem GetById(int id)
        {
            return context.FoodItems.FirstOrDefault(f => f.FoodItemID == id);
        }

        public void Add(FoodItem obj)
        {
            context.FoodItems.Add(obj);
        }

        public void Update(FoodItem obj)
        {
            context.FoodItems.Update(obj);
        }

        public void Delete(int id)
        {
            var foodItem = GetById(id);
            if (foodItem != null)
            {
                context.FoodItems.Remove(foodItem);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        // Get food items by category
        public List<FoodItem> GetFoodItemsByCategory(int categoryId)
        {
            return context.FoodItems
                .Where(f => f.FoodCategoryID == categoryId)
                .Include(f => f.FoodCategory)
                .ToList();
        }

        object IFoodItemRepository.GetFoodItemsByCategory(int categoryId)
        {
            return GetFoodItemsByCategory(categoryId);
        }
    }
}