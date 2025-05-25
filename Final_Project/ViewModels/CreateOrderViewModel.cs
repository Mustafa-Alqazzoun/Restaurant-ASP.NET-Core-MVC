using Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels
{
    public class CreateOrderViewModel
    {
        public int? CustomerID { get; set; }

        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters.")]
        [RegularExpression(@"^[\d\s\-\(\)]+$", ErrorMessage = "Contact number can only contain digits, spaces, hyphens, and parentheses.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Please select a branch.")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }

        [Range(1, 100, ErrorMessage = "Table number must be between 1 and 100.")]
        [Display(Name = "Table Number")]
        public int? TableID { get; set; }

        [Required(ErrorMessage = "Please add at least one item to your order.")]
        [MinLength(1, ErrorMessage = "Please add at least one item to your order.")]
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        [Required(ErrorMessage = "Please select a payment method.")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        // For display purposes
        public List<FoodItem> AvailableFoodItems { get; set; } = new List<FoodItem>();
        public List<Branch> AvailableBranches { get; set; } = new List<Branch>();

        // Computed property for total amount validation - changed to float
        public float TotalAmount => OrderItems?.Sum(item => item.Price * item.Quantity) ?? 0f;
    }

    public class OrderItemViewModel
    {
        [Required(ErrorMessage = "Food item is required.")]
        [Display(Name = "Food Item")]
        public int FoodItemID { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 50, ErrorMessage = "Quantity must be between 1 and 50.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 9999.99, ErrorMessage = "Price must be between $0.01 and $9999.99.")]
        [Display(Name = "Price")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        // Computed property for item total - changed to float
        public float ItemTotal => Price * Quantity;
    }
}