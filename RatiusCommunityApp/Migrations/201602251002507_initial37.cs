namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "isAlertBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "isAlertBlocked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "isAlertBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Members", "isAlertBlocked");
        }
    }
}
