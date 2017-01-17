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
using System.Web;

namespace RatiusCommunityApp.Controllers
{
    [Authorize]
    public class ServiceStaffsController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/ServiceStaffs
        public IQueryable<ServiceStaff> GetServiceStaffs()
        {
            return db.ServiceStaffs;
        }

        // GET: api/ServiceStaffs/5
        [ResponseType(typeof(ServiceStaff))]
        public async Task<Response<ServiceStaff>> GetServiceStaff(int id)
        {
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaff serviceStaff = new ServiceStaff();
            serviceStaff = (from l in db.ServiceStaffs
                                          where l.id == id
                                          select l).FirstOrDefault();
            if (serviceStaff == null)
            {

                responseServiceStaff.status = "Failed: No Service Staff";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            responseServiceStaff.status = "Success";
            responseServiceStaff.model = serviceStaff;
            return responseServiceStaff;
        }

        // GET: api/ServiceStaffs/5
        [ResponseType(typeof(Response<List<ServiceStaff>>))]
        public async Task<Response<List<ServiceStaff>>> GetServiceStaffbyService(int communityServiceID,int communityID)
        {
            Response<List<ServiceStaff>> responseServiceStaff = new Response<List<ServiceStaff>>();
            List<ServiceStaff> communityContactsbyService = new List<ServiceStaff>();
            communityContactsbyService = (from l in db.ServiceStaffs
                                          where l.communityServiceID == communityServiceID && l.communityID==communityID
                                          select l).ToList();
            if (communityContactsbyService == null)
            {

                responseServiceStaff.status = "Failed: No Service Staff";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            responseServiceStaff.status = "Success";
            responseServiceStaff.model = communityContactsbyService;
            return responseServiceStaff;
        }
        
        
        
        // PUT: api/ServiceStaffs/5
        [ResponseType(typeof(Response<ServiceStaff>))]
        public async Task<Response<ServiceStaff>> PutServiceStaff(int id, ServiceStaff serviceStaff)
        {
            var httpRequest = HttpContext.Current.Request;


            //if (httpRequest.Files.Count > 0)
            //{
            //    var docfiles = new List<string>();
            
            //    foreach (string file in httpRequest.Files)
            //    {
            //        var postedFile = httpRequest.Files[file];
            //        if (postedFile.ContentLength == 0)
            //        {
            //            Response<ServiceStaff> responseService = new Response<ServiceStaff>();
            //            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            //            responseService = await serviceStaffController.GetServiceStaff(id);
            //            serviceStaff.image = responseService.model.image;
            //        }
            //        else
            //        {
            //            imageForBlob imageForBlob = new imageForBlob();
            //            string blobImageURL = imageForBlob.ConvertImageForBlob();
            //            serviceStaff.image = blobImageURL;
            //        }

            //    }
               

            //}
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            if (!ModelState.IsValid)
            {
                responseServiceStaff.status = "Failure";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }
            if (id != serviceStaff.id)
            {
                responseServiceStaff.status = "Failed: ID Did Not Match";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            if (!ServiceStaffExists(id))
            {
                responseServiceStaff.status = "Failed: User Does Not Exist";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            db.Entry(serviceStaff).State = EntityState.Modified;
            await db.SaveChangesAsync();

            responseServiceStaff.status = "Success";
            responseServiceStaff.model = serviceStaff;
     
            
       
            return responseServiceStaff;
        }
        

        // POST: api/ServiceStaffs
        [ResponseType(typeof(Response<ServiceStaff>))]
        public async Task<Response<ServiceStaff>> PostServiceStaff(ServiceStaff serviceStaff)
        {

            var httpRequest = HttpContext.Current.Request;
         

                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile.ContentLength != 0)
                        {
                            imageForBlob imageForBlob = new imageForBlob();
                            string blobImageURL = imageForBlob.ConvertImageForBlob();
                            serviceStaff.image = blobImageURL;
                        }

                        else
                        {
                            serviceStaff.image = "../images/NoUserImage.png";
                        }
                    }

                }
               
               
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            if (!ModelState.IsValid)
            {
                responseServiceStaff.status = "Failure";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            db.ServiceStaffs.Add(serviceStaff);
            await db.SaveChangesAsync();
            responseServiceStaff.status = "Success";
            responseServiceStaff.model = serviceStaff;
            return responseServiceStaff;
        }

        // DELETE: api/ServiceStaffs/5
        [ResponseType(typeof(ServiceStaff))]
        public async Task<Response<ServiceStaff>> DeleteServiceStaff(int id)
        {
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaff serviceStaff = await db.ServiceStaffs.FindAsync(id);
            if (serviceStaff == null)
            {
                responseServiceStaff.status = "Failure";
                responseServiceStaff.model = null;
                return responseServiceStaff;
            }

            db.ServiceStaffs.Remove(serviceStaff);
            await db.SaveChangesAsync();
            responseServiceStaff.status = "Success";
            responseServiceStaff.model = serviceStaff;
            return responseServiceStaff;
        }


        //public async void DeleteServiceStaffbyCommunityServiceID(int communityServiceId)
        //{
        //    Response<List<ServiceStaff>> responseServiceStaffList = new Response<List<ServiceStaff>>();
        //    List<ServiceStaff> ServiceStaffList = new List<ServiceStaff>();
        //    ServiceStaffList = (from l in db.ServiceStaffs
        //                        where l.communityServiceID == communityServiceId
        //                        select l).ToList();
        //    if (ServiceStaffList == null)
        //    {
        //        responseServiceStaffList.status = "Failure";
        //        responseServiceStaffList.model = null;
        //    }
        //    else
        //    {
        //        foreach (var item in ServiceStaffList)
        //        {
        //            ServiceStaff serviceStaff = await db.ServiceStaffs.FindAsync(item.id);
        //            db.ServiceStaffs.Remove(serviceStaff);


        //        }

        //        await db.SaveChangesAsync();
        //    }
   

           
           
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceStaffExists(int id)
        {
            return db.ServiceStaffs.Count(e => e.id == id) > 0;
        }
    }
}