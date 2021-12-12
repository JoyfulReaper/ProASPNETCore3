namespace LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; } = "Watersports"; // Auto-implemented property with Property Initiazer
        public decimal? Price { get; set; }
        public Product Releated { get; set; }
        //public bool InStock { get; } = true; // Read only auto-implemented property
        public bool InStock { get; } // Can be assigned a value in the constructor!

        public Product(bool stock = true)
        {
            InStock = stock;
        }

        public static Product[] GetProducts()
        {
            // Object Initilizer Syntax
            Product kayak = new Product(false)
            {
                Name = "Kayak",
                Price = 275m,
                Category = "Water Craft"
            };

            Product lifejacket = new Product
            {
                Name = "LifeJacket",
                Price = 48.95m
            };

            kayak.Releated = lifejacket;

            return new Product[] {kayak, lifejacket, null};
        }
    }
}
