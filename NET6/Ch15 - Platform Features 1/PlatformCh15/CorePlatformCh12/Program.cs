using CorePlatformCh12;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageOptions>(builder.Configuration.GetSection("Location"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider($"{app.Environment.ContentRootPath}/staticfiles"),
    RequestPath = "/files"
});
app.UseRouting();

app.UseMiddleware<LocationMiddleWare>();

app.Use(async (context, next) =>
{
    if(context.Request.Path == "/config")
    {
        string defaultDebug = app.Configuration["Logging:LogLevel:Default"];
        await context.Response
            .WriteAsync($"The config setting is {defaultDebug}");
        string environ = app.Configuration["ASPNETCORE_ENVIRONMENT"];
        await context.Response
            .WriteAsync($"\nThe env setting is: {environ}");

        string wsId = app.Configuration["WebService:Id"];
        string wsKey = app.Configuration["WebService:Key"];
        await context.Response.WriteAsync($"\nThe Secret ID is: {wsId}");
        await context.Response.WriteAsync($"\nThe Secret Key is: {wsKey}");
    } else
    {
        await next();
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        var logger = app.Logger;
        logger.LogDebug("Response for / started");
        await context.Response.WriteAsync("Hello World!");
        logger.LogDebug("Response for / completed");
    });
});

app.Run();
