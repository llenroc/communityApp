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
    public class NotificationsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Notifications
        public IQueryable<Notification> GetNotifications()
        {
            return db.Notifications;
        }

        // GET: api/Notifications/5
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> GetNotification(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }





        public async Task<Response<Notification>> GetNotificationbyUserID(int userID)
        {
            Response<Notification> responceNotification = new Response<Notification>();
            Notification notification = new Notification();
            notification =await (from l in db.Notifications
                            where l.userID == userID
                            select l).Include(x=>x.user).FirstOrDefaultAsync();
            if (notification == null)
            {
                responceNotification.status = "Failuer:No user Found";
                responceNotification.model = null;
                return responceNotification;
            }
            else
            {
                responceNotification.status = "Success";
                responceNotification.model = notification;
                return responceNotification;
            }
        }
        // PUT: api/Notifications/5
        [ResponseType(typeof(void))]
        public async Task<Response<Notification>> PutNotification(int userID, Notification notification)
        {
            NotificationsController notificationController=new NotificationsController();
            Response<Notification> responceNotification = new Response<Notification>();
            if (!ModelState.IsValid)
            {
                responceNotification.status = "Failed";
                responceNotification.model = null;
                return responceNotification;
            }

            if (userID != notification.userID)
            {

                responceNotification.status = "Failed: Id did not match";
                responceNotification.model = null;
                return responceNotification;
            }

            db.Entry(notification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!useridNotificationExists(userID))
                {
                    responceNotification.status = "Failed: Id did not Exist";
                    responceNotification.model = null;
                    return responceNotification;
                }
                else
                {
                    throw;
                }
            }

        responceNotification.status = "Success";
                responceNotification.model = notification;
                return responceNotification;
        }


        public async Task<Response<Notification>> PutNotification(int userID, bool Messages,bool Notices,bool Report)
        {
            NotificationsController notificationController = new NotificationsController();
            Response<Notification> responceNotification = new Response<Notification>();
            Notification notification = new Notification();
            responceNotification = await notificationController.GetNotificationbyUserID(userID);
            notification.Messages = Messages;
            notification.Notices = Notices;
            notification.Report = Report;
            if (!ModelState.IsValid)
            {
                responceNotification.status = "Failed";
                responceNotification.model = null;
                return responceNotification;
            }

            if (userID != notification.userID)
            {

                responceNotification.status = "Failed: Id did not match";
                responceNotification.model = null;
                return responceNotification;
            }

            db.Entry(notification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!useridNotificationExists(userID))
                {
                    responceNotification.status = "Failed: Id did not Exist";
                    responceNotification.model = null;
                    return responceNotification;
                }
                else
                {
                    throw;
                }
            }

            responceNotification.status = "Failed: Id did not match";
            responceNotification.model = notification;
            return responceNotification;
        }

        // POST: api/Notifications
        [ResponseType(typeof(Notification))]
        public async Task<Response<Notification>> PostNotification(Notification notification)
        {
            Response<Notification> responceNotification = new Response<Notification>();
            if (!ModelState.IsValid)
            {
                responceNotification.status = "Failure";
                responceNotification.model = null;
                return responceNotification;
            }

            db.Notifications.Add(notification);
            await db.SaveChangesAsync();
            responceNotification.status = "Failure";
            responceNotification.model = notification;
            return responceNotification;
        }

        // DELETE: api/Notifications/5
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> DeleteNotification(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notification);
            await db.SaveChangesAsync();

            return Ok(notification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int id)
        {
            return db.Notifications.Count(e => e.id == id) > 0;
        }
        private bool useridNotificationExists(int userID)
        {
            return db.Notifications.Count(e => e.userID == userID) > 0;
        }
    }
}