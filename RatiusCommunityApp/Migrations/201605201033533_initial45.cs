namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourStart", c => c.Int(nullable: false));
            AddColumn("dbo.CommunityEmergencyContacts", "workingHourEnd", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourEnd");
            DropColumn("dbo.CommunityEmergencyContacts", "workingHourStart");
        }
    }
}
