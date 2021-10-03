using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class QueryStringMiddleware
    {
        private RequestDelegate next;

        public QueryStringMiddleware(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Method == HttpMethods.Get
                && context.Request.Query["custom"] == "true")
            {
                await context.Response.WriteAsync("Class-based Middleware \n");
            }
            await next(context);
        }
    }

    public class TerminalQueryStringMiddleware
    {
        private RequestDelegate next;

        public TerminalQueryStringMiddleware(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public TerminalQueryStringMiddleware()
        {
            // Terminal Middleware - Do nothing
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
                && context.Request.Query["custom"] == "true")
            {
                await context.Response.WriteAsync("Terminal Class-based Middleware \n");
            }

            if (next != null)
            {
                await next(context);
            }
        }
    }

    public class LocationMiddleware
    {
        private readonly RequestDelegate _nextDelegate;
        private readonly MessageOptions _options;

        public LocationMiddleware(RequestDelegate nextDelegate,
            IOptions<MessageOptions> options)
        {
            _nextDelegate = nextDelegate;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/classlocation")
            {
                await context.Response.WriteAsync($"{_options.CityName}, {_options.CountryName}");
            }
            else
            {
                await _nextDelegate(context);
            }
        }
    }
}
