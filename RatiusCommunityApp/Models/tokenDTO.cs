using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class tokenDTO<communityAppModel>
    {
        public string token { get; set; }
        public string status { get; set; }
        public communityAppModel model { get; set; }
    }
}