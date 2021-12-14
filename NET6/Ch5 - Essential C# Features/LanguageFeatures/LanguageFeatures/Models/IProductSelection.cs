namespace LanguageFeatures.Models
{
    public interface IProductSelection
    {
        IEnumerable<Product> Products { get; }

        //Default implementation in an interface
        IEnumerable<string> Names => Products.Select(p => p.Name);
    }
}
