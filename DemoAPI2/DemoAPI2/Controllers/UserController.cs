using DemoCommon.Models;
using DemoService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await _userService.GetAllUser();
            return Ok(data);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetUser(int? id)
        {
            var data = await _userService.GetUser(id);
            return Ok(data);
        }
    }
}
