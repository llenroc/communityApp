using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class AnnouncementDTO
    {
        public Announcement announcement { get; set; }
        public List<Announcement> announcementList { get; set; }
        public IPagedList<Announcement> PagedannouncementList { get; set; }
    }
}