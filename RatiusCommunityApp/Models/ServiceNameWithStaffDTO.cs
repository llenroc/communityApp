using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ServiceNameWithStaffDTO
    {
        public CommunityService service { get; set; }
        public List<ServiceStaff> serviceStaff { get; set; }
    }
}