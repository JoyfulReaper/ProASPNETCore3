using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform2
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configService)
        {
            Configuration = configService;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration data can be accessed here
            services.Configure<MessageOptions>(Configuration.GetSection("Location"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new
                    PhysicalFileProvider($"{env.ContentRootPath}/staticfiles"),
                RequestPath = "/files"
            });

            app.UseRouting();

            app.UseMiddleware<LocationMiddleware>();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/config")
                {
                    string defaultDebug = Configuration["Logging:LogLevel:Default"];
                    await context.Response
                        .WriteAsync($"The config settings is {defaultDebug}");
                    string environ = Configuration["ASPNETCORE_ENVIRONMENT"];
                    await context.Response
                        .WriteAsync($"\nThe env setting is: {environ}");
                    string wsId = Configuration["WebService:Id"];
                    string wsKey = Configuration["WebService:Key"];
                    await context.Response.WriteAsync($"\nThe secret Id is: {wsId}");
                    await context.Response.WriteAsync($"\nThe secret Key is: {wsKey}");
                }
                else
                {
                    await next();
                }
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    logger.LogDebug("Response for / started");
                    await context.Response.WriteAsync("Hello World!");
                    logger.LogDebug("Response for / completed");
                });
            });
        }
    }
}
