namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "communityPassword", c => c.String());
            AddColumn("dbo.Communities", "isChangePassword", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "isChangePassword");
            DropColumn("dbo.Communities", "communityPassword");
        }
    }
}
