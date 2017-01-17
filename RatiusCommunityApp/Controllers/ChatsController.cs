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
using System.Web;

namespace RatiusCommunityApp.Controllers
{
    //[Authorize]
    public class ChatsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Chats
        public IQueryable<Chat> GetChats()
        {
            return db.Chats;
        }
        public async Task<Response<List<Chat>>> GetUnReadChatUsers(int adminUserId, int CommunityID,int DumbWeb)
        {
            List<Chat> userLastMessage = new List<Chat>();
            List<Chat> userLastUnReadMessage = new List<Chat>();
            List<Chat> chat = new List<Chat>();
            Response<List<Chat>> chatResponse = new Response<List<Chat>>();

            var chatUsers = db.Chats.Where(y => y.from != adminUserId && y.communityID == CommunityID).Select(x => x.from).Distinct().ToList();

            if (chatUsers == null)
            {
                chatResponse.status = "Failed: No User Exist";
                chatResponse.model = null;
                return chatResponse;
            }
            else
            {
                foreach (var item in chatUsers)
                {
                    Chat lastMessage = new Chat();

                    Chat listmesg = db.Chats.Where(x => x.from == item).Include(x => x.userFrom).ToList().LastOrDefault();

                    userLastMessage.Add(listmesg);
                }

                foreach (var item in userLastMessage)
                {
                    if (item.isRead != true)
                    {
                        userLastUnReadMessage.Add(item);
                    }
                }
                chatResponse.status = "Success";
                chatResponse.model = userLastUnReadMessage;
                return chatResponse;
            }

        }

        [ResponseType(typeof(Chat))]
        public async Task<Response<Chat>> GetChatUserbyMessageIDAndReadMessage(int messageID,string status)
        {
            Response<Chat> chatResponse = new Response<Chat>();
            Chat chat = new Chat();
            List<Chat> chatTo = new List<Chat>();
            ChatsController chatController = new ChatsController();
            chat = (from l in db.Chats
                      where l.chatMessageID == messageID
                      select l).FirstOrDefault();
            int userID = chat.to;
            int CommunityID = chat.communityID;
           
            if (chat == null)
            {
                chatResponse.status = "Failed: Message did not Found";
                chatResponse.model = null;
                return chatResponse;
            }
            else
            {
                if (status == "Read")
                {
                    chatTo = (from l in db.Chats
                              where l.to == userID && l.communityID == CommunityID
                              select l).ToList();
                    foreach (var item in chatTo)
                    {
                        if (item.isRead != true)
                        {
                            item.isRead = true;
                            await chatController.PutChat(item.chatMessageID, item);
                        }

                    }
                }
                else
                {
                    chat.isRead = false;
                    await chatController.PutChat(chat.chatMessageID, chat);
                }
                chatResponse.status = "Success";
                chatResponse.model = chat;
                return chatResponse;
            }
           
        }


        // GET: api/Chats/5
        [ResponseType(typeof(Response<List<Chat>>))]
        public async Task<Response<List<ShortChat>>> GetChatForMobileApp(int userId, int communityID)
        {
            List<ShortChat> chatTo = new List<ShortChat>();
            List<ShortChat> chatFrom = new List<ShortChat>();
            //List<Chat> chat = new List<Chat>();
            List<ShortChat> shortchatList = new List<ShortChat>();
            Response<List<ShortChat>> chatResponse = new Response<List<ShortChat>>();
            if (UserExists(userId))
            {
                chatTo = (from l in db.Chats
                          where l.to == userId && l.communityID==communityID
                          select new ShortChat(){
                          chat=l,
                          fromName=l.userFrom.firstName,
                          toName=l.userTo.firstName,
                          IsMe=false,
                          adminID=l.@from
                          }).ToList();
                chatFrom = (from l in db.Chats
                            where l.@from == userId && l.communityID==communityID
                            select new ShortChat()
                            {
                                chat = l,
                                fromName = l.userFrom.firstName,
                                toName = l.userTo.firstName,
                                IsMe=true,
                                adminID = l.to
                            }).ToList();
   
                //adding chatTo in chat    
                shortchatList.AddRange(chatTo);
                //adding chatFrom in chat
                shortchatList.AddRange(chatFrom);
                ChatsController chatController = new ChatsController();
                //set Message isRead=true where userID is not from.(from=Admin UserID)
               
                
                shortchatList = shortchatList.OrderBy(x => x.chat.chatMessageID).ToList();
                chatResponse.status = "Success";
                chatResponse.model = shortchatList;
                return chatResponse;
            }
            else
            {
                chatResponse.status = "Failed: User does not exists";
                chatResponse.model = null;
                return chatResponse;
            }
             
        }



        //[ResponseType(typeof(Response<List<Chat>>))]
        //public async Task<Response<List<Chat>>> GetChatUsers(int adminUserId, int CommunityID)
        //{
        //                List<Chat> userLastMessage = new List<Chat>();
        //    List<Chat> chat = new List<Chat>();
        //    Response<List<Chat>> chatResponse = new Response<List<Chat>>();

        //    var chatUsers = db.Chats.Where(y => y.from != adminUserId && y.communityID == CommunityID && y.to == adminUserId).Select(x => x.from).Distinct().ToList();

        //    if(chatUsers==null)
        //    {
        //        chatResponse.status = "Failed: No User Exist";
        //        chatResponse.model = null;
        //        return chatResponse;
        //    }
        //    else
        //    {
        //        foreach (var item in chatUsers)
        //        {
        //            Chat lastMessage = new Chat();
                    
        //            Chat listmesg = db.Chats.Where(x => x.from == item).Include(x=>x.userFrom).ToList().LastOrDefault();
                    
        //            userLastMessage.Add(listmesg);
        //        }
        //        chatResponse.status = "Success";
        //        userLastMessage = userLastMessage.OrderByDescending(x => x.Date).ToList();
        //        chatResponse.model = userLastMessage;
        //        return chatResponse;
        //    }

        //}








        [ResponseType(typeof(Response<List<Chat>>))]
        public async Task<Response<List<Chat>>> GetChatUsers(int adminUserId, int CommunityID)
        {
            List<Chat> userLastMessage = new List<Chat>();
            List<Chat> chat = new List<Chat>();
            Response<List<Chat>> chatResponse = new Response<List<Chat>>();

            var chatUsers = db.Members.Where(y => y.communityID == CommunityID && y.userId != adminUserId).Include(x => x.user).ToList();

            if (chatUsers == null)
            {
                chatResponse.status = "Failed: No User Exist";
                chatResponse.model = null;
                return chatResponse;
            }
            else
            {
           
                foreach (var item in chatUsers)
                {


                    Chat lastMessage = db.Chats.Where(x => x.from == item.userId && x.to == adminUserId && x.communityID == CommunityID).OrderByDescending(x => x.chatMessageID).FirstOrDefault();
                    if (lastMessage != null)
                    {
                        Chat dbAdminLastMessage = db.Chats.Where(x => x.from == adminUserId && x.to == item.userId && x.communityID == CommunityID).Include(x => x.userTo).OrderByDescending(x => x.chatMessageID).FirstOrDefault();
                        if (dbAdminLastMessage != null)
                        {
                            if (dbAdminLastMessage.chatMessageID > lastMessage.chatMessageID)
                            {
                                lastMessage.desc = dbAdminLastMessage.desc;
                                lastMessage.image = dbAdminLastMessage.image;
                            }
                        }
                      
                        userLastMessage.Add(lastMessage);
                    }
                    if (lastMessage == null)
                    {
                        Chat listmesg = new Chat();
                        Chat dbAdminLastMessage = db.Chats.Where(x => x.from == adminUserId && x.to == item.userId && x.communityID == CommunityID).Include(x => x.userTo).ToList().LastOrDefault();
                        if (dbAdminLastMessage != null)
                        {
                            listmesg = dbAdminLastMessage;
                            listmesg.from = dbAdminLastMessage.to;
                            listmesg.userFrom = dbAdminLastMessage.userTo;
                            listmesg.isRead= true;
                            userLastMessage.Add(listmesg);
                        }
                 
                       
                    }
                  

                }
                chatResponse.status = "Success";
                userLastMessage = userLastMessage.OrderByDescending(x => x.Date).ToList();
                chatResponse.model = userLastMessage;
                return chatResponse;
            }

        }
        [ResponseType(typeof(Response<List<Chat>>))]
        public async Task<Response<List<Chat>>> GetChatUsersbyDate(int adminUserId, int CommunityID,int DummyDate)
        {
            List<Chat> userLastMessage = new List<Chat>();
            List<Chat> chat = new List<Chat>();
            Response<List<Chat>> chatResponse = new Response<List<Chat>>();

            var chatUsers = db.Members.Where(y => y.communityID == CommunityID && y.userId != adminUserId).Include(x => x.user).ToList();

            if (chatUsers == null)
            {
                chatResponse.status = "Failed: No User Exist";
                chatResponse.model = null;
                return chatResponse;
            }
            else
            {
                foreach (var item in chatUsers)
                {
                    Chat lastMessage = db.Chats.Where(x => x.from == item.userId).Include(x => x.userFrom).ToList().LastOrDefault();
                    if (lastMessage != null)
                    {
                        Chat dbAdminLastMessage = db.Chats.Where(x => x.from == adminUserId && x.to == item.userId && x.communityID == CommunityID).Include(x => x.userTo).ToList().LastOrDefault();
                        if (dbAdminLastMessage.chatMessageID > lastMessage.chatMessageID)
                        {
                            lastMessage.desc = dbAdminLastMessage.desc;
                            lastMessage.image = dbAdminLastMessage.image;
                        }
                        userLastMessage.Add(lastMessage);
                    }
                    if (lastMessage == null)
                    {
                        Chat listmesg = new Chat();
                        Chat dbAdminLastMessage = db.Chats.Where(x => x.from == adminUserId && x.to == item.userId && x.communityID == CommunityID).Include(x => x.userTo).ToList().LastOrDefault();
                        if (dbAdminLastMessage != null)
                        {
                            listmesg = dbAdminLastMessage;
                            listmesg.from = dbAdminLastMessage.to;
                            listmesg.userFrom = dbAdminLastMessage.userTo;
                            listmesg.isRead = true;
                            userLastMessage.Add(listmesg);
                        }


                    }
                }
                chatResponse.status = "Success";
                userLastMessage = userLastMessage.OrderByDescending(x => x.Date).ToList();
                chatResponse.model = userLastMessage;
                return chatResponse;
            }

        }




        [ResponseType(typeof(Response<List<Chat>>))]
        public async Task<Response<List<Chat>>> GetChatForAdmin(int userIdTo, int AdminUserID,int communityID)
        {
            List<Chat> chatTo = new List<Chat>();
            List<Chat> chatFrom = new List<Chat>(); 
            List<Chat> chat = new List<Chat>();
            Response<List<Chat>> chatResponse = new Response<List<Chat>>();
            if (UserExists(userIdTo) && UserExists(AdminUserID))
            {
                chatTo = (from l in db.Chats
                          where l.to == userIdTo && l.@from == AdminUserID && l.communityID==communityID
                          select l).Include(x=>x.userFrom).Include(x=>x.userTo).ToList();
                chatFrom = (from l in db.Chats
                            where l.to == AdminUserID && l.@from == userIdTo && l.communityID == communityID
                            select l).Include(x=>x.userFrom).Include(x=>x.userTo).ToList();
               
                //adding chatTo in chat    
                chat.AddRange(chatTo);
                //adding chatFrom in chat
                chat.AddRange(chatFrom);
                ChatsController chatController = new ChatsController();
                //set Message isRead=true where adminUserID is not from.(from=UserID)
                //foreach (var item in chatTo)
                //{
                //    if (item.isRead != true)
                //    {
                //        await chatController.PutChat(item.chatMessageID, item);
                //    }

                //}
                chat = chat.OrderBy(x => x.chatMessageID).ToList();
                chatResponse.status = "Success";
                chatResponse.model = chat;
                return chatResponse;
            }
            else
            {
                chatResponse.status = "Failed: userIdTo or userIdFrom does not exists";
                chatResponse.model = null;
                return chatResponse;
            }

        }
        public async Task<Response<List<Chat>>> GetLatestChatForAdmin(int userIdTo, int AdminUserID, int communityID,int messageID)
        {
            List<Chat> chatTo = new List<Chat>();
            List<Chat> chatFrom = new List<Chat>();
            List<Chat> chat = new List<Chat>();
            Response<List<Chat>> chatResponse = new Response<List<Chat>>();
            if (UserExists(userIdTo) && UserExists(AdminUserID))
            {
                //chatTo = (from l in db.Chats
                //          where l.to == userIdTo && l.@from == AdminUserID && l.communityID == communityID && l.chatMessageID>messageID
                //          select l).Include(x => x.userFrom).ToList();
                chatFrom = (from l in db.Chats
                            where l.to == AdminUserID && l.@from == userIdTo && l.communityID == communityID && l.chatMessageID>messageID
                            select l).Include(x => x.userFrom).ToList();

                //adding chatTo in chat    '


                //chat.AddRange(chatTo);
                //adding chatFrom in chat
                chat.AddRange(chatFrom);
                ChatsController chatController = new ChatsController();
                //set Message isRead=true where adminUserID is not from.(from=UserID)
                //foreach (var item in chatTo)
                //{
                //    if (item.isRead != true)
                //    {
                //        await chatController.PutChat(item.chatMessageID, item);
                //    }

                //}
                chat = chat.OrderBy(x => x.chatMessageID).ToList();
                chatResponse.status = "Success";
                chatResponse.model = chat;
                return chatResponse;
            }
            else
            {
                chatResponse.status = "Failed: userIdTo or userIdFrom does not exists";
                chatResponse.model = null;
                return chatResponse;
            }

        }

        public async Task<IHttpActionResult> GetChatforAndroid(string desc,int to,int from,int communityID)
        {


        
                Chat chat = new Chat();
             

                            chat.desc = desc;
                            chat.to = to;
                            chat.from = from;
                            chat.communityID =communityID;
              
                chat.isRead = false;


                DateTime ServerDateTime = DateTime.Now;
                DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                // ID from: 
                // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                string malayTimeZoneKey = "Singapore Standard Time";
                TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);


                chat.Date = malayDateTime;
                Response<Chat> chatResponse = new Response<Chat>();
                if (!ModelState.IsValid)
                {
                    chatResponse.status = "Failure";
                    chatResponse.model = null;
                    return Ok(chatResponse);
                }
                db.Chats.Add(chat);
                await db.SaveChangesAsync();

                chatResponse.status = "Success";
                chatResponse.model = chat;
                return Ok(chatResponse);

        

        }
        // PUT: api/Chats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChat(int id, Chat chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chat.chatMessageID)
            {
                return BadRequest();
            }

            db.Entry(chat).State = EntityState.Modified;

            try
            {
          
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
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

        // POST: api/Chats
        [ResponseType(typeof(Chat))]
        public async Task<IHttpActionResult> PostChat()
        {


            var httpRequest = HttpContext.Current.Request;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                Chat chat = new Chat();
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        
                        if (key.Equals("desc"))
                            chat.desc= val;
                        if (key.Equals("to"))
                            chat.to = Convert.ToInt32(val);
                        if (key.Equals("from"))
                            chat.from =Convert.ToInt32(val);
                        if (key.Equals("communityID"))
                            chat.communityID = Convert.ToInt32(val);
                    }

                }
                chat.isRead = false;



                DateTime ServerDateTime = DateTime.Now;
                DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                // ID from: 
                // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                string malayTimeZoneKey = "Singapore Standard Time";
                TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);


                chat.Date = malayDateTime;
                Response<Chat> chatResponse = new Response<Chat>();
                if (!ModelState.IsValid)
                {
                    chatResponse.status = "Failure";
                    chatResponse.model = null;
                    return Ok(chatResponse);
                }
                if (httpRequest.Files.Count > 0)
                {
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    chat.image = blobImageURL;
                }
                db.Chats.Add(chat);
                await db.SaveChangesAsync();

                chatResponse.status = "Success";
                chatResponse.model = chat;
                return Ok(chatResponse);

            }
            else
            {
                Response<Community> userResponse = new Response<Community>();
                userResponse.status = "Failed: Not Multipart Content";
                userResponse.model = null;
                return Ok(userResponse);
            }

        }


        [ResponseType(typeof(Chat))]
        public async Task<Response<Chat>> PostChatForAdmin(string description, int userIdTo, int userIdFrom, int communityID, string image)
        {

            
           
                Chat chat = new Chat();
                chat.desc = description;
                chat.to = userIdTo;
                chat.from = userIdFrom;
                chat.isRead = false;




                DateTime ServerDateTime = DateTime.Now;
                DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                // ID from: 
                // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                string malayTimeZoneKey = "Singapore Standard Time";
                TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);



                chat.Date = malayDateTime;
                chat.communityID = communityID;
                chat.image = image;
                Response<Chat> chatResponse = new Response<Chat>();
                chatResponse.model = chat;
                if (!ModelState.IsValid)
                {
                    chatResponse.status = "Failure";
                    chatResponse.model = null;
                    return chatResponse;
                }
        
               
                db.Chats.Add(chat);
                await db.SaveChangesAsync();

                chatResponse.status = "Success";
                chatResponse.model = chat;


                Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
                        Response<Notification> responceNotification = new Response<Notification>();
            NotificationsController notificationController = new NotificationsController();
            responceNotification = await notificationController.GetNotificationbyUserID(userIdTo);


            var ChatFromDB = await (from l in db.Chats
                                      where l.communityID == communityID
                                      select l).Include(x => x.community).FirstOrDefaultAsync();
            if (responceNotification.model.user.Islogout == false){

            if (responceNotification.model.Messages == true)
            {

                // iOS
                var alert = "{\"aps\":{\"alert\":\"" + ChatFromDB.community.name + " : You have a new message.\",\"id\":\"1\",\"communityid\":\"" + chat.communityID + "\",\"Message\":\"" + chat.desc + "\",\"image\":\"" + chat.image+ "\",\"sound\":\"default\"}}";
                outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(userIdTo));


                // Android
                //"{ \"data\" : {\"message\":\"You have a new message.\",\"id\":\"1\"}}"

                var notif = "{\"data\":{\"message\":\"" + ChatFromDB.community.name + " : You have a new message.\",\"badge\":\"1\",\"id\":\"1\",\"Message\":\"" + chat.desc + "\",\"image\":\"" + chat.image + "\",\"communityid\":\"" + chat.communityID + "\"}}";
                outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(userIdTo));
            }
            }
            return chatResponse;

         







                //var alert = "{\"aps\":{\"alert\":\"You have a new message.\",\"badge\":" + msgCount + ",\"id\":\"2\",\"sound\":\"default\"}}";
                //var outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, messages.messageTo);

                //var notif = "{ \"data\" : {\"message\":\"You have a new message.\",\"badge\":" + msgCount + ",\"id\":\"2\"}}";
                //outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, messages.messageTo);
          
            
         

        }
        // DELETE: api/Chats/5
        [ResponseType(typeof(Chat))]
        public async Task<IHttpActionResult> DeleteChat(int id)
        {
            Chat chat = await db.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            db.Chats.Remove(chat);
            await db.SaveChangesAsync();

            return Ok(chat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatExists(int id)
        {
            return db.Chats.Count(e => e.chatMessageID == id) > 0;
        }
        [Authorize]
        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.userID == id) > 0;
        }
    }
}