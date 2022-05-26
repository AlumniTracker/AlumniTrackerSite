using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlumniTrackerSite.Contexts;
using AlumniTrackerSite.Models;
using static AlumniTrackerSite.Data.Security;


namespace AlumniTrackerSite
{
    public class SCAFFOLDAlumniUsersController : Controller
    {
        private readonly TrackerContext _context;

        public SCAFFOLDAlumniUsersController(TrackerContext context)
        {
            _context = context;
        }

        // GET: AlumniUsers
        public async Task<IActionResult> Index()
        {
            var trackerContext = _context.AlumniUsers.Include(a => a.IdNavigation);
            return View(await trackerContext.ToListAsync());
        }

        // GET: AlumniUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.AlumniId == id);
            if (alumniUser == null)
            {
                return NotFound();
            }

            return View(alumniUser);
        }

        // GET: AlumniUsers/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Set<AspNetUsers>(), "Id", "Id");
            return View();
        }

        // POST: AlumniUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumniUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Set<AspNetUsers>(), "Id", "Id", alumniUser.Id);
            return View(alumniUser);
        }

        // GET: AlumniUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers.FindAsync(id);
            if (alumniUser == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Set<AspNetUsers>(), "Id", "Id", alumniUser.Id);
            return View(alumniUser);
        }

        // POST: AlumniUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser)
        {
            if (id != alumniUser.AlumniId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumniUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumniUserExists(alumniUser.AlumniId))
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
            ViewData["Id"] = new SelectList(_context.Set<AspNetUsers>(), "Id", "Id", alumniUser.Id);
            return View(alumniUser);
        }

        // GET: AlumniUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.AlumniId == id);
            if (alumniUser == null)
            {
                return NotFound();
            }

            return View(alumniUser);
        }

        // POST: AlumniUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AlumniUsers == null)
            {
                return Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
            }
            var alumniUser = await _context.AlumniUsers.FindAsync(id);
            if (alumniUser != null)
            {
                _context.AlumniUsers.Remove(alumniUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlumniUserExists(int id)
        {
          return (_context.AlumniUsers?.Any(e => e.AlumniId == id)).GetValueOrDefault();
        }
    }
}
