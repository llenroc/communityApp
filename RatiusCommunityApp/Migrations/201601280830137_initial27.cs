namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial27 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommunityEmergencyContacts", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityImages", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityStreetFloors", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunityEmergencyContacts", new[] { "communityID" });
            DropIndex("dbo.CommunityImages", new[] { "communityID" });
            DropIndex("dbo.CommunityStreetFloors", new[] { "communityID" });
            AlterColumn("dbo.CommunityEmergencyContacts", "communityID", c => c.Int(nullable: false));
            AlterColumn("dbo.CommunityImages", "communityID", c => c.Int(nullable: false));
            AlterColumn("dbo.CommunityStreetFloors", "communityID", c => c.Int(nullable: false));
            CreateIndex("dbo.CommunityEmergencyContacts", "communityID");
            CreateIndex("dbo.CommunityImages", "communityID");
            CreateIndex("dbo.CommunityStreetFloors", "communityID");
            AddForeignKey("dbo.CommunityEmergencyContacts", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
            AddForeignKey("dbo.CommunityImages", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
            AddForeignKey("dbo.CommunityStreetFloors", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommunityStreetFloors", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityImages", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityEmergencyContacts", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunityStreetFloors", new[] { "communityID" });
            DropIndex("dbo.CommunityImages", new[] { "communityID" });
            DropIndex("dbo.CommunityEmergencyContacts", new[] { "communityID" });
            AlterColumn("dbo.CommunityStreetFloors", "communityID", c => c.Int());
            AlterColumn("dbo.CommunityImages", "communityID", c => c.Int());
            AlterColumn("dbo.CommunityEmergencyContacts", "communityID", c => c.Int());
            CreateIndex("dbo.CommunityStreetFloors", "communityID");
            CreateIndex("dbo.CommunityImages", "communityID");
            CreateIndex("dbo.CommunityEmergencyContacts", "communityID");
            AddForeignKey("dbo.CommunityStreetFloors", "communityID", "dbo.Communities", "communityID");
            AddForeignKey("dbo.CommunityImages", "communityID", "dbo.Communities", "communityID");
            AddForeignKey("dbo.CommunityEmergencyContacts", "communityID", "dbo.Communities", "communityID");
        }
    }
}
