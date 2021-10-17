using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform2
{
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
            if (context.Request.Path == "/location")
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
