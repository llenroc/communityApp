using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ShortChat
    {
        public string toName { get; set; }
        public string fromName { get; set;}
        public Chat chat { get; set; }
        public bool IsMe { get; set; }
        public int adminID { get; set; }
    }
}