using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages
{
    public class EditorModel : PageModel
    {
        public Product Product { get; set; }
        private readonly DataContext _context;

        public EditorModel(DataContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(long id)
        {
            Product = await _context.Products.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(long id, decimal price)
        {
            Product p = await _context.Products.FindAsync(id);
            p.Price = price;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
