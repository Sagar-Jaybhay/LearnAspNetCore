using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnAspCore.Models
{
    public class OurDbContext:IdentityDbContext
    {
        public DbSet<Student> Students { get; set; }
        public OurDbContext(DbContextOptions<OurDbContext> options):base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          modelBuilder.Seed();
        }
    }
}
