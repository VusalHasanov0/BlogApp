using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfTagRepository : ITagRepository
    {
        private readonly BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags; 

        public void Create(Tag Tag)
        {
            _context.Add(Tag);
            _context.SaveChanges();
        }
    }
}