namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Chats", "messageID", "dbo.Messages");
            DropIndex("dbo.Chats", new[] { "messageID" });
            DropIndex("dbo.Messages", new[] { "communityID" });
            DropColumn("dbo.Chats", "messageID");
            DropTable("dbo.Messages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        messageID = c.Int(nullable: false, identity: true),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.messageID);
            
            AddColumn("dbo.Chats", "messageID", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "communityID");
            CreateIndex("dbo.Chats", "messageID");
            AddForeignKey("dbo.Chats", "messageID", "dbo.Messages", "messageID", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
        }
    }
}
