using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantContext context;

        public OrderRepository()
        {
            context = new RestaurantContext();
        }

        public List<Order> GetAll(string includes = "")
        {
            IQueryable<Order> query = context.Orders;

            if (!string.IsNullOrEmpty(includes))
            {
                if (includes.Contains("Customer"))
                {
                    query = query.Include(o => o.Customer);
                }
                if (includes.Contains("Branch"))
                {
                    query = query.Include(o => o.Branch);
                }
                if (includes.Contains("OrderItems"))
                {
                    query = query.Include(o => o.OrderItems);
                }
            }

            return query.ToList();
        }

        public Order GetById(int id)
        {
            return context.Orders.FirstOrDefault(o => o.OrderID == id);
        }

        // New method to get order with all details including order items and food item details
        public Order GetOrderWithDetails(int id)
        {
            return context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Branch)
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                        .ThenInclude(fi => fi.FoodCategory)
                .Include(o => o.Payments)
                .FirstOrDefault(o => o.OrderID == id);
        }

        public void Add(Order obj)
        {
            context.Orders.Add(obj);
        }

        public void Update(Order obj)
        {
            context.Orders.Update(obj);
        }

        public void Delete(int id)
        {
            var order = GetById(id);
            if (order != null)
            {
                context.Orders.Remove(order);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        // Add an order with items and return the created order
        public Order AddOrderWithItems(Order order, List<OrderItem> orderItems)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Add the order
                    context.Orders.Add(order);
                    context.SaveChanges();

                    // Add the order items
                    foreach (var item in orderItems)
                    {
                        item.OrderID = order.OrderID;
                        context.OrderItems.Add(item);
                    }
                    context.SaveChanges();

                    transaction.Commit();

                    // Return the order with its generated ID
                    return order;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}