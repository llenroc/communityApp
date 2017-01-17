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
    [Authorize]
    public class AnnouncementsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Announcements
        public IQueryable<Announcement> GetAnnouncements()
        {
            return db.Announcements;
        }

        // GET: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<Response<Announcement>> GetAnnouncement(int id)
        {
            Response<Announcement> responseAnnouncement = new Response<Announcement>();
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                responseAnnouncement.status = "Failed: No Announcement Found";
                responseAnnouncement.model = null;
                return responseAnnouncement;
            }

            responseAnnouncement.status = "Success";
            responseAnnouncement.model = announcement;
            return responseAnnouncement;
        }

        public async Task<Response<List<Announcement>>> GetAllAnnouncementsbyCommunityID(int communityID)
        {
            Response<List<Announcement>> responceAnnouncement = new Response<List<Announcement>>();
            List<Announcement> announcements = new List<Announcement>();
            announcements = (from l in db.Announcements
                       where l.communityID == communityID
                       select l).ToList();
         
            announcements = announcements.OrderByDescending(x => x.id).ToList();
            if (announcements == null)
            {
                responceAnnouncement.status = "Failed: No Announcements";
                responceAnnouncement.model = null;
                return responceAnnouncement;
            }

            responceAnnouncement.status = "Success";
            responceAnnouncement.model = announcements;
            return responceAnnouncement;
        }

        // PUT: api/Announcements/5
        [ResponseType(typeof(void))]
        public async Task<Response<Announcement>> PutAnnouncement(int id, Announcement announcement)
        {
            Response<Announcement> responseAnnouncement = new Response<Announcement>();
            if (!ModelState.IsValid)
            {
                responseAnnouncement.status = "Failure";
                responseAnnouncement.model = null;
                return responseAnnouncement;
            }

            if (id != announcement.id)
            {
                responseAnnouncement.status = "Failed: ID did not match";
                responseAnnouncement.model = null;
                return responseAnnouncement;
            }
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile.ContentLength == 0)
                    {
                        Response<Announcement> responseGetAnnouncement = new Response<Announcement>();
                        AnnouncementsController announcementController = new AnnouncementsController();
                        responseGetAnnouncement =await announcementController.GetAnnouncement(announcement.id);
                        announcement.image = responseGetAnnouncement.model.image;

                    }
                    else
                    {
                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertfileForBlob();
                        announcement.image = blobImageURL;
                    }
                }
            }
       

            db.Entry(announcement).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
                {
                    responseAnnouncement.status = "Failed: ID does not exist";
                    responseAnnouncement.model = null;
                    return responseAnnouncement;
                }
                else
                {
                    throw;
                }
            }

            responseAnnouncement.status = "Success";
            responseAnnouncement.model = announcement;





            List<Member> member = new List<Member>();
            member = db.Members.Where(m => m.communityID == announcement.communityID).Include(x=>x.user).ToList();
            Community community = new Community();
            community = await db.Communities.Where(m => m.communityID == announcement.communityID).FirstOrDefaultAsync();
            string communityName = community.name;
            foreach (var item in member)
            {

                Response<Notification> responceNotification = new Response<Notification>();
                NotificationsController notificationController = new NotificationsController();
                responceNotification = await notificationController.GetNotificationbyUserID(item.userId);
                if (responceNotification.model != null)
                {
                    if (responceNotification.model.user.Islogout == false)
                    {


                        if (responceNotification.model.Notices == true)
                        {

                            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
                            // iOS
                            var alert = "{\"aps\":{\"alert\":\"" + communityName + " : The Notice has been updated.\",\"id\":\"5\",\"communityid\":\"" + item.communityID + "\",\"sound\":\"default\"}}";
                            outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(item.userId));


                            // Android

                            //var notif = "{ \"data\" : {\"message\":\"You have a new Notice.\",\"id\":\"2\"}}";
                            var notif = "{\"data\":{\"message\":\"" + communityName + " : The Notice has been updated.\",\"badge\":\"1\",\"id\":\"5\",\"communityid\":\"" + item.communityID + "\"}}";
                            outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(item.userId));
                        }
                    }
                }
                //else
                //{
                //    responseAnnouncement.status =Convert.ToString(item.userId);
                //    responseAnnouncement.model = null;
                //    return responseAnnouncement;
                //}
            }
            return responseAnnouncement;
        }

        // POST: api/Announcements
        [ResponseType(typeof(Announcement))]
        public async Task<Response<Announcement>> PostAnnouncement(Announcement announcement)
        {
            try
            {


                Response<Announcement> responseAnnouncement = new Response<Announcement>();
                if (!ModelState.IsValid)
                {
                    responseAnnouncement.status = "Failure";
                    responseAnnouncement.model = null;
                    return responseAnnouncement;
                }
                if (announcement.image != null)
                {
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertfileForBlob();
                    announcement.image = blobImageURL;
                }



                DateTime ServerDateTime = DateTime.Now;
                DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                // ID from: 
                // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                string malayTimeZoneKey = "Singapore Standard Time";
                TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);



                announcement.date = malayDateTime;
                db.Announcements.Add(announcement);
                await db.SaveChangesAsync();
                responseAnnouncement.status = "Success";
                responseAnnouncement.model = announcement;

                List<Member> member = new List<Member>();
                member = db.Members.Where(m => m.communityID == announcement.communityID).Include(x=>x.user).ToList();
                Community community = new Community();
                community =await db.Communities.Where(m => m.communityID == announcement.communityID).FirstOrDefaultAsync();
                string communityName = community.name;
                foreach (var item in member)
                {
                    
                        Response<Notification> responceNotification = new Response<Notification>();
                        NotificationsController notificationController = new NotificationsController();
                        responceNotification = await notificationController.GetNotificationbyUserID(item.userId);
                        if (responceNotification.model != null)
                        {
                            if (responceNotification.model.user.Islogout == false)
                            { 
                            if (responceNotification.model.Notices == true)
                            {

                                Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
                                // iOS
                                var alert = "{\"aps\":{\"alert\":\"" + communityName + " : You have a new Notice.\",\"id\":\"2\",\"communityid\":\"" + item.communityID + "\",\"sound\":\"default\"}}";
                                outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(item.userId));


                                // Android

                                //var notif = "{ \"data\" : {\"message\":\"You have a new Notice.\",\"id\":\"2\"}}";
                                var notif = "{\"data\":{\"message\":\"" + communityName + " : You have a new Notice.\",\"badge\":\"1\",\"id\":\"2\",\"communityid\":\"" + item.communityID + "\"}}";
                                outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(item.userId));
                            }
                }
                        }
                        //else
                        //{
                        //    responseAnnouncement.status =Convert.ToString(item.userId);
                        //    responseAnnouncement.model = null;
                        //    return responseAnnouncement;
                        //}
                }
                return responseAnnouncement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<Response<Announcement>> DeleteAnnouncement(int id)
        {
            Response<Announcement> responseAnnouncement = new Response<Announcement>();
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                responseAnnouncement.status = "Failed: Announcement not Fount";
                responseAnnouncement.model = null;
                return responseAnnouncement;
            }

            db.Announcements.Remove(announcement);
            await db.SaveChangesAsync();

            responseAnnouncement.status = "Success";
            responseAnnouncement.model = null;
            return responseAnnouncement;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnouncementExists(int id)
        {
            return db.Announcements.Count(e => e.id == id) > 0;
        }
    }
}