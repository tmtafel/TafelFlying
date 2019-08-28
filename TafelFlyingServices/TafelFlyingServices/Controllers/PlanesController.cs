using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Controllers
{
    [Authorize]
    public class PlanesController : Controller
    {
        public ApplicationDbContext _db = new ApplicationDbContext();

        //private async Task Run()
        //{
        //    var service = new FusiontablesService(new BaseClientService.Initializer()
        //    {
        //        ApiKey = "AIzaSyCFyqTpotZJBGzsULjGkjTN87tnTiT0isE"
        //    });

        //    var table = await service.Table.List().ExecuteAsync();
        //    table.Items.ToList();
        //}

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        private List<ApplicationUser> GetAdmins()
        {
            var admins = new List<ApplicationUser>();
            IdentityRole adminRole = (from r in _db.Roles.ToList() where r.Name == "Admin" select r).First();
            foreach (ApplicationUser u in _db.Users.ToList())
            {
                List<IdentityUserRole> userRoles = u.Roles.ToList();
                admins.AddRange(from identityUserRole in userRoles
                                where identityUserRole.RoleId == adminRole.Id
                                select u);
            }
            return admins;
        }

        // GET: Planes
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<string> planeList =
                UserManager.GetClaims(User.Identity.GetUserId())
                    .Where(c => c.Type == "PlaneAccess")
                    .Select(c => c.Value);
            IQueryable<Plane> planes = _db.Planes.Where(p => planeList.Contains(p.TailNumber)).Select(p => p);
            var indexPlanes = planes.Select(p => new IndexPlaneViewModel()
            {
                BlackText = p.BlackText ? "#000000" : "#FFFFFF",
                Color = p.Color,
                Make = p.Aircraft.Make,
                Model = p.Aircraft.Model,
                TailNumber = p.TailNumber
            });
            return View(indexPlanes);
        }

        // GET: Planes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plane plane = _db.Planes.Find(id);
            if (plane == null)
            {
                return HttpNotFound();
            }
            if (!UserManager.GetClaims(User.Identity.GetUserId())
                .Where(c => c.Type == "PlaneAccess")
                .Select(c => c.Value)
                .Contains(id)) return RedirectToAction("Login", "Account");
            List<ApplicationUser> users = UserManager.Users.ToList();
            List<ApplicationUser> viewers = users.Where(user => user.Claims.Select(c => c.ClaimValue).Contains(plane.TailNumber)).ToList();

            var detailsPlane = new DetailsPlaneViewModel
            {
                TailNumber = plane.TailNumber,
                Aircraft = plane.Aircraft,
                Viewers = viewers,
                Color = plane.Color,
                BlackText = "#FFFFFF",
                CrewMin = plane.Crew.Min,
                CrewMax = plane.Crew.Max,
                PassengerMin = plane.Passengers.Min,
                PassengerMax = plane.Passengers.Max
            };
            if (plane.BlackText)
            {
                detailsPlane.BlackText = "#000000";
            }
            return View(detailsPlane);
        }

        // GET: Planes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditPlaneViewModel plane)
        {
            if (!ModelState.IsValid) return View(plane);
            List<string> tailNumbers = _db.Planes.Select(p => p.TailNumber).ToList();
            string tailNumber = plane.TailNumber.ToUpper();
            if (tailNumbers.Contains(tailNumber))
            {
                ModelState.AddModelError("TailNumber", "TailNumber Already Used");
                return View(plane);
            }
            var newPlane = new Plane
            {
                Aircraft = _db.Aircraft.Find(plane.AircraftId),
                TailNumber = tailNumber,
                Color = plane.Color,
                BlackText = plane.BlackText,
                Passengers = new Range
                {
                    Min = 0,
                    Max = plane.PassengerMax
                },
                Crew = new Range
                {
                    Min = plane.CrewMin,
                    Max = plane.CrewMax
                }
            };


            List<ApplicationUser> admins = GetAdmins();
            foreach (ApplicationUser user in admins)
            {
                UserManager.AddClaim(user.Id, new Claim("PlaneAccess", tailNumber));
            }

            _db.Planes.Add(newPlane);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Planes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plane plane = _db.Planes.Find(id);
            if (plane == null)
            {
                return HttpNotFound();
            }

            var model = new EditPlaneViewModel
            {
                AircraftId = plane.Aircraft.AircraftId,
                Color = plane.Color,
                TailNumber = id,
                AvailibleViewers = PlaneViewerRepository.GetAllThatAreNotNotAdmin().ToList(),
                BlackText = plane.BlackText,
                CrewMin = plane.Crew.Min,
                CrewMax = plane.Crew.Max,
                PassengerMax = plane.Passengers.Max
            };


            List<ApplicationUser> admins = GetAdmins();

            List<ApplicationUser> users = UserManager.Users.ToList();
            string[] postedViewerIds = (from user in users
                                        where !admins.Contains(user)
                                        let claims =
                                            UserManager.GetClaims(user.Id)
                                                .Where(c => c.Type == "PlaneAccess" && c.Value == model.TailNumber)
                                                .Select(c => c)
                                        where claims.Any()
                                        select user.Id).ToArray();
            model.PostedViewers = new PostedViewers { PlaneViewerIds = postedViewerIds };
            if (postedViewerIds.Any())
            {
                model.SelectedViewers = PlaneViewerRepository.GetAll()
                    .Where(x => postedViewerIds.Any(s => x.UserId.ToString(CultureInfo.InvariantCulture).Equals(s)))
                    .ToList();
            }


            return View(model);
        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPlaneViewModel plane)
        {
            if (!ModelState.IsValid) return View(plane);


            var selectedViewerIds = new List<string>();
            if (plane.PostedViewers != null)
            {
                selectedViewerIds = plane.PostedViewers.PlaneViewerIds.ToList();
            }

            IEnumerable<string> admins = GetAdmins().Select(a => a.Id);
            IList<string> enumerable = admins as IList<string> ?? admins.ToList();
            selectedViewerIds.AddRange(enumerable);

            List<ApplicationUser> users = UserManager.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                IEnumerable<Claim> userClaims = UserManager.GetClaims(user.Id)
                    .Where(c => c.Type == "PlaneAccess" && c.Value == plane.TailNumber)
                    .Select(c => c);
                if (selectedViewerIds.Contains(user.Id))
                {
                    if (!userClaims.Any())
                    {
                        UserManager.AddClaim(user.Id, new Claim("PlaneAccess", plane.TailNumber));
                    }
                }
                else
                {
                    if (userClaims.Any())
                    {
                        UserManager.RemoveClaim(user.Id, new Claim("PlaneAccess", plane.TailNumber));
                    }
                }
            }
            Plane editPlane = _db.Planes.Find(plane.TailNumber);
            editPlane.Color = plane.Color;
            editPlane.Aircraft = _db.Aircraft.Find(plane.AircraftId);
            editPlane.BlackText = plane.BlackText;
            editPlane.Crew.Min = 0;
            editPlane.Crew.Max = plane.CrewMax;
            editPlane.Passengers.Max = plane.PassengerMax;
            editPlane.Passengers.Min = 0;
            _db.Entry(editPlane).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = editPlane.TailNumber });
        }

        public ActionResult CabinLayout(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plane plane = _db.Planes.Find(id);
            if (plane == null)
            {
                return HttpNotFound();
            }
            if (plane.Layout == null)
            {
                var layout = new LayoutViewModel()
                {
                    Height = 180,
                    Width = 90,
                    Top = 0,
                    Left = 0
                };

                plane.Layout = layout;
                _db.SaveChanges();
            }
            return View(plane.Layout);
        }

        [HttpPost]
        public void UpdateCabinLayout(string data)
        {
            if (data != null)
            {
                var json = JObject.Parse(data);
                var id = (string)json["tailnumber"];

                Plane plane = _db.Planes.Find(id);

                plane.Layout.Height = (int)json["height"];
                plane.Layout.Width = (int)json["width"];
                plane.Layout.Top = (int)json["top"];
                plane.Layout.Left = (int)json["left"];

                var seats = json["seats"].ToList();
                plane.Layout.Seats = plane.Layout.Seats.OrderBy(s => s.Index).ToList();
                while (plane.Layout.Seats.Count() > seats.Count())
                {
                    _db.LayoutSeats.Remove(plane.Layout.Seats.Last());
                    _db.SaveChanges();
                }
                if (plane.Layout.Seats.Any())
                {
                    foreach (var dbSeat in plane.Layout.Seats)
                    {
                        var seat = seats.Where(s => (int) s["index"] == dbSeat.Index).Select(s => s).FirstOrDefault();
                        if (seat == null) continue;
                        dbSeat.Direction = (string) seat["direction"];
                        dbSeat.Left = (int) seat["left"];
                        dbSeat.Top = (int) seat["top"];
                        dbSeat.Type = (string) seat["type"];
                        dbSeat.SeatId = id + "_" + (int) seat["index"];
                        _db.SaveChanges();
                    }
                }

                if (plane.Layout.Seats.Count() < seats.Count())
                {
                    for (var i = plane.Layout.Seats.Count(); i < seats.Count; i++)
                    {
                        var newSeat = new LayoutSeat()
                        {
                            SeatId = id + "_" + (int) seats[i]["index"],
                            Direction = (string) seats[i]["direction"],
                            Index = (int) seats[i]["index"],
                            Left = (int) seats[i]["left"],
                            Top = (int) seats[i]["top"],
                            Type = (string) seats[i]["type"]
                        };
                        plane.Layout.Seats.Add(newSeat);
                    _db.SaveChanges();
                    }
                }
                
                _db.SaveChanges();
            }
        }
        // GET: Planes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plane plane = _db.Planes.Find(id);
            if (plane == null)
            {
                return HttpNotFound();
            }
            return View(plane);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            List<ApplicationUser> users = UserManager.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                IEnumerable<Claim> userClaims = UserManager.GetClaims(user.Id)
                    .Where(c => c.Type == "PlaneAccess" && c.Value == id)
                    .Select(c => c);
                foreach (Claim claim in userClaims.Where(claim => claim.Type == "PlaneAccess" && claim.Value == id))
                {
                    UserManager.RemoveClaim(user.Id, claim);
                }
            }
            Plane plane = _db.Planes.Find(id);
            _db.Planes.Remove(plane);
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