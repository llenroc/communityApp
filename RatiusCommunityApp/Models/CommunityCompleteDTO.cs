using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityCompleteDTO
    {
        public Community community { get; set; }
        public List<CommunityImage> communityFeaturesImage { get; set; }
        public List<CommunityStreetFloor> communityStreetFloor { get; set; }
        public List<CommunityEmergencyContacts> communityEmergencyContacts { get; set; }
        public List<CommunitySecretCodes> communitySecretKeys { get; set; }
        public string address { get; set; }
        public string streetFloor { get; set; }
        public bool isBlocked { get; set; }
    }
}