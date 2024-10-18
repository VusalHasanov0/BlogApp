using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void Create(Post Post);
        void EditPost(Post Post);
        void EditPost(Post Post,int[] tagIds);
    }
}