namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CommunityServices", "id", "communityServiceID");
            //DropForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services");
            //DropForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices");
            //DropIndex("dbo.ServiceStaffs", new[] { "serviceID" });
            //DropPrimaryKey("dbo.CommunityServices");
           
            //AddColumn("dbo.CommunityServices", "communityServiceID", c => c.Int(nullable: false, identity: true));
            //AddColumn("dbo.ServiceStaffs", "communityServiceID", c => c.Int(nullable: false));
            //DropColumn("dbo.CommunityServices", "id");
            //AddPrimaryKey("dbo.CommunityServices", "communityServiceID");
            //CreateIndex("dbo.ServiceStaffs", "communityServiceID");
            //AddForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices", "communityServiceID", cascadeDelete: true);
            //DropColumn("dbo.ServiceStaffs", "serviceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceStaffs", "serviceID", c => c.Int(nullable: false));
            AddColumn("dbo.CommunityServices", "id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices");
            DropIndex("dbo.ServiceStaffs", new[] { "communityServiceID" });
            DropPrimaryKey("dbo.CommunityServices");
            DropColumn("dbo.ServiceStaffs", "communityServiceID");
            DropColumn("dbo.CommunityServices", "communityServiceID");
            AddPrimaryKey("dbo.CommunityServices", "id");
            CreateIndex("dbo.ServiceStaffs", "serviceID");
            AddForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices", "communityServiceID", cascadeDelete: true);
            AddForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services", "serviceID", cascadeDelete: true);
        }
    }
}
