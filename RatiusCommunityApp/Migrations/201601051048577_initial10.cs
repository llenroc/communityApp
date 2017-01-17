namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "contact", c => c.String());
            DropColumn("dbo.Users", "contactNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "contactNumber", c => c.String());
            DropColumn("dbo.Users", "contact");
        }
    }
}
