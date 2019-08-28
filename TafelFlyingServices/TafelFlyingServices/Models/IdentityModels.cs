using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TafelFlyingServices.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public override string Email { get; set; }

        public bool IsAPilot { get; set; }

        public bool ViewToPublic { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity =
                await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TafelFlyingServices", false)
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    if (modelBuilder == null)
        //    {
        //        throw new ArgumentNullException("modelBuilder");
        //    }

        //    // Keep this:
        //    modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

        //    // Change TUser to ApplicationUser everywhere else - 
        //    // IdentityUser and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
        //    EntityTypeConfiguration<ApplicationUser> table =
        //        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

        //    table.Property((ApplicationUser u) => u.UserName).IsRequired();

        //    // EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
        //    modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
        //    modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
        //        new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

        //    // Leave this alone:
        //    EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
        //        modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
        //            new
        //            {
        //                UserId = l.UserId,
        //                LoginProvider = l.LoginProvider,
        //                ProviderKey
        //                    = l.ProviderKey
        //            }).ToTable("AspNetUserLogins");

        //    entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.UserId);
        //    EntityTypeConfiguration<IdentityUserClaim> table1 =
        //        modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

        //    table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u..Id);

        //    // Add this, so that IdentityRole can share a table with ApplicationRole:
        //    modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

        //    // Change these from IdentityRole to ApplicationRole:
        //    EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
        //        modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

        //    entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();
        //}

        public DbSet<Pilot> Pilots { get; set; }

        public DbSet<Plane> Planes { get; set; }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Aircraft> Aircraft { get; set; }

        public DbSet<LayoutViewModel> LayoutViewModels { get; set; }

        public DbSet<LayoutSeat> LayoutSeats { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class IdentityManager
    {
        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private ApplicationDbContext _db = new ApplicationDbContext();

        private RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new ApplicationDbContext()));

        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            IdentityResult idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            IdentityResult idResult = um.Create(user, password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            IdentityResult idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(string userId)
        {
            ApplicationUser user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                _userManager.RemoveFromRole(userId, role.RoleId);
            }
        }
    }
}