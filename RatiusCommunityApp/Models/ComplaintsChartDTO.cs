using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ComplaintsChartDTO
    {
        public string iReportName { get; set; }
        public int totalUpdateiReport { get; set; }
        public int totalClosediReport { get; set; }
        public int totalReceivediReport { get; set; }
    }
}