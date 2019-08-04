using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Web.SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IProjectService projectService;

        //ivate readonly 

        public ChatHub(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public async Task SendMessage(string user, string message, DateTime data, int projectid)//, string UserName)
        {
            projectService.AddCommentary(projectid, message, "qwe@mail.ru");
            await Clients.All.SendAsync("ReceiveMessage", user, message, data.ToString("d") );
        }
    }
}
