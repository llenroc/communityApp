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
    public class DirectoryImagesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/DirectoryImages
        public Response<List<DirectoryImages>> GetAllDirectoryImages()
        {
            Response<List<DirectoryImages>> responceDirectoryImage = new Response<List<DirectoryImages>>();
            List<DirectoryImages> directoryImages = new List<DirectoryImages>();
            directoryImages = (from l in db.DirectoryImages
                               select l).ToList();
            if (directoryImages == null)
            {
                responceDirectoryImage.status = "failed: No Directory Image Found";
                responceDirectoryImage.model = null;
                return responceDirectoryImage;
            }
            else
            {
                responceDirectoryImage.status = "Success";
                responceDirectoryImage.model = directoryImages;
                return responceDirectoryImage;
            }
        }

        // GET: api/DirectoryImages/5
        [ResponseType(typeof(DirectoryImages))]
        public async Task<IHttpActionResult> GetDirectoryImages(int id)
        {
            DirectoryImages directoryImages = await db.DirectoryImages.FindAsync(id);
            if (directoryImages == null)
            {
                return NotFound();
            }

            return Ok(directoryImages);
        }

        // PUT: api/DirectoryImages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDirectoryImages(int id, DirectoryImages directoryImages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != directoryImages.Id)
            {
                return BadRequest();
            }

            db.Entry(directoryImages).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectoryImagesExists(id))
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

        // POST: api/DirectoryImages
        [ResponseType(typeof(DirectoryImages))]
        public async Task<IHttpActionResult> PostDirectoryImages(DirectoryImages directoryImages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DirectoryImages.Add(directoryImages);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = directoryImages.Id }, directoryImages);
        }

        // DELETE: api/DirectoryImages/5
        [ResponseType(typeof(DirectoryImages))]
        public async Task<IHttpActionResult> DeleteDirectoryImages(int id)
        {
            DirectoryImages directoryImages = await db.DirectoryImages.FindAsync(id);
            if (directoryImages == null)
            {
                return NotFound();
            }

            db.DirectoryImages.Remove(directoryImages);
            await db.SaveChangesAsync();

            return Ok(directoryImages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DirectoryImagesExists(int id)
        {
            return db.DirectoryImages.Count(e => e.Id == id) > 0;
        }
    }
}