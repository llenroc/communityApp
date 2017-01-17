namespace RatiusCommunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initla : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        description = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.userID)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.Communities",
                c => new
                    {
                        communityID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        adminUserID = c.Int(nullable: false),
                        coverImage = c.String(),
                        logoImage = c.String(),
                    })
                .PrimaryKey(t => t.communityID)
                .ForeignKey("dbo.Users", t => t.adminUserID, cascadeDelete: false)
                .Index(t => t.adminUserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        emailID = c.String(),
                        name = c.String(),
                        password = c.String(),
                        emergencyNumber = c.Int(nullable: false),
                        securityOfficeContactNumber = c.Int(nullable: false),
                        neighbourContactNumber = c.Int(nullable: false),
                        managementContactNumber = c.Int(nullable: false),
                        contactNumber = c.Int(nullable: false),
                        isMainAdmin = c.Boolean(nullable: false),
                        address = c.String(),
                        image = c.String(),
                        isBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.userID);
            
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        annoucementTypeID = c.Int(nullable: false),
                        description = c.String(),
                        communityID = c.Int(nullable: false),
                        isMedia = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AnnouncementTypes", t => t.annoucementTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.annoucementTypeID)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.AnnouncementTypes",
                c => new
                    {
                        annoucementTypeID = c.Int(nullable: false, identity: true),
                        annoucementType = c.String(),
                    })
                .PrimaryKey(t => t.annoucementTypeID);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        chatMessageID = c.Int(nullable: false, identity: true),
                        messageID = c.Int(nullable: false),
                        description = c.String(),
                        isRead = c.Boolean(nullable: false),
                        isMedia = c.Boolean(nullable: false),
                        communityID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        to = c.Int(nullable: false),
                        from = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.chatMessageID)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.messageID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.from, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.to, cascadeDelete: false)
                .Index(t => t.messageID)
                .Index(t => t.communityID)
                .Index(t => t.to)
                .Index(t => t.from);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        messageID = c.Int(nullable: false, identity: true),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.messageID)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: false)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.CommunityContacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        communityID = c.Int(nullable: false),
                        name = c.String(),
                        contactNumber = c.String(),
                        roleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.CommunityContactsRoles", t => t.roleID, cascadeDelete: true)
                .Index(t => t.communityID)
                .Index(t => t.roleID);
            
            CreateTable(
                "dbo.CommunityContactsRoles",
                c => new
                    {
                        roleID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.roleID);
            
            CreateTable(
                "dbo.CommunityServices",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        communityID = c.Int(nullable: false),
                        serviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.serviceID, cascadeDelete: true)
                .Index(t => t.communityID)
                .Index(t => t.serviceID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        serviceID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.serviceID);
            
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        complaintID = c.Int(nullable: false, identity: true),
                        status = c.String(),
                        userID = c.Int(nullable: false),
                        description = c.String(),
                        Date = c.DateTime(nullable: false),
                        typeID = c.Int(nullable: false),
                        communityID = c.Int(nullable: false),
                        address = c.String(),
                        image = c.String(),
                        contactNumber = c.String(),
                    })
                .PrimaryKey(t => t.complaintID)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: false)
                .ForeignKey("dbo.ComplaintTypes", t => t.typeID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.userID)
                .Index(t => t.typeID)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.ComplaintTypes",
                c => new
                    {
                        typeID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.typeID)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        communityID = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.communityID)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.ServiceStaffs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        serviceID = c.Int(nullable: false),
                        contactNumber = c.String(),
                        emailID = c.String(),
                        communityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Communities", t => t.communityID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.serviceID, cascadeDelete: true)
                .Index(t => t.serviceID)
                .Index(t => t.communityID);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        complaintID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Complaints", t => t.complaintID, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.complaintID)
                .Index(t => t.userID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "userID", "dbo.Users");
            DropForeignKey("dbo.Votes", "complaintID", "dbo.Complaints");
            DropForeignKey("dbo.ServiceStaffs", "serviceID", "dbo.Services");
            DropForeignKey("dbo.ServiceStaffs", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Members", "userId", "dbo.Users");
            DropForeignKey("dbo.Members", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Complaints", "userID", "dbo.Users");
            DropForeignKey("dbo.Complaints", "typeID", "dbo.ComplaintTypes");
            DropForeignKey("dbo.ComplaintTypes", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Complaints", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityServices", "serviceID", "dbo.Services");
            DropForeignKey("dbo.CommunityServices", "communityID", "dbo.Communities");
            DropForeignKey("dbo.CommunityContacts", "roleID", "dbo.CommunityContactsRoles");
            DropForeignKey("dbo.CommunityContacts", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Chats", "to", "dbo.Users");
            DropForeignKey("dbo.Chats", "from", "dbo.Users");
            DropForeignKey("dbo.Chats", "messageID", "dbo.Messages");
            DropForeignKey("dbo.Messages", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Chats", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Announcements", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Announcements", "annoucementTypeID", "dbo.AnnouncementTypes");
            DropForeignKey("dbo.Alerts", "userID", "dbo.Users");
            DropForeignKey("dbo.Alerts", "communityID", "dbo.Communities");
            DropForeignKey("dbo.Communities", "adminUserID", "dbo.Users");
            DropIndex("dbo.Votes", new[] { "userID" });
            DropIndex("dbo.Votes", new[] { "complaintID" });
            DropIndex("dbo.ServiceStaffs", new[] { "communityID" });
            DropIndex("dbo.ServiceStaffs", new[] { "serviceID" });
            DropIndex("dbo.Members", new[] { "userId" });
            DropIndex("dbo.Members", new[] { "communityID" });
            DropIndex("dbo.ComplaintTypes", new[] { "communityID" });
            DropIndex("dbo.Complaints", new[] { "communityID" });
            DropIndex("dbo.Complaints", new[] { "typeID" });
            DropIndex("dbo.Complaints", new[] { "userID" });
            DropIndex("dbo.CommunityServices", new[] { "serviceID" });
            DropIndex("dbo.CommunityServices", new[] { "communityID" });
            DropIndex("dbo.CommunityContacts", new[] { "roleID" });
            DropIndex("dbo.CommunityContacts", new[] { "communityID" });
            DropIndex("dbo.Messages", new[] { "communityID" });
            DropIndex("dbo.Chats", new[] { "from" });
            DropIndex("dbo.Chats", new[] { "to" });
            DropIndex("dbo.Chats", new[] { "communityID" });
            DropIndex("dbo.Chats", new[] { "messageID" });
            DropIndex("dbo.Announcements", new[] { "communityID" });
            DropIndex("dbo.Announcements", new[] { "annoucementTypeID" });
            DropIndex("dbo.Communities", new[] { "adminUserID" });
            DropIndex("dbo.Alerts", new[] { "communityID" });
            DropIndex("dbo.Alerts", new[] { "userID" });
            DropTable("dbo.Votes");
            DropTable("dbo.ServiceStaffs");
            DropTable("dbo.Members");
            DropTable("dbo.ComplaintTypes");
            DropTable("dbo.Complaints");
            DropTable("dbo.Services");
            DropTable("dbo.CommunityServices");
            DropTable("dbo.CommunityContactsRoles");
            DropTable("dbo.CommunityContacts");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
            DropTable("dbo.AnnouncementTypes");
            DropTable("dbo.Announcements");
            DropTable("dbo.Users");
            DropTable("dbo.Communities");
            DropTable("dbo.Alerts");
        }
    }
}
