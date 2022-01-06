using UrlRoutingCh13;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.ConstraintMap.Add("countryName",
        typeof(CountryRouteConstraint));
});

var app = builder.Build();

app.UseDeveloperExceptionPage();

//app.UseMiddleware<Population>();
//app.UseMiddleware<Capital>();

app.UseRouting();

app.Use(async (context, next) =>
{
    Endpoint end = context.GetEndpoint();
    if(end != null)
    {
        await context.Response
            .WriteAsync($"{end.DisplayName} Selected\n");
    } else
    {
        await context.Response
            .WriteAsync("No endpoint selected\n");
    }
    await next();
});

app.UseEndpoints(endpoints =>
{
    endpoints.Map("{number:int}", async context =>
    {
        await context.Response.WriteAsync("Routed to int endpoint");
    })
        .WithDisplayName("Int endpoint")
        .Add(b => ((RouteEndpointBuilder)b).Order = 1);

    endpoints.Map("{number:double}", async context =>
    {
        await context.Response.WriteAsync("Routed to double endpoint");
    })
        .WithDisplayName("Double endpoint")
        .Add(b => ((RouteEndpointBuilder)b).Order = 2);

    endpoints.MapGet("routing", async context =>
    {
        await context.Response.WriteAsync("Request was Routed");
    });

    // Constraint
    endpoints.MapGet("{first:alpha:length(3)}/{second:bool}", async context =>
    {
        await context.Response.WriteAsync("Request was Routed\n");
        foreach (var kvp in context.Request.RouteValues)
        {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
        }
    });

    // Constraint
    endpoints.MapGet("{first:int}/{second:bool}", async context =>
    {
        await context.Response.WriteAsync("Request was Routed\n");
        foreach (var kvp in context.Request.RouteValues)
        {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
        }
    });

    endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);

    // Catch all: 2 or more segments will match
    endpoints.MapGet("{first}/{second}/{*catachall}", async context =>
    {
        await context.Response.WriteAsync("Request was Routed\n");
        foreach (var kvp in context.Request.RouteValues)
        {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
        }
    });

    endpoints.MapGet("files/{filename}.{ext}", async context =>
    {
        await context.Response.WriteAsync("Request was Routed\n");
        foreach (var kvp in context.Request.RouteValues)
        {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
        }
    });

    //endpoints.MapGet("capital/{country=France}", Capital.Endpoint);
    //endpoints.MapGet("capital/{country:regex(^uk|france|monaco$)}", Capital.Endpoint);
    

    endpoints.MapGet("size/{city?}", Population.Endpoint)
        .WithMetadata(new RouteNameMetadata("population"));

    //endpoints.MapGet("capital/uk", new Capital().Invoke);
    //endpoints.MapGet("population/paris", new Population().Invoke);

    endpoints.MapFallback(async context =>
    {
        await context.Response.WriteAsync("Routed to fallback endpoint");
    });
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Terminal Middleware Reached");
});

app.Run();
