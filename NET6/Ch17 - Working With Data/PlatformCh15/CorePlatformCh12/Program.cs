using CorePlatformCh12;
using PlatformCh16;
using DICh14.Services;
using DataCh17;

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

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapEndpoint<SumEndpoint>("/sum/{count:int=2000000000}");

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();
