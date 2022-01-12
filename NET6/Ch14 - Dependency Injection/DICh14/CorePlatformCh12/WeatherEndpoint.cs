using DICh14.Services;

namespace DICh14
{
    public class WeatherEndpoint
    {
        //public static async Task Endpoint(HttpContext context)
        //{
        //    IResponseFormatter formatter = 
        //        context.RequestServices.GetService<IResponseFormatter>();

        //    await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");

        //    //await context.Response
        //    //    .WriteAsync("Endpoint Class: It is cloudy in Milan");
        //}

        public static async Task Endpoint(HttpContext context, IResponseFormatter formatter)
        {
            await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");

            //await context.Response
            //    .WriteAsync("Endpoint Class: It is cloudy in Milan");
        }
    }

    public class WeatherEndpointDI
    {
        //private readonly IResponseFormatter _formatter;

        //public WeatherEndpointDI(IResponseFormatter formatter)
        //{
        //    _formatter = formatter;
        //}

        public async Task Endpoint(HttpContext context,
            IResponseFormatter formatter)
        {
            await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        }
    }
}
