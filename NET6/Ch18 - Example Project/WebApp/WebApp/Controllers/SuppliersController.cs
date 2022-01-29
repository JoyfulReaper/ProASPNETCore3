using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly DataContext _context;

        public SuppliersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<Supplier> GetSupplier(long id)
        {
            // Circular References in Related Data (pg 472 - 473)
            //return await _context.Suppliers
            //    .Include(s => s.Products)
            //    .FirstAsync(s => s.SupplierId == id);

            // Breaking cirular References
            Supplier supplier = await _context.Suppliers
                .Include(s => s.Products)
                .FirstAsync(s => s.SupplierId == id);

            foreach(Product p in supplier.Products)
            {
                p.Supplier = null;
            };

            return supplier;
        }

        [HttpPatch("{id}")]
        public async Task<Supplier> PatchSupplier(long id,
            JsonPatchDocument<Supplier> patchDoc)
        {
            Supplier s = await _context.Suppliers.FindAsync(id);
            if(s != null)
            {
                patchDoc.ApplyTo(s);
                await _context.SaveChangesAsync();
            }

            return s;
        }
    }
}
