using Final_Project.Models;

namespace Final_Project.Repositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        // Additional methods specific to branches
        List<Branch> GetBranchesByLocation(double latitude, double longitude, double radiusKm);
        Branch GetBranchByManager(int managerId);
    }
}
