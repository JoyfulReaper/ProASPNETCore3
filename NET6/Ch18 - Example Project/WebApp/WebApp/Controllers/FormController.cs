using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FormController : Controller
    {
        private readonly DataContext _dbContext;

        public FormController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(long id = 1)
        {
            ViewBag.Categories
                = new SelectList(_dbContext.Categories, "CategoryId", "Name");

            return View("Form", await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstAsync(p => p.ProductId == id));
        }

        public async Task<IActionResult> BindingForm(long? id)
        {
            ViewBag.Categories
                = new SelectList(_dbContext.Categories, "CategoryId", "Name");

            return View("BindingForm", await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => id == null || p.ProductId == id));
        }

        //[HttpPost]
        //public IActionResult SubmitBindingForm(string name, decimal price)
        //{
        //    TempData["name param"] = name;
        //    TempData["price param"] = price.ToString();

        //    return RedirectToAction(nameof(Results));
        //}

        [HttpPost]
        public IActionResult SubmitBindingForm(Product product)
        {
            TempData["product"] = System.Text.Json.JsonSerializer.Serialize(product);
            return RedirectToAction(nameof(Results));
        }

        [HttpPost]
        public IActionResult SubmitForm()
        {
            foreach(string key in Request.Form.Keys)
                //.Where(k => !k.StartsWith("_")))
            {
                TempData[key] = string.Join(", ", Request.Form[key]);
            }
            return RedirectToAction(nameof(Results));
        }

        public IActionResult Results()
        {
            return View(TempData);
        }

        public async Task<IActionResult> MVForm(long? id)
        {
            return View("MVForm", await _dbContext.Products
                .FirstOrDefaultAsync(p =>id == null || p.ProductId == id));
        }

        [HttpPost]
        public IActionResult SubmitMVForm(Product product)
        {
            if(string.IsNullOrEmpty(product.Name))
            {
                ModelState.AddModelError(nameof(Product.Name), "Enter a name");
            }

            if(ModelState.GetValidationState(nameof(Product.Price))
                == ModelValidationState.Valid && product.Price < 1)
            {
                ModelState.AddModelError(nameof(Product.Price), "Enter a positive price");
            }

            if(ModelState.GetValidationState(nameof(Product.Name))
                == ModelValidationState.Valid
                && ModelState.GetValidationState(nameof(Product.Price))
                == ModelValidationState.Valid
                && product.Name.ToLower().StartsWith("small") && product.Price > 100)
            {
                ModelState.AddModelError("", "Small products cannot cost more than $100");
            }

            if(!_dbContext.Categories.Any(c => c.CategoryId == product.CategoryId))
            {
                ModelState.AddModelError(nameof(Product.CategoryId), "Enter an existing category id");
            }

            if (!_dbContext.Suppliers.Any(s => s.SupplierId == product.SupplierId))
            {
                ModelState.AddModelError(nameof(Product.SupplierId), "Enter an existing supplier id");
            }

            if (ModelState.IsValid)
            {
                TempData["name"] = product.Name;
                TempData["price"] = product.Price.ToString();
                TempData["categoryId"] = product.CategoryId.ToString();
                TempData["supplierId"] = product.SupplierId.ToString();
                return RedirectToAction(nameof(Results));
            }
            else
            {
                return View("MVForm");
            }
        }
    }
}
