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
    public class CommunityEmergencyContactsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunityEmergencyContacts
        public IQueryable<CommunityEmergencyContacts> GetCommunityEmergencyContacts()
        {
            return db.CommunityEmergencyContacts;
        }

        // GET: api/CommunityEmergencyContacts/5
        [ResponseType(typeof(CommunityEmergencyContacts))]
        public async Task<Response<CommunityEmergencyContacts>> GetCommunityEmergencyContacts(int id)
        {
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            CommunityEmergencyContacts communityEmergencyContacts = await db.CommunityEmergencyContacts.FindAsync(id);
            if (communityEmergencyContacts == null)
            {
                responceCommunityEmergencyContacts.status = "Failed: Emergency Contact did not found";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityEmergencyContacts;
            return responceCommunityEmergencyContacts;
        }

        // PUT: api/CommunityEmergencyContacts/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunityEmergencyContacts>> PutCommunityEmergencyContacts(int id, CommunityEmergencyContacts communityEmergencyContacts)
        {
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            if (!ModelState.IsValid)
            {
                responceCommunityEmergencyContacts.status = "Failure";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            if (id != communityEmergencyContacts.id)
            {
                responceCommunityEmergencyContacts.status = "Failed: ID did not Match";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            db.Entry(communityEmergencyContacts).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityEmergencyContactsExists(id))
                {
                    responceCommunityEmergencyContacts.status = "Failed: ID did not Exist";
                    responceCommunityEmergencyContacts.model = null;
                    return responceCommunityEmergencyContacts;
                }
                else
                {
                    responceCommunityEmergencyContacts.status = "Failed: DB Update Exception";
                    responceCommunityEmergencyContacts.model = null;
                    return responceCommunityEmergencyContacts;
                }
            }
            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityEmergencyContacts;
            return responceCommunityEmergencyContacts;
        }

        // POST: api/CommunityEmergencyContacts
        [ResponseType(typeof(CommunityEmergencyContacts))]
        public async Task<Response<CommunityEmergencyContacts>> PostCommunityEmergencyContacts(CommunityEmergencyContacts communityEmergencyContacts)
        {
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            if (!ModelState.IsValid)
            {
                responceCommunityEmergencyContacts.status = "Failure";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            db.CommunityEmergencyContacts.Add(communityEmergencyContacts);
            await db.SaveChangesAsync();

            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityEmergencyContacts;
            return responceCommunityEmergencyContacts;
        }

        // DELETE: api/CommunityEmergencyContacts/5
        [ResponseType(typeof(CommunityEmergencyContacts))]
        public async Task<IHttpActionResult> DeleteCommunityEmergencyContacts(int id)
        {
            CommunityEmergencyContacts communityEmergencyContacts = await db.CommunityEmergencyContacts.FindAsync(id);
            if (communityEmergencyContacts == null)
            {
                return NotFound();
            }

            db.CommunityEmergencyContacts.Remove(communityEmergencyContacts);
            await db.SaveChangesAsync();

            return Ok(communityEmergencyContacts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityEmergencyContactsExists(int id)
        {
            return db.CommunityEmergencyContacts.Count(e => e.id == id) > 0;
        }
    }
}