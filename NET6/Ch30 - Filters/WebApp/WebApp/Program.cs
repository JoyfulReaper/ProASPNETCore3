using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApp;
using WebApp.Models;
using WebApp.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo { Title = "WebApp", Version = "v1" });
});


builder.Services.AddSingleton<CitiesData>();

builder.Services.Configure<RazorPagesOptions>(opts =>
{
    opts.Conventions.AddPageRoute("/Index", "/extra/page/{id:long?}");
});

builder.Services.Configure<AntiforgeryOptions>(opts =>
{
    opts.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.Configure<MvcOptions>(opts =>
    opts.ModelBindingMessageProvider
        .SetValueMustNotBeNullAccessor(value => "Please enter a value"));

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseMiddleware<TestMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute("forms",
        "controllers/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
});

var antiforgery = app.Services.CreateScope()
    .ServiceProvider
    .GetRequiredService<IAntiforgery>();

app.Use(async (context, next) =>
{
    if(!context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.Cookies.Append("XSRF-TOKEN",
            antiforgery.GetAndStoreTokens(context).RequestToken,
            new CookieOptions { HttpOnly = false });
    }
    await next();
});

SeedData.SeedDatabase(app.Services
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<DataContext>());

app.Run();