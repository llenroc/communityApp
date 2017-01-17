namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial40 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommunitySecretKeys",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        secretKey = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommunitySecretKeys", "communityID", "dbo.Communities");
            DropIndex("dbo.CommunitySecretKeys", new[] { "communityID" });
            DropTable("dbo.CommunitySecretKeys");
        }
    }
}
