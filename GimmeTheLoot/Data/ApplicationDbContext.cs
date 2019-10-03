using System;
using System.Collections.Generic;
using System.Text;
using GimmeTheLoot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GimmeTheLoot.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity
               .HasMany(d => d.Projects)
               .WithOne(p => p.AspNetUser)
               .HasForeignKey(d => d.AspNetUserId);

                entity
                .HasMany(d => d.Comments)
                .WithOne(p => p.AspNetUser)
                .HasForeignKey(d => d.AspNetUserId);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity
                .HasOne(d => d.Content)
                .WithOne(p => p.Project)
                .HasForeignKey<Content>(d => d.ProjectId);

                entity
                .HasMany(d => d.Comments)
                .WithOne(p => p.Project)
                .HasForeignKey(d => d.ProjectId);

                entity
                .HasMany(d => d.Posts)
                .WithOne(p => p.Project)
                .HasForeignKey(d => d.ProjectId);
            });
        }


        public DbSet<Project> Projects { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Content> Contents { get; set; }


        public DbSet<AspNetUser> AspNetUsers { get; set; }
    }
}
