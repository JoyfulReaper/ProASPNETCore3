using Microsoft.AspNetCore.Http;
using Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    //public class WeatherMiddleware
    //{
    //    private readonly RequestDelegate _nextDelegate;
    //    private readonly IResponseFormatter _formatter;

    //    public WeatherMiddleware(RequestDelegate nextDelegate,
    //        IResponseFormatter formatter)
    //    {
    //        _nextDelegate = nextDelegate;
    //        _formatter = formatter;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        if(context.Request.Path == "/middleware/class")
    //        {
    //            //await context.Response
    //            //    .WriteAsync("Middleware Class: It is raining in London");
    //            await _formatter.Format(context,
    //                "Middleware Class: It is raining in London");
    //        } else
    //        {
    //            await _nextDelegate(context);
    //        }
    //    }
    //}

    public class WeatherMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public WeatherMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task Invoke(HttpContext context, IResponseFormatter formatter)
        {
            if (context.Request.Path == "/middleware/class")
            {
                await formatter.Format(context,
                    "Middleware Class: It is raining in London");
            }
            else
            {
                await _nextDelegate(context);
            }
        }
    }
}
