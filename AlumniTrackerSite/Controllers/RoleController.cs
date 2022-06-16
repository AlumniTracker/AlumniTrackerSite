using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AlumniTrackerSite.Models;
using AlumniTrackerSite.Data;
using AlumniTrackerSite.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlumniTrackerSite.Controllers
{
    public class RoleController : Controller
    {
        private readonly TrackerContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AlumniUsersController> _logger;
        private RoleManager<IdentityRole> roleManager;

        public RoleController(TrackerContext context, UserManager<IdentityUser> userManager, ILogger<AlumniUsersController> logger, RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            roleManager = roleMgr;
        }

        //[Authorize(Policy = "writepolicy")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();
            foreach (IdentityUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
        //[Authorize(Policy = "writepolicy")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    IdentityUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    IdentityUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }                
            }
            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId);
        }

        //[Authorize(Policy = "readpolicy")]
        [Authorize(Roles = "SuperAdmin")]
        public ViewResult Index() => View(roleManager.Roles);
        // GET: RoleController
        //public ActionResult Index()
        //{
        //    var Roles = roleManager.Roles.ToList();
        //    return View(Roles);
        //}

        // GET: RoleController/Details/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Details(int id)
        {
            return View(id);
        }

        // GET: RoleController/Create
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            return View(new IdentityRole());
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create(IdentityRole Role
            )
        {
            _context.Roles.Add(Role);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: RoleController/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int id)
        {
            return View(id);
        }

        // POST: RoleController/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }
        [Authorize(Roles = "SuperAdmin")]
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
