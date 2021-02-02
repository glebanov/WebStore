using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebStore.Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; init; }

        public int? BrandId { get; init; }
    }
}

