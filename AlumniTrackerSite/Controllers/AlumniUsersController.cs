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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AlumniTrackerSite
{
    public class AlumniUsersController : Controller
    {
        private readonly TrackerContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AlumniUsersController(TrackerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AlumniUsers
        [Authorize]
        public async Task<IActionResult> Index()//replace this to be search, and then use general input?
        {
              return _context.AlumniUsers != null ? 
                          View(await _context.AlumniUsers.ToListAsync()) :
                          Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
        }
        
        [HttpPost]
        [Authorize]
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
        //public bool Mapper(int StudentID)
        //{
        //    Random random = new Random();
        //    string mapID = random.Next(1000000000).ToString(); 
        //    HttpContext.Session.SetString(mapID, StudentID.ToString()); 
        //    return true;
        //}
        //public int IdGetter(string? mapID)
        //{
        //    int StudentID;
        //    int.TryParse(HttpContext.Session.GetString(mapID), out StudentID);
        //    return StudentID;
        //}
        // GET: AlumniUsers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string? idString)// be able to map random numbers to an id per session
        {
            if (idString == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
                .FirstOrDefaultAsync(m => m.StudentId.Equals(idString));
            if (alumniUser == null)
            {
                return NotFound();
            }

            return View(alumniUser);
        }

        // GET: AlumniUsers/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AlumniUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser)
        {
            if (!CheckInputs(alumniUser))
            {
                return View(); // CHANGE TO ERROR
            }
            if (ModelState.IsValid)
            {
                alumniUser.DateModified = DateTime.Now;
                alumniUser.Id = _userManager.GetUserId(User);
                if(_context.AlumniUsers.Where(m => m.Id.Equals(alumniUser.Id)).Any() || alumniUser.Id == null)
                {
                    //return 
                }
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser)
        {
            if (id != alumniUser.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    alumniUser.DateModified = DateTime.Now;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AlumniUsers == null)
            {
                return Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
            }
            //FindAsync(id);
            AlumniUser? alumniUser = await _context.AlumniUsers.Where(c => c.StudentId == c.StudentId).FirstOrDefaultAsync();
            if (alumniUser != null)
            {
                 IdentityUser user = await _userManager.FindByIdAsync(alumniUser.Id);

                _context.AlumniUsers.Remove(alumniUser);
                _context.Users.Remove(user);
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
