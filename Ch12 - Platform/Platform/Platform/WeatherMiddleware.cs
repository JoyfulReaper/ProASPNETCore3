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

        public async Task Invoke(HttpContext context, IResponseFormatter formatter1,
            IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if (context.Request.Path == "/middleware/class")
            {
                await formatter1.Format(context,
                    "");
                await formatter2.Format(context,
                     "");
                await formatter3.Format(context,
                     "");
            }
            else
            {
                await _nextDelegate(context);
            }
        }
    }
}
