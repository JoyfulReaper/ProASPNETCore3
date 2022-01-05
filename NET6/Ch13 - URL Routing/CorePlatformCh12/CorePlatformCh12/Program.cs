using UrlRoutingCh13;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage();

//app.UseMiddleware<Population>();
//app.UseMiddleware<Capital>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("routing", async context =>
    {
        await context.Response.WriteAsync("Request was Routed");
    });

    endpoints.MapGet("{first}/{second}/{third}", async context =>
    {
        await context.Response.WriteAsync("Request was Routed\n");
        foreach (var kvp in context.Request.RouteValues)
        {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
        }
    });

    endpoints.MapGet("capital/{country}", Capital.Endpoint);
    endpoints.MapGet("population/{city}", Population.Endpoint)
        .WithMetadata(new RouteNameMetadata("population"));

    //endpoints.MapGet("capital/uk", new Capital().Invoke);
    //endpoints.MapGet("population/paris", new Population().Invoke);
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Terminal Middleware Reached");
});

app.Run();
