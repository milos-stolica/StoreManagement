using System.Collections.Generic;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public interface IProductsLoader
    {
        Dictionary<string, Product> Load();
    }
}
