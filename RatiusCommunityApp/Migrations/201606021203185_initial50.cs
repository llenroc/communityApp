namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial50 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SubCommunitySelectedStreetFloors", name: "streetFloorId", newName: "communityStreetFloorId");
            RenameIndex(table: "dbo.SubCommunitySelectedStreetFloors", name: "IX_streetFloorId", newName: "IX_communityStreetFloorId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SubCommunitySelectedStreetFloors", name: "IX_communityStreetFloorId", newName: "IX_streetFloorId");
            RenameColumn(table: "dbo.SubCommunitySelectedStreetFloors", name: "communityStreetFloorId", newName: "streetFloorId");
        }
    }
}
