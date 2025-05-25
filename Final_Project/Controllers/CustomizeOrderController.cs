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
        private readonly RestaurantContext _db;

        public CustomizeOrderController(RestaurantContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
            {
                return View("Error", new ErrorViewModel { Message = "Customer ID claim not found or invalid." });
            }

            var customer = _db.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
            {
                return View("Error", new ErrorViewModel { Message = $"Customer with ID={customerId} not found." });
            }

            var viewModel = new OrderFormViewModel
            {
                AvailableUnits = _db.Units.ToList(),
                CustomerID = customer.CustomerID,
                CustomerName = customer.Name,
                TableID = "", // Set default empty string
                Customer = new OrderCustomerDto
                {
                    CustomerID = customer.CustomerID,
                    Name = customer.Name
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderFormViewModel viewModel)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized("User is not authenticated.");
                }

                var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                {
                    ModelState.AddModelError("", "Customer ID claim not found or invalid.");
                    viewModel.AvailableUnits = _db.Units.ToList();
                    return View(viewModel);
                }

                var customer = _db.Customers.FirstOrDefault(c => c.CustomerID == customerId);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer not found.");
                    viewModel.AvailableUnits = _db.Units.ToList();
                    return View(viewModel);
                }

                // Process the form data - convert quantities to unit IDs
                var selectedUnitIds = new List<int>();
                decimal totalPrice = 0;

                if (viewModel.SelectedUnitQuantities != null && viewModel.SelectedUnitQuantities.Any())
                {
                    var allUnits = _db.Units.ToList(); // Get all units for price calculation

                    foreach (var kvp in viewModel.SelectedUnitQuantities.Where(x => x.Value > 0))
                    {
                        var unit = allUnits.FirstOrDefault(u => u.Id == kvp.Key);
                        if (unit != null)
                        {
                            // Add the unit ID for each quantity
                            for (int i = 0; i < kvp.Value; i++)
                            {
                                selectedUnitIds.Add(kvp.Key);
                            }
                            totalPrice += (decimal)(unit.Price * kvp.Value);
                        }
                    }
                }

                // Check if any items were selected
                if (!selectedUnitIds.Any())
                {
                    ModelState.AddModelError("", "Please select at least one ingredient.");
                    viewModel.AvailableUnits = _db.Units.ToList();
                    return View(viewModel);
                }

                // Create the order
                var order = new Order2
                {
                    CustomerID = customer.CustomerID,
                    TotalPrice = (float)totalPrice,
                    CustomerName = customer.Name,
                    OrderDate = DateTime.Now,
                    TableID = "T001", // Fixed table ID
                    CustomerNumber = 1 // Fixed customer number
                };

                _db.Order2s.Add(order);
                _db.SaveChanges();

                // Add customized order entries
                foreach (var unitId in selectedUnitIds)
                {
                    var customizedOrder = new CustomizedOrder
                    {
                        OrderID = order.Id,
                        UnitID = unitId
                    };
                    _db.CustomizedOrders.Add(customizedOrder);
                }

                _db.SaveChanges();
                return RedirectToAction("Menu", "Order");
            }
            catch (Exception ex)
            {
                // Log the actual error for debugging
                ModelState.AddModelError("", $"Error: {ex.Message}");
                if (viewModel.AvailableUnits == null)
                {
                    viewModel.AvailableUnits = _db.Units.ToList();
                }
                return View(viewModel);
            }
        }
    }
}