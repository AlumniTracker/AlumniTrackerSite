using AlumniTrackerSite.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AlumniTrackerSite.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using AlumniTrackerSite.Services;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Conn") ?? throw new InvalidOperationException("Connection string 'Conn' not found.");

builder.Services.AddDbContext<AlumniIdentityContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AlumniIdentityContext>()
    // For admin accounts
    //.AddRoles<IdentityRole>()
    ; ;

// Add services to the container.
builder.Services.AddDbContext<TrackerContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); //states that this should not be used with multiple servers, currently a non issue

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Alumni.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(20); // to be at default time
    options.Cookie.IsEssential = true;
});

//builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Home controller error will be bad
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
