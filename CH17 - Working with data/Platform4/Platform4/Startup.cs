using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Platform4.Models;
using Platform4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform4
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedSqlServerCache(opts =>
            {
                opts.ConnectionString = _configuration["ConnectionStrings:CacheConnection"];
                opts.SchemaName = "dbo";
                opts.TableName = "Create";
            });

            services.AddResponseCaching();
            services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
            services.AddTransient<SeedData>();

            services.AddDbContext<CalculationContext>(opts =>
            {
                opts.UseSqlServer(_configuration["ConnectionStrings:CalcConnection"]);
                opts.EnableSensitiveDataLogging(true);
            });

            //services.AddDistributedMemoryCache(opts =>
            //{
            //    opts.SizeLimit = 200;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime,
            IWebHostEnvironment env, SeedData seeData)
        {

            app.UseDeveloperExceptionPage();
            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEndpoint<SumEndpoint>("/sum/{count:int=1000000000}");
                //endpoints.MapEndpoint<SumEndpoint>("/sumc/{count:int=1000000000}", "EndpointResponseCache");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            bool cmdLineInit = (_configuration["INITDB" ?? "false"]) == "true";
            if(env.IsDevelopment() || cmdLineInit)
            {
                seeData.SeedDatabase();
                if (cmdLineInit)
                {
                    lifetime.StopApplication();
                }
            }
        }
    }
}
