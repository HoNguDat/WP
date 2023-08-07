using DemoCommon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = DemoCommon.Models.Group;

namespace DemoService
{
    public interface IGroupService
    {
        Task<List<Group>> GetAllGroup();
    }
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        public GroupService(ApplicationDbContext context)
        {
                this._context = context;
        }
        public async Task<List<Group>> GetAllGroup()
        {
            return await _context.Groups.ToListAsync();
        }

       
    }
}
