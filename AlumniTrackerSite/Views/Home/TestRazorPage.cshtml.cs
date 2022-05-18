using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlumniTrackerSite.Views.Home
{
    [Authorize]
    
    public class TestRazorPage : PageModel
    {
        
        private readonly ILogger<TestRazorPage> _logger;
        public TestRazorPage(ILogger<TestRazorPage> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
