using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Assign02_AWS_Movies.Models;

namespace Assign02_AWS_Movies.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Assign02_AWS_Movies.Models.Movie> Movie { get; set; }
        public DbSet<Assign02_AWS_Movies.Models.Category> Category { get; set; }
        public DbSet<Assign02_AWS_Movies.Models.Comment> Comment { get; set; }
    }
}
