namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePlaneLayout4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.LayoutSeats");
            AddColumn("dbo.LayoutSeats", "SeatId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.LayoutSeats", "SeatId");
            DropColumn("dbo.LayoutSeats", "SeatNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LayoutSeats", "SeatNumber", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.LayoutSeats");
            DropColumn("dbo.LayoutSeats", "SeatId");
            AddPrimaryKey("dbo.LayoutSeats", "SeatNumber");
        }
    }
}
