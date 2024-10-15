using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private  IPostRepository _postrepository;
        private  ICommentRepository _commentrepository;
        public PostsController(IPostRepository postrepository, ICommentRepository commentrepository)
        {
            _postrepository = postrepository;
            _commentrepository = commentrepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts =  _postrepository.Posts;
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(a => a.Url == tag));
            }
            return View(
                new PostsViewModel
                {
                    Posts = await posts.ToListAsync()
                }
            );
        } 

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postrepository
                    .Posts
                    .Include(x => x.Tags)
                    .Include(x => x.Comments)
                    .ThenInclude(x=>x.User)
                    .FirstOrDefaultAsync(p=> p.Url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId,string Text,string Url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserName =  User.FindFirstValue(ClaimTypes.Name);
            var avatar =  User.FindFirstValue(ClaimTypes.UserData);
            var entity = new Comment{
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };
            _commentrepository.CreateComment(entity);
            
            return Json(new {
                UserName,
                Text,
                entity.PublishedOn,
                avatar
            });
        }
        
    }
}