using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public int? ProfileID { get; set; }


      
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string ContactNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        // Navigation properties
        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Order2> CustOrders { get; set; }
    }


}

