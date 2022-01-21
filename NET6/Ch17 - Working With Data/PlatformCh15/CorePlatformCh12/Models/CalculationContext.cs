using Microsoft.EntityFrameworkCore;

namespace DataCh17.Models
{
    public class CalculationContext : DbContext
    {
        public CalculationContext(DbContextOptions<CalculationContext> opts) : base(opts)
        { }

        public DbSet<Calculation> Calculations { get; set; }
    }
}
