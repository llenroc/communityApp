using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Alert
    {
        [Key]
        public int id { get; set; }
        public int userID { get; set; }
        public User user { get; set; }
        public DateTime date { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }
        public int memberID { get; set; }
        [ForeignKey("memberID")]
        public Member member{ get; set; }
        public bool isViewed { get; set; }
    }
}