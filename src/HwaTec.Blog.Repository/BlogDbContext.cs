using HwaTec.Blog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.Repository
{
    public class BlogDbContext :DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext>  dbContextOptions) :base(dbContextOptions)
        {

        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

    }
}
