namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initital : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityEmergencyContacts", "name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommunityEmergencyContacts", "name");
        }
    }
}
