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
    public class CommunityImagesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunityImages
        public IQueryable<CommunityImage> GetCommunityImages()
        {
            return db.CommunityImages;
        }
    
        public List<CommunityImage> GetCommunityImagesbyCommunityID(int communityID)
        {
            List<CommunityImage> listCommunityImages = new List<CommunityImage>();
            listCommunityImages = (from l in db.CommunityImages
                                   where l.communityID==communityID
                                   select l).ToList();
            return listCommunityImages;
        }
        // GET: api/CommunityImages/5
        [ResponseType(typeof(CommunityImage))]
        public async Task<Response<CommunityImage>> GetCommunityImagebyID(int id)
        {
            Response<CommunityImage> responceCommunityEmergencyContacts = new Response<CommunityImage>();
            CommunityImage communityImage = await db.CommunityImages.FindAsync(id);
            if (communityImage == null)
            {
                responceCommunityEmergencyContacts.status = "Failed: No Community Image Found";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityImage;
            return responceCommunityEmergencyContacts;
        }

        // PUT: api/CommunityImages/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunityImage>> PutCommunityImage(int id, CommunityImage communityImage)
        {
            Response<CommunityImage> responceCommunityEmergencyContacts = new Response<CommunityImage>();
            if (!ModelState.IsValid)
            {
                responceCommunityEmergencyContacts.status = "Failure";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            if (id != communityImage.id)
            {
                responceCommunityEmergencyContacts.status = "Failed: CommunityID did not Match";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            db.Entry(communityImage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityImageExists(id))
                {
                    responceCommunityEmergencyContacts.status = "Failed: Id did not Exist";
                    responceCommunityEmergencyContacts.model = null;
                    return responceCommunityEmergencyContacts;
                }
                else
                {
                    responceCommunityEmergencyContacts.status = "Failed: DB Exception";
                    responceCommunityEmergencyContacts.model = null;
                    return responceCommunityEmergencyContacts;
                }
            }

            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityImage;
            return responceCommunityEmergencyContacts;
        }

        // POST: api/CommunityImages
        [ResponseType(typeof(CommunityImage))]
        public async Task<Response<CommunityImage>> PostCommunityImage(CommunityImage communityImage)
        {
            Response<CommunityImage> responceCommunityEmergencyContacts = new Response<CommunityImage>();
            if (!ModelState.IsValid)
            {
                responceCommunityEmergencyContacts.status = "Failure";
                responceCommunityEmergencyContacts.model = null;
                return responceCommunityEmergencyContacts;
            }

            db.CommunityImages.Add(communityImage);
            await db.SaveChangesAsync();

            responceCommunityEmergencyContacts.status = "Success";
            responceCommunityEmergencyContacts.model = communityImage;
            return responceCommunityEmergencyContacts;
        }

        // DELETE: api/CommunityImages/5
        [ResponseType(typeof(CommunityImage))]
        public async Task<IHttpActionResult> DeleteCommunityImage(int id)
        {
            CommunityImage communityImage = await db.CommunityImages.FindAsync(id);
            if (communityImage == null)
            {
                return NotFound();
            }

            db.CommunityImages.Remove(communityImage);
            await db.SaveChangesAsync();

            return Ok(communityImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityImageExists(int id)
        {
            return db.CommunityImages.Count(e => e.id == id) > 0;
        }
    }
}