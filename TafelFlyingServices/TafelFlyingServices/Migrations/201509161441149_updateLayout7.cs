namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLayout7 : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.TailNumber)
                .ForeignKey("dbo.Planes", t => t.TailNumber)
                .Index(t => t.TailNumber);
            
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
                .PrimaryKey(t => t.SeatId)
                .ForeignKey("dbo.LayoutViewModels", t => t.LayoutViewModel_TailNumber)
                .Index(t => t.LayoutViewModel_TailNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LayoutSeats", "LayoutViewModel_TailNumber", "dbo.LayoutViewModels");
            DropForeignKey("dbo.LayoutViewModels", "TailNumber", "dbo.Planes");
            DropIndex("dbo.LayoutSeats", new[] { "LayoutViewModel_TailNumber" });
            DropIndex("dbo.LayoutViewModels", new[] { "TailNumber" });
            DropTable("dbo.LayoutSeats");
            DropTable("dbo.LayoutViewModels");
        }
    }
}
