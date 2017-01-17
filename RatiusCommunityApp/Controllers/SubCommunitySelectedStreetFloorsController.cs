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
    public class SubCommunitySelectedStreetFloorsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/SubCommunitySelectedStreetFloors
        public List<SubCommunitySelectedStreetFloor> GetSubCommunitySelectedStreetFloors()
        {
            List<SubCommunitySelectedStreetFloor> SubCommunitySelectedStreetFloorList = new List<SubCommunitySelectedStreetFloor>();
            SubCommunitySelectedStreetFloorList = db.SubCommunitySelectedStreetFloors.ToList();
            return SubCommunitySelectedStreetFloorList;
        }
        [Route("GetALLSubCommunitySelectedStreetFloorsID")]
        public List<int> GetALLSubCommunitySelectedStreetFloorsID()
        {
            List<int> SubCommunitySelectedStreetFloorList = new List<int>();
            SubCommunitySelectedStreetFloorList = db.SubCommunitySelectedStreetFloors.Select(x=>x.communityStreetFloorId).ToList();
            return SubCommunitySelectedStreetFloorList;
        }


        [Route("GetSubCommunitySelectedStreetFloorsID")]
        public List<int> GetSubCommunitySelectedStreetFloorsID(int subCommunityId)
        {
            List<int> SubCommunitySelectedStreetFloorList = new List<int>();
            SubCommunitySelectedStreetFloorList = db.SubCommunitySelectedStreetFloors.Where(x=>x.subCommunityId==subCommunityId).Select(x => x.communityStreetFloorId).ToList();
            return SubCommunitySelectedStreetFloorList;
        }



        [Route("GetSubCommunitySelectedStreetFloorsIDbyCommunityStreetFloorID")]
        public SubCommunitySelectedStreetFloor GetSubCommunitySelectedStreetFloorsIDbyCommunityStreetFloorID(int communityStreetFloorId)
        {
            SubCommunitySelectedStreetFloor SubCommunitySelectedStreetFloor = new SubCommunitySelectedStreetFloor();
            SubCommunitySelectedStreetFloor = db.SubCommunitySelectedStreetFloors.Where(x => x.communityStreetFloorId == communityStreetFloorId).FirstOrDefault();
            return SubCommunitySelectedStreetFloor;
        }
        // GET: api/SubCommunitySelectedStreetFloors/5
        [ResponseType(typeof(SubCommunitySelectedStreetFloor))]
        public async Task<IHttpActionResult> GetSubCommunitySelectedStreetFloor(int id)
        {
            SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor = await db.SubCommunitySelectedStreetFloors.FindAsync(id);
            if (subCommunitySelectedStreetFloor == null)
            {
                return NotFound();
            }

            return Ok(subCommunitySelectedStreetFloor);
        }


        // PUT: api/SubCommunitySelectedStreetFloors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubCommunitySelectedStreetFloor(int id, SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCommunitySelectedStreetFloor.id)
            {
                return BadRequest();
            }

            db.Entry(subCommunitySelectedStreetFloor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCommunitySelectedStreetFloorExists(id))
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

        // POST: api/SubCommunitySelectedStreetFloors
        [ResponseType(typeof(SubCommunitySelectedStreetFloor))]
        public async Task<Response<SubCommunitySelectedStreetFloor>> PostSubCommunitySelectedStreetFloor(SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor)
        {
            Response<SubCommunitySelectedStreetFloor> responseSubCommunitySelectedStreetFloor = new Response<SubCommunitySelectedStreetFloor>();

            if (!ModelState.IsValid)
            {
                responseSubCommunitySelectedStreetFloor.status = "failuer";
                responseSubCommunitySelectedStreetFloor.model = null;
                return responseSubCommunitySelectedStreetFloor;
            }

            db.SubCommunitySelectedStreetFloors.Add(subCommunitySelectedStreetFloor);
            await db.SaveChangesAsync();
            responseSubCommunitySelectedStreetFloor.status = "Success";
            responseSubCommunitySelectedStreetFloor.model = subCommunitySelectedStreetFloor;
            return responseSubCommunitySelectedStreetFloor;
        }

        // DELETE: api/SubCommunitySelectedStreetFloors/5
        [ResponseType(typeof(SubCommunitySelectedStreetFloor))]
        public async Task<IHttpActionResult> DeleteSubCommunitySelectedStreetFloor(int id)
        {
            SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor = await db.SubCommunitySelectedStreetFloors.FindAsync(id);
            if (subCommunitySelectedStreetFloor == null)
            {
                return NotFound();
            }

            db.SubCommunitySelectedStreetFloors.Remove(subCommunitySelectedStreetFloor);
            await db.SaveChangesAsync();

            return Ok(subCommunitySelectedStreetFloor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCommunitySelectedStreetFloorExists(int id)
        {
            return db.SubCommunitySelectedStreetFloors.Count(e => e.id == id) > 0;
        }
    }
}