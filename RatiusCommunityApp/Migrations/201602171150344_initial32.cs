namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial32 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectoryImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DirectoryImages");
        }
    }
}
