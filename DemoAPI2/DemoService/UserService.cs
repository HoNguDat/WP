using DemoCommon.Exceptions;
using DemoCommon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUser();
        Task<User> GetUser(int? id);
      
    }
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            this._context = context;
        }
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
      
    }
}
