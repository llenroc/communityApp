using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RatiusCommunityApp.Models;

namespace RatiusCommunityApp.Controllers
{
    [Authorize]
    public class MembersController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Members
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // GET: api/Members/5
        [ResponseType(typeof(Member))]
        public async Task<Response<Member>> GetMember(int id)
        {
            Response<Member> responceMember = new Response<Member>();
            Member member = await db.Members.Where(x=>x.id==id).Include(x=>x.community).FirstOrDefaultAsync();
            if (member == null)
            {
                responceMember.status = "Failed : Did not found Member.";
                responceMember.model = null;
                return responceMember;
            }

            responceMember.status = "Success";
            responceMember.model = member;
            return responceMember;
        }
        // GET: api/Members/5
        public async Task<Response<List<Member>>> GetAllCommunityMembersbyCommunityID(int communityID)
        {
            Response<List<Member>> responceMember = new Response<List<Member>>();
            List<Member> members = new List<Member>();
            members = (from l in db.Members
                                 where  l.communityID==communityID
                                 select l).Include(x=>x.user).Include(y=>y.community).ToList();
            if (members == null)
            {
                responceMember.status = "Failed: No Community Member";
                responceMember.model = null;
                return responceMember;
            }

            responceMember.status = "Success";
            responceMember.model = members;
            return responceMember;
        }
        [Route("api/getcommunityMemberwithoutAdmin")]
        public async Task<Response<List<Member>>> getcommunityMemberwithoutAdmin(int communityID, int adminUserID)
        {
            Response<List<Member>> responceMember = new Response<List<Member>>();
            List<Member> members = new List<Member>();
            members = (from l in db.Members
                       where l.communityID == communityID && l.userId!=adminUserID
                       select l).Include(x => x.user).ToList();
            if (members == null)
            {
                responceMember.status = "Failed: No Community Member";
                responceMember.model = null;
                return responceMember;
            }

            responceMember.status = "Success";
            responceMember.model = members;
            return responceMember;
        }
        public async Task<Response<Member>> GetCommunityMember(int communityID,int userID)
        {
            Response<Member> responceMember = new Response<Member>();
            Member member = new Member();
            member = await (from l in db.Members
                            where l.communityID == communityID && l.userId==userID
                            select l).FirstOrDefaultAsync();
            if (member == null)
            {
                responceMember.status = "Failed:You Are Not the Member of this Community";
                responceMember.model = null;
                return responceMember;
            }
            if (member.isBlocked == true)
            {
                responceMember.status = "Failed:You Are Blocked for this Community";
                responceMember.model = member;
                return responceMember;
            }
            responceMember.status = "Success";
            responceMember.model = member;
            return responceMember;
        }
        public async Task<Response<List<Member>>> GetCommunityMemberbyUserID(int userID)
        {
            Response<List<Member>> responceMember = new Response<List<Member>>();
            List<Member> members = new List<Member>();
            members = (from l in db.Members
                            where  l.userId == userID
                            select l).ToList();
            if (members == null)
            {
                responceMember.status = "Failed:You Are Not the Member of this Community";
                responceMember.model = null;
                return responceMember;
            }
        
            responceMember.status = "Success";
            responceMember.model = members;
            return responceMember;
        }
        // PUT: api/Members/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMember(int id, Member member)
        {
           int communityID = member.communityID;
            int userID = member.userId;
            var memberFromDB = await (from l in db.Members
                            where l.communityID == communityID && l.userId == userID
                            select l).Include(x=>x.user).Include(x=>x.community).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.id)
            {
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
                if (memberFromDB.user.Islogout == false)
                {
                    if (member.isBlocked == true)
                    {
                        // iOS
                        var alert = "{\"aps\":{\"alert\":\"" + memberFromDB.community.name + " : You have been BLOCKED.\",\"id\":\"4\",\"communityid\":\"" + member.communityID + "\",\"sound\":\"default\"}}";
                        outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(member.userId));


                        // Android
                        //var notif = "{ \"data\" : {\"message\":\"You are blocked\",\"id\":\"4\"}}";
                        //var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You are blocked.\",\"badge\":\"1\",\"id\":\"4\",\"communityid\":\"" + member.communityID + "\"}}";

                        var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You have been BLOCKED.\",\"badge\":\"1\",\"id\":\"4\",\"communityName\":\"" + memberFromDB.community.name + "\",\"communityid\":\"" + member.communityID + "\"}}";

                        outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(member.userId));
                    }
                    else
                    {
                        // iOS
                        var alert = "{\"aps\":{\"alert\":\"" + memberFromDB.community.name + " : You are now an ACTIVE member.\",\"id\":\"6\",\"communityid\":\"" + member.communityID + "\",\"sound\":\"default\"}}";
                        outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(member.userId));


                        // Android
                        //var notif = "{ \"data\" : {\"message\":\"You are blocked\",\"id\":\"4\"}}";
                        //var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You are blocked.\",\"badge\":\"1\",\"id\":\"4\",\"communityid\":\"" + member.communityID + "\"}}";

                        var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You are now an ACTIVE member.\",\"badge\":\"1\",\"id\":\"6\",\"communityName\":\"" + memberFromDB.community.name + "\",\"communityid\":\"" + member.communityID + "\"}}";

                        outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(member.userId));
                    }

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Members
        [ResponseType(typeof(Response<Member>))]
        public async Task<Response<Member>> PostMember(Member member)
        {
            try { 
            Response<Member> responseMember = new Response<Member>();
            if (!ModelState.IsValid)
            {
                responseMember.status = "Failure";
                responseMember.model = null;
                return responseMember;
            }

            if (MemberExists(member.userId,member.communityID))
            {
                responseMember.status = "Failed: You Are Already Member of this Community";
                Member existMember = new Member();
                existMember =await (from l in db.Members
                              where l.userId ==member.userId && l.communityID==member.communityID 
                              select l).FirstOrDefaultAsync();
                responseMember.model = existMember;
                return responseMember;
            }
            member.isBlocked = false;
            db.Members.Add(member);
            await db.SaveChangesAsync();
            responseMember.status = "Success";
            responseMember.model = member;
            return responseMember;
            }
            catch (Exception ex)
            {
                Response<Member> responseMember = new Response<Member>();
                responseMember.status = ex.Message;
                responseMember.model = null;
                return responseMember;
            }
        }

        // DELETE: api/Members/5
        [ResponseType(typeof(Member))]
        public async Task<IHttpActionResult> DeleteMember(int id)
        {
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            await db.SaveChangesAsync();

            return Ok(member);
        }
        public async Task<Response<Member>> DeleteMember(int communityID, int userID)
        {
           


            //Remove Alerts
            List<Alert> Alerts = new List<Alert>();
            Alerts = db.Alerts.Where(x => x.communityID == communityID && x.userID == userID).ToList();
            if (Alerts.Count != 0)
            {
                foreach (var item in Alerts)
                {
                    Alert Alert = await db.Alerts.FindAsync(item.id);
                    db.Alerts.Remove(Alert);

                }
                await db.SaveChangesAsync();
            }


            List<Chat> ChatsTo = new List<Chat>();
            List<Chat> ChatsFrom = new List<Chat>();
            ChatsTo = db.Chats.Where(x => x.communityID == communityID &&  x.to==userID).ToList();
            ChatsFrom = db.Chats.Where(x => x.communityID == communityID &&  x.from == userID).ToList();
           
            if (ChatsTo.Count != 0)
            {
                foreach (var item in ChatsTo)
                {
                    Chat chat = await db.Chats.FindAsync(item.chatMessageID);
                    db.Chats.Remove(chat);

                }
                await db.SaveChangesAsync();
            }
            if (ChatsFrom.Count != 0)
            {
                foreach (var item in ChatsFrom)
                {
                    Chat chat = await db.Chats.FindAsync(item.chatMessageID);
                    db.Chats.Remove(chat);

                }
                await db.SaveChangesAsync();
            }


            List<Complaint> Complaints = new List<Complaint>();
            Complaints = db.Complaints.Where(x => x.communityID == communityID && x.userID==userID).ToList();
            if (Complaints.Count != 0)
            {
                foreach (var item in Complaints)
                {
                    Complaint Complaint = await db.Complaints.FindAsync(item.complaintID);
                    db.Complaints.Remove(Complaint);

                }
                await db.SaveChangesAsync();
            }


      


            Response<Member> responceMember = new Response<Member>();
            Member member = new Member();
            member = await (from l in db.Members
                            where l.communityID == communityID && l.userId == userID
                            select l).FirstOrDefaultAsync();
            if (member == null)
            {
                responceMember.status = "Failed: Incorrect CommunityID or UserID";
                responceMember.model = null;
                return responceMember;
            }
            
            db.Members.Remove(member);
            await db.SaveChangesAsync();
            responceMember.status = "Success";
            responceMember.model = member;
            return responceMember;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.id == id) > 0;
        }
        private bool MemberExists(int userID,int communityID)
        {
            return db.Members.Count(e => e.userId == userID && e.communityID==communityID) > 0;
        }
    }
}