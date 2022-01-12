using DICh14;
using DICh14.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
//builder.Services.AddSingleton<IResponseFormatter, GuidService>();
builder.Services.AddTransient<IResponseFormatter, GuidService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseMiddleware<WeatherMiddleware>();



IResponseFormatter formatter = app.Services.GetRequiredService<IResponseFormatter>();


//IResponseFormatter formatter = new TextResponseFormatter();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/middleware/function")
    {
        //await formatter.Format(context,
        //    "Middleware Function: It is snowing in Chicago");
        //await TextResponseFormatter.Singleton.Format(context,
        //    "Middleware Function: It is snowing in Chicago");
        //await TypeBroker.Formatter
        //    .Format(context, "Middleware Function: It is snowing in Chicago");
        //await formatter.Format(context,
        //    "Middleware Function: It is snowing in Chicago");
        IResponseFormatter formatter1 
            = app.Services.GetRequiredService<IResponseFormatter>();
        await formatter1.Format(context,
            "Middleware Function: It is snowing in Chicago");
    }
    else
    {
        await next();
    }
});

app.UseEndpoints(endpoints =>
{
    //endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);
    //endpoints.MapWeather("/endpoint/class");
    endpoints.MapEndpoint<WeatherEndpointDI>("endpoint/class");

    endpoints.MapGet("/endpoint/function", async context =>
    {
        //await TextResponseFormatter.Singleton.Format(context,
        //    "Endpoint Function: It is sunny in LA");
        //await TypeBroker.Formatter
        //    .Format(context, "Endpoint Function: It is sunny in LA");
        IResponseFormatter formatter1
            = app.Services.GetRequiredService<IResponseFormatter>();
        await formatter1.Format(context,
            "Enpoint Function: It is sunny in LA");
    });
});

app.Run();
