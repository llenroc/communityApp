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
    public class CommunitySecretCodesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunitySecretKeys
        public IQueryable<CommunitySecretCodes> GetCommunitySecretCodes()
        {
            return db.CommunitySecretKeys;
        }

        // GET: api/CommunitySecretKeys/5
        [ResponseType(typeof(CommunitySecretCodes))]
        public async Task<Response<CommunitySecretCodes>> GetCommunitySecretCode(int id)
        {
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            CommunitySecretCodes communitySecretKeys = await db.CommunitySecretKeys.FindAsync(id);
            if (communitySecretKeys == null)
            {
                responceCommunitySecretKeys.status = "Failed: SecretKey did not found";
                responceCommunitySecretKeys.model = null;
                return responceCommunitySecretKeys;
            }

            responceCommunitySecretKeys.status = "Success";
            responceCommunitySecretKeys.model = communitySecretKeys;
            return responceCommunitySecretKeys;
        }
    [Route("api/GetCommunitySecretKeysBySecretCode")]
        public async Task<Response<CommunitySecretCodes>> GetCommunitySecretKeysBySecretCode(string SecretCode)
        {
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            CommunitySecretCodes communitySecretKeys = await db.CommunitySecretKeys.Where(x => x.secretCode == SecretCode).Include(x=>x.community).FirstOrDefaultAsync(); ;
            if (communitySecretKeys == null)
            {
                responceCommunitySecretKeys.status = "Failed: SecretKey did not found";
                responceCommunitySecretKeys.model = null;
                return responceCommunitySecretKeys;
            }

            responceCommunitySecretKeys.status = "Success";
            responceCommunitySecretKeys.model = communitySecretKeys;
            return responceCommunitySecretKeys;
        }

    [Route("api/GetCommunitySecretKeysAndStreetFloorBySecretCode")]
    public async Task<Response<SecretCodeDTO>> GetCommunitySecretKeysAndStreetFloorBySecretCode(string SecretCode)
    {
        Response<SecretCodeDTO> responceSecretCodeDTO = new Response<SecretCodeDTO>();
        SecretCodeDTO secretCodeDTO = new SecretCodeDTO();
        CommunitySecretCodes communitySecretKeys = await db.CommunitySecretKeys.Where(x => x.secretCode == SecretCode).Include(x => x.community).FirstOrDefaultAsync(); ;
        if (communitySecretKeys == null)
        {
            responceSecretCodeDTO.status = "Failed: SecretKey did not found";
            responceSecretCodeDTO.model = null;
            return responceSecretCodeDTO;
        }
        int communityID = communitySecretKeys.communityID;
        secretCodeDTO.community = (from l in db.Communities
                                          where l.communityID == communityID
                                   select l).AsNoTracking().FirstOrDefault();

        secretCodeDTO.communityStreetFloor = (from l in db.CommunityStreetFloors
                                                     where l.communityID == communityID
                                              select l).OrderByDescending(x => x.id).AsNoTracking().ToList();
        responceSecretCodeDTO.status = "Success";
        responceSecretCodeDTO.model = secretCodeDTO;
        return responceSecretCodeDTO;
    }
        // PUT: api/CommunitySecretKeys/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunitySecretCodes>> PutCommunitySecretCodes(int id, CommunitySecretCodes communitySecretKeys)
        {
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            if (!ModelState.IsValid)
            {
                responceCommunitySecretKeys.status = "Failure";
                responceCommunitySecretKeys.model = null;
                return responceCommunitySecretKeys;
            }

            if (id != communitySecretKeys.id)
            {
                responceCommunitySecretKeys.status = "Failed: ID did not Match";
                responceCommunitySecretKeys.model = null;
                return responceCommunitySecretKeys;
            }

            db.Entry(communitySecretKeys).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunitySecretKeysExists(id))
                {
                    responceCommunitySecretKeys.status = "Failed: ID did not Exist";
                    responceCommunitySecretKeys.model = null;
                    return responceCommunitySecretKeys;
                }
                else
                {
                    responceCommunitySecretKeys.status = "Failed: DB Update Exception";
                    responceCommunitySecretKeys.model = null;
                    return responceCommunitySecretKeys;
                }
            }

            responceCommunitySecretKeys.status = "Success";
            responceCommunitySecretKeys.model = communitySecretKeys;
            return responceCommunitySecretKeys;
        }


        // POST: api/CommunitySecretKeys
        [ResponseType(typeof(CommunitySecretCodes))]
        public async Task<Response<CommunitySecretCodes>> PostCommunitySecretCodes(CommunitySecretCodes communitySecretCodes)
        {
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            if (!ModelState.IsValid)
            {
                responceCommunitySecretKeys.status = "Failure";
                responceCommunitySecretKeys.model = null;
                return responceCommunitySecretKeys;
            }

            db.CommunitySecretKeys.Add(communitySecretCodes);
            await db.SaveChangesAsync();
            responceCommunitySecretKeys.status = "Success";
            responceCommunitySecretKeys.model = communitySecretCodes;
            return responceCommunitySecretKeys;
        }

        // DELETE: api/CommunitySecretKeys/5
        [ResponseType(typeof(CommunitySecretCodes))]
        public async Task<IHttpActionResult> DeleteCommunitySecretKeys(int id)
        {
            CommunitySecretCodes communitySecretKeys = await db.CommunitySecretKeys.FindAsync(id);
            if (communitySecretKeys == null)
            {
                return NotFound();
            }

            db.CommunitySecretKeys.Remove(communitySecretKeys);
            await db.SaveChangesAsync();

            return Ok(communitySecretKeys);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunitySecretKeysExists(int id)
        {
            return db.CommunitySecretKeys.Count(e => e.id == id) > 0;
        }
    }
}