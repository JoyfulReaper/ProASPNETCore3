using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class Capital
    {
        private readonly RequestDelegate _nextDelegate;

        public Capital()
        {

        }

        public Capital(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public static async Task Endpoint(HttpContext context)
        {
            string capital = null;
            string country = context.Request.RouteValues["country"] as string;
            switch ((country ?? "").ToLower())
            {
                case "uk":
                    capital = "London";
                    break;
                case "france":
                    capital = "Paris";
                    break;
                case "monaco":
                    LinkGenerator generator =
                        context.RequestServices.GetService<LinkGenerator>();
                    string url = generator.GetPathByRouteValues(context,
                        "population", new { city = country });
                    context.Response.Redirect(url);
                    return;
            }
            if (capital != null)
            {
                await context.Response
                    .WriteAsync($"{capital} is the capital of {country}");
                return;
            } else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }

        public async Task Invoke(HttpContext context)
        {
            string[] parts = context.Request.Path.ToString()
                .Split("/", StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length == 2 && parts[0] == "capital")
            {
                string capital = null;
                string country = parts[1];
                switch(country.ToLower())
                {
                    case "uk":
                        capital = "London";
                        break;
                    case "france":
                        capital = "Paris";
                        break;
                    case "monaco":
                        context.Response.Redirect($"/population/{country}");
                        return;
                }
                if (capital != null)
                {
                    await context.Response
                        .WriteAsync($"{capital} is the capital of {country}");
                    return;
                }
            }
            if(_nextDelegate != null)
            {
                await _nextDelegate(context);
            }
        }
    }
}
