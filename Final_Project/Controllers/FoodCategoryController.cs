using Microsoft.AspNetCore.Mvc;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class FoodCategoryController : Controller
    {
        RestaurantContext context = new RestaurantContext();

        public IActionResult Index()
        {
            List<FoodCategory> categoryList = context.FoodCategories.ToList();
            return View("Index", categoryList);
        }

        public IActionResult Details(int id)
        {
            var category = context.FoodCategories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View("Details", category);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(FoodCategory category)
        {
            if (ModelState.IsValid)
            {
                context.FoodCategories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", category);
        }

        public IActionResult Edit(int id)
        {
            var category = context.FoodCategories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View("Edit", category);
        }

        [HttpPost]
        public IActionResult Edit(FoodCategory category)
        {
            if (ModelState.IsValid)
            {
                context.FoodCategories.Update(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", category);
        }

        public IActionResult Delete(int id)
        {
            var category = context.FoodCategories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View("Delete", category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = context.FoodCategories.FirstOrDefault(c => c.CategoryID == id);
            if (category != null)
            {
                context.FoodCategories.Remove(category);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
