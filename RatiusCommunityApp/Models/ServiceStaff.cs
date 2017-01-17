using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ServiceStaff
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
     
        public string contactNumber { get; set; }
        public string image { get; set; }
        public string emailID { get; set; }
        public string staffRole { get; set; }
        public int communityServiceID { get; set; }
        public CommunityService CommunityService { get; set; }
       
        public int communityID { get; set; }
        public Community community { get; set; }
        public bool isActive { get; set; }
    }
}