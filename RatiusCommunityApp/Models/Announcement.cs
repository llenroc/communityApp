using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Announcement
    {
        [Key]
        public int id { get; set; }
        
        public string shortDescription { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        public string image { get; set; }
    }
}