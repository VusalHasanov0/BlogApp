using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private  IPostRepository _postrepository;
        public PostsController(IPostRepository postrepository)
        {
            _postrepository = postrepository;
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
                    .FirstOrDefaultAsync(p=> p.Url == url));
        }
        
    }
}