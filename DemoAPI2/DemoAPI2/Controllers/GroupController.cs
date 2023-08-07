using DemoService;
using Microsoft.AspNetCore.Mvc;
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
    }
}
