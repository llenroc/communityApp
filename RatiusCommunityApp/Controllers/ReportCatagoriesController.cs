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
    public class ReportCatagoriesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/ReportCatagories
        public IQueryable<ReportCatagory> GetReportCatagories()
        {
            return db.ReportCatagories;
        }

        // GET: api/ReportCatagories/5
        [ResponseType(typeof(ReportCatagory))]
        public async Task<IHttpActionResult> GetReportCatagory(int id)
        {
            ReportCatagory reportCatagory = await db.ReportCatagories.FindAsync(id);
            if (reportCatagory == null)
            {
                return NotFound();
            }

            return Ok(reportCatagory);
        }

        // PUT: api/ReportCatagories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReportCatagory(int id, ReportCatagory reportCatagory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportCatagory.reportCatagoryID)
            {
                return BadRequest();
            }

            db.Entry(reportCatagory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportCatagoryExists(id))
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

        // POST: api/ReportCatagories
        [ResponseType(typeof(ReportCatagory))]
        public async Task<IHttpActionResult> PostReportCatagory(ReportCatagory reportCatagory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReportCatagories.Add(reportCatagory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reportCatagory.reportCatagoryID }, reportCatagory);
        }

        // DELETE: api/ReportCatagories/5
        [ResponseType(typeof(ReportCatagory))]
        public async Task<IHttpActionResult> DeleteReportCatagory(int id)
        {
            ReportCatagory reportCatagory = await db.ReportCatagories.FindAsync(id);
            if (reportCatagory == null)
            {
                return NotFound();
            }

            db.ReportCatagories.Remove(reportCatagory);
            await db.SaveChangesAsync();

            return Ok(reportCatagory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportCatagoryExists(int id)
        {
            return db.ReportCatagories.Count(e => e.reportCatagoryID == id) > 0;
        }
    }
}