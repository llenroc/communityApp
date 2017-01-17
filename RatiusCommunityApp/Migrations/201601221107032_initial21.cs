namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complaints", "lat", c => c.Int());
            AddColumn("dbo.Complaints", "lng", c => c.Int());
            AddColumn("dbo.Complaints", "address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Complaints", "address");
            DropColumn("dbo.Complaints", "lng");
            DropColumn("dbo.Complaints", "lat");
        }
    }
}
