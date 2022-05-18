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
    public class AlumniUsersController : Controller
    {
        private readonly TrackerContext _context;

        public AlumniUsersController(TrackerContext context)
        {
            _context = context;
        }

        // GET: AlumniUsers
        public async Task<IActionResult> Index()//replace this to be search, and then use general input?
        {
              return _context.AlumniUsers != null ? 
                          View(await _context.AlumniUsers.ToListAsync()) :
                          Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
        }

        // GET: AlumniUsers/Details/5
        public async Task<IActionResult> Details(string? id)// be able to map random numbers to an id per session
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
            if (CheckInputs(alumniUser))
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                _context.Add(alumniUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumniUser);
        }
        public static bool CheckInputs(AlumniUser alumniUser)
        {
            bool[] goodInput = new bool[11];
            goodInput[0] = GeneralInput(alumniUser.Name);
            goodInput[1] = GeneralInput(alumniUser.EmployerName);
            goodInput[2] = GeneralInput(alumniUser.FieldofEmployment);
            goodInput[3] = NumericalInput(alumniUser.YearGraduated);
            goodInput[4] = GeneralInput(alumniUser.Degree);
            goodInput[5] = GeneralInput(alumniUser.Notes);
            goodInput[6] = GeneralInput(alumniUser.Address);
            goodInput[7] = GeneralInput(alumniUser.City);
            goodInput[8] = GeneralInput(alumniUser.State);
            goodInput[9] = NumericalInput(alumniUser.Zip);
            goodInput[10] = PhoneInput(alumniUser.Phone);
            //PhoneInput(alumniUser.PhoneNumber); //ASP NET Identity Phone Number




            if (!goodInput.Contains(false))
            {
                return true;
            }
            return false;
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
