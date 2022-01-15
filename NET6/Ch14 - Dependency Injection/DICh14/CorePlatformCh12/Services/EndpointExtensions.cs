using System.Reflection;

namespace DICh14.Services
{
    public static class EndpointExtensions
    {
        public static void MapWeather(this IEndpointRouteBuilder app, string path)
        {
            IResponseFormatter formatter = 
                app.ServiceProvider.GetService<IResponseFormatter>();
            app.MapGet(path, context => DICh14.WeatherEndpoint.Endpoint(context, formatter));
        }

        public static void MapEndpoint<T>(this IEndpointRouteBuilder app,
            string path, string methodName = "Endpoint")
        {

            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if(methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new Exception("Method cannot be used");
            }


            ParameterInfo[] methodParams = methodInfo.GetParameters();
            app.MapGet(path, context =>
            {
                T endpointInstance = 
                    ActivatorUtilities.CreateInstance<T>(context.RequestServices);

                return (Task)methodInfo.Invoke(endpointInstance, methodParams.Select(p =>
                    p.ParameterType == typeof(HttpContext) ? context :
                    context.RequestServices.GetService(p.ParameterType)).ToArray());
            });



            //T endpointInstance =
            //    ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);

            //ParameterInfo[] methodParams = methodInfo.GetParameters();

            //app.MapGet(path, context => (Task)methodInfo.Invoke(endpointInstance,
            //    methodParams.Select(p => p.ParameterType == typeof(HttpContext)
            //    ? context
            //    : context.RequestServices.GetService(p.ParameterType)).ToArray()));
            //: app.ServiceProvider.GetService(p.ParameterType)).ToArray()));

            //app.MapGet(path, (RequestDelegate)methodInfo
            //    .CreateDelegate(typeof(RequestDelegate), endpointInstance));
        }

    }
}
