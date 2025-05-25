using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }

        [Required]
        public int BranchID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string ContactNumber { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public decimal Salary { get; set; }

        // Navigation properties
        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }

}

