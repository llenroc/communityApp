namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "communityID", "dbo.Communities");
            DropIndex("dbo.Chats", new[] { "communityID" });
            DropColumn("dbo.Chats", "communityID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "communityID", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "communityID");
            AddForeignKey("dbo.Chats", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
        }
    }
}
