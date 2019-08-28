namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePlaneLayout3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.LayoutSeats");
            AddColumn("dbo.LayoutSeats", "SeatNumber", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.LayoutSeats", "SeatNumber");
            DropColumn("dbo.LayoutSeats", "TailNumber");
            DropColumn("dbo.LayoutSeats", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LayoutSeats", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.LayoutSeats", "TailNumber", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.LayoutSeats");
            DropColumn("dbo.LayoutSeats", "SeatNumber");
            AddPrimaryKey("dbo.LayoutSeats", "TailNumber");
        }
    }
}
