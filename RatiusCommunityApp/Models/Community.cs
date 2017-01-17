using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Community
    {
        [Key]
        public int communityID { get; set; }
        public string name { get; set; }
        public string communityAddress { get; set; }
        public int adminUserID { get; set; }

        public string coverImage { get; set; }

        public string about { get; set; }

        [ForeignKey("adminUserID")]
        public User user { get ; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string communityPassword { get; set; }
        public bool isChangePassword { get; set; }
        public bool isFirstTimeUser { get; set; }
        public int emergencyContactRange { get; set; }
        //public string secretCode { get; set; }
    }
}