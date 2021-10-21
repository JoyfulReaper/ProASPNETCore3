using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform4.Models
{
    public class SeedData
    {
        private readonly CalculationContext _dataContext;
        private readonly ILogger<SeedData> _log;

        private static Dictionary<int, long> data
            = new Dictionary<int, long>()
            {
                {1,1}, {2,3}, {3,6}, {4,10},{5,15},
                {6,21}, {7,28}, {8,36},{9,45}, {10,55}
            };
        public SeedData(CalculationContext dataContext,
            ILogger<SeedData> log)
        {
            _dataContext = dataContext;
            _log = log;
        }

        public void SeedDatabase()
        {
            _dataContext.Database.Migrate();
            if(_dataContext.Calculations.Count() == 0)
            {
                _log.LogInformation("Preparing to see database");
                _dataContext.Calculations!.AddRange(data.Select(kvp => new Calculation()
                {
                    Count = kvp.Key, Result = kvp.Value
                }));
                _dataContext.SaveChanges();
                _log.LogInformation("Database seeded");
            } else
            {
                _log.LogInformation("Database not seeded");
            }
        }
    }
}
