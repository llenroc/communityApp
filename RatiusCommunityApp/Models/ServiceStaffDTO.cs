using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ServiceStaffDTO
    {
        public List<ServiceStaff> serviceStaffList { get; set; }
        public ServiceStaff serviceStaff { get; set; }
        public string serviceName { get; set; }
    }
}