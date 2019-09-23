using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Entities.Entities;
using Data.EntityFrameWork;
using System.Security.Claims;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DataBaseContext _context;

        public ProjectController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Projects.Include(p => p.AspNetUsers);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.AspNetUsers)
                .FirstOrDefaultAsync(m => m.Id == id);

            project.Comments = new List<Commentary>();
            var commentaries = GetCommentaries(project.Id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        [HttpPost]
        public async Task<IActionResult> Details(Project project, string NewCommentary)
        {
            project.Comments = new List<Commentary>();
            var commentaries = GetCommentaries(project.Id);
            foreach (var commentary in commentaries)
            {
                project.Comments.Add(commentary);
            }

            AddCommentary(project, NewCommentary);
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Created,Subject,ExpirationDate,AspNetUserId")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.AspNetUserId);
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["AspNetUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.AspNetUserId);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Created,Subject,ExpirationDate,AspNetUserId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.AspNetUsers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
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

        public void AddCommentary(Project project, string NewCommentary)
        {
            Commentary commentary = new Commentary
            {
                Project = project,
                ProjectId = project.Id,
                Text = NewCommentary,
                Date = DateTime.Now,
                AspNetUsers = _context.AspNetUsers.Where(a => a.UserName == HttpContext.User.Identity.Name).First(),
                AspNetUserId = _context.AspNetUsers.Where(a => a.UserName == HttpContext.User.Identity.Name).First().Id
            };
            project.Comments.Add(commentary); //= new List<Commentary> { commentary };
        }
        public List<Commentary> GetCommentaries(int projid)
        {
            var commentaries = _context.Commentaries.Where(a => a.ProjectId == projid).ToList();
            return commentaries;
        }
    }
}
