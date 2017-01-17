using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Complaint
    {
        [Key]
        public int complaintID { get; set; }
        public int complaintStatusID { get; set; }
        public ComplaintStatus complaintStatus { get; set; }

        public int userID { get; set; }

        public User user { get; set; }
        public string desc { get; set; }

        public DateTime Date { get; set; }

        public double? lat { get; set; }
        public double? lng { get; set; }
        public string address { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        //public int complaintCatagoryID { get; set; }
        //public ComplaintCatagory complaintCatagory { get; set; }
        public int CommunityiReportsID { get; set; }
        public CommunityiReports CommunityiReports { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }


    }
}