using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly RestaurantContext context;

        public BranchRepository()
        {
            context = new RestaurantContext();
        }

        public List<Branch> GetAll(string includes = "")
        {
            IQueryable<Branch> query = context.Branches;
            if (!string.IsNullOrEmpty(includes))
            {
                if (includes.Contains("Staff"))
                {
                    query = query.Include(b => b.Staff);
                }
                if (includes.Contains("Tables"))
                {
                    query = query.Include(b => b.Tables);
                }
                if (includes.Contains("Orders"))
                {
                    query = query.Include(b => b.Orders);
                }
                if (includes.Contains("Reservations"))
                {
                    query = query.Include(b => b.Reservations);
                }
            }
            return query.ToList();
        }

        public Branch GetById(int id)
        {
            return context.Branches.FirstOrDefault(b => b.BranchID == id);
        }

        public void Add(Branch obj)
        {
            context.Branches.Add(obj);
        }

        public void Update(Branch obj)
        {
            context.Branches.Update(obj);
        }

        public void Delete(int id)
        {
            var branch = GetById(id);
            if (branch != null)
            {
                context.Branches.Remove(branch);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        // Additional methods specific to branches
        public List<Branch> GetBranchesByLocation(double latitude, double longitude, double radiusKm)
        {
            // Since Latitude and Longitude are now non-nullable doubles, no need to check HasValue
            return context.Branches
                .Where(b =>
                    Math.Sqrt(Math.Pow(b.Latitude - latitude, 2) + Math.Pow(b.Longitude - longitude, 2)) <= radiusKm / 111.0
                )
                .ToList();
        }

        public Branch GetBranchByManager(int managerId)
        {
            return context.Branches.FirstOrDefault(b => b.ManagerID == managerId);
        }

        // Additional helper method to get branches with coordinates (all branches now have coordinates)
        public List<Branch> GetBranchesWithCoordinates()
        {
            return context.Branches
                .Where(b => b.Latitude != 0 && b.Longitude != 0) // Check for non-zero values instead
                .ToList();
        }

        // Helper method to get branches without coordinates (check for zero values)
        public List<Branch> GetBranchesWithoutCoordinates()
        {
            return context.Branches
                .Where(b => b.Latitude == 0 || b.Longitude == 0)
                .ToList();
        }
    }
}