namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "firstName", c => c.String());
            AddColumn("dbo.Users", "lastName", c => c.String());
            AddColumn("dbo.Users", "streetName1", c => c.String());
            AddColumn("dbo.Users", "streetName2", c => c.String());
            AddColumn("dbo.Users", "postalCode", c => c.String());
            AddColumn("dbo.Users", "homePhoneNumber", c => c.String());
            AddColumn("dbo.Users", "mobileNumber", c => c.String());
            AddColumn("dbo.Users", "emergencyContactName1", c => c.String());
            AddColumn("dbo.Users", "emergencyContactNumber1", c => c.String());
            AddColumn("dbo.Users", "emergencyContactName2", c => c.String());
            AddColumn("dbo.Users", "emergencyContactNumber2", c => c.String());
            AddColumn("dbo.Users", "gender", c => c.String());
            DropColumn("dbo.Users", "name");
            DropColumn("dbo.Users", "emergencyNumber");
            DropColumn("dbo.Users", "securityOfficeContactNumber");
            DropColumn("dbo.Users", "neighbourContactNumber");
            DropColumn("dbo.Users", "managementContactNumber");
            DropColumn("dbo.Users", "contactNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "contactNumber", c => c.String());
            AddColumn("dbo.Users", "managementContactNumber", c => c.String());
            AddColumn("dbo.Users", "neighbourContactNumber", c => c.String());
            AddColumn("dbo.Users", "securityOfficeContactNumber", c => c.String());
            AddColumn("dbo.Users", "emergencyNumber", c => c.String());
            AddColumn("dbo.Users", "name", c => c.String());
            DropColumn("dbo.Users", "gender");
            DropColumn("dbo.Users", "emergencyContactNumber2");
            DropColumn("dbo.Users", "emergencyContactName2");
            DropColumn("dbo.Users", "emergencyContactNumber1");
            DropColumn("dbo.Users", "emergencyContactName1");
            DropColumn("dbo.Users", "mobileNumber");
            DropColumn("dbo.Users", "homePhoneNumber");
            DropColumn("dbo.Users", "postalCode");
            DropColumn("dbo.Users", "streetName2");
            DropColumn("dbo.Users", "streetName1");
            DropColumn("dbo.Users", "lastName");
            DropColumn("dbo.Users", "firstName");
        }
    }
}
