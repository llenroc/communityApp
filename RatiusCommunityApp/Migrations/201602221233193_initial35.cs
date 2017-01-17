namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "language", c => c.String());
            AddColumn("dbo.Users", "userType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "userType");
            DropColumn("dbo.Users", "language");
        }
    }
}
