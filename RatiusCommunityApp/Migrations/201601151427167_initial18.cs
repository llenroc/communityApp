namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "isBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "isBlocked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "isBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Members", "isBlocked");
        }
    }
}
