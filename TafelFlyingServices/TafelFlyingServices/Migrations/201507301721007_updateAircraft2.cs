using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class updateAircraft2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Aircraft", "Make", c => c.String(false));
            AlterColumn("dbo.Aircraft", "Model", c => c.String(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Aircraft", "Model", c => c.String());
            AlterColumn("dbo.Aircraft", "Make", c => c.String());
        }
    }
}