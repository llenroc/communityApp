namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommunityContacts", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityContacts", "roleID", "dbo.CommunityContactsRoles");
            DropForeignKey("dbo.CommunityContacts", "serviceID", "dbo.Services");
            DropIndex("dbo.CommunityContacts", new[] { "communityID" });
            DropIndex("dbo.CommunityContacts", new[] { "roleID" });
            DropIndex("dbo.CommunityContacts", new[] { "serviceID" });
            CreateTable(
                "dbo.ComplaintStatus",
                c => new
                    {
                        complaintStatusID = c.Int(nullable: false, identity: true),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.complaintStatusID);
            
            CreateTable(
                "dbo.ReportCatagories",
                c => new
                    {
                        reportCatagoryID = c.Int(nullable: false, identity: true),
                        description = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.reportCatagoryID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        reportTypeID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                        communityID = c.Int(nullable: false),
                        reportCatagoryID = c.Int(nullable: false),
                        description = c.String(),
                        Date = c.DateTime(nullable: false),
                        image1 = c.String(),
                        image2 = c.String(),
                        image3 = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.ReportCatagories", t => t.reportCatagoryID, cascadeDelete: true)
                .ForeignKey("dbo.ReportTypes", t => t.reportTypeID, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.reportTypeID)
                .Index(t => t.userID)
                .Index(t => t.communityID)
                .Index(t => t.reportCatagoryID);
            
            CreateTable(
                "dbo.ReportTypes",
                c => new
                    {
                        reportTypeID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        reportCatagoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.reportTypeID)
                .ForeignKey("dbo.ReportCatagories", t => t.reportCatagoryID, cascadeDelete: true)
                .Index(t => t.reportCatagoryID);
            
            AddColumn("dbo.Communities", "communityAddress", c => c.String());
            AddColumn("dbo.Communities", "totalBuildingFloor", c => c.String());
            AddColumn("dbo.Communities", "totalStreet", c => c.String());
            AddColumn("dbo.Communities", "lat", c => c.Double(nullable: false));
            AddColumn("dbo.Communities", "lng", c => c.Double(nullable: false));
            AddColumn("dbo.Users", "lat", c => c.Double(nullable: false));
            AddColumn("dbo.Users", "lng", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "Image", c => c.String());
            AddColumn("dbo.Complaints", "complaintStatusID", c => c.Int(nullable: false));
            AddColumn("dbo.Complaints", "image1", c => c.String());
            AddColumn("dbo.Complaints", "image2", c => c.String());
            AddColumn("dbo.Complaints", "image3", c => c.String());
            AddColumn("dbo.Members", "address", c => c.String());
            AddColumn("dbo.Members", "buildingFloorNumber", c => c.String());
            AddColumn("dbo.Members", "streetNumber", c => c.String());
            AddColumn("dbo.ServiceStaffs", "image", c => c.String());
            //AddColumn("dbo.ServiceStaffs", "serviceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Complaints", "complaintStatusID");
            //CreateIndex("dbo.ServiceStaffs", "serviceID");
            AddForeignKey("dbo.Complaints", "complaintStatusID", "dbo.ComplaintStatus", "complaintStatusID", cascadeDelete: true);
            //AddForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services", "serviceID", cascadeDelete: true);
            DropColumn("dbo.Complaints", "status");
            DropColumn("dbo.Complaints", "address");
            DropColumn("dbo.Complaints", "image");
            DropColumn("dbo.Complaints", "contactNumber");
            DropTable("dbo.CommunityContacts");
            DropTable("dbo.CommunityContactsRoles");
        }
        
        public override void Down()
        {
            
        }
    }
}
