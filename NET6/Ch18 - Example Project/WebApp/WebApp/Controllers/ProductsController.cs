using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            //return new Product[]
            //{
            //    new Product() { Name = "Product #1"},
            //    new Product() { Name = "Product #2"},
            //};
            return _context.Products;
        }

        [HttpGet("{id}")]
        public Product GetProduct(long id,
            [FromServices] ILogger<ProductsController> logger)
        {
            //return new Product
            //{
            //    ProductId = 1,
            //    Name = "Test Product"
            //};
            logger.LogDebug($"GetProduct Action Invoked: {id}");
            return _context.Products.Find(id);
        }

        [HttpPost]
        public void SaveProduct([FromBody]Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [HttpPut]
        public void UpdateProduct([FromBody] Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(long id)
        {
            _context.Products.Remove(new Product { ProductId = id });
            _context.SaveChanges();
        }
    }
}
