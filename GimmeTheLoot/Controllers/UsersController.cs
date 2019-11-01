using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using Data.Entities.Entities;
//using Data.EntityFrameWork;
using Microsoft.AspNetCore.Authorization;
using GimmeTheLoot.Data;
using GimmeTheLoot.Models;

using GimmeTheLoot.Enums;

namespace GimmeTheLoot.Controllers
{
    [Authorize(Roles = "admin")]
    [ViewComponent]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int page;
        private int pageSize;
        private int recordsPerPage;
        private int TotalItemCount;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult Index()
        {

            var page = 1;


            var allusers = _context.AspNetUsers.ToList();
            var users = _context.Search(users: allusers, page: page, recordsPerPage: recordsPerPage, term: "", sortBy: SortBy.AddDate, sortOrder: SortOrder.Desc, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            return View(users);
        }


        public ActionResult Search(int page = 1, string term = "", SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)
        {
            //System.Threading.Thread.Sleep(700);

            var allusers = _context.AspNetUsers.Where(d => d.UserName != "123").ToList();
            var users = _context.Search(users: allusers, page: page, recordsPerPage: recordsPerPage, term: term, sortBy: sortBy, sortOrder: sortOrder, pageSize: out pageSize, TotalItemCount: out TotalItemCount);


            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            //return PartialView("_UsersList", users);
            return View("Index", users);
        }




        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }
            return View(aspNetUsers);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AspNetUser aspNetUser)
        {
            //if (id != aspNetUser.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(aspNetUser);

            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!AspNetUsersExists(aspNetUser.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(aspNetUser);

            var saved = false;
            while (!saved)
            {
                try
                {
                    // Attempt to save changes to the database
                    _context.Update(aspNetUser);
                    _context.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is AspNetUser)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);

                        }
                    }
                }
                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }

}
