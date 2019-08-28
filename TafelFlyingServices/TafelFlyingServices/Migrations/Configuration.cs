using System.Data.Entity.Migrations;
using TafelFlyingServices.Models;

namespace TafelFlyingServices.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //


            //Add other entities using context methods
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //var user = new ApplicationUser { Email = "tmtafel@gmail.com", UserName = "tmtafel@gmail.com", FirstName = "Tosh", LastName = "Tafel", PhoneNumber = "5026411990" };

            //manager.Create(user, "Valley06$");

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //roleManager.Create(new IdentityRole("Admin"));
            //roleManager.Create(new IdentityRole("Pilot"));
            //roleManager.Create(new IdentityRole("Client"));
            //manager.AddToRole(user.Id, "Admin");
        }
    }
}