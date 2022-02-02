using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        public Product Product { get; set; }

        private DataContext _context;

        public IndexModel(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(long id = 1)
        {
            Product = await _context.Products.FindAsync(id);
            if(Product == null)
            {
                return RedirectToPage("NotFound");
            }
            return Page();
        }
    }
}
