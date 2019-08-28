using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Geocoding;
using Newtonsoft.Json;
using TafelFlyingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TafelFlyingServices.Controllers
{
    [Authorize]
    public class AirportsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private IEnumerable<string[]> GetAirportStringArrayAllAirports()
        {
            string filePath = Server.MapPath(Url.Content("~/App_Data/airports.dat"));
            var airports = new List<string[]>();
            if (!System.IO.File.Exists(filePath))
            {
                return airports;
            }
            using (StreamReader sr = System.IO.File.OpenText(filePath))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    string[] airport = s.Split(',');
                    for (int i = 0; i < airport.Length; i++)
                    {
                        airport[i] = airport[i].Trim('\"');
                        if (airport[i] == "\\N")
                        {
                            airport[i] = "";
                        }
                    }
                    if (airport.Length == 12)
                        airports.Add(airport);
                }
            }
            return airports;
        }

        private IEnumerable<string> GetCountries()
        {
            IEnumerable<string[]> airports = GetAirportStringArrayAllAirports();
            var listCountries = new List<string>();
            foreach (var airport in airports.Where(airport => !listCountries.Contains(airport[3])))
            {
                listCountries.Add(airport[3]);
            }
            listCountries.Sort();
            return listCountries;
        }

        private List<Airport> GetAirports(string country = null)
        {
            IEnumerable<string[]> airports = GetAirportStringArrayAllAirports();
            var airportList = new List<Airport>();
            if (country == null)
            {
                airportList.AddRange(airports.Select(a => new Airport
                {
                    AirportId = a[5],
                    Altitude = int.Parse(a[8]),
                    City = a[2],
                    Country = a[3],
                    DST = a[10],
                    Latitude = double.Parse(a[6]),
                    Longitude = double.Parse(a[7]),
                    Name = a[1],
                    TimezoneName = a[11]
                }));
                return airportList;
            }
            airportList.AddRange(from a in airports
                where a[3] == country
                select new Airport
                {
                    AirportId = a[5],
                    Altitude = int.Parse(a[8]),
                    City = a[2],
                    Country = a[3],
                    DST = a[10],
                    Latitude = double.Parse(a[6]),
                    Longitude = double.Parse(a[7]),
                    Name = a[1],
                    TimezoneName = a[11]
                });
            return airportList;
        }

        private Airport GetAirport(string id)
        {
            IEnumerable<string[]> airports = GetAirportStringArrayAllAirports();
            var airport = new Airport();
            foreach (var a in airports.Where(a => a[5] == id))
            {
                airport.AirportId = a[5];
                airport.Altitude = int.Parse(a[8]);
                airport.City = a[2];
                airport.Country = a[3];
                airport.DST = a[10];
                ;
                airport.Latitude = double.Parse(a[6]);
                airport.Longitude = double.Parse(a[7]);
                airport.Name = a[1];
                airport.TimezoneName = a[11];
            }
            return airport;
        }

        protected string GetMarkers(List<Airport> markerList)
        {
            string markers = "";
            for (int i = 0; i < markerList.Count; i++)
            {
                markers +=
                    @"var marker" + i + @" = new google.maps.Marker({
                position: new google.maps.LatLng(" +
                    markerList[i].Latitude + ", " +
                    markerList[i].Longitude + ")," +
                    @"map: myMap,
                title:'" + markerList[i].Name + "'});";
            }
            return markers;
        }

        private Bounds CreateBounds()
        {
            if (db.Airports == null) return new Bounds(33.942536, -118.408075, 40.639751, -73.778925);
            IQueryable<double> latitudes = from a in db.Airports
                orderby a.Latitude ascending
                select a.Latitude;
            IQueryable<double> longitudes = from a in db.Airports
                orderby a.Longitude ascending
                select a.Longitude;

            return new Bounds(latitudes.ToList().First(), longitudes.ToList().First(), latitudes.ToList().Last(),
                longitudes.ToList().Last());
        }

        // GET: Airports
        public ActionResult Index()
        {
            var airportsList = new AirportList
            {
                Bounds = CreateBounds(),
                Airports = db.Airports.ToList()
            };
            return View(airportsList);
        }

        // GET: Airports/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        //GET: Airports/Search

        public ActionResult Search(string country, string searchString, bool selector = true)
        {
            if (selector)
            {
                if (country == null)
                {
                    country = "United States";
                }
                List<Airport> airports = GetAirports(country);
                ViewBag.countries = new SelectList(GetCountries(), country);

                if (!string.IsNullOrEmpty(searchString))
                {
                    airports = airports.FindAll(s => s.City.ToLower().Contains(searchString.ToLower()));
                }
                return View(airports);
            }
            List<Airport> airportsCode = GetAirports();
            ViewBag.countries = new SelectList(GetCountries());
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                airportsCode = airportsCode.FindAll(s => s.AirportId.Contains(searchString.ToUpper()));
            }
            return View(airportsCode);
        }

        //GET: Airports/Create
        public ActionResult Create(string id)
        {
            Airport airport = GetAirport(id);
            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + airport.Latitude + "," +
                         airport.Longitude;
            string response = new WebClient().DownloadString(url);
            var test = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(response);
            var addresses = new List<string>();
            foreach (address_component r in from result in test.results
                from r in result.address_components
                where r.types.Contains("administrative_area_level_1")
                select r)
            {
                if (!addresses.Contains(r.long_name)) addresses.Add(r.long_name);
                if (!addresses.Contains(r.short_name)) addresses.Add(r.short_name);
            }
            IOrderedEnumerable<string> sorted = (from s in addresses orderby s.Length ascending select s);
            airport.StateOrProvidence = sorted.FirstOrDefault();
            return View(airport);
        }

        //POST: Airports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Airport airport)
        {
            if (ModelState.IsValid)
            {
                if (db.Airports.Select(a => a.AirportId).ToList().Contains(airport.AirportId))
                {
                    return RedirectToAction("Index");
                }
                db.Airports.Add(airport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airport);
        }

        // GET: Airports/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        // POST: Airports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Airport airport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airport);
        }

        // GET: Airports/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //var flightsToDelete = db.Flights.Where(flight => flight.ArrivalAirport.AirportId == id || flight.DepartureAirport.AirportId == id).ToList();

            //foreach (var removeFlight in flightsToDelete)
            //{
            //    db.Flights.Remove(removeFlight);
            //}
            Airport airport = db.Airports.Find(id);
            db.Airports.Remove(airport);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class GoogleGeoCodeResponse
        {
            public string status { get; set; }
            public results[] results { get; set; }
        }

        public class address_component
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }

        public class geometry
        {
            public string location_type { get; set; }
            public location location { get; set; }
        }

        public class location
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }

        public class results
        {
            public string formatted_address { get; set; }
            public geometry geometry { get; set; }
            public string[] types { get; set; }
            public address_component[] address_components { get; set; }
        }
    }
}