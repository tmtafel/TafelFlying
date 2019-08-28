using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class addblacktext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Planes", "BlackText", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Planes", "BlackText");
        }
    }
}