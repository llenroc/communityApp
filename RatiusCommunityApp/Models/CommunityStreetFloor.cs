﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class CommunityStreetFloor
    {
        [Key]
        public int id { get; set; }
        public string streetFloor { get; set; }
        public int communityID { get; set; }
        public Community community { get; set; }

    }
}