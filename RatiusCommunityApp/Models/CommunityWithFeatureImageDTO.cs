using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityWithFeatureImageDTO
    {
        public Community community { get; set; }
        public List<CommunityImage> communityFeaturedImages { get; set; }
        //public string AdminContact { get; set; }
        public List<SubCommunityEmergencyContacts> communityEmergencyContacts { get; set; }
        public bool isAlertBlock { get; set; }
        public JObject  Weather { get; set; }
    }
}