using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StoreManagement.API.Entities;
using StoreManagement.API.Entities.Extensions;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Repositories
{
    public class InMemoryProductsRepository : IProductsRepository
    {
        #region Fields

        protected readonly Dictionary<string, Product> products;

        //make threads synchronization for products access 
        protected readonly ReaderWriterLockSlim rwls;

        #endregion Fields

        #region Constructor

        public InMemoryProductsRepository(IProductsLoader productsLoader)
        {
            ArgumentValidator.ThrowIfNull(productsLoader, nameof(productsLoader));
            products = productsLoader.Load();
            rwls = new ReaderWriterLockSlim();
        }

        #endregion Constructor

        #region IProductsRepository members

        public IEnumerable<ExtendedEntity<Product>> DecreaseProductsQuantity(IDictionary<string, float> skuToQuantity)
        {
            ArgumentValidator.ThrowIfNull(skuToQuantity, nameof(skuToQuantity));

            if(skuToQuantity.Any(kvp => kvp.Value < 0))
            {
                throw new ArgumentException($"Negative quantity values are not allowed for {nameof(skuToQuantity)}.");
            }

            try
            {
                rwls.EnterWriteLock();
                
                return DecreaseProductsQuantityUnsafe(skuToQuantity);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        #endregion IProductsRepository members

        #region Private methods

        private IEnumerable<ExtendedEntity<Product>> DecreaseProductsQuantityUnsafe(IDictionary<string, float> skuToQuantity)
        {
            IEnumerable<ExtendedEntity<Product>> notExistingProducts = GetNonExistingProductsUnsafe(skuToQuantity.Keys);
            IEnumerable<ExtendedEntity<Product>> outOfStockProducts = GetInsufficientQuantityProductsUnsafe(skuToQuantity);
            if (notExistingProducts.Any() || outOfStockProducts.Any())
            {
                return skuToQuantity.Keys
                          .Except(notExistingProducts.ToSkus())
                          .Except(outOfStockProducts.ToSkus())
                          .Select(sku => GetProductUnsafe(sku).ExtendedProduct(EntityStatus.NotUpdated))
                          .Union(notExistingProducts)
                          .Union(outOfStockProducts);
            }

            //Everything is ok, decrease quantity and return products
            return skuToQuantity.Select(kvp => DecreaseProductQuantityUnsafe(kvp.Key, kvp.Value)).ToList();
        }

        //If negative quantity decrease product quantity for that value, otherwise increase
        private ExtendedEntity<Product> DecreaseProductQuantityUnsafe(string sku, float quantity)
        {
            Product product = GetProductUnsafe(sku);
            //put minus sign because decrease is wanted
            product.UpdateQuantityBy(-quantity);

            return product.ExtendedProduct(EntityStatus.UpdatedSuccessfully);
        }

        private Product GetProductUnsafe(string sku)
        {
            return products[sku];
        }

        private bool HasEnoughQuantityUnsafe(string sku, float quantity)
        {
            return GetProductUnsafe(sku).Quantity >= quantity;
        }

        private bool ProductExistsUnsafe(string sku)
        {
            return products.ContainsKey(sku);
        }

        private IEnumerable<ExtendedEntity<Product>> GetInsufficientQuantityProductsUnsafe(IDictionary<string, float> skuToQuantity)
        {
            return skuToQuantity.Where(kvp => ProductExistsUnsafe(kvp.Key) && !HasEnoughQuantityUnsafe(kvp.Key, kvp.Value))
                                .Select(kvp => GetProductUnsafe(kvp.Key).ExtendedProduct(EntityStatus.UnprocessableUpdate));
        }

        private IEnumerable<ExtendedEntity<Product>> GetNonExistingProductsUnsafe(IEnumerable<string> productSkuList)
        {
            return productSkuList.Where(sku => !products.ContainsKey(sku))
                                 .Select(sku => new NullProduct() { Sku = sku }.ExtendedProduct(EntityStatus.NotFound));
        }

        #endregion Private methods
    }
}
