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
    public class ComplaintStatusController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/ComplaintStatus
        public IQueryable<ComplaintStatus> GetComplaintStatus()
        {
            return db.ComplaintStatus;
        }

        // GET: api/ComplaintStatus/5
        [ResponseType(typeof(ComplaintStatus))]
        public async Task<IHttpActionResult> GetComplaintStatus(int id)
        {
            ComplaintStatus complaintStatus = await db.ComplaintStatus.FindAsync(id);
            if (complaintStatus == null)
            {
                return NotFound();
            }

            return Ok(complaintStatus);
        }

        // PUT: api/ComplaintStatus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutComplaintStatus(int id, ComplaintStatus complaintStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaintStatus.complaintStatusID)
            {
                return BadRequest();
            }

            db.Entry(complaintStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintStatusExists(id))
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

        // POST: api/ComplaintStatus
        [ResponseType(typeof(ComplaintStatus))]
        public async Task<IHttpActionResult> PostComplaintStatus(ComplaintStatus complaintStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ComplaintStatus.Add(complaintStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = complaintStatus.complaintStatusID }, complaintStatus);
        }

        // DELETE: api/ComplaintStatus/5
        [ResponseType(typeof(ComplaintStatus))]
        public async Task<IHttpActionResult> DeleteComplaintStatus(int id)
        {
            ComplaintStatus complaintStatus = await db.ComplaintStatus.FindAsync(id);
            if (complaintStatus == null)
            {
                return NotFound();
            }

            db.ComplaintStatus.Remove(complaintStatus);
            await db.SaveChangesAsync();

            return Ok(complaintStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComplaintStatusExists(int id)
        {
            return db.ComplaintStatus.Count(e => e.complaintStatusID == id) > 0;
        }
    }
}