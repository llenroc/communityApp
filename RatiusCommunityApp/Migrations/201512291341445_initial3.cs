namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "dateTo", c => c.String());
            AddColumn("dbo.Announcements", "dateFrom", c => c.String());
            AddColumn("dbo.Chats", "image", c => c.String());
            DropColumn("dbo.Chats", "isMedia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "isMedia", c => c.Boolean(nullable: false));
            DropColumn("dbo.Chats", "image");
            DropColumn("dbo.Announcements", "dateFrom");
            DropColumn("dbo.Announcements", "dateTo");
        }
    }
}
