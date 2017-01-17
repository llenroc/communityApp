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
    public class ServicesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Services
        public IQueryable<Service> GetServices()
        {
            return db.Services;
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> GetService(int id)
        {
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }
        [ResponseType(typeof(Service))]
        public async Task<Response<Service>> GetServicebyName(string serviceName)
        {
            Response<Service> responceService = new Response<Service>();
            Service service = new Service();
            service = await (from l in db.Services
                             where l.description == serviceName
                             select l).FirstOrDefaultAsync();
            if (service == null)
            {
                responceService.status = "Failed: No Service Found";
                responceService.model = null;
                return responceService;
            }
            else
            {
                responceService.status = "Success";
                responceService.model = service;
                return responceService;
            }

        }
        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.serviceID)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = service.serviceID }, service);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> DeleteService(int id)
        {
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            await db.SaveChangesAsync();

            return Ok(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.serviceID == id) > 0;
        }
    }
}