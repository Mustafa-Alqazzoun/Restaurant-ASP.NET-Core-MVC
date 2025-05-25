using Final_Project.Models;
using Final_Project.Repositories;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final_Project.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository orderRepo;
        IFoodItemRepository foodItemRepo;
        IFoodCategoryRepository foodCategoryRepo;
        IBranchRepository branchRepo;
        ICustomerRepository customerRepo;

        public OrderController(IOrderRepository _orderRepo,
                              IFoodItemRepository _foodItemRepo,
                              IFoodCategoryRepository _foodCategoryRepo,
                              IBranchRepository _branchRepo,
                              ICustomerRepository _customerRepo)
        {
            orderRepo = _orderRepo;
            foodItemRepo = _foodItemRepo;
            foodCategoryRepo = _foodCategoryRepo;
            branchRepo = _branchRepo;
            customerRepo = _customerRepo;
        }

        public IActionResult Index()
        {
            List<Order> Orders = orderRepo.GetAll("Customer,Branch");
            return View("Index", Orders);
        }


        public IActionResult Details(int id)
        {
            try
            {
                // Get order with all related data
                Order order = orderRepo.GetOrderWithDetails(id);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("Index");
                }

                // Create status options for dropdown
                ViewBag.StatusOptions = new SelectList(
                    new[] { "Pending", "Preparing", "Ready", "Served", "Completed", "Cancelled" },
                    order.Status
                );

                return View("Details", order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading order details.";
                return RedirectToAction("Index");
            }
        }

        // New method to update order status
        [HttpPost]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            try
            {
                // Validate status
                var validStatuses = new[] { "Pending", "Preparing", "Ready", "Served", "Completed", "Cancelled" };
                if (!validStatuses.Contains(status))
                {
                    TempData["ErrorMessage"] = "Invalid status selected.";
                    return RedirectToAction("Details", new { id = orderId });
                }

                // Get the order
                Order order = orderRepo.GetById(orderId);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("Index");
                }

                // Update the status
                order.Status = status;
                orderRepo.Update(order);
                orderRepo.Save();

                TempData["SuccessMessage"] = $"Order status updated to '{status}' successfully.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating order status.";
                return RedirectToAction("Details", new { id = orderId });
            }
        }

        public IActionResult Menu()
        {
            List<FoodCategory> Categories = foodCategoryRepo.GetAll("FoodItems");
            return View("Menu", Categories);
        }

        public IActionResult GetFoodItemsByCategory(int categoryId)
        {
            List<FoodItem> FoodItems = (List<FoodItem>)foodItemRepo.GetFoodItemsByCategory(categoryId);
            return Json(FoodItems);
        }

        public IActionResult Create()
        {
            CreateOrderViewModel OrderVM = new CreateOrderViewModel();
            OrderVM.AvailableFoodItems = foodItemRepo.GetAll("FoodCategory");
            OrderVM.AvailableBranches = branchRepo.GetAll();
            OrderVM.OrderItems = new List<OrderItemViewModel>();

            ViewBag.BranchID = new SelectList(branchRepo.GetAll(), "BranchID", "Name");

            return View("Create", OrderVM);
        }

        // Replace the existing SaveNew method with this updated version
        [HttpPost]
        public IActionResult SaveNew(CreateOrderViewModel NewOrder)
        {
            // Custom validation for order items
            if (NewOrder.OrderItems == null || NewOrder.OrderItems.Count == 0)
            {
                ModelState.AddModelError("OrderItems", "Please add at least one item to your order.");
            }
            else
            {
                // Validate each order item
                for (int i = 0; i < NewOrder.OrderItems.Count; i++)
                {
                    var item = NewOrder.OrderItems[i];

                    if (item.Quantity <= 0 || item.Quantity > 50)
                    {
                        ModelState.AddModelError($"OrderItems[{i}].Quantity", "Quantity must be between 1 and 50.");
                    }

                    if (item.Price <= 0)
                    {
                        ModelState.AddModelError($"OrderItems[{i}].Price", "Price must be greater than 0.");
                    }

                    if (string.IsNullOrEmpty(item.Name))
                    {
                        ModelState.AddModelError($"OrderItems[{i}].Name", "Item name is required.");
                    }

                    // Validate that the food item exists
                    var foodItem = foodItemRepo.GetById(item.FoodItemID);
                    if (foodItem == null)
                    {
                        ModelState.AddModelError($"OrderItems[{i}].FoodItemID", "Selected food item is not valid.");
                    }
                    else if (Math.Abs(foodItem.Price - item.Price) > 0.01f) // Changed from 0.01m to 0.01f for float comparison
                    {
                        ModelState.AddModelError($"OrderItems[{i}].Price", "Item price has been modified. Please refresh and try again.");
                    }
                }

                // Check for duplicate items
                var duplicateItems = NewOrder.OrderItems
                    .GroupBy(x => x.FoodItemID)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                if (duplicateItems.Any())
                {
                    ModelState.AddModelError("OrderItems", "Duplicate items found. Please combine quantities for the same item.");
                }
            }

            // Validate contact number format if provided
            if (!string.IsNullOrEmpty(NewOrder.ContactNumber))
            {
                var cleanedContact = NewOrder.ContactNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                if (!System.Text.RegularExpressions.Regex.IsMatch(cleanedContact, @"^\d{10,20}$"))
                {
                    ModelState.AddModelError("ContactNumber", "Contact number must be 10-20 digits.");
                }
            }

            // Validate branch exists
            if (NewOrder.BranchID > 0)
            {
                var branch = branchRepo.GetById(NewOrder.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Selected branch is not valid.");
                }
            }

            // Validate table number if provided
            if (NewOrder.TableID.HasValue && (NewOrder.TableID.Value < 1 || NewOrder.TableID.Value > 100))
            {
                ModelState.AddModelError("TableID", "Table number must be between 1 and 100.");
            }

            // Validate payment method
            var validPaymentMethods = new[] { "Cash", "Credit Card", "Debit Card", "Digital Wallet" };
            if (!string.IsNullOrEmpty(NewOrder.PaymentMethod) && !validPaymentMethods.Contains(NewOrder.PaymentMethod))
            {
                ModelState.AddModelError("PaymentMethod", "Selected payment method is not valid.");
            }

            // Get customer ID from contact number if provided
            int? customerId = null;
            if (!string.IsNullOrEmpty(NewOrder.ContactNumber))
            {
                try
                {
                    customerId = GetCustomerIdByContact(NewOrder.ContactNumber);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ContactNumber", "Error validating customer contact number.");
                    // Log the exception here if you have logging configured
                }
            }

            // Validate total amount - Convert to float for comparison
            var calculatedTotal = NewOrder.OrderItems?.Sum(item => item.Price * item.Quantity) ?? 0f;
            if (calculatedTotal <= 0)
            {
                ModelState.AddModelError("", "Order total must be greater than $0.");
            }
            else if (calculatedTotal > 10000f) // Changed from 10000 to 10000f for float comparison
            {
                ModelState.AddModelError("", "Order total cannot exceed $10,000.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Order Order = new Order
                    {
                        CustomerID = customerId,
                        BranchID = NewOrder.BranchID,
                        TableID = NewOrder.TableID,
                        OrderTime = DateTime.Now,
                        Status = "Pending",
                        TotalAmount = calculatedTotal, // calculatedTotal is already float
                        PaymentMethod = NewOrder.PaymentMethod
                    };

                    List<OrderItem> OrderItems = new List<OrderItem>();
                    foreach (var item in NewOrder.OrderItems)
                    {
                        OrderItems.Add(new OrderItem
                        {
                            FoodItemID = item.FoodItemID,
                            Quantity = item.Quantity,
                            Price = item.Price // item.Price should already be float
                        });
                    }

                    // Add the order and get the created order with its ID
                    var createdOrder = orderRepo.AddOrderWithItems(Order, OrderItems);

                    TempData["SuccessMessage"] = $"Order created successfully! Order total: ${calculatedTotal:F2}";

                    // Redirect to Details view instead of Index
                    return RedirectToAction("Details", new { id = createdOrder.OrderID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the order. Please try again.");
                    // Log the exception here if you have logging configured
                }
            }

            // If we got this far, something failed, redisplay form
            try
            {
                NewOrder.AvailableFoodItems = foodItemRepo.GetAll("FoodCategory");
                NewOrder.AvailableBranches = branchRepo.GetAll();
                ViewBag.BranchID = new SelectList(branchRepo.GetAll(), "BranchID", "Name", NewOrder.BranchID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading form data. Please refresh the page.");
                // Log the exception here if you have logging configured
            }

            return View("Create", NewOrder);
        }

        public IActionResult GetFoodItemDetails(int id)
        {
            try
            {
                FoodItem FoodItem = foodItemRepo.GetById(id);
                if (FoodItem == null)
                {
                    return Json(new { error = "Food item not found." });
                }
                return Json(new
                {
                    id = FoodItem.FoodItemID,
                    name = FoodItem.Name,
                    price = FoodItem.Price,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error retrieving food item details." });
            }
        }

        private int? GetCustomerIdByContact(string contactNumber)
        {
            if (string.IsNullOrEmpty(contactNumber))
                return null;

            try
            {
                var customer = customerRepo.GetByContactNumber(contactNumber);
                return customer?.CustomerID;
            }
            catch (Exception ex)
            {
                // Log the exception here if you have logging configured
                throw new Exception("Error retrieving customer information.", ex);
            }
        }
    }
}