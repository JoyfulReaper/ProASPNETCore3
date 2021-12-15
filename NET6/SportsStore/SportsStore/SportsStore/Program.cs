using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

var app = builder.Build();

app.UseDeveloperExceptionPage(); // Show detailed exception information
app.UseStatusCodePages(); // Simple messages to HTTP responses that normally would not have a body
app.UseStaticFiles(); // Server static content from wwwroot

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

SeedData.EnsurePopulated(app);

app.Run();
