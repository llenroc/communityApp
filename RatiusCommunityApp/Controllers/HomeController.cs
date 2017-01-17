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

namespace RatiusCommunityApp.Controllers
{
    public class HomeController : Controller
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();
        public ActionResult Index()
        {
            TempData["error"] = "Null";
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(string name, string communityPassword)
        {
            if (name == "")
            {
                TempData["error"] = "Please Enter Community Name";
                return View();
            }
            else 
            { 
            Session["loginUserID"] = null;
            Session["loginCommunityID"] = null;
            CommunitiesController communityController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = await communityController.GetCommunityByNameAndPassword(name, communityPassword);
            if (communityResponse.status == "Success")
            {
                Session.Timeout = 1000;
                Session["loginUserID"] = communityResponse.model.adminUserID;
                Session["loginCommunityID"] = communityResponse.model.communityID;
                return RedirectToAction("home", "Home");
            }
            else
            {
                //UsersController userController = new UsersController();
                //tokenDTO<SignInDTO1> userResponse = new tokenDTO<SignInDTO1>();
                //userResponse =await userController.GetUser(name, communityPassword);
                //if (userResponse.model != null)
                //{
                //    if (userResponse.model.user.isMainAdmin == true)
                //    {
                //        Session["loginUserID"] = userResponse.model.user.userID;
                //        return RedirectToAction("Home", "SystemAdminHome");
                //    }
                  
                //}
                TempData["error"] = communityResponse.status;
                return View();
            }
        }
        }

        public ActionResult AdminLogin()
        {
            TempData["error"] = "Null";
            ViewBag.Title = "Home Page";

            return View();
        }
         [HttpPost]
        public async Task<ActionResult> AdminLogin(string email, string password)
        {
            if (email == "")
            {
                TempData["error"] = "Please Enter E-mail ";
                return View();
            }
            else
            {
                Session["loginUserID"] = null;
                Session["loginCommunityID"] = null;
                    UsersController userController = new UsersController();
                    tokenDTO<SignInDTO1> userResponse = new tokenDTO<SignInDTO1>();
                    userResponse = await userController.GetUser(email, password);
                    if (userResponse.model != null)
                    {
                        if (userResponse.model.user.isMainAdmin == true)
                        {
                            Session["loginUserID"] = userResponse.model.user.userID;
                            return RedirectToAction("Home", "SystemAdminHome");
                        }
                        else
                        {
                            TempData["error"] = "Please Enter Valid E-mail Address";
                            return View();
                        }
                    }
                    TempData["error"] = userResponse.status;
                    return View();
                
            }
        }
        public ActionResult ForgotPassword()
        {
            TempData["error"] = "Null";
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult SystemAdminForgotPassword()
        {
            TempData["error"] = "Null";
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string emailID,string communityName)
        {
            Session["loginUserID"] = null;
            Session["loginCommunityID"] = null;
            UsersController userController = new UsersController();
            Response<string> response = new Response<string>();
            response = await userController.PostForgotPasswordforCommunityAdmin(emailID, communityName);
            if (response.status == "Success")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response.status;
                return View();
            }

          
        }
         [HttpPost]
        public async Task<ActionResult> SystemAdminForgotPassword(string emailID)
        {
            Session["loginUserID"] = null;
            Session["loginCommunityID"] = null;
            UsersController userController = new UsersController();
            Response<string> response = new Response<string>();
            response = await userController.PostForgotPasswordforSystemAdmin(emailID);
            if (response.status == "Success")
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                TempData["error"] = response.status;
                return View();
            }


        }
 //-------------------------------------------------------_CommunityAdminNavbarAndSideMenu Controllers---------------------------------------------------------
        public  ActionResult _CommunityAdminNavbarAndSideMenu()
      {
          if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
          {
              TempData["error"] = "Null";
              return View("index");
          }
          Session.Timeout = 1000;
            UsersController userController = new UsersController();
            tokenDTO<User> userResponse = new tokenDTO<User>();
            userResponse = userController.GetUserforNavBar(Convert.ToInt32(Session["loginUserID"].ToString()),1);
            CommunityNavbarAndSideMenuDTO communitysidebar = new CommunityNavbarAndSideMenuDTO();
            communitysidebar.tokenUser = userResponse;
            CommunitiesController communityController = new CommunitiesController();
            Response<Community> responceCommunity = new Response<Community>();
            responceCommunity =communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            communitysidebar.community = responceCommunity.model;
            return PartialView(communitysidebar);
        }

        public async Task<ActionResult> _GetUnreadAlertsCount()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return View("index");
            }
            Session.Timeout = 1000;
            AlertsController alertsController = new AlertsController();

            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            responseAlerts = await alertsController.GetCountUnReadAlertByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            int countUnreadAlert = responseAlerts.model.Count();
            return Json(new { UnreadAlertCount = countUnreadAlert });
        }
        public async Task<JsonResult> _SetAlertAsRead()
        {
            AlertsController alertsController = new AlertsController();

            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            responseAlerts = await alertsController.GetUnReadAlertByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            Response<List<Alert>> responseUnReadAlerts = new Response<List<Alert>>();
            responseUnReadAlerts = await alertsController.GetCountUnReadAlertByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);

            foreach (var item in responseUnReadAlerts.model)
            {
                item.isViewed = true;
                var alertIsView = await alertsController.PutAlert(item.id, item);
            }
            //for (int i = 0; i < messagelist.Count; i++)
            //{
            //    messagelist[i].isRead = true;
            //    int id = messagelist[i].messageID;
            //    var xxx = await messageController.PutMessages(id, messagelist[i]);
            //}
            int countUnreadAlert = 0;
            return Json(new { UnreadAlertCount = countUnreadAlert });
        }

        public async Task<ActionResult> _GetAllUnReadAlerts()
        {
            AlertsController alertsController = new AlertsController();

            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            responseAlerts = await alertsController.GetUnReadAlertByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            return Json(new { responseAlerts = responseAlerts.model });
        }
//-------------------------------------------------------_startupModal Controllers---------------------------------------------------------
        public ActionResult _startupModal()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return View("index");
            }
            Session.Timeout = 1000;
            CommunityNavbarAndSideMenuDTO communitysidebar = new CommunityNavbarAndSideMenuDTO();
            StartupModal startupModal = new StartupModal();
            CommunitiesController communityController = new CommunitiesController();

            Response<CommunityCompleteDTO> communityResponse = new Response<CommunityCompleteDTO>();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);
            communityResponse.model.community.isFirstTimeUser = false;
            communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model.community);
            UsersController userController = new UsersController();
            Response<User> user = new Response<User>();


            DirectoryImagesController directoryImagesController = new DirectoryImagesController();
            Response<List<DirectoryImages>> responceDirectoryImage = new Response<List<DirectoryImages>>();
            responceDirectoryImage = directoryImagesController.GetAllDirectoryImages();
            startupModal.directoryImage = responceDirectoryImage.model;

            CommunityServicesController communityServicesController =new CommunityServicesController();
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            responceCommunityService= communityServicesController.GetAllCommunityServices(Convert.ToInt32(Session["loginCommunityID"].ToString()),1);
            startupModal.selectedCommunityServices = responceCommunityService.model;
            
            
            
            
            
            user = userController.GetCommunityUserbyID(communityResponse.model.community.adminUserID, 1);
            startupModal.community = communityResponse.model.community;
            startupModal.user = user.model;
            startupModal.communityFeaturesImage = communityResponse.model.communityFeaturesImage;
            startupModal.communityStreetFloor=communityResponse.model.communityStreetFloor;
            startupModal.communityServices = db.Services.ToList();

            return PartialView(startupModal);
        }
        public async Task<ActionResult> SetDropdownServicesStartUpModal()
        {
            CommunityServicesController comServController = new CommunityServicesController();

            //int counter = 0;
            //foreach (var serviceName in services)
            //{
            //    int communityid = Convert.ToInt32(Session["loginCommunityID"].ToString());
            //    CommunityService comService = new CommunityService();
            //    comService.communityID = communityid;
            //    comService.serviceName = serviceName;
            //    comService.icon = icons[counter];
            //    counter++;
            //    await comServController.PostCommunityService(comService);
            //}
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
            responceCommunityService = communityServicesController.GetAllCommunityServices(Convert.ToInt32(Session["loginCommunityID"].ToString()), 1);

            return Json(new { ResponceCommunityService = responceCommunityService.model });
        }
        public async Task<ActionResult> UpdateModalFirstScreen(FormCollection data)
        {
            string responceMessage = "Success";
            string imageString = null;
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }

            Session.Timeout = 1000;
            string FirstName = data["FirstName"].ToString();
            string Email = data["Email"].ToString();
            string aboutCommunity = data["aboutCommunity"].ToString();
            string addressWords = data["addressWords"].ToString();
            double addressLongitude;
            double addressLatitude ;
            if(data["addressLongitude"].ToString()!=""){
                addressLongitude = Convert.ToDouble(data["addressLongitude"].ToString());
            }
            else
            {
                addressLongitude = 0.0;
            }
            if (data["addressLatitude"].ToString() != "")
            {
                addressLatitude = Convert.ToDouble(data["addressLatitude"].ToString());
            }
            else
            {
                 addressLatitude = 0.0;
            }


            //Update community
            CommunitiesController communityController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));

            communityResponse.model.about = aboutCommunity;
            communityResponse.model.lat = addressLongitude;
            communityResponse.model.lng = addressLatitude; 
            communityResponse.model.communityAddress =addressWords;
            communityResponse =await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model);


            //Update User
            
            UsersController userController = new UsersController();
            tokenDTO<User> userTokenResponce = new tokenDTO<User>();
            Response<User> userResponse = new Response<User>();
            userTokenResponce = await userController.GetUserbyID(communityResponse.model.adminUserID);
            userResponse.model = userTokenResponce.model;
            userResponse.model.firstName = FirstName;
            userResponse.model.emailID = Email;
            userResponse.model.firstName = FirstName;
            if (Request.Files["userPhoto"] != null)
            {
                var postedFile = Request.Files["userPhoto"];
                if (postedFile.ContentLength != 0)
                {

                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    userResponse.model.image = blobImageURL;
                    imageString = blobImageURL;
                    userResponse = await userController.PutUser(userResponse.model.emailID, userResponse.model);
                    if (userResponse.status != "Success")
                    {
                        responceMessage = "Failed To Update";
                        return Json(new { ResponceMessage = responceMessage });
                    }
                }

            }

            return Json(new { ResponceMessage = responceMessage, ImageString = imageString });
        }


        public ActionResult blobImageURL(FormCollection data)
        {
            if (Request.Files["iconImage"] != null)
            {
                var postedFile = Request.Files["iconImage"];
                if (postedFile.ContentLength != 0)
                {

                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();

                    return Json(new { ResponceImage = blobImageURL });
                }
                else
                {
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();

                    return Json(new { ResponceImage = blobImageURL });
                }

            }
            else
            {
                return Json(new { ResponceImage ="null" });
            }
        }
        public async Task<ActionResult> ChangeCommunityAdminImage(FormCollection data)
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
        public async Task<ActionResult> ChangeCommunityPassword(FormCollection data)
        {
            string responceMessage="Success";
            //string imageString = null;
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            string oldpassword = data["oldpass"].ToString();
            string newpassword = data["newpass"].ToString();
            string emailID = data["emailID"].ToString();
            //string communitySecretCode = data["communitySecretCode"].ToString();
            int userID = Convert.ToInt32(Session["loginUserID"].ToString());
            CommunitiesController communityController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));


            UsersController userController = new UsersController();
            tokenDTO<User> userTokenResponce = new tokenDTO<User>();
            Response<User> userResponse = new Response<User>();
            userTokenResponce = await userController.GetUserbyID(userID);
            userResponse.model = userTokenResponce.model;
            if (communityResponse.model.communityPassword == oldpassword)
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
            if (communityResponse.model.communityPassword == oldpassword)
            {

                communityResponse.model.communityPassword = newpassword;
             
                communityResponse = await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model);
                if (communityResponse.status != "Success")
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
        

   

        //please set it only string term if you change it than it will not work 
        public async Task<ActionResult> SearchCommunity(string term)
        {
            
            CommunitiesController communityController = new CommunitiesController();
            List<string> communityList= new List<string>();
            communityList = communityController.SearchCommunity(term);
            return Json(communityList, JsonRequestBehavior.AllowGet);

            
        }

        public async Task<ActionResult> home()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return View("index");
            }
            Session.Timeout = 1000;
            CommunitiesController communityController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));


            CommunityDashboardDTO communityDashboardDTO = new CommunityDashboardDTO();
            communityDashboardDTO.isFirstTime = communityResponse.model.isFirstTimeUser;


            //Get unread iReports
            ComplaintsController complaintsController = new ComplaintsController();
            Response<List<Complaint>> responseComplaint = new Response<List<Complaint>>();
            responseComplaint =await complaintsController.GetUnreadComplaintByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()),1);
            communityDashboardDTO.NewiReports = responseComplaint.model.Count();


            //Get unread Messages
            Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
            ChatsController chatController = new ChatsController();
            chatUserResponce = await chatController.GetUnReadChatUsers(Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()),1);
            communityDashboardDTO.NewMessages = chatUserResponce.model.Count();
            ViewBag.Title = "Home";

            return View(communityDashboardDTO);
        }
        public async Task<ActionResult> logout()
        {
            Session["loginUserID"] = null;
            Session["loginCommunityID"] = null;
            TempData["error"] = "Null";


            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CreateCommunity()
        {
            TempData["error"] = "Null";
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateCommunity(string communityName, string communityPassword, string communityRequiredPassword)
        {
            if (communityName == "")
            {
                //TempData["error"] = "Please Enter Community Name";
                //return View();
                return Json(new { status = "Please Enter Community Name" });
            }
            else 
            { 
            //createCommunityDTO.community.name = createCommunityDTO.community.name.ToLower();
            CommunitiesController GetCommunitiesController = new CommunitiesController();
            Response<Community> responseGetCommunity = new Response<Community>();
            responseGetCommunity = await GetCommunitiesController.GetCommunityByName(communityName);
            if (responseGetCommunity.model==null)
            {
                if (communityRequiredPassword == "vineetun")
            {
                CommunitiesController communitiesController = new CommunitiesController();

                UsersController userController = new UsersController();
                Response<User> userResponse = new Response<User>();
                userResponse = await userController.PostCommunityAdminUser(communityName, communityPassword);

                if (userResponse.status != "Success")
                {
                    //TempData["error"] = userResponse.status;
                    //return View();
                    return Json(new { status = userResponse.status });
                }
                Community community = new Community();
                community.communityPassword = communityPassword;
                community.name= communityName;
                community.isChangePassword = false;
                community.adminUserID = userResponse.model.userID;
                community.isFirstTimeUser = true;
                community.emergencyContactRange = 2;
                Response<Community> communityResponse = new Response<Community>();
                communityResponse = await communitiesController.PostCommunity(community);



                //Add preDefined ireports types

                CommunityiReportsController communityiReportsController = new CommunityiReportsController();

                ComplaintCatagoriesController complaintCatagoryController = new ComplaintCatagoriesController();
                Response<List<ComplaintCatagory>> responceComplaintCatagory = new Response<List<ComplaintCatagory>>();
                responceComplaintCatagory = await complaintCatagoryController.GetComplaintCatagories();
                foreach (var item in responceComplaintCatagory.model)
                {
                    CommunityiReports communityiReports = new CommunityiReports();
                    communityiReports.iReportsName = item.description;
                    communityiReports.communityID = communityResponse.model.communityID;
                    Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
                    responceCommunityiReports = await communityiReportsController.PostCommunityiReports(communityiReports);
                }

                if (communityResponse.status == "Success")
                {
                    
                    //TempData["error"] = "Null";
                    //return RedirectToAction("Index");
                    return Json(new { status = "Success", CommunityResponse = communityResponse.model });
                }
                else
                {
                    //TempData["error"] = communityResponse.status;
                    //return View();
                    return Json(new { status = communityResponse.status });
                }
            }
            else
            {
                //TempData["error"] = "Required Password did not Match";
                    //return View();
                return Json(new { status = "Required Password did not Match" });
            }
        }
            else
            {
                //TempData["error"] = "This Community name is already exists";
                //return View();
                return Json(new { status = "This Community name is already exists" });
            }
        }
        }
//-------------------------------------------------------Announcement Controllers---------------------------------------------------------


        public async Task<ActionResult> Announcements(int? page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index","Home");
            }
            Session.Timeout = 1000;
            int currentPage;
            if (page == null)
            {
                currentPage = 1;
            }
            else
            {
                string pageString = page.ToString();
                currentPage = Convert.ToInt32(pageString);
            }
            AnnouncementsController announcementController = new AnnouncementsController();
            Response<List<Announcement>> responceAnnouncement = new Response<List<Announcement>>();
            responceAnnouncement = await announcementController.GetAllAnnouncementsbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
        ViewBag.Title = "Community Dashboard";
        AnnouncementDTO announcementDTO = new AnnouncementDTO();
        announcementDTO.announcementList = responceAnnouncement.model;
        announcementDTO.PagedannouncementList = announcementDTO.announcementList.ToPagedList(currentPage,15);
        return View(announcementDTO);
        }
        [HttpPost]
        public async Task<ActionResult> AnnouncementsPost(AnnouncementDTO announcementDTO)
        {
            ViewBag.Title = "Community Dashboard";

             
                announcementDTO.announcement.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            AnnouncementsController announcementController = new AnnouncementsController();
            Response<Announcement> responceAnnouncement = new Response<Announcement>();
            Announcement announcement = new Announcement();
            announcement=announcementDTO.announcement;
            responceAnnouncement = await announcementController.PostAnnouncement(announcement);

            return RedirectToAction("Announcements","Home");
        }
        [HttpPost]
        public async Task<ActionResult> AnnouncementsEdit(AnnouncementDTO announcementDTO)
        {
           
            announcementDTO.announcement.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            AnnouncementsController announcementController = new AnnouncementsController();
            Response<Announcement> responceAnnouncement = new Response<Announcement>();
            int announcementID = Convert.ToInt32(announcementDTO.announcement.id);


            DateTime ServerDateTime = DateTime.Now;
            DateTime utcDateTime = ServerDateTime.ToUniversalTime();

            // ID from: 
            // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
            // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
            string malayTimeZoneKey = "Singapore Standard Time";
            TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
            DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);




            announcementDTO.announcement.date = malayDateTime;
            responceAnnouncement = await announcementController.PutAnnouncement(announcementID,announcementDTO.announcement);

            return RedirectToAction("Announcements", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> AnnouncementsDelete(AnnouncementDTO announcementDTO)
        {

            AnnouncementsController announcementController = new AnnouncementsController();
            Response<Announcement> responceAnnouncement = new Response<Announcement>();
            int announcementID = Convert.ToInt32(announcementDTO.announcement.id);
            responceAnnouncement = await announcementController.DeleteAnnouncement(announcementID);

            return RedirectToAction("Announcements", "Home");
        }
        
        
//-------------------------------------------------------Complaints Controllers---------------------------------------------------------
        //this globel variable is for pdf
        static ComplaintCatagoryTypeDTO complaintCatagoryTypeDTOForActions=null;
        public async Task<ActionResult> Complaints(int? page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index","Home");
            }
            Session.Timeout = 1000;
            int currentPage;
            if (page == null)
            {
                currentPage = 1;
            }
            else
            {
                string pageString = page.ToString();
                currentPage = Convert.ToInt32(pageString);
            }
            //Get All Complaints with member
            ComplaintsController complaintsController = new ComplaintsController();
            Response<List<Complaint>> responseComplaints = new Response<List<Complaint>>();
            List<ComplaintsDTO> ListComplaintDTO = new List<ComplaintsDTO>();
            responseComplaints =await complaintsController.GetComplaintByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseComplaints.model = responseComplaints.model.OrderBy(x=>x.complaintStatusID).ThenByDescending(x=>x.complaintID).ToList();
            foreach (var item in responseComplaints.model)
            {
                Response<Member> responseMember = new Response<Member>();
                MembersController memberController = new MembersController();
                responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.userID);
              
                ComplaintsDTO complaintDTO = new ComplaintsDTO();
                complaintDTO.complaint = item;
                complaintDTO.member = responseMember.model;
                ListComplaintDTO.Add(complaintDTO);
            }
            //Get All CommunityiReports
            CommunityiReportsController complaintsCatagoryController = new CommunityiReportsController();
            Response<List<CommunityiReports>> responseCommunityiReports = new Response<List<CommunityiReports>>();
            responseCommunityiReports = complaintsCatagoryController.GetCommunityiReportsbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseCommunityiReports.model = responseCommunityiReports.model.OrderByDescending(x => x.CommunityiReportsID).ToList();
            //Set into ComplaintCatagoryDTO
            ComplaintCatagoryTypeDTO complaintCatagoryTypeDTO = new ComplaintCatagoryTypeDTO();

            complaintCatagoryTypeDTO.iReportName = "All iReports";
            complaintCatagoryTypeDTO.complaints = ListComplaintDTO.OrderBy(x=>x.complaint.complaintID).ToList();
            int totalReceivedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()),-1, 2);
            int totalUpdatedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()),-1, 1);
            int totalClosedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), -1, 3);
            
            
            complaintCatagoryTypeDTO.CommunityiReports = responseCommunityiReports.model;
   
            
            //complaintCatagoryTypeDTO.complaintsChartDTOList=complaintsChartDTOList;
            complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = (int)Math.Round((double)(100 * ListComplaintDTO.Count) / ListComplaintDTO.Count);

            complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = (int)Math.Round((double)(100 * totalReceivedComplaintsCount) / ListComplaintDTO.Count);

            complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = (int)Math.Round((double)(100 * totalUpdatedComplaintsCount) / ListComplaintDTO.Count);
            complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = (int)Math.Round((double)(100 * totalClosedComplaintsCount) / ListComplaintDTO.Count);

            if (responseCommunityiReports.model.Count > 0)
            {
                complaintCatagoryTypeDTO.communityName = responseCommunityiReports.model[0].community.name;
                complaintCatagoryTypeDTO.communityAdminEmail = responseCommunityiReports.model[0].community.user.emailID;
                complaintCatagoryTypeDTO.communityAddress = responseCommunityiReports.model[0].community.communityAddress;
            }
            complaintCatagoryTypeDTOForActions = complaintCatagoryTypeDTO;
            complaintCatagoryTypeDTO.Pagedcomplaints = complaintCatagoryTypeDTO.complaints.OrderByDescending(x=>x.complaint.complaintID).ToPagedList(currentPage, 15);
            TempData["DropDownSelectedIndex"] = -1;
            return View(complaintCatagoryTypeDTO);
        }


        public async Task<ActionResult> _ComplaintsChartPartialView()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            //Get All CommunityiReports
            CommunityiReportsController complaintsCatagoryController = new CommunityiReportsController();
            Response<List<CommunityiReports>> responseCommunityiReports = new Response<List<CommunityiReports>>();
            responseCommunityiReports = complaintsCatagoryController.GetCommunityiReportsbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));






            ComplaintsController complaintsController = new ComplaintsController();
            List<ComplaintsChartDTO> complaintsChartDTOList = new List<ComplaintsChartDTO>();
            foreach (var item in responseCommunityiReports.model)
            {
                ComplaintsChartDTO complaintsChartDTO = new ComplaintsChartDTO();
                int totalReceivedComplaints = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.CommunityiReportsID, 2);
                int totalUpdatedComplaints = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.CommunityiReportsID, 1);
                int totalClosedComplaints = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.CommunityiReportsID, 3);
                complaintsChartDTO.iReportName = item.iReportsName;
                complaintsChartDTO.totalClosediReport = totalClosedComplaints;
                complaintsChartDTO.totalReceivediReport = totalReceivedComplaints;
                complaintsChartDTO.totalUpdateiReport = totalUpdatedComplaints;
                complaintsChartDTOList.Add(complaintsChartDTO);
            }
           
            return View(complaintsChartDTOList);
        }

        public async Task<ActionResult> PdfiReport(string isImageInclude, string isLocationInclude)
        {
            ComplaintCatagoryTypeDTO complaintCatagoryTypeDTO = new ComplaintCatagoryTypeDTO();
            complaintCatagoryTypeDTO.communityAddress = complaintCatagoryTypeDTOForActions.communityAddress;
            complaintCatagoryTypeDTO.communityAdminEmail = complaintCatagoryTypeDTOForActions.communityAdminEmail;
            complaintCatagoryTypeDTO.CommunityiReports = complaintCatagoryTypeDTOForActions.CommunityiReports;
            complaintCatagoryTypeDTO.communityName = complaintCatagoryTypeDTOForActions.communityName;
            complaintCatagoryTypeDTO.complaints = complaintCatagoryTypeDTOForActions.complaints;
            complaintCatagoryTypeDTO.iReportName = complaintCatagoryTypeDTOForActions.iReportName;
            complaintCatagoryTypeDTO.isImageInclude = complaintCatagoryTypeDTOForActions.isImageInclude;
            complaintCatagoryTypeDTO.isLocationInclude = complaintCatagoryTypeDTOForActions.isLocationInclude;
            complaintCatagoryTypeDTO.Pagedcomplaints = complaintCatagoryTypeDTOForActions.Pagedcomplaints;
            complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalClosedComplaintsPercentage;
            complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalReceivedComplaintsPercentage;
            complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalSelectedCatagoryComplaintsPercentage;
            complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalUpdatedComplaintsPercentage;
            if (isImageInclude == "true")
            {
                complaintCatagoryTypeDTO.isImageInclude = true;
            }
            if (isLocationInclude == "true")
            {
                complaintCatagoryTypeDTO.isLocationInclude = true;
            }

            return new PartialViewAsPdf("_ComplaintsPDFPartialView", complaintCatagoryTypeDTO)
            {
                FileName = "iReport.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(0, 0, 0, 0),
                CustomSwitches = "--print-media-type",
            };
        }


        public async Task<ActionResult> PdfiReportTest(string isImageInclude, string isLocationInclude)
        {
            ComplaintCatagoryTypeDTO complaintCatagoryTypeDTO = new ComplaintCatagoryTypeDTO();
            complaintCatagoryTypeDTO.communityAddress = complaintCatagoryTypeDTOForActions.communityAddress;
            complaintCatagoryTypeDTO.communityAdminEmail = complaintCatagoryTypeDTOForActions.communityAdminEmail;
            complaintCatagoryTypeDTO.CommunityiReports = complaintCatagoryTypeDTOForActions.CommunityiReports;
            complaintCatagoryTypeDTO.communityName = complaintCatagoryTypeDTOForActions.communityName;
            complaintCatagoryTypeDTO.complaints = complaintCatagoryTypeDTOForActions.complaints;
            complaintCatagoryTypeDTO.iReportName = complaintCatagoryTypeDTOForActions.iReportName;
            complaintCatagoryTypeDTO.isImageInclude = complaintCatagoryTypeDTOForActions.isImageInclude;
            complaintCatagoryTypeDTO.isLocationInclude = complaintCatagoryTypeDTOForActions.isLocationInclude;
            complaintCatagoryTypeDTO.Pagedcomplaints = complaintCatagoryTypeDTOForActions.Pagedcomplaints;
            complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalClosedComplaintsPercentage;
            complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalReceivedComplaintsPercentage;
            complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalSelectedCatagoryComplaintsPercentage;
            complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = complaintCatagoryTypeDTOForActions.totalUpdatedComplaintsPercentage;
            if (isImageInclude == "true")
            {
                complaintCatagoryTypeDTO.isImageInclude = true;
            }
            if (isLocationInclude == "true")
            {
                complaintCatagoryTypeDTO.isLocationInclude = true;
            }

            //return new PartialViewAsPdf("_ComplaintsPDFPartialView", complaintCatagoryTypeDTO)
            //{
            //    FileName = "iReport.pdf" , 
            //    PageSize = Size.Tabloid,
            //    PageOrientation = Orientation.Portrait,
            //    PageMargins = new Margins(0, 0, 0, 0),
            //    CustomSwitches = "--print-media-type",
            //  };
            return View("_ComplaintsPDFPartialView", complaintCatagoryTypeDTO);
        }
        public async Task<ActionResult> ComplaintChangeStatus(int complaintID, int statusID)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ComplaintsController complaintsController = new ComplaintsController();
            Response<Complaint> responseGetComplaint = new Response<Complaint>();
            Response<Complaint> responseComplaint = new Response<Complaint>();
            responseGetComplaint = await complaintsController.GetComplaintByID(complaintID);

            responseGetComplaint.model.complaintStatusID = statusID;
            responseComplaint = await complaintsController.PutComplaint(complaintID,responseGetComplaint.model);



            //get Allcomplaint for pdf
            Response<List<Complaint>> responseComplaints = new Response<List<Complaint>>();
            List<ComplaintsDTO> ListComplaintDTO = new List<ComplaintsDTO>();
            responseComplaints = await complaintsController.GetComplaintByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseComplaints.model = responseComplaints.model.OrderBy(x => x.complaintStatusID).ThenByDescending(x => x.complaintID).ToList();
            
            foreach (var item in responseComplaints.model)
            {
                Response<Member> responseMember = new Response<Member>();
                MembersController memberController = new MembersController();
                responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.userID);

                ComplaintsDTO complaintDTO = new ComplaintsDTO();
                complaintDTO.complaint = item;
                complaintDTO.member = responseMember.model;
                ListComplaintDTO.Add(complaintDTO);
            }
            complaintCatagoryTypeDTOForActions.complaints = ListComplaintDTO;
            return Json(new {success="Success"});
        }
        public async Task<ActionResult> GetComplaintType(int complaintCatagoryID)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

    
            return Json(new { ComplaintType = "Success" });
        }


        public async Task<ActionResult> ComplaintSearch(int ComplaintsiReportID, int page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ComplaintCatagoryTypeDTO complaintCatagoryTypeDTO = new ComplaintCatagoryTypeDTO();
            //Get  Complaints

            CommunityiReportsController communityiReportsController = new CommunityiReportsController();
            Response<List<CommunityiReports>> responseCommunityiReports = new Response<List<CommunityiReports>>();
            responseCommunityiReports = communityiReportsController.GetCommunityiReportsbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseCommunityiReports.model = responseCommunityiReports.model.OrderByDescending(x => x.CommunityiReportsID).ToList();
            complaintCatagoryTypeDTO.CommunityiReports = responseCommunityiReports.model;
            
            ComplaintsController complaintsController = new ComplaintsController();
            Response<List<Complaint>> responseComplaints = new Response<List<Complaint>>();
            List<ComplaintsDTO> ListComplaintDTO = new List<ComplaintsDTO>();
            responseComplaints = await complaintsController.GetComplaintByCommunityIDCatagoryID(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID);
            responseComplaints.model = responseComplaints.model.OrderBy(x => x.complaintStatusID).ThenByDescending(x => x.complaintID).ToList();
            

            foreach (var item in responseComplaints.model)
            {
                Response<Member> responseMember = new Response<Member>();
                MembersController memberController = new MembersController();
                responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.userID);

                ComplaintsDTO complaintDTO = new ComplaintsDTO();
                complaintDTO.complaint = item;
                complaintDTO.member = responseMember.model;
                ListComplaintDTO.Add(complaintDTO);
            }
            ////Get All Complaints Catagories
            //ComplaintCatagoriesController complaintsCatagoryController = new ComplaintCatagoriesController();
            //Response<List<ComplaintCatagory>> responseComplaintCatagory = new Response<List<ComplaintCatagory>>();
            //responseComplaintCatagory = await complaintsCatagoryController.GetComplaintCatagories();

            Response<List<Complaint>> responseAllComplaints = new Response<List<Complaint>>();
            responseAllComplaints = await complaintsController.GetComplaintByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));


            //Set into ComplaintCatagoryDTO
            int totalReceivedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 2);
            int totalUpdatedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 1);
            int totalClosedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 3);

            complaintCatagoryTypeDTO.complaints = ListComplaintDTO;
            if (responseAllComplaints.model.Count != 0)
            {
                complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = (int)Math.Round((double)(100 * ListComplaintDTO.Count) / responseAllComplaints.model.Count);

            }
            else
            {
                complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = 0;
            }
            if (ListComplaintDTO.Count != 0)
            {
                complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = (int)Math.Round((double)(100 * totalReceivedComplaintsCount) / ListComplaintDTO.Count);

                complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = (int)Math.Round((double)(100 * totalUpdatedComplaintsCount) / ListComplaintDTO.Count);
                complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = (int)Math.Round((double)(100 * totalClosedComplaintsCount) / ListComplaintDTO.Count);

            }

            else
            {
                complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = 0;
                complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = 0;
                complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = 0;
            }
            if (responseAllComplaints.model.Count > 0)
            {
                complaintCatagoryTypeDTO.communityName = responseAllComplaints.model[0].community.name;
                complaintCatagoryTypeDTO.communityAdminEmail = responseAllComplaints.model[0].community.user.emailID;
                complaintCatagoryTypeDTO.communityAddress = responseAllComplaints.model[0].community.communityAddress;
            }
            complaintCatagoryTypeDTOForActions = complaintCatagoryTypeDTO;
            complaintCatagoryTypeDTO.Pagedcomplaints = complaintCatagoryTypeDTO.complaints.OrderByDescending(x => x.complaint.complaintID).ToPagedList(page, 15);
            //complaintCatagoryTypeDTO.ComplaintCatagory = responseComplaintCatagory.model;
            TempData["DropDownSelectedIndex"]= ComplaintsiReportID;
            if (ComplaintsiReportID == -1)
            {
                complaintCatagoryTypeDTO.iReportName = "All iReports";
            }
            else
            {
                Response<CommunityiReports> responseCommunityiReport = new Response<CommunityiReports>();
                responseCommunityiReport = await communityiReportsController.GetCommunityiReports(ComplaintsiReportID);
                complaintCatagoryTypeDTO.iReportName = responseCommunityiReport.model.iReportsName;
            }
            return PartialView(complaintCatagoryTypeDTO);
           
        }

        //For page 2,3,4.......... click
        public async Task<ActionResult> PaginationComplaintSearch(int ComplaintsiReportID, int page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ComplaintCatagoryTypeDTO complaintCatagoryTypeDTO = new ComplaintCatagoryTypeDTO();
            //Get  Complaints

            CommunityiReportsController communityiReportsController = new CommunityiReportsController();
            Response<List<CommunityiReports>> responseCommunityiReports = new Response<List<CommunityiReports>>();
            responseCommunityiReports = communityiReportsController.GetCommunityiReportsbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseCommunityiReports.model = responseCommunityiReports.model.OrderByDescending(x => x.CommunityiReportsID).ToList();
            complaintCatagoryTypeDTO.CommunityiReports = responseCommunityiReports.model;

            ComplaintsController complaintsController = new ComplaintsController();
            Response<List<Complaint>> responseComplaints = new Response<List<Complaint>>();
            List<ComplaintsDTO> ListComplaintDTO = new List<ComplaintsDTO>();
            responseComplaints = await complaintsController.GetComplaintByCommunityIDCatagoryID(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID);
            responseComplaints.model = responseComplaints.model.OrderBy(x => x.complaintStatusID).ThenByDescending(x => x.complaintID).ToList();
            

            foreach (var item in responseComplaints.model)
            {
                Response<Member> responseMember = new Response<Member>();
                MembersController memberController = new MembersController();
                responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.userID);

                ComplaintsDTO complaintDTO = new ComplaintsDTO();
                complaintDTO.complaint = item;
                complaintDTO.member = responseMember.model;
                ListComplaintDTO.Add(complaintDTO);
            }
            ////Get All Complaints Catagories
            //ComplaintCatagoriesController complaintsCatagoryController = new ComplaintCatagoriesController();
            //Response<List<ComplaintCatagory>> responseComplaintCatagory = new Response<List<ComplaintCatagory>>();
            //responseComplaintCatagory = await complaintsCatagoryController.GetComplaintCatagories();

            Response<List<Complaint>> responseAllComplaints = new Response<List<Complaint>>();
            responseAllComplaints = await complaintsController.GetComplaintByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));


            //Set into ComplaintCatagoryDTO
            int totalReceivedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 2);
            int totalUpdatedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 1);
            int totalClosedComplaintsCount = complaintsController.GetCountComplaitsByStatus(Convert.ToInt32(Session["loginCommunityID"].ToString()), ComplaintsiReportID, 3);

            complaintCatagoryTypeDTO.complaints = ListComplaintDTO;
            if (responseAllComplaints.model.Count != 0)
            {
                complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = (int)Math.Round((double)(100 * ListComplaintDTO.Count) / responseAllComplaints.model.Count);

            }
            else
            {
                complaintCatagoryTypeDTO.totalSelectedCatagoryComplaintsPercentage = 0;
            }
            if (ListComplaintDTO.Count != 0)
            {
                complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = (int)Math.Round((double)(100 * totalReceivedComplaintsCount) / ListComplaintDTO.Count);

                complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = (int)Math.Round((double)(100 * totalUpdatedComplaintsCount) / ListComplaintDTO.Count);
                complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = (int)Math.Round((double)(100 * totalClosedComplaintsCount) / ListComplaintDTO.Count);

            }

            else
            {
                complaintCatagoryTypeDTO.totalReceivedComplaintsPercentage = 0;
                complaintCatagoryTypeDTO.totalUpdatedComplaintsPercentage = 0;
                complaintCatagoryTypeDTO.totalClosedComplaintsPercentage = 0;
            }
            if (responseAllComplaints.model.Count > 0)
            {
                complaintCatagoryTypeDTO.communityName = responseAllComplaints.model[0].community.name;
                complaintCatagoryTypeDTO.communityAdminEmail = responseAllComplaints.model[0].community.user.emailID;
                complaintCatagoryTypeDTO.communityAddress = responseAllComplaints.model[0].community.communityAddress;
            }
            complaintCatagoryTypeDTOForActions = complaintCatagoryTypeDTO;
            complaintCatagoryTypeDTO.Pagedcomplaints = complaintCatagoryTypeDTO.complaints.OrderByDescending(x => x.complaint.complaintID).ToPagedList(page, 15);
            TempData["DropDownSelectedIndex"] = ComplaintsiReportID;
            if (ComplaintsiReportID == -1)
            {
                complaintCatagoryTypeDTO.iReportName = "All iReports";
            }
            else
            {
                Response<CommunityiReports> responseCommunityiReport = new Response<CommunityiReports>();
                responseCommunityiReport = await communityiReportsController.GetCommunityiReports(ComplaintsiReportID);
                complaintCatagoryTypeDTO.iReportName = responseCommunityiReport.model.iReportsName;
            }
            //complaintCatagoryTypeDTO.ComplaintCatagory = responseComplaintCatagory.model;
            return PartialView("Complaints", complaintCatagoryTypeDTO);
        }








        //-------------------------iReportsCatagory Controllers----------------------

        public async Task<ActionResult> AddiReportCatagory(string iReportCatagory)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityiReportsController communityiReportsController = new CommunityiReportsController();
            CommunityiReports communityiReports = new CommunityiReports();
            communityiReports.iReportsName = iReportCatagory;
            communityiReports.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            responceCommunityiReports = await communityiReportsController.PostCommunityiReports(communityiReports);
            return Json(new { responceCommunityiReports = responceCommunityiReports });
        }

        public async Task<ActionResult> UpdateiReportCatagory(string iReportCatagory, int iReportCatagoryId)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityiReportsController communityiReportsController = new CommunityiReportsController();
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            responceCommunityiReports = await communityiReportsController.GetCommunityiReports(iReportCatagoryId);
            responceCommunityiReports.model.iReportsName = iReportCatagory;
            responceCommunityiReports = await communityiReportsController.PutCommunityiReports(iReportCatagoryId, responceCommunityiReports.model);
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> DeleteiReportCatagory(int iReportCatagoryId)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityiReportsController communityiReportsController = new CommunityiReportsController();
            Response<CommunityiReports> responceCommunityiReports = new Response<CommunityiReports>();
            await communityiReportsController.DeleteCommunityiReports(iReportCatagoryId);
            return Json(new { success = "Success" });
        }


//-------------------------------------------------------Messages Controllers---------------------------------------------------------

        public async Task<ActionResult> Messages(int? page, string sortingOption)
        {
                 if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
                 Session.Timeout = 1000;
                 int currentPage;
                 if (page == null)
                 {
                     currentPage = 1;
                 }
                 else
                 {
                     string pageString = page.ToString();
                     currentPage = Convert.ToInt32(pageString);
                 }
                 Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
                 List<ChatDTO> listChatDTO = new List<ChatDTO>();
            ChatsController chatController = new ChatsController();

            if (sortingOption == "ByDate")
            {
                chatUserResponce = await chatController.GetChatUsers(Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()));
                chatUserResponce.model = chatUserResponce.model.OrderByDescending(x => x.chatMessageID).ToList();
                //var count=chatUserResponce.model.Count();
                //chatUserResponce.model = chatUserResponce.model.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
               
                TempData["SortingOption"] = "ByDate";
            }
            else
            {
                chatUserResponce = await chatController.GetChatUsers(Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()));
                TempData["SortingOption"] = "sort";
            }
            
            //chatUserResponce.model = chatUserResponce.model.OrderByDescending(x => x.Date).ToList();
            //var count=chatUserResponce.model.Count();
            //chatUserResponce.model = chatUserResponce.model.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
            foreach (var item in chatUserResponce.model)
            {

                ChatDTO chatDTO = new ChatDTO();
                Response<Member> responseMember = new Response<Member>();
                MembersController memberController = new MembersController();
                responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.from);
                chatDTO.chat = item;
                chatDTO.member = responseMember.model;
                listChatDTO.Add(chatDTO);
            
            }


            MemberAndChatDTO memberAndChatDTO = new MemberAndChatDTO();
            Response<List<Member>> responceMember = new Response<List<Member>>();
           
            MembersController allMemberController=new MembersController();
            responceMember = await allMemberController.getcommunityMemberwithoutAdmin(Convert.ToInt32(Session["loginCommunityID"].ToString()), Convert.ToInt32(Session["loginUserID"].ToString()));
            memberAndChatDTO.listChatDTO = listChatDTO;
            memberAndChatDTO.communityMembers = responceMember.model;
            memberAndChatDTO.PagedlistChatDTO = memberAndChatDTO.listChatDTO.ToPagedList(currentPage,15);
            return View(memberAndChatDTO);

        }
        //public async Task<ActionResult> MessagesbyDate(int? page)
        //{
        //    if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
        //    {
        //        TempData["error"] = "Null";
        //        return RedirectToAction("Index", "Home");
        //    }
         //    Session.Timeout = 1000;
        //    int currentPage;
        //    if (page == null)
        //    {
        //        currentPage = 1;
        //    }
        //    else
        //    {
        //        string pageString = page.ToString();
        //        currentPage = Convert.ToInt32(pageString);
        //    }
        //    Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
        //    List<ChatDTO> listChatDTO = new List<ChatDTO>();
        //    ChatsController chatController = new ChatsController();
        //    chatUserResponce = await chatController.GetChatUsers(Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()));
        //    //chatUserResponce.model = chatUserResponce.model.OrderByDescending(x => x.Date).ToList();
        //    //var count=chatUserResponce.model.Count();
        //    //chatUserResponce.model = chatUserResponce.model.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        //    foreach (var item in chatUserResponce.model)
        //    {

        //        ChatDTO chatDTO = new ChatDTO();
        //        Response<Member> responseMember = new Response<Member>();
        //        MembersController memberController = new MembersController();
        //        responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), item.from);
        //        chatDTO.chat = item;
        //        chatDTO.member = responseMember.model;
        //        listChatDTO.Add(chatDTO);

        //    }


        //    MemberAndChatDTO memberAndChatDTO = new MemberAndChatDTO();
        //    Response<List<Member>> responceMember = new Response<List<Member>>();

        //    MembersController allMemberController = new MembersController();
        //    responceMember = await allMemberController.getcommunityMemberwithoutAdmin(Convert.ToInt32(Session["loginCommunityID"].ToString()), Convert.ToInt32(Session["loginUserID"].ToString()));
        //    memberAndChatDTO.listChatDTO = listChatDTO;
        //    memberAndChatDTO.communityMembers = responceMember.model;
        //    memberAndChatDTO.PagedlistChatDTO = memberAndChatDTO.listChatDTO.ToPagedList(currentPage, 2);
        //    return View(memberAndChatDTO);
        //}
        public async Task<ActionResult> GetUsersMessages(int userID)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
            ChatsController chatController = new ChatsController();
            chatUserResponce = await chatController.GetChatForAdmin(userID,Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()));
            return Json(new {ChatUserResponce=chatUserResponce.model });
        }
        public async Task<ActionResult> SendMessage(FormCollection data)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Chat message = new Chat();
            int userID =Convert.ToInt32(data["userId"]);
            string image = null;
            string description = data["message"].ToString();
            if (Request.Files["files"] == null && description == "\r\n")
            {
                return Json(new { status="failed" });
            }
            if (Request.Files["files"] != null)
            {
                using (var binaryReader = new BinaryReader(Request.Files["files"].InputStream))
                {
                   
                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    image = blobImageURL;
                }
            }
            Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
            Response<Chat> PostchatUserResponce = new Response<Chat>();
            ChatsController chatController = new ChatsController();
            await chatController.PostChatForAdmin(description, userID, Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()),image);
            chatUserResponce = await chatController.GetChatForAdmin(userID, Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()));
            return Json(new { ChatUserResponce = chatUserResponce.model, status = "Success" });
        }
        public async Task<ActionResult> ChangeMessageStatus(int messageID, string status)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Chat> chatUser = new Response<Chat>();
            ChatsController chatController = new ChatsController();
           
            
                chatUser = await chatController.GetChatUserbyMessageIDAndReadMessage(messageID,status);
            
           
            return Json(new { Success= "success" });
        }
        public async Task<ActionResult> GetLatestUsersMessages(int userID,int messageID)
        {
            //if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            //{
            //    TempData["error"] = "Null";
            //    return RedirectToAction("Index", "Home");
            //}
            Session.Timeout = 1000;
            Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
            ChatsController chatController = new ChatsController();
            chatUserResponce = await chatController.GetLatestChatForAdmin(userID, Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()),messageID);
            return Json(new { ChatUserResponce = chatUserResponce.model });
        }


        public async Task<ActionResult> GettUsersNewMessages(int userID, int messageID)
        {
            //if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            //{
            //    TempData["error"] = "Null";
            //    return RedirectToAction("Index", "Home");
            //}
            Session.Timeout = 1000;
            Response<List<Chat>> chatUserResponce = new Response<List<Chat>>();
            ChatsController chatController = new ChatsController();
            chatUserResponce = await chatController.GetLatestChatForAdmin(userID, Convert.ToInt32(Session["loginUserID"].ToString()), Convert.ToInt32(Session["loginCommunityID"].ToString()), messageID);
            Chat chatLast =new Chat();
            chatLast = chatUserResponce.model.LastOrDefault();

            return Json(new { chatLast = chatLast });
        }



//-------------------------------------------------------Services Controllers------------------------------------------------------------
        
        
        public async Task<ActionResult> Services()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<List<CommunityService>> responceCommunityService = new Response<List<CommunityService>>();
                Response<List<Service>> responceCommunityRemainingService = new Response<List<Service>>();
                //comunity Selected Services
            responceCommunityService =await communityServicesController.GetServicebyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
           //comunity Remaining Services
            responceCommunityRemainingService =await communityServicesController.GetRemainingServiceOfCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()),1);
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
            return View(serviceDTO);
        }
        public async Task<ActionResult> AddCommunityServices(string serviceName,string imageURL)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityServicesController communityServicesController = new CommunityServicesController();
            Response<CommunityService> responcePostCommunityService = new Response<CommunityService>();
            responcePostCommunityService =await communityServicesController.GetCommunityService(communityServiceID);
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
           
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            ServiceStaffsController serviceStaffsController = new ServiceStaffsController();
            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaff serviceStaff = new ServiceStaff();
            serviceStaff.communityServiceID= Convert.ToInt32(data["communityServiceID"].ToString());
            serviceStaff.name= data["Name"].ToString();
            serviceStaff.staffRole= data["Designation"].ToString();
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            //ServicesController serviceController = new ServicesController();
            //Response<Service> responceService = new Response<Service>();
            //responceService = await serviceController.GetServicebyName(serviceName);

            Response<CommunityService> responceCommunityService = new Response<CommunityService>();
            CommunityServicesController communityServicesController=new CommunityServicesController();
            responceCommunityService=await communityServicesController.GetCommunityService(serviceID);
            
           
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;

            Response<ServiceStaff> responseServiceStaff = new Response<ServiceStaff>();
            ServiceStaffsController serviceStaffController = new ServiceStaffsController();
            responseServiceStaff = await serviceStaffController.DeleteServiceStaff(CommunityServiceID);
            responseServiceStaff=null;
            return Json(new{Success="sucess"});
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
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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

            //For Push Notification
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            //CommunitiesController communitiesController = new CommunitiesController();
            MembersController membercontroller = new MembersController();
            //Response<Community> communityResponse = new Response<Community>();
            int communityID = responseServiceStaff.model.communityID;
            //communityResponse = communitiesController.GetCommunitybyID(communityID);

               Response<List<Member>> responceMember = new Response<List<Member>>();
            responceMember =await membercontroller.GetAllCommunityMembersbyCommunityID(communityID);
            foreach (var item in responceMember.model)
            {
                if (item.user.Islogout == false)
                {
                    if (responseServiceStaff.model.isActive == true)
                    {


                        // iOS
                        var alert = "{\"aps\":{\"alert\":\"" + item.community.name + " : " + responseServiceStaff.model.name + " have been ACTIVE.\",\"id\":\"7\",\"communityid\":\"" + item.communityID + "\",\"sound\":\"default\"}}";
                        outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(item.userId));


                        // Android
                        //var notif = "{ \"data\" : {\"message\":\"You are blocked\",\"id\":\"4\"}}";
                        //var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You are blocked.\",\"badge\":\"1\",\"id\":\"4\",\"communityid\":\"" + member.communityID + "\"}}";

                        var notif = "{\"data\":{\"message\":\"" + item.community.name + " : " + responseServiceStaff.model.name + " have been ACTIVE.\",\"badge\":\"1\",\"id\":\"7\",\"communityName\":\"" + item.community.name + "\",\"communityid\":\"" + item.communityID + "\"}}";

                        outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(item.userId));

                                            }
                    else
                    {
                        // iOS
                        var alert = "{\"aps\":{\"alert\":\"" + item.community.name + " : " + responseServiceStaff.model.name + " have been BLOCKED.\",\"id\":\"7\",\"communityid\":\"" + item.communityID + "\",\"sound\":\"default\"}}";
                        outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, Convert.ToString(item.userId));


                        // Android
                        //var notif = "{ \"data\" : {\"message\":\"You are blocked\",\"id\":\"4\"}}";
                        //var notif = "{\"data\":{\"message\":\"" + memberFromDB.community.name + " : You are blocked.\",\"badge\":\"1\",\"id\":\"4\",\"communityid\":\"" + member.communityID + "\"}}";

                        var notif = "{\"data\":{\"message\":\"" + item.community.name + " : " + responseServiceStaff.model.name + " have been BLOCKED.\",\"badge\":\"1\",\"id\":\"7\",\"communityName\":\"" + item.community.name + "\",\"communityid\":\"" + item.communityID + "\"}}";

                        outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, Convert.ToString(item.userId));

                    }
                }
            }
            return Json(new { success = "Success" });
        }
//-------------------------------------------------------User Controllers--------------------------------------------------------------------

        public async Task<ActionResult> Users(int? page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            MembersController memberController = new MembersController();
            Response<List<Member>> responceMember = new Response<List<Member>>();
            responceMember =await memberController.GetAllCommunityMembersbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            int pageNumber = (page ?? 1);
            int pageSize = 15;
            return View(responceMember.model.ToPagedList(pageNumber, pageSize));
        }
        public async Task<ActionResult> ChangeUserStatus(int userID, bool isBlocked)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Member> responseMember = new Response<Member>();
            MembersController memberController = new MembersController();
            responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), userID);
            if(isBlocked==true){
                responseMember.model.isBlocked = false;
            }
            else
            {
                responseMember.model.isBlocked = true;
            }
           
            await memberController.PutMember(responseMember.model.id, responseMember.model);
         
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> ChangeUserIsAlertblocked(int userID, bool isAlertBlocked)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Member> responseMember = new Response<Member>();
            MembersController memberController = new MembersController();
            responseMember = await memberController.GetCommunityMember(Convert.ToInt32(Session["loginCommunityID"].ToString()), userID);
            if (isAlertBlocked == true)
            {
                responseMember.model.isAlertBlocked = false;
            }
            else
            {
                responseMember.model.isAlertBlocked = true;
            }

            await memberController.PutMember(responseMember.model.id, responseMember.model);

            return Json(new { success = "Success" });
        }
//-------------------------------------------------------AlertLog Controllers--------------------------------------------------------------------
        public async Task<ActionResult> AlertLog(int? page)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            AlertsController alertController = new AlertsController();
            responseAlerts= await alertController.GetAlertByCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            responseAlerts.model = responseAlerts.model.OrderByDescending(x => x.id).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 15;
            return View(responseAlerts.model.ToPagedList(pageNumber,pageSize));
        }
        public async Task<ActionResult> ChangeAlertStatus(int alertID)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Alert> responseAlert = new Response<Alert>();
            AlertsController alertController = new AlertsController();
            responseAlert =await alertController.GetAlertbyID(alertID);
            responseAlert.model.isViewed = true;
            await alertController.PutAlert(alertID, responseAlert.model);
            return Json(new{success="Success"});
        }

//-------------------------------------------------------Communities Controllers--------------------------------------------------------------------
        public async Task<ActionResult> CommunityProfile()
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<CommunityCompleteDTO> communityResponse = new Response<CommunityCompleteDTO>();
            CommunitiesController communityController = new CommunitiesController();
            communityResponse =communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()),1);
            return View(communityResponse.model);
        }

        public async Task<ActionResult> CommunityChangeAddress(string communityAddress,double? lat,double? lng)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Community> communityResponse = new Response<Community>();
            CommunitiesController communityController = new CommunitiesController();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            //if (communityAddress == "")
            //{
            //    communityAddress = "Null";

            //}
            communityResponse.model.communityAddress = communityAddress;
            if (lat != null)
            {
                communityResponse.model.lat = Convert.ToDouble(lat);
            }
            if (lng != null)
            {
                communityResponse.model.lng = Convert.ToDouble(lng);
            }
        
            communityResponse = await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model);
            return Json(new { success = "Success" });
        }



        public async Task<ActionResult> CommunityChangeAbout(string communityAbout)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<Community> communityResponse = new Response<Community>();
            CommunitiesController communityController = new CommunitiesController();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            communityResponse.model.about = communityAbout;
            communityResponse = await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()), communityResponse.model);
            return Json(new { success = "Success" });
        }

                                //-------------------------Street/Floor Controllers----------------------
   
        public async Task<ActionResult> AddCommunityStreetFloor(string StreetFloor)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityStreetFloorsController communityStreetFloorController = new CommunityStreetFloorsController();
              CommunityStreetFloor communityStreetFloor = new CommunityStreetFloor();
              Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            communityStreetFloor.streetFloor=StreetFloor;
            communityStreetFloor.communityID=Convert.ToInt32(Session["loginCommunityID"].ToString());
            responceCommunityStreetFloor= await communityStreetFloorController.PostCommunityStreetFloor(communityStreetFloor);
            return Json(new { ResponceCommunityStreetFloor = responceCommunityStreetFloor });
        }

        public async Task<ActionResult> UpdateCommunityStreetFloor(string StreetFloor, int Id)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityStreetFloorsController communityStreetFloorController = new CommunityStreetFloorsController();
            Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            responceCommunityStreetFloor=await communityStreetFloorController.GetCommunityStreetFloor(Id);
            responceCommunityStreetFloor.model.streetFloor = StreetFloor;
            responceCommunityStreetFloor = await communityStreetFloorController.PutCommunityStreetFloor(Id,responceCommunityStreetFloor.model);
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> DeleteCommunityStreetFloor(int Id)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityStreetFloorsController communityStreetFloorController = new CommunityStreetFloorsController();
            Response<CommunityStreetFloor> responceCommunityStreetFloor = new Response<CommunityStreetFloor>();
            await communityStreetFloorController.DeleteCommunityStreetFloor(Id);
            return Json(new { success = "Success" });
        }

        //-------------------------SecretCode Controllers----------------------

        public async Task<ActionResult> AddCommunitySecterKey(string SecterKey)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            string status = null;
            CommunitySecretCodesController communitySecretKeysController = new CommunitySecretCodesController();
            CommunitySecretCodes communitySecretKeys = new CommunitySecretCodes();
            Response<CommunitySecretCodes> responceGetCommunitySecretKeys = new Response<CommunitySecretCodes>();
            communitySecretKeys.secretCode = SecterKey;
            communitySecretKeys.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
            responceGetCommunitySecretKeys = await communitySecretKeysController.GetCommunitySecretKeysBySecretCode(SecterKey);
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            responceCommunitySecretKeys = null;
            {
                if (responceGetCommunitySecretKeys.model == null)
                {
                   
                    responceCommunitySecretKeys = await communitySecretKeysController.PostCommunitySecretCodes(communitySecretKeys);
                    status = "Success";
                }
                else
                {
                    status = "This SecretCode is Already Exists";
                }
            }
            
            return Json(new { ResponceCommunitySecretKeys = responceCommunitySecretKeys,Status=status });
        }

        public async Task<ActionResult> UpdateCommunitySecterKey(string SecterKey, int Id)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunitySecretCodesController communitySecretKeysController = new CommunitySecretCodesController();
            Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
            responceCommunitySecretKeys = await communitySecretKeysController.GetCommunitySecretCode(Id);
            responceCommunitySecretKeys.model.secretCode = SecterKey;
            responceCommunitySecretKeys = await communitySecretKeysController.PutCommunitySecretCodes(Id, responceCommunitySecretKeys.model);
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> DeleteCommunitySecterKey(int Id)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunitySecretCodesController communitySecretKeysController = new CommunitySecretCodesController();
            Response<CommunitySecretCodes> responceCommunityStreetFloor = new Response<CommunitySecretCodes>();
            await communitySecretKeysController.DeleteCommunitySecretKeys(Id);
            return Json(new { success = "Success" });
        }

                        //-------------------------EmergencyContact Controllers----------------------

        public async Task<ActionResult> AddCommunityEmergencyContact(string SelectedWorkinghoursFrom,string SelectedWorkinghoursTo, string EmergencyContact, int EmergencyContactsLength, string EmergencyContactName)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunitiesController communitiesController = new CommunitiesController();
            Response<Community> communityResponse = new Response<Community>();
            communityResponse = communitiesController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            CommunityEmergencyContacts communityEmergencyContacts = new CommunityEmergencyContacts();
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContact = new Response<CommunityEmergencyContacts>();
            string status = "fail";
            if (EmergencyContactsLength < communityResponse.model.emergencyContactRange)
            {
                string contact = EmergencyContact;
                communityEmergencyContacts.contact = contact;
                communityEmergencyContacts.name = EmergencyContactName;
                communityEmergencyContacts.workingHourStart = SelectedWorkinghoursFrom;
                communityEmergencyContacts.workingHourEnd = SelectedWorkinghoursTo;
                communityEmergencyContacts.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
                responceCommunityEmergencyContact = await communityEmergencyContactsController.PostCommunityEmergencyContacts(communityEmergencyContacts);
                status = "Success";
            }
        
         
            //if (EmergencyContact != null)
            //{
            //    string[] contactArrayPlus = contact.Split('+');
            //    contact = contactArrayPlus[1];
            //}

            return Json(new { ResponceCommunityEmergencyContact = responceCommunityEmergencyContact, Status = status });
        }

        public async Task<ActionResult> UpdateCommunityEmergencyContact(string SelectedWorkinghoursFrom,string SelectedWorkinghoursTo,string EmergencyContact, int Id, string EmergencyContactName)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
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
            CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            responceCommunityEmergencyContacts = await communityEmergencyContactsController.GetCommunityEmergencyContacts(Id);
            responceCommunityEmergencyContacts.model.contact = contact;
            responceCommunityEmergencyContacts.model.name = EmergencyContactName;
            responceCommunityEmergencyContacts.model.workingHourStart = SelectedWorkinghoursFrom;
            responceCommunityEmergencyContacts.model.workingHourEnd = SelectedWorkinghoursTo;
            responceCommunityEmergencyContacts = await communityEmergencyContactsController.PutCommunityEmergencyContacts(Id, responceCommunityEmergencyContacts.model);
            return Json(new { success = "Success" });
        }
        public async Task<ActionResult> DeleteEmergencyContact(int Id)
        {
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";

                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            CommunityEmergencyContactsController communityEmergencyContactsController = new CommunityEmergencyContactsController();
            Response<CommunityEmergencyContacts> responceCommunityEmergencyContacts = new Response<CommunityEmergencyContacts>();
            await communityEmergencyContactsController.DeleteCommunityEmergencyContacts(Id);
            return Json(new { success = "Success" });
        }



        public async Task<ActionResult> ChangeCommunityCover(FormCollection data)
        {
           
            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
              Response<Community> communityResponse = new Response<Community>();
            CommunitiesController communityController = new CommunitiesController();
            communityResponse = communityController.GetCommunitybyID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
                
            if(communityResponse!=null){
                if (Request.Files["coverfile"] != null)
                {
                    var postedFile = Request.Files["coverfile"];
                    if (postedFile.ContentLength != 0)
                    {

                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertImageForBlob();
                        communityResponse.model.coverImage= blobImageURL;
                       communityResponse= await communityController.PutCommunity(Convert.ToInt32(Session["loginCommunityID"].ToString()),communityResponse.model);

                    }

                }
            }
            return Json(new { success = "Success", ImageString = communityResponse.model.coverImage });

            }




        public async Task<ActionResult> ChangeFeatureImage(FormCollection data)
        {

            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<CommunityImage> communityResponse = new Response<CommunityImage>();
            CommunityImagesController communityImageController = new CommunityImagesController();
            List<CommunityImage> listCommunityImage = new List<CommunityImage>();
            CommunityImage communityImage = new CommunityImage();
            listCommunityImage = communityImageController.GetCommunityImagesbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
            int id = Convert.ToInt32(data["Id"].ToString());
            if (id <= 0)
            {
                if (Request.Files["Featurefile"] != null)
                {
                    var postedFile = Request.Files["Featurefile"];
                    if (postedFile.ContentLength != 0)
                    {

                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertImageForBlob();
                        communityImage.image = blobImageURL;
                        communityImage.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
                        communityResponse = await communityImageController.PostCommunityImage(communityImage);

                    }

                }
            }
            else
            {
                int imageID = Convert.ToInt32(data["Id"].ToString());
                if (Request.Files["Featurefile"] != null)
                {
                    var postedFile = Request.Files["Featurefile"];
                    if (postedFile.ContentLength != 0)
                    {
                        communityResponse = await communityImageController.GetCommunityImagebyID(imageID);
                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertImageForBlob();
                        communityResponse.model.image = blobImageURL;
                        //communityImage.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
                        communityResponse = await communityImageController.PutCommunityImage(imageID, communityResponse.model);

                    }

                }
            }
            return Json(new { CommunityResponse = communityResponse, });

        }

        //public async Task<ActionResult> ChangeFeatureImage(FormCollection data)
        //{

        //    if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
        //    {
        //        TempData["error"] = "Null";
        //        return RedirectToAction("Index", "Home");
        //    }
        //     Session.Timeout = 1000;
        //    Response<CommunityImage> communityResponse = new Response<CommunityImage>();
        //    CommunityImagesController communityImageController = new CommunityImagesController();
        //    List<CommunityImage> listCommunityImage = new List<CommunityImage>();
        //    CommunityImage communityImage = new CommunityImage();
        //    listCommunityImage = communityImageController.GetCommunityImagesbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
        //        if (Request.Files["Featurefile"] != null)
        //        {
        //            var postedFile = Request.Files["Featurefile"];
        //            if (postedFile.ContentLength != 0)
        //            {
                      
        //                imageForBlob imageForBlob = new imageForBlob();
        //                string blobImageURL = imageForBlob.ConvertImageForBlob();
        //                communityImage.image = blobImageURL;
        //                communityImage.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
        //                communityResponse = await communityImageController.PostCommunityImage(communityImage);

        //            }

        //        }
     
        //    return Json(new { CommunityResponse = communityResponse, });

        //}
        public async Task<ActionResult> AddFeatureImage(FormCollection data)
        {

            if (Session["loginUserID"] == null || Session["loginCommunityID"] == null)
            {
                TempData["error"] = "Null";
                return RedirectToAction("Index", "Home");
            }
            Session.Timeout = 1000;
            Response<CommunityImage> communityResponse = new Response<CommunityImage>();
            CommunityImagesController communityImageController = new CommunityImagesController();
            List<CommunityImage> listCommunityImage = new List<CommunityImage>();
            CommunityImage communityImage = new CommunityImage();
            listCommunityImage = communityImageController.GetCommunityImagesbyCommunityID(Convert.ToInt32(Session["loginCommunityID"].ToString()));
         
            if (Request.Files["addFeatureImagefile"] != null)
            {
                var postedFile = Request.Files["addFeatureImagefile"];
                if (postedFile.ContentLength != 0)
                {

                    imageForBlob imageForBlob = new imageForBlob();
                    string blobImageURL = imageForBlob.ConvertImageForBlob();
                    communityImage.image = blobImageURL;
                    communityImage.communityID = Convert.ToInt32(Session["loginCommunityID"].ToString());
                    communityResponse = await communityImageController.PostCommunityImage(communityImage);

                }

            }

            return Json(new { communityResponse = communityResponse, countImages = listCommunityImage.Count });

        }



        //-------------------------Terms and Privacy Controllers----------------------
        public async Task<ActionResult> Terms()
        {

            return View();

        }
        public async Task<ActionResult> privacy()
        {

            return View();

        }



    }
}



