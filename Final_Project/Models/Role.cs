using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        // Navigation properties
        public virtual ICollection<Staff> Staff { get; set; }
    }

}

