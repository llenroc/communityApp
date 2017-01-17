namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "date");
        }
    }
}
