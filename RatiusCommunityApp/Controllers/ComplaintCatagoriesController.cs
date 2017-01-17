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
    public class ComplaintCatagoriesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/ComplaintCatagories
        public async Task<Response<List<ComplaintCatagory>>> GetComplaintCatagories()
        {
            Response<List<ComplaintCatagory>> responceComplaintCatagory = new Response<List<ComplaintCatagory>>();
            List<ComplaintCatagory> complaintCatagory = new List<ComplaintCatagory>();
            complaintCatagory = (from l in db.ComplaintCatagories
                       select l).ToList();
            if (complaintCatagory == null)
            {
                responceComplaintCatagory.status = "Failed: No Community Member";
                responceComplaintCatagory.model = null;
                return responceComplaintCatagory;
            }

            responceComplaintCatagory.status = "Success";
            responceComplaintCatagory.model = complaintCatagory;
            return responceComplaintCatagory;
        }

        // GET: api/ComplaintCatagories/5
        [ResponseType(typeof(ComplaintCatagory))]
        public async Task<IHttpActionResult> GetComplaintCatagory(int id)
        {
            ComplaintCatagory complaintCatagory = await db.ComplaintCatagories.FindAsync(id);
            if (complaintCatagory == null)
            {
                return NotFound();
            }

            return Ok(complaintCatagory);
        }

        // PUT: api/ComplaintCatagories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutComplaintCatagory(int id, ComplaintCatagory complaintCatagory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaintCatagory.complaintCatagoryID)
            {
                return BadRequest();
            }

            db.Entry(complaintCatagory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintCatagoryExists(id))
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

        // POST: api/ComplaintCatagories
        [ResponseType(typeof(ComplaintCatagory))]
        public async Task<IHttpActionResult> PostComplaintCatagory(ComplaintCatagory complaintCatagory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ComplaintCatagories.Add(complaintCatagory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = complaintCatagory.complaintCatagoryID }, complaintCatagory);
        }

        // DELETE: api/ComplaintCatagories/5
        [ResponseType(typeof(ComplaintCatagory))]
        public async Task<IHttpActionResult> DeleteComplaintCatagory(int id)
        {
            ComplaintCatagory complaintCatagory = await db.ComplaintCatagories.FindAsync(id);
            if (complaintCatagory == null)
            {
                return NotFound();
            }

            db.ComplaintCatagories.Remove(complaintCatagory);
            await db.SaveChangesAsync();

            return Ok(complaintCatagory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComplaintCatagoryExists(int id)
        {
            return db.ComplaintCatagories.Count(e => e.complaintCatagoryID == id) > 0;
        }
    }
}