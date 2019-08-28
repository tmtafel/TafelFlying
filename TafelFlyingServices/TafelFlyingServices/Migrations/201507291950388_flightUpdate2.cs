using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class flightUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Flights", "PlaneTailNumber", "dbo.Planes");
            DropIndex("dbo.Flights", new[] {"PlaneTailNumber"});
            RenameColumn("dbo.Flights", "PlaneTailNumber", "TailNumber");
            AddColumn("dbo.Flights", "ArrivalAirport_AirportId", c => c.String(maxLength: 128));
            AddColumn("dbo.Flights", "DepartureAirport_AirportId", c => c.String(maxLength: 128));
            AddColumn("dbo.Flights", "PilotInCommand_PilotId", c => c.Int());
            AddColumn("dbo.Flights", "SecondInCommand_PilotId", c => c.Int());
            AlterColumn("dbo.Flights", "TailNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.Flights", "TailNumber");
            CreateIndex("dbo.Flights", "ArrivalAirport_AirportId");
            CreateIndex("dbo.Flights", "DepartureAirport_AirportId");
            CreateIndex("dbo.Flights", "PilotInCommand_PilotId");
            CreateIndex("dbo.Flights", "SecondInCommand_PilotId");
            AddForeignKey("dbo.Flights", "ArrivalAirport_AirportId", "dbo.Airports", "AirportId");
            AddForeignKey("dbo.Flights", "DepartureAirport_AirportId", "dbo.Airports", "AirportId");
            AddForeignKey("dbo.Flights", "PilotInCommand_PilotId", "dbo.Pilots", "PilotId");
            AddForeignKey("dbo.Flights", "SecondInCommand_PilotId", "dbo.Pilots", "PilotId");
            AddForeignKey("dbo.Flights", "TailNumber", "dbo.Planes", "TailNumber");
            DropColumn("dbo.Flights", "DepartureAirportId");
            DropColumn("dbo.Flights", "ArrivalAirportId");
            DropColumn("dbo.Flights", "PilotInCommandId");
            DropColumn("dbo.Flights", "SecondInCommandId");
        }

        public override void Down()
        {
            AddColumn("dbo.Flights", "SecondInCommandId", c => c.Int());
            AddColumn("dbo.Flights", "PilotInCommandId", c => c.Int());
            AddColumn("dbo.Flights", "ArrivalAirportId", c => c.String(false));
            AddColumn("dbo.Flights", "DepartureAirportId", c => c.String(false));
            DropForeignKey("dbo.Flights", "TailNumber", "dbo.Planes");
            DropForeignKey("dbo.Flights", "SecondInCommand_PilotId", "dbo.Pilots");
            DropForeignKey("dbo.Flights", "PilotInCommand_PilotId", "dbo.Pilots");
            DropForeignKey("dbo.Flights", "DepartureAirport_AirportId", "dbo.Airports");
            DropForeignKey("dbo.Flights", "ArrivalAirport_AirportId", "dbo.Airports");
            DropIndex("dbo.Flights", new[] {"SecondInCommand_PilotId"});
            DropIndex("dbo.Flights", new[] {"PilotInCommand_PilotId"});
            DropIndex("dbo.Flights", new[] {"DepartureAirport_AirportId"});
            DropIndex("dbo.Flights", new[] {"ArrivalAirport_AirportId"});
            DropIndex("dbo.Flights", new[] {"TailNumber"});
            AlterColumn("dbo.Flights", "TailNumber", c => c.String(false, 128));
            DropColumn("dbo.Flights", "SecondInCommand_PilotId");
            DropColumn("dbo.Flights", "PilotInCommand_PilotId");
            DropColumn("dbo.Flights", "DepartureAirport_AirportId");
            DropColumn("dbo.Flights", "ArrivalAirport_AirportId");
            RenameColumn("dbo.Flights", "TailNumber", "PlaneTailNumber");
            CreateIndex("dbo.Flights", "PlaneTailNumber");
            AddForeignKey("dbo.Flights", "PlaneTailNumber", "dbo.Planes", "TailNumber", true);
        }
    }
}