using GimmeTheLoot.Data;
using GimmeTheLoot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GimmeTheLoot.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext context;

        public ChatHub(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task SendMessage(string user, string message, string projid)
        {
                int id = Convert.ToInt16(projid);
                var project = context.Projects.Where(a => a.Id == id).First();
                project.Comments = new List<Commentary>();
                GetCommentaries(project.Id);

                AddCommentary(project, message, user);

                //context.Projects.Update(project);
                await context.SaveChangesAsync();

                await Clients.All.SendAsync("ReceiveMessage", user, message);
            

        }

        public void GetCommentaries(int projid)
        {
            context.Commentaries.Where(a => a.ProjectId == projid).ToList();
        }
        public void AddCommentary(Project project, string NewCommentary, string UserName)
        {
            Commentary commentary = new Commentary
            {
                Project = project,
                ProjectId = project.Id,
                Text = NewCommentary,
                Date = DateTime.Now,
                UserName = UserName,
            };

            var AspNetUser = context.AspNetUsers.Where(a => a.UserName == UserName).First();
            commentary.AspNetUser = AspNetUser;
            commentary.AspNetUserId = AspNetUser.Id;

            project.Comments.Add(commentary); //= new List<Commentary> { commentary };

        }
    }
}
