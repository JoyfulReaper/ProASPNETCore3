using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class SimpleCacheAttribute : Attribute, IResourceFilter
    {
        private Dictionary<PathString, IActionResult> CachedResponses = new();

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            CachedResponses.Add(context.HttpContext.Request.Path, context.Result);
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            PathString path = context.HttpContext.Request.Path;
            if (CachedResponses.ContainsKey(path))
            {
                context.Result = CachedResponses[path];
                CachedResponses.Remove(path);
            }
        }
    }
}
