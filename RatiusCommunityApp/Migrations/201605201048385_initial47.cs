namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourStart", c => c.String());
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourEnd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourEnd");
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourStart");
        }
    }
}
