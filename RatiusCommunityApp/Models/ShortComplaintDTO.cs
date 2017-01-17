using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ShortComplaintDTO
    {
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public string messages { get; set; }
        public int complaintStatusId { get; set; }
    }
}