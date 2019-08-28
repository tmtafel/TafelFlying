using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AircraftController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Aircraft
        public ActionResult Index()
        {
            IOrderedEnumerable<Aircraft> aircrafts = db.Aircraft.OrderBy(x => x.Model).ToList().OrderBy(y => y.Make);

            return View(aircrafts);
        }

        // GET: Aircraft/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aircraft aircraft = db.Aircraft.Find(id);
            if (aircraft == null)
            {
                return HttpNotFound();
            }
            return View(aircraft);
        }

        // GET: Aircraft/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aircraft/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aircraft aircraft)
        {
            if (!ModelState.IsValid) return View(aircraft);
            db.Aircraft.Add(aircraft);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Aircraft/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aircraft aircraft = db.Aircraft.Find(id);
            if (aircraft == null)
            {
                return HttpNotFound();
            }
            return View(aircraft);
        }

        // POST: Aircraft/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aircraft aircraft)
        {
            if (!ModelState.IsValid) return View(aircraft);
            db.Entry(aircraft).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Aircraft/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aircraft aircraft = db.Aircraft.Find(id);
            if (aircraft == null)
            {
                return HttpNotFound();
            }
            return View(aircraft);
        }

        // POST: Aircraft/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aircraft aircraft = db.Aircraft.Find(id);
            db.Aircraft.Remove(aircraft);
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
    }
}