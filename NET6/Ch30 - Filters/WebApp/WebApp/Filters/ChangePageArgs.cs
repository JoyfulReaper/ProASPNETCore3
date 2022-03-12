using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class ChangePageArgs : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            // DO NOTHING
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if(context.HandlerArguments.ContainsKey("message1"))
            {
                context.HandlerArguments["message1"] = "New Message";
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // Do Nothing
        }
    }
}
