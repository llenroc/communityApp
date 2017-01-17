using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class RatiusCommunityAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public RatiusCommunityAppContext() : base("name=RatiusCommunityAppContext")
        {
        }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Alert> Alerts { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Community> Communities { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Announcement> Announcements { get; set; }


        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Chat> Chats { get; set; }

       

    

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunityService> CommunityServices { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Service> Services { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Complaint> Complaints { get; set; }

        //public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ComplaintType> ComplaintTypes { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Member> Members { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ServiceStaff> ServiceStaffs { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Vote> Votes { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ComplaintStatus> ComplaintStatus { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Report> Reports { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ReportCatagory> ReportCatagories { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ReportType> ReportTypes { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.ComplaintCatagory> ComplaintCatagories { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunityStreetFloor> CommunityStreetFloors { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunityImage> CommunityImages { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunityEmergencyContacts> CommunityEmergencyContacts { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.DirectoryImages> DirectoryImages { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunityiReports> CommunityiReports { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.CommunitySecretCodes> CommunitySecretKeys { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.SubCommunity> SubCommunities { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.SubCommunitySelectedStreetFloor> SubCommunitySelectedStreetFloors { get; set; }

        public System.Data.Entity.DbSet<RatiusCommunityApp.Models.SubCommunityEmergencyContacts> SubCommunityEmergencyContacts { get; set; }

   
    
    }
}
