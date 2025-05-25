//using Final_Project.Models;
//using Final_Project.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Final_Project.Controllers
//{
//    public class Order2Controller : Controller
//    {
//        RestaurantContext context=new RestaurantContext();

//        public IActionResult AddOrder()
//        {
//            AddOrder2 addOrder = new AddOrder2();
//            addOrder.AvailableUnits=context.Units.ToList();
//            return View("AddOrder", addOrder);
//        }

//        public IActionResult SaveOrder(AddOrder2 addOrder)
//        {
//            if (addOrder.SelectedUnitsIDs == null )
//            {
//                ModelState.AddModelError("SelectedUnitIds", "Please select at least one unit.");
//            }
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var order = addOrder.order;
//                    var units =  context.Units.Where(u => addOrder.SelectedUnitsIDs.Contains(u.Id)).ToList();
//                    order.TotalPrice = units.Sum(u => u.Price);
//                    context.Add(order);
//                    context.SaveChanges();

//                    foreach (var unitId in addOrder.SelectedUnitsIDs)
//                    {
//                        CustomizedOrder customizedOrder = new CustomizedOrder();
//                        customizedOrder.OrderID = order.Id;
//                        customizedOrder.UnitID = unitId;
//                        context.CustomizedOrders.Add(customizedOrder);
//                    }
//                    context.SaveChangesAsync();

//                    return RedirectToAction(".....");
                
//                }
//                catch(Exception ex)
//                {
//                    ModelState.AddModelError("Key", ex.InnerException.Message);
//                }
//            }
//            addOrder.AvailableUnits = context.Units.ToList();   
//            return View("AddOrder", addOrder);
//        }
//        public IActionResult EditOrder(int id)
//        {
//            Order2 order = context.Orders2.Include(o => o.Units).FirstOrDefault(o => o.Id == id);
//            if (order == null)
//            {
//                return NotFound();
//            }
//            AddOrder2 order2 = new AddOrder2();
//            order2.order= order;
//            order2.AvailableUnits = context.Units.ToList(); 
//            order2.SelectedUnitsIDs=(List<int>)order.Units.Select(u => u.Id);


//            return View("EditOrder", order2);
//        }


//    }
//}
