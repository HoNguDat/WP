using DemoCommon.Exceptions;
using DemoCommon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPost();
        Task<Post> GetPost(int? id);
        Task<Post> AddPost(Post post);
        Task<Post> UpdatePost(int id,Post post);
        void DeletePost(int id);

    }
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Post>> GetAllPost()
        {
            return await _context.Posts.ToListAsync();
        }
        public async Task<Post> GetPost(int? id)
        {
            var entity = new Post();
            if (id != null)
            {
                entity = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id)
                    ?? throw new NotFoundException($"Id:{id} không tồn tại !");


            }
            return entity;
        }
        public async Task<Post> AddPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<Post> UpdatePost(int id,Post post)
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
