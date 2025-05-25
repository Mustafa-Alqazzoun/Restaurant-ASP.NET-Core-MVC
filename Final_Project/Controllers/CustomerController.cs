using Microsoft.AspNetCore.Mvc;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class CustomerController : Controller
    {
        RestaurantContext context = new RestaurantContext();

        public IActionResult Index()
        {
            List<Customer> customerList = context.Customers.ToList();
            return View("Index", customerList);
        }

        public IActionResult Details(int id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View("Details", customer);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", customer);
        }

        public IActionResult Edit(int id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View("Edit", customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Update(customer);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", customer);
        }

        public IActionResult Delete(int id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View("Delete", customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}