using Microsoft.EntityFrameworkCore;

namespace DataCh17.Models
{
    public class SeedData
    {
        private readonly CalculationContext _context;
        private readonly ILogger<SeedData> _log;

        private static readonly Dictionary<int, long> _data
            = new Dictionary<int, long>()
            {
                {1,1}, {2, 3}, {3,6}, {4, 10}, {5, 15},
                {6,21}, {7, 28}, {8,36}, {9, 45}, {10, 55}
            };

        public SeedData(CalculationContext context, ILogger<SeedData> log)
        {
            _context = context;
            _log = log;
        }

        public void SeedDatabase()
        {
            _context.Database.Migrate();
            if(_context.Calculations.Count() == 0)
            {
                _log.LogInformation("Preparing to seed database");
                _context.Calculations!.AddRange(_data.Select(kvp => new Calculation()
                {
                    Count = kvp.Key,
                    Result = kvp.Value
                }));
                _context.SaveChanges();
                _log.LogInformation("Database seeded");
            } else
            {
                _log.LogInformation("Database not seeded");
            }
        }
    }
}
