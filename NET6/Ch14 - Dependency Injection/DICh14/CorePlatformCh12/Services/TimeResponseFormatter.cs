namespace DICh14.Services
{
    public class TimeResponseFormatter : IResponseFormatter
    {
        private readonly ITimeStapmer _timeStapmer;

        public TimeResponseFormatter(ITimeStapmer timeStapmer)
        {
            _timeStapmer = timeStapmer;
        }

        public async Task Format(HttpContext context, string content)
        {
            await context.Response.WriteAsync($"{_timeStapmer.TimeStamp}: {content}");
        }
    }
}
