using StoreManagement.API.Repositories;

namespace StoreManagement.API.Entities.Extensions
{
    public static class ProductExt
    {
        public static void UpdateQuantityBy(this Product product,  float quantity)
        {
            product.Quantity += quantity;
        }

        public static ExtendedEntity<Product> ExtendedProduct(this Product product, EntityStatus status)
        {
            return new ExtendedEntity<Product>(status, product);
        }
    }
}
