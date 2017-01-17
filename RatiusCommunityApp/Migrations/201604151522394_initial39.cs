namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "secretCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "secretCode");
        }
    }
}
