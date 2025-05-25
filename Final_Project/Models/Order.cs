 using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int BranchID { get; set; }

        public int? CustomerID { get; set; }

        public int? TableID { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // Pending, Preparing, Ready, Served, Completed, etc.

        [Required]
        public float TotalAmount { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        // Navigation properties
        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }


}

