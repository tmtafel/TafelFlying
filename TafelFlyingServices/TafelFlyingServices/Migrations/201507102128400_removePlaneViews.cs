using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class removePlaneViews : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Plane_TailNumber", "dbo.Planes");
            DropIndex("dbo.AspNetUsers", new[] {"Plane_TailNumber"});
            DropColumn("dbo.AspNetUsers", "Plane_TailNumber");
        }

        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Plane_TailNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Plane_TailNumber");
            AddForeignKey("dbo.AspNetUsers", "Plane_TailNumber", "dbo.Planes", "TailNumber");
        }
    }
}