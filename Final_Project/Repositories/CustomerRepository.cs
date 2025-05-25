using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantContext context;

        public CustomerRepository()
        {
            context = new RestaurantContext();
        }

        public List<Customer> GetAll(string includes = "")
        {
            IQueryable<Customer> query = context.Customers;

            if (!string.IsNullOrEmpty(includes))
            {
                if (includes.Contains("Orders"))
                {
                    query = query.Include(c => c.Orders);
                }
                if (includes.Contains("Reservations"))
                {
                    query = query.Include(c => c.Reservations);
                }
                if (includes.Contains("Profile"))
                {
                    query = query.Include(c => c.Profile);
                }
            }

            return query.ToList();
        }

        public Customer GetById(int id)
        {
            return context.Customers.FirstOrDefault(c => c.CustomerID == id);
        }

        public Customer GetByContactNumber(string contactNumber)
        {
            return context.Customers.FirstOrDefault(c => c.ContactNumber == contactNumber);
        }

        public void Add(Customer obj)
        {
            context.Customers.Add(obj);
        }

        public void Update(Customer obj)
        {
            context.Customers.Update(obj);
        }

        public void Delete(int id)
        {
            var customer = GetById(id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}