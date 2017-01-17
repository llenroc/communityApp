namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComplaintTypes", "complaintCatagoryID", "dbo.ComplaintCatagories");
            DropForeignKey("dbo.Complaints", "typeID", "dbo.ComplaintTypes");
            DropIndex("dbo.Complaints", new[] { "typeID" });
            DropIndex("dbo.ComplaintTypes", new[] { "complaintCatagoryID" });
            DropColumn("dbo.Complaints", "typeID");
            DropTable("dbo.ComplaintTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ComplaintTypes",
                c => new
                    {
                        typeID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        complaintCatagoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.typeID);
            
            AddColumn("dbo.Complaints", "typeID", c => c.Int(nullable: false));
            CreateIndex("dbo.ComplaintTypes", "complaintCatagoryID");
            CreateIndex("dbo.Complaints", "typeID");
            AddForeignKey("dbo.Complaints", "typeID", "dbo.ComplaintTypes", "typeID", cascadeDelete: true);
            AddForeignKey("dbo.ComplaintTypes", "complaintCatagoryID", "dbo.ComplaintCatagories", "complaintCatagoryID", cascadeDelete: true);
        }
    }
}
