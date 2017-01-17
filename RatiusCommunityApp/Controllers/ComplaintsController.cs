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
    public class ComplaintsController : ApiController
    {
        
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Complaints
        public IQueryable<Complaint> GetComplaints()
        {
           
            return db.Complaints;
        }

        // GET: api/Complaints/5
        [ResponseType(typeof(Complaint))]
        public async Task<Response<List<Complaint>>> GetComplaintByCommunityID(int communityID)
        {
            Response<List<Complaint>> responseComplaint = new Response<List<Complaint>>();
            List<Complaint> complaints = new List<Complaint>();
            complaints = db.Complaints.Where(x => x.communityID == communityID).Include(x => x.user).Include(y => y.CommunityiReports).Include(x => x.community).Include(x => x.community.user).ToList();
            complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            if (complaints == null)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }
            responseComplaint.status = "Success";
            responseComplaint.model = complaints;
            return responseComplaint;
        }

        public int GetCountComplaitsByStatus(int communityID, int complaintsiReportID, int complaintStatusID)
        {
        
            Response<List<Complaint>> responseComplaint = new Response<List<Complaint>>();
            List<Complaint> complaints = new List<Complaint>();
            if (complaintsiReportID == -1)
            {
                complaints = db.Complaints.Where(x => x.communityID == communityID &&  x.complaintStatusID == complaintStatusID).ToList(); complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            }
            else
            {
                complaints = db.Complaints.Where(x => x.communityID == communityID && x.CommunityiReportsID == complaintsiReportID && x.complaintStatusID == complaintStatusID).ToList();
                complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            }

            if (complaints == null)
            {
             
                return 0;
            }

            return complaints.Count;
        }

        public async Task<Response<List<Complaint>>> GetUnreadComplaintByCommunityID(int communityID,int DumbWeb)
        {
            Response<List<Complaint>> responseComplaint = new Response<List<Complaint>>();
            List<Complaint> complaints = new List<Complaint>();
            complaints = db.Complaints.Where(x => x.communityID == communityID && x.complaintStatusID==1).ToList();
            complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            if (complaints == null)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }
            responseComplaint.status = "Success";
            responseComplaint.model = complaints;
            return responseComplaint;
        }
        public async Task<Response<List<ShortComplaintDTO>>> GetComplaintByCommunityIDUserID(int communityID, int userID)
        {
            Response<List<ShortComplaintDTO>> responseComplaint = new Response<List<ShortComplaintDTO>>();
            List<ShortComplaintDTO> shortComplaintDTOList = new List<ShortComplaintDTO>();
         
            List<Complaint> complaints = new List<Complaint>();
            complaints = db.Complaints.Where(x => x.communityID == communityID && x.userID==userID).Include(x => x.user).Include(y => y.CommunityiReports).ToList();
            complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            foreach (var item in complaints)
            {
                ShortComplaintDTO shortComplaintDTO = new ShortComplaintDTO();
                shortComplaintDTO.image1 = item.image1;
                shortComplaintDTO.image2 = item.image2;
                shortComplaintDTO.image3 = item.image3;
                shortComplaintDTO.messages = item.desc;
                if (item.complaintStatusID == 2)
                {
                    shortComplaintDTO.IsRead = true;
                }
                else
                {
                    shortComplaintDTO.IsRead = false;
                }
                shortComplaintDTO.complaintStatusId = item.complaintStatusID;
                shortComplaintDTO.Type = item.CommunityiReports.iReportsName;

                shortComplaintDTOList.Add(shortComplaintDTO);
            }



            if (complaints == null)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }
            responseComplaint.status = "Success";
            responseComplaint.model = shortComplaintDTOList;
            return responseComplaint;
        }

        [ResponseType(typeof(Complaint))]
        public async Task<Response<Complaint>> GetComplaintByID(int complaintID)
        {
            Response<Complaint> responseComplaint = new Response<Complaint>();
            Complaint complaint = new Complaint();
            complaint =await db.Complaints.Where(x => x.complaintID == complaintID).FirstOrDefaultAsync();
            if (complaint == null)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }
            responseComplaint.status = "Success";
            responseComplaint.model = complaint;
            return responseComplaint;
        }
        public async Task<Response<List<Complaint>>> GetComplaintByCommunityIDCatagoryID(int communityID, int selectedComplaintsiReportID)
        {
            Response<List<Complaint>> responseComplaint = new Response<List<Complaint>>();
            List<Complaint> complaints = new List<Complaint>();
            //All Complaints
            if (selectedComplaintsiReportID == -1)
            {
                complaints = db.Complaints.Where(x => x.communityID == communityID).Include(x => x.user).Include(y => y.CommunityiReports).ToList();
                complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            }
            //Sort by Ascending
            else if (selectedComplaintsiReportID == -2){
                complaints = db.Complaints.Where(x => x.communityID == communityID).Include(x => x.user).Include(y => y.CommunityiReports).ToList();

            }
            //Sort by Descending
            else if (selectedComplaintsiReportID == -3)
            {
                complaints = db.Complaints.Where(x => x.communityID == communityID).Include(x => x.user).Include(y => y.CommunityiReports).ToList();
                complaints = complaints.OrderByDescending(x => x.complaintID).ToList();
            }
            else
            {
                complaints = db.Complaints.Where(x => x.communityID == communityID && x.CommunityiReportsID == selectedComplaintsiReportID).Include(x => x.user).Include(y => y.CommunityiReports).ToList(); 
            }
             
            if (complaints == null)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }
            responseComplaint.status = "Success";
            responseComplaint.model = complaints;
            return responseComplaint;
        }
        // GET: api/Complaints/5
        //[ResponseType(typeof(Complaint))]
        //public async Task<IHttpActionResult> GetComplaint(int id)
        //{
        //    Complaint complaint = await db.Complaints.FindAsync(id);
        //    if (complaint == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(complaint);
        //}

        // PUT: api/Complaints/5
        [ResponseType(typeof(void))]
        public async Task<Response<Complaint>> PutComplaint(int id, Complaint complaint)
        {
            Response<Complaint> responseComplaint = new Response<Complaint>();
            if (!ModelState.IsValid)
            {
                responseComplaint.status = "No Complaints";
                responseComplaint.model = null;
                return responseComplaint;
            }

            if (id != complaint.complaintID)
            {
                responseComplaint.status = "Failed: ID Did Not Match";
                responseComplaint.model = null;
                return responseComplaint;
            }

            db.Entry(complaint).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(id))
                {
                    responseComplaint.status = "Failed: No Complaint ID Found";
                    responseComplaint.model = null;
                    return responseComplaint;
                }
                else
                {
                    throw;
                }
            }
            responseComplaint.status = "Success";
            responseComplaint.model = complaint;
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;


            Response<Notification> responceNotification = new Response<Notification>();
            NotificationsController notificationController = new NotificationsController();
            responceNotification = await notificationController.GetNotificationbyUserID(complaint.userID);


            int communityID = complaint.communityID;
            int complaintID = complaint.complaintID;
            var ComplaintFromDB = await (from l in db.Complaints
                                         where l.communityID == communityID && l.complaintID == complaintID
                                      select l).Include(x => x.community).FirstOrDefaultAsync();
            if (responceNotification.model.user.Islogout == false)
            {
                if (responceNotification.model.Report == true)
                {

                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + ComplaintFromDB.community.name + " : Your iReport has been Received.\",\"id\":\"3\",\"communityid\":\"" + complaint.communityID + "\",\"sound\":\"default\"}}";
                    outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(complaint.userID));


                    // Android
                    //"{ \"data\" : {\"message\":\"Your iReport has been Received.\",\"id\":\"3\"}}"

                    var notif = "{\"data\":{\"message\":\"" + ComplaintFromDB.community.name + " : Your iReport has been Received.\",\"badge\":\"1\",\"id\":\"3\",\"communityid\":\"" + complaint.communityID + "\"}}";
                    outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(complaint.userID));

                }
            }
           return responseComplaint;
        }




        public async Task<IHttpActionResult> GetComplaintforAndroid(int userID, string desc, int CommunityiReportsID, int communityID, string address, double? lat, double? lng)
        {
        
      

                Complaint complaint = new Complaint();



                complaint.userID = userID;
               
                            complaint.desc = desc;

                            complaint.CommunityiReportsID = CommunityiReportsID;
                   
                            complaint.communityID = communityID;
                   
                            complaint.address = address;
            
                            complaint.lat = lat;
                  
                            complaint.lng = lng;



                            DateTime ServerDateTime = DateTime.Now;
                            DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                            // ID from: 
                            // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                            // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                            string malayTimeZoneKey = "Singapore Standard Time";
                            TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                            DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);



                complaint.Date = malayDateTime;
                complaint.complaintStatusID = 1;

                Response<Complaint> complaintResponse = new Response<Complaint>();
                db.Entry(complaint).State = EntityState.Modified;
                //if (!ModelState.IsValid)
                //{
                //    complaintResponse.status = "Failure";
                //    complaintResponse.model = null;
                //    return Ok(complaintResponse);
                //}
                db.Complaints.Add(complaint);

                await db.SaveChangesAsync();
                complaintResponse.status = "Success";
                complaintResponse.model = complaint;
                return Ok(complaintResponse);

            
         
        }


        // POST: api/Complaints
        [ResponseType(typeof(Complaint))]
        public async Task<IHttpActionResult> PostComplaint()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);
                if (Request.Content.IsMimeMultipartContent())
                {
                    await Request.Content.ReadAsMultipartAsync(provider);

                    Complaint complaint = new Complaint();
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {

                            if (key.Equals("userID"))
                                complaint.userID = Convert.ToInt32(val);
                            if (key.Equals("desc"))
                                complaint.desc = val;
                            if (key.Equals("CommunityiReportsID"))
                                complaint.CommunityiReportsID = Convert.ToInt32(val);
                            if (key.Equals("communityID"))
                                complaint.communityID = Convert.ToInt32(val);
                            if (key.Equals("address"))
                                complaint.address = val;
                            if (key.Equals("lat"))
                                complaint.lat = Convert.ToDouble(val);
                            if (key.Equals("lng"))
                                complaint.lng = Convert.ToDouble(val);

                        }

                    }
                    


                    // Coordinated Universal Time string from 
                    // DateTime.Now.ToUniversalTime().ToString("u");
                    // Local .NET timeZone.
                    DateTime ServerDateTime = DateTime.Now;
                    DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                    // ID from: 
                    // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                    // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                    string malayTimeZoneKey = "Singapore Standard Time";
                    TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                    DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);

                    complaint.Date = malayDateTime;



                    complaint.complaintStatusID = 2;

                    Response<Complaint> complaintResponse = new Response<Complaint>();
                    if (!ModelState.IsValid)
                    {
                        complaintResponse.status = "Failure";
                        complaintResponse.model = null;
                        return Ok(complaintResponse);
                    }


                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        int count = 0;
                        foreach (string file in httpRequest.Files)
                        {

                            if (count == 0)
                            {
                                imageForBlob imageForBlob = new imageForBlob();
                                string blobImageURL = imageForBlob.ConvertImageForBlob(file, count);
                                complaint.image1 = blobImageURL;
                            }

                            else if (count == 1)
                            {
                                imageForBlob imageForBlob = new imageForBlob();
                                string blobImageURL = imageForBlob.ConvertImageForBlob(file, count);
                                complaint.image2 = blobImageURL;
                            }

                            else
                            {
                                imageForBlob imageForBlob = new imageForBlob();
                                string blobImageURL = imageForBlob.ConvertImageForBlob(file, count);
                                complaint.image3 = blobImageURL;
                            }

                            count++;
                        }
                    }




                    db.Complaints.Add(complaint);

                    await db.SaveChangesAsync();
                    complaintResponse.status = "Success";
                    complaintResponse.model = complaint;
                    return Ok(complaintResponse);

                }
                else
                {
                    Response<Complaint> complaintResponse = new Response<Complaint>();
                    complaintResponse.status = "Failed: Not Multipart Content";
                    complaintResponse.model = null;
                    return Ok(complaintResponse);
                }

            }
            catch (Exception ex)
            {
                throw new HttpException (ex.InnerException.ToString());
            }
        }

   
        // DELETE: api/Complaints/5
        [ResponseType(typeof(Complaint))]
        public async Task<IHttpActionResult> DeleteComplaint(int id)
        {
            Complaint complaint = await db.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            db.Complaints.Remove(complaint);
            await db.SaveChangesAsync();

            return Ok(complaint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComplaintExists(int id)
        {
            return db.Complaints.Count(e => e.complaintID == id) > 0;
        }
    }
}