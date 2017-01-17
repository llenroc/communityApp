namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial34 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        Report = c.Boolean(nullable: false),
                        Notices = c.Boolean(nullable: false),
                        Messages = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.userID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "userID", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "userID" });
            DropTable("dbo.Notifications");
        }
    }
}
