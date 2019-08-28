using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aircraft",
                c => new
                {
                    AircraftId = c.Int(false, true),
                    Make = c.String(),
                    Model = c.String(),
                    Length = c.Double(),
                    Wingspan = c.Double(),
                    Height = c.Double(),
                    WingArea = c.Double(),
                    AspectRation = c.String(),
                    EmptyWeight = c.Double(),
                    MaxTakeoffWeight = c.Double(),
                    Powerplant = c.String(),
                    Crew_Max = c.Int(),
                    Crew_Min = c.Int(),
                    Passengers_Max = c.Int(),
                    Passengers_Min = c.Int(),
                })
                .PrimaryKey(t => t.AircraftId);

            CreateTable(
                "dbo.Airports",
                c => new
                {
                    AirportId = c.String(false, 128),
                    Name = c.String(false),
                    City = c.String(false),
                    StateOrProvidence = c.String(),
                    Country = c.String(false),
                    Latitude = c.Double(false),
                    Longitude = c.Double(false),
                    Altitude = c.Int(false),
                    Timezone = c.Double(false),
                    TimezoneName = c.String(),
                    DST = c.String(),
                })
                .PrimaryKey(t => t.AirportId);

            CreateTable(
                "dbo.Expenses",
                c => new
                {
                    ExpenseId = c.Int(false, true),
                    Type = c.String(),
                    Amount = c.Decimal(false, 18, 2),
                    Photo_ContentType = c.String(),
                    Photo_ImageBytes = c.Binary(),
                    Photo_SourceFilename = c.String(),
                    TripId = c.Int(false),
                })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.Trips", t => t.TripId, true)
                .Index(t => t.TripId);

            CreateTable(
                "dbo.Trips",
                c => new
                {
                    TripId = c.Int(false, true),
                    InvoiceNumber = c.Int(false),
                    PilotFee = c.Decimal(precision: 18, scale: 2),
                    CoPilotFee = c.Decimal(precision: 18, scale: 2),
                    Paid = c.Boolean(false),
                    TailNumber = c.String(false),
                })
                .PrimaryKey(t => t.TripId);

            CreateTable(
                "dbo.Flights",
                c => new
                {
                    FlightId = c.Int(false, true),
                    FlightNumber = c.Int(),
                    DepartureDate = c.DateTime(false),
                    DepartureTime = c.DateTime(false),
                    ArrivalDate = c.DateTime(false),
                    ArrivalTime = c.DateTime(false),
                    DepartureAirportId = c.String(false),
                    ArrivalAirportId = c.String(false),
                    PilotInCommandId = c.Int(),
                    SecondInCommandId = c.Int(),
                    PlaneTailNumber = c.String(false, 128),
                    TripId = c.Int(),
                    PilotInCommand_PilotId = c.Int(),
                    SecondInCommand_PilotId = c.Int(),
                })
                .PrimaryKey(t => t.FlightId)
                .ForeignKey("dbo.Pilots", t => t.PilotInCommand_PilotId)
                .ForeignKey("dbo.Pilots", t => t.SecondInCommand_PilotId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.Planes", t => t.PlaneTailNumber, true)
                .Index(t => t.PlaneTailNumber)
                .Index(t => t.TripId)
                .Index(t => t.PilotInCommand_PilotId)
                .Index(t => t.SecondInCommand_PilotId);

            CreateTable(
                "dbo.Pilots",
                c => new
                {
                    PilotId = c.Int(false, true),
                    ApplicationUserId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.PilotId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(false, 128),
                    FirstName = c.String(false),
                    LastName = c.String(false),
                    Email = c.String(false, 256),
                    IsAPilot = c.Boolean(false),
                    ViewToPublic = c.Boolean(false),
                    EmailConfirmed = c.Boolean(false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(false),
                    TwoFactorEnabled = c.Boolean(false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(false),
                    AccessFailedCount = c.Int(false),
                    UserName = c.String(false, 256),
                    Plane_TailNumber = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Planes", t => t.Plane_TailNumber)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Plane_TailNumber);

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(false, 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(false, 128),
                    ProviderKey = c.String(false, 128),
                    UserId = c.String(false, 128),
                })
                .PrimaryKey(t => new {t.LoginProvider, t.ProviderKey, t.UserId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(false, 128),
                    RoleId = c.String(false, 128),
                })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.PlaneAccesses",
                c => new
                {
                    Id = c.Int(false, true),
                    PlaneId = c.String(),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Planes",
                c => new
                {
                    TailNumber = c.String(false, 128),
                    Color = c.String(),
                    AircraftId = c.Int(false),
                })
                .PrimaryKey(t => t.TailNumber);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(false, 128),
                    Name = c.String(false, 256),
                    Description = c.String(),
                    Discriminator = c.String(false, 128),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "Plane_TailNumber", "dbo.Planes");
            DropForeignKey("dbo.Flights", "PlaneTailNumber", "dbo.Planes");
            DropForeignKey("dbo.Flights", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Flights", "SecondInCommand_PilotId", "dbo.Pilots");
            DropForeignKey("dbo.Flights", "PilotInCommand_PilotId", "dbo.Pilots");
            DropForeignKey("dbo.Pilots", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenses", "TripId", "dbo.Trips");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] {"RoleId"});
            DropIndex("dbo.AspNetUserRoles", new[] {"UserId"});
            DropIndex("dbo.AspNetUserLogins", new[] {"UserId"});
            DropIndex("dbo.AspNetUserClaims", new[] {"UserId"});
            DropIndex("dbo.AspNetUsers", new[] {"Plane_TailNumber"});
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Pilots", new[] {"ApplicationUserId"});
            DropIndex("dbo.Flights", new[] {"SecondInCommand_PilotId"});
            DropIndex("dbo.Flights", new[] {"PilotInCommand_PilotId"});
            DropIndex("dbo.Flights", new[] {"TripId"});
            DropIndex("dbo.Flights", new[] {"PlaneTailNumber"});
            DropIndex("dbo.Expenses", new[] {"TripId"});
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Planes");
            DropTable("dbo.PlaneAccesses");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Pilots");
            DropTable("dbo.Flights");
            DropTable("dbo.Trips");
            DropTable("dbo.Expenses");
            DropTable("dbo.Airports");
            DropTable("dbo.Aircraft");
        }
    }
}