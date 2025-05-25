
using System;
using System.Linq;
using System.Security.Claims;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class CustomizeOrderController : Controller
    {
        private RestaurantContext db = new RestaurantContext();

        [HttpGet]
        public ActionResult Create()
        {
          
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
            {
                return Content("Customer ID claim not found or invalid.");
            }

            var customer = db.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
            {
                return Content($"Customer with ID={customerId} not found");
            }

            var viewModel = new OrderFormViewModel
            {
                AvailableUnits = db.Units.ToList(),
                Customer = customer
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(OrderFormViewModel viewModel)
        {
       
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
            {
                ModelState.AddModelError("", "Customer ID claim not found or invalid.");
                viewModel.AvailableUnits = db.Units.ToList();
                return View(viewModel);
            }

            var customer = db.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
            {
                ModelState.AddModelError("", "Customer not found.");
                viewModel.AvailableUnits = db.Units.ToList();
                return View(viewModel);
            }

            if (viewModel.SelectedUnitIds == null || !viewModel.SelectedUnitIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one ingredient.");
                viewModel.AvailableUnits = db.Units.ToList();
                return View(viewModel);
            }

            var selectedUnits = db.Units
                .Where(u => viewModel.SelectedUnitIds.Contains(u.Id))
                .ToList();

            decimal totalPrice = (decimal)selectedUnits.Sum(u => (float)u.Price);

            var order = new Order2
            {
                CustomerID = customer.CustomerID,
                TotalPrice = (float)totalPrice,
                CustomerName = customer.Name,
                OrderDate = DateTime.Now
            };

            db.Order2s.Add(order);
            db.SaveChanges();

            foreach (var unitId in viewModel.SelectedUnitIds)
            {
                var customizedOrder = new CustomizedOrder
                {
                    OrderID = order.Id,
                    UnitID = unitId
                };
                db.CustomizedOrders.Add(customizedOrder);
            }

            db.SaveChanges();
            return RedirectToAction("Menu", "Order");
        }
    }
}

