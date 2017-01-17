namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alerts", "date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Alerts", "memberID", c => c.Int(nullable: false));
            AddColumn("dbo.Alerts", "isViewed", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Alerts", "memberID");
            AddForeignKey("dbo.Alerts", "memberID", "dbo.Members", "id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alerts", "memberID", "dbo.Members");
            DropIndex("dbo.Alerts", new[] { "memberID" });
            DropColumn("dbo.Alerts", "isViewed");
            DropColumn("dbo.Alerts", "memberID");
            DropColumn("dbo.Alerts", "date");
        }
    }
}
