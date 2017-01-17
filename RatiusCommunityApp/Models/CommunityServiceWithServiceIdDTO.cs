using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityServiceWithServiceIdDTO
    {
        public int serviceID { get; set; }
        public CommunityService communityService { get; set; }
        
    }
}