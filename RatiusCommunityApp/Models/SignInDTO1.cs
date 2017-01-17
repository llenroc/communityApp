using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SignInDTO1
    {
        public User user { get; set; }
        public List<CommunityDTO1> community { get; set; }
    }
}