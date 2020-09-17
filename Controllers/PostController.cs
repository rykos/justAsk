using justAsk.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using justAsk.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace justAsk.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{controller}")]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserHelperProvider userHelper;
        public PostController(ApplicationDbContext dbContext, UserHelperProvider userHelper)
        {
            this.dbContext = dbContext;
            this.userHelper = userHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public Post[] GetPosts()
        {
            return this.dbContext.Posts.AsNoTracking().ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            ApplicationUser user = await this.userHelper.GetApplicationUser(this.User);
            if (user == default)
            {
                return Unauthorized();
            }
            post.ApplicationUserId = user.Id;
            post.ApplicationUser = user;

            Post dbPost = this.dbContext.Posts.Add(post).Entity;
            await this.dbContext.SaveChangesAsync();
            return Created($"api/post/{dbPost.Id}", dbPost);
        }

        [HttpGet]
        [Route("vote/post")]
        public async Task<IActionResult> VotePost(int id, string state)
        {
            VoteState newVoteState;
            if (!Enum.TryParse<VoteState>(state, out newVoteState))
            {
                return BadRequest(new Response() { Status = "Error", Message = "Bad vote state" });
            }

            Post post = this.dbContext.Posts.FirstOrDefault(x => x.Id == id);
            if (post == default)
            {
                return NotFound();
            }

            ApplicationUser user = await this.userHelper.GetApplicationUser(this.User);
            if (user == default)
            {
                return Unauthorized();
            }

            Vote vote = dbContext.Votes.FirstOrDefault(v => v.ContentId == id && v.ApplicationUserId == user.Id);
            dbContext.Posts.Update(post);
            if (vote == default)//new vote
            {
                vote = new Vote() { ContentId = id, State = newVoteState, ApplicationUserId = user.Id, ApplicationUser = user };
                dbContext.Votes.Add(vote);//add new vote to database
                post.Karma += VoteValue(newVoteState);//Update post karma score
            }
            else//Change vote
            {
                if (vote.State != newVoteState)//Different vote
                {
                    dbContext.Votes.Update(vote);
                    post.Karma += VoteValue(newVoteState) - VoteValue(vote.State);//Subtracts old vote and adds new vote value
                    vote.State = newVoteState;//Updates vote state
                }
            }
            dbContext.SaveChanges();

            return Ok(new { id = vote.Id, newKarma = post.Karma });
        }

        public int VoteValue(VoteState vote)
        {
            return vote == VoteState.plus ? 1 : -1;
        }
    }
}