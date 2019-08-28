using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class flightUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Flights", "PilotInCommand_PilotId", "dbo.Pilots");
            DropForeignKey("dbo.Flights", "SecondInCommand_PilotId", "dbo.Pilots");
            DropIndex("dbo.Flights", new[] {"PilotInCommand_PilotId"});
            DropIndex("dbo.Flights", new[] {"SecondInCommand_PilotId"});
            AddColumn("dbo.Flights", "Departure", c => c.DateTime(false));
            AddColumn("dbo.Flights", "Arrival", c => c.DateTime(false));
            DropColumn("dbo.Flights", "DepartureDate");
            DropColumn("dbo.Flights", "DepartureTime");
            DropColumn("dbo.Flights", "ArrivalDate");
            DropColumn("dbo.Flights", "ArrivalTime");
            DropColumn("dbo.Flights", "PilotInCommand_PilotId");
            DropColumn("dbo.Flights", "SecondInCommand_PilotId");
        }

        public override void Down()
        {
            AddColumn("dbo.Flights", "SecondInCommand_PilotId", c => c.Int());
            AddColumn("dbo.Flights", "PilotInCommand_PilotId", c => c.Int());
            AddColumn("dbo.Flights", "ArrivalTime", c => c.DateTime(false));
            AddColumn("dbo.Flights", "ArrivalDate", c => c.DateTime(false));
            AddColumn("dbo.Flights", "DepartureTime", c => c.DateTime(false));
            AddColumn("dbo.Flights", "DepartureDate", c => c.DateTime(false));
            DropColumn("dbo.Flights", "Arrival");
            DropColumn("dbo.Flights", "Departure");
            CreateIndex("dbo.Flights", "SecondInCommand_PilotId");
            CreateIndex("dbo.Flights", "PilotInCommand_PilotId");
            AddForeignKey("dbo.Flights", "SecondInCommand_PilotId", "dbo.Pilots", "PilotId");
            AddForeignKey("dbo.Flights", "PilotInCommand_PilotId", "dbo.Pilots", "PilotId");
        }
    }
}