using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreRepository _storeRepository;

        public CartModel(IStoreRepository storeRepository, Cart cartService)
        {
            _storeRepository = storeRepository;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnlUrl { get; set; }

        public void OnGet(string returnUrl)
        {
            ReturnlUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnId)
        {
            Product product = _storeRepository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            Cart.AddItem(product, 1);
            return RedirectToPage(new { ReturlUrl = ReturnlUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl =>
                cl.Product.ProductId == productId).Product);
            return RedirectToPage(new { ReturnUrl = returnUrl });
        }
    }
}
