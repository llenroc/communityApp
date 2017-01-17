namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "country", c => c.String());
            AddColumn("dbo.Users", "ContactNumber", c => c.String());
            DropColumn("dbo.Users", "address");
            DropColumn("dbo.Users", "streetName1");
            DropColumn("dbo.Users", "streetName2");
            DropColumn("dbo.Users", "postalCode");
            DropColumn("dbo.Users", "homePhoneNumber");
            DropColumn("dbo.Users", "mobileNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "mobileNumber", c => c.String());
            AddColumn("dbo.Users", "homePhoneNumber", c => c.String());
            AddColumn("dbo.Users", "postalCode", c => c.String());
            AddColumn("dbo.Users", "streetName2", c => c.String());
            AddColumn("dbo.Users", "streetName1", c => c.String());
            AddColumn("dbo.Users", "address", c => c.String());
            DropColumn("dbo.Users", "ContactNumber");
            DropColumn("dbo.Users", "country");
        }
    }
}
