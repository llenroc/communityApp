using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SubCommunityEmergencyContactsDTO
    {
        public int subCommunityID { get; set; }
        public List<SubCommunityEmergencyContacts> subCommunityEmergencyContactsList{ get; set; }
        public List<CommunityStreetFloor> allStreetFloors { get; set; }
        public List<int> selectedStreetFloorsIDs { get; set; }
        public List<int> subCommunitySelectedStreetFloorsIDs { get; set; }
    }
}