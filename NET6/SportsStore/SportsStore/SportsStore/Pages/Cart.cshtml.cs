using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        private readonly IStoreRepository _repo;

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            _repo = repo;
            Cart = cartService;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = _repo.Products
                .FirstOrDefault(p => p.ProductId == productId);
            Cart.AddItem(product, 1);
            return RedirectToPage(new {returnUrl = returnUrl});
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => 
                cl.Product.ProductId == productId).Product);

            return RedirectToPage(new {returnUrl});
        }
    }
}
