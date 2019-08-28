namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LayoutSeats", "LayoutViewModel_TailNumber", "dbo.LayoutViewModels");
            DropForeignKey("dbo.Planes", "Layout_TailNumber", "dbo.LayoutViewModels");
            DropIndex("dbo.Planes", new[] { "Layout_TailNumber" });
            DropIndex("dbo.LayoutSeats", new[] { "LayoutViewModel_TailNumber" });
            DropColumn("dbo.Planes", "Layout_TailNumber");
            DropTable("dbo.LayoutViewModels");
            DropTable("dbo.LayoutSeats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LayoutSeats",
                c => new
                    {
                        SeatId = c.String(nullable: false, maxLength: 128),
                        LayoutViewModelId = c.String(),
                        Index = c.Int(nullable: false),
                        Left = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                        Direction = c.String(),
                        Type = c.String(),
                        Saved = c.Boolean(nullable: false),
                        LayoutViewModel_TailNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SeatId);
            
            CreateTable(
                "dbo.LayoutViewModels",
                c => new
                    {
                        TailNumber = c.String(nullable: false, maxLength: 128),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Left = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TailNumber);
            
            AddColumn("dbo.Planes", "Layout_TailNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.LayoutSeats", "LayoutViewModel_TailNumber");
            CreateIndex("dbo.Planes", "Layout_TailNumber");
            AddForeignKey("dbo.Planes", "Layout_TailNumber", "dbo.LayoutViewModels", "TailNumber");
            AddForeignKey("dbo.LayoutSeats", "LayoutViewModel_TailNumber", "dbo.LayoutViewModels", "TailNumber");
        }
    }
}
