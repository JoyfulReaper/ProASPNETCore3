using Microsoft.Extensions.Options;

namespace CorePlatformCh12
{
    public class QueryStringMiddleWare
    {
        private RequestDelegate next;

        public QueryStringMiddleWare()
        {}

        public QueryStringMiddleWare(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Method == HttpMethods.Get
                && context.Request.Query["custom"] == "true")
            {
                await context.Response.WriteAsync("Class-based MiddleWare \n");
            }
            if(next != null)
            {
                await (next(context));
            }  
        }
    }

    public class LocationMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly MessageOptions _options;

        public LocationMiddleWare(RequestDelegate next,
            IOptions<MessageOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == "/location")
            {
                await context.Response
                    .WriteAsync($"Class: {_options.CityName}, {_options.CountryName}");
            } else
            {
                await _next(context);
            }
        }
    }
}
