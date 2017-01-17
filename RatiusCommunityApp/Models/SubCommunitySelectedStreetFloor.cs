using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class SubCommunitySelectedStreetFloor
    {
        [Key]
        public int id { get; set; }
        public int communityStreetFloorId { get; set; }
      
        public int subCommunityId { get; set; }


        [ForeignKey("communityStreetFloorId")]
        public CommunityStreetFloor communityStreetFloor { get; set; }
        [ForeignKey("subCommunityId")]
        public SubCommunity subCommunity { get; set; }

    }
}