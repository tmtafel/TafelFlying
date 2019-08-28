using System.Data.Entity.Migrations;

namespace TafelFlyingServices.Migrations
{
    public partial class newDatabase : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.PlaneAccesses");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.PlaneAccesses",
                c => new
                {
                    Id = c.Int(false, true),
                    PlaneId = c.String(),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
    }
}