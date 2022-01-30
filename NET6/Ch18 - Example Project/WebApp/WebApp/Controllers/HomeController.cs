using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(long id = 1)
        {
            ViewBag.AveragePrice = await _context.Products.AverageAsync(p => p.Price);
            return View(await _context.Products.FindAsync(id));
        }

        public IActionResult Html()
		{
            return View((object)"This is a <h3><i>string</i></h3>");
		}

        // Chapter 21
        //public async Task<IActionResult> Index(long id = 1)
        //{
        //    Product prod = await _context.Products.FindAsync(id);
        //    if (prod.CategoryId == 1)
        //    {
        //        return View("Watersports", prod);
        //    }
        //    else
        //    {
        //        return View(prod);
        //    }
        //}

        //public IActionResult Common()
        //{
        //    return View();
        //}

        public IActionResult List()
        {
            return View(_context.Products);
        }
    }
}
