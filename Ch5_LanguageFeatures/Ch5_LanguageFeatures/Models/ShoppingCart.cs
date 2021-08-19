using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ch5_LanguageFeatures.Models
{
    public class ShoppingCart : IEnumerable<Product>, IProductSelection
    {
        private List<Product> _products = new List<Product>();

        public IEnumerable<Product> Products { get => _products; }

        public ShoppingCart(params Product[] prods)
        {
            _products.AddRange(prods);
        }

        public IEnumerator<Product> GetEnumerator()
        {
            return Products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
