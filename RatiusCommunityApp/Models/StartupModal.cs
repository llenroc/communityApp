using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class StartupModal
    {
        public Community community { get; set; }
        public User user { get; set; }
        public List<CommunityImage> communityFeaturesImage { get; set; }
        public List<DirectoryImages> directoryImage { get; set; }
        public List<CommunityStreetFloor> communityStreetFloor { get; set; }
        public List<Service> communityServices { get; set; }

        public List<CommunityService> selectedCommunityServices { get; set; }
    }
}