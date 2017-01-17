namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial33 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Complaints", "complaintCatagoryID", "dbo.ComplaintCatagories");
            DropIndex("dbo.Complaints", new[] { "complaintCatagoryID" });
            CreateTable(
                "dbo.CommunityiReports",
                c => new
                    {
                        CommunityiReportsID = c.Int(nullable: false, identity: true),
                        iReportsName = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommunityiReportsID)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            AddColumn("dbo.Complaints", "CommunityiReportsID", c => c.Int(nullable: false));
            CreateIndex("dbo.Complaints", "CommunityiReportsID");
            AddForeignKey("dbo.Complaints", "CommunityiReportsID", "dbo.CommunityiReports", "CommunityiReportsID", cascadeDelete: true);
            DropColumn("dbo.Complaints", "complaintCatagoryID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complaints", "complaintCatagoryID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Complaints", "CommunityiReportsID", "dbo.CommunityiReports");
            DropForeignKey("dbo.CommunityiReports", "communityID", "dbo.Communities");
            DropIndex("dbo.Complaints", new[] { "CommunityiReportsID" });
            DropIndex("dbo.CommunityiReports", new[] { "communityID" });
            DropColumn("dbo.Complaints", "CommunityiReportsID");
            DropTable("dbo.CommunityiReports");
            CreateIndex("dbo.Complaints", "complaintCatagoryID");
            AddForeignKey("dbo.Complaints", "complaintCatagoryID", "dbo.ComplaintCatagories", "complaintCatagoryID", cascadeDelete: true);
        }
    }
}
