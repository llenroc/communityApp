namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial41 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Communities", "secretCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Communities", "secretCode", c => c.String());
        }
    }
}
