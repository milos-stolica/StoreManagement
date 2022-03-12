using StoreManagement.API.Common;

namespace StoreManagement.API.Services
{
    public class FallbackErrorFactory : IErrorMessageFactory<OrderedProductDetails>
    {
        public string CreateErrorMessage(OrderedProductDetails errorModel)
        {
            return $"Unspecified error happened for {errorModel.Sku} product";
        }
    }
}
