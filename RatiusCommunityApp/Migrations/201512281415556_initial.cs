namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "emergencyNumber", c => c.String());
            AlterColumn("dbo.Users", "securityOfficeContactNumber", c => c.String());
            AlterColumn("dbo.Users", "neighbourContactNumber", c => c.String());
            AlterColumn("dbo.Users", "managementContactNumber", c => c.String());
            AlterColumn("dbo.Users", "contactNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "contactNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "managementContactNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "neighbourContactNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "securityOfficeContactNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "emergencyNumber", c => c.Int(nullable: false));
        }
    }
}
