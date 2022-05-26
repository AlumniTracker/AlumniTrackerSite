using AlumniTrackerSite.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AlumniTrackerSite.Data;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Conn") ?? throw new InvalidOperationException("Connection string 'Conn' not found.");

builder.Services.AddDbContext<AlumniIdentityContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AlumniIdentityContext>();;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TrackerContext>();

builder.Services.AddDistributedMemoryCache(); //states that this should not be used with multiple servers, currently a non issue

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Alumni.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(20); // to be at default time
    options.Cookie.IsEssential = true;
});

//builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddRazorPages();

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
