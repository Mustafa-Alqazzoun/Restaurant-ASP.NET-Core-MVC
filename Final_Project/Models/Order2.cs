using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    [Table("Order2")] 
    public class Order2
    {
        [Key]
        public int Id { get; set; }

        public int CustomerID { get; set; }


        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public string? TableID { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNumber { get; set; }
        public float TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

       
    }
}

