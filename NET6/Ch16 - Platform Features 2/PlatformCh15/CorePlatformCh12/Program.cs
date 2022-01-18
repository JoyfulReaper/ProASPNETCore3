using CorePlatformCh12;
using Microsoft.Extensions.FileProviders;
using PlatformCh16;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(opts =>
{
    opts.CheckConsentNeeded = context => true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddHsts(opts =>
{
    opts.MaxAge = TimeSpan.FromDays(1);
    opts.IncludeSubDomains = true;
});

var app = builder.Build();

if(app.Environment.IsProduction())
{
    app.UseHsts();
}

//app.UseDeveloperExceptionPage(); // In .NET 6 Developer Exception pages are ALWAYS used in development environment.
// There does not seem to be a way to disable them

// Can be over ridden though with an exception handler
app.UseExceptionHandler("/error.html");

app.UseStatusCodePages("text/html", Responses.DefaultResponse);

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseStaticFiles();
app.UseMiddleware<ConsentMiddleware>();
app.UseSession();
app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/error")
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await Task.CompletedTask;
    } else
    {
        await next();
    }
});

app.Run(context =>
{
    throw new Exception("Something went wrong!!");
});

app.Use(async (context, next) =>
{
    await next();
    await context.Response
        .WriteAsync($"\nHTTPS Request: {context.Request.IsHttps}\n");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/session", async context =>
    {
        int counter1 = (context.Session.GetInt32("counter1") ?? 0) + 1;
        int counter2 = (context.Session.GetInt32("counter2") ?? 0) + 1;

        context.Session.SetInt32("counter1", counter1);
        context.Session.SetInt32("counter2", counter2);

        await context.Session.CommitAsync();

        await context.Response
            .WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");
    });

    endpoints.MapGet("/cookie", async context =>
    {
        int counter1 = 
            int.Parse(context.Request.Cookies["counter1"] ?? "0") + 1;
        context.Response.Cookies.Append("counter1", counter1.ToString(),
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(30),
                IsEssential = true,
            });

        int counter2 = int.Parse(context.Request.Cookies["counter2"] ?? "0") + 1;
        context.Response.Cookies.Append("counter2", counter1.ToString(),
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(30)
            });

        await context.Response
            .WriteAsync($"Counter 1: {counter1}, Counter2: {counter2}");
    });

    endpoints.MapGet("clear", context =>
    {
        context.Response.Cookies.Delete("counter1");
        context.Response.Cookies.Delete("counter2");
        context.Response.Redirect("/");
        return Task.CompletedTask;
    });

    endpoints.MapFallback(async context =>
    {
        await context.Response.WriteAsync("Hello World");
    });
});

app.Run();
