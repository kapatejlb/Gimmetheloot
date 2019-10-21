using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GimmeTheLoot.Data;
using GimmeTheLoot.Models;
using System.Security.Claims;

namespace GimmeTheLoot.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        //particular
        public ActionResult WriteProject()
        {
            return PartialView("_Project");
        }

        public ActionResult Connentaries()
        {
            return PartialView("_Commentaries");
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projects.Include(p => p.AspNetUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.AspNetUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            GetContent(id);

            project.Comments = new List<Commentary>();
            GetCommentaries(project.Id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, string NewCommentary, string UserName)
        {
            var project = _context.Projects.Where(a => a.Id == id).First();

            GetContent(id);

            //project.Comments = new List<Commentary>();
            //GetCommentaries(project.Id);

            //AddCommentary(project, NewCommentary, UserName);
            //_context.Projects.Update(project);
            //await _context.SaveChangesAsync();
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]////////////////////////////////////////////////////
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Created,Subject,ExpirationDate,AspNetUserId")] Project project, string ContentText)
        {
            if (ModelState.IsValid)
            {
                //project.AspNetUserId = User.Identity.GetUserId();
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                project.AspNetUserId = currentUserID;


                _context.Add(project);
                await _context.SaveChangesAsync();

                AddContent(project.Id, ContentText);

                return RedirectToAction(nameof(Index));
            }
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.AspNetUserId);
            return View(project);
        }////////////////////////////////////////////////////////////

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            GetContent(id);

            if (project == null)
            {
                return NotFound();
            }
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.AspNetUserId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Created,Subject,ExpirationDate,AspNetUserId")] Project project, string ContentText)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Content content = _context.Contents.Where(a => a.ProjectId == id).ToList().First();

                    content.Date = DateTime.Now;
                    content.Text = ContentText;

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.AspNetUserId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.AspNetUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
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

            var AspNetUser = _context.AspNetUsers.Where(a => a.UserName == HttpContext.User.Identity.Name).First();
            commentary.AspNetUser = AspNetUser;
            commentary.AspNetUserId = AspNetUser.Id;

            project.Comments.Add(commentary); //= new List<Commentary> { commentary };
        }
        public void GetCommentaries(int projid)
        {
            _context.Commentaries.Where(a => a.ProjectId == projid).ToList();
        }

        public void AddContent(int projid, string ContentText)
        {
            var content = new Content
            {
                Date = DateTime.Now,
                ProjectId = projid,
                Text = ContentText,
                Project = _context.Projects.Find(projid),
            };

            _context.Contents.Add(content);
            _context.SaveChanges();
        }

        public void GetContent(int? projid)
        {
            _context.Contents.Where(a => a.ProjectId == projid).ToList();
        }
    }
}
