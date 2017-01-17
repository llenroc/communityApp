using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SecretCodeDTO
    {
        public Community community { get; set; }
        public List<CommunityStreetFloor> communityStreetFloor { get; set; }
    }
}