using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Data
{
   public class NewsDBContext : DbContext
    {
        public NewsDBContext(DbContextOptions<NewsDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

       public DbSet<News> news { get; set; }
    }
}
