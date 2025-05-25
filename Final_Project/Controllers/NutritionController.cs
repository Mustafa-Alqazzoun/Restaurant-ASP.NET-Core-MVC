using System.Linq;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class NutritionController : Controller
    {
        RestaurantContext context = new RestaurantContext();
        public IActionResult ShowNutritionByID(int FoodItemID)
        {
            Nutrition nutrition;

            nutrition = context.Nutritions.FirstOrDefault(n => n.NutritionID == FoodItemID);
            return View();
        }
        public IActionResult AddNutrition()
        {
            List<FoodItem> FoodItemsFromDB = context.FoodItems.ToList();
            NutritionWithFoodItemList nutritionwithfooditemlist = new();
            nutritionwithfooditemlist.FoodItems = FoodItemsFromDB;

            return View("AddNutrition", nutritionwithfooditemlist);
        }
        public IActionResult SaveNutrition(NutritionWithFoodItemList nutritionwithfooditemlistFromRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Nutrition NutritionToDB = new();
                    NutritionToDB.FoodItemID = nutritionwithfooditemlistFromRequest.FoodItemID;
                    NutritionToDB.Calories = nutritionwithfooditemlistFromRequest.Calories;
                    NutritionToDB.Fat = nutritionwithfooditemlistFromRequest.Fat;
                    NutritionToDB.Carbohydrates = nutritionwithfooditemlistFromRequest.Carbohydrates;
                    NutritionToDB.Fiber = nutritionwithfooditemlistFromRequest.Fiber;
                    NutritionToDB.Sugar = nutritionwithfooditemlistFromRequest.Sugar;
                    NutritionToDB.Protein = nutritionwithfooditemlistFromRequest.Protein;

                    // Add the nutrition object to context before saving
                    context.Nutritions.Add(NutritionToDB);
                    context.SaveChanges();
                    return RedirectToAction("Index"); // Changed from "........" to a proper action

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Key", ex.InnerException?.Message ?? ex.Message);
                }
            }
            nutritionwithfooditemlistFromRequest.FoodItems = context.FoodItems.ToList();
            return View("AddNutrition", nutritionwithfooditemlistFromRequest);
        }
        public IActionResult EditNutrition(int ID)
        {
            Nutrition nutrition = context.Nutritions.FirstOrDefault(n => n.NutritionID == ID);
            List<FoodItem> fooditems = context.FoodItems.ToList();
            NutritionWithFoodItemList nutritionwithfooditemlist = new();

            nutritionwithfooditemlist.Calories = nutrition.Calories;
            nutritionwithfooditemlist.Fat = nutrition.Fat;
            nutritionwithfooditemlist.Carbohydrates = nutrition.Carbohydrates;
            nutritionwithfooditemlist.Fiber = nutrition.Fiber;
            nutritionwithfooditemlist.Protein = nutrition.Protein;
            nutritionwithfooditemlist.Sugar = nutrition.Sugar;
            // Removed duplicate Calories assignment
            nutritionwithfooditemlist.FoodItemID = nutrition.FoodItemID; // Added this line to set the FoodItemID
            nutritionwithfooditemlist.FoodItems = fooditems;

            return View("EditNutrition", nutritionwithfooditemlist);
        }
        public IActionResult SaveEditNutrition(NutritionWithFoodItemList nutritionwithfooditemlist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Nutrition nutrition = context.Nutritions.FirstOrDefault(n => n.NutritionID == nutritionwithfooditemlist.id);

                    if (nutrition != null)
                    {
                        nutrition.Calories = nutritionwithfooditemlist.Calories;
                        nutrition.Fat = nutritionwithfooditemlist.Fat;
                        nutrition.Carbohydrates = nutritionwithfooditemlist.Carbohydrates;
                        nutrition.Fiber = nutritionwithfooditemlist.Fiber;
                        nutrition.Protein = nutritionwithfooditemlist.Protein;
                        nutrition.Sugar = nutritionwithfooditemlist.Sugar;
                        // Removed duplicate Calories assignment
                        nutrition.FoodItemID = nutritionwithfooditemlist.FoodItemID;
                        context.SaveChanges();

                        return RedirectToAction("Index"); // Changed from "........" to a proper action
                    }
                    else
                    {
                        ModelState.AddModelError("Key", "Nutrition record not found.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Key", ex.InnerException?.Message ?? ex.Message);
                }


            }
            nutritionwithfooditemlist.FoodItems = context.FoodItems.ToList();
            return View("EditNutrition", nutritionwithfooditemlist);

        }
    }
}