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
    public class OldAlumniUsersController : Controller
    {
        private readonly TrackerContext _context;

        public OldAlumniUsersController(TrackerContext context)
        {
            _context = context;
        }

        // GET: AlumniUsers
        public async Task<IActionResult> Index()
        {
              return _context.AlumniUsers != null ? 
                          View(await _context.AlumniUsers.ToListAsync()) :
                          Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
        }

        [HttpPost]
        public IActionResult Index(string SearchPhrase, string type)
        {
            if (!GeneralInput(SearchPhrase)) return View(); // Returns complete index, may be bad?
            if (!GeneralInput(type)) return View();         // Again Returns complete Index 

            return View(SearchHelper(SearchPhrase, type));
        }
        public IEnumerable<AlumniUser> SearchHelper(string Phrase, string Type)
        {
            if (Phrase != null)
            {
                switch (Type)
                {
                    case "studentid": //
                        return (_context.AlumniUsers
                            .Where(c => c.StudentId.ToLower() == Phrase.ToLower()));

                    case "name":
                        return (_context.AlumniUsers
                            .Where(c => c.Name.ToLower().Contains(Phrase.ToLower())));

                    case "employer":
                        return (_context.AlumniUsers
                            .Where(c => c.EmployerName.ToLower().Contains(Phrase.ToLower())));

                    default:
                        return _context.AlumniUsers.ToList(); // Returns Full List, which is bad
                }
            }
            return _context.AlumniUsers.ToList(); // Returns Full list

        }
        // GET: AlumniUsers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (alumniUser == null)
            {
                return NotFound();
            }

            return View(alumniUser);
        }

        // GET: AlumniUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AlumniUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,AdminType,DateModified,Address,City,State,Zip,Phone,IsAdmin")] AlumniUser alumniUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumniUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumniUser);
        }

        public string GetName(int id)
        {
            //some code to get alumniuser identity user
            if (id == null || _context.AlumniUsers == null)
            {
                return "not found";
            }
            return "yes";

        }

        // GET: AlumniUsers/Edit/5
        public async Task<IActionResult> Edit(string? id)
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
            return View(alumniUser);
        }

        // POST: AlumniUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,AdminType,DateModified,Address,City,State,Zip,Phone,IsAdmin")] AlumniUser alumniUser)
        {
            if (id != alumniUser.StudentId)
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
                    if (!AlumniUserExists(alumniUser.StudentId))
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
            return View(alumniUser);
        }

        // GET: AlumniUsers/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (alumniUser == null)
            {
                return NotFound();
            }

            return View(alumniUser);
        }

        // POST: AlumniUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
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

        private bool AlumniUserExists(string id)
        {
          return (_context.AlumniUsers?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
