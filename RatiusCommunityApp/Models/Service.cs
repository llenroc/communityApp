using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class Service
    {
        [Key]
        public int serviceID { get; set; }
        public string description { get; set; }
        public string Image { get; set; }
    }
}