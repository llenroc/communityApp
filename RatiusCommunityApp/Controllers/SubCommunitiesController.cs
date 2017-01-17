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
    public class SubCommunitiesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/SubCommunities
        public IQueryable<SubCommunity> GetSubCommunities()
        {
            return db.SubCommunities;
        }
        [Route("GetLastCommunity")]
        public Response<SubCommunity> GetLastSubCommunity()
        {
            Response<SubCommunity> responseSubCommunity = new Response<SubCommunity>();
            var lastSubcommunity = db.SubCommunities.LastOrDefault();
            responseSubCommunity.model = lastSubcommunity;
            return responseSubCommunity;
        }

        // GET: api/SubCommunities/5
        [ResponseType(typeof(SubCommunity))]
        public async Task<IHttpActionResult> GetSubCommunity(int id)
        {
            SubCommunity subCommunity = await db.SubCommunities.FindAsync(id);
            if (subCommunity == null)
            {
                return NotFound();
            }

            return Ok(subCommunity);
        }
        public async Task<List<SubCommunity>> GetSubCommunitiesByCommunityId(int communityId)
        {

            List<SubCommunity> SubCommunityList = new List<SubCommunity>();
            SubCommunityList = (from l in db.SubCommunities
                                        where l.communityID == communityId
                                        select l).OrderByDescending(x => x.id).ToList();



            return SubCommunityList;
        }

        // PUT: api/SubCommunities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubCommunity(int id, SubCommunity subCommunity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCommunity.id)
            {
                return BadRequest();
            }

            db.Entry(subCommunity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCommunityExists(id))
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

        // POST: api/SubCommunities
        [ResponseType(typeof(SubCommunity))]
        public async Task<Response<SubCommunity>> PostSubCommunity(SubCommunity subCommunity)
        {
            Response<SubCommunity> responseSubCommunity = new Response<SubCommunity>();
            if (!ModelState.IsValid)
            {
                responseSubCommunity.status = "Failed";
                responseSubCommunity.model = null;
                return responseSubCommunity;
            }

            db.SubCommunities.Add(subCommunity);
            await db.SaveChangesAsync();

            responseSubCommunity.status = "Success";
            responseSubCommunity.model = subCommunity;

            return responseSubCommunity;
        }

        // DELETE: api/SubCommunities/5
        [ResponseType(typeof(SubCommunity))]
        public async Task<IHttpActionResult> DeleteSubCommunity(int id)
        {
            SubCommunity subCommunity = await db.SubCommunities.FindAsync(id);
            if (subCommunity == null)
            {
                return NotFound();
            }

            db.SubCommunities.Remove(subCommunity);
            await db.SaveChangesAsync();

            return Ok(subCommunity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCommunityExists(int id)
        {
            return db.SubCommunities.Count(e => e.id == id) > 0;
        }
    }
}