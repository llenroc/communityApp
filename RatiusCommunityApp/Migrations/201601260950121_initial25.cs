namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommunityEmergencyContacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contact = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.CommunityImages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        image = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.CommunityStreetFloors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        streetFloor = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            AddColumn("dbo.Communities", "about", c => c.String());
            DropColumn("dbo.Communities", "totalBuildingFloor");
            DropColumn("dbo.Communities", "totalStreet");
            DropColumn("dbo.Communities", "logoImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Communities", "logoImage", c => c.String());
            AddColumn("dbo.Communities", "totalStreet", c => c.String());
            AddColumn("dbo.Communities", "totalBuildingFloor", c => c.String());
            DropForeignKey("dbo.CommunityStreetFloors", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityImages", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityEmergencyContacts", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunityStreetFloors", new[] { "communityID" });
            DropIndex("dbo.CommunityImages", new[] { "communityID" });
            DropIndex("dbo.CommunityEmergencyContacts", new[] { "communityID" });
            DropColumn("dbo.Communities", "about");
            DropTable("dbo.CommunityStreetFloors");
            DropTable("dbo.CommunityImages");
            DropTable("dbo.CommunityEmergencyContacts");
        }
    }
}
