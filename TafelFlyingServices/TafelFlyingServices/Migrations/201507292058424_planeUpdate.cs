using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class planeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Planes", "Aircraft_AircraftId", c => c.Int());
            CreateIndex("dbo.Planes", "Aircraft_AircraftId");
            AddForeignKey("dbo.Planes", "Aircraft_AircraftId", "dbo.Aircraft", "AircraftId");
            DropColumn("dbo.Planes", "AircraftId");
        }

        public override void Down()
        {
            AddColumn("dbo.Planes", "AircraftId", c => c.Int(false));
            DropForeignKey("dbo.Planes", "Aircraft_AircraftId", "dbo.Aircraft");
            DropIndex("dbo.Planes", new[] {"Aircraft_AircraftId"});
            DropColumn("dbo.Planes", "Aircraft_AircraftId");
        }
    }
}