namespace DICh14.Services
{
    public interface IResponseFormatter
    {
        Task Format(HttpContext context, string content);
    }
}
