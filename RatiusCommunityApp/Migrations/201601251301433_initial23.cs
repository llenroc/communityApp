namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "shortDescription", c => c.String());
            AddColumn("dbo.Announcements", "message", c => c.String());
            AddColumn("dbo.Announcements", "title", c => c.String());
            DropColumn("dbo.Announcements", "description");
            DropColumn("dbo.Announcements", "dateFrom");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "dateFrom", c => c.String());
            AddColumn("dbo.Announcements", "description", c => c.String());
            DropColumn("dbo.Announcements", "title");
            DropColumn("dbo.Announcements", "message");
            DropColumn("dbo.Announcements", "shortDescription");
        }
    }
}
