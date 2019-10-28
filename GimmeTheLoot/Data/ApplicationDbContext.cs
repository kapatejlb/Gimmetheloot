using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GimmeTheLoot.Enums;
using GimmeTheLoot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GimmeTheLoot.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser>
    {

        //private List<AspNetUser> users;

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


        public void Create(AspNetUser u, List<AspNetUser> users)
        {
            //users = new List<AspNetUser>();////////
            users.Add(
                new AspNetUser
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    NormalizedUserName = u.NormalizedUserName,
                    PhoneNumber = u.PhoneNumber,
                    PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    NormalizedEmail = u.NormalizedEmail,

                    AccessFailedCount = u.AccessFailedCount,

                    TwoFactorEnabled = u.TwoFactorEnabled,

                    Comments = u.Comments,
                    ConcurrencyStamp = u.ConcurrencyStamp,
                    LockoutEnabled = u.LockoutEnabled,
                    LockoutEnd = u.LockoutEnd,
                    PasswordHash = u.PasswordHash,
                    Projects = u.Projects,
                    SecurityStamp = u.SecurityStamp,
                    //Id = input.Id,
                    //Family = input.Family,
                    //Name = input.Name,
                    //AddDate = input.AddDate
                });
        }



        public IEnumerable<AspNetUser> Search(List<AspNetUser> users, int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount)
        {
            //users = new List<AspNetUser>();/////////
            var queryable = AspNetUsers.AsQueryable();


            #region بر اساس متن



            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(c => c.UserName.Contains(term) || c.Email.Contains(term) || c.NormalizedEmail.Contains(term)
                || c.PhoneNumber.Contains(term));

            }



            #endregion

            #region مرتب سازی



            switch (sortBy)
            {
                //case SortBy.AddDate:
                //    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.AddDate) : queryable.OrderByDescending(u => u.AddDate);
                //    break;
                case SortBy.AddDate:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.Email).ThenBy(u => u.EmailConfirmed) : queryable.OrderByDescending(u => u.Email).ThenByDescending(u => u.EmailConfirmed);
                    break;
                case SortBy.DisplayName:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.UserName).ThenBy(u => u.PhoneNumber) : queryable.OrderByDescending(u => u.UserName).ThenByDescending(u => u.PhoneNumber);
                    break;
                default:
                    break;
            }




            #endregion

            #region دریافت تعداد کل صفحات

            TotalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;

            #endregion

            #region دریافت تعداد رکوردهای مورد تیاز


            var skiped = (page - 1) * recordsPerPage;
            queryable = queryable.Skip(skiped).Take(recordsPerPage);


            #endregion



            return queryable.Select(u => new AspNetUser
            {
                Id = u.Id,
                UserName = u.UserName,
                NormalizedUserName = u.NormalizedUserName,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                NormalizedEmail = u.NormalizedEmail,

                AccessFailedCount = u.AccessFailedCount,

                TwoFactorEnabled = u.TwoFactorEnabled,

                Comments = u.Comments,
                ConcurrencyStamp = u.ConcurrencyStamp, 
                LockoutEnabled = u.LockoutEnabled,
                LockoutEnd = u.LockoutEnd, 
                PasswordHash = u.PasswordHash,
                Projects = u.Projects,
                SecurityStamp = u.SecurityStamp,

                //AddDate = u.AddDate.ToShortDateString(),
                //Name = u.Name,
                //Family = u.Family,

            }).ToList();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
    }
}
