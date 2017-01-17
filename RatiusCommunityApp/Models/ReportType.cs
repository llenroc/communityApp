using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ReportType
    {
        public int reportTypeID { get; set; }
        public string description { get; set; }
        public int reportCatagoryID { get; set; }
        public ReportCatagory reportCatagory { get; set; }
    }
}