using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string? DescriptionType { get; set; }
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
        public List<CustomizedOrder> CustomizedOrders { get; set; }
    }
}
