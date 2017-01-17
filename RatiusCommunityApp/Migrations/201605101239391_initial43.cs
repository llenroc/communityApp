namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "emergencyContactRange", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "emergencyContactRange");
        }
    }
}
