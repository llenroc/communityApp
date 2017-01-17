using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityDashboardDTO
    {
        public bool isFirstTime { get; set; }
        public int NewiReports { get; set; }
        public int NewMessages { get; set; }
    }
}