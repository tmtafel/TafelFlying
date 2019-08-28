using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TafelFlyingServices.Models
{
    public class Plane
    {
        [Key]
        [Required]
        [Display(Name = "Tail Number")]
        public string TailNumber { get; set; }

        public string Color { get; set; }

        public virtual Aircraft Aircraft { get; set; }

        [Display(Name = "Black Text")]
        public bool BlackText { get; set; }

        public Range Crew { get; set; }

        public Range Passengers { get; set; }

        public virtual LayoutViewModel Layout { get; set; }
    }

    public class IndexPlaneViewModel
    {
        public string TailNumber { get; set; }
        public string Color { get; set; }
        public string BlackText { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

    }

    public class DetailsPlaneViewModel
    {
        public string TailNumber { get; set; }
        public Aircraft Aircraft { get; set; }
        public string Color { get; set; }
        public string BlackText { get; set; }
        public int? CrewMax { get; set; }
        public int? CrewMin { get; set; }
        public int? PassengerMax { get; set; }
        public int? PassengerMin { get; set; }
        public List<ApplicationUser> Viewers { get; set; }
    }

    public class LayoutViewModel
    {
        [Key]
        [ForeignKey("Plane")]
        public string TailNumber { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }

        public virtual Plane Plane { get; set; }
        public virtual ICollection<LayoutSeat> Seats { get; set; }
    }

    public class LayoutSeat
    {
        [Key]
        public string SeatId { get; set; }
        public int Index { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public string Direction { get; set; }
        public string Type { get; set; }

        public virtual LayoutViewModel LayoutViewModel { get; set; }
    }


    public class EditPlaneViewModel
    {
        [Required]
        public string TailNumber { get; set; }

        [Display(Name = "Aircraft")]
        public int AircraftId { get; set; }

        public string Color { get; set; }
        public bool BlackText { get; set; }
        public int? CrewMax { get; set; }
        public int? CrewMin { get; set; }
        public int? PassengerMax { get; set; }
        public virtual List<PlaneViewer> AvailibleViewers { get; set; }
        public virtual IList<PlaneViewer> SelectedViewers { get; set; }
        public PostedViewers PostedViewers { get; set; }
    }

    public class PlaneViewer
    {
        public string UserId { get; set; } // Integer value of a checkbox
        public string UserName { get; set; } // String name of a checkbox

        public List<string> Tags { get; set; }

        public bool IsSelected { get; set; } // Boolean value to select a checkbox on the list
    }

    public class PlaneViewerRepository
    {
        public static PlaneViewer Get(string id)
        {
            return GetAll().FirstOrDefault(x => x.UserId.Equals(id));
        }

        public static IEnumerable<PlaneViewer> GetAll()
        {
            var db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();
            return users.Select(user => new PlaneViewer { UserName = user.Email, UserId = user.Id }).ToList();
        }

        public static IEnumerable<PlaneViewer> GetAllThatAreNotNotAdmin()
        {
            var db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();
            IEnumerable<ApplicationUser> returnList = (from applicationUser in users
                                                       let auRoles = applicationUser.Roles.Select(r => r.RoleId)
                                                       from auRole in auRoles
                                                       where db.Roles.Find(auRole).Name != "Admin"
                                                       select applicationUser);
            return returnList.Select(user => new PlaneViewer { UserName = user.Email, UserId = user.Id });
        }
    }

    // Helper class to make posting back selected values easier
    public class PostedViewers
    {
        public string[] PlaneViewerIds { get; set; }
    }

    public class Range
    {
        public int? Max { get; set; }
        public int? Min { get; set; }
    }
}