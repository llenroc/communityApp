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
    public class ReportsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Reports
        public IQueryable<Report> GetReports()
        {
            return db.Reports;
        }

        // GET: api/Reports/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> GetReport(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }
        // GET: api/Reports/5
        [ResponseType(typeof(Report))]
        public async Task<Response<List<Report>>> GetComplaintByCommunityID(int communityID)
        {
            Response<List<Report>> reportResponse = new Response<List<Report>>();
            List<Report> reports = new List<Report>();
            reports = (from l in db.Reports
                          where l.communityID == communityID
                          select l).ToList();
            if (reports == null)
            {
                reportResponse.status = "No Reports";
                reportResponse.model = null;
                return reportResponse;
            }
            reportResponse.status = "Success";
            reportResponse.model = reports;
            return reportResponse;
        }

        // PUT: api/Reports/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReport(int id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.id)
            {
                return BadRequest();
            }

            db.Entry(report).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
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

        // POST: api/Reports
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> PostReport()
        {
            var httpRequest = HttpContext.Current.Request;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                Report report = new Report();
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {

                        if (key.Equals("reportTypeID"))
                            report.userID = Convert.ToInt32(val);
                        if (key.Equals("userID"))
                            report.userID =Convert.ToInt32(val);
                        if (key.Equals("communityID"))
                            report.communityID = Convert.ToInt32(val);
                        if (key.Equals("reportCatagoryID"))
                            report.reportCatagoryID= Convert.ToInt32(val);
                        if (key.Equals("description"))
                            report.description = val;
                    }

                }

                DateTime ServerDateTime = DateTime.Now;
                DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                // ID from: 
                // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                string malayTimeZoneKey = "Singapore Standard Time";
                TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);
                report.Date = malayDateTime;

                Response<Report> reportResponse = new Response<Report>();
                if (!ModelState.IsValid)
                {
                    reportResponse.status = "Failure";
                    reportResponse.model = null;
                    return Ok(reportResponse);
                }


                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    int count = 0;
                    foreach (string file in httpRequest.Files)
                    {
                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertImageForBlob();
                        if (count == 0)
                            report.image1 = blobImageURL;
                        if (count == 1)
                            report.image2 = blobImageURL;
                        else
                            report.image3 = blobImageURL;
                        count++;
                    }
                }




                db.Reports.Add(report);

                await db.SaveChangesAsync();
                reportResponse.status = "Success";
                reportResponse.model = report;
                return Ok(reportResponse);

            }
            else
            {
                Response<Report> reportResponse = new Response<Report>();
                reportResponse.status = "Failed: Not Multipart Content";
                reportResponse.model = null;
                return Ok(reportResponse);
            }
        }

        // DELETE: api/Reports/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> DeleteReport(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            db.Reports.Remove(report);
            await db.SaveChangesAsync();

            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportExists(int id)
        {
            return db.Reports.Count(e => e.id == id) > 0;
        }
    }
}