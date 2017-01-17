namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceschanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommunityServices", "serviceID", "dbo.Services");
            DropForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services");
            DropIndex("dbo.CommunityServices", new[] { "serviceID" });
            DropIndex("dbo.ServiceStaffs", new[] { "serviceID" });
            AddColumn("dbo.CommunityServices", "serviceName", c => c.String());
            AddColumn("dbo.CommunityServices", "icon", c => c.String());
            AddColumn("dbo.CommunityServices", "isPredefined", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceStaffs", "communityServiceID", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceStaffs", "communityServiceID");
            AddForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices", "communityServiceID", cascadeDelete: false);
            DropColumn("dbo.CommunityServices", "serviceID");
            DropColumn("dbo.ServiceStaffs", "serviceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceStaffs", "serviceID", c => c.Int(nullable: false));
            AddColumn("dbo.CommunityServices", "serviceID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ServiceStaffs", "communityServiceID", "dbo.CommunityServices");
            DropIndex("dbo.ServiceStaffs", new[] { "communityServiceID" });
            DropColumn("dbo.ServiceStaffs", "communityServiceID");
            DropColumn("dbo.CommunityServices", "isPredefined");
            DropColumn("dbo.CommunityServices", "icon");
            DropColumn("dbo.CommunityServices", "serviceName");
            CreateIndex("dbo.ServiceStaffs", "serviceID");
            CreateIndex("dbo.CommunityServices", "serviceID");
            AddForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services", "serviceID", cascadeDelete: true);
            AddForeignKey("dbo.CommunityServices", "serviceID", "dbo.Services", "serviceID", cascadeDelete: false);
        }
    }
}
