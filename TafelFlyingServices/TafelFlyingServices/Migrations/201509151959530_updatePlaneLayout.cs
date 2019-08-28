namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePlaneLayout : DbMigration
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
                .PrimaryKey(t => t.TailNumber);
            
            CreateTable(
                "dbo.LayoutSeats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Left = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                        Direction = c.String(),
                        LayoutViewModel_TailNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LayoutViewModels", t => t.LayoutViewModel_TailNumber)
                .Index(t => t.LayoutViewModel_TailNumber);
            
            AddColumn("dbo.Planes", "Layout_TailNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.Planes", "Layout_TailNumber");
            AddForeignKey("dbo.Planes", "Layout_TailNumber", "dbo.LayoutViewModels", "TailNumber");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Planes", "Layout_TailNumber", "dbo.LayoutViewModels");
            DropForeignKey("dbo.LayoutSeats", "LayoutViewModel_TailNumber", "dbo.LayoutViewModels");
            DropIndex("dbo.LayoutSeats", new[] { "LayoutViewModel_TailNumber" });
            DropIndex("dbo.Planes", new[] { "Layout_TailNumber" });
            DropColumn("dbo.Planes", "Layout_TailNumber");
            DropTable("dbo.LayoutSeats");
            DropTable("dbo.LayoutViewModels");
        }
    }
}
