using Microsoft.AspNetCore.Mvc;
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
            Product prod = await _context.Products.FindAsync(id);
            if (prod.CategoryId == 1)
            {
                return View("Watersports", prod);
            }
            else
            {
                return View(prod);
            }
        }

        public IActionResult Common()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(_context.Products);
        }
    }
}
