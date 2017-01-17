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
    public class CommunityServicesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/CommunityServices
        public IQueryable<CommunityService> GetCommunityServices()
        {
            return db.CommunityServices;
        }

        // GET: api/CommunityServices/5
        [ResponseType(typeof(CommunityService))]
        public async Task<Response<CommunityService>> GetCommunityService(int id)
        {
            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
           
            CommunityService communityService = await db.CommunityServices.FindAsync(id);
            if (communityService == null)
            {
                responceCommunityService.status = "No Community Service Found";
                responceCommunityService.model = null;
                return responceCommunityService;
            }

            responceCommunityService.status = "Success";
            responceCommunityService.model = communityService;
            return responceCommunityService;
        }

        // GET: api/CommunityServices/5
        [ResponseType(typeof(CommunityService))]
        public async Task<Response<List<CommunityService>>> GetServicebyCommunityID(int communityID)
        {
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            List<CommunityService> communityServiceList = new List<CommunityService>();
            communityServiceList = (from l in db.CommunityServices
                                    where l.communityID == communityID
                                    select l).ToList();
            if (communityServiceList == null)
            {
                responceCommunityService.status = "Failed: No Community Service";
                responceCommunityService.model = null;
                return responceCommunityService;
            }
            responceCommunityService.status = "Success";
            responceCommunityService.model = communityServiceList;
            return responceCommunityService;
        }

        public async Task<Response<List<CommunityServiceWithServiceIdDTO>>> GetServicebyWithServiceID(int communityID, int dumbWeb)
        {
            Response<List<CommunityServiceWithServiceIdDTO>> responceCommunityServiceWithServiceIdDTO = new Response<List<CommunityServiceWithServiceIdDTO>>();
            List<CommunityService> communityServiceList = new List<CommunityService>();
            List<CommunityServiceWithServiceIdDTO> communityServiceWithServiceIdDTOList = new List<CommunityServiceWithServiceIdDTO>();
            communityServiceList = (from l in db.CommunityServices
                                    where l.communityID == communityID
                                    select l).ToList();
            if (communityServiceList == null)
            {
                responceCommunityServiceWithServiceIdDTO.status = "Failed: No Community Service";
                responceCommunityServiceWithServiceIdDTO.model = null;
                return responceCommunityServiceWithServiceIdDTO;
            }
            int count = 0;
            foreach (var item in communityServiceList)
            {
                CommunityServiceWithServiceIdDTO communityServiceWithServiceIdDTO = new CommunityServiceWithServiceIdDTO();
                string name = item.serviceName;
                int serviceID = (from l in db.Services
                                        where l.description == name
                                        select l.serviceID).FirstOrDefault();
                if (serviceID != 0)
                {
                    communityServiceWithServiceIdDTO.serviceID = serviceID;
                    communityServiceWithServiceIdDTO.communityService = item;
                    communityServiceWithServiceIdDTOList.Add(communityServiceWithServiceIdDTO);
                }
                else
                {
                    count--;
                    communityServiceWithServiceIdDTO.serviceID = count;
                    communityServiceWithServiceIdDTO.communityService = item;
                    communityServiceWithServiceIdDTOList.Add(communityServiceWithServiceIdDTO);
                }
          

            }
            responceCommunityServiceWithServiceIdDTO.status = "Success";
            responceCommunityServiceWithServiceIdDTO.model = communityServiceWithServiceIdDTOList;
            return responceCommunityServiceWithServiceIdDTO;
        }
        public Response<List<CommunityService>> GetAllCommunityServices(int communityID,int dumbWeb)
        {
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
          
            List<CommunityService> communityServiceList = new List<CommunityService>();
            communityServiceList = (from l in db.CommunityServices
                                    where l.communityID == communityID
                                    select l).ToList();
            if (communityServiceList == null)
            {
                responceCommunityService.status = "Failed: No Community Service";
                responceCommunityService.model = null;
                return responceCommunityService;
            }
        
            responceCommunityService.status = "Success";
            responceCommunityService.model = communityServiceList;
            return responceCommunityService;
        }
        public async Task<Response<List<ServiceNameWithStaffDTO>>> GetServicebyCommunityID(int communityID,int DummyService)
        {
            Response<List<ServiceNameWithStaffDTO>> responceServiceNameWithStaffDTO = new Response<List<ServiceNameWithStaffDTO>>();
            List<Service> serviceList = new List<Service>();
            List<ServiceNameWithStaffDTO> serviceNameWithStaffDTOList = new List<ServiceNameWithStaffDTO>();
            Service service = new Service();
            List<CommunityService> communityServiceList = new List<CommunityService>();
            communityServiceList = (from l in db.CommunityServices
                                    where l.communityID == communityID
                                    select l).ToList();
            foreach (var item in communityServiceList)
            {
                ServiceNameWithStaffDTO serviceNameWithStaffDTO = new ServiceNameWithStaffDTO();
                serviceNameWithStaffDTO.service = await (db.CommunityServices.Where(x => x.communityServiceID == item.communityServiceID)).FirstOrDefaultAsync();

                ServiceStaffsController serviceStaffsController = new ServiceStaffsController();
                Response<List<ServiceStaff>> responseServiceStaff = new Response<List<ServiceStaff>>();

                responseServiceStaff = await serviceStaffsController.GetServiceStaffbyService(serviceNameWithStaffDTO.service.communityServiceID, communityID);
                serviceNameWithStaffDTO.serviceStaff = responseServiceStaff.model;
                serviceNameWithStaffDTOList.Add(serviceNameWithStaffDTO);

            }
            if (communityServiceList == null)
            {
                responceServiceNameWithStaffDTO.status = "Failed: No Community Service";
                responceServiceNameWithStaffDTO.model = null;
                return responceServiceNameWithStaffDTO;
            }
            foreach (var item in communityServiceList)
            {
                service = await (from l in db.Services
                                 where l.serviceID == item.communityServiceID
                                 select l).FirstOrDefaultAsync();
                serviceList.Add(service);
            }
            responceServiceNameWithStaffDTO.status = "Success";
            responceServiceNameWithStaffDTO.model = serviceNameWithStaffDTOList;
            return responceServiceNameWithStaffDTO;
        }
        public async Task<Response<List<Service>>> GetRemainingServiceOfCommunityID(int communityID,int? RemainingDummy)
        {
            Response<List<Service>> responceCommunityService = new Response<List<Service>>();
            List<Service> unSelectedServiceList = new List<Service>();
            Service service = new Service();
            List<CommunityService> communityServiceList = new List<CommunityService>();
            communityServiceList = (from l in db.CommunityServices
                                    where l.communityID == communityID
                                    select l).ToList();
            
            if (communityServiceList.Count == 0)
            {

               List<Service> AllServiceList = new List<Service>();
                AllServiceList = (from l in db.Services
                                 select l).ToList();
              
              
                responceCommunityService.status = "Success";
                responceCommunityService.model = AllServiceList;
                return responceCommunityService;
            }
            else {
                List<int> AllServiceIDList = new List<int>();
                List<int> SelectedServiceIDList = new List<int>();
                List<CommunityService> AllServiceList= new List<CommunityService>();
                List<CommunityService> SelectedServiceList = new List<CommunityService>();

                AllServiceIDList = (from l in db.Services
                                           select l.serviceID).ToList();
               // foreach (var item in AllServiceList)
               // {
               //     string name=item.serviceName;
               //int serviceID = (from l in db.Services
               //                     where l.description==name
               //                   select l.serviceID).FirstOrDefault();
               //if (serviceID !=0)
               //{
               //    AllServiceIDList.Add(serviceID);
               //}
             
               // }
                SelectedServiceList = (from l in db.CommunityServices
                                        where l.communityID == communityID
                                         select l).ToList();

                foreach (var item in SelectedServiceList)
                {
                    string name = item.serviceName;
                    int serviceID = (from l in db.Services
                                     where l.description == name
                                     select l.serviceID).FirstOrDefault();
                    if (serviceID != 0)
                    {
                        SelectedServiceIDList.Add(serviceID);
                    }

                }
                var unSelectedList = AllServiceIDList.Except(SelectedServiceIDList).ToList();
            foreach (var item in unSelectedList)
            {
                service = await (from l in db.Services
                                 where l.serviceID == item
                                 select l).FirstOrDefaultAsync();
                unSelectedServiceList.Add(service);
            }
            responceCommunityService.status = "Success";
            responceCommunityService.model = unSelectedServiceList;
            return responceCommunityService;
            }
        }
        // PUT: api/CommunityServices/5
        [ResponseType(typeof(void))]
        public async Task<Response<CommunityService>> PutCommunityService(int id, CommunityService communityService)
        {
            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
            if (!ModelState.IsValid)
            {
                responceCommunityService.status = "Failuer";
                responceCommunityService.model = null;
                return responceCommunityService;
            }

            if (id != communityService.communityServiceID)
            {
                responceCommunityService.status = "Failed: id did not match";
                responceCommunityService.model = null;
                return responceCommunityService;
            }

            db.Entry(communityService).State = EntityState.Modified;

            try
            {
              
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityServiceExists(id))
                {
                    responceCommunityService.status = "Failed: id did not exist";
                    responceCommunityService.model = null;
                    return responceCommunityService;
                }
                else
                {
                    throw;
                }
            }

            responceCommunityService.status = "Success";
            responceCommunityService.model = communityService;
            return responceCommunityService;
        }

        // POST: api/CommunityServices
        [ResponseType(typeof(CommunityService))]
        public async Task<Response<CommunityService>> PostCommunityService(CommunityService communityService)
        {
            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
            if (!ModelState.IsValid)
            {
                responceCommunityService.status = "Failed";
                responceCommunityService.model = null;
                return responceCommunityService;
            }
            string name = communityService.serviceName;
            int serviceID = (from l in db.Services
                             where l.description == name
                             select l.serviceID).FirstOrDefault();
            if (serviceID != 0)
            {
                communityService.isPredefined = true;
            }
            else
            {
                communityService.isPredefined = false;
            }
            db.CommunityServices.Add(communityService);
            await db.SaveChangesAsync();

            responceCommunityService.status = "Success";
            responceCommunityService.model = communityService;
            return responceCommunityService;
        }

        // DELETE: api/CommunityServices/5
        [ResponseType(typeof(CommunityService))]
        public async Task<Response<CommunityService>> DeleteCommunityService(int communityServiceId)
        {
            Response<CommunityService> responseCommunityService = new Response<CommunityService>();
            CommunityService communityService = await db.CommunityServices.FindAsync(communityServiceId);

            if (communityService != null)
            {
                List<ServiceStaff> ServiceStaffList = new List<ServiceStaff>();
                ServiceStaffList = (from l in db.ServiceStaffs
                                    where l.communityServiceID == communityServiceId
                                    select l).ToList();
                if (ServiceStaffList != null)
                {
                    foreach (var item in ServiceStaffList)
                    {
                        ServiceStaff serviceStaff = await db.ServiceStaffs.FindAsync(item.id);
                        db.ServiceStaffs.Remove(serviceStaff);


                    }

                    await db.SaveChangesAsync();
                }
                db.CommunityServices.Remove(communityService);
                await db.SaveChangesAsync();

                responseCommunityService.status = "Success";
                responseCommunityService.model = communityService;
                return responseCommunityService;
            }
            else
            {
                responseCommunityService.status = "Failed: No Service Found";
                responseCommunityService.model = null;
                return responseCommunityService;
            }
           
            




        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityServiceExists(int id)
        {
            return db.CommunityServices.Count(e => e.communityServiceID == id) > 0;
        }
        private bool CommunityService(int communityid)
        {
            return db.CommunityServices.Count(e => e.communityID == communityid) > 0;
        }
    }
}