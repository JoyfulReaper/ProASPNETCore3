using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ch5_LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; } = "Watersports";
        public decimal? Price { get; set; }
        public Product Related { get; set; }
        public bool Instock { get; } = true;
        public bool NameBeginsWithS => Name?[0] == 'S'; // Lambda Expression defining a Property

        public Product(bool stock = true)
        {
            Instock = stock;
        }

        public static Product[] GetProducts()
        {
            Product kayak = new Product
            {
                Name = "Kayak",
                Category = "Water Craft",
                Price = 275M
            };

            Product lifejacket = new Product(false)
            {
                Name = "Lifejacket",
                Price = 48.95M
            };

            kayak.Related = lifejacket;

            return new Product[] { kayak, lifejacket, null };
        }
    }
}
