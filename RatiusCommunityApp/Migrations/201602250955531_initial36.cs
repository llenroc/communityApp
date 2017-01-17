namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "isAlertBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "isAlertBlocked");
        }
    }
}
