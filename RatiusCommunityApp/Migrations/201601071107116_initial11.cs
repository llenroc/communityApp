namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComplaintTypes", "communityID", "dbo.Communities");
            DropIndex("dbo.ComplaintTypes", new[] { "communityID" });
            CreateTable(
                "dbo.ComplaintCatagories",
                c => new
                    {
                        complaintCatagoryID = c.Int(nullable: false, identity: true),
                        description = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.complaintCatagoryID);
            
            AddColumn("dbo.Complaints", "complaintCatagoryID", c => c.Int(nullable: false));
            AddColumn("dbo.ComplaintTypes", "complaintCatagoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Complaints", "complaintCatagoryID");
            CreateIndex("dbo.ComplaintTypes", "complaintCatagoryID");
            AddForeignKey("dbo.Complaints", "complaintCatagoryID", "dbo.ComplaintCatagories", "complaintCatagoryID", cascadeDelete: true);
            AddForeignKey("dbo.ComplaintTypes", "complaintCatagoryID", "dbo.ComplaintCatagories", "complaintCatagoryID", cascadeDelete: false);
            DropColumn("dbo.ComplaintTypes", "communityID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplaintTypes", "communityID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ComplaintTypes", "complaintCatagoryID", "dbo.ComplaintCatagories");
            DropForeignKey("dbo.Complaints", "complaintCatagoryID", "dbo.ComplaintCatagories");
            DropIndex("dbo.ComplaintTypes", new[] { "complaintCatagoryID" });
            DropIndex("dbo.Complaints", new[] { "complaintCatagoryID" });
            DropColumn("dbo.ComplaintTypes", "complaintCatagoryID");
            DropColumn("dbo.Complaints", "complaintCatagoryID");
            DropTable("dbo.ComplaintCatagories");
            CreateIndex("dbo.ComplaintTypes", "communityID");
            AddForeignKey("dbo.ComplaintTypes", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
        }
    }
}
