namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LayoutSeats", "LayoutViewModelId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LayoutSeats", "LayoutViewModelId");
        }
    }
}
