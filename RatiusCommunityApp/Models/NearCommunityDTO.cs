using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class NearCommunityDTO
    {
        public Community community { get; set; }
        public double distance { get; set; }
    }
}