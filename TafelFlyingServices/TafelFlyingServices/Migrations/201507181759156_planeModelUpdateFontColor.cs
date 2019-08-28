namespace TafelFlyingServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class planeModelUpdateFontColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Planes", "FontColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Planes", "FontColor");
        }
    }
}
