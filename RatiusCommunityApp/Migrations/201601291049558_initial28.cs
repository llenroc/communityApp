namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "streetFloor", c => c.String());
            DropColumn("dbo.Members", "buildingFloorNumber");
            DropColumn("dbo.Members", "streetNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "streetNumber", c => c.String());
            AddColumn("dbo.Members", "buildingFloorNumber", c => c.String());
            DropColumn("dbo.Members", "streetFloor");
        }
    }
}
