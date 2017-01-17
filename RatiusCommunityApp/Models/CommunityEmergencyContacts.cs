using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityEmergencyContacts
    {
        public int id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string workingHourStart { get; set; }
        public string workingHourEnd { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
    }
}