using DemoCommon.Models;
using DemoCommon.ReqModels;
using DemoService;
using DemoService.Helpers;
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
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [Authorize]
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
