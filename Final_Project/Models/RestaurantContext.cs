using Microsoft.EntityFrameworkCore;
using Final_Project.Models;

namespace Final_Project.Models
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order2> Orders_2 { get; set; }
        public DbSet<CustomizedOrder> CustomOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order2> Order2s { get; set; }
        public DbSet<CustomizedOrder> CustomizedOrders { get; set; }



        public RestaurantContext() : base()
        {
        }

        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-5GF48P6\\SQLEXPRESS;Initial Catalog=Restaurant;Integrated Security=True;Encrypt=False");
            }
        }
     
    }
}
        

    



