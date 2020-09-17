using justAsk.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using justAsk.Models;

namespace justAsk.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{controller}")]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public PostController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public Post[] GetPosts()
        {
            return this.dbContext.Posts.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            Post dbPost = this.dbContext.Posts.Add(post).Entity;
            await this.dbContext.SaveChangesAsync();
            return Created($"api/post/{dbPost.Id}", dbPost);
        }
    }
}