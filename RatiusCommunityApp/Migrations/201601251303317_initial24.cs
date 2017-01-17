namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial24 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Announcements", "dateTo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "dateTo", c => c.String());
        }
    }
}
