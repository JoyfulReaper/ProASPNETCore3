using Microsoft.AspNetCore.Mvc;
using Ch5_LanguageFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ch5_LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            List<string> results = new List<string>();

            foreach (Product p in Product.GetProducts())
            {
                string name = p?.Name ?? "<No Name>";
                decimal? price = p?.Price ?? 0;
                string relatedName = p?.Related?.Name ?? "<None>";

                //results.Add(string.Format("Name: {0}, Price: {1}, Related: {2}", name, price, relatedName));

                // String Interpolation
                //results.Add($"Name: {name}, Price: {price}, Related: {relatedName}");
            }


            // Index Initializer
            //Dictionary<string, Product> products = new Dictionary<string, Product>
            //{
            //    ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
            //    ["Lifejacket"] = new Product { Name = "Kayak", Price = 48.9M }
            //};
            //return View("Index", products.Keys);


            //Pattern Matching
            //object[] data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };
            //decimal total = 0;
            //for (int i = 0; i < data.Length; i++)
            //{
            //    if (data[i] is decimal d)
            //    {
            //        total += d;
            //    }
            //}
            //return View("Index", new string[] { $"Total: {total:c2}" });


            //Pattern Matching Switch Statement
            //object[] data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };
            //decimal total = 0;
            //for (int i = 0; i < data.Length; i++)
            //{
            //    switch (data[i])
            //    {
            //        case decimal decimalValue:
            //            total += decimalValue;
            //            break;
            //        case int intValue when intValue > 50:
            //            total += intValue;
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //return View("Index", new string[] { $"Total: {total:c2}" });


            //Extension Method
            //ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };
            //decimal cartTotal = cart.TotalPrices();
            //return View("Index", new string[] { $"Total: {cartTotal:c2}" });


            //Extension Method on Interface
            //ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };
            //Product[] productArray =
            //{
            //    new Product {Name = "Kayak", Price = 275M},
            //    new Product {Name = "Lifejacket", Price = 48.95M},
            //};

            //decimal cartTotal = cart.TotalPrices();
            //decimal arrayTotal = productArray.TotalPrices();

            //return View("Index", new string[] {
            //    $"Cart Total: {cartTotal:c2}",
            //    $"ArrayTotal: {arrayTotal:c2}" });


            //   ////////////////////////////////////////////////////////
            //   // Filtering Extension Method
            //   Product[] productArray =
            //   {
            //       new Product { Name = "Kayak", Price = 275M },
            //       new Product { Name = "Lifejacket", Price = 48.95M },
            //       new Product { Name = "Soccer Ball", Price = 19.50M },
            //       new Product { Name = "Corner Flag", Price = 34.95M },
            //   };

            //   Func<Product, bool> nameFilter = delegate (Product prod)
            //   {
            //       return prod?.Name?[0] == 'S';
            //   };

            //   // Without Lambda Expression:
            //   //decimal priceFilterTotal = productArray.Filter(FilterByPrice).TotalPrices();
            //   //decimal nameFilterTotal = productArray.Filter(nameFilter).TotalPrices();

            //   // With Lambda Expression:
            //   decimal priceFilterTotal = productArray.Filter(p => (p?.Price ?? 0) >= 20).TotalPrices();
            //   decimal nameFilterTotal = productArray.Filter(p => (p?.Name?[0] == 'S')).TotalPrices();


            //   decimal arrayTotal = productArray.FilterByPrice(20).TotalPrices();
            //   return View("Index", new string[] { $"Price Total: {priceFilterTotal:c2}",
            //       $"Name Total: {nameFilterTotal:c2}" });
            //////////////////////////////////////////////////////////////////////////////////////

            return View(Product.GetProducts().Select(p => p?.Name));

            //return View(new string[] { "C#", "Language", "Features" });
        }

        // Single statement method rewritten as Lambda Expression
        public ViewResult Index2() =>
            View("Index", Product.GetProducts().Select(p => p?.Name));

        public ViewResult Index3()
        {
            IProductSelection cart = new ShoppingCart(
                       new Product { Name = "Kayak", Price = 275M },
                       new Product { Name = "Lifejacket", Price = 48.95M },
                       new Product { Name = "Soccer Ball", Price = 19.50M },
                       new Product { Name = "Corner Flag", Price = 34.95M }
                );

            //return View("Index", cart.Products.Select(p => p.Name));
            return View("Index", cart.Names);
        }

        public async Task<ViewResult> Index4()
        {
            long? length = await MyAsyncMethods.GetPageLength2();
            return View("Index", new string[] { $"Length: {length}" });
        }

        public async Task<ViewResult> AsyncIndex1()
        {
            List<string> output = new List<string>();
            foreach (var len in await MyAsyncMethods.GetPageLength3(output,
                "apress.com", "microsoft.com", "amazon.com", "kgivler.com"))
            {
                output.Add($"Page length: {len}");
            }

            return View("Index", output);
        }

        public async Task<ViewResult> AsyncIndex2()
        {
            List<string> output = new List<string>();
            await foreach (long? len in MyAsyncMethods.GetPageLength4(output,
                "apress.com", "microsoft.com", "amazon.com", "kgivler.com"))
            {
                output.Add($"Page length: {len}");
            }

            return View("Index", output);
        }

        bool FilterByPrice(Product p)
        {
            return ((p?.Price ?? 0) >= 20);
        }
    }
}
