using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlumniTrackerSite.Data;

public class AlumniIdentityContext : IdentityDbContext<IdentityUser>
{
    public AlumniIdentityContext(DbContextOptions<AlumniIdentityContext> options)
        : base(options)
    {
    }
    protected AlumniIdentityContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
