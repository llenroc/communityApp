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
    public class ReportTypesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/ReportTypes
        public IQueryable<ReportType> GetReportTypes()
        {
            return db.ReportTypes;
        }

        // GET: api/ReportTypes/5
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> GetReportType(int id)
        {
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            if (reportType == null)
            {
                return NotFound();
            }

            return Ok(reportType);
        }

        // PUT: api/ReportTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReportType(int id, ReportType reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportType.reportTypeID)
            {
                return BadRequest();
            }

            db.Entry(reportType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportTypeExists(id))
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

        // POST: api/ReportTypes
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> PostReportType(ReportType reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReportTypes.Add(reportType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reportType.reportTypeID }, reportType);
        }

        // DELETE: api/ReportTypes/5
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> DeleteReportType(int id)
        {
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            if (reportType == null)
            {
                return NotFound();
            }

            db.ReportTypes.Remove(reportType);
            await db.SaveChangesAsync();

            return Ok(reportType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportTypeExists(int id)
        {
            return db.ReportTypes.Count(e => e.reportTypeID == id) > 0;
        }
    }
}