using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class flightUpdate4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Flights", "TailNumber", "Plane_TailNumber");
            RenameIndex("dbo.Flights", "IX_TailNumber", "IX_Plane_TailNumber");
        }

        public override void Down()
        {
            RenameIndex("dbo.Flights", "IX_Plane_TailNumber", "IX_TailNumber");
            RenameColumn("dbo.Flights", "Plane_TailNumber", "TailNumber");
        }
    }
}