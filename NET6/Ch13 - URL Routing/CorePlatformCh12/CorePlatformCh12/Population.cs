namespace UrlRoutingCh13
{
    public static class Population
    {
        public static async Task Endpoint(HttpContext context)
        {
            string city = context.Request.RouteValues["city"] as string;
            int? pop = null;
            switch ((city ?? "").ToLower())
            {
                case "london":
                    pop = 8_136_000;
                    break;
                case "paris":
                    pop = 2_141_000;
                    break;
                case "monaco":
                    pop = 39_000;
                    break;
            }
            if (pop.HasValue)
            {
                await context.Response
                    .WriteAsync($"City: {city}, Population: {pop}");
                return;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }

    //public class Population
    //{
    //    private readonly RequestDelegate _requestDelegate;

    //    public Population()
    //    {}

    //    public Population(RequestDelegate requestDelegate)
    //    {
    //        _requestDelegate = requestDelegate;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        string[] parts = context.Request.Path.ToString()
    //            .Split("/", StringSplitOptions.RemoveEmptyEntries);
    //        if (parts.Length == 2 && parts [0] == "population")
    //        {
    //            string city = parts [1];
    //            int? pop = null;
    //            switch (city.ToLower())
    //            {
    //                case "london":
    //                    pop = 8_136_000;
    //                    break;
    //                case "paris":
    //                    pop = 2_141_000;
    //                    break;
    //                case "monaco":
    //                    pop = 39_000;
    //                    break;
    //            }
    //            if (pop.HasValue)
    //            {
    //                await context.Response
    //                    .WriteAsync($"City: {city}, Population: {pop}");
    //                return;
    //            }
    //        }
    //        if(_requestDelegate != null)
    //        {
    //            await _requestDelegate(context);
    //        }
    //    }
    //}
}
