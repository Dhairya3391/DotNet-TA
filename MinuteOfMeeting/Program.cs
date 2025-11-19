using Microsoft.AspNetCore.Session;
using MinuteOfMeeting.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".MOM.Session";
});

// Configure Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.OnAppendCookie = cookieContext =>
    {
        if (cookieContext.CookieOptions.SameSite == SameSiteMode.None)
        {
            cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
        }
    };
    options.OnDeleteCookie = cookieContext =>
    {
        if (cookieContext.CookieOptions.SameSite == SameSiteMode.None)
        {
            cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Use Cookie Policy
app.UseCookiePolicy();

// Use Static Files
app.UseStaticFiles();

// Use Session (must be before UseRouting and UseAuthorization)
app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Default route - redirect to Account/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Additional routes for better organization
app.MapControllerRoute(
    name: "dashboard",
    pattern: "{controller=Dashboard}/{action=Index}");

app.MapControllerRoute(
    name: "meetingtype",
    pattern: "meetingtype/{action=Index}/{id?}",
    defaults: new { controller = "MeetingType" });

app.MapControllerRoute(
    name: "department",
    pattern: "department/{action=Index}/{id?}",
    defaults: new { controller = "Department" });

app.MapControllerRoute(
    name: "meetingvenue",
    pattern: "venue/{action=Index}/{id?}",
    defaults: new { controller = "MeetingVenue" });

app.MapControllerRoute(
    name: "staff",
    pattern: "staff/{action=Index}/{id?}",
    defaults: new { controller = "Staff" });

app.MapControllerRoute(
    name: "meeting",
    pattern: "meeting/{action=Index}/{id?}",
    defaults: new { controller = "Meeting" });

app.MapControllerRoute(
    name: "meetingmember",
    pattern: "attendance/{action=Index}/{id?}",
    defaults: new { controller = "MeetingMember" });

// Run the application
app.Run();
