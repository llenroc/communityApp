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
using Newtonsoft.Json.Linq;

namespace RatiusCommunityApp.Controllers
{
     [Authorize]
    public class CommunitiesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Communities
        public IQueryable<Community> GetCommunities()
        {
            return db.Communities;
        }
        [Route("GetFirstCommunity")]
        public async Task<List<Community>> GetAllCommunities()
        {
            List<Community> communitiesList = new List<Community>();
            communitiesList = db.Communities.ToList();
            return communitiesList;
        }
        public List<string> SearchCommunity(string searchstring)
        {
            List<string> communityList = new List<string>();
            communityList = db.Communities.Where(x => x.name.StartsWith(searchstring)).Select(e => e.name).Distinct().ToList();
            return communityList;
        }
        [Route("GetCommunityByName")]
        public async Task<Response<Community>> GetCommunityByName(string communityName)
        {
            Response<Community> responseCommunity = new Response<Community>();
            var community = await (from u in db.Communities
                                   where u.name == communityName
                                   select u).FirstOrDefaultAsync();
            if (community == null)
            {
                responseCommunity.status = "This Community Name is already Exists";
                responseCommunity.model = null;
                return responseCommunity;
            }
            responseCommunity.status = "Success";
            responseCommunity.model = community;
            return responseCommunity;
        }

        [ResponseType(typeof(tokenDTO<User>))]
        public async Task<Response<Community>> GetCommunityByNameAndPassword(string communityName, string password)
        {
            Response<Community> responseCommunity = new Response<Community>();
            var community = await (from u in db.Communities
                              where u.name == communityName && u.communityPassword == password
                              select u).Include(x=>x.user).FirstOrDefaultAsync();


            if (community == null)
            {
                responseCommunity.status = "Failed: Wrong Community Name or Password";
                responseCommunity.model = null;
                return responseCommunity;
            }

            //using (var client = new HttpClient())
            //{
            //    var values = new Dictionary<string, string>
            //     {
            //      { "grant_type", "password" },
            //       { "userName", email },
            //       { "password", password }
            //      };

            //    var content = new FormUrlEncodedContent(values);

            //    var response = await client.PostAsync(tokenUri, content);

            //    var responseString = await response.Content.ReadAsStringAsync();
            //    string[] tokenString = responseString.Split('"');
            //    responseCommunity.token = tokenString[3];


                
            //}
            responseCommunity.status = "Success";
            responseCommunity.model = community;
            return responseCommunity;
        }


       
          // GET: api/Communities/5
          [ResponseType(typeof(Response<Community>))]
          public Response<CommunityCompleteDTO> GetCommunitybyID(int communityID,int? Dummy)
          {
              Response<CommunityCompleteDTO> communityResponse = new Response<CommunityCompleteDTO>();
              CommunityCompleteDTO communityCompleteDTO = new CommunityCompleteDTO();
              if (!ModelState.IsValid)
              {
                  communityResponse.status = "Failure";
                  communityResponse.model = null;
                  return communityResponse;
              }


              communityCompleteDTO.community = (from l in db.Communities
                                                where l.communityID == communityID
                                                select l).Include(x => x.user).FirstOrDefault();

              if (communityCompleteDTO.community == null)
              {
                  communityResponse.status = "Failed: No Community Found";
                  communityResponse.model = null;
                  return communityResponse;
              }
              communityCompleteDTO.communityFeaturesImage = (from l in db.CommunityImages
                                                             where l.communityID == communityID
                                                             select l).ToList();
              communityCompleteDTO.communityEmergencyContacts = (from l in db.CommunityEmergencyContacts
                                                                 where l.communityID == communityID
                                                                 select l).OrderByDescending(x=>x.id).ToList();
              communityCompleteDTO.communityStreetFloor = (from l in db.CommunityStreetFloors
                                                           where l.communityID == communityID
                                                           select l).OrderByDescending(x=>x.id).ToList();
              communityCompleteDTO.communitySecretKeys = (from l in db.CommunitySecretKeys
                                                           where l.communityID == communityID
                                                           select l).OrderByDescending(x => x.id).ToList();


              communityResponse.status = "Success";
              communityResponse.model = communityCompleteDTO;
              return communityResponse; ;
          }





      
        // GET: api/Communities/5
        [ResponseType(typeof(Response<Community>))]
        public Response<Community> GetCommunitybyID(int communityID)
        {
            Response<Community> communityResponse = new Response<Community>();
            Community community = new Community();
            if (!ModelState.IsValid)
            {
                communityResponse.status = "Failure";
                communityResponse.model = null;
                return communityResponse;
            }


            community = (from l in db.Communities
                                   where l.communityID == communityID 
                                   select l).Include(x=>x.user).FirstOrDefault();
            if (community == null)
                {
                    communityResponse.status = "Failed: No Community Found";
                    communityResponse.model = null;
                    return communityResponse;
                }
          
                
                communityResponse.status = "Success";
                communityResponse.model = community;
            
               

            
         

            return communityResponse; ;
        }
      
        // GET: api/Communities/5


        //[ResponseType(typeof(Response<Community>))]
        //public async Task<Response<Community>> GetCommunity(string adminUserEmail, string password)
        //{
        //    Response<Community> communityResponse = new Response<Community>();
        //    User adminUser = new User();
        //    Community community = new Community();
        //    if (!ModelState.IsValid)
        //    {
        //        communityResponse.status = "Failure";
        //        communityResponse.model = null;
        //        return communityResponse;
        //    }

        //    if (UserExists(adminUserEmail))
        //    {
        //        adminUser = await (from l in db.Users
        //                               where l.emailID == adminUserEmail && l.password==password
        //                               select l).FirstOrDefaultAsync();
        //        if (adminUser == null)
        //        {
        //            communityResponse.status = "Failed: Incorrect Username or Password";
        //            communityResponse.model = null;
        //            return communityResponse;
        //        }
        //    }
        //    else
        //    {
        //        //communityResponse.status = "User does not exists";
        //        communityResponse.status = "Failed: Incorrect Username or Password";
        //        communityResponse.model = null;
        //        return communityResponse;
        //    }

        //    if (CommunityExists(adminUser.userID))
        //    {
        //        community = await (from l in db.Communities
        //                               where l.adminUserID == adminUser.userID
        //                               select l).FirstOrDefaultAsync();
        //        communityResponse.status = "Success";
        //        communityResponse.model = community;
            
        //    }
        //    else
        //    {
        //        communityResponse.status = "Failed: Incorrect Username or Password";
        //        communityResponse.model = null;
        //    }

        //   return communityResponse;;
        //}

        bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }
        public async Task<Response<CommunityWithFeatureImageDTO>> GetCommunityWithFeaturedImages(int communityID,int userID)
        {
            Response<CommunityWithFeatureImageDTO> responceCommunityWithFeaturedImage = new Response<CommunityWithFeatureImageDTO>();
            CommunityWithFeatureImageDTO communityWithFeatureImageDTO = new CommunityWithFeatureImageDTO();

            var community = await db.Communities.Where(x => x.communityID == communityID).AsNoTracking().FirstOrDefaultAsync();
            int adminID = community.adminUserID;
            //User user=new User();
            //user =await db.Users.Where(x => x.userID == adminID).FirstOrDefaultAsync();
            //string AdminContact = user.contact;
            int Communityid = community.communityID;

         
        
    

            Member member = new Member();
            member = await (from l in db.Members
                            where l.communityID == communityID && l.userId == userID
                            select l).FirstOrDefaultAsync();
            bool isAlertBlocked=false;
            CommunityStreetFloor communityStreetFloor=new CommunityStreetFloor();

            //Get First Emergency Contact Number of a Community
            List<SubCommunityEmergencyContacts> dbCommunityContacts = new List<SubCommunityEmergencyContacts>();
            List<SubCommunityEmergencyContacts> CommunityContacts = new List<SubCommunityEmergencyContacts>();
            SubCommunitySelectedStreetFloor subCommunitySelectedStreetFloor=new SubCommunitySelectedStreetFloor();

            if (member != null)
            {
                    
            isAlertBlocked = member.isAlertBlocked;
            communityStreetFloor = await db.CommunityStreetFloors.Where(x => x.communityID == communityID && x.streetFloor == member.streetFloor).FirstOrDefaultAsync();
            if (communityStreetFloor != null)
            {
                subCommunitySelectedStreetFloor = await db.SubCommunitySelectedStreetFloors.Where(x => x.communityStreetFloorId == communityStreetFloor.id).FirstOrDefaultAsync();
                if (subCommunitySelectedStreetFloor != null)
                {
                    dbCommunityContacts = (from l in db.SubCommunityEmergencyContacts
                                           where l.subCommunityId == subCommunitySelectedStreetFloor.subCommunityId
                                           select l).ToList();
                    bool selected = false;
                    foreach (var item in dbCommunityContacts)
                    {
                        TimeSpan start = new TimeSpan(Convert.ToInt32(item.workingHourStart), 0, 0);
                        TimeSpan end = new TimeSpan(Convert.ToInt32(item.workingHourEnd), 0, 0);

                        DateTime ServerDateTime = DateTime.Now;
                        DateTime utcDateTime = ServerDateTime.ToUniversalTime();

                        // ID from: 
                        // "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone"
                        // See http://msdn.microsoft.com/en-us/library/system.timezoneinfo.id.aspx
                        string malayTimeZoneKey = "Singapore Standard Time";
                        TimeZoneInfo malayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(malayTimeZoneKey);
                        DateTime malayDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, malayTimeZone);


                        bool silenceAlarm = TimeBetween(malayDateTime, start, end);
                        if (silenceAlarm == true && selected == false)
                        {
                            selected = true;
                            CommunityContacts.Add(item);
                        }
                    }
                }
           
            }
      
        }
            //string AdminContact = null;
            //if (CommunityContacts != null)
            //{
            //    AdminContact = CommunityContacts.contact;
            //}
            communityWithFeatureImageDTO.communityEmergencyContacts = CommunityContacts;
            //Check User is Blocked or Not for spacific Community



            if (community == null)
            {
                responceCommunityWithFeaturedImage.status = "No Community Exist";
                responceCommunityWithFeaturedImage.model = null;
                return responceCommunityWithFeaturedImage;
            }

            List<CommunityImage> communityImages = new List<CommunityImage>();
            communityImages = db.CommunityImages.Where(x => x.communityID == communityID).AsNoTracking().ToList();

            //Set Values in DTO
            communityWithFeatureImageDTO.communityFeaturedImages = communityImages;
            //communityWithFeatureImageDTO.AdminContact = AdminContact;

            communityWithFeatureImageDTO.isAlertBlock = isAlertBlocked;
            communityWithFeatureImageDTO.community = community;
            responceCommunityWithFeaturedImage.status = "Success";
            responceCommunityWithFeaturedImage.model = communityWithFeatureImageDTO;
            string lati = community.lat.ToString("N2");
            string longi = community.lng.ToString("N2");
            string date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                 using (var wb = new WebClient())
            {

                string call = "http://api.openweathermap.org/data/2.5/weather?lat="+community.lat+"&lon="+community.lng+"&appid=b7ff5ba1f63426ce46f6dfcc6b960f2b";
                var response = wb.DownloadString(call);
                JObject weatherJson = JObject.Parse(response);
                communityWithFeatureImageDTO.Weather = weatherJson;

                  }      
            return responceCommunityWithFeaturedImage;
        }











       
        
        [ResponseType(typeof(Response<Community>))]

        public async Task<Response<List<CommunityCompleteDTO>>> GetCommunityNearby(double lat, double lng)
        {
            var communities = (from b in db.Communities
                                  select b).ToList();
            Response<List<CommunityCompleteDTO>> completeCommunityResponse = new Response<List<CommunityCompleteDTO>>();
            List<CommunityCompleteDTO> completeCommunityList = new List<CommunityCompleteDTO>();
            List<NearCommunityDTO> nearCommunitiesWithDistance = new List<NearCommunityDTO>();
            NearCommunityDTO nearCommunityDTO = new NearCommunityDTO();
            List<Community> communityList = new List<Community>();
            Community community = new Community();
            for (int i = 0; i < communities.Count; i++)
            {
                var countedDistance = distance(communities[i].lat, communities[i].lng, lat, lng, 'k');
                //if (countedDistance <= 12.00)
                //{
                    community = communities[i];
                    nearCommunityDTO.community = communities[i];
                    nearCommunityDTO.distance = countedDistance;
                    communityList.Add(community);
                    
                //}
            }
           
           
            if (nearCommunitiesWithDistance == null)
            {
                
                    completeCommunityResponse.status = "No Community Exist";
                    completeCommunityResponse.model = null;
                    return completeCommunityResponse;
                
            }
            else
            {
                nearCommunitiesWithDistance = nearCommunitiesWithDistance.OrderBy(x => x.distance).ToList();
               
                foreach(var item in communityList)
                {
                    Community nearCommunity = new Community();
                    CommunityCompleteDTO communityCompleteDTO = new CommunityCompleteDTO();
                    nearCommunity.communityAddress=item.communityAddress;
                    nearCommunity.communityID=item.communityID;
                    nearCommunity.coverImage=item.coverImage;
                    nearCommunity.lat=item.lat;
                    nearCommunity.lng=item.lng;
                    nearCommunity.name=item.name;
                    nearCommunity.about = item.about;
                    communityCompleteDTO.community = nearCommunity;
                    //List<CommunityImage> communityImages = new List<CommunityImage>();
                    //communityImages = (from l in db.CommunityImages
                    //                   where l.communityID == item.communityID
                    //                   select l).AsNoTracking().ToList();
                    
                    //List<CommunityEmergencyContacts> communityContacts = new List<CommunityEmergencyContacts>();
                    //communityContacts = (from l in db.CommunityEmergencyContacts
                    //                     where l.communityID == item.communityID
                    //                     select l).AsNoTracking().ToList();
                    //List<CommunityStreetFloor> communityStreetFloor = new List<CommunityStreetFloor>();
                    //communityStreetFloor = (from l in db.CommunityStreetFloors
                    //                        where l.communityID == item.communityID
                    //                        select l).AsNoTracking().ToList();
                   
                    
                    
                    communityCompleteDTO.communityFeaturesImage = (from l in db.CommunityImages
                                                                   where l.communityID == item.communityID
                                                                   select l).AsNoTracking().ToList();

    //                var query = from l in db.CommunityImages
    //                                                               where l.communityID == item.communityID
    //                                                               select l;
    //                  var image = query.ToList().Select(r => new CommunityImage
    //{
    //    image = r.image,
    //    id=r.id,
    //}).ToList();
    //                  communityCompleteDTO.communityFeaturesImage = image;
                    communityCompleteDTO.communityEmergencyContacts = (from l in db.CommunityEmergencyContacts
                                                                       where l.communityID == item.communityID
                                                                       select l).AsNoTracking().ToList();
                    communityCompleteDTO.communityStreetFloor = (from l in db.CommunityStreetFloors
                                                                 where l.communityID == item.communityID
                                                                 select l).AsNoTracking().ToList();

                    completeCommunityList.Add(communityCompleteDTO);

                }
                completeCommunityResponse.status = "Success";
                completeCommunityResponse.model = completeCommunityList;
                return completeCommunityResponse;
            }
          
            
        }


        public async Task<Response<List<CommunityCompleteDTO>>> GetCommunityNearbySelected(int userID)
        {
       
            Response<List<CommunityCompleteDTO>> completeCommunityResponse = new Response<List<CommunityCompleteDTO>>();
            List<CommunityCompleteDTO> completeCommunityList = new List<CommunityCompleteDTO>();
          
            //Response<SelectedAndUnselectedCommunitiesDTO> responceSelectedAndUnselectedCommunitiesDTO = new Response<SelectedAndUnselectedCommunitiesDTO>();
            //SelectedAndUnselectedCommunitiesDTO selectedAndUnselectedCommunitiesDTO = new SelectedAndUnselectedCommunitiesDTO();
           
            //Selected Communities
            Response<List<Member>> responceMember = new Response<List<Member>>();
            List<Member> members = new List<Member>();
            members = (from l in db.Members
                       where l.userId == userID
                       select l).Include(x=>x.community).ToList();
            List<Community> SelectedCommunities = new List<Community>();
          
            foreach (var item in members)
            {
                Community Selectedcommunity = new Community();
                Selectedcommunity.about = item.community.about;
                Selectedcommunity.adminUserID = item.community.adminUserID;
                Selectedcommunity.communityAddress = item.community.communityAddress;
                Selectedcommunity.communityID = item.community.communityID;
                Selectedcommunity.coverImage = item.community.coverImage;
                Selectedcommunity.lat = item.community.lat;
                Selectedcommunity.lng = item.community.lng;
                Selectedcommunity.name = item.community.name;
                SelectedCommunities.Add(Selectedcommunity);
            }




            if (SelectedCommunities == null)
            {

                completeCommunityResponse.status = "No Community Member";
                completeCommunityResponse.model = null;
                return completeCommunityResponse;

            }
            else
            {
                foreach (var item in SelectedCommunities)
                {
                    Community nearCommunity = new Community();
                    CommunityCompleteDTO communityCompleteDTO = new CommunityCompleteDTO();
                    nearCommunity.communityAddress=item.communityAddress;
                    nearCommunity.communityID=item.communityID;
                    nearCommunity.coverImage=item.coverImage;
                    nearCommunity.lat=item.lat;
                    nearCommunity.lng=item.lng;
                    nearCommunity.name=item.name;
                    nearCommunity.about = item.about;
                    communityCompleteDTO.community = nearCommunity;

                    Member member = new Member();
                    member = (from l in db.Members
                              where l.communityID == item.communityID && l.userId==userID
                              select l).AsNoTracking().FirstOrDefault();
                    communityCompleteDTO.isBlocked = member.isBlocked;
                    communityCompleteDTO.address = member.address;
                    communityCompleteDTO.streetFloor = member.streetFloor;
                    communityCompleteDTO.communityFeaturesImage = (from l in db.CommunityImages
                                                                   where l.communityID == item.communityID
                                                                   select l).AsNoTracking().ToList();

    
    //                  communityCompleteDTO.communityFeaturesImage = image;
                    communityCompleteDTO.communityEmergencyContacts = (from l in db.CommunityEmergencyContacts
                                                                       where l.communityID == item.communityID
                                                                       select l).AsNoTracking().ToList();
                    communityCompleteDTO.communityStreetFloor = (from l in db.CommunityStreetFloors
                                                                 where l.communityID == item.communityID
                                                                 select l).AsNoTracking().ToList();

                    completeCommunityList.Add(communityCompleteDTO);

                }
                completeCommunityResponse.status = "Success";
                completeCommunityResponse.model = completeCommunityList;
                return completeCommunityResponse;
            }

        }




        public async Task<Response<List<CommunityCompleteDTO>>> GetCommunityNearbyUnSelected(int userID, double lat, double lng)
        {
            //All Communities
            var communities = (from b in db.Communities
                               select b).ToList();
            Response<List<CommunityCompleteDTO>> completeCommunityResponse = new Response<List<CommunityCompleteDTO>>();
            List<CommunityCompleteDTO> completeCommunityList = new List<CommunityCompleteDTO>();
            List<NearCommunityDTO> nearCommunitiesWithDistance = new List<NearCommunityDTO>();
            NearCommunityDTO nearCommunityDTO = new NearCommunityDTO();
            List<Community> allCommunityList = new List<Community>();
            Community community = new Community();
            List<Community> unSelectedCommunityList = new List<Community>();
            for (int i = 0; i < communities.Count; i++)
            {
                var countedDistance = distance(communities[i].lat, communities[i].lng, lat, lng, 'k');
                //if (countedDistance <= 12.00)
                //{
                community = communities[i];
                nearCommunityDTO.community = communities[i];
                nearCommunityDTO.distance = countedDistance;
                allCommunityList.Add(community);
                unSelectedCommunityList.Add(community);
                //}
            }

            Response<SelectedAndUnselectedCommunitiesDTO> responceSelectedAndUnselectedCommunitiesDTO = new Response<SelectedAndUnselectedCommunitiesDTO>();
            SelectedAndUnselectedCommunitiesDTO selectedAndUnselectedCommunitiesDTO = new SelectedAndUnselectedCommunitiesDTO();



            //Selected Communities
            Response<List<Member>> responceMember = new Response<List<Member>>();
            List<Member> members = new List<Member>();
            members = (from l in db.Members
                       where l.userId == userID
                       select l).Include(x => x.community).ToList();
            List<Community> SelectedCommunities = new List<Community>();

            foreach (var item in members)
            {
                Community Selectedcommunity = new Community();
                Selectedcommunity.about = item.community.about;
                Selectedcommunity.adminUserID = item.community.adminUserID;
                Selectedcommunity.communityAddress = item.community.communityAddress;
                Selectedcommunity.communityID = item.community.communityID;
                Selectedcommunity.coverImage = item.community.coverImage;
                Selectedcommunity.lat = item.community.lat;
                Selectedcommunity.lng = item.community.lng;
                Selectedcommunity.name = item.community.name;
                SelectedCommunities.Add(Selectedcommunity);
            }

            selectedAndUnselectedCommunitiesDTO.SelectedCommunities = SelectedCommunities;

            //MembersController memberController = new MembersController();
            // List<Community> SelectedCommunityList = new List<Community>();
            // for (int i = 0; i < SelectedCommunities.Count; i++)
            // {
            //     var countedDistance = distance(SelectedCommunities[i].lat, SelectedCommunities[i].lng, lat, lng, 'k');
            //     if (countedDistance <= 12.00)
            //     {
            //         community = SelectedCommunities[i];
            //         nearCommunityDTO.community = SelectedCommunities[i];
            //         nearCommunityDTO.distance = countedDistance;
            //         SelectedCommunityList.Add(community);

            //     }
            // }









            //UnSelected Communities

        
            int countCommunities = 0;
            SelectedCommunities = SelectedCommunities.OrderBy(x => x.communityID).ToList();
            countCommunities = SelectedCommunities.Count();
            
            if (countCommunities != 0)
            {
                
                foreach (var item in SelectedCommunities)
                {
                  
                        var itemToRemove = allCommunityList.Single(r => r.communityID == item.communityID);
                        //var index = allCommunityList.FindIndex(a => a.communityID == item.communityID);
                        unSelectedCommunityList.Remove(itemToRemove);
                        
                   
                   
                     
                }
            }

            //foreach (var item in unSelectedCommunityList)
            //{
            //    Community unSelectedcommunity = new Community();
            //    unSelectedcommunity.about = item.about;
            //    unSelectedcommunity.adminUserID = item.adminUserID;
            //    unSelectedcommunity.communityAddress = item.communityAddress;
            //    unSelectedcommunity.communityID = item.communityID;
            //    unSelectedcommunity.coverImage = item.coverImage;
            //    unSelectedcommunity.lat = item.lat;
            //    unSelectedcommunity.lng = item.lng;
            //    unSelectedcommunity.name = item.name;
            //    unSelectedCommunityList.Add(unSelectedcommunity);
            //}
            selectedAndUnselectedCommunitiesDTO.UnselectedCommunities = unSelectedCommunityList;


            if (responceSelectedAndUnselectedCommunitiesDTO == null)
            {

                completeCommunityResponse.status = "No Community Exist";
                completeCommunityResponse.model = null;
                return completeCommunityResponse;

            }
            else
            {



                nearCommunitiesWithDistance = nearCommunitiesWithDistance.OrderBy(x => x.distance).ToList();

                foreach (var item in unSelectedCommunityList)
                {
                    Community nearCommunity = new Community();
                    CommunityCompleteDTO communityCompleteDTO = new CommunityCompleteDTO();
                    nearCommunity.communityAddress = item.communityAddress;
                    nearCommunity.communityID = item.communityID;
                    nearCommunity.coverImage = item.coverImage;
                    nearCommunity.lat = item.lat;
                    nearCommunity.lng = item.lng;
                    nearCommunity.name = item.name;
                    nearCommunity.about = item.about;
                    communityCompleteDTO.community = nearCommunity;




                    communityCompleteDTO.communityFeaturesImage = (from l in db.CommunityImages
                                                                   where l.communityID == item.communityID
                                                                   select l).AsNoTracking().ToList();


                    //                  communityCompleteDTO.communityFeaturesImage = image;
                    communityCompleteDTO.communityEmergencyContacts = (from l in db.CommunityEmergencyContacts
                                                                       where l.communityID == item.communityID
                                                                       select l).AsNoTracking().ToList();
                    communityCompleteDTO.communityStreetFloor = (from l in db.CommunityStreetFloors
                                                                 where l.communityID == item.communityID
                                                                 select l).AsNoTracking().ToList();

                    completeCommunityList.Add(communityCompleteDTO);

                }
                completeCommunityResponse.status = "Success";
                completeCommunityResponse.model = completeCommunityList;
                return completeCommunityResponse;
            }




        }
   
        private double deg2Rad(double deg)
        {
            return (deg * Math.PI / 180);
        }
     
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180);
        }
       
        private double distance(double lat1, double lng1, double lat2, double lng2, char unit)
        {

            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lng1 - lng2;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'k': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;



            //var R = 6371d; // Radius of the earth in km
            //var dLat = deg2Rad(lat2 - lat1);  // deg2rad below
            //var dLon = Deg2Rad(lng2 - lng1);
            //var a =
            //  Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) +
            //  Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
            //  Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d);
            //var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
            //var d = R * c; // Distance in km
            //return d;
        }


        double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180d);
        }


   
        // PUT: api/Communities/5
        [ResponseType(typeof(void))]
        public async Task<Response<Community>> PutCommunity(int id, Community community)
        {
            Response<Community> communityResponse = new Response<Community>();
            if (!ModelState.IsValid)
            {
                communityResponse.status = "Failure";
                communityResponse.model = null;
                return communityResponse;
            }

            if (id != community.communityID)
            {
                communityResponse.status = "Failed: Wrong Community ID";
                communityResponse.model = null;
                return communityResponse;
            }
            community.isChangePassword = true;
            db.Entry(community).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                communityResponse.status = "Success";
                communityResponse.model = community;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    communityResponse.status = "Failed: User does not exist";
                    communityResponse.model = null;
                    return communityResponse;
                }
                else
                {
                    throw;
                }
            }

            return communityResponse; 
        }

        // POST: api/Communities
        //[ResponseType(typeof(Community))]
        //public async Task<IHttpActionResult> PostCommunity()
        //{

        //     var httpRequest = HttpContext.Current.Request;
        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    if (Request.Content.IsMimeMultipartContent())
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        Community community = new Community();
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                if (key.Equals("adminEmailID"))
        //                {
        //                      var existUser = await (from l in db.Users
        //                                   where l.emailID == val
        //                                   select l).FirstOrDefaultAsync();
        //                    community.adminUserID =existUser.userID ;
        //                }
        //                if (key.Equals("name"))
        //                    community.name = val;
        //                if (key.Equals("communityAddress"))
        //                    community.communityAddress = val;
        //                if (key.Equals("totalBuildingFloor"))
        //                    community.totalBuildingFloor = val;
        //                if (key.Equals("totalStreet"))
        //                    community.totalStreet = val;
        //                if (key.Equals("lat"))
        //                    community.lat =Convert.ToDouble(val);
        //                if (key.Equals("lng"))
        //                    community.lng = Convert.ToDouble(val);
        //            }

        //        }
        //        Response<Community> communityResponse = new Response<Community>();
        //        if (!ModelState.IsValid)
        //        {
        //            communityResponse.status = "Failure";
        //            communityResponse.model = null;
        //            return Ok(communityResponse);
        //        }
        //        if (CommunityExists(community.adminUserID))
        //        {
        //            communityResponse.status = "Failed: Community Already Exist";
        //            var existUser = await (from l in db.Communities
        //                                   where l.adminUserID == community.adminUserID
        //                                   select l).FirstOrDefaultAsync();
        //            communityResponse.model = existUser;
        //            return Ok(communityResponse);
        //        }

        //        if (httpRequest.Files.Count > 0)
        //        {
        //            var docfiles = new List<string>();
        //            int count = 0;
        //            foreach (string file in httpRequest.Files)
        //            {
        //                imageForBlob imageForBlob = new imageForBlob();
        //                string blobImageURL = imageForBlob.ConvertImageForBlob();
        //                if (count== 0)
        //                    community.logoImage = blobImageURL;
        //                else
        //                    community.coverImage = blobImageURL;
        //                count++;
        //            }
        //        }



              
        //        db.Communities.Add(community);

        //        await db.SaveChangesAsync();
        //        communityResponse.status = "Success";
        //        communityResponse.model = community;
        //        return Ok(communityResponse);

        //    }
        //    else
        //    {
        //        Response<Community> communityResponse = new Response<Community>();
        //        communityResponse.status = "Failed: Not Multipart Content";
        //        communityResponse.model = null;
        //        return Ok(communityResponse);
        //    }
        //}




     
         public async Task<Response<Community>> PostCommunity(Community community)
        {

              
             
                Response<Community> communityResponse = new Response<Community>();
                if (!ModelState.IsValid)
                {
                    communityResponse.status = "Failure";
                    communityResponse.model = null;
                    return communityResponse;
                }
                if (CommunityExists(community.adminUserID))
                {
                    communityResponse.status = "Failed: This user is already Admin";
                    var existUser = await (from l in db.Communities
                                           where l.adminUserID == community.adminUserID
                                           select l).FirstOrDefaultAsync();
                    communityResponse.model = existUser;
                    return communityResponse;
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        imageForBlob imageForBlob = new imageForBlob();
                        string blobImageURL = imageForBlob.ConvertImageForBlob();
                            community.coverImage = blobImageURL;
                    }
                }


          
             
          
                community.isChangePassword = false;
                db.Communities.Add(community);

                await db.SaveChangesAsync();
             string communityName=community.name;
             string password=community.communityPassword;
          var communitydb = await (from u in db.Communities
                              where u.name == communityName && u.communityPassword == password
                              select u).FirstOrDefaultAsync();
             GenerateCommunitySecretKey:
                   //Random generator = new Random();
          //string SecterKey = generator.Next(0, 1000000).ToString("D6");
         string SecterKey= Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
             
                CommunitySecretCodesController communitySecretKeysController = new CommunitySecretCodesController();
                Response<CommunitySecretCodes> responceGetCommunitySecretKeys = new Response<CommunitySecretCodes>();
                Response<CommunitySecretCodes> responceCommunitySecretKeys = new Response<CommunitySecretCodes>();
                responceGetCommunitySecretKeys = await communitySecretKeysController.GetCommunitySecretKeysBySecretCode(SecterKey);
                if (responceGetCommunitySecretKeys.model == null)
                {
                    CommunitySecretCodes communitySecretKeys = new CommunitySecretCodes();
                    communitySecretKeys.secretCode = SecterKey;
                    communitySecretKeys.communityID= communitydb.communityID;
                    responceCommunitySecretKeys = await communitySecretKeysController.PostCommunitySecretCodes(communitySecretKeys);
                }
                else
                {
                    goto GenerateCommunitySecretKey;
                }



                communityResponse.status = "Success";
                communityResponse.model = community;
                return communityResponse;

           
        }




        
        // DELETE: api/Communities/5
        [ResponseType(typeof(Community))]
         public async Task<Response<Community>> DeleteCommunity(int communityID)
        {
            Response<Community> communityResponse = new Response<Community>();

            Community community = await db.Communities.FindAsync(communityID);
            if (community == null)
            {
                communityResponse.status = "No Community Found";
                communityResponse.model = null;
                return communityResponse;
            }

       

             //Remove Alerts
            List<Alert> alerts = new List<Alert>();
            alerts = db.Alerts.Where(x => x.communityID == communityID).ToList();
             if(alerts.Count!=0){
                 foreach(var item in alerts)
                 {
                     Alert alert = await db.Alerts.FindAsync(item.id);
                     db.Alerts.Remove(alert);
                     
                 }
                 await db.SaveChangesAsync();
             }




             List<Announcement> Announcements = new List<Announcement>();
             Announcements = db.Announcements.Where(x => x.communityID == communityID).ToList();
             if (Announcements.Count != 0)
             {
                 foreach (var item in Announcements)
                 {
                     Announcement announcement = await db.Announcements.FindAsync(item.id);
                     db.Announcements.Remove(announcement);

                 }
                 await db.SaveChangesAsync();
             }




             List<Chat> Chats = new List<Chat>();
             Chats = db.Chats.Where(x => x.communityID == communityID).ToList();
             if (Chats.Count != 0)
             {
                 foreach (var item in Chats)
                 {
                     Chat chat = await db.Chats.FindAsync(item.chatMessageID);
                     db.Chats.Remove(chat);

                 }
                 await db.SaveChangesAsync();
             }


             List<CommunityEmergencyContacts> CommunityEmergencyContacts = new List<CommunityEmergencyContacts>();
             CommunityEmergencyContacts = db.CommunityEmergencyContacts.Where(x => x.communityID == communityID).ToList();
             if (CommunityEmergencyContacts.Count != 0)
             {
                 foreach (var item in CommunityEmergencyContacts)
                 {
                     CommunityEmergencyContacts CommunityEmergencyContact = await db.CommunityEmergencyContacts.FindAsync(item.id);
                     db.CommunityEmergencyContacts.Remove(CommunityEmergencyContact);

                 }
                 await db.SaveChangesAsync();
             }




             List<CommunityImage> CommunityImages = new List<CommunityImage>();
             CommunityImages = db.CommunityImages.Where(x => x.communityID == communityID).ToList();
             if (CommunityImages.Count != 0)
             {
                 foreach (var item in CommunityImages)
                 {
                     CommunityImage CommunityImage = await db.CommunityImages.FindAsync(item.id);
                     db.CommunityImages.Remove(CommunityImage);

                 }
                 await db.SaveChangesAsync();
             }






             List<CommunityiReports> CommunityiReports = new List<CommunityiReports>();
             CommunityiReports = db.CommunityiReports.Where(x => x.communityID == communityID).ToList();
             if (CommunityiReports.Count != 0)
             {
                 foreach (var item in CommunityiReports)
                 {
                     CommunityiReports CommunityiReport = await db.CommunityiReports.FindAsync(item.CommunityiReportsID);
                     db.CommunityiReports.Remove(CommunityiReport);

                 }
                 await db.SaveChangesAsync();
             }


       




             List<CommunityStreetFloor> CommunityStreetFloors = new List<CommunityStreetFloor>();
             CommunityStreetFloors = db.CommunityStreetFloors.Where(x => x.communityID == communityID).ToList();
             if (CommunityStreetFloors.Count != 0)
             {
                 foreach (var item in CommunityStreetFloors)
                 {
                     CommunityStreetFloor CommunityStreetFloor = await db.CommunityStreetFloors.FindAsync(item.id);
                     db.CommunityStreetFloors.Remove(CommunityStreetFloor);

                 }
                 await db.SaveChangesAsync();
             }




             List<Complaint> Complaints = new List<Complaint>();
             Complaints = db.Complaints.Where(x => x.communityID == communityID).ToList();
             if (Complaints.Count != 0)
             {
                 foreach (var item in Complaints)
                 {
                     Complaint Complaint = await db.Complaints.FindAsync(item.complaintID);
                     db.Complaints.Remove(Complaint);

                 }
                 await db.SaveChangesAsync();
             }


             List<Member> Members = new List<Member>();
             Members = db.Members.Where(x => x.communityID == communityID).ToList();
             if (Members.Count != 0)
             {
                 foreach (var item in Members)
                 {
                     Member Member = await db.Members.FindAsync(item.id);
                     db.Members.Remove(Member);

                 }
                 await db.SaveChangesAsync();
             }




             List<ServiceStaff> ServiceStaffs = new List<ServiceStaff>();
             ServiceStaffs = db.ServiceStaffs.Where(x => x.communityID == communityID).ToList();
             if (ServiceStaffs.Count != 0)
             {
                 foreach (var item in ServiceStaffs)
                 {
                     ServiceStaff ServiceStaff = await db.ServiceStaffs.FindAsync(item.id);
                     db.ServiceStaffs.Remove(ServiceStaff);

                 }
                 await db.SaveChangesAsync();
             }




             List<CommunityService> CommunityServices = new List<CommunityService>();
             CommunityServices = db.CommunityServices.Where(x => x.communityID == communityID).ToList();
             if (CommunityiReports.Count != 0)
             {
                 foreach (var item in CommunityServices)
                 {
                     CommunityService CommunityService = await db.CommunityServices.FindAsync(item.communityServiceID);
                     db.CommunityServices.Remove(CommunityService);

                 }
                 await db.SaveChangesAsync();
             }

             db.Communities.Remove(community);
             await db.SaveChangesAsync();
            communityResponse.status = "Success";
            communityResponse.model = community;
            return communityResponse;
        }
     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
        private bool CommunityExists(int adminUserID)
        {
            return db.Communities.Count(e => e.adminUserID == adminUserID) > 0;
        }

        private bool UserExists(string email)
        {
            return db.Users.Count(e => e.emailID == email) > 0;
        }
      






    }
}