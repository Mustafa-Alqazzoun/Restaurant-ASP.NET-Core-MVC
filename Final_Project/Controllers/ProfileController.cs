using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly RestaurantContext _context;

        public ProfileController(RestaurantContext context)
        {
            _context = context;
        }

        private int? GetLoggedInCustomerId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int customerId))
                return customerId;

            return null;
        }

        //index
        [HttpGet]
        public IActionResult Index()
        {
            int? customerId = GetLoggedInCustomerId();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var customer = _context.Customers
                .Include(c => c.Profile)
                .Include(c => c.Reservations)
                .FirstOrDefault(c => c.CustomerID == customerId.Value);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            return View(customer);
        }

        [HttpPost]
        public IActionResult SaveNew(Profile profileFromReq)
        {
            if (ModelState.IsValid)
            {
                profileFromReq.RegisteredDate = DateTime.Now;
                profileFromReq.Gender ??= "NotSpecified";
                _context.Profiles.Add(profileFromReq);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("New", profileFromReq);
        }



        // GET: Profile/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var profile = _context.Profiles
                .Include(p => p.Customer)   // Include related Customer data
                .FirstOrDefault(p => p.ProfileID == id);

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Profile profile)
        {
            if (id != profile.ProfileID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var profileToUpdate = _context.Profiles
                    .Include(p => p.Customer)
                    .FirstOrDefault(p => p.ProfileID == id);

                if (profileToUpdate == null)
                    return NotFound();

                // Update Profile fields
                profileToUpdate.Username = profile.Username;
                profileToUpdate.Gender = profile.Gender;

                // Update Customer fields
                if (profileToUpdate.Customer != null)
                {
                    profileToUpdate.Customer.Name = profile.Customer?.Name; 
                    profileToUpdate.Customer.Email = profile.Customer?.Email;
                }

                try
                {
                    _context.Update(profileToUpdate);  
                    _context.SaveChanges();

                    return RedirectToAction("Details", new { id = profileToUpdate.ProfileID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Profiles.Any(e => e.ProfileID == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(profile);
        }







        //Details

        public IActionResult Details(int id)
        {
            var profile = _context.Profiles
                .Include(p => p.Customer)
                 .Include(p => p.Coupons)
                .FirstOrDefault(p => p.ProfileID == id);

            if (profile == null)
                return NotFound();

            return View("Details", profile);
        }



        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var profile = _context.Profiles
                .Include(p => p.Customer)
                .FirstOrDefault(p => p.ProfileID == id);

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var profile = _context.Profiles
                .Include(p => p.Customer)
                .ThenInclude(c => c.Reservations)
                .FirstOrDefault(p => p.ProfileID == id);

            if (profile != null)
            {
                // Remove reservations
                if (profile.Customer?.Reservations != null)
                {
                    _context.Reservations.RemoveRange(profile.Customer.Reservations);
                }

                // Remove customer
                if (profile.Customer != null)
                {
                    _context.Customers.Remove(profile.Customer);
                }

                // Remove profile
                _context.Profiles.Remove(profile);

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    


        //Image
        [HttpPost]
        public async Task<IActionResult> UploadImage(int ProfileID, IFormFile ImageFile)
        {
            var profile = await _context.Profiles.FindAsync(ProfileID);
            if (profile == null || ImageFile == null || ImageFile.Length == 0)
                return RedirectToAction("Index");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            profile.ProfilePictureUrl = "/uploads/" + fileName;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
