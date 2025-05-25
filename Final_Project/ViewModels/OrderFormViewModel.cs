using Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels
{
    public class OrderFormViewModel
    {

        [Display(Name = "Selected Items")]
        public List<int> SelectedUnitIds { get; set; } = new List<int>();

        public List<Unit> AvailableUnits { get; set; }
        public int CustomerNumber { get; internal set; }
        public string TableID { get; internal set; }
        public string CustomerName { get; internal set; }
        public int CustomerID { get; internal set; }

        public Customer Customer { get; set; }
    }
}


