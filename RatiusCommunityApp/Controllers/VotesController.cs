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
    public class VotesController : ApiController
    {
        private RatiusCommunityAppContext db = new RatiusCommunityAppContext();

        // GET: api/Votes
        public IQueryable<Vote> GetVotes()
        {
            return db.Votes;
        }

        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<IHttpActionResult> GetVote(int id)
        {
            Vote vote = await db.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<Response<Vote>> GetVote(int userID, int complaintID)
        {
            Response<Vote> responceMember = new Response<Vote>();
            Vote vote = new Vote();
            vote = await (from l in db.Votes
                            where l.complaintID == complaintID && l.userID== userID
                            select l).FirstOrDefaultAsync();
            if (vote == null)
            {
                responceMember.status = "Failed: No Vote";
                responceMember.model = null;
                return responceMember;
            }
            responceMember.status = "Success";
            responceMember.model = vote;
            return responceMember;
        }
        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<Response<List<Vote>>> GetAllVotesOfComplaints(int complaintID)
        {
           Response<List<Vote>> responceVote = new Response<List<Vote>>();
            List<Vote> votes = new List<Vote>();
            votes = (from l in db.Votes
                                 where  l.complaintID==complaintID
                                 select l).ToList();
            if (votes == null)
            {
                responceVote.status = "Failed: No Vote";
                responceVote.model = null;
                return responceVote;
            }

            responceVote.status = "Success";
            responceVote.model = votes;
            return responceVote;
        }
        // PUT: api/Votes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVote(int id, Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.id)
            {
                return BadRequest();
            }

            db.Entry(vote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        [ResponseType(typeof(Vote))]
        public async Task<Response<Vote>> PostVote(Vote vote)
        {
            Response<Vote> responseVote = new Response<Vote>();
            if (!ModelState.IsValid)
            {
                responseVote.status = "Failure";
                responseVote.model = null;
                return responseVote;
            }
            if (VoteExists(vote.userID, vote.complaintID))
            {
                responseVote.status = "Failed: You Are Already Vote it";
                Vote existVote = new Vote();
                existVote = await (from l in db.Votes
                                     where l.userID == vote.userID&& l.complaintID == vote.complaintID
                                     select l).FirstOrDefaultAsync();
                responseVote.model = existVote;
                return responseVote;
            }

            db.Votes.Add(vote);
            await db.SaveChangesAsync();
            responseVote.status = "Success";
            responseVote.model = vote;
            return responseVote;
        }

        // DELETE: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<IHttpActionResult> DeleteVote(int id)
        {
            Vote vote = await db.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            db.Votes.Remove(vote);
            await db.SaveChangesAsync();

            return Ok(vote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteExists(int id)
        {
            return db.Votes.Count(e => e.id == id) > 0;
        }
        private bool VoteExists(int userID,int complaintID)
        {
            return db.Votes.Count(e => e.id == userID && e.complaintID==complaintID) > 0;
        }
    }
}