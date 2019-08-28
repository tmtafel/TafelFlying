namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout8 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LayoutSeats", name: "LayoutViewModel_TailNumber", newName: "TailNumber");
            RenameIndex(table: "dbo.LayoutSeats", name: "IX_LayoutViewModel_TailNumber", newName: "IX_TailNumber");
            DropColumn("dbo.LayoutSeats", "LayoutViewModelId");
            DropColumn("dbo.LayoutSeats", "Saved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LayoutSeats", "Saved", c => c.Boolean(nullable: false));
            AddColumn("dbo.LayoutSeats", "LayoutViewModelId", c => c.String());
            RenameIndex(table: "dbo.LayoutSeats", name: "IX_TailNumber", newName: "IX_LayoutViewModel_TailNumber");
            RenameColumn(table: "dbo.LayoutSeats", name: "TailNumber", newName: "LayoutViewModel_TailNumber");
        }
    }
}
