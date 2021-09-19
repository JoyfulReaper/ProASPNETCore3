using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApp.Models
{
    public class IDataSource
    {
        public IEnumerable<Product> Products { get; }
    }
}
