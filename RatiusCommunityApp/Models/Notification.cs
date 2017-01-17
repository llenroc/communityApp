using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Notification
    {
        public int id { get; set; }
        public int userID { get; set; }
        public User user { get; set; }
        public bool Report { get; set; }
        public bool Notices { get; set; }
        public bool Messages { get; set; }
    }
}