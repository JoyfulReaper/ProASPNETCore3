using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class WeatherEndpoint
    {
        //private readonly IResponseFormatter _formatter;

        //public static async Task Endpoint(HttpContext context)
        //{
        //    //await context.Response
        //    //    .WriteAsync("Endpoint Class: It is cloudy in Milan");

        //    // Access services through HttpContext in static class.
        //    IResponseFormatter formatter =
        //        context.RequestServices.GetRequiredService<IResponseFormatter>();
        //    await formatter.Format(context, "Endpoint class: It is cloudy in Milan");
        //}


        // Adapter Function receives service when the route is created
        //public WeatherEndpoint(IResponseFormatter formatter)
        //{
        //    _formatter = formatter;
        //}

        //public async Task Endpoint(HttpContext context)
        //{
        //    await _formatter.Format(context, "Endpoint Class: It's cloudy in Milan");
        //}


        public async Task Endpoint(HttpContext context,
            IResponseFormatter formatter)
        {
            await formatter.Format(context, "Endpoint Class: It's cloudy in Milan");
        }
    }
}
