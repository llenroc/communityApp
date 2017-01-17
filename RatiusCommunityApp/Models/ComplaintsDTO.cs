using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ComplaintsDTO
    {
        public Complaint complaint { get; set; }
        public Member member { get; set; }
    }
}