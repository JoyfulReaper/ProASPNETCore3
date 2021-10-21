using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform4.Models
{
    public class CalculationContext : DbContext
    {
        public CalculationContext(DbContextOptions<CalculationContext> opts) : base (opts)
        {

        }

        public DbSet<Calculation> Calculations { get; set; }
    }
}
