using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.ViewModels;
using Data.ViewModels.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;

        //ivate readonly 

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            var project = projectService.Get(id);

            project.Comments = new List<Commentary>();

            var commentaries = projectService.GetCommentaries(id);
            foreach (var commentary in commentaries)
            {
                project.Comments.Add(commentary);//Add(new Commentary(commentary));
            }


            var content = projectService.GetContent(id);
            project.Content = content;

            return View(project);
        }

        [HttpPost]
        public ActionResult Details(int idofproj, string NewCommentary, string UserName)
        {
            projectService.AddCommentary(idofproj, NewCommentary, UserName);

            var project = projectService.Get(idofproj);

            project.Comments = new List<Commentary>();

            var commentaries = projectService.GetCommentaries(idofproj);
            foreach (var commentary in commentaries)
            {
                project.Comments.Add(commentary);
            }

            var content = projectService.GetContent(idofproj);
            project.Content = content;

            return View(project);//RedirectToAction("GetAll");
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectViewModel projectView, string Content, string UserName)
        {   
            var projectId = projectService.Add(projectView, UserName);

            projectService.AddContent(projectId, Content, UserName); 

            return RedirectToAction("GetAll");
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            var project = projectService.Get(id);

            var content = projectService.GetContent(id);
            project.Content = content;

            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectViewModel viewModel, string NewContent)
        {
            //viewModel.Content = new Content();
            //viewModel.Content.Text = NewContent;

            projectService.Update(viewModel);



            return RedirectToAction("GetAll");
        }

        public IActionResult GetAll()
        {
            var prpjects = projectService.GetAll();
            return View(prpjects);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            projectService.Delete(id);
            return RedirectToAction("GetAll");
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}