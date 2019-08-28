using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Geocoding;

namespace TafelFlyingServices.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Display(Name = "Flight Number")]
        public int? FlightNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        [Display(Name = "Depart")]
        public virtual Airport DepartureAirport { get; set; }

        [Display(Name = "Arrive")]
        public virtual Airport ArrivalAirport { get; set; }

        [Display(Name = "First In Command")]
        public virtual Pilot PilotInCommand { get; set; }

        [Display(Name = "Second In Command")]
        public virtual Pilot SecondInCommand { get; set; }

        public virtual Plane Plane { get; set; }


        [Display(Name = "Invoice")]
        public int? TripId { get; set; }
    }

    public class EditFlight
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Leaving On?")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Leaving When?")]
        [DataType(DataType.DateTime)]
        public DateTime DepatureTime { get; set; }

        [Required(ErrorMessage = "Arriving At?")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Departing From?")]
        [Display(Name = "Departure")]
        public string DepartureAirportId { get; set; }

        [Required(ErrorMessage = "Arriving To?")]
        [Display(Name = "Arrival")]
        public string ArrivalAirportId { get; set; }

        [Display(Name = "First In Command")]
        public int? PilotInCommandId { get; set; }

        [Display(Name = "Second In Command")]
        public int? SecondInCommandId { get; set; }

        [Required(ErrorMessage = "Select A Plane")]
        [Display(Name = "Tail Number")]
        public string TailNumber { get; set; }

        [Display(Name = "Flight Number")]
        public int? FlightNumber { get; set; }

        public string Color { get; set; }

        public string BlackText { get; set; }
    }


    public class FlightView
    {
        public int Id { get; set; }

        [Display(Name = "Flight Number")]
        public int? FlightNumber { get; set; }

        [Display(Name = "Depart")]
        public LocationDate Departure { get; set; }

        [Display(Name = "Arrive")]
        public LocationDate Arrival { get; set; }

        [Display(Name = "First In Command")]
        public string Pic { get; set; }


        [Display(Name = "Second In Command")]
        public string Sic { get; set; }

        [Display(Name = "Tail Number")]
        public string TailNumber { get; set; }

        public string Color
        {
            get
            {
                var db = new ApplicationDbContext();
                return db.Planes.Find(TailNumber).Color;
            }
        }


        [Display(Name = "Black Text")]
        public bool BlackText
        {
            get
            {
                var db = new ApplicationDbContext();
                return db.Planes.Find(TailNumber).BlackText;
            }
        }
    }

    public class LocationDate
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AirportCode { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class FlightCalendar
    {
        public List<FlightDay> Days { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class FlightDay
    {
        public List<FlightView> Flights { get; set; }
        public DateTime Day { get; set; }
        public Bounds Bounds { get; set; }
    }
}