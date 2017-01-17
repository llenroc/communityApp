using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SelectedAndUnselectedCommunitiesDTO
    {
        public List<Community> SelectedCommunities { get; set; }
        public List<Community> UnselectedCommunities { get; set; }
    }
}