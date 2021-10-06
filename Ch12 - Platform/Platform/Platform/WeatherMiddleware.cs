using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class WeatherMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public WeatherMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == "/middleware/class")
            {
                await context.Response
                    .WriteAsync("Middleware Class: It is raining in London");
            } else
            {
                await _nextDelegate(context);
            }
        }
    }
}
