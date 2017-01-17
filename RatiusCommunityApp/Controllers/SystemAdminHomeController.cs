using RatiusCommunityApp.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace RatiusCommunityApp.Controllers
{
    public class SystemAdminHomeController : Controller
    {
        // GET: SystemAdminHome
        public ActionResult Index()
        {
            return View();
        }

//------------------------------------------------------System Admin Controllers----------------------------------------------------------
        public async Task<ActionResult> Home()
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("index","home");
            }
            Session.Timeout = 1000;
            return View();

        }
//-------------------------------------------------------_SystemAdminNavbarAndSideMenu Controllers---------------------------------------------------------
       
        public ActionResult _SystemAdminNavbarAndSideMenu()
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("index", "home");
            }
            Session.Timeout = 1000;
            UsersController userController = new UsersController();
            tokenDTO<User> userResponse = new tokenDTO<User>();
            userResponse = userController.GetUserforNavBar(Convert.ToInt32(Session["loginUserID"].ToString()), 1);
            CommunityNavbarAndSideMenuDTO communitysidebar = new CommunityNavbarAndSideMenuDTO();
            communitysidebar.tokenUser = userResponse;
            //CommunitiesController communityController = new CommunitiesController();
            //Response<Community> responceCommunity = new Response<Community>();
            //responceCommunity =communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            //communitysidebar.community = responceCommunity.model;
            return PartialView(communitysidebar);
        }

//-------------------------------------------------------Services Controllers------------------------------------------------------------


        public async Task<ActionResult> Services(int? communityId)
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
              CommunitiesController communitiesController = new CommunitiesController();
                List<Community> communitiesList = new List<Community>();
                communitiesList = await communitiesController.GetAllCommunities();
            if (communityId == null)
            {
              
            
                if (communitiesList.Count != 0)
                {
                    Session["loginCommunityID"] = communitiesList.FirstOrDefault().communityID;
                }
                else
                {
                    Session["loginCommunityID"] = 0;
                }
            }
            else
            {
                Session["loginCommunityID"] = communityId;
            }
           
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            Response<List<Service>> responceCommunityRemainingService = new Response<List<Service>>();
            //comunity Selected Services
            responceCommunityService = await communityServicesController.GetServicebyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            //comunity Remaining Services
            responceCommunityRemainingService = await communityServicesController.GetRemainingServiceOfCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            ServiceDTO serviceDTO = new ServiceDTO();
            serviceDTO.selectedServices = responceCommunityService.model;
            serviceDTO.remainingServices = responceCommunityRemainingService.model;

            DirectoryImagesController directoryImagesController = new DirectoryImagesController();
            Response<List<DirectoryImages>> responceDirectoryImage = new Response<List<DirectoryImages>>();
            responceDirectoryImage = directoryImagesController.GetAllDirectoryImages();
            serviceDTO.directoryImage = responceDirectoryImage.model;

            Response<List<CommunityServiceWithServiceIdDTO>> responceCommunityServiceWithServiceIdDTO = new Response<List<CommunityServiceWithServiceIdDTO>>();
            responceCommunityServiceWithServiceIdDTO = await communityServicesController.GetServicebyWithServiceID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            serviceDTO.CommunityServiceWithServiceIdDTOList = responceCommunityServiceWithServiceIdDTO.model;
            serviceDTO.communitieslist = communitiesList;
            serviceDTO.emergencyContactRange = communitiesController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString())).model.emergencyContactRange;
            return View(serviceDTO);
        }

        public async Task<ActionResult> AddCommunityServices(string serviceName, string imageURL)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityService communityService = new CommunityService();
            communityService.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            communityService.icon = imageURL;
            communityService.serviceName = serviceName;
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<CommunityService> responcePostCommunityService = new Response<CommunityService>();
            responcePostCommunityService = await communityServicesController.PostCommunityService(communityService);

            //Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            //responceCommunityService =await communityServicesController.GetServicebyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            return Json(new { CommunityServiceID = responcePostCommunityService.model.communityServiceID });
        }

        public async Task<ActionResult> EditCommunityServices(string serviceName, string imageURL, int communityServiceID)
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<CommunityService> responcePostCommunityService = new Response<CommunityService>();
            responcePostCommunityService = await communityServicesController.GetCommunityService(communityServiceID);
            CommunityService communityService = new CommunityService();
            communityService = responcePostCommunityService.model;
            communityService.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            communityService.icon = imageURL;
            communityService.serviceName = serviceName;

            responcePostCommunityService = await communityServicesController.PutCommunityService(communityServiceID, communityService);

            //Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            //responceCommunityService =await communityServicesController.GetServicebyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            return Json(new { CommunityServiceID = responcePostCommunityService.model.communityServiceID });
        }
        public async Task<ActionResult> RemoveCommunityServices(int CommunityServiceID)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<CommunityService> responcePostCommunityService = new Response<CommunityService>();
            await communityServicesController.DeleteCommunityService(CommunityServiceID);

            //Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            //responceCommunityService =await communityServicesController.GetServicebyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            return Json(new { Success = "success" });
        }

//-------------------------------------------------------Services Staff Controllers---------------------------------------------------------



        [HttpPost]
        public async Task<ActionResult> getServiceStaffModal(string CommunityServiceName, int CommunityServiceId)
        {
            selectedServiceName = CommunityServiceName;
            serviceID = CommunityServiceId;

            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            //ServicesController serviceController = new ServicesController();
            //Response<Service> responceService = new Response<Service>();
            //responceService = await serviceController.GetServicebyName(serviceName);

            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
            CommunityServicesController communityServicesController = new CommunityServicesController();
            responceCommunityService = await communityServicesController.GetCommunityService(serviceID);


            Response<List<ServiceStaff>> responceServiceStaff = new Response<List<ServiceStaff>>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();


            responceServiceStaff = await serviceStaffController.GetServiceStaffbyService(serviceID, Convert.ToInt32(Session["loginCommunityID"].ToString()));
            ServiceStaffDTO serviceStaffDTO = new ServiceStaffDTO();
            serviceStaffDTO.serviceStaffList = responceServiceStaff.model;
            serviceStaffDTO.serviceName = responceCommunityService.model.serviceName;

            serviceID = responceCommunityService.model.communityServiceID;


            //return to action with parameter
            return Json(new { ResponseServiceStaff = responceServiceStaff.model });
        }
        public async Task<ActionResult> AddServicesStaff(FormCollection data)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ServiceStaffsController serviceStaffsController = new ServiceStaffsController();
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaff serviceStaff = new ServiceStaff();
            serviceStaff.communityServiceID = Convert.ToInt32(data["communityServiceID"].ToString());
            serviceStaff.name = data["Name"].ToString();
            serviceStaff.staffRole = data["Designation"].ToString();
            serviceStaff.contactNumber = data["Contact"].ToString();
            serviceStaff.emailID = data["EmailID"].ToString();
            serviceStaff.isActive = true;
            serviceStaff.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());

            if (Request.Files["Featurefile"] != null)
            {
                var postedFile = Request.Files["staffImage"];
                if (postedFile.ContentLength != 0)
                {
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    serviceStaff.image = blobImageURL;
                }

            }




            responseServiceStaff = await serviceStaffsController.PostServiceStaff(serviceStaff);




            //return to action with parameter
            return Json(new { ResponseServiceStaff = responseServiceStaff.model });
        }

        public async Task<ActionResult> EditServicesStaff(FormCollection data)
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ServiceStaff serviceStaff = new ServiceStaff();
            serviceStaff.id = Convert.ToInt32(data["id"].ToString());
            ServiceStaffsController serviceStaffsController = new ServiceStaffsController();
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            responseServiceStaff = await serviceStaffsController.GetServiceStaff(serviceStaff.id);

            serviceStaff = responseServiceStaff.model;

            serviceStaff.communityServiceID = Convert.ToInt32(data["communityServiceID"].ToString());
            serviceStaff.name = data["Name"].ToString();
            serviceStaff.staffRole = data["Designation"].ToString();
            serviceStaff.contactNumber = data["Contact"].ToString();
            serviceStaff.emailID = data["EmailID"].ToString();
            serviceStaff.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());

            if (Request.Files["Featurefile"] != null)
            {
                var postedFile = Request.Files["staffImage"];
                if (postedFile.ContentLength != 0)
                {
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    serviceStaff.image = blobImageURL;
                }

            }




            responseServiceStaff = await serviceStaffsController.PutServiceStaff(serviceStaff.id, serviceStaff);




            //return to action with parameter
            return Json(new { ResponseServiceStaff = responseServiceStaff.model });
        }












        static int serviceID;
        static string selectedServiceName;
        public async Task<ActionResult> ServiceStaff(string serviceName)
        {
            serviceID = Convert.ToInt32(serviceName);
            selectedServiceName = serviceName;
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            //ServicesController serviceController = new ServicesController();
            //Response<Service> responceService = new Response<Service>();
            //responceService = await serviceController.GetServicebyName(serviceName);

            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
            CommunityServicesController communityServicesController = new CommunityServicesController();
            responceCommunityService = await communityServicesController.GetCommunityService(serviceID);


            Response<List<ServiceStaff>> responceServiceStaff = new Response<List<ServiceStaff>>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();


            responceServiceStaff = await serviceStaffController.GetServiceStaffbyService(serviceID, Convert.ToInt32(Session["loginCommunityID"].ToString()));
            ServiceStaffDTO serviceStaffDTO = new ServiceStaffDTO();
            serviceStaffDTO.serviceStaffList = responceServiceStaff.model;
            serviceStaffDTO.serviceName = responceCommunityService.model.serviceName;

            serviceID = responceCommunityService.model.communityServiceID;
            return View(serviceStaffDTO);
        }





        public async Task<ActionResult> AddServicesStaffFromModel(ServiceStaff serviceStaff)
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            string contact = serviceStaff.contactNumber;
            //if ( serviceStaff.contactNumber != null)
            //{
            //    string[] contactArrayPlus = contact.Split('+');
            //    contact = contactArrayPlus[1];
            //}


            //string[] contactArray = contact.Split('-');
            //contact = contactArrayPlus[0] + contactArrayPlus[1];
            serviceStaff.contactNumber = contact;
            serviceStaff.isActive = true;
            serviceStaff.communityServiceID = serviceID;
            serviceStaff.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());

            responseServiceStaff = await serviceStaffController.PostServiceStaff(serviceStaff);




            //return to action with parameter
            return RedirectToAction("ServiceStaff", new { serviceName = selectedServiceName });
        }







        public async Task<ActionResult> EditServicesStaffFromModel(ServiceStaff serviceStaff)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            string contact = serviceStaff.contactNumber;
            //if (serviceStaff.contactNumber != null)
            //{
            //    string[] contactArrayPlus = contact.Split('+');
            //    contact = contactArrayPlus[1];
            //}

            //string[] contactArray = contact.Split('-');
            //contact = contactArrayPlus[0] + contactArrayPlus[1];


            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            responseServiceStaff = await serviceStaffController.GetServiceStaff(serviceStaff.id);
            //responseServiceStaff =await serviceStaffController.GetServiceStaff(serviceStaff.id);
            responseServiceStaff.model.contactNumber = contact;
            var httpRequest = System.Web.HttpContext.Current.Request;


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
                        responseServiceStaff = await serviceStaffController.GetServiceStaff(serviceStaff.id);
                        serviceStaff.image = responseServiceStaff.model.image;
                    }


                }

            }
            else
            {
                responseServiceStaff = await serviceStaffController.GetServiceStaff(serviceStaff.id);
                serviceStaff.image = responseServiceStaff.model.image;
            }
            serviceStaff.communityServiceID = serviceID;
            serviceStaff.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());


            responseServiceStaff.model.image = serviceStaff.image;
            responseServiceStaff.model.name = serviceStaff.name;
            responseServiceStaff.model.staffRole = serviceStaff.staffRole;
            responseServiceStaff.model.isActive = serviceStaff.isActive;
            responseServiceStaff.model.emailID = serviceStaff.emailID;
            responseServiceStaff = await serviceStaffController.PutServiceStaff(serviceStaff.id, responseServiceStaff.model);





            //return to action with parameter
            return RedirectToAction("ServiceStaff", new { serviceName = selectedServiceName });

        }
        public async Task<ActionResult> DeleteServicesStaff(int CommunityServiceID)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            responseServiceStaff = await serviceStaffController.DeleteServiceStaff(CommunityServiceID);
            responseServiceStaff = null;
            return Json(new { Success = "sucess" });
        }


        public async Task<ActionResult> DeleteServicesStaffwithMemberID(int StaffMemberID)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }

            Session.Timeout = 1000;
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            responseServiceStaff = await serviceStaffController.DeleteServiceStaff(StaffMemberID);
            responseServiceStaff = null;
            return Json(new { Success = "sucess" });
        }








        public async Task<ActionResult> ChangeServiceStaffStatus(int staffID, bool isBlocked)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            responseServiceStaff = await serviceStaffController.GetServiceStaff(staffID);
            if (isBlocked == true)
            {
                responseServiceStaff.model.isActive = false;
            }
            else
            {
                responseServiceStaff.model.isActive = true;
            }

            responseServiceStaff = await serviceStaffController.PutServiceStaff(staffID, responseServiceStaff.model);

            return Json(new { success = "Success" });
        }
//-------------------------------------------------------EmergencyContactRange Controllers---------------------------------------------------------
        public async Task<ActionResult> UpdateEmergencyContactRange(int EmergencyContactRange)
        {
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            CommunitiesController communitiesController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = communitiesController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            communityResponse.model.emergencyContactRange = EmergencyContactRange;
            communityResponse = await communitiesController.PutCommunity(communityResponse.model.communityID, communityResponse.model);
            return Json(new { Success = communityResponse.status });
        }
//--------------------------------------------Change SystemAdmin Settings Controllers-----------------------------------------------------
        public async Task<ActionResult> ChangeSystemAdminImage(FormCollection data)
        {
            int userID = Convert.ToInt32(Session["loginUserID"].ToString());
            string responceMessage = "Success";
            string imageString = null;
            UsersController userController = new UsersController();
            Response<User> userResponse = new Response<User>();
            tokenDTO<User> userTokenResponce = new tokenDTO<User>();
            userTokenResponce = await userController.GetUserbyID(userID);
            if (Request.Files["files"] != null)
            {
                var postedFile = Request.Files["files"];
                if (postedFile.ContentLength != 0)
                {

                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();

                    userTokenResponce.model.image = blobImageURL;
                    userResponse = await userController.PutUser(userTokenResponce.model.emailID, userTokenResponce.model);
                    if (userResponse.status != "Success")
                    {
                        responceMessage = "Failed To Update";
                        return Json(new { ResponceMessage = responceMessage });
                    }
                    imageString = blobImageURL;
                }
                else
                {
                    imageString = userTokenResponce.model.image;
                }

            }
            return Json(new { ResponceMessage = responceMessage, ImageString = imageString });
        }
        public async Task<ActionResult> ChangeSystemAdminPassword(FormCollection data)
        {
            string responceMessage = "Success";
            //string imageString = null;
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            string oldpassword = data["oldpass"].ToString();
            string newpassword = data["newpass"].ToString();
            string emailID = data["emailID"].ToString();
            //string communitySecretCode = data["communitySecretCode"].ToString();
            UsersController userController = new UsersController();
            tokenDTO<User> userTokenResponce = new tokenDTO<User>();
            Response<User> userResponse = new Response<User>();
            userTokenResponce = await userController.GetUserbyID(Convert.ToInt32(Session["loginUserID"].ToString()));
            userResponse.model = userTokenResponce.model;
            if (userResponse.model.password == oldpassword)
            {
                if (userTokenResponce.model.emailID != emailID)
                {
                    userResponse.model.emailID = emailID;
                    userResponse = await userController.PutUser(userResponse.model.emailID, userResponse.model);
                    responceMessage = "E-mail Update successfully";
                }
                else
                {
                    responceMessage = "E-mail id is alread updated.";
                }
            }

            else
            {
                responceMessage = "Please Enter Correct Old Password";
            }







            //communityResponse.model.secretCode = communitySecretCode;
            //communityResponse = await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model);
            if (newpassword != "")
            {
                if (userResponse.model.password == oldpassword)
                {

                    userResponse.model.password = newpassword;

                    userResponse = await userController.PutUser(userResponse.model.emailID, userResponse.model);
                    if (userResponse.status != "Success")
                    {
                        responceMessage = "Failed To Update";
                        return Json(new { ResponceMessage = responceMessage });
                    }
                    responceMessage = "Successfully update";

                    //if (Request.Files["files"] != null)
                    //{
                    //    var postedFile = Request.Files["files"];
                    //    if (postedFile.ContentLength != 0)
                    //    {

                    //        imageForBlob imageForBlob = new imageForBlob();
                    //        string blobImageURL = imageForBlob.ConvertImageForBlob();
                    //        userResponse.model.image = blobImageURL;

                    //        imageString = blobImageURL;
                    //        userResponse = await userController.PutUser(userResponse.model.emailID, userResponse.model);
                    //        if (userResponse.status != "Success")
                    //        {
                    //            responceMessage = "Failed To Update";
                    //            return Json(new { ResponceMessage = responceMessage});
                    //        }
                    //    }

                    //}

                }
                else
                {
                    responceMessage = "Failed to update";
                }
            }

            //return Json(new { ResponceMessage = responceMessage, ImageString = imageString });
            return Json(new { ResponceMessage = responceMessage });
        }

//--------------------------------------------SMS Test Feature Controller-----------------------------------------------------

        public ActionResult SMSTestFeature()
        {
            return View();
        }
        public ActionResult SendSms(string number1, string number2, string number3, string SMSmessage)
        {
            string Status = "Success";
            if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("index", "home");
            }
            if (number1 != "")
            {
            using (var wb = new WebClient())
            {
              
                    //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= "+SMSmessage+" %0A From : " + "United Neighbourhoods" + " %0A Visit : " + "http://www.unitedneighbourhoods.com/" + " %0A Contact Us : " + "support@unitedneighbourhoods.com" + "");
                    var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : United Neighbourhoods %0A ADDRESS : Outlier Intel-ligence Sdn Bhd, A-7-2 Vista Kiara Jalan Kiara 3 Mont Kiara Kuala Lumpur Malaysia %0A Contact : 123456789");    
                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactNumber1 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + ""); 
                    if (response == null)
                    {
                        Status = "fail";
                    }

            }
            }
            if (number2 != "")
            {
            using (var wb = new WebClient())
            {
                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : United Neighbourhoods %0A ADDRESS : Outlier Intel-ligence Sdn Bhd, A-7-2 Vista Kiara Jalan Kiara 3 Mont Kiara Kuala Lumpur Malaysia %0A Contact : 123456789");    

                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number2 + "&senderid=onewaysms&languagetype=1&message= " + SMSmessage + " %0A From : " + "United Neighbourhoods" + " %0A Visit : " + "http://www.unitedneighbourhoods.com/" + " %0A Contact Us : " + "support@unitedneighbourhoods.com" + "");
                //    //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactNumber1 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + ""); 
                if (response == null)
                {
                    Status = "fail";
                }

            }
            }
            if (number3 != "")
            {
            using (var wb = new WebClient())
            {
                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : United Neighbourhoods %0A ADDRESS : Outlier Intel-ligence Sdn Bhd, A-7-2 Vista Kiara Jalan Kiara 3 Mont Kiara Kuala Lumpur Malaysia %0A Contact : 123456789");    

                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number3 + "&senderid=onewaysms&languagetype=1&message= " + SMSmessage + " %0A From : " + "United Neighbourhoods" + " %0A Visit : " + "http://www.unitedneighbourhoods.com/" + " %0A Contact Us : " + "support@unitedneighbourhoods.com" + "");
                //    //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactNumber1 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + ""); 
                if (response == null)
                {
                    Status = "fail";
                }

            }
            }
            return Json(new { status = Status });
        }


//--------------------------------------------Add Community Controller-----------------------------------------------------
        public async Task<ActionResult> AddCommunity()
        {
            CommunitiesController communityController = new CommunitiesController();
            List<Community> listcommunity = new List<Community>();
            listcommunity = await communityController.GetAllCommunities();
            listcommunity = listcommunity.OrderByDescending(x => x.communityID).ToList();
            return View(listcommunity);
        }

//--------------------------------------------Community Settings Controller-----------------------------------------------------
        public async Task<ActionResult> CommunitySettings(int? communityId)
        {
           if (Session["loginUserID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunitySettingsDTO communitySettingsDTO = new CommunitySettingsDTO();
              CommunitiesController communitiesController = new CommunitiesController();
                List<Community> communitiesList = new List<Community>();
                communitiesList = await communitiesController.GetAllCommunities();
                communitySettingsDTO.communityList = communitiesList;
            if (communityId == null)
            {
              
            
                if (communitiesList.Count != 0)
                {
                    Session["loginCommunityID"] = communitiesList.FirstOrDefault().communityID;
                }
                else
                {
                    Session["loginCommunityID"] = 0;
                }
            }
            else
            {
                Session["loginCommunityID"] = communityId;
            }

            
            
            //SubCommunities of Selected Communities
            SubCommunitiesController subCommunitiesController = new SubCommunitiesController();
            List<SubCommunity> subCommunitiesList = new List<SubCommunity>();
            subCommunitiesList =await subCommunitiesController.GetSubCommunitiesByCommunityId(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            communitySettingsDTO.subCommunityList = subCommunitiesList;

            //Street Floor of Community
            CommunityStreetFloorsController communityStreetFloorsController = new CommunityStreetFloorsController();
            List<CommunityStreetFloor> communityStreetFloorList = new List<CommunityStreetFloor>();
            communityStreetFloorList =await communityStreetFloorsController.GetCommunityStreetFloorByCommunityId(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            communitySettingsDTO.allStreetFloors = communityStreetFloorList;
            
            SubCommunitySelectedStreetFloorsController subCommunitySelectedStreetFloorsController = new SubCommunitySelectedStreetFloorsController();
            List<int> subCommunitySelectedStreetFloorIDsList = new List<int>();
            subCommunitySelectedStreetFloorIDsList =subCommunitySelectedStreetFloorsController.GetALLSubCommunitySelectedStreetFloorsID();
            communitySettingsDTO.selectedStreetFloorsIDs = subCommunitySelectedStreetFloorIDsList;

            return View(communitySettingsDTO);
        }




        public async Task<ActionResult> CreateSubCommunity(string subCommunityName, List<String> selectedStreetFloorIDArray,int CommunityId)
        {
            SubCommunitiesController subCommunitiesController = new SubCommunitiesController();
             Response<SubCommunity> responseSubCommunity = new Response<SubCommunity>();
            SubCommunity subCommunity =new SubCommunity();
            subCommunity.communityID=CommunityId;
            subCommunity.name=subCommunityName;
            responseSubCommunity = await subCommunitiesController.PostSubCommunity(subCommunity);
         





            SubCommunitySelectedStreetFloorsController subCommunitySelectedStreetFloorsController=new SubCommunitySelectedStreetFloorsController();
            if(responseSubCommunity.model!=null){
   foreach(var item in selectedStreetFloorIDArray)
            {
                SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor =new SubCommunitySelectedStreetFloor();
       subCommunitySelectedStreetFloor.communityStreetFloorId=Convert.ToInt32(item);
       subCommunitySelectedStreetFloor.subCommunityId=responseSubCommunity.model.id;

       await subCommunitySelectedStreetFloorsController.PostSubCommunitySelectedStreetFloor(subCommunitySelectedStreetFloor);

            }
           }
         
         
            return Json(new{Status="Success"});
        }


        public async Task<ActionResult> SubCommunityEmergencyContactPartialView(int SubcommunityId)
        {
            SubCommunityEmergencyContactsDTO subCommunityEmergencyContactsDTO = new SubCommunityEmergencyContactsDTO();
            SubCommunityEmergencyContactsController subCommunityEmergencyContactsController = new SubCommunityEmergencyContactsController();
            List<SubCommunityEmergencyContacts> SubCommunityEmergencyContactsList = new List<SubCommunityEmergencyContacts>();
            SubCommunityEmergencyContactsList =await subCommunityEmergencyContactsController.GetSubCommunityEmergencyContactsbySubCommunity(SubcommunityId);
            subCommunityEmergencyContactsDTO.subCommunityEmergencyContactsList = SubCommunityEmergencyContactsList;
            subCommunityEmergencyContactsDTO.subCommunityID = SubcommunityId;




            //Street Floor of Community
            CommunityStreetFloorsController communityStreetFloorsController = new CommunityStreetFloorsController();
            List<CommunityStreetFloor> communityStreetFloorList = new List<CommunityStreetFloor>();
            communityStreetFloorList = await communityStreetFloorsController.GetCommunityStreetFloorByCommunityId(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            subCommunityEmergencyContactsDTO.allStreetFloors = communityStreetFloorList;

            SubCommunitySelectedStreetFloorsController subCommunitySelectedStreetFloorsController = new SubCommunitySelectedStreetFloorsController();
            List<int> AllsubCommunitySelectedStreetFloorIDsList = new List<int>();
            AllsubCommunitySelectedStreetFloorIDsList = subCommunitySelectedStreetFloorsController.GetALLSubCommunitySelectedStreetFloorsID();
            subCommunityEmergencyContactsDTO.selectedStreetFloorsIDs = AllsubCommunitySelectedStreetFloorIDsList;

            //Subcommunity Selected street Floor List
            List<int> subCommunitySelectedStreetFloorIDsList = new List<int>();
            subCommunitySelectedStreetFloorIDsList = subCommunitySelectedStreetFloorsController.GetSubCommunitySelectedStreetFloorsID(SubcommunityId);
            subCommunityEmergencyContactsDTO.subCommunitySelectedStreetFloorsIDs = subCommunitySelectedStreetFloorIDsList;

            return View(subCommunityEmergencyContactsDTO);
        }


        public async Task<ActionResult> AddStreetFloor(int subCommunityId, int streetFloorId)
        {
            SubCommunitySelectedStreetFloorsController subCommunitySelectedStreetFloorsController = new SubCommunitySelectedStreetFloorsController();
            SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor = new SubCommunitySelectedStreetFloor();
            subCommunitySelectedStreetFloor.communityStreetFloorId = Convert.ToInt32(streetFloorId);
            subCommunitySelectedStreetFloor.subCommunityId = subCommunityId;

            await subCommunitySelectedStreetFloorsController.PostSubCommunitySelectedStreetFloor(subCommunitySelectedStreetFloor);
            return Json(new { Status = "Success" });
          }
        public async Task<ActionResult> RemoveStreetFloor(int streetFloorId)
        {

            SubCommunitySelectedStreetFloorsController subCommunitySelectedStreetFloorsController = new SubCommunitySelectedStreetFloorsController();
            SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor = new SubCommunitySelectedStreetFloor();

            subCommunitySelectedStreetFloor =subCommunitySelectedStreetFloorsController.GetSubCommunitySelectedStreetFloorsIDbyCommunityStreetFloorID(streetFloorId);
            if (subCommunitySelectedStreetFloor != null)
            {
                await subCommunitySelectedStreetFloorsController.DeleteSubCommunitySelectedStreetFloor(subCommunitySelectedStreetFloor.id);
            
            }
           
            
            
            return Json(new { Status = "Success" });
        }



        //-------------------------EmergencyContact Controllers----------------------

        public async Task<ActionResult> AddCommunityEmergencyContact(string SelectedWorkinghoursFrom, string SelectedWorkinghoursTo, string EmergencyContact, int EmergencyContactsLength, string EmergencyContactName, int SubCommunityId)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            //CommunitiesController communitiesController = new CommunitiesController();
            //Response<Community> communityResponse = new Response<Community>();
            //communityResponse = communitiesController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));

            SubCommunityEmergencyContactsController subCommunityEmergencyContactsController = new SubCommunityEmergencyContactsController();
            SubCommunityEmergencyContacts subCommunityEmergencyContacts = new SubCommunityEmergencyContacts();
            Response<SubCommunityEmergencyContacts> responceSubCommunityEmergencyContact = new Response<SubCommunityEmergencyContacts>();


            //CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            //CommunityEmergencyContacts communityEmergencyContacts = new CommunityEmergencyContacts();
            //Response<CommunityEmergencyContacts> responceCommunityEmergencyContact = new Response<CommunityEmergencyContacts>();
            string status = "fail";
            //if (EmergencyContactsLength < communityResponse.model.emergencyContactRange)
            //{
                string contact = EmergencyContact;
                subCommunityEmergencyContacts.contact = contact;
                subCommunityEmergencyContacts.name = EmergencyContactName;
                subCommunityEmergencyContacts.workingHourStart = SelectedWorkinghoursFrom;
                subCommunityEmergencyContacts.workingHourEnd = SelectedWorkinghoursTo;
                subCommunityEmergencyContacts.subCommunityId = SubCommunityId;
                responceSubCommunityEmergencyContact = await subCommunityEmergencyContactsController.PostSubCommunityEmergencyContacts(subCommunityEmergencyContacts);
                status = "Success";
            //}


            //if (EmergencyContact != null)
            //{
            //    string[] contactArrayPlus = contact.Split('+');
            //    contact = contactArrayPlus[1];
            //}

                return Json(new { ResponceCommunityEmergencyContact = responceSubCommunityEmergencyContact, Status = status });
        }

        public async Task<ActionResult> UpdateCommunityEmergencyContact(string SelectedWorkinghoursFrom, string SelectedWorkinghoursTo, string EmergencyContact, int Id, string EmergencyContactName)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            string contact = EmergencyContact;
            //if (EmergencyContact != null)
            //{
            //    string[] contactArrayPlus = contact.Split('+');
            //    contact = contactArrayPlus[1];
            //}

            SubCommunityEmergencyContactsController subCommunityEmergencyContactsController = new SubCommunityEmergencyContactsController();
            Response<SubCommunityEmergencyContacts> responceSubCommunityEmergencyContact = new Response<SubCommunityEmergencyContacts>();


            //CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            //Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            responceSubCommunityEmergencyContact = await subCommunityEmergencyContactsController.GetSubCommunityEmergencyContacts(Id);
            responceSubCommunityEmergencyContact.model.contact = contact;
            responceSubCommunityEmergencyContact.model.name = EmergencyContactName;
            responceSubCommunityEmergencyContact.model.workingHourStart = SelectedWorkinghoursFrom;
            responceSubCommunityEmergencyContact.model.workingHourEnd = SelectedWorkinghoursTo;
            responceSubCommunityEmergencyContact = await subCommunityEmergencyContactsController.PutSubCommunityEmergencyContacts(Id, responceSubCommunityEmergencyContact.model);
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> DeleteEmergencyContact(int Id)
        {
            if (Session["loginUserID"] == null )
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            SubCommunityEmergencyContactsController subCommunityEmergencyContactsController = new SubCommunityEmergencyContactsController();
            Response<SubCommunityEmergencyContacts> responceSubCommunityEmergencyContact = new Response<SubCommunityEmergencyContacts>();

            //CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            //Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            await subCommunityEmergencyContactsController.DeleteSubCommunityEmergencyContacts(Id);
            return Json(new { success = "Success" });
        }
    }
}