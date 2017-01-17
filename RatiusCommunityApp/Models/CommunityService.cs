using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityService
    {
        [Key]
        public int communityServiceID { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        public string serviceName { get; set; }
        public string icon { get; set; }
        public bool isPredefined { get; set; }
    }
}