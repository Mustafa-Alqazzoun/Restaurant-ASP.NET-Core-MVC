using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Final_Project.Models;

namespace Final_Project.ViewModels
{
    public class MakeReservation
    {
        public int ReservationID { get; set; }
        [Required]
        public int? CustomerID { get; set; }
        [Required]
        //public int? BranchID { get; set; }
        public DateTime ReservationTime { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1.")]
        public int NumberOfGuests { get; set; }
        public Table? Table { get; set; } // Nullable to avoid validation
        [Required]
        public bool IsIndoor { get; set; }
        [Required]
        public int TableID { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
        public List<Table>? AvailableTables { get; set; } // Nullable to avoid validation
    }
}