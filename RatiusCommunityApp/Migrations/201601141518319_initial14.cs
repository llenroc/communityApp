namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceStaffs", "staffRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceStaffs", "staffRole");
        }
    }
}
