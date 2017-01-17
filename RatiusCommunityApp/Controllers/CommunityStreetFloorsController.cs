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
    public class CommunityStreetFloorsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunityStreetFloors
        public IQueryable<CommunityStreetFloor> GetCommunityStreetFloors()
        {
            return db.CommunityStreetFloors;
        }
      
        // GET: api/CommunityStreetFloors/5
        [ResponseType(typeof(CommunityStreetFloor))]
        public async Task<Response<CommunityStreetFloor>> GetCommunityStreetFloor(int id)
        {
            Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            CommunityStreetFloor communityStreetFloor = await db.CommunityStreetFloors.FindAsync(id);
            if (communityStreetFloor == null)
            {
                responceCommunityStreetFloor.status = "Failed: Street/Floor did not found";
                responceCommunityStreetFloor.model = null;
                return responceCommunityStreetFloor;
            }

            responceCommunityStreetFloor.status = "Success";
            responceCommunityStreetFloor.model = communityStreetFloor;
            return responceCommunityStreetFloor;
        }
        public async Task<List<CommunityStreetFloor>> GetCommunityStreetFloorByCommunityId(int communityId)
        {
           
            List<CommunityStreetFloor> communityStreetFloorList = new List<CommunityStreetFloor>();
            communityStreetFloorList = (from l in db.CommunityStreetFloors
                                    where l.communityID == communityId
                                    select l).OrderByDescending(x => x.id).ToList();



            return communityStreetFloorList;
        }

        // PUT: api/CommunityStreetFloors/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunityStreetFloor>> PutCommunityStreetFloor(int id, CommunityStreetFloor communityStreetFloor)
        {
            Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            if (!ModelState.IsValid)
            {
                responceCommunityStreetFloor.status = "Failure";
                responceCommunityStreetFloor.model = null;
                return responceCommunityStreetFloor;
            }

            if (id != communityStreetFloor.id)
            {
                responceCommunityStreetFloor.status = "Failed: ID did not Match";
                responceCommunityStreetFloor.model = null;
                return responceCommunityStreetFloor;
            }

            db.Entry(communityStreetFloor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityStreetFloorExists(id))
                {
                    responceCommunityStreetFloor.status = "Failed: ID did not Exist";
                    responceCommunityStreetFloor.model = null;
                    return responceCommunityStreetFloor;
                }
                else
                {
                    responceCommunityStreetFloor.status = "Failed: DB Update Exception";
                    responceCommunityStreetFloor.model = null;
                    return responceCommunityStreetFloor;
                }
            }

            responceCommunityStreetFloor.status = "Success";
            responceCommunityStreetFloor.model = communityStreetFloor;
            return responceCommunityStreetFloor;
        }

        // POST: api/CommunityStreetFloors
        [ResponseType(typeof(CommunityStreetFloor))]
        public async Task<Response<CommunityStreetFloor>> PostCommunityStreetFloor(CommunityStreetFloor communityStreetFloor)
        {
            Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            if (!ModelState.IsValid)
            {
                responceCommunityStreetFloor.status = "Failure";
                responceCommunityStreetFloor.model = null;
                return responceCommunityStreetFloor;
            }

            db.CommunityStreetFloors.Add(communityStreetFloor);
            await db.SaveChangesAsync();
            responceCommunityStreetFloor.status = "Success";
            responceCommunityStreetFloor.model = communityStreetFloor;
            return responceCommunityStreetFloor;
        }

        // DELETE: api/CommunityStreetFloors/5
        [ResponseType(typeof(CommunityStreetFloor))]
        public async Task<IHttpActionResult> DeleteCommunityStreetFloor(int id)
        {
            CommunityStreetFloor communityStreetFloor = await db.CommunityStreetFloors.FindAsync(id);
            if (communityStreetFloor == null)
            {
                return NotFound();
            }

            db.CommunityStreetFloors.Remove(communityStreetFloor);
            await db.SaveChangesAsync();

            return Ok(communityStreetFloor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityStreetFloorExists(int id)
        {
            return db.CommunityStreetFloors.Count(e => e.id == id) > 0;
        }
    }
}