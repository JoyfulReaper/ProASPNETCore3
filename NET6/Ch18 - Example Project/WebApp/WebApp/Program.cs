using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApp;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

//builder.Services.AddControllers();
//.AddNewtonsoftJson()
//.AddXmlSerializerFormatters();

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});

//builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
//{
//    opts.SerializerSettings.NullValueHandling
//        = Newtonsoft.Json.NullValueHandling.Ignore;
//});

//builder.Services.Configure<MvcOptions>(opts =>
//{
//    opts.RespectBrowserAcceptHeader = true; // Disable JSON fall back when header contains */*
//    opts.ReturnHttpNotAcceptable = true; // Disable JSON fall back when unsupported content is requested
//});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo { Title = "WebApp", Version = "v1" });
});

//builder.Services.Configure<JsonOptions>(opts =>
//{
//    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//});

builder.Services.AddSingleton<CitiesData>();

builder.Services.Configure<RazorPagesOptions>(opts =>
{
    opts.Conventions.AddPageRoute("/Index", "/extra/page/{id:long?}");
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseMiddleware<TestMiddleware>();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapGet("/", async context =>
    //{
    //    await context.Response.WriteAsync("Hello World");
    //});
    //endpoints.MapWebService();
    endpoints.MapControllers();
    //endpoints.MapControllerRoute("Default",
    //    "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
});

SeedData.SeedDatabase(app.Services
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<DataContext>());

app.Run();