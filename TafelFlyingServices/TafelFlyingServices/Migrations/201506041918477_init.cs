namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aircraft",
                c => new
                    {
                        AircraftId = c.Int(nullable: false, identity: true),
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
                        AirportId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        City = c.String(nullable: false),
                        StateOrProvidence = c.String(),
                        Country = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Altitude = c.Int(nullable: false),
                        Timezone = c.Double(nullable: false),
                        TimezoneName = c.String(),
                        DST = c.String(),
                    })
                .PrimaryKey(t => t.AirportId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Photo_ContentType = c.String(),
                        Photo_ImageBytes = c.Binary(),
                        Photo_SourceFilename = c.String(),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        TripId = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.Int(nullable: false),
                        PilotFee = c.Decimal(precision: 18, scale: 2),
                        CoPilotFee = c.Decimal(precision: 18, scale: 2),
                        Paid = c.Boolean(nullable: false),
                        TailNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TripId);
            
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        FlightId = c.Int(nullable: false, identity: true),
                        FlightNumber = c.Int(),
                        DepartureDate = c.DateTime(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        DepartureAirportId = c.String(nullable: false),
                        ArrivalAirportId = c.String(nullable: false),
                        PilotInCommandId = c.Int(),
                        SecondInCommandId = c.Int(),
                        PlaneTailNumber = c.String(nullable: false, maxLength: 128),
                        TripId = c.Int(),
                        PilotInCommand_PilotId = c.Int(),
                        SecondInCommand_PilotId = c.Int(),
                    })
                .PrimaryKey(t => t.FlightId)
                .ForeignKey("dbo.Pilots", t => t.PilotInCommand_PilotId)
                .ForeignKey("dbo.Pilots", t => t.SecondInCommand_PilotId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.Planes", t => t.PlaneTailNumber, cascadeDelete: true)
                .Index(t => t.PlaneTailNumber)
                .Index(t => t.TripId)
                .Index(t => t.PilotInCommand_PilotId)
                .Index(t => t.SecondInCommand_PilotId);
            
            CreateTable(
                "dbo.Pilots",
                c => new
                    {
                        PilotId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PilotId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        IsAPilot = c.Boolean(nullable: false),
                        ViewToPublic = c.Boolean(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
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
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PlaneAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaneId = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Planes",
                c => new
                    {
                        TailNumber = c.String(nullable: false, maxLength: 128),
                        Color = c.String(),
                        AircraftId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TailNumber);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Plane_TailNumber" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Pilots", new[] { "ApplicationUserId" });
            DropIndex("dbo.Flights", new[] { "SecondInCommand_PilotId" });
            DropIndex("dbo.Flights", new[] { "PilotInCommand_PilotId" });
            DropIndex("dbo.Flights", new[] { "TripId" });
            DropIndex("dbo.Flights", new[] { "PlaneTailNumber" });
            DropIndex("dbo.Expenses", new[] { "TripId" });
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
