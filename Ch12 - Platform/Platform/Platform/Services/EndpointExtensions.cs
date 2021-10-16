using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndpointExtensions
    {
        //public static void MapWeather (this IEndpointRouteBuilder app, string path)
        //{
        //    IResponseFormatter formatter =
        //        app.ServiceProvider.GetService<IResponseFormatter>();

        //    app.MapGet(path, context => Platform.WeatherEndpoint
        //        .Endpoint(context, formatter));
        //}

        public static void MapEndpoint<T>(this IEndpointRouteBuilder app, 
            string path,
            string methodName)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if(methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new System.Exception("Method cannot by used!");
            }
            T endpointInstance =
                ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);

            //app.MapGet(path, (RequestDelegate)methodInfo
            //    .CreateDelegate(typeof(RequestDelegate), endpointInstance));

            ParameterInfo[] methodParams = methodInfo.GetParameters();
            app.MapGet(path, context => (Task)methodInfo.Invoke(endpointInstance,
                methodParams.Select(p => p.ParameterType == typeof(HttpContext) ?
                context
                : app.ServiceProvider.GetService(p.ParameterType)).ToArray()));
        }
    }
}
