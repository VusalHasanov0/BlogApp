using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)    
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();    
                }

                if(!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag {Text = "web programlama",Url = "web-programlama",Color = TagColors.warning},
                        new Tag {Text = "backend",Url = "backend",Color = TagColors.info},
                        new Tag {Text = "frontend",Url = "frontend",Color = TagColors.success},
                        new Tag {Text = "fullstack",Url = "fullstack",Color = TagColors.secondary},
                        new Tag {Text = "php",Url = "php",Color = TagColors.primary}
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User {UserName = "vusalhesenov",Name = "Vusal Hesenov",Email = "vusalhesenov361@gmail.com",Password = "vusal361",Image = "p1.jpg"},
                        new User {UserName = "vuqarhesenov",Name = "Vuqar Hesenov",Email = "vuqarhesenov1968@gmail.com",Password = "vuqar1968",Image = "p2.jpg"}
                        
                    );
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post {
                            Title = "Asp.Net core",
                            Description = "Asp.Net core dersleri",
                            Content = "Asp.Net core dersleri",
                            Url = "aspnet-core",
                            IsActive = true,
                            Image = "1.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            UserId = 1,
                            Comments = new List<Comment> { 
                                new Comment {Text = "iyi bu kurs",PublishedOn = DateTime.UtcNow.AddDays(-20),UserId = 1},
                                new Comment {Text = "cok faydalandigim bir kurs",PublishedOn = DateTime.UtcNow.AddDays(-10),UserId = 2}
                            }
                        },
                        new Post {
                            Title = "Php",
                            Description = "Php dersleri",
                            Content = "Php dersleri",
                            Url = "php",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1
                        },
                        new Post {
                            Title = "Django",
                            Description = "Django dersleri",
                            Content = "Django  dersleri",
                            Url = "django",
                            IsActive = true,
                            Image = "3.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-5),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        },
                        new Post {
                            Title = "React",
                            Content = "React dersleri",
                            Url = "React",
                            IsActive = true,
                            Image = "1.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-40),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        },
                        new Post {
                            Title = "Angular",
                            Content = "Angular dersleri",
                            Url = "Angular",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-50),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        },
                        new Post {
                            Title = "Web Tasarim",
                            Content = "Web Tasarim dersleri",
                            Url = "web-tasarim",
                            IsActive = true,
                            Image = "3.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-60),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                        
                    );
                    context.SaveChanges();
                }

            }
        }
    }
}