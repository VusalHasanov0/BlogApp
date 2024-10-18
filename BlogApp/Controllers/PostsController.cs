using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Model;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private  IPostRepository _postrepository;
        private  ICommentRepository _commentrepository;
        private ITagRepository _tagrepository;  
        public PostsController(IPostRepository postrepository, ICommentRepository commentrepository,ITagRepository tagrepository)
        {
            _postrepository = postrepository;
            _commentrepository = commentrepository;
            _tagrepository = tagrepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts =  _postrepository.Posts.Where(i=>i.IsActive);
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
                    .Include(x=>x.User)
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _postrepository.Create(
                    new Post {
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId!),
                        PublishedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> List()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role =  User.FindFirstValue(ClaimTypes.Role);
            var posts = _postrepository.Posts;
            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(u=>u.UserId == userId);
            }
            _postrepository.Posts.Where(x=>x.IsActive == true).ToList();
            return View(await posts.ToListAsync());
        }
    
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            var post = _postrepository.Posts.Include(i=>i.Tags).FirstOrDefault(i=>i.PostId == id); 

            if (post == null) 
            {
                return NotFound();
            }

            ViewBag.Tags = _tagrepository.Tags.ToList();

            return View(new PostCreateViewModel{
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
            
        }

        [Authorize]
        [HttpPost]
        
        public IActionResult Edit(PostCreateViewModel model,int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var entityUpdate = new Post {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };

                if(User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityUpdate.IsActive = model.IsActive;
                }

                _postrepository.EditPost(entityUpdate, tagIds);
                return RedirectToAction("List");
            }
            ViewBag.Tags = _tagrepository.Tags.ToList();
            return View(model);
        }
    }
}