using Final_Project.Models;

namespace Final_Project.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        // Additional methods specific to orders
        Order AddOrderWithItems(Order order, List<OrderItem> orderItems);
        Order GetOrderWithDetails(int id);
    }
}