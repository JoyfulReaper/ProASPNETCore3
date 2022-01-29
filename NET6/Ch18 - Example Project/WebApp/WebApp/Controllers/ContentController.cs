using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly DataContext _context;

        public ContentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("string")]
        public string GetString() => "This is a string response";

        [HttpGet("object/{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")] // Constrain data formats for an action
        public async Task<ProductBindingTarget> GetObject()
        {
            //return await _context.Products.FirstAsync();

            Product p = await _context.Products.FirstAsync();
            return new ProductBindingTarget()
            {
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId,
            };
        }

        [HttpPost]
        [Consumes("application/json")]
        public string SaveProductJson(ProductBindingTarget product)
        {
            return $"JSON: {product.Name}";
        }

        // OpenAPI doesnt support non-unique METHOD/URL patterns....
        //[HttpPost]
        //[Consumes("application/xml")]
        //public string SaveProductXml(ProductBindingTarget product)
        //{
        //    return $"XML: {product.Name}";
        //}
    }
}
