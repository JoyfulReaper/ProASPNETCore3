using DICh14;
using DICh14.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseMiddleware<WeatherMiddleware>();

//IResponseFormatter formatter = new TextResponseFormatter();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/middleware/function")
    {
        //await formatter.Format(context,
        //    "Middleware Function: It is snowing in Chicago");
        //await TextResponseFormatter.Singleton.Format(context,
        //    "Middleware Function: It is snowing in Chicago");
        await TypeBroker.Formatter
            .Format(context, "Middleware Function: It is snowing in Chicago");
    } else
    {
        await next();
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);

    endpoints.MapGet("/endpoint/function", async context =>
    {
        //await TextResponseFormatter.Singleton.Format(context,
        //    "Endpoint Function: It is sunny in LA");
        await TypeBroker.Formatter
            .Format(context, "Endpoint Function: It is sunny in LA");
    });
});

app.Run();
