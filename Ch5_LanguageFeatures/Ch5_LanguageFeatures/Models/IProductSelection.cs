using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ch5_LanguageFeatures.Models
{
    public interface IProductSelection
    {
        IEnumerable<Product> Products { get; }

        // Default implementation
        IEnumerable<string> Names => Products.Select(p => p.Name);
    }
}
