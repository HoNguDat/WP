using DemoCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCommon.ResModels
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Token = token;
        }
    }
}
