using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController] // No longer need to use [FromBody] Attrib or Check modelstate is valid
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            //return new Product[]
            //{
            //    new Product() { Name = "Product #1"},
            //    new Product() { Name = "Product #2"},
            //};
            return _context.Products.AsAsyncEnumerable();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Fot Swagger/OpenAPI
        public async Task<IActionResult> GetProduct(long id,
            [FromServices] ILogger<ProductsController> logger)
        {
            //return new Product
            //{
            //    ProductId = 1,
            //    Name = "Test Product"
            //};
            logger.LogDebug($"GetProduct Action Invoked: {id}");

            Product p = await _context.Products.FindAsync(id);
            if(p == null)
            {
                return NotFound();
            }

            //return Ok(p);
            return Ok(new
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId,
            });
        }

        [HttpPost]
        //public async Task<IActionResult> SaveProduct([FromBody]ProductBindingTarget target)
        public async Task<IActionResult> SaveProduct(ProductBindingTarget target)
        {
            //if (ModelState.IsValid)
            //{
                Product p = target.ToProduct();
                await _context.Products.AddAsync(p);
                await _context.SaveChangesAsync();
                return Ok(p);
            //}
            //return BadRequest(ModelState);
        }

        [HttpPut]
        //public async Task UpdateProduct([FromBody] Product product)
        public async Task UpdateProduct(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            _context.Products.Remove(new Product { ProductId = id });
            _context.SaveChangesAsync();
        }

        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            //return Redirect("/api/products/1");
            //return RedirectToAction(nameof(GetProduct), new { Id = 1 });
            return RedirectToRoute(new { controller = "Products", action = "GetProduct", Id = 1 });
        }
    }
}
