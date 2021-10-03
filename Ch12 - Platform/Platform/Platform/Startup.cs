using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<MessageOptions> msgOptions) // Options pattern
        {

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
