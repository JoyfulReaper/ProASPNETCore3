using DICh14;
using DICh14.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
//builder.Services.AddSingleton<IResponseFormatter, GuidService>();
//builder.Services.AddTransient<IResponseFormatter, GuidService>();
//builder.Services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
builder.Services.AddScoped<ITimeStapmer, DefaultTimeStamper>();

builder.Services.AddScoped<IResponseFormatter, TextResponseFormatter>();
builder.Services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
builder.Services.AddScoped<IResponseFormatter, GuidService>();
builder.Services.AddSingleton(typeof(ICollection<>), typeof(List<>));


//builder.Services.AddScoped<IResponseFormatter>(serviceProvider =>
//{
//    string typeName = builder.Configuration["services:IResponseFormatter"];
//    return (IResponseFormatter)ActivatorUtilities
//        .CreateInstance(serviceProvider, typeName == null 
//            ? typeof(GuidService) : Type.GetType(typeName, true));
//});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseMiddleware<WeatherMiddleware>();

IConfiguration config = app.Configuration;


//IResponseFormatter formatter = app.Services.GetRequiredService<IResponseFormatter>();


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
        //IResponseFormatter formatter1 
        //    = app.Services.GetRequiredService<IResponseFormatter>();
        IResponseFormatter formatter1 = context.RequestServices.GetService<IResponseFormatter>();
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
    endpoints.MapGet("/string", async context =>
    {
        ICollection<string> collection =
            context.RequestServices.GetService<ICollection<string>>();
        collection.Add($"Request: {DateTime.Now.ToLongTimeString()}");

        foreach (var str in collection)
        {
            await context.Response.WriteAsync($"String: {str}\n");
        }
    });


    endpoints.MapGet("/int", async context =>
    {
        ICollection<int> collection =
            context.RequestServices.GetService<ICollection<int>>();
        collection.Add(collection.Count() + 1);

        foreach (var val in collection)
        {
            await context.Response.WriteAsync($"int: {val}\n");
        }
    });
    endpoints.MapGet("/single", async context =>
    {
        ICollection<string> collection =
            context.RequestServices.GetService<ICollection<string>>();
        collection.Add($"Request: {DateTime.Now.ToLongTimeString()}");

        IResponseFormatter formatter = context.RequestServices
            .GetService<IResponseFormatter>();
        await formatter.Format(context, "Single Service");
    });

        endpoints.MapGet("/", async context =>
    {
        IResponseFormatter formatter = context.RequestServices
              .GetServices<IResponseFormatter>().First(f => f.RichOutput);
        await formatter.Format(context, "Multiple Services");
    });

    //endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);
    //endpoints.MapWeather("/endpoint/class");
    endpoints.MapEndpoint<WeatherEndpointDI>("endpoint/class");

    endpoints.MapGet("/endpoint/function", async context =>
    {
        //await TextResponseFormatter.Singleton.Format(context,
        //    "Endpoint Function: It is sunny in LA");
        //await TypeBroker.Formatter
        //    .Format(context, "Endpoint Function: It is sunny in LA");
        //IResponseFormatter formatter1
        //    = app.Services.GetRequiredService<IResponseFormatter>();
        IResponseFormatter formatter1 = context.RequestServices.GetService<IResponseFormatter>();
        await formatter1.Format(context,
            "Enpoint Function: It is sunny in LA");
    });
});

app.Run();
