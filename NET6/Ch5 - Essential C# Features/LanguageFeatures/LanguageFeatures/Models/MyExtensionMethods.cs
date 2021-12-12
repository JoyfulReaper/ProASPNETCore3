namespace LanguageFeatures.Models
{
    public static class MyExtensionMethods
    {
        public static decimal TotalPrices(this ShoppingCart cartParam)
        {
            decimal total = 0;
            foreach(Product product in cartParam.Products)
            {
                total += product?.Price ?? 0;
            }
            return total;
        }

        public static decimal TotalPrices2(this IEnumerable<Product> products)
        {
            decimal total = 0;
            foreach (Product product in products)
            {
                total += product?.Price ?? 0;
            }
            return total;
        }
    }
}
