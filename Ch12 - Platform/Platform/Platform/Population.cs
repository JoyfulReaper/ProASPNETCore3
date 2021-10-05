using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class Population
    {
        private readonly RequestDelegate _nextDelegate;

        public Population()
        {

        }

        public Population(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public static async Task Endpoint(HttpContext context)
        {
            string city = context.Request.RouteValues["city"] as string ?? "london";
            int? pop = null;
            switch (city.ToLower())
            {
                case "london":
                    pop = 8_136_000;
                    break;
                case "paris":
                    pop = 2_141_000;
                    break;
                case "monaco":
                    pop = 39_000;
                    break;
            }

            if (pop.HasValue)
            {
                await context.Response
                    .WriteAsync($"City: {city}, Population: {pop}");
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
            if(parts.Length == 2 && parts[0] == "population")
            {
                string city = parts[1];
                int? pop = null;
                switch(city)
                {
                    case "london":
                        pop = 8_136_000;
                        break;
                    case "paris":
                        pop = 2_141_000;
                        break;
                    case "monaco":
                        pop = 39_000;
                        break;
                }

                if(pop.HasValue)
                {
                    await context.Response
                        .WriteAsync($"City: {city}, Population: {pop}");
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
