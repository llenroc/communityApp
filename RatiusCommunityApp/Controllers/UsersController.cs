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
using System.Net.Mail;

namespace RatiusCommunityApp.Controllers
{
    //[Authorize]
    public class UsersController : ApiController
    {

        //private static string accountRegisterUri = "http://localhost:53173/api/Account/Register";
        //private static string tokenUri = "http://localhost:53173/token";

        private static string accountRegisterUri = "http://ratiuscommunity.azurewebsites.net/api/Account/Register";
        private static string tokenUri = "http://ratiuscommunity.azurewebsites.net/token";
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();
     
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }
        [ResponseType(typeof(tokenDTO<User>))]
        public async Task<tokenDTO<User>> GetUserbyID(int Id)
        {
            tokenDTO<User> Response = new tokenDTO<User>();
            User user = await db.Users.FindAsync(Id);
          
            if (user == null)
            {
                Response.token = "Null";
                Response.status = "Failed: User does not exist";
                Response.model = null;
                return Response;
            }
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                 {
                  { "grant_type", "password" },
                   { "userName", user.emailID},
                   { "password", user.password }
                  };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(tokenUri, content);

                var responseString = await response.Content.ReadAsStringAsync();
                Response.token = responseString;
            }

            Response.status = "Success";
            Response.model = user;
            return Response;
        }
        [ResponseType(typeof(tokenDTO<User>))]
        public Response<User> GetCommunityUserbyID(int Id,int dummyidforwebModal)
        {
            Response<User> userResponse = new Response<User>();
            User user =  db.Users.Find(Id);

            if (user == null)
            {
                userResponse.status = "Failed: User does not exist";
                userResponse.model = null;
                return userResponse;
            }


            userResponse.status = "Success";
            userResponse.model = user;
            return userResponse;
        }
        // GET: api/Users/5
     
        [ResponseType(typeof(tokenDTO<SignInDTO1>))]
        public async Task<tokenDTO<SignInDTO1>> GetUser(string email, string password)
        {
            tokenDTO<SignInDTO1> Response = new tokenDTO<SignInDTO1>();
            User user = null;
            var userEmail =await (from u in db.Users
                            where u.emailID == email
                              select u).FirstOrDefaultAsync();


             if (userEmail == null)
            {
                Response.token = "Null";
                Response.status = "Failed: Incorrect Email address";
                Response.model = null;
                return Response;
            }
             else
             {
                 var userPassword = await (from u in db.Users
                                           where u.emailID == email && u.password == password
                                        select u).FirstOrDefaultAsync();
                 if (userPassword == null)
                 {
                     Response.token = "Null";
                     Response.status = "Failed: Incorrect Password";
                     Response.model = null;
                     return Response;
                 }
                 else
                 {
                     user = userPassword; 
                 }
             }
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                 {
                  { "grant_type", "password" },
                   { "userName", email },
                   { "password", password }
                  };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(tokenUri, content);

                var responseString = await response.Content.ReadAsStringAsync();
               string[] tokenString=responseString.Split('"');
               Response.token = tokenString[3];

               SignInDTO1 signInDto = new SignInDTO1();
               user.Islogout = false;
               db.Entry(user).State = EntityState.Modified;
               await db.SaveChangesAsync();
               signInDto.user = user;
              var communityList = db.Members.Where(x => x.userId == user.userID).ToList();
              List<CommunityDTO1> comList = new List<CommunityDTO1>();
          
                foreach (var community in communityList)
              {
                  CommunityDTO1 cdto = new CommunityDTO1();
                  cdto.communityId = community.communityID;
                  cdto.adminId = db.Communities.Where(x => x.communityID == community.communityID).Select(x=>x.adminUserID).FirstOrDefault();
                  cdto.userAddress = db.Members.Where(x => x.communityID == community.communityID && x.userId==user.userID).Select(x => x.address).FirstOrDefault();
                  cdto.adminContactNumber = db.CommunityEmergencyContacts.Where(x => x.communityID == community.communityID).Select(x => x.contact).FirstOrDefault();
                    comList.Add(cdto);
                }

                signInDto.community = comList;
                Response.status = "Success";
            Response.model = signInDto;
            return Response;
            }
          
        }

        // PUT: api/Users/5
    
        [ResponseType(typeof(Response<User>))]
        public async Task<Response<User>> PutUser(string email, User user)
        {
            Response<User> userResponse = new Response<User>();
            if (!ModelState.IsValid)
            {
                userResponse.status = "Failure";
                userResponse.model = null;
                return userResponse;
            }

            if (email != user.emailID)
            {
                userResponse.status = "Failed: Wrong Email ID";
                userResponse.model = null;
                return userResponse;
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                userResponse.status = "Success";
                using (var client = new HttpClient())
                {
                    var responseString = client.GetStringAsync("http://www.example.com/recepticle.aspx");
                }
                userResponse.model = user;
             
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.emailID))
                {
                    userResponse.status = "Failed: User does not exist";
                    userResponse.model = null;
                    return userResponse;
                }
                else
                {
                    throw;
                }
            }

            return userResponse;
        }


        public async Task<IHttpActionResult> PutUserImage(int userID)
        {
            UsersController userController=new UsersController();
                 Response<User> userResponse = new Response<User>();
                 userResponse = userController.GetCommunityUserbyID(userID, 1);

                 if (userResponse.model == null)
                 {
                      userResponse.status = "Failed: No User Found";
                     userResponse.model = null;
                     return Ok(userResponse);
                 }

                 User user = new User();
                 user = userResponse.model;
         
            
            var httpRequest = HttpContext.Current.Request;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(provider);

              
     
              
                imageForBlob imageForBlob = new imageForBlob();
                string blobImageURL = imageForBlob.ConvertImageForBlob();
                user.image = blobImageURL;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();







                userResponse.status = "Success";

                userResponse.model = user;
                return Ok(userResponse);

            }
            else
            {
               
                userResponse.status = "Failed: Not Multipart Content";
                userResponse.model = null;
                return Ok(userResponse);
            }
        }
        public async Task<Response<MobSettingDTO>> PutUserData(MobSettingDTO mobSettingDTO)
        {
            try { 
            UsersController userController = new UsersController();
            Response<User> userResponse = new Response<User>();
             Response<MobSettingDTO> MobSettingDTOResponse = new Response<MobSettingDTO>();
           
            MembersController memberController = new MembersController();
           
           
            userResponse = userController.GetCommunityUserbyID(mobSettingDTO.userID, 1);
            if (userResponse.model == null)
            {
                MobSettingDTOResponse.status = "Failed: No User Found";
                MobSettingDTOResponse.model = null;
                return MobSettingDTOResponse;
            }
            User user = new User();
            user = userResponse.model;
            if (mobSettingDTO.EmailID != null)
            {
                user.emailID = mobSettingDTO.EmailID;
            }
            if (mobSettingDTO.password != null)
            {
                user.password = mobSettingDTO.password;
            }
            if (mobSettingDTO.emergencyContactName1 != null)
            {
                user.emergencyContactName1 = mobSettingDTO.emergencyContactName1;
            }
            if (mobSettingDTO.emergencyContactName2 != null)
            {
                user.emergencyContactName2 = mobSettingDTO.emergencyContactName2;
            }
            if (mobSettingDTO.emergencyContactNumber1 != null)
            {
                user.emergencyContactNumber1 = mobSettingDTO.emergencyContactNumber1;
            }

            if (mobSettingDTO.emergencyContactNumber2 != null)
            {
                user.emergencyContactNumber2 = mobSettingDTO.emergencyContactNumber2;
            }
            if (mobSettingDTO.language != null)
            {
                user.language = mobSettingDTO.language;
            }
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (mobSettingDTO.communityList != null)
            {
                try
                {
                    foreach (var item in mobSettingDTO.communityList)
                    {

                        Response<Member> memberResponse = new Response<Member>();
                        memberResponse = await memberController.GetCommunityMember(item.communityID, mobSettingDTO.userID);
                        Member member = new Member();
                        member = memberResponse.model;
                        if (item.address != null)
                        {
                            member.address = item.address;
                        }
                        if (item.streetFloor != null)
                        {
                            member.streetFloor = item.streetFloor;
                        }

                        db.Entry(member).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    MobSettingDTOResponse.status = ex.Message;
                    MobSettingDTOResponse.model = null;
                    return MobSettingDTOResponse;
                }
            }
            else
            {
                MobSettingDTOResponse.status = "Success Without Community List update";
                MobSettingDTOResponse.model = mobSettingDTO;
                return MobSettingDTOResponse;
            }
            Response<Notification> responceNotification = new Response<Notification>();
            NotificationsController notificationController = new NotificationsController();
            responceNotification = await notificationController.GetNotificationbyUserID(mobSettingDTO.userID);
            Notification notification = new Notification();
                notification=responceNotification.model;
            notification.Messages = mobSettingDTO.Messages;
            notification.Notices= mobSettingDTO.Notices;
            notification.Report= mobSettingDTO.Report;
            responceNotification = await notificationController.PutNotification(mobSettingDTO.userID, notification);
            MobSettingDTOResponse.status = "Success";
            MobSettingDTOResponse.model = mobSettingDTO;
            return MobSettingDTOResponse;
            }
            catch (Exception ex)
            {
                Response<MobSettingDTO> MobSettingDTOResponse = new Response<MobSettingDTO>();
                MobSettingDTOResponse.status = ex.Message;
                MobSettingDTOResponse.model = null;
                return MobSettingDTOResponse;
            }
        }



        public async Task<Response<MobSettingDTOAndroid>> PutUserData(MobSettingDTOAndroid mobSettingDTOAndroid, int dummy)
        {
            try
            {
                UsersController userController = new UsersController();
                Response<User> userResponse = new Response<User>();
                Response<MobSettingDTOAndroid> MobSettingDTOAndroidResponse = new Response<MobSettingDTOAndroid>();

                MembersController memberController = new MembersController();


                userResponse = userController.GetCommunityUserbyID(mobSettingDTOAndroid.userID, 1);
                if (userResponse.model == null)
                {
                    MobSettingDTOAndroidResponse.status = "Failed: No User Found";
                    MobSettingDTOAndroidResponse.model = null;
                    return MobSettingDTOAndroidResponse;
                }
                User user = new User();
                user = userResponse.model;
                if (mobSettingDTOAndroid.EmailID != null)
                {
                    user.emailID = mobSettingDTOAndroid.EmailID;
                }
                if (mobSettingDTOAndroid.password != null)
                {
                    user.password = mobSettingDTOAndroid.password;
                }
                if (mobSettingDTOAndroid.emergencyContactName1 != null)
                {
                    user.emergencyContactName1 = mobSettingDTOAndroid.emergencyContactName1;
                }
                if (mobSettingDTOAndroid.emergencyContactName2 != null)
                {
                    user.emergencyContactName2 = mobSettingDTOAndroid.emergencyContactName2;
                }
                if (mobSettingDTOAndroid.emergencyContactNumber1 != null)
                {
                    user.emergencyContactNumber1 = mobSettingDTOAndroid.emergencyContactNumber1;
                }

                if (mobSettingDTOAndroid.emergencyContactNumber2 != null)
                {
                    user.emergencyContactNumber2 = mobSettingDTOAndroid.emergencyContactNumber2;
                }
                if (mobSettingDTOAndroid.language != null)
                {
                    user.language = mobSettingDTOAndroid.language;
                }

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (mobSettingDTOAndroid.communityList !=null)
                {
                    foreach (var item in mobSettingDTOAndroid.communityList)
                    {
                        Response<Member> memberResponse = new Response<Member>();
                        memberResponse = await memberController.GetCommunityMember(item.communityID, mobSettingDTOAndroid.userID);
                        Member member = new Member();
                        member = memberResponse.model;
                        if (item.address != null)
                        {
                            member.address = item.address;
                        }
                        if (item.streetFloor != null)
                        {
                            member.streetFloor = item.streetFloor;
                        }

                        db.Entry(member).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                Response<Notification> responceNotification = new Response<Notification>();
                NotificationsController notificationController = new NotificationsController();
                responceNotification = await notificationController.GetNotificationbyUserID(mobSettingDTOAndroid.userID);
                Notification notification = new Notification();
                notification = responceNotification.model;
                if (mobSettingDTOAndroid.Messages == null)
                {
                    notification.Messages = notification.Messages;
                }
                if(mobSettingDTOAndroid.Messages=="true"){
                    notification.Messages = true;
                }
                 if(mobSettingDTOAndroid.Messages=="false"){
                     notification.Messages = false;
                }






                 if (mobSettingDTOAndroid.Notices == null)
                 {
                     notification.Notices = notification.Notices;
                 }
                 if (mobSettingDTOAndroid.Notices == "true")
                 {
                     notification.Notices = true;
                 }
                 if (mobSettingDTOAndroid.Notices == "false")
                 {
                     notification.Notices = false;
                 }


                 if (mobSettingDTOAndroid.Report == null)
                 {
                     notification.Report = notification.Report;
                 }
                 if (mobSettingDTOAndroid.Report == "true")
                 {
                     notification.Report = true;
                 }
                 if (mobSettingDTOAndroid.Report == "false")
                 {
                     notification.Report = false;
                 }
               
                responceNotification = await notificationController.PutNotification(mobSettingDTOAndroid.userID, notification);
                responceNotification.model = notification;
                MobSettingDTOAndroidResponse.status = "Success";
                MobSettingDTOAndroidResponse.model = mobSettingDTOAndroid;
                return MobSettingDTOAndroidResponse;
            }
            catch (Exception ex)
            {
                Response<MobSettingDTOAndroid> MobSettingDTOAndroidResponse = new Response<MobSettingDTOAndroid>();
                MobSettingDTOAndroidResponse.status = ex.Message;
                MobSettingDTOAndroidResponse.model = null;
                return MobSettingDTOAndroidResponse;
            }
        }




        public async Task<Response<User>> PostForgotPassword(string userEmailID)
        {
            Response<User> responceUser = new Response<User>();
            User user = new User();
             user = await (from u in db.Users
                             where u.emailID == userEmailID 
                             select u).FirstOrDefaultAsync();
             string password = user.password;
             if (user != null)
             {


                 using (MailMessage mm = new MailMessage("unitedneighbourhoodsapp@gmail.com", userEmailID))
                 {
                     mm.Subject = "Password ";
                     mm.Body = "You Password is" + password;
                     //mm.Attachments.Add(new Attachment(stream, "Report.xls"));
                     mm.IsBodyHtml = true;
                     SmtpClient smtp = new SmtpClient();
                     smtp.Host = "smtp.gmail.com";
                     NetworkCredential credential = new NetworkCredential();
                     credential.UserName = "unitedneighbourhoodsapp@gmail.com";
                     credential.Password = "neighbourhoods";
                     smtp.UseDefaultCredentials = true;
                     smtp.Credentials = credential;
                     smtp.Port = 587;
                     smtp.EnableSsl = true;
                     smtp.Send(mm);
                 }
                 responceUser.status = "Success";
                     responceUser.model=user;
                 return responceUser;
             }
             else
             {
                 responceUser.status = "Failed: E-mail did not match";
                 responceUser.model = null;
                 return responceUser;
             }
        }




        public async Task<Response<string>> PostForgotPasswordforCommunityAdmin(string userEmailID, string communityName)
        {
            Response<string> responceUser = new Response<string>();
            List<User> usersList = new List<User>();
            usersList = (from u in db.Users
                          where u.emailID == userEmailID
                          select u).ToList();
 
            if (usersList != null)
            {
                Community community = new Community();
                community = null;
                
                
                foreach (var item in usersList) 
                {
                int userid = item.userID;
                Community dbCommunity = new Community();
                dbCommunity = await db.Communities.Where(x => x.adminUserID == userid && x.name == communityName).FirstOrDefaultAsync();
                if (dbCommunity != null)
                {
                    community = dbCommunity;
                }
                }




                if (community != null)
                {
                    string password = community.communityPassword;
                    using (MailMessage mm = new MailMessage("unitedneighbourhoodsapp@gmail.com", userEmailID))
                    {
                        mm.Subject = "Password ";
                        mm.Body = "You Password is " + password;
                        //mm.Attachments.Add(new Attachment(stream, "Report.xls"));
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        NetworkCredential credential = new NetworkCredential();
                        credential.UserName = "unitedneighbourhoodsapp@gmail.com";
                        credential.Password = "neighbourhoods";
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credential;
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(mm);
                    }
                    responceUser.status = "Success";
                    responceUser.model = "Email send Successfully";
                    return responceUser;
                }
                else
                {
                    responceUser.status = "Failed: E-mail or CommunityName did not match";
                    responceUser.model = null;
                    return responceUser;
                }
             
            }
            else
            {
                responceUser.status = "Failed: E-mail or CommunityName did not match";
                responceUser.model = null;
                return responceUser;
            }
        }

        public async Task<Response<string>> PostForgotPasswordforSystemAdmin(string EmailID)
        {
            Response<string> responceUser = new Response<string>();
            User user = new User();
            user =await (from u in db.Users
                          where u.emailID == EmailID && u.isMainAdmin==true
                          select u).FirstOrDefaultAsync();
 
            if (user != null)
            {
                string password = user.password;
                    using (MailMessage mm = new MailMessage("unitedneighbourhoodsapp@gmail.com", EmailID))
                    {
                        mm.Subject = "Password ";
                        mm.Body = "You Password is " + password;
                        //mm.Attachments.Add(new Attachment(stream, "Report.xls"));
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        NetworkCredential credential = new NetworkCredential();
                        credential.UserName = "unitedneighbourhoodsapp@gmail.com";
                        credential.Password = "neighbourhoods";
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credential;
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(mm);
                    }
                    responceUser.status = "Success";
                    responceUser.model = "Email send Successfully";
                    return responceUser;
             
             
            }
            else
            {
                responceUser.status = "Failed: E-mail or CommunityName did not match";
                responceUser.model = null;
                return responceUser;
            }
        }
        // POST: api/Users
        [ResponseType(typeof(Response<UserMemberDTO>))]

        public async Task<IHttpActionResult> PostUser()
        {
            var httpRequest = HttpContext.Current.Request;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                User user = new User();
                Member member = new Member();

                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key.Equals("emailID"))
                        {
                            user.emailID = val;
                            //var userEmail = db.Users.Where(x => x.emailID == val).FirstOrDefault();
                            //if (userEmail == null)
                            //{
                            //    user.emailID = val;
                            //}
                            //else
                            //{
                            //    Response<User> userResponse = new Response<User>();
                            //    userResponse.status = "Failed: This Email Id Already Exists.";
                            //    userResponse.model = null;
                            //    return Ok(userResponse);
                            //}
                        }
                           

                        if (key.Equals("password"))
                            user.password = val;
                        if (key.Equals("firstName"))
                            user.firstName = val;
                        if (key.Equals("lastName"))
                            user.lastName = val;
                        if (key.Equals("country"))
                            user.country = val;
                        if (key.Equals("gender"))
                            user.gender = val;
                        if (key.Equals("contact"))
                            user.contact = val;
                        if (key.Equals("emergencyContactName1"))
                            user.emergencyContactName1 = val;
                        if (key.Equals("emergencyContactNumber1"))
                            user.emergencyContactNumber1 = val;
                        if (key.Equals("emergencyContactName2"))
                            user.emergencyContactName2 = val;
                        if (key.Equals("emergencyContactNumber2"))
                            user.emergencyContactNumber2 = val;
                            
                        if (key.Equals("address"))
                            member.address= val;
                        if (key.Equals("streetFloor"))
                            member.streetFloor = val;
                        if (key.Equals("communityID"))
                            member.communityID =Convert.ToInt32(val);

                        if (key.Equals("lat"))
                            user.lat =Convert.ToDouble(val);

                        if (key.Equals("lng"))
                            user.lng =Convert.ToDouble(val);
                        if (key.Equals("language"))
                            user.language = val;
                        if (key.Equals("userType"))
                            user.userType = val;
                    }

                }
                user.Islogout = false;
                user.isMainAdmin = false;
                member.isBlocked = false;
                member.isAlertBlocked = false;
                Response<UserMemberDTO> userMemberResponse = new Response<UserMemberDTO>();
                UserMemberDTO userMemberDto = new UserMemberDTO();
                if (!ModelState.IsValid)
                {
                    userMemberResponse.status = "Failure";
                    userMemberResponse.model = userMemberDto;
                    return Ok(userMemberResponse);
                }
                if (UserExists(user.emailID))
                {

                    userMemberResponse.status = "Failed: User Already Exist";

                    userMemberDto.user = await (from l in db.Users
                                           where l.emailID == user.emailID
                                           select l).FirstOrDefaultAsync();

                  
                    userMemberResponse.model = userMemberDto;
                    return Ok(userMemberResponse);
                }
                imageForBlob imageForBlob = new imageForBlob();
                string blobImageURL = imageForBlob.ConvertImageForBlob();
                user.image = blobImageURL;
                db.Users.Add(user);
                await db.SaveChangesAsync();
             
                AccountController accountController = new AccountController();
                RegisterBindingModel registerBindingModel = new RegisterBindingModel();
                registerBindingModel.Email = user.emailID;
                registerBindingModel.Password = user.password;
                registerBindingModel.ConfirmPassword = user.password;

                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                 {
                  { "Email", user.emailID },
                   { "Password", user.password },
                   { "ConfirmPassword", user.password }
                  };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(accountRegisterUri, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                }

                MembersController memberController = new MembersController();
                Response<Member> responseMember = new Response<Member>();
                userMemberDto.user = await (from l in db.Users
                                            where l.emailID == user.emailID
                                            select l).FirstOrDefaultAsync();

                member.userId = userMemberDto.user.userID;
               responseMember= await memberController.PostMember(member);
               responseMember =await memberController.GetMember(responseMember.model.id);


               Response<Notification> responceNotification = new Response<Notification>();
               NotificationsController notificationController = new NotificationsController();
               Notification notification = new Notification();
               notification.Messages = true;
               notification.Notices = true;
               notification.Report = true;
               notification.userID = userMemberDto.user.userID;
               responceNotification =await notificationController.PostNotification(notification);

                userMemberResponse.status = "Success";
                userMemberDto.member = member;
                userMemberDto.user = user;
                userMemberDto.AdminID = responseMember.model.community.adminUserID;
                userMemberResponse.model = userMemberDto;
                return Ok(userMemberResponse);

            }
            else
            {
                Response<User> userResponse = new Response<User>();
                userResponse.status = "Failed: Not Multipart Content";
                userResponse.model = null;
                return Ok(userResponse);
            }
        }


        public async Task<Response<User>> PostCommunityAdminUser(string name, string password)
        {
          

                User user = new User();
                user.password = password;
                user.firstName = name;
                Response<User> userResponse = new Response<User>();
                if (!ModelState.IsValid)
                {
                    userResponse.status = "Failure";
                    userResponse.model = null;
                    return userResponse;
                }
                //if (UserExists(user.emailID))
                //{
                //    userResponse.status = "Failed: Already Exist";
                //    var existUser = await (from l in db.Users
                //                           where l.emailID == user.emailID
                //                           select l).FirstOrDefaultAsync();
                //    userResponse.model = existUser;
                //    return userResponse;
                //}
              
                db.Users.Add(user);
                await db.SaveChangesAsync();
                var userFromDB = await (from u in db.Users
                                  where u.firstName == name && u.password == password
                                  select u).FirstOrDefaultAsync();
                 Response<Notification> responceNotification = new Response<Notification>();
               NotificationsController notificationController = new NotificationsController();
               Notification notification = new Notification();
               notification.Messages = false;
               notification.Notices = false;
               notification.Report = false;
               notification.userID = userFromDB.userID;
               responceNotification =await notificationController.PostNotification(notification);
      


                await db.SaveChangesAsync();
                userResponse.status = "Success";
                userResponse.model = user;
                return userResponse;
        }


        [Route("api/Putlogout")]
        public async Task<Response<User>> Putlogout(int userId)
        {
            Response<User> userResponse = new Response<User>();
            User user = new User();
            user = db.Users.Where(x => x.userID == userId).FirstOrDefault();
            if (user == null)
            {
                userResponse.status = "Failed: User does not exist";
                userResponse.model = null;
                return userResponse;
            }
            user.Islogout = true;
            db.Entry(user).State = EntityState.Modified;

          
                await db.SaveChangesAsync();
                userResponse.status = "Success";
                
                userResponse.model = user;

            

            return userResponse;
        }






        //public async Task<Response<User>> PostUser(User user)
        //{
        //    Response<User> userResponse = new Response<User>();
        //    if (!ModelState.IsValid)
        //    {
        //        userResponse.status = "Failure";
        //        userResponse.model = null;
        //        return userResponse;
        //    }
        //    if (UserExists(user.emailID))
        //    {
        //        userResponse.status = "User Already Exist";
        //        var existUser = await (from l in db.Users
        //                   where l.emailID == user.emailID
        //                   select l).FirstOrDefaultAsync();
        //        userResponse.model = existUser;
        //        return userResponse;
        //    }

        //    db.Users.Add(user);

        //    AccountController accountController = new AccountController();
        //    RegisterBindingModel registerBindingModel = new RegisterBindingModel();
        //    registerBindingModel.Email = user.emailID;
        //    registerBindingModel.Password = user.password;
        //    registerBindingModel.ConfirmPassword = user.password;

        //    using (var client = new HttpClient())
        //    {
        //        var values = new Dictionary<string, string>
        //         {
        //          { "Email", user.emailID },
        //           { "Password", user.password },
        //           { "ConfirmPassword", user.password }
        //          };

        //        var content = new FormUrlEncodedContent(values);

        //        var response = await client.PostAsync(accountRegisterUri, content);

        //        var responseString = await response.Content.ReadAsStringAsync();
        //    }

         
        //    await db.SaveChangesAsync();
        //    userResponse.status = "Success";
        //    userResponse.model = user;
        //    return userResponse;
        //}

        // DELETE: api/Users/5
  
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string email)
        {
            return db.Users.Count(e => e.emailID== email) > 0;
        }



        [ResponseType(typeof(tokenDTO<User>))]
        public tokenDTO<User> GetUserforNavBar(int Id,int? dummy)
        {
            tokenDTO<User> Response = new tokenDTO<User>();
            User user = db.Users.Find(Id);

            if (user == null)
            {
                Response.token = "Null";
                Response.status = "Failed: User does not exist";
                Response.model = null;
                return Response;
            }

            Response.status = "Success";
            Response.model = user;
            return Response;
        }
    }
}