using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class UserMemberDTO
    {
        public User user { get; set; }
        public Member member { get; set; }
        public int AdminID { get; set; }
   
    }
}