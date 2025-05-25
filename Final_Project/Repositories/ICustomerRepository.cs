using Final_Project.Models;

namespace Final_Project.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Additional methods specific to customers
        Customer GetByContactNumber(string contactNumber);
    }
}