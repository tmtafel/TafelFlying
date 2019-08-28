using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private Trip FindTripByInvoiceNumber(int? invoiceNumber)
        {
            List<Trip> trips = _db.Trips.ToList();
            Trip trip = (from t in trips where t.InvoiceNumber == invoiceNumber select t).FirstOrDefault();
            return trip;
        }

        // GET: Trips
        public ActionResult Index()
        {
            IEnumerable<Trip> trips = _db.Trips.ToList().Select(t => t).Where(t => !t.IsOver || !t.Paid);
            foreach (Trip trip in trips)
            {
                trip.SelectedFlightIds =
                    _db.Flights.Where(f => f.TripId == trip.TripId).Select(f => f.FlightId).ToArray();
            }
            return View(trips);
        }

        public ActionResult PastTrips()
        {
            IEnumerable<Trip> trips = _db.Trips.ToList().Select(t => t).Where(t => t.IsOver && t.Paid);

            return View(trips);
        }

        // GET: Trips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Trip trip = FindTripByInvoiceNumber(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            trip.Expenses = _db.Expenses.Select(e => e).Where(e => e.TripId == trip.TripId).ToList();
            trip.Flights = _db.Flights.Where(f => f.TripId == trip.TripId).Select(f => f).ToList();

            return View(trip);
        }

        // GET: Trips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trip trip)
        {
            if (ModelState.IsValid)
            {
                _db.Trips.Add(trip);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trip);
        }

        // GET: Trips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = FindTripByInvoiceNumber(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            var tvm = new TripViewModel
            {
                AvailableFlights = new List<TripViewer>(),
                SelectedFlights = new List<TripViewer>(),
                CoPilotFee = trip.CoPilotFee,
                InvoiceNumber = trip.InvoiceNumber,
                Paid = trip.Paid,
                PilotFee = trip.PilotFee,
                TailNumber = trip.TailNumber,
                TripId = trip.TripId
            };

            tvm.SelectedFlightIds = (from f in _db.Flights where f.TripId == tvm.TripId select f.FlightId).ToArray();
            if (tvm.Expenses == null) tvm.Expenses = new List<Expense>();
            foreach (Expense e in _db.Expenses.ToList().Where(e => e.TripId == trip.TripId))
            {
                tvm.Expenses.Add(e);
            }
            if (tvm.Flights == null) tvm.Flights = new List<Flight>();
            foreach (Flight f in _db.Flights.ToList().Where(f => f.TripId == trip.TripId))
            {
                tvm.Flights.Add(f);
            }

            var availableFlights = new List<TripViewer>();
            var selectedFlights = new List<TripViewer>();
            foreach (Flight flight in _db.Flights.ToList())
            {
                if (flight.Plane.TailNumber != tvm.TailNumber) continue;
                bool contains = tvm.SelectedFlightIds != null && tvm.SelectedFlightIds.Contains(flight.FlightId);
                if (flight.TripId != null && !contains) continue;
                bool isSelected = false;
                if (tvm.SelectedFlightIds != null)
                {
                    isSelected = tvm.SelectedFlightIds.Contains(flight.FlightId);
                }
                var tripViewer = new TripViewer
                {
                    Id = flight.FlightId,
                    IsSelected = isSelected,
                    Name =
                        flight.Plane.TailNumber + " (" + flight.DepartureAirport.AirportId + "-" +
                        flight.ArrivalAirport.AirportId + ")"
                };
                availableFlights.Add(tripViewer);
                if (tripViewer.IsSelected)
                {
                    selectedFlights.Add(tripViewer);
                }
            }
            tvm.SelectedFlights = selectedFlights;
            tvm.AvailableFlights = availableFlights;

            return View(tvm);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TripViewModel trip)
        {
            if (!ModelState.IsValid) return View(trip);
            if (trip.SelectedFlightIds == null) trip.SelectedFlightIds = new List<int>().ToArray();

            Trip dbTrip = FindTripByInvoiceNumber(trip.InvoiceNumber);

            dbTrip.Paid = trip.Paid;
            dbTrip.PilotFee = trip.PilotFee;
            dbTrip.CoPilotFee = trip.CoPilotFee;
            dbTrip.TailNumber = trip.TailNumber;
            foreach (
                Expense e in _db.Expenses.ToList().Where(e => e.TripId == dbTrip.TripId && !dbTrip.Expenses.Contains(e))
                )
            {
                dbTrip.Expenses.Add(e);
            }

            foreach (Flight f in _db.Flights.Where(f => f.TripId == dbTrip.TripId).ToList())
            {
                if (!trip.SelectedFlightIds.Contains(f.FlightId))
                {
                    f.TripId = null;
                }
            }

            foreach (int flightId in trip.SelectedFlightIds)
            {
                _db.Flights.ToList().Where(f => f.FlightId == flightId).Select(f => f).First().TripId = trip.TripId;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Trips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = _db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IEnumerable<Flight> flights = _db.Flights.ToList().Where(f => f.TripId == id).Select(f => f);
            foreach (Flight flight in flights)
            {
                flight.TripId = null;
                _db.SaveChanges();
            }
            Trip trip = _db.Trips.Find(id);
            _db.Trips.Remove(trip);
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