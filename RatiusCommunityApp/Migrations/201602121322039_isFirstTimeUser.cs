namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isFirstTimeUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "isFirstTimeUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "isFirstTimeUser");
        }
    }
}
