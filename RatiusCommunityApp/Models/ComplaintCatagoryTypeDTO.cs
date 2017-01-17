using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class ComplaintCatagoryTypeDTO
    {
        public string iReportName { get; set; }
        public List<ComplaintsDTO> complaints { get; set; }
        public List<CommunityiReports> CommunityiReports { get; set; }
        public double totalSelectedCatagoryComplaintsPercentage { get; set; }
        public double totalUpdatedComplaintsPercentage { get; set; }
        public double totalReceivedComplaintsPercentage { get; set; }
        public double totalClosedComplaintsPercentage { get; set; }
        //public List<ComplaintsChartDTO> complaintsChartDTOList { get; set; }
        public string communityName { get; set; }
        public string communityAdminEmail { get; set; }
        public string communityAddress { get; set; }
        public IPagedList<ComplaintsDTO> Pagedcomplaints { get; set; }
        public bool isImageInclude { get; set; }
        public bool isLocationInclude { get; set; }
    }
}