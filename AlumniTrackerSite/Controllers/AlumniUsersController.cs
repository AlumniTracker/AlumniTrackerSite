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
using System.Text.RegularExpressions;
using System.Text;

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
        [Authorize(Roles = "SuperAdmin")]
        public FileStreamResult CSV()
        {
            // Would combine these, except this method can not pass _context without being non static
            return CSVBuilder.MakeCSV(_context); ;
        }
        // GET: AlumniUsers
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()//replace this to be search, and then use general input?
        {
            return _context.GetAlumnis() != null ?
                        View(_context.GetAlumnis()) :
                        Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index(string SearchPhrase, string type)
        {
            return View(SearchHelper(SearchPhrase, type));
        }
        public IEnumerable<Alumnis> SearchHelper(string Phrase, string Type)
        {
            // currently there is no error being sent to the user. So these are confusing to a user.
            if (Phrase == null || Phrase == "") return _context.GetAlumnis();
            if (!GeneralInput(_logger, Phrase)) return _context.GetAlumnis();
            if (!GeneralInput(_logger, Type)) return  _context.GetAlumnis();


            // Set up Regex, this only allows * to be used as a single character wildcard.
            Phrase = Phrase.Replace("*", ".").ToLower();
            Regex reg = new Regex(Phrase);

            if (Phrase != null)
            {
                switch (Type)
                {
                    case "studentid":   // find alumni who have this exact studentid
                        return (_context.GetAlumnis()
                            .Where(c => c.StudentId.ToLower() == Phrase.ToLower()));

                    case "name":
                        return (_context.GetAlumnis()
                            .Where(c => reg.IsMatch(c.Name.ToLower())));

                        // all of these search terms can be null. Which will throw an exception if not handled
                    case "employer":
                        return (_context.GetAlumnis()
                            .Where(c =>
                            {
                                if(c.EmployerName != null) 
                                { return reg.IsMatch(c.EmployerName.ToLower()); }
                                return false;
                            }));

                    case "yeargrad":
                        return (_context.GetAlumnis()
                            .Where(c =>
                            {
                                if (c.YearGraduated != null) 
                                { return reg.IsMatch(c.YearGraduated.ToLower()); }
                                return false;
                            }));

                    case "degree":
                        return (_context.GetAlumnis()
                            .Where(c =>
                            {
                                if (c.Degree != null) 
                                { return reg.IsMatch(c.Degree.ToLower()); }
                                return false;
                            }));

                    default:
                        return _context.GetAlumnis();
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create() // unused page
        {
            return View();
        }


// Not in use and is not functional, currently the register page is used instead
        //// POST: AlumniUsers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,SuperAdmin")]
        //public async Task<IActionResult> Create([Bind("StudentId,Name,EmployerName,FieldofEmployment,YearGraduated,Degree,Notes,DateModified,Address,City,State,Zip,Phone,AlumniId,Id")] AlumniUser alumniUser, [Bind("Email")] string  email)
        //{
        //    if (!CheckInputs(_logger, alumniUser))
        //    { return View(); } // Change to error
        //    if (ModelState.IsValid)
        //    {
        //        alumniUser.DateModified = DateTime.Today;
        //        alumniUser.Id = _userManager.GetUserId(User);
        //        if(_context.AlumniUsers.Where(m => m.Id.Equals(alumniUser.Id)).Any() || alumniUser.Id == null)
        //        {
        //            //return 
        //        }
        //        _context.Add(alumniUser);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(alumniUser);
        //}

        public string GetName(int id)
        {
            //some code to get alumniuser identity user
            if (id == 0 || _context.AlumniUsers == null)
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.AlumniUsers == null)
            {
                return Problem("Entity set 'TrackerContext.AlumniUsers'  is null.");
            }
            AlumniUser? alumniUser = await _context.AlumniUsers.Where(c => c.AlumniId == id).FirstOrDefaultAsync();
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
