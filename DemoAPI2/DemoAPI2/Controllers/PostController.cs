using DemoCommon.Models;
using DemoService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [Route("getAllPost")]
        public async Task<IActionResult> GetAllPost()
        {
            var data = await _postService.GetAllPost();
            return Ok(data);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetPost(int? id)
        {
            var data = await _postService.GetPost(id);
            return Ok(data);
        }

        [HttpPost]
        [Route("addPost")]
        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            var data = await _postService.AddPost(post);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public JsonResult Deletepost(int id)
        {
            _postService.DeletePost(id);
            return new JsonResult("Deleted Successfully");
        }

        [HttpPut]
        [Route("updatePost/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        {
            var data = await _postService.UpdatePost(id,post);
            return Ok(data);
        }
    }
}
