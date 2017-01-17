namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Announcements", "annoucementTypeID", "dbo.AnnouncementTypes");
            DropIndex("dbo.Announcements", new[] { "annoucementTypeID" });
            AddColumn("dbo.Announcements", "image", c => c.String());
            DropColumn("dbo.Announcements", "annoucementTypeID");
            DropColumn("dbo.Announcements", "isMedia");
            DropTable("dbo.AnnouncementTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AnnouncementTypes",
                c => new
                    {
                        annoucementTypeID = c.Int(nullable: false, identity: true),
                        annoucementType = c.String(),
                    })
                .PrimaryKey(t => t.annoucementTypeID);
            
            AddColumn("dbo.Announcements", "isMedia", c => c.Boolean(nullable: false));
            AddColumn("dbo.Announcements", "annoucementTypeID", c => c.Int(nullable: false));
            DropColumn("dbo.Announcements", "image");
            CreateIndex("dbo.Announcements", "annoucementTypeID");
            AddForeignKey("dbo.Announcements", "annoucementTypeID", "dbo.AnnouncementTypes", "annoucementTypeID", cascadeDelete: true);
        }
    }
}
