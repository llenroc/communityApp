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
    public class CommunityiReportsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunityiReports
        public IQueryable<CommunityiReports> GetCommunityiReports()
        {
            return db.CommunityiReports;
        }


          public Response<List<CommunityiReports>> GetCommunityiReportsbyCommunityID(int CommunityID)
        {
            Response<List<CommunityiReports>> responCecommunityiReports = new Response<List<CommunityiReports>>();
            List<CommunityiReports> communityiReports = new List<CommunityiReports>();
            communityiReports = db.CommunityiReports.Where(x => x.communityID == CommunityID).Include(x=>x.community).Include(x=>x.community.user).ToList();
            if (communityiReports != null)
            {
                responCecommunityiReports.status = "Success";
                responCecommunityiReports.model = communityiReports;
                return responCecommunityiReports;
            }
            else
            {
                responCecommunityiReports.status = "Failed: No iReports found";
                responCecommunityiReports.model = null;
                return responCecommunityiReports;
            }
      
        }

        // GET: api/CommunityiReports/5
        [ResponseType(typeof(CommunityiReports))]
          public async Task<Response<CommunityiReports>> GetCommunityiReports(int id)
        {
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            CommunityiReports communityiReports = await db.CommunityiReports.FindAsync(id);
            if (communityiReports == null)
            {
                responceCommunityiReports.status = "No iReportCatagory found";
                responceCommunityiReports.model = null;
                return responceCommunityiReports;
            }


            responceCommunityiReports.status = "Success";
            responceCommunityiReports.model = communityiReports;
            return responceCommunityiReports;
        }

        // PUT: api/CommunityiReports/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunityiReports>> PutCommunityiReports(int id, CommunityiReports communityiReports)
        {
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            if (!ModelState.IsValid)
            {
                responceCommunityiReports.status = "failuer";
                responceCommunityiReports.model = null;
                return responceCommunityiReports;
            }

            if (id != communityiReports.CommunityiReportsID)
            {
                responceCommunityiReports.status = "failed: Id did not Match";
                responceCommunityiReports.model = null;
                return responceCommunityiReports;
            }

            db.Entry(communityiReports).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityiReportsExists(id))
                {
                    responceCommunityiReports.status = "failed: Id did not exists";
                    responceCommunityiReports.model = null;
                    return responceCommunityiReports;
                }
                else
                {
                    throw;
                }
            }

            responceCommunityiReports.status = "Success";
            responceCommunityiReports.model = communityiReports;
            return responceCommunityiReports;
        }

        // POST: api/CommunityiReports
        [ResponseType(typeof(CommunityiReports))]
        public async Task<Response<CommunityiReports>> PostCommunityiReports(CommunityiReports communityiReports)
        {
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            if (!ModelState.IsValid)
            {
                responceCommunityiReports.status = "failuer";
                responceCommunityiReports.model = null;
                return responceCommunityiReports;
            }

            db.CommunityiReports.Add(communityiReports);
            await db.SaveChangesAsync();

              responceCommunityiReports.status = "Success";
              responceCommunityiReports.model = communityiReports;
                return responceCommunityiReports;
      
        }

        // DELETE: api/CommunityiReports/5
        [ResponseType(typeof(CommunityiReports))]
        public async Task<Response<CommunityiReports>> DeleteCommunityiReports(int id)
        {
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            CommunityiReports communityiReports = await db.CommunityiReports.FindAsync(id);
            if (communityiReports == null)
            {
                responceCommunityiReports.status = "No iReportCatagory found";
                responceCommunityiReports.model = null;
                return responceCommunityiReports;
            }

            db.CommunityiReports.Remove(communityiReports);
            await db.SaveChangesAsync();
            responceCommunityiReports.status = "Success";
            responceCommunityiReports.model = communityiReports;
            return responceCommunityiReports;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityiReportsExists(int id)
        {
            return db.CommunityiReports.Count(e => e.CommunityiReportsID == id) > 0;
        }
    }
}