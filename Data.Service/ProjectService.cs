using Data.Entities;
using Data.EntitiyFrameWork;
using Data.ViewModels;
using Data.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Data.Service
{
    public class ProjectService : IProjectService
    {
        private readonly DataBase1Context dbContext;
        public ProjectService(DataBase1Context dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(ProjectViewModel Entity, string UserName)
        {
            var project = new Project
            {
                Title = Entity.Title,
                Created = Entity.Created,
                Subject = Entity.Subject,
                ExpirationDate = Entity.ExpirationDate
            };

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList().First();
            project.UserId = userName.Id;

            dbContext.Projects.Add(project);
            dbContext.SaveChanges();
            return project.Id;
        }

        public void AddCommentary(int Id, string CommentaryText, string UserName)
        {

            var commentary = new Commentary
            {
                Text = CommentaryText,
                ProjectId = Id,
                Date = DateTime.Now,
                UserName = UserName
            };

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList().First();
            commentary.UserId = userName.Id;



            var entity = dbContext.Projects.Find(Id);
            commentary.Project = entity;

            entity.Comments = new List<Commentary>
            {
                commentary
            };

            dbContext.Commentaries.Add(commentary);
            dbContext.SaveChanges();
        }

        public void AddContent(int id, string Content, string UserName)
        {
            var content = new Content
            {
                Date = DateTime.Now,
                ProjectId = id,
                Text = Content
            };

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList().First();
            var entity = dbContext.Projects.Find(id);
            content.Project = entity;



            entity.Content = new Content();
            entity.Content = content;
            entity.UserId = userName.Id;


            dbContext.Contents.Add(content);
            dbContext.SaveChanges();   
        }

        public IEnumerable<Commentary> GetCommentaries(int ProjectID)//
        {
            var commentaries = dbContext.Commentaries.Where(a => a.ProjectId == ProjectID).ToList();

            return commentaries;
        }

        public Content GetContent(int ProjectID)
        {
            var content = dbContext.Contents.Where(a => a.ProjectId == ProjectID).ToList();

            return content.First();            
        }

        public void Delete(int Id)
        {
            var entity = dbContext.Projects.Find(Id);
            dbContext.Projects.Remove(entity);
            dbContext.SaveChanges();
        }

        public ProjectViewModel Get(int Id)
        {
            var project = dbContext.Projects.Find(Id);
            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Created = project.Created,
                Subject = project.Subject,
                ExpirationDate = project.ExpirationDate
            };
            return projectViewModel;
        }

        public IEnumerable<ProjectViewModel> GetAll()
        {
            var projects = dbContext.Projects.ToList();
            var projectViewModels = new List<ProjectViewModel>();
            foreach(var proj in projects)
            {
                projectViewModels.Add(new ProjectViewModel
                {
                    Id = proj.Id,
                    Title = proj.Title,
                    Created = proj.Created,
                    Subject = proj.Subject,
                    ExpirationDate = proj.ExpirationDate,
                });
            }

            return projectViewModels;
        }



        public void Update(ProjectViewModel viewModel)
        {
            var project = dbContext.Projects.Find(viewModel.Id);
            project.Title = viewModel.Title;
            project.Created = viewModel.Created;
            project.Subject = viewModel.Subject;
            project.ExpirationDate = viewModel.ExpirationDate;
            dbContext.SaveChanges();
        }


    }
}
