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
using AlumniTrackerSite.Data;

namespace AlumniTrackerSite
{
    public class AlumniUsersController : Controller
    {
        private readonly TrackerContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AlumniUsersController> _logger;
        RoleManager<IdentityRole> _roleManager;

        public AlumniUsersController(TrackerContext context, UserManager<IdentityUser> userManager, ILogger<AlumniUsersController> logger, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }
        public FileStreamResult CSV()
        {
            string filepath = Directory.GetCurrentDirectory();
            filepath += @"\wwwroot\CSVS\Alum.csv";
            CSVBuilder.MakeCSV(_context, filepath);
            FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(file, "text/csv");
            //MimeType
        }
        // GET: AlumniUsers
        [Authorize] //(Roles = "Admin,SuperAdmin")
        public async Task<IActionResult> Index()//replace this to be search, and then use general input?
        {
            return _context.GetAlumnis() != null ?
                        View(_context.GetAlumnis()) :
                        Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
        }

        [HttpPost]
        [Authorize]//(Roles = "Admin,SuperAdmin")
        public IActionResult Index(string SearchPhrase, string type)
        {
            return View(SearchHelper(SearchPhrase, type));
        }
        public IEnumerable<Alumnis> SearchHelper(string Phrase, string Type)
        {
            if(!GeneralInput(_logger, Phrase)) return new List<Alumnis>();
            if (!GeneralInput(_logger, Type)) return new List<Alumnis>();

            if (Phrase != null)
            {
                switch (Type)
                {
                    case "studentid":   // find alumni who have this exact studentid
                        return (_context.GetAlumnis()
                            .Where(c => c.StudentId.ToLower() == Phrase.ToLower()));

                    case "name":
                        return (_context.GetAlumnis()
                            .Where(c => c.Name.ToLower().Contains(Phrase.ToLower())));

                    case "employer":
                        return (_context.GetAlumnis()
                            .Where(c => c.EmployerName.ToLower().Contains(Phrase.ToLower())));

                    case "yeargrad":
                        return (_context.GetAlumnis()
                            .Where(c => c.YearGraduated.ToLower().Contains(Phrase.ToLower())));

                    case "degreepath":
                        return (_context.GetAlumnis()
                            .Where(c => c.Degree.ToLower().Contains(Phrase.ToLower())));

                    default:
                        return _context.GetAlumnis(); 
                        // Returns Full List
                        //case "studentid":   // find alumni who have this exact studentid
                        //    return (_context.AlumniUsers
                        //        .Where(c => c.StudentId.ToLower() == Phrase.ToLower()));

                        //case "name":        
                        //    return (_context.AlumniUsers
                        //        .Where(c => c.Name.ToLower().Contains(Phrase.ToLower())));

                        //case "employer":    
                        //    return (_context.AlumniUsers
                        //        .Where(c => c.EmployerName.ToLower().Contains(Phrase.ToLower())));

                        //case "yeargrad":    
                        //    return (_context.AlumniUsers
                        //        .Where(c => c.YearGraduated.ToLower().Contains(Phrase.ToLower())));

                        //case "degreepath": 
                        //    return (_context.AlumniUsers
                        //        .Where(c => c.Degree.ToLower().Contains(Phrase.ToLower())));

                        //default:
                        //    return _context.AlumniUsers.ToList(); // Returns Full List
                }
            }
            return _context.GetAlumnis(); // Returns Full list

        }
        // GET: AlumniUsers/Details/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(string? idString)
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
        [Authorize]//(Roles = "Admin,SuperAdmin")
        public IActionResult Create() // unused page
        {
            return View();
        }

        // POST: AlumniUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]//(Roles = "Admin,SuperAdmin")
        public async Task<IActionResult> Create([Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser, [Bind("Email")] string  email)
        {
            if (!CheckInputs(_logger, alumniUser))
            { return View(); } // Change to error
            if (ModelState.IsValid)
            {
                alumniUser.DateModified = DateTime.Today;
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
                    alumniUser.DateModified = DateTime.Today;
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
        [Authorize]//(Roles = "Admin,SuperAdmin")
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AlumniUsers == null)
            {
                return NotFound();
            }

            var alumniUser = await _context.AlumniUsers
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
        [Authorize]//(Roles = "Admin,SuperAdmin")
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.AlumniUsers == null)
            {
                return Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
            }
            //FindAsync(id);
            AlumniUser? alumniUser = await _context.AlumniUsers.Where(c => c.AlumniId == c.AlumniId).FirstOrDefaultAsync();
            if (alumniUser != null)
            {
                 IdentityUser user = await _userManager.FindByIdAsync(alumniUser.Id);

                _context.AlumniUsers.Remove(alumniUser);
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public string SmolDate(DateTime dateToSmol)
        {
            string smolDate = dateToSmol.ToString("mm/dd/yyyy");
            return smolDate;
        }
        private bool AlumniUserExists(string id)
        {
          return (_context.AlumniUsers?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
