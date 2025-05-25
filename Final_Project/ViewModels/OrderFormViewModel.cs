using System.ComponentModel.DataAnnotations;
using Final_Project.Models;

namespace Final_Project.ViewModels
{
    public class OrderFormViewModel
    {
        [Display(Name = "Selected Items")]
        public List<int> SelectedUnitIds { get; set; } = new List<int>();

        public List<Unit> AvailableUnits { get; set; } = new List<Unit>();

        public int? CustomerNumber { get; set; }
        public string TableID { get; set; } = "T001";  // Default table ID
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }

        public OrderCustomerDto Customer { get; set; }

        // Add this property to handle the quantities from the form
        public Dictionary<int, int> SelectedUnitQuantities { get; set; } = new Dictionary<int, int>();
    }

    public class OrderCustomerDto
    {
        public int CustomerID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}