namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceStaffs", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceStaffs", "isActive");
        }
    }
}
