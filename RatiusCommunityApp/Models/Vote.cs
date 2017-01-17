using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Vote
    {
        public int id { get; set; }
        public int complaintID { get; set; }
        public Complaint complaint { get; set; }
        public int userID { get; set; }
        public User user { get; set; }
    }
}