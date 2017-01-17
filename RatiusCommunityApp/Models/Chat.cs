using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Chat
    {
        [Key]
        public int chatMessageID { get; set; }

      
        public string desc { get; set; }
   
        public bool isRead { get; set; }
        public string image { get; set; }
     
        public DateTime Date { get; set; }
        public int to { get; set; }

        public int communityID { get; set; }
        public Community community { get; set; }
        public int from { get; set; }

        [ForeignKey("to")]
        public User userTo { get; set; }
        [ForeignKey("from")]
        public User userFrom { get; set; }

        

    }
}