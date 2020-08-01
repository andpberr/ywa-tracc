using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ywa_tracc.Models;

namespace ywa_tracc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Video> Video {get; set;}
        public DbSet<UserActivity> UserActivity { get; set; }
        public DbSet<LastRefresh> LastRefresh { get; set; }
    }
}
