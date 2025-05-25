using Final_Project.Models;

namespace Final_Project.ViewModels
{
    public class OrderSummaryViewModel
    {
        public Order2 Order { get; set; }
        public List<Unit> Units { get; set; }
    }
}