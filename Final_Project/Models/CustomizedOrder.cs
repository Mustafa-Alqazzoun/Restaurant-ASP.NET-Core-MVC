namespace Final_Project.Models
{
    public class CustomizedOrder
    {

        public int Id { get; set; }
        public int OrderID { get; set; }
        public Order2 Order { get; set; }

        public int UnitID { get; set; }
        public Unit Unit { get; set; }

    }
}
