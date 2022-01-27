using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApp;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers();
builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
    //endpoints.MapWebService();
    endpoints.MapControllers();
});

SeedData.SeedDatabase(app.Services
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<DataContext>());

app.Run();