using RatiusCommunityApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Controllers
{
    public class MobSettingDTO
    {
        public int communityID { get; set; }
        public int userID { get; set; }
        public string EmailID { get; set; }
        public string password { get; set; }
        public string emergencyContactName1 { get; set; }
        public string emergencyContactName2 { get; set; }
        public string emergencyContactNumber1 { get; set; }
        public string emergencyContactNumber2 { get; set; }
        public List<CommunityDTO> communityList { get; set; }
        public string language { get; set; }
        public bool Report { get; set; }
        public bool Notices { get; set; }
        public bool Messages { get; set; }

    }
}