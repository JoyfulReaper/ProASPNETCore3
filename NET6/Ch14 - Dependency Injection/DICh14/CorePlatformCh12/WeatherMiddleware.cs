using DICh14.Services;

namespace DICh14
{
    public class WeatherMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IResponseFormatter _formatter;

        public WeatherMiddleware(RequestDelegate next)
            //,IResponseFormatter formatter)
        {
            _next = next;
            //_formatter = formatter;
        }

        public async Task Invoke(HttpContext context, IResponseFormatter formatter1,
            IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if (context.Request.Path == "/middleware/class")
            {
                //await context.Response
                //    .WriteAsync("Middleware Class: It is raining in London");
                //await formatter1.Format(context,
                //    "Middleware Class: It is raining in London");
                await formatter1.Format(context, string.Empty);
                await formatter2.Format(context, string.Empty);
                await formatter3.Format(context, string.Empty);
            } else
            {
                await _next(context);
            }
        }
    }
}
