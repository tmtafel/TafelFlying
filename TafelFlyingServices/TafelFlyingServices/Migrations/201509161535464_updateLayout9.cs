namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout9 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LayoutSeats", name: "TailNumber", newName: "LayoutViewModel_TailNumber");
            RenameIndex(table: "dbo.LayoutSeats", name: "IX_TailNumber", newName: "IX_LayoutViewModel_TailNumber");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.LayoutSeats", name: "IX_LayoutViewModel_TailNumber", newName: "IX_TailNumber");
            RenameColumn(table: "dbo.LayoutSeats", name: "LayoutViewModel_TailNumber", newName: "TailNumber");
        }
    }
}
