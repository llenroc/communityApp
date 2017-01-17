using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ServiceDTO
    {
        public Service service { get; set; }
        public List<CommunityService> selectedServices { get; set; }
        public List<Service> remainingServices { get; set; }
        public List<DirectoryImages> directoryImage { get; set; }
        public List<CommunityServiceWithServiceIdDTO> CommunityServiceWithServiceIdDTOList { get; set; }
        public List<Community> communitieslist { get; set; }
        public int emergencyContactRange { get; set; }
    }
}