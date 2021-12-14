namespace SimpleApp.Model
{
    public interface IDataSource
    {
        IEnumerable<Product> Products { get; }
    }
}
