using Final_Project.Models;

namespace Final_Project.ViewModels
{
    public class AddOrder2
    {
        public Order2 order { get; set; }
        public List<Unit> AvailableUnits { get; set; }
        public List<int> SelectedUnitsIDs { get; set; }

    }
}
