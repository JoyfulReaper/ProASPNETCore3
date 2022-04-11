using Advanced.Models;
using Microsoft.EntityFrameworkCore;
using Advanced.Services;
using Microsoft.AspNetCore.ResponseCompression;

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

var app = builder.Build();

app.UseBlazorFrameworkFiles("/webassembly");

app.UseStaticFiles();
//app.UseStaticFiles("/webassembly");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("controllers", "controllers/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();

    endpoints.MapFallbackToFile("/webassembly/{*path:nonfile}","index.html");
    endpoints.MapFallbackToPage("/_Host"); 
});

SeedData.SeedDatabase(app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>());

app.Run();
