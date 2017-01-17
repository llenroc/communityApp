namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "desc", c => c.String());
            DropColumn("dbo.Chats", "description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "description", c => c.String());
            DropColumn("dbo.Chats", "desc");
        }
    }
}
