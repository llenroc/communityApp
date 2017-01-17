using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SubCommunity
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
    }
}