using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class User
    {
    
        [Key]
        public int userID { get; set; }
        public string emailID { get; set; }
        public string password { get; set; }
        public string image { get; set; }
  
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string country { get; set; }
        public string gender { get; set; }
        public string contact { get; set; }
        public string emergencyContactName1 { get; set; }
        public string emergencyContactNumber1 { get; set; }
        public string emergencyContactName2 { get; set; }
        public string emergencyContactNumber2 { get; set; }
        public bool isMainAdmin { get; set; }

        public double lat { get; set; }
        public double lng { get; set; }
        public string language { get; set; }
        public string userType { get; set; }
        public bool Islogout { get; set; }
      
    }
}