namespace SimpleApp.Model
{
    public class Product
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public static Product[] GetProducts()
        {
            Product kayay = new Product
            {
                Name = "Kayak",
                Price = 275m
            };

            Product lifejacket = new Product
            {
                Name = "Lifejacket",
                Price = 48.95m
            };

            return new Product[] { kayay, lifejacket };
        }
    }

    public class ProductDataSource : IDataSource
    {
        public IEnumerable<Product> Products =>
            new Product[]
            {
                    new Product { Name = "Kayak", Price = 275M },
                    new Product { Name = "Lifejacket", Price = 48.95m },
            };
    }
}
