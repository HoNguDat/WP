using DemoCommon.Exceptions;
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
        Task<Group> GetGroup(int? id);
        Task<Group> AddGroup(Group group);
        Task<Group> UpdateGroup(int id, Group group);
        void DeleteGroup(int id);
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
        public async Task<Group> GetGroup(int? id)
        {
            var entity = new Group();
            if (id != null)
            {
                entity = await _context.Groups.FirstOrDefaultAsync(x => x.GroupId == id)
                    ?? throw new NotFoundException($"Id:{id} không tồn tại !");
            }
            return entity;
        }
        public async Task<Group> AddGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }
        public async Task<Group> UpdateGroup(int id, Group group)
        {
            group.GroupId = id;
            _context.Entry(group).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return group;
        }
        public void DeleteGroup(int id)
        {
            var group = _context.Groups.Find(id);

            if (group == null)
            {
                throw new NotFoundException("Id không tồn tại");
            }

            _context.Entry(group).State = EntityState.Deleted;
            _context.SaveChanges();
        }


    }
}
