namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial44 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Islogout", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Islogout");
        }
    }
}
