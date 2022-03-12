using Newtonsoft.Json.Linq;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public class FruitFactory : IJsonProductFactory
    {
        public Product CreateProduct(JObject jObject)
        {
            return jObject.ToObject<Fruit>();
        }
    }
}
