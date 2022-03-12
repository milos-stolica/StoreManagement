using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public interface IProductFactory<T>
    {
        Product CreateProduct(T arg);
    }
}
