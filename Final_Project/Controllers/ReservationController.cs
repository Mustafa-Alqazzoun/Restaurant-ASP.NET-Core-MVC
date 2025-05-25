using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_Project.Models;
using Final_Project.ViewModels;
using System.Device.Location;

namespace Final_Project.Controllers
{
    public class ReservationController : Controller
    {
        private readonly RestaurantContext _context;

        public ReservationController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowAllReservations()
        {
            var reservations = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Select(r => new MakeReservation
                {
                    ReservationID = r.ReservationID,
                    CustomerID = r.CustomerID,
                    ReservationTime = r.ReservationTime,
                    NumberOfGuests = r.NumberOfGuests,
                    TableID = r.TableID,
                    Table = r.Table,
                    IsIndoor = r.Table.Location == "Indoor",
                    Status = r.Status
                })
                .ToList();

            return View(reservations);
        }

        [HttpGet]
        public IActionResult AddReservation()
        {
            var model = new MakeReservation
            {
                AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available")
                    .ToList(),
                ReservationTime = DateTime.Now.AddHours(1)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReservation(MakeReservation model)
        {
            System.Diagnostics.Debug.WriteLine($"Input: CustomerID={model.CustomerID}, ReservationTime={model.ReservationTime}, NumberOfGuests={model.NumberOfGuests}, IsIndoor={model.IsIndoor}");

            if (model.ReservationTime <= DateTime.Now)
            {
                ModelState.AddModelError("ReservationTime", "Reservation time must be in the future.");
            }

            if (!_context.Customers.Any(c => c.CustomerID == model.CustomerID))
            {
                ModelState.AddModelError("CustomerID", "Invalid Customer ID. Customer does not exist.");
            }

            //if (model.BranchID.HasValue && !_context.Branches.Any(b => b.BranchID == model.BranchID))
            //{
            //    ModelState.AddModelError("BranchID", "Invalid Branch ID. Branch does not exist.");
            //}

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                System.Diagnostics.Debug.WriteLine($"ModelState errors: {string.Join("; ", errors)}");
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available")
                    .ToList();
                return View(model);
            }

            var location = model.IsIndoor ? "Indoor" : "Outdoor";
            var suitableTable = _context.Tables
                .Where(t => t.Status == "Available"
                    && t.Location == location
                    && t.Capacity >= model.NumberOfGuests)
                //&& t.BranchID == model.BranchID)
                .OrderBy(t => t.Capacity)
                .FirstOrDefault();

            if (suitableTable == null)
            {
                ModelState.AddModelError("", $"No suitable table available for {model.NumberOfGuests} guests at {location} location.");
                System.Diagnostics.Debug.WriteLine($"No table found for {model.NumberOfGuests} guests at {location}");
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available")
                    .ToList();
                return View(model);
            }

            var reservation = new Reservation
            {
                CustomerID = model.CustomerID,
                ReservationTime = model.ReservationTime,
                NumberOfGuests = model.NumberOfGuests,
                TableID = suitableTable.TableID,
                Status = "Confirmed",
                //BranchID = model.BranchID
            };

            suitableTable.Status = "Reserved";

            try
            {
                _context.Reservations.Add(reservation);
                _context.Tables.Update(suitableTable);
                _context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Reservation created: ID={reservation.ReservationID}, TableID={suitableTable.TableID}");
                return RedirectToAction("ShowAllReservations");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while saving the reservation: {ex.Message}");
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available")
                    .ToList();
                return View(model);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditReservation(int id, MakeReservation model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available" || t.TableID == model.TableID)
                    .ToList();
                return View(model);
            }

            var reservation = _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var originalTableId = reservation.TableID;

            var location = model.IsIndoor ? "Indoor" : "Outdoor";
            var suitableTable = _context.Tables
                .Where(t => (t.Status == "Available" || t.TableID == reservation.TableID)
                    && t.Location == location
                    && t.Capacity >= model.NumberOfGuests)
                .OrderBy(t => t.Capacity)
                .FirstOrDefault();

            if (suitableTable == null)
            {
                ModelState.AddModelError("", "No suitable table available for the selected criteria.");
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available" || t.TableID == model.TableID)
                    .ToList();
                return View(model);
            }

            reservation.CustomerID = model.CustomerID;
            reservation.ReservationTime = model.ReservationTime;
            reservation.NumberOfGuests = model.NumberOfGuests;
            reservation.TableID = suitableTable.TableID;
            reservation.Status = model.Status;

            try
            {
                if (originalTableId != suitableTable.TableID)
                {
                    var originalTable = _context.Tables.Find(originalTableId);
                    if (originalTable != null)
                    {
                        originalTable.Status = "Available";
                        _context.Tables.Update(originalTable);
                    }
                    suitableTable.Status = "Reserved";
                    _context.Tables.Update(suitableTable);
                }

                if (model.Status == "Pending")
                {
                    suitableTable.Status = "Available";
                    _context.Tables.Update(suitableTable);
                }

                _context.Reservations.Update(reservation);
                _context.SaveChanges();
                return RedirectToAction("ShowAllReservations");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while updating the reservation: {ex.Message}");
                model.AvailableTables = _context.Tables
                    .Where(t => t.Status == "Available" || t.TableID == model.TableID)
                    .ToList();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelReservation(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.Status = "Pending";
            reservation.Table.Status = "Available";

            try
            {
                _context.Reservations.Update(reservation);
                _context.Tables.Update(reservation.Table);
                _context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Reservation cancelled: ID={reservation.ReservationID}, TableID={reservation.TableID}");
                return RedirectToAction("ShowAllReservations");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while cancelling the reservation: {ex.Message}");
                return RedirectToAction("ShowAllReservations");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var table = reservation.Table;
            table.Status = "Available";

            try
            {
                _context.Reservations.Remove(reservation);
                _context.Tables.Update(table);
                _context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Reservation deleted: ID={id}, TableID={table.TableID}");
                return RedirectToAction("ShowAllReservations");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while deleting the reservation: {ex.Message}");
                return RedirectToAction("ShowAllReservations");
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Select(r => new MakeReservation
                {
                    ReservationID = r.ReservationID,
                    CustomerID = r.CustomerID,
                    ReservationTime = r.ReservationTime,
                    NumberOfGuests = r.NumberOfGuests,
                    TableID = r.TableID,
                    Table = r.Table,
                    IsIndoor = r.Table.Location == "Indoor",
                    Status = r.Status
                })
                .FirstOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View("Details", reservation);
        }

        [HttpGet]
        public IActionResult FindNearestBranch()
        {
            var branches = _context.Branches
                .Select(b => new BranchViewModel
                {
                    BranchID = b.BranchID,
                    Name = b.Name,
                    Address = b.Address,
                    ContactNumber = b.ContactNumber,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude
                })
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Branches loaded: {branches.Count}");
            foreach (var branch in branches)
            {
                System.Diagnostics.Debug.WriteLine($"Branch: {branch.Name}, Lat: {branch.Latitude}, Lon: {branch.Longitude}");
            }

            var model = new NearestBranchViewModel
            {
                Branches = branches
            };

            return View("FindNearestBranch", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            try
            {
                var coord1 = new GeoCoordinate(lat1, lon1);
                var coord2 = new GeoCoordinate(lat2, lon2);
                double distance = coord1.GetDistanceTo(coord2) / 1000; // Convert meters to kilometers
                return Json(distance);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CalculateDistance error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Error calculating distance: " + ex.Message);
            }
        }

        //coupon
        public string GenerateCouponCode()
        {
            return "SAVE" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }

    public class BranchViewModel
    {
        public int BranchID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class NearestBranchViewModel
    {
        public List<BranchViewModel> Branches { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public BranchViewModel NearestBranch { get; set; }
    }
}