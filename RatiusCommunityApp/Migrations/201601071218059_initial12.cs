namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintCatagories", "description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintCatagories", "description", c => c.Int(nullable: false));
        }
    }
}
