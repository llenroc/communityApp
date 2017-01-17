using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SubCommunityEmergencyContacts
    {
        public int id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string workingHourStart { get; set; }
        public string workingHourEnd { get; set; }
        public int subCommunityId { get; set; }
        [ForeignKey("subCommunityId")]
        public SubCommunity subCommunity { get; set; }
    }
}