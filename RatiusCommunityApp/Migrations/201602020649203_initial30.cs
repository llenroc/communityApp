namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complaints", "desc", c => c.String());
            DropColumn("dbo.Complaints", "description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complaints", "description", c => c.String());
            DropColumn("dbo.Complaints", "desc");
        }
    }
}
