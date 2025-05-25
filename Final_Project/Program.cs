using Final_Project.Models;
using Final_Project.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register Entity Framework DbContext
            builder.Services.AddDbContext<RestaurantContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            // Add Authentication services
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            // Register Repository interfaces with their implementations
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
            builder.Services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            //builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            //builder.Services.AddScoped<IStaffRepository, StaffRepository>();
            //builder.Services.AddScoped<ITableRepository, TableRepository>();
            //builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            //builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            //builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
            //builder.Services.AddScoped<INutritionRepository, NutritionRepository>();
            //builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}