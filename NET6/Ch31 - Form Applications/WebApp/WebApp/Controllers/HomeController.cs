using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApp.Filters;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        private IEnumerable<Category> Categories => _context.Categories;
        private IEnumerable<Supplier> Suppliers => _context.Suppliers;
 
        public HomeController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier));
        }

        public async Task<IActionResult> Details(long id)
        {
            Product p = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            ProductViewModel model = ViewModelFactory.Details(p);
            return View("ProductEditor", model);
        }

        public IActionResult Create()
        {
            return View("ProductEditor", ViewModelFactory.Create(new Product(), Categories, Suppliers));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product)
        {
            if(ModelState.IsValid)
            {
                product.ProductId = default;
                product.Category = default;
                product.Supplier = default;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("ProductEditor", ViewModelFactory.Create(product, Categories, Suppliers));
        }

        public async Task<IActionResult> Edit(long id)
        {
            Product p = await _context.Products.FindAsync(id);
            ProductViewModel model = ViewModelFactory.Edit(p, Categories, Suppliers);
            return View("ProductEditor", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm]Product product)
        {
            if(ModelState.IsValid)
            {
                product.Category = default;
                product.Supplier = default;
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View("ProductEditor", ViewModelFactory.Edit(product, Categories, Suppliers));
        }

        public async Task<IActionResult> Delete (long id)
        {
            ProductViewModel model = ViewModelFactory.Delete(await _context.Products.FindAsync(id), Categories, Suppliers);
            return View("ProductEditor", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
