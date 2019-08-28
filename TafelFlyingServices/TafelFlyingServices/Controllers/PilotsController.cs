using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    [Authorize]
    public class PilotsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pilots
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Pilot> pilots = db.Pilots.ToList();
            List<Pilot> removePilotList =
                pilots.Where(pilot => pilot.ApplicationUserId == null || pilot.ApplicationUser == null).ToList();
            foreach (Pilot pilot in removePilotList)
            {
                pilots.Remove(pilot);
                db.SaveChanges();
            }
            List<Pilot> publicPilots = pilots.Where(pilot => pilot.ApplicationUser.ViewToPublic).ToList();
            return View(publicPilots);
        }

        // GET: Pilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                return HttpNotFound();
            }
            return View(pilot);
        }

        // GET: Pilots/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Pilots/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ApplicationUserId")] Pilot pilot)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        pilot.ApplicationUser = db.Users.Find(pilot.ApplicationUserId);
        //        db.Pilots.Add(pilot);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(pilot);
        //}

        // GET: Pilots/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                return HttpNotFound();
            }
            return View(pilot);
        }

        // POST: Pilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ApplicationUserId, PilotId")] Pilot pilot)
        {
            if (ModelState.IsValid)
            {
                pilot.ApplicationUser = db.Users.Find(pilot.ApplicationUserId);

                db.Entry(pilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pilot);
        }

        //// GET: Pilots/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pilot pilot = db.Pilots.Find(id);
        //    if (pilot == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pilot);
        //}

        //// POST: Pilots/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Pilot pilot = db.Pilots.Find(id);
        //    db.Pilots.Remove(pilot);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}