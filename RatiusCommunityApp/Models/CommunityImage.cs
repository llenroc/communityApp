using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityImage
    {
        public int id { get; set; }
        public string image { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        
    }
}