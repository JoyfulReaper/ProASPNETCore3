using LanguageFeatures.Models;
using Microsoft.AspNetCore.Mvc;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index8()
        {
            List<string> output = new List<string>();
            await foreach (long? len in MyAsyncMethods.GetPageLengthsAE(output,
                "apress.com", "microsoft.com", "amazon.com"))
            {
                output.Add($"Page length: {len }");
            }

            return View("Index", output);
        }

        public async Task<IActionResult> Index7()
        {
            List<string> output = new List<string>();
            foreach (long? len in await MyAsyncMethods.GetPageLengths(output,
                "apress.com", "microsoft.com", "amazon.com"))
            {
                output.Add($"Page length: {len }");
            }

            return View("Index", output);
        }

        public async Task<ViewResult> Index6()
        {
            long? length = await MyAsyncMethods.GetPageLength2();
            return View("Index", new string[] { $"Length: {length}" });
        }

        public ViewResult Index5()
        {

            IProductSelection cart = new ShoppingCart3(   
                new Product { Name = "Kayak", Price = 275m},
                new Product { Name = "LifeJacket", Price = 48.95m},
                new Product { Name = "Soccer Ball", Price = 19.50m},
                new Product { Name = "Corner Flag", Price = 34.95m}
            );


            //return View("Index", cart.Products.Select(p => p.Name));
            return View("Index", cart.Names);
        }

        private bool FilterByPrice(Product p)
        {
            return (p?.Price ?? 0) > 20;
        }

        Func<Product, bool> nameFilter = delegate (Product prod)
        {
            return prod?.Name?[0] == 'S';
        };

        public ViewResult Index4()
        {
            Product[] productArray =
            {
                new Product { Name = "Kayak", Price = 275m},
                new Product { Name = "LifeJacket", Price = 48.95m},
                new Product { Name = "Soccer Ball", Price = 19.50m},
                new Product { Name = "Corner Flag", Price = 34.95m},
            };
            decimal priceFilterTotal = productArray
                .Filter(p => (p?.Price ?? 0) >= 20)
                .TotalPrices2();

            decimal nameFilter1 = productArray
                .Filter(p => (p?.Name?[0]) == 'S')
                .TotalPrices2();

            decimal priceFilterTotal2 = productArray
                .Filter(FilterByPrice)
                .TotalPrices2();

            decimal nameFilter2 = productArray
                .Filter(nameFilter)
                .TotalPrices2();

            return View("Index", new string[] {
                $"Price Total: {priceFilterTotal:c2}",
                $"Name Total: {nameFilter1:c2}" } 
            );
        }

        public ViewResult Index3()
        {
            Product[] productArray =
            {
                new Product { Name = "Kayak", Price = 275m},
                new Product { Name = "LifeJacket", Price = 48.95m},
                new Product { Name = "Soccer Ball", Price = 19.50m},
                new Product { Name = "Corner Flag", Price = 34.95m},
            };
            decimal arrayTotal = productArray.FilterByPrice(20).TotalPrices2();
            return View("Index", new string[] {$"Array Total: {arrayTotal:c2}"});
        }

        public ViewResult Index2()
        {
            ShoppingCart2 cart = new ShoppingCart2
            {
                Products = Product.GetProducts()
            };

            Product[] productArray =
            {
                new Product { Name = "Kayak", Price = 275m},
                new Product { Name = "LifeJacket", Price = 48.95m},
            };

            decimal cartTotal = cart.TotalPrices2();
            decimal arrayTotal = productArray.TotalPrices2();

            return View("Index", new string[]
            {
                $"Cart Total: {cartTotal:c2}",
                $"Array Total: {arrayTotal}"
            });
        }

        public ViewResult Index()
        {
            // Using an extension Method
            ShoppingCart cart = new ShoppingCart
            {
                Products = Product.GetProducts()
            };
            decimal cartTotal = cart.TotalPrices();
            return View("Index", new string[] { $"Total: {cartTotal:c2}"});


            /////////////////////////////////////////////////

            object[] data = new object[] { 275m, 29.95m, "apple", "orange", 100, 10 };
            decimal total = 0;
            for (int i = 0; i < data.Length; i++)
            {
                switch(data[i])
                {
                    case decimal decimalValue:
                        total += decimalValue;
                        break;
                    case int intValue when intValue > 50:
                        total += intValue;
                        break;
                }
            }
            return View("Index", new string[] { $"Total: {total:C2}" });

            ////////////////////////////////////////////////

            object[] data2 = new object[] { 275m, 29.95m, "apple", "orange", 100, 10 };
            decimal total2 = 0;
            for(int i = 0; i < data.Length; i++)
            {
                if(data[i] is decimal d)
                {
                    total += d;
                }
            }
            return View("Index", new string[] { $"Total: {total2:C2}" });

            ///////////////////////////////////////////////

            List<string> results = new List<string>();
            foreach (var p in Product.GetProducts())
            {
                // Null conditional operator and Null Coalescing Operator
                string name = p?.Name ?? "<No Name>";
                decimal? price = p?.Price ?? 0;
                string relatedName = p?.Releated?.Name ?? "<None>";
                //results.Add(string.Format("Name: {0}, Price {1}, Related: {2}", name, price, relatedName));
                results.Add($"Name: {name}, Price: {price}, Related: {relatedName}"); // String Interpolation
            }
            return View(results);

            ////////////////////////////////////////////////

            //Index Initializers
            Dictionary<string, Product> products = new Dictionary<string, Product>
            {
                ["Kayak"] = new Product { Name = "Kayak", Price = 275m },
                ["Lifejacket"] = new Product { Name = "Lifejacket", Price = 48.95m },
            };
            return View(products.Keys);

            /////////////////////////////////////////////

            //Collection Initializer:
            // return View("Index", new string[] { "Bob", "Joe", "Alice" });
        }
    }
}
