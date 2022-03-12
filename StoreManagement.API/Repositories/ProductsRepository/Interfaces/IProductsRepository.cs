using System.Collections.Generic;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Repositories
{
    public interface IProductsRepository
    {
        IEnumerable<ExtendedEntity<Product>> DecreaseProductsQuantity(IDictionary<string, float> skuToQuantity);
    }
}
