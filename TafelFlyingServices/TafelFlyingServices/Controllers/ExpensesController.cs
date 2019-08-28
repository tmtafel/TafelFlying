using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public FileContentResult getImage(int id)
        {
            Photo photo = db.Expenses.Find(id).Photo;
            byte[] byteArray = photo.ImageBytes;
            if (byteArray != null)
            {
                return new FileContentResult(byteArray, photo.ContentType);
            }
            return null;
        }

        // GET: Expenses
        public ActionResult Index()
        {
            ViewBag.Invoices = "Which Invoice?";
            List<Trip> invoices = db.Trips.ToList();
            return View(invoices);
        }

        public ActionResult Invoice(string id)
        {
            if (id == null)
            {
                string route = Url.Action();
                string[] routeArray = route.Split('/');
                try
                {
                    id = routeArray[3];
                }
                catch
                {
                    id = "";
                }
            }
            int routeNumber = 0;
            try
            {
                routeNumber = Convert.ToInt32(id);
            }
            catch
            {
                routeNumber = 0;
            }

            List<Trip> trips = db.Trips.ToList();
            bool isThereATrip = false;
            List<int> invoiceNumbers = (from t in trips select t.InvoiceNumber).ToList();
            if (invoiceNumbers.Contains(routeNumber))
            {
                isThereATrip = true;
            }
            if (!isThereATrip) return RedirectToAction("Index");
            List<Expense> expenses = db.Expenses.Include(e => e.Trip).ToList();
            expenses = (from t in expenses where t.InvoiceNumber == routeNumber select t).ToList();
            ViewBag.InvoiceNumber = routeNumber;
            return View(expenses);
        }

        // GET: Expenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public ActionResult Create(int? invoiceNumber)
        {
            if (invoiceNumber != null)
            {
                ViewBag.InvoiceNumber = invoiceNumber;
                var expense = new ExpenseModel();

                foreach (Trip trip in db.Trips)
                {
                    if (trip.InvoiceNumber == invoiceNumber)
                    {
                        expense.TripId = trip.TripId;
                        return View(expense);
                    }
                }
                return View();
            }

            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseModel expense)
        {
            if (ModelState.IsValid)
            {
                var newExpense = new Expense
                {
                    Amount = expense.Amount,
                    TripId = expense.TripId,
                    Type = expense.Type
                };

                var photo = new Photo();
                if (expense.Photo != null)
                {
                    int length = expense.Photo.ContentLength;
                    var tempImage = new byte[length];
                    expense.Photo.InputStream.Read(tempImage, 0, length);
                    photo.ImageBytes = tempImage;
                    photo.ContentType = expense.Photo.ContentType;
                    photo.SourceFilename = expense.Photo.FileName;
                }
                newExpense.Photo = photo;

                db.Expenses.Add(newExpense);
                db.SaveChanges();
                return RedirectToAction("Invoice/" + newExpense.InvoiceNumber);
            }
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            var expenseModel = new ExpenseModel
            {
                Amount = expense.Amount,
                ExpenseId = expense.ExpenseId,
                TripId = expense.TripId,
                Type = expense.Type,
                PhotoSource = expense.Photo
            };

            return View(expenseModel);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenseModel expense)
        {
            if (ModelState.IsValid)
            {
                Expense expenseDb = db.Expenses.Find(expense.ExpenseId);
                expenseDb.Amount = expense.Amount;
                expenseDb.Type = expense.Type;
                expenseDb.TripId = expense.TripId;

                if (expense.Photo != null)
                {
                    var photo = new Photo();
                    int length = expense.Photo.ContentLength;
                    var tempImage = new byte[length];
                    expense.Photo.InputStream.Read(tempImage, 0, length);
                    photo.ImageBytes = tempImage;
                    photo.ContentType = expense.Photo.ContentType;
                    photo.SourceFilename = expense.Photo.FileName;
                    expenseDb.Photo = photo;
                }
                db.Entry(expenseDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Invoice", "Expenses", new {id = expenseDb.InvoiceNumber});
            }
            ViewBag.TripId = new SelectList(db.Trips, "TripId", "TripId", expense.TripId);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expenses.Find(id);
            int invoiceNumber = expense.InvoiceNumber;
            db.Expenses.Remove(expense);
            db.SaveChanges();
            return RedirectToAction("Invoice", "Expenses", new {id = invoiceNumber});
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