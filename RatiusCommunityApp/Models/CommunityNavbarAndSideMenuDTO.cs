using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityNavbarAndSideMenuDTO
    {
        public tokenDTO<User> tokenUser { get; set; }
        public Member member { get; set; }
        public Community community { get; set; }
    }
}