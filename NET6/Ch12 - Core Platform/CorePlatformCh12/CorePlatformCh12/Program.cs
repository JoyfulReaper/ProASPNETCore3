using CorePlatformCh12;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MessageOptions>(options =>
{
    options.CityName = "Albany";
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<LocationMiddleWare>();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/location")
    {
        MessageOptions opts = app.Services.GetRequiredService<IOptions<MessageOptions>>().Value;
        await context.Response.WriteAsync($"{opts.CityName}, {opts.CountryName}");
    }
    else
    {
        await next();
    }
});

app.Map("/branch", (branch) =>
{
    // Terminal Middleware
    branch.Run(new QueryStringMiddleWare().Invoke);
});

app.Map("/branch", (branch) =>
{
    branch.UseMiddleware<QueryStringMiddleWare>();

    // Run is used with terminal middleware, next delegate is not called
    branch.Run(async (context) =>
    {
        await context.Response.WriteAsync($"Branch Middleware");
    });
});

app.Use(async (context, next) =>
{
    await next();
    await context.Response.WriteAsync($"\nStatus Code: {context.Response.StatusCode}");
});

app.Use(async (context, next) =>
{
    if(context.Request.Path == "/short")
    {
        await context.Response.WriteAsync("Request Short Circuited!");
    } else
    {
        await next();
    }
});

app.Use(async (context, next) =>
{
    if(context.Request.Method == HttpMethods.Get
        && context.Request.Query["custom"] == "true")
    {
        await context.Response.WriteAsync("Custom Middleware \n");
    }
    await next();
});


app.UseMiddleware<QueryStringMiddleWare>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();
