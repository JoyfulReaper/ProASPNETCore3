using Advanced.Models;
using Microsoft.EntityFrameworkCore;
using Advanced.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:PeopleConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ToggleService>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddDbContext<IdentityContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequiredLength = 3;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
});

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts =>
{
    opts.Events.DisableRedirectionForPath(e => e.OnRedirectToLogin, "/api", StatusCodes.Status401Unauthorized);
    opts.Events.DisableRedirectionForPath(e => e.OnRedirectToAccessDenied, "/api", StatusCodes.Status403Forbidden);
}).AddJwtBearer(opts =>
{
    opts.RequireHttpsMetadata = false;
    opts.SaveToken = true;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration["jwtSecret"])),
        ValidateAudience = false,
        ValidateIssuer = false,
    };
    opts.Events = new JwtBearerEvents
    {
        OnTokenValidated = async ctx =>
        {
            var usrmgr = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var signinmgr = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
            string username = ctx.Principal.FindFirst(ClaimTypes.Name)?.Value;
            IdentityUser idUser = await usrmgr.FindByNameAsync(username);
            ctx.Principal =
                await signinmgr.CreateUserPrincipalAsync(idUser);
        }
    };
});

//builder.Services.Configure<CookieAuthenticationOptions>(
//    IdentityConstants.ApplicationScheme, opts =>
//    {
//        opts.LoginPath = "/Authenticate";
//        opts.AccessDeniedPath = "/NotAllowed";
//    });

var app = builder.Build();

app.UseBlazorFrameworkFiles("/webassembly");
app.MapFallbackToFile("/webassembly/{*path:nonfile}", "/webassembly/index.html");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("controllers", "controllers/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();

    endpoints.MapFallbackToPage("/_Host"); 
});

SeedData.SeedDatabase(app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>());
IdentitySeedData.CreateAdminAccount(app.Services, app.Configuration);

app.Run();
