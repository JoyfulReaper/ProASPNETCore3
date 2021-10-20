using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Platform4.Services
{
    public interface IResponseFormatter
    {

        Task Format(HttpContext context, string content);

        public bool RichOutput => false;
    }
}