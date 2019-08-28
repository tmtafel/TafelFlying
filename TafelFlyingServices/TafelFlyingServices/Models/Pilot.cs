using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TafelFlyingServices.Models
{
    public class Pilot
    {
        [Key]
        [Required]
        public int PilotId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class PilotManager
    {
        public bool AddPilot(ApplicationUser user)
        {
            try
            {
                var db = new ApplicationDbContext();
                IEnumerable<ApplicationUser> pilotEmailsToRemove =
                    db.Pilots.ToList().Where(p => p.ApplicationUser.Email == user.Email).Select(p => p.ApplicationUser);
                foreach (ApplicationUser pilot1 in pilotEmailsToRemove)
                {
                    RemovePilot(pilot1);
                }
                var pilot = new Pilot {ApplicationUserId = user.Id};
                db.Pilots.Add(pilot);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemovePilot(ApplicationUser user)
        {
            try
            {
                var db = new ApplicationDbContext();
                Pilot pilot = db.Pilots.Where(p => p.ApplicationUserId == user.Id).Select(p => p).First();
                db.Pilots.Remove(pilot);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PilotExists(ApplicationUser user)
        {
            var db = new ApplicationDbContext();
            IQueryable<Pilot> pilot = db.Pilots.Where(p => p.ApplicationUserId == user.Id).Select(p => p);
            return pilot.Any();
        }
    }
}