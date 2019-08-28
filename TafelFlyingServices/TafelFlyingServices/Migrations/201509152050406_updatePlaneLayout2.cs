namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePlaneLayout2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.LayoutSeats");
            AddColumn("dbo.LayoutSeats", "TailNumber", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.LayoutSeats", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.LayoutSeats", "TailNumber");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LayoutSeats");
            AlterColumn("dbo.LayoutSeats", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.LayoutSeats", "TailNumber");
            AddPrimaryKey("dbo.LayoutSeats", "Id");
        }
    }
}
