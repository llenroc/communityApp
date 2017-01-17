using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityiReports
    {
        [Key]
        public int CommunityiReportsID { get; set; }
        public string iReportsName { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }

        

    }
}