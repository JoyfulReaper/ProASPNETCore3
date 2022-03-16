using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApp.Filters
{
    public class ResultDiagnosticsAttribute : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.HttpContext.Request.Query.ContainsKey("diag"))
            {
                Dictionary<string, string> diagData = new()
                {
                    { "Result Type", context.Result.GetType().Name }
                };

                if (context.Result is ViewResult vr)
                {
                    diagData["View Name"] = vr.ViewName;
                    diagData["Model Type"] = vr.Model.GetType().Name;
                    diagData["Model Data"] = vr.Model.ToString();
                } else if (context.Result is PageResult pr)
                {
                    diagData["Model Type"] = pr.Model.GetType().Name;
                    diagData["Model Date"] = pr.Model.ToString();
                }

                context.Result = new ViewResult()
                {
                    ViewName = "/Views/Shared/Message.cshtml",
                    ViewData = new ViewDataDictionary(
                        new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    {
                        Model = diagData
                    }
                };
            }
            await next();
        }
    }
}
