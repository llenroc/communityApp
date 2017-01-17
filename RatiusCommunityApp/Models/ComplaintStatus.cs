using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ComplaintStatus
    {
        [Key]
        public int complaintStatusID { get; set; }
        public string status { get; set; }
    }
}