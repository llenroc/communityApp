using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class MemberAndChatDTO
    {
        public  List<ChatDTO> listChatDTO { get; set; }
        public List<Member> communityMembers { get; set; }
        public IPagedList<ChatDTO> PagedlistChatDTO { get; set; }
    }
}