using DemoCommon.Exceptions;
using DemoCommon.Models;
using DemoCommon.ReqModels;
using DemoCommon.ResModels;
using DemoService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<List<User>> GetAllUser();
        Task<User> GetUser(int? id);
        Task<User> AddUser(User user);
        bool CheckEmail(string email);
       User GetEmail(string email);
    }
    public class UserService:IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        //Constructor
        public UserService(ApplicationDbContext context,IOptions<AppSettings> appSettings)
        {
            this._context = context;
            _appSettings = appSettings.Value;
        }
        //Method
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == model.Email && x.Password==model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            //var token = GenerateJwtToken(user);
            var token = GenerateAccessToken(user);

            return new AuthenticateResponse(user, token);
        }

        public string GenerateAccessToken(User user)
        {
            var issuedTime = DateTime.UtcNow;
            var expiredAt = issuedTime.Add(TimeSpan.FromDays(30));
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, (issuedTime.Subtract(new DateTime(1970,1,1))).TotalSeconds.ToString(), ClaimValueTypes.Integer64),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("d0dd4162d3ab48b7adae097b755e671e7411"));

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: "jwtIssuer",
                audience: "jwtAudience",
                claims: claims,
                notBefore: issuedTime,
                expires: expiredAt,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }


        //private string GenerateJwtToken(User user)
        //{
        //    // generate token that is valid for 7 days
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes("d0dd4162d3ab48b7adae097b755e671e7411");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] { new Claim("UserId", user.UserId.ToString()) }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUser(int? id)
        {
            var entity = new User();
            if (id != null)
            {
                entity = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id)
                    ?? throw new NotFoundException($"Id:{id} không tồn tại !");
            }
            return entity;
        }
        public User GetEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User>AddUser(User user)
        {
            if(CheckEmail(user.Email))
            {
                throw new NotFoundException($"Email:{user.Email} đã tồn tại !");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public bool CheckEmail(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

      
    }
}
