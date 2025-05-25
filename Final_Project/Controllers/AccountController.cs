using Final_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly RestaurantContext _context;
        private readonly PasswordHasher<Profile> _passwordHasher;

        public AccountController(RestaurantContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Profile>();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string username = model.Username?.Trim();

            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Username is required.");
                return View(model);
            }

            bool usernameExists = await _context.Profiles.AnyAsync(p => p.Username == username);
            if (usernameExists)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(model);
            }

            var customer = new Customer
            {
                Name = model.Name,
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                Address = model.Address
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var profile = new Profile
            {
                Username = username,
                RegisteredDate = DateTime.Now,
                CustomerID = customer.CustomerID,
                Gender = model.Gender ?? "NotSpecified",
                DateOfBirth = model.DateOfBirth
            };

            profile.Password = _passwordHasher.HashPassword(profile, model.Password);

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string username = model.Username?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Username is required.");
                return View(model);
            }

            var profile = await _context.Profiles
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Username == username);

            if (profile == null ||
                _passwordHasher.VerifyHashedPassword(profile, profile.Password, model.Password) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, profile.Username),
                new Claim(ClaimTypes.NameIdentifier, profile.CustomerID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Profile");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
