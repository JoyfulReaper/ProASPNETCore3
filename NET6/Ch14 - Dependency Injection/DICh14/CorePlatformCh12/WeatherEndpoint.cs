namespace DICh14
{
    public class WeatherEndpoint
    {
        public static async Task Endpoint(HttpContext context)
        {
            await context.Response
                .WriteAsync("Endpoint Class: It is cloudy in Milan");
        }
    }
}
