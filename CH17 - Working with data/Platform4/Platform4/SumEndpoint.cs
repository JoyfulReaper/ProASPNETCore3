using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Platform4.Models;
using Platform4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform4
{
    public class SumEndpoint
    {
        public async Task Endpoint(HttpContext context, CalculationContext dataContext)
        {
            int count = int.Parse((string)context.Request.RouteValues["count"]);
            long total = dataContext.Calculations
                .FirstOrDefault(c => c.Count == count)?.Result ?? 0;

            if (total == 0)
            {
                for (int i = 1; i <= count; i++)
                {
                    total += i;
                }
                dataContext.Calculations!
                    .Add(new Calculation() { Count = count, Result = total });
                await dataContext.SaveChangesAsync();
            }
            string totalString = $"({ DateTime.Now.ToLongTimeString() }) {total} ";

            await context.Response.WriteAsync(
                $"({DateTime.Now.ToLongTimeString()}) Total for {count}" +
                $" values:\n{totalString}\n");
        }

        //public async Task Endpoint(HttpContext context, IDistributedCache cache)
        //{
        //    int count = int.Parse((string)context.Request.RouteValues["count"]);
        //    string cacheKey = $"sum_{count}";
        //    string totalString = await cache.GetStringAsync(cacheKey);
        //    if(totalString == null)
        //    {
        //        long total = 0;
        //        for (int i = 1; i <= count; i++)
        //        {
        //            total += i;
        //        }
        //        totalString = $"({ DateTime.Now.ToLongTimeString() }) {total} ";
        //        await cache.SetStringAsync(cacheKey, totalString,
        //            new DistributedCacheEntryOptions
        //            {
        //                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        //            });
        //    }
        //    await context.Response.WriteAsync(
        //        $"({DateTime.Now.ToLongTimeString()}) Total for {count}" +
        //        $" values:\n{totalString}\n");
        //}

        public async Task EndpointResponseCache(HttpContext context, IDistributedCache cache,
            IResponseFormatter formatter, LinkGenerator generator)
        {
            int count = int.Parse((string)context.Request.RouteValues["count"]);
            long total = 0;
            for (int i = 1; i <= count; i++)
            {
                total += i;
            }

            string totalString = $"({ DateTime.Now.ToLongTimeString() }) {total} ";

            context.Response.Headers["Cache-Control"] = "public, max-age=120";

            string url = generator.GetPathByRouteValues(context, null,
                new { count = count });

            await formatter.Format(context,
                $"<div>({DateTime.Now.ToLongTimeString()}) Total for {count}" +
                $" values:</div><div>{totalString}</div>" +
                $"<a href={url}>Reload</a>");
        }

        // No Caching
        //public async Task Endpoint(HttpContext context)
        //{
        //    int count = int.Parse((string)context.Request.RouteValues["count"]);
        //    long total = 0;
        //    for(int i = 1; i <= count; i++)
        //    {
        //        total += i;
        //    }

        //    string totalString = $"({ DateTime.Now.ToLongTimeString() }) {total} ";
        //    await context.Response.WriteAsync(
        //        $"({DateTime.Now.ToLongTimeString()}) Total for {count}" +
        //        $" values:\n{totalString}\n");
        //}
    }
}
