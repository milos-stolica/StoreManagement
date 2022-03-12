using System.Collections.Generic;
using System.Linq;
using StoreManagement.API.Repositories;

namespace StoreManagement.API.Entities.Extensions
{
    public static class ExtendedProductCollectionExt
    {
        public static IEnumerable<string> ToSkus(this IEnumerable<ExtendedEntity<Product>> extendedProducts)
        {
            return extendedProducts.Select(extendedProducts => extendedProducts.Entity.Sku);
        }
    }
}
