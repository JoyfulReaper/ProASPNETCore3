using System.Collections;

namespace LanguageFeatures.Models
{
    public class ShoppingCart
    {
        public IEnumerable<Product> Products { get; set; }
    }

    public class ShoppingCart2 : IEnumerable<Product>
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerator<Product> GetEnumerator()
        {
            return Products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ShoppingCart3 : IProductSelection
    {
        private List<Product> _products = new List<Product>();

        public ShoppingCart3(params Product[] products)
        {
            _products.AddRange(products);
        }

        public IEnumerable<Product> Products { get => _products; }
    }
}
