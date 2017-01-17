namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial46 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourStart");
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourEnd", c => c.Int(nullable: false));
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourStart", c => c.Int(nullable: false));
        }
    }
}
