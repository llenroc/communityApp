namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices");
            DropIndex("dbo.ServiceStaffs", new[] { "communityServiceID" });
            //DropColumn("dbo.ServiceStaffs", "communityServiceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceStaffs", "communityServiceID", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceStaffs", "communityServiceID");
            AddForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices", "communityServiceID", cascadeDelete: true);
        }
    }
}
