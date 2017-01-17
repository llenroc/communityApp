namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial49 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCommunities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.SubCommunityEmergencyContacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        contact = c.String(),
                        workingHourStart = c.String(),
                        workingHourEnd = c.String(),
                        subCommunityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SubCommunities", t => t.subCommunityId, cascadeDelete: true)
                .Index(t => t.subCommunityId);
            
            CreateTable(
                "dbo.SubCommunitySelectedStreetFloors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        streetFloorId = c.Int(nullable: false),
                        subCommunityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CommunityStreetFloors", t => t.streetFloorId, cascadeDelete: true)
                .ForeignKey("dbo.SubCommunities", t => t.subCommunityId, cascadeDelete: false)
                .Index(t => t.streetFloorId)
                .Index(t => t.subCommunityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubCommunitySelectedStreetFloors", "subCommunityId", "dbo.SubCommunities");
            DropForeignKey("dbo.SubCommunitySelectedStreetFloors", "streetFloorId", "dbo.CommunityStreetFloors");
            DropForeignKey("dbo.SubCommunityEmergencyContacts", "subCommunityId", "dbo.SubCommunities");
            DropForeignKey("dbo.SubCommunities", "communityID", "dbo.Communities");
            DropIndex("dbo.SubCommunitySelectedStreetFloors", new[] { "subCommunityId" });
            DropIndex("dbo.SubCommunitySelectedStreetFloors", new[] { "streetFloorId" });
            DropIndex("dbo.SubCommunityEmergencyContacts", new[] { "subCommunityId" });
            DropIndex("dbo.SubCommunities", new[] { "communityID" });
            DropTable("dbo.SubCommunitySelectedStreetFloors");
            DropTable("dbo.SubCommunityEmergencyContacts");
            DropTable("dbo.SubCommunities");
        }
    }
}
