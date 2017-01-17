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
    public class SubCommunityEmergencyContactsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/SubCommunityEmergencyContacts
        public IQueryable<SubCommunityEmergencyContacts> GetSubCommunityEmergencyContacts()
        {
            return db.SubCommunityEmergencyContacts;
        }

        // GET: api/SubCommunityEmergencyContacts/5
        [ResponseType(typeof(SubCommunityEmergencyContacts))]
        public async Task<Response<SubCommunityEmergencyContacts>> GetSubCommunityEmergencyContacts(int id)
        {
            Response<SubCommunityEmergencyContacts> responseSubCommunityEmergencyContacts = new Response<SubCommunityEmergencyContacts>();
            SubCommunityEmergencyContacts subCommunityEmergencyContacts = await db.SubCommunityEmergencyContacts.FindAsync(id);
            if (subCommunityEmergencyContacts == null)
            {
                responseSubCommunityEmergencyContacts.status = "No SubCommunity Find";
                responseSubCommunityEmergencyContacts.model = null;
                return responseSubCommunityEmergencyContacts;
            }

            responseSubCommunityEmergencyContacts.status = "Success";
            responseSubCommunityEmergencyContacts.model = subCommunityEmergencyContacts;
            return responseSubCommunityEmergencyContacts;
        }


        public async Task<List<SubCommunityEmergencyContacts>> GetSubCommunityEmergencyContactsbySubCommunity(int SubCommunityid)
        {
            List<SubCommunityEmergencyContacts> subCommunityEmergencyContacts = new List<SubCommunityEmergencyContacts>();
            subCommunityEmergencyContacts = db.SubCommunityEmergencyContacts.Where(x => x.subCommunityId==SubCommunityid).ToList();
            return subCommunityEmergencyContacts;
        }
        // PUT: api/SubCommunityEmergencyContacts/5
        [ResponseType(typeof(void))]
        public async Task<Response<SubCommunityEmergencyContacts>> PutSubCommunityEmergencyContacts(int id, SubCommunityEmergencyContacts subCommunityEmergencyContacts)
        {
            Response<SubCommunityEmergencyContacts> responseSubCommunityEmergencyContacts = new Response<SubCommunityEmergencyContacts>();
            
            if (!ModelState.IsValid)
            {
                responseSubCommunityEmergencyContacts.status = "Failuer : Model State is invalid";
                responseSubCommunityEmergencyContacts.model = null;
                return responseSubCommunityEmergencyContacts;
            }

            if (id != subCommunityEmergencyContacts.id)
            {
                responseSubCommunityEmergencyContacts.status = "Failuer : id did not match";
                responseSubCommunityEmergencyContacts.model = null;
                return responseSubCommunityEmergencyContacts;
            }

            db.Entry(subCommunityEmergencyContacts).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCommunityEmergencyContactsExists(id))
                {
                    responseSubCommunityEmergencyContacts.status = "Failuer : Db Update Concurrency Exception";
                    responseSubCommunityEmergencyContacts.model = null;
                    return responseSubCommunityEmergencyContacts;
                }
                else
                {
                    throw;
                }
            }

            responseSubCommunityEmergencyContacts.status = "Success";
            responseSubCommunityEmergencyContacts.model = subCommunityEmergencyContacts;
            return responseSubCommunityEmergencyContacts;
        }

        // POST: api/SubCommunityEmergencyContacts
        [ResponseType(typeof(SubCommunityEmergencyContacts))]
        public async Task<Response<SubCommunityEmergencyContacts>> PostSubCommunityEmergencyContacts(SubCommunityEmergencyContacts subCommunityEmergencyContacts)
        {
            Response<SubCommunityEmergencyContacts> responseSubCommunityEmergencyContacts = new Response<SubCommunityEmergencyContacts>();
            
            if (!ModelState.IsValid)
            {
                responseSubCommunityEmergencyContacts.status = "Failuer : Model State is invalid";
                responseSubCommunityEmergencyContacts.model = null;
                return responseSubCommunityEmergencyContacts;
            }

            db.SubCommunityEmergencyContacts.Add(subCommunityEmergencyContacts);
            await db.SaveChangesAsync();

            responseSubCommunityEmergencyContacts.status = "Success";
            responseSubCommunityEmergencyContacts.model = subCommunityEmergencyContacts;
            return responseSubCommunityEmergencyContacts;
        }

        // DELETE: api/SubCommunityEmergencyContacts/5
        [ResponseType(typeof(SubCommunityEmergencyContacts))]
        public async Task<IHttpActionResult> DeleteSubCommunityEmergencyContacts(int id)
        {
            SubCommunityEmergencyContacts subCommunityEmergencyContacts = await db.SubCommunityEmergencyContacts.FindAsync(id);
            if (subCommunityEmergencyContacts == null)
            {
                return NotFound();
            }

            db.SubCommunityEmergencyContacts.Remove(subCommunityEmergencyContacts);
            await db.SaveChangesAsync();

            return Ok(subCommunityEmergencyContacts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCommunityEmergencyContactsExists(int id)
        {
            return db.SubCommunityEmergencyContacts.Count(e => e.id == id) > 0;
        }
    }
}