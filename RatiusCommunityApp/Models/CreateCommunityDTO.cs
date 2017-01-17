using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CreateCommunityDTO
    {
        public Community community { get; set; }
        public string staticPassword { get; set; }
    }
}