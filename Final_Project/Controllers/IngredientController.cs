using Microsoft.AspNetCore.Mvc;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class IngredientController : Controller
    {
        RestaurantContext context = new RestaurantContext();

        public IActionResult Index()
        {
            List<Ingredient> ingredientList = context.Ingredients.ToList();
            return View("Index", ingredientList);
        }

        public IActionResult Details(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.IngredientID == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View("Details", ingredient);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                context.Ingredients.Add(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", ingredient);
        }

        public IActionResult Edit(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.IngredientID == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View("Edit", ingredient);
        }

        [HttpPost]
        public IActionResult Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                context.Ingredients.Update(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", ingredient);
        }

        public IActionResult Delete(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.IngredientID == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View("Delete", ingredient);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.IngredientID == id);
            if (ingredient != null)
            {
                context.Ingredients.Remove(ingredient);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
