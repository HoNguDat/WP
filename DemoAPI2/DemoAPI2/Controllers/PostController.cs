using DemoCommon.Models;
using DemoCommon.ReqModels;
using DemoCommon.ResModels;
using DemoService;
using DemoService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IFileService _fileService;
        public PostController(IPostService postService,IFileService fileService)
        {
            _postService = postService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("getAllPost")]
        public async Task<List<PostResponse>> GetAllPost(string keyword)
        {
            var data = await _postService.GetAllPost(keyword);
            var result = data.Select(p => new PostResponse(p)).OrderByDescending(p=>p.CreatedDateTime).ToList();
            return result;
        }

        [HttpGet]
        [Route("getById/{id?}")]
        public async Task<Post> GetPost(int? id)
        {
            var data = await _postService.GetPost(id);
            return data;
        }

        [HttpPost]
        [Route("addPost")]
        
        public async Task<PostResponse> AddPost([FromForm] Post post)
        {
            var fileResult = _fileService.SaveFile(post.ImageFile);
            if(fileResult.Item1==1)
            {
                post.PostImage = fileResult.Item2;
            }
            post.CreatedDateTime = DateTime.Now;
            var data = await _postService.AddPost(post);

            var result = await GetPost(data.PostId);

            return new PostResponse(result);
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
            var data = await _postService.UpdatePost(id, post);
            return Ok(data);
        }
    }
}
