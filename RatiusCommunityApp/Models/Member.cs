using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Member
    {
        [Key]
        public int id { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public string address { get; set; }
        public string streetFloor { get; set; }
        public bool isBlocked { get; set; }
        public bool isAlertBlocked { get; set; }
    }
}