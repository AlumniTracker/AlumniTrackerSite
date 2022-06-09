// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlumniTrackerSite.Contexts;
using AlumniTrackerSite.Models;
using AlumniTrackerSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlumniTrackerSite.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private TrackerContext _context;
        public AlumniUser MyUser { get; set; }

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            TrackerContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public AlumniUser alumniUser { get
            {
                string id = _signInManager.UserManager.GetUserId(User);
                IEnumerable<AlumniUser> alum = _context.AlumniUsers.Where(x => x.Id.Equals(id));
                return alum.FirstOrDefault();
            }
            
        }
        public string Name { get 
            {
                // returns the Id field of the logged in user
                string signedInUser = _signInManager.UserManager.GetUserId(User);

                AlumniUser IdFromAlumniUserTable = (AlumniUser)_context.AlumniUsers.Where(c => c.Id == signedInUser).FirstOrDefault();
                return IdFromAlumniUserTable.Name;
                
                }
            
        }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        ///
        [Display(Name ="This is a name")]
        public string Username { get; set; }

        /// <summary>
        ///     Look WHAT I CAN DO!!!!!!! But alas, I was wrong
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            //[Required]
            //[DataType(DataType.Text)]
            //[Display(Name = "Full name")]
            //public string Name { get {  } }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var updatedUser = alumniUser;
            //var formName = Request.Form["alumniUser.Name"];
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);                
                return Page();
            }
            if (!Request.Form["alumniUser.Name"].Equals(updatedUser.Name))
                {
                    updatedUser.Name = Request.Form["alumniUser.Name"];
                }
            if (!Request.Form["alumniUser.Degree"].Equals(updatedUser.Degree))
            {
                updatedUser.Degree = Request.Form["alumniUser.Degree"];
            }
            if (!Request.Form["alumniUser.YearGraduated"].Equals(updatedUser.YearGraduated))
            {
                updatedUser.YearGraduated = Request.Form["alumniUser.YearGraduated"];
            }
            if (!Request.Form["alumniUser.Address"].Equals(updatedUser.Address))
            {
                updatedUser.Address = Request.Form["alumniUser.Address"];
            }
            if (!Request.Form["alumniUser.City"].Equals(updatedUser.City))
            {
                updatedUser.City = Request.Form["alumniUser.City"];
            }
            if (!Request.Form["alumniUser.State"].Equals(updatedUser.State))
            {
                updatedUser.State = Request.Form["alumniUser.State"];
            }
            if (!Request.Form["alumniUser.Zip"].Equals(updatedUser.Zip))
            {
                updatedUser.Zip = Request.Form["alumniUser.Zip"];
            }
            if (!Request.Form["alumniUser.Phone"].Equals(updatedUser.Phone))
            {
                updatedUser.Phone = Request.Form["alumniUser.Phone"];
            }
            if (!Request.Form["alumniUser.EmployerName"].Equals(updatedUser.EmployerName))
            {
                updatedUser.EmployerName = Request.Form["alumniUser.EmployerName"];
            }
            if (!Request.Form["alumniUser.FieldofEmployment"].Equals(updatedUser.FieldofEmployment))
            {
                updatedUser.FieldofEmployment = Request.Form["alumniUser.FieldofEmployment"];
            }
            if (!Request.Form["alumniUser.Notes"].Equals(updatedUser.Notes))
            {
                updatedUser.Notes = Request.Form["alumniUser.Notes"];
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            updatedUser.DateModified = DateTime.Today;
            _context.Update(updatedUser);
            _context.SaveChanges();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
