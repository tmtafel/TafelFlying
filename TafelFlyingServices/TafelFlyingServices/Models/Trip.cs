using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TafelFlyingServices.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        [Display(Name = "Invoice")]
        [DataType(DataType.PhoneNumber)]
        public int InvoiceNumber { get; set; }

        public List<Expense> Expenses { get; set; }

        [Display(Name = "Expenses")]
        public decimal? TotalExpenses
        {
            get
            {
                var db = new ApplicationDbContext();
                List<Expense> expenses = db.Expenses.ToList();
                return expenses.Where(expense => expense.TripId == TripId)
                    .Aggregate<Expense, decimal>(0, (current, expense) => current + expense.Amount);
            }
        }

        [Display(Name = "Pilot Fee")]
        [DataType(DataType.Currency)]
        public decimal? PilotFee { get; set; }

        [Display(Name = "Co-Pilot Fee")]
        [DataType(DataType.Currency)]
        public decimal? CoPilotFee { get; set; }

        public bool Paid { get; set; }

        public bool IsOver
        {
            get { return Flights != null && Flights.Any(flight => flight.Arrival.Date < DateTime.Now.Date); }
        }

        [Required]
        public string TailNumber { get; set; }

        public Plane Plane
        {
            get
            {
                var db = new ApplicationDbContext();
                List<Plane> planes = db.Planes.ToList();
                return planes.Where(p => p.TailNumber == TailNumber).Select(p => p).First();
            }
        }


        public List<Flight> Flights { get; set; }
        public int[] SelectedFlightIds { get; set; }

        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public decimal? TotalCost
        {
            get
            {
                decimal pFee = 0;
                decimal cFee = 0;
                decimal totExpenses = 0;
                if (PilotFee != null) pFee = PilotFee.Value;
                if (PilotFee != null) cFee = CoPilotFee.Value;
                if (TotalExpenses != null) totExpenses = TotalExpenses.Value;
                return totExpenses + pFee + cFee;
            }
        }
    }

    public class TripViewer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public object Tags { get; set; }
    }

    public class TripViewModel
    {
        [Key]
        public int TripId { get; set; }

        [Display(Name = "Invoice")]
        public int InvoiceNumber { get; set; }

        public List<Flight> Flights { get; set; }
        public List<Expense> Expenses { get; set; }

        public decimal? PilotFee { get; set; }
        public decimal? CoPilotFee { get; set; }

        public bool Paid { get; set; }

        public bool IsOver
        {
            get { return Flights != null && Flights.Any(flight => flight.Arrival.Date < DateTime.Now.Date); }
        }

        [Required]
        public string TailNumber { get; set; }

        public Plane Plane
        {
            get
            {
                var db = new ApplicationDbContext();
                List<Plane> planes = db.Planes.ToList();
                return planes.Where(p => p.TailNumber == TailNumber).Select(p => p).First();
            }
        }

        [Display(Name = "Expenses")]
        public decimal? TotalExpenses
        {
            get
            {
                var db = new ApplicationDbContext();
                List<Expense> expenses = db.Expenses.ToList();
                return expenses.Where(expense => expense.TripId == TripId)
                    .Aggregate<Expense, decimal>(0, (current, expense) => current + expense.Amount);
            }
        }

        public int[] SelectedFlightIds { get; set; }
        public IList<TripViewer> AvailableFlights { get; set; }
        public IList<TripViewer> SelectedFlights { get; set; }
    }
}