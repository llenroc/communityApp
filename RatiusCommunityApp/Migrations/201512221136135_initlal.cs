namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initlal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityContacts", "serviceID", c => c.Int(nullable: false));
            CreateIndex("dbo.CommunityContacts", "serviceID");
            AddForeignKey("dbo.CommunityContacts", "serviceID", "dbo.Services", "serviceID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommunityContacts", "serviceID", "dbo.Services");
            DropIndex("dbo.CommunityContacts", new[] { "serviceID" });
            DropColumn("dbo.CommunityContacts", "serviceID");
        }
    }
}
