using DemoCommon.Exceptions;
using DemoCommon.Models;
using DemoCommon.ReqModels;
using DemoCommon.ResModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPost(string? keyword);
        Task<Post> GetPost(int? id);
        Task<Post> AddPost(Post post);
        Task<Post> UpdatePost(int id, Post post);
        void DeletePost(int id);
        //Task<List<Post>> Search(string keyword);
    }
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Post>> GetAllPost(string? keyword)
        {
            //if(!string.IsNullOrEmpty(keyword))
            //    return await _context.Posts.Include(p => p.User).Include(p => p.Group).Where(x => x.Content.ToLower().Contains(keyword.ToLower())).ToListAsync();
            //return await _context.Posts.Include(p => p.User).Include(p => p.Group).ToListAsync();

            var query = _context.Posts.Include(p => p.User).Include(p => p.Group).OrderByDescending(p=>p.CreatedDateTime).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(x => x.Content.ToLower().Contains(lowerKeyword));
            }

            return await query.ToListAsync();
        }

        public async Task<Post> GetPost(int? id)
        {
            var entity = await _context.Posts.Include(p => p.User).Include(p => p.Group).FirstOrDefaultAsync(x => x.PostId == id)
                    ?? throw new NotFoundException($"Id:{id} không tồn tại !");

            return entity;

            //var entity = new Post();
            //if (id != null)
            //{
            //    entity = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id)
            //        ?? throw new NotFoundException($"Id:{id} không tồn tại !");


            //}
            //return entity;
        }
        public async Task<Post> AddPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<Post> UpdatePost(int id, Post post)
        {
            post.PostId = id;
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return post;
        }
        public void DeletePost(int id)
        {
            var category = _context.Posts.Find(id);

            if (category == null)
            {
                throw new NotFoundException("Id không tồn tại");
            }

            _context.Entry(category).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}
