using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Report
    {
        public int id { get; set; }
        public int reportTypeID { get; set; }
        public ReportType reportType { get; set; }
        public int userID { get; set; }
        public User user { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        public int reportCatagoryID { get; set; }
        public ReportCatagory reportCatagory { get; set; }
        public string description { get; set; }
        public DateTime Date { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }

    }
}