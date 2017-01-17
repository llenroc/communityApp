namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Complaints", "lat", c => c.Double());
            AlterColumn("dbo.Complaints", "lng", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Complaints", "lng", c => c.Int());
            AlterColumn("dbo.Complaints", "lat", c => c.Int());
        }
    }
}
