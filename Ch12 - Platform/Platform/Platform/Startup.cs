using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Options pattern
            services.Configure<MessageOptions>(options =>
            {
                options.CityName = "Albany";
            });

            // Route Constraints
            services.Configure<RouteOptions>(opts =>
            {
                opts.ConstraintMap.Add("countryName",
                    typeof(CountryRouteConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<MessageOptions> msgOptions) // Options pattern
        {
            /////////////////////////////////////////////////
            /// Routing
            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();

            app.UseRouting();

            app.UseMiddleware<WeatherMiddleware>();
            IResponseFormatter formatter = new TextResponseFormatter();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/middleware/function")
                {
                    //await formatter.Format(context,
                    //    "Middleware function: It is snowing in Chicago");

                    //Singleton
                    //await TextResponseFormatter.Singleton.Format(context,
                    //    "Middleware Function: It is snowing in Chicago");

                    // Type Broker pattern
                    await TypeBroker.Formatter.Format(context,
                        "Middleware Function: It is snowing in Chicago");
                }
                else
                {
                    await next();
                }
            });

            //////////////////
            //app.Use(async (context, next) =>
            //{
            //    Endpoint end = context.GetEndpoint();
            //    if(end != null)
            //    {
            //        await context.Response
            //            .WriteAsync($"{end.DisplayName} selected \n");
            //    }
            //    else
            //    {
            //        await context.Response
            //            .WriteAsync($"No Endpoint selected \n");
            //    }
            //    await next();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);
                endpoints.MapGet("/endpoint/function", async context =>
                {
                    //await context.Response.WriteAsync("Endpoint Function: It is sunny in LA");

                    // Singleton
                    //await TextResponseFormatter.Singleton.Format(context,
                    //    "Endpoint Function: It is sunny in LA");

                    //Type Broker Pattern
                    await TypeBroker.Formatter.Format(context,
                        "Endpoint Function: It is sunny in LA");
                });


                // Segment constriants
                endpoints.MapGet("{first:int}/{second:bool}", async context =>
                {
                    await context.Response.WriteAsync("Constraint Request Was Routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });

                endpoints.MapGet("files/{filename}.{ext}", async context =>
                {
                    await context.Response.WriteAsync("Request was Routed\n");
                    foreach(var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });

                endpoints.MapGet("{first}/{second}/{thrid}", async context =>
                {
                    await context.Response.WriteAsync("Request Was Routed\n");
                    foreach(var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });

                // Catchall
                endpoints.MapGet("{first}/{second}/{thrid}/{*catchall}", async context =>
                {
                    await context.Response.WriteAsync("Request Was Routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });

                endpoints.MapGet("routing", async context =>
                {
                    await context.Response.WriteAsync("Request was Routed");
                });

                //endpoints.MapGet("capital/uk", new Capital().Invoke);
                //endpoints.MapGet("population/paris", new Population().Invoke);
                //endpoints.MapGet("capital/{country}", Capital.Endpoint);

                //endpoints.MapGet("capital/{country=france}", Capital.Endpoint);

                //endpoints.MapGet("capital/{country:regex(^uk|france|monaco$)=france}", Capital.Endpoint);
                endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);

                //Optional segment
                endpoints.MapGet("size/{city?}", Population.Endpoint)
                    .WithMetadata(new RouteNameMetadata("population"));

                //Fallback endpoint
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync("Routed to fallback endpoint");
                });
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Terminal Middleware reached");
            });

            ////////////////////////////////////////////////////
            app.UseMiddleware<LocationMiddleware>();

            // Options pattern
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/location")
                {
                    MessageOptions opts = msgOptions.Value;
                    await context.Response
                        .WriteAsync($"{opts.CityName}, {opts.CountryName}");
                }
                else
                {
                    await next();
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Terminal Middleware - Never forwards to the next middleware component in the pipline
            app.Map("/stopbranch", branch =>
            {
                branch.Run(async (context) =>
                {
                    await context.Response.WriteAsync($"Stop Branch Middleware");
                });
            });


            // Terminal Middleware - Never forwards to the next middleware component in the pipline
            app.Map("/classbranch", branch =>
            {
                branch.Run(new TerminalQueryStringMiddleware().Invoke);
            });

            app.Map("/branch", branch =>
            {
                branch.UseMiddleware<QueryStringMiddleware>();

                branch.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync($"Branch Middleware");
                });
            });

            app.Use(async (context, next) =>
            {
                await next();
                await context.Response
                    .WriteAsync($"\nStatus Code: { context.Response.StatusCode}");
            });

            app.Use(async (context, next) =>
            {
                if(context.Request.Path == "/short")
                {
                    await context.Response
                        .WriteAsync($"Request Short Circuted");
                } else
                {
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Method == HttpMethods.Get
                    && context.Request.Query["custom"] == "true")
                {
                    await context.Response.WriteAsync("Custom Middleware \n");
                }
                await next();
            });

            app.UseMiddleware<QueryStringMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
