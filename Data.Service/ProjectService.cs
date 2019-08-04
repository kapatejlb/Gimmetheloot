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
            var project = new Project();
            project.Title = Entity.Title;
            project.Created = Entity.Created;
            project.Subject = Entity.Subject;
            project.ExpirationDate = Entity.ExpirationDate;

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList()[0];
            project.UserId = userName.Id;

            dbContext.Projects.Add(project);
            dbContext.SaveChanges();
            return project.Id;
        }

        public void AddCommentary(int Id, string CommentaryText, string UserName)
        {

            var commentary = new Commentary();
            
            commentary.Text = CommentaryText;
            commentary.ProjectId = Id;
            commentary.Date = DateTime.Now;
            commentary.UserName = UserName;

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList()[0];
            commentary.UserId = userName.Id;



            var entity = dbContext.Projects.Find(Id);
            commentary.Project = entity;

            entity.Comments = new List<Commentary>();
            entity.Comments.Add(commentary);

            dbContext.Commentaries.Add(commentary);
            dbContext.SaveChanges();
        }

        public void AddContent(int id, string Content, string UserName)
        {
            var content = new Content();

            content.Date = DateTime.Now;
            content.ProjectId = id;
            content.Text = Content;

            var userName = dbContext.AspNetUsers.Where(a => a.UserName == UserName).ToList()[0];
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

            return content[0];            
        }

        public void Delete(int Id)
        {
            var entity = dbContext.Projects.Find(Id);
            dbContext.Projects.Remove(entity);
            dbContext.SaveChanges();
        }

        public ProjectViewModel Get(int Id)
        {
            var projectViewModel = new ProjectViewModel();
            var project = dbContext.Projects.Find(Id);
            projectViewModel.Id = project.Id;
            projectViewModel.Title = project.Title;
            projectViewModel.Created = project.Created;
            projectViewModel.Subject = project.Subject;
            projectViewModel.ExpirationDate = project.ExpirationDate;
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
