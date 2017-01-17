namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial31 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Alerts", "description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Alerts", "description", c => c.String());
        }
    }
}
