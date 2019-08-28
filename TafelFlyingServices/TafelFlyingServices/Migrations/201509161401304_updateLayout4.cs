namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LayoutSeats", "Type", c => c.String());
            AddColumn("dbo.LayoutSeats", "Saved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LayoutSeats", "Saved");
            DropColumn("dbo.LayoutSeats", "Type");
        }
    }
}
