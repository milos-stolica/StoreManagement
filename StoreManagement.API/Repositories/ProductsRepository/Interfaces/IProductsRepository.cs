using System.Collections.Generic;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Repositories
{
    /// <summary>
    /// CRUD operations on store products
    /// </summary>
    public interface IProductsRepository
    {
        IEnumerable<ExtendedEntity<Product>> DecreaseProductsQuantity(IDictionary<string, float> skuToQuantity);
    }
}
