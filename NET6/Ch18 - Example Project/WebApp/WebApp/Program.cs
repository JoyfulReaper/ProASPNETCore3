using Microsoft.EntityFrameworkCore;
using WebApp;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseMiddleware<TestMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World");
    });
});

SeedData.SeedDatabase(app.Services
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<DataContext>());

app.Run();