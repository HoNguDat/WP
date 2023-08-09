using DemoCommon.Models;
using DemoService;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly JWTService _jwtService;
        public AuthController(IUserService userService,JWTService jWTService)
        {
            _userService = userService;
            _jwtService = jWTService;
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            var model = _userService.GetEmail(registerModel.Email);
            if (model != null) return BadRequest(new { massage = "Email already exists !" });
            
            var user = new User
            {
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
            };
            return Created("Register success", _userService.AddUser(user));
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var user = _userService.GetEmail(loginModel.Email);

            if (user == null) return BadRequest(new { message = "Email does not exists !" });

            if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return BadRequest(new { message = "Incorrect password !" });
            }

            var jwt = _jwtService.Generate(user.UserId);

           

            return Ok(new
            {
                
                jwt
            });
        }
        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userService.GetUser(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }

    }
}
