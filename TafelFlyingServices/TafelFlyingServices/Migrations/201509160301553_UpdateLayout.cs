namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLayout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LayoutSeats", "Index", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LayoutSeats", "Index");
        }
    }
}
