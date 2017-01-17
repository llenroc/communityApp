namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommunitySecretKeys", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunitySecretKeys", new[] { "communityID" });
            CreateTable(
                "dbo.CommunitySecretCodes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        secretCode = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            DropTable("dbo.CommunitySecretKeys");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CommunitySecretKeys",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        secretKey = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("dbo.CommunitySecretCodes", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunitySecretCodes", new[] { "communityID" });
            DropTable("dbo.CommunitySecretCodes");
            CreateIndex("dbo.CommunitySecretKeys", "communityID");
            AddForeignKey("dbo.CommunitySecretKeys", "communityID", "dbo.Communities", "communityID", cascadeDelete: true);
        }
    }
}
