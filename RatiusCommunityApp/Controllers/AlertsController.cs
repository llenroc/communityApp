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
using System.Collections.Specialized;

namespace RatiusCommunityApp.Controllers
{
      //[Authorize]
    public class AlertsController : ApiController
    {
        private static string tokenUri = "http://gateway.onewaysms.com.my:10001/api.aspx?";
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Alerts
        public IQueryable<Alert> GetAlerts()
        {
            return db.Alerts;
        }


        // GET: api/Alerts/5
        [ResponseType(typeof(Alert))]
        public async Task<Response<Alert>> GetAlertbyID(int id)
        {
            Response<Alert> responseAlerts = new Response<Alert>();
            Alert alert = new Alert();
            alert = db.Alerts.Where(x => x.id == id).FirstOrDefault();
            if (alert == null)
            {
                responseAlerts.status = "No Alerts";
                responseAlerts.model = null;
                return responseAlerts;
            }
            responseAlerts.status = "Success";
            responseAlerts.model = alert;
            return responseAlerts;
        }

        [ResponseType(typeof(Alert))]
        public async Task<Response<List<Alert>>> GetAlertByCommunityID(int communityID)
        {
            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            List<Alert> alerts = new List<Alert>();
            alerts = db.Alerts.Where(x => x.communityID == communityID).Include(x => x.user).Include(y => y.member).ToList();
            if (alerts == null)
            {
                responseAlerts.status = "No Alerts";
                responseAlerts.model = null;
                return responseAlerts;
            }
            responseAlerts.status = "Success";
            responseAlerts.model = alerts;
            return responseAlerts;
        }



        public async Task<Response<List<Alert>>> GetCountUnReadAlertByCommunityID(int communityID,int dumbWeb)
        {
            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            List<Alert> alerts = new List<Alert>();
            alerts = db.Alerts.Where(x => x.communityID == communityID && x.isViewed==false).Include(x=>x.user).ToList();
            if (alerts == null)
            {
                responseAlerts.status = "No new Alerts";
                responseAlerts.model = null;
                return responseAlerts;
            }
            responseAlerts.status = "Success";
            responseAlerts.model = alerts;
            return responseAlerts;
        }

        public async Task<Response<List<Alert>>> GetUnReadAlertByCommunityID(int communityID, int dumbWeb1)
        {
            Response<List<Alert>> responseAlerts = new Response<List<Alert>>();
            List<Alert> alerts = new List<Alert>();
            alerts = db.Alerts.Where(x => x.communityID == communityID).Include(x => x.user).OrderByDescending(x=>x.id).Take(4).ToList();
            if (alerts == null)
            {
                responseAlerts.status = "No new Alerts";
                responseAlerts.model = null;
                return responseAlerts;
            }
            responseAlerts.status = "Success";
            responseAlerts.model = alerts;
            return responseAlerts;
        }
        // PUT: api/Alerts/5
        [ResponseType(typeof(void))]
        public async Task<Response<Alert>> PutAlert(int id, Alert alert)
        {
            Response<Alert> responseAlerts = new Response<Alert>();
            if (!ModelState.IsValid)
            {
                responseAlerts.status = "Failuer";
                responseAlerts.model = null;
                return responseAlerts;
            }

            if (id != alert.id)
            {
                responseAlerts.status = "Failed: id did not match";
                responseAlerts.model = null;
                return responseAlerts;
            }

            db.Entry(alert).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertExists(id))
                {
                    responseAlerts.status = "Failed: id did not exists";
                    responseAlerts.model = null;
                    return responseAlerts;
                }
                else
                {
                    throw;
                }
            }

            responseAlerts.status = "Success";
            responseAlerts.model = alert;
            return responseAlerts;
        }

        // POST: api/Alerts
        [ResponseType(typeof(Alert))]
        public async Task<Response<Alert>> PostAlert(int communityID, int UserID, string number1, string number2, string number3)
        {
            number1 = number1.Trim();
            number2 = number2.Trim();
            number3 = number3.Trim();
            Alert alert=new Alert();
            Response<Alert> responceAlert = new Response<Alert>();
            if (!ModelState.IsValid)
            {
                responceAlert.status = "Failuer";
                responceAlert.model = null;
                return responceAlert;
            }
            alert.communityID=communityID;
            alert.userID=UserID;



            DateTime ServerDateTime = DateTime.Now;
            DateTime utcDateTime = ServerDateTime.ToUniversalTime();

            // ID from: 
            // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
            // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
            string malayTimeZoneKey = "Singapore Standard Time";
            TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
            DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);


            alert.date = malayDateTime;
            alert.isViewed = false;
            MembersController memberController=new MembersController();
            Response<Member> responceMember=new Response<Member>();
            responceMember=await memberController.GetCommunityMember(communityID,UserID);
            if (responceMember == null)
            {
                responceAlert.status = "Failuer: responceMember not found";
                responceAlert.model = null;
                return responceAlert;
            }
            alert.memberID = responceMember.model.id;




            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
            List<CommunityEmergencyContacts> CommunityContactsList = new List<CommunityEmergencyContacts>();
            CommunityContactsList=(from l in db.CommunityEmergencyContacts
                                       where l.communityID == communityID
                                       select l).ToList();
      
            UsersController userController=new UsersController();
            Response<User> userResponse = new Response<User>();
            userResponse = userController.GetCommunityUserbyID(UserID, 1);
            //if (userResponse == null)
            //{
            //    responceAlert.status = "Failuer: userResponse not found";
            //    responceAlert.model = null;
            //    return responceAlert;
            //}
            //if (userResponse.model == null)
            //{
            //    responceAlert.status = "Failuer: userResponse.model not found";
            //    responceAlert.model = null;
            //    return responceAlert;
            //}
            using (var wb = new WebClient())
            {


                if (number1 != "no")
                {

                    bool plusCheck = number1.Contains("+");
                        if (plusCheck == true)
                        {
                            string[] contactArrayPlus = number1.Split('+');
                            number1 = contactArrayPlus[1];
                            bool isNumeric = number1.All(char.IsDigit);
                            if (isNumeric == true)
                            {
                                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactNumber1 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                            }
                        }
                        else
                        {
                            char[] num1 = number1.Take(2).ToArray();
                            if (num1[0] != '0' && num1[1] != '0')
                            {
                                number1 = "00" + number1;
                            }
                            int lenth = number1.Length;
                            string contactArrayPlus = number1.Substring(2);
                            number1 = contactArrayPlus;
                            bool isNumeric = number1.All(char.IsDigit);
                            if (isNumeric == true)
                            {
                                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number1 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactNumber1 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                            }
                        }

                    

                }



                if (number2 != "no")
                {


                    bool plusCheck = number2.Contains("+");
                    if (plusCheck == true)
                    {
                        string[] contactArrayPlus = number2.Split('+');
                        number2 = contactArrayPlus[1];
                        bool isNumeric = number2.All(char.IsDigit);
                        if (isNumeric == true)
                        {
                            var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number2 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                            //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactnumber2 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                        }
                    }
                    else
                    {
                        char[] num2 = number2.Take(2).ToArray();
                            if (num2[0] != '0' && num2[1] != '0')
                            {
                                number2 = "00" + number2;
                            }
                        int lenth = number2.Length;
                        string contactArrayPlus = number2.Substring(2);
                        number2 = contactArrayPlus;
                        bool isNumeric = number2.All(char.IsDigit);
                        if (isNumeric == true)
                        {
                            var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number2 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                            //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactnumber2 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                        }
                    }
                }
                    if (number3 != "no")
                    {
                        bool plusCheck = number3.Contains("+");
                        if (plusCheck == true)
                        {
                            string[] contactArrayPlus = number3.Split('+');
                            number3 = contactArrayPlus[1];
                            bool isNumeric = number3.All(char.IsDigit);
                            if (isNumeric == true)
                            {
                                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number3 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactnumber3 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                            }
                        }
                        else
                        {
                            char[] num3 = number3.Take(2).ToArray();
                            if (num3[0] != '0' && num3[1] != '0')
                            {
                                number3 = "00" + number3;
                            }
                            int lenth = number3.Length;
                            string contactArrayPlus = number3.Substring(2);
                            number3 = contactArrayPlus;
                            bool isNumeric = number3.All(char.IsDigit);
                            if (isNumeric == true)
                            {
                                var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + number3 + "&senderid=onewaysms&languagetype=1&message= SECURITY ALERT TRIGGERED! %0A NAME : " + userResponse.model.firstName + " " + userResponse.model.lastName + " %0A ADDRESS : " + responceMember.model.address + " " + responceMember.model.streetFloor + " %0A Contact : " + userResponse.model.contact + "");
                                //var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=" + userResponse.model.emergencyContactnumber3 + "&senderid=onewaysms&languagetype=1&message=This Alert Notification is sent by " + userResponse.model.firstName + "");
                            }
                        }
                    }
                
               }

            //using (var wb = new WebClient())
            //{
            //    var response = wb.DownloadString("http://gateway.onewaysms.com.my:10001/api.aspx?apiusername=API112A4611M6&apipassword=API112A4611M6EP9PC&mobileno=60126658836,923315254672,923125199654,923105224222&senderid=onewaysms&languagetype=1&message=Testing SMS API Hi Vineet");
            //}
            responceAlert.status = "Success";
            responceAlert.model = alert;
            return responceAlert;
        }

        // DELETE: api/Alerts/5
        [ResponseType(typeof(Alert))]
        public async Task<IHttpActionResult> DeleteAlert(int id)
        {
            Alert alert = await db.Alerts.FindAsync(id);
            if (alert == null)
            {
                return NotFound();
            }

            db.Alerts.Remove(alert);
            await db.SaveChangesAsync();

            return Ok(alert);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlertExists(int id)
        {
            return db.Alerts.Count(e => e.id == id) > 0;
        }
    }
}