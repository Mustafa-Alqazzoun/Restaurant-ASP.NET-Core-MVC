using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Repositories
{
    public class FoodCategoryRepository : IFoodCategoryRepository
    {
        private readonly RestaurantContext context;

        public FoodCategoryRepository()
        {
            context = new RestaurantContext();
        }

        public List<FoodCategory> GetAll(string includes = "")
        {
            IQueryable<FoodCategory> query = context.FoodCategories;

            if (!string.IsNullOrEmpty(includes))
            {
                if (includes.Contains("FoodItems"))
                {
                    query = query.Include(c => c.FoodItems);
                }
            }

            return query.ToList();
        }

        public FoodCategory GetById(int id)
        {
            return context.FoodCategories.FirstOrDefault(c => c.CategoryID == id);
        }

        public void Add(FoodCategory obj)
        {
            context.FoodCategories.Add(obj);
        }

        public void Update(FoodCategory obj)
        {
            context.FoodCategories.Update(obj);
        }

        public void Delete(int id)
        {
            var category = GetById(id);
            if (category != null)
            {
                context.FoodCategories.Remove(category);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}