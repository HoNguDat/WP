using DemoCommon.Models;
using DemoCommon.ResModels;
using DemoService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DemoAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpGet]
        [Route("getAllGroup")]
        public async Task<IActionResult> GetAllGroup()
        {
            var data = await _groupService.GetAllGroup();
            return Ok(data);
        }
        [HttpGet]
        [Route("getById/{id?}")]
        public async Task<Group> GetGroup(int? id)
        {
            var data = await _groupService.GetGroup(id);
            return data;
        }

        [HttpPost]
        [Route("addGroup")]

        public async Task<IActionResult> AddGroup([FromBody] Group group)
        {
           
            var data = await _groupService.AddGroup(group);

         

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public JsonResult DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
            return new JsonResult("Deleted Successfully");
        }

        //[HttpPut]
        //[Route("updatePost/{id}")]
        //public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        //{
        //    var data = await _postService.UpdatePost(id, post);
        //    return Ok(data);
        //}
    }
}
