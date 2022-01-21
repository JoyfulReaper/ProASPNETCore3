using CorePlatformCh12;
using PlatformCh16;
using DICh14.Services;
using DataCh17;
using DataCh17.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDistributedMemoryCache(opts =>
//{
//    opts.SizeLimit = 200;
//});

builder.Services.AddDistributedSqlServerCache(opts =>
{
    opts.ConnectionString
        = builder.Configuration["ConnectionStrings:CacheConnection"];
    opts.SchemaName = "dbo";
    opts.TableName = "DataCache";
});

builder.Services.AddResponseCaching();
builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

builder.Services.AddDbContext<CalculationContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:CalcConnection"]);
    if(builder.Environment.IsDevelopment())
    {
        opts.EnableSensitiveDataLogging(true);
    }
});

builder.Services.AddTransient<SeedData>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseResponseCaching();
//app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapEndpoint<SumEndpoint>("/sum/{count:int=2000000000}");

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

bool cmdLineInit = (app.Configuration["INITDB"] ?? "false") == "true";
if (app.Environment.IsDevelopment() || cmdLineInit)
{
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<SeedData>()
            .SeedDatabase();
    }
    if (cmdLineInit)
    {
        app.Lifetime.StopApplication();
    }
}

app.Run();