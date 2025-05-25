using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Coupon
    {
        [Key]
        public int CouponID { get; set; }

        [Required]
        public string Code { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; } = false;

        // Foreign key to Profile
        public int ProfileID { get; set; }

        [ForeignKey("ProfileID")]
        public Profile Profile { get; set; }
    }
}
