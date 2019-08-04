using Data.Entities;
using Data.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public interface IProjectService
    {
        int Add(ProjectViewModel Entity, string UserName);

        void Delete(int Id);
        void Update(ProjectViewModel viewModel);

        ProjectViewModel Get(int Id);

        IEnumerable<ProjectViewModel> GetAll();

        void AddCommentary(int id, string CommentaryText, string UserName);

        IEnumerable<Commentary> GetCommentaries(int ProjectID);

        void AddContent(int id, string Content, string UserName);

        Content GetContent(int ProjectID);

        //void UpdateCommentaries(ProjectViewModel projectViewModel, List<Commentary> commentaries);
    }
}
