using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunitySecretCodes
    {
        public int id { get; set; }
        public string secretCode { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
    }
}