using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunitySettingsDTO
    {
        public List<Community> communityList { get; set; }
        public List<SubCommunity> subCommunityList { get; set; }
        public List<CommunityStreetFloor> allStreetFloors { get; set; }
        public List<int> selectedStreetFloorsIDs { get; set; }

    }
}