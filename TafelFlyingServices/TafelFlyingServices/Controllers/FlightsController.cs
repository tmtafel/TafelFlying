using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Geocoding;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    [Authorize]
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private FlightView CreateFlightView(Flight flight)
        {
            if (flight == null) return new FlightView();
            var fv = new FlightView
            {
                Id = flight.FlightId,
                TailNumber = flight.Plane.TailNumber
            };
            var arrival = new LocationDate
            {
                AirportCode = flight.ArrivalAirport.AirportId,
                Name = flight.ArrivalAirport.Name,
                City = flight.ArrivalAirport.City,
                State = flight.ArrivalAirport.StateOrProvidence,
                Latitude = flight.ArrivalAirport.Latitude,
                Longitude = flight.ArrivalAirport.Longitude,
                Date = flight.Arrival.ToShortDateString(),
                Time = flight.Arrival.ToShortTimeString()
            };

            fv.Arrival = arrival;

            var departure = new LocationDate
            {
                AirportCode = flight.DepartureAirport.AirportId,
                Name = flight.DepartureAirport.Name,
                City = flight.DepartureAirport.City,
                State = flight.DepartureAirport.StateOrProvidence,
                Latitude = flight.DepartureAirport.Latitude,
                Longitude = flight.DepartureAirport.Longitude,
                Date = flight.Departure.ToShortDateString(),
                Time = flight.Departure.ToShortTimeString()
            };

            fv.Departure = departure;


            if (flight.PilotInCommand != null)
                fv.Pic = flight.PilotInCommand.ApplicationUser.FirstName + " " +
                         flight.PilotInCommand.ApplicationUser.LastName;
            if (flight.SecondInCommand != null)
                fv.Sic = flight.SecondInCommand.ApplicationUser.FirstName + " " +
                         flight.SecondInCommand.ApplicationUser.LastName;
            if (flight.FlightNumber != null)
                fv.FlightNumber = flight.FlightNumber;
            return fv;
        }

        private Bounds CreateBounds(IEnumerable<FlightView> flightList)
        {
            var latitudes = new List<double>();
            var longitudes = new List<double>();
            foreach (FlightView flight in flightList)
            {
                double depatureLatitude =
                    _db.Airports.Where(f => f.AirportId == flight.Departure.AirportCode).Select(f => f).First().Latitude;
                double arrivalLatitude =
                    _db.Airports.Where(f => f.AirportId == flight.Arrival.AirportCode).Select(f => f).First().Latitude;
                if (!latitudes.Contains(depatureLatitude)) latitudes.Add(depatureLatitude);
                if (!latitudes.Contains(arrivalLatitude)) latitudes.Add(arrivalLatitude);

                double depatureLongitude =
                    _db.Airports.Where(f => f.AirportId == flight.Departure.AirportCode)
                        .Select(f => f)
                        .First()
                        .Longitude;
                double arrivalLongitude =
                    _db.Airports.Where(f => f.AirportId == flight.Arrival.AirportCode).Select(f => f).First().Longitude;
                if (!longitudes.Contains(depatureLongitude)) longitudes.Add(depatureLongitude);
                if (!longitudes.Contains(arrivalLongitude)) longitudes.Add(arrivalLongitude);
            }
            var bounds = new Bounds(33.942536, -118.408075, 40.639751, -73.778925);
            if (latitudes.Count == 0 && longitudes.Count == 0) return bounds;
            latitudes.Sort();
            longitudes.Sort();
            bounds = new Bounds(latitudes.First(), longitudes.First(), latitudes.Last(), longitudes.Last());

            return bounds;
        }

        // GET: Flights
        public ActionResult Index()
        {
            List<Flight> flightList = _db.Flights.ToList();
            flightList.Sort((x, y) => DateTime.Compare(x.Departure, y.Departure));
            return View(flightList);
        }

        //[HttpGet]
        //public JsonResult GetNextWeek(string dateString)
        //{
        //    var dSplit = dateString.Split('-');
        //    var date = new DateTime(Convert.ToInt32(dSplit[0]), Convert.ToInt32(dSplit[1]), Convert.ToInt32(dSplit[2]));
        //    var week = new Week();

        //    var sunday = new FlightDay { Day = date.AddDays(1) };
        //    var sundayFlights = _db.Flights.Where(f => f.DepartureDate.Year == sunday.Day.Year && f.DepartureDate.Month == sunday.Day.Month && f.DepartureDate.Day == sunday.Day.Day).Select(f => f).ToList();
        //    sunday.Flights = sundayFlights.Select(CreateFlightView).ToList();
        //    week.Sunday = sunday;

        //    var monday = new FlightDay { Day = date.AddDays(2) };
        //    var mondayFlights = _db.Flights.Where(f => f.DepartureDate.Year == monday.Day.Year && f.DepartureDate.Month == monday.Day.Month && f.DepartureDate.Day == monday.Day.Day).Select(f => f).ToList();
        //    monday.Flights = mondayFlights.Select(CreateFlightView).ToList();
        //    week.Monday = monday;

        //    var tuesday = new FlightDay { Day = date.AddDays(3) };
        //    var tuesdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == tuesday.Day.Year && f.DepartureDate.Month == tuesday.Day.Month && f.DepartureDate.Day == tuesday.Day.Day).Select(f => f).ToList();
        //    tuesday.Flights = tuesdayFlights.Select(CreateFlightView).ToList();
        //    week.Tuesday = tuesday;

        //    var wednesday = new FlightDay { Day = date.AddDays(4) };
        //    var wednesdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == wednesday.Day.Year && f.DepartureDate.Month == wednesday.Day.Month && f.DepartureDate.Day == wednesday.Day.Day).Select(f => f).ToList();
        //    wednesday.Flights = wednesdayFlights.Select(CreateFlightView).ToList();
        //    week.Wednesday = wednesday;

        //    var thursday = new FlightDay { Day = date.AddDays(5) };
        //    var thursdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == thursday.Day.Year && f.DepartureDate.Month == thursday.Day.Month && f.DepartureDate.Day == thursday.Day.Day).Select(f => f).ToList();
        //    thursday.Flights = thursdayFlights.Select(CreateFlightView).ToList();
        //    week.Thursday = thursday;

        //    var friday = new FlightDay { Day = date.AddDays(6) };
        //    var fridayFlights = _db.Flights.Where(f => f.DepartureDate.Year == friday.Day.Year && f.DepartureDate.Month == friday.Day.Month && f.DepartureDate.Day == friday.Day.Day).Select(f => f).ToList();
        //    friday.Flights = fridayFlights.Select(CreateFlightView).ToList();
        //    week.Friday = friday;

        //    var saturday = new FlightDay { Day = date.AddDays(7) };
        //    var saturdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == saturday.Day.Year && f.DepartureDate.Month == saturday.Day.Month && f.DepartureDate.Day == saturday.Day.Day).Select(f => f).ToList();
        //    saturday.Flights = saturdayFlights.Select(CreateFlightView).ToList();
        //    week.Saturday = saturday;

        //    return Json(week.ToJSON(), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetPrevWeek(string dateString)
        //{
        //    var dSplit = dateString.Split('-');
        //    var date = new DateTime(Convert.ToInt32(dSplit[0]), Convert.ToInt32(dSplit[1]), Convert.ToInt32(dSplit[2]));
        //    var week = new Week();

        //    var sunday = new FlightDay { Day = date.AddDays(-7) };
        //    var sundayFlights = _db.Flights.Where(f => f.DepartureDate.Year == sunday.Day.Year && f.DepartureDate.Month == sunday.Day.Month && f.DepartureDate.Day == sunday.Day.Day).Select(f => f).ToList();
        //    sunday.Flights = sundayFlights.Select(CreateFlightView).ToList();
        //    week.Sunday = sunday;

        //    var monday = new FlightDay { Day = date.AddDays(-6) };
        //    var mondayFlights = _db.Flights.Where(f => f.DepartureDate.Year == monday.Day.Year && f.DepartureDate.Month == monday.Day.Month && f.DepartureDate.Day == monday.Day.Day).Select(f => f).ToList();
        //    monday.Flights = mondayFlights.Select(CreateFlightView).ToList();
        //    week.Monday = monday;

        //    var tuesday = new FlightDay { Day = date.AddDays(-5) };
        //    var tuesdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == tuesday.Day.Year && f.DepartureDate.Month == tuesday.Day.Month && f.DepartureDate.Day == tuesday.Day.Day).Select(f => f).ToList();
        //    tuesday.Flights = tuesdayFlights.Select(CreateFlightView).ToList();
        //    week.Tuesday = tuesday;

        //    var wednesday = new FlightDay { Day = date.AddDays(-4) };
        //    var wednesdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == wednesday.Day.Year && f.DepartureDate.Month == wednesday.Day.Month && f.DepartureDate.Day == wednesday.Day.Day).Select(f => f).ToList();
        //    wednesday.Flights = wednesdayFlights.Select(CreateFlightView).ToList();
        //    week.Wednesday = wednesday;

        //    var thursday = new FlightDay { Day = date.AddDays(-3) };
        //    var thursdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == thursday.Day.Year && f.DepartureDate.Month == thursday.Day.Month && f.DepartureDate.Day == thursday.Day.Day).Select(f => f).ToList();
        //    thursday.Flights = thursdayFlights.Select(CreateFlightView).ToList();
        //    week.Thursday = thursday;

        //    var friday = new FlightDay { Day = date.AddDays(-2) };
        //    var fridayFlights = _db.Flights.Where(f => f.DepartureDate.Year == friday.Day.Year && f.DepartureDate.Month == friday.Day.Month && f.DepartureDate.Day == friday.Day.Day).Select(f => f).ToList();
        //    friday.Flights = fridayFlights.Select(CreateFlightView).ToList();
        //    week.Friday = friday;

        //    var saturday = new FlightDay { Day = date.AddDays(-1) };
        //    var saturdayFlights = _db.Flights.Where(f => f.DepartureDate.Year == saturday.Day.Year && f.DepartureDate.Month == saturday.Day.Month && f.DepartureDate.Day == saturday.Day.Day).Select(f => f).ToList();
        //    saturday.Flights = saturdayFlights.Select(CreateFlightView).ToList();
        //    week.Saturday = saturday;

        //    return Json(week.ToJSON(), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Calendar(int month, int year)
        {
            var dayOfMonth = new DateTime(year, month, 1);
            var flightCalendar = new FlightCalendar
            {
                Month = month,
                Year = year,
                MonthName = dayOfMonth.ToString("MMMM", CultureInfo.InvariantCulture),
                Days = new List<FlightDay>()
            };
            while (dayOfMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                dayOfMonth = dayOfMonth.AddDays(-1);
            }
            while (dayOfMonth.Month < month + 1)
            {
                var flightDay = new FlightDay
                {
                    Day = dayOfMonth,
                    Flights = new List<FlightView>()
                };
                flightCalendar.Days.Add(flightDay);
                dayOfMonth = dayOfMonth.AddDays(1);
            }
            dayOfMonth = dayOfMonth.AddDays(-1);
            while (dayOfMonth.DayOfWeek != DayOfWeek.Saturday)
            {
                dayOfMonth = dayOfMonth.AddDays(1);
                var flightDay = new FlightDay
                {
                    Day = dayOfMonth,
                    Flights = new List<FlightView>()
                };
                flightCalendar.Days.Add(flightDay);
            }
            foreach (FlightDay day in flightCalendar.Days)
            {
                foreach (Flight f in _db.Flights)
                {
                    if ((f.Departure.Year == day.Day.Date.Year) &&
                        (f.Departure.Month == day.Day.Date.Month) &&
                        (f.Departure.Day == day.Day.Date.Day))
                    {
                        FlightView flightView = CreateFlightView(f);
                        day.Flights.Add(flightView);
                    }
                }
            }

            return View(flightCalendar);
        }

        public ActionResult Day(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var flightList = new List<FlightView>();

            flightList.AddRange(from flight in _db.Flights.ToList()
                where flight.Departure.ToShortDateString() == date.ToShortDateString()
                select CreateFlightView(flight));


            flightList.Sort(
                (x, y) =>
                    DateTime.Compare(Convert.ToDateTime(x.Departure.Date + " " + x.Departure.Time),
                        Convert.ToDateTime(y.Departure.Date + " " + y.Departure.Time)));

            Bounds bounds = CreateBounds(flightList);

            var flightDay = new FlightDay
            {
                Day = date,
                Flights = flightList,
                Bounds = bounds
            };


            ViewBag.MonthName = flightDay.Day.ToString("MMMM", CultureInfo.InvariantCulture);
            return View(flightDay);
        }


        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = _db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET: Flights/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditFlight flight)
        {
            if (!ModelState.IsValid) return View(flight);
            DateTime arrival =
                flight.DepartureDate.AddHours(flight.ArrivalTime.Hour).AddMinutes(flight.ArrivalTime.Minute);
            if (flight.ArrivalTime.TimeOfDay <= flight.DepatureTime.TimeOfDay)
            {
                arrival =
                    flight.DepartureDate.AddDays(1)
                        .AddHours(flight.ArrivalTime.Hour)
                        .AddMinutes(flight.ArrivalTime.Minute);
            }
            var newFlight = new Flight
            {
                Departure =
                    flight.DepartureDate.AddHours(flight.DepatureTime.Hour).AddMinutes(flight.DepatureTime.Minute),
                Arrival = arrival,
                Plane = _db.Planes.Find(flight.TailNumber),
                FlightNumber = flight.FlightNumber,
                PilotInCommand = _db.Pilots.Find(flight.PilotInCommandId),
                SecondInCommand = _db.Pilots.Find(flight.SecondInCommandId),
                DepartureAirport = _db.Airports.Find(flight.DepartureAirportId),
                ArrivalAirport = _db.Airports.Find(flight.ArrivalAirportId)
            };
            _db.Flights.Add(newFlight);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = _db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            var editFlight = new EditFlight();

            editFlight.FlightId = flight.FlightId;

            editFlight.TailNumber = flight.Plane.TailNumber;
            editFlight.Color = flight.Plane.Color;
            editFlight.DepatureTime = flight.Departure;
            editFlight.DepartureDate = flight.Departure;
            editFlight.ArrivalTime = flight.Arrival;

            editFlight.DepartureAirportId = flight.DepartureAirport.AirportId;
            editFlight.ArrivalAirportId = flight.ArrivalAirport.AirportId;

            if (flight.PilotInCommand != null)
            {
                editFlight.PilotInCommandId = flight.PilotInCommand.PilotId;
            }
            if (flight.SecondInCommand != null)
            {
                editFlight.SecondInCommandId = flight.SecondInCommand.PilotId;
            }
            if (flight.FlightNumber != null)
            {
                editFlight.FlightNumber = flight.FlightNumber;
            }

            editFlight.BlackText = flight.Plane.BlackText ? "#000000" : "#FFFFFF";
            return View(editFlight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditFlight flight)
        {
            if (!ModelState.IsValid) return View(flight);
            Flight editFlight = _db.Flights.Find(flight.FlightId);
            DateTime arrival =
                flight.DepartureDate.AddHours(flight.ArrivalTime.Hour).AddMinutes(flight.ArrivalTime.Minute);
            if (flight.ArrivalTime.TimeOfDay <= flight.DepatureTime.TimeOfDay)
            {
                arrival =
                    flight.DepartureDate.AddDays(1)
                        .AddHours(flight.ArrivalTime.Hour)
                        .AddMinutes(flight.ArrivalTime.Minute);
            }
            editFlight.Departure =
                flight.DepartureDate.AddHours(flight.DepatureTime.Hour).AddMinutes(flight.DepatureTime.Minute);
            editFlight.Arrival = arrival;

            editFlight.Plane = _db.Planes.Find(flight.TailNumber);
            editFlight.FlightNumber = flight.FlightNumber;
            editFlight.PilotInCommand = _db.Pilots.Find(flight.PilotInCommandId);
            editFlight.SecondInCommand = _db.Pilots.Find(flight.SecondInCommandId);
            editFlight.DepartureAirport = _db.Airports.Find(flight.DepartureAirportId);
            editFlight.ArrivalAirport = _db.Airports.Find(flight.ArrivalAirportId);

            _db.Entry(editFlight).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = _db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = _db.Flights.Find(id);
            _db.Flights.Remove(flight);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}