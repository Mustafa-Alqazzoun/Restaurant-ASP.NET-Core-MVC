using Final_Project.Models;
using Final_Project.Repositories;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final_Project.Controllers
{
    public class FoodItemController : Controller
    {
        IFoodItemRepository foodItemRepo;
        IFoodCategoryRepository foodCategoryRepo;

        public FoodItemController(IFoodItemRepository _foodItemRepo,
                                 IFoodCategoryRepository _foodCategoryRepo)
        {
            foodItemRepo = _foodItemRepo;
            foodCategoryRepo = _foodCategoryRepo;
        }

        public IActionResult Index()
        {
            List<FoodItem> FoodItemList = foodItemRepo.GetAll();
            return View("Index", FoodItemList);
        }

       
        public IActionResult Details(int id)
        {
            try
            {
                // Get the food item with category
                var foodItem = foodItemRepo.GetById(id);
                if (foodItem == null)
                {
                    TempData["ErrorMessage"] = "Food item not found.";
                    return RedirectToAction("Menu", "Order");
                }

                // Get the food category
                var category = foodCategoryRepo.GetById(foodItem.FoodCategoryID);

                // Get nutrition information
                var nutrition = GetNutritionByFoodItemId(id);

                // Get related items from the same category (excluding current item)
                var relatedItems = ((List<FoodItem>)foodItemRepo.GetFoodItemsByCategory(foodItem.FoodCategoryID))
                    .Where(item => item.FoodItemID != id)
                    .Take(4)
                    .ToList();

                var viewModel = new FoodItemDetailsViewModel
                {
                    FoodItem = foodItem,
                    FoodCategory = category,
                    Nutrition = nutrition,
                    RelatedItems = relatedItems
                };

                return View("Details", viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading food item details.";
                return RedirectToAction("Menu", "Order");
            }
        }

        // Helper method to get nutrition information
        private Nutrition GetNutritionByFoodItemId(int foodItemId)
        {
            try
            {
                using (var context = new RestaurantContext())
                {
                    return context.Nutritions.FirstOrDefault(n => n.FoodItemID == foodItemId);
                }
            }
            catch
            {
                return null;
            }
        }

        public IActionResult Create()
        {
            ViewBag.FoodCategoryID = new SelectList(foodCategoryRepo.GetAll(), "CategoryID", "CategoryName");
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(FoodItem foodItem)
        {
            if (ModelState.IsValid == true)
            {
                foodItemRepo.Add(foodItem);
                foodItemRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.FoodCategoryID = new SelectList(foodCategoryRepo.GetAll(), "CategoryID", "CategoryName", foodItem.FoodCategoryID);
            return View("Create", foodItem);
        }

        public IActionResult Edit(int id)
        {
            FoodItem FoodItem = foodItemRepo.GetById(id);
            if (FoodItem == null)
            {
                return NotFound();
            }
            ViewBag.FoodCategoryID = new SelectList(foodCategoryRepo.GetAll(), "CategoryID", "CategoryName", FoodItem.FoodCategoryID);
            return View("Edit", FoodItem);
        }

        [HttpPost]
        public IActionResult Edit(FoodItem foodItem)
        {
            if (ModelState.IsValid == true)
            {
                foodItemRepo.Update(foodItem);
                foodItemRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.FoodCategoryID = new SelectList(foodCategoryRepo.GetAll(), "CategoryID", "CategoryName", foodItem.FoodCategoryID);
            return View("Edit", foodItem);
        }

        public IActionResult Delete(int id)
        {
            FoodItem FoodItem = foodItemRepo.GetById(id);
            if (FoodItem == null)
            {
                return NotFound();
            }
            return View("Delete", FoodItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            foodItemRepo.Delete(id);
            foodItemRepo.Save();
            return RedirectToAction("Index");
        }
    }
}