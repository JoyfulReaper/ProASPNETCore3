using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Filters;

namespace WebApp.Controllers
{
    [HttpsOnly]
    public class HomeController : Controller
    {
        //[RequireHttps]
        public IActionResult Index()
        {
            return View("Message", "This is the Index action on the Home Controller");
        }

        public IActionResult Secure()
        {
            if (Request.IsHttps)
            {
                return View("Message", "This is the Secure action on the Home Controller");
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        //[ChangeArg]
        public IActionResult Message(string message1, string message2 = "None")
        {
            return View("Message", $"{message1}, {message2}");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("message1"))
            {
                context.ActionArguments["message1"] = "New Message";
            }
        }
    }
}
