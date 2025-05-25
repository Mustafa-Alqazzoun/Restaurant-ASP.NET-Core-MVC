using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Table
    {
        [Key]
        public int TableID { get; set; }

        public int BranchID { get; set; }
        public int Capacity { get; set; }

        
        public string Location { get; set; } = "Indoor"; // Default to Indoor

        
        public string Status { get; set; } = "Available"; // Default to Available

        // Navigation property
        public virtual List<Reservation> Reservations { get; set; } 
    }

    public class ValidLocationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string location && (location == "Indoor" || location == "Outdoor"))
                return ValidationResult.Success;
            return new ValidationResult(ErrorMessage);
        }
    }
}