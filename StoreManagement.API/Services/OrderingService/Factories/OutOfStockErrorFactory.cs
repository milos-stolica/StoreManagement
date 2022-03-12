using StoreManagement.API.Common;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Services
{
    public class OutOfStockErrorFactory : IErrorMessageFactory<OrderedProductDetails>
    {
        public string CreateErrorMessage(OrderedProductDetails productDetails)
        {
            return $"Insufficient product {productDetails.Sku} quantity. " +
                   $"Remaining quantity: {MathWrapper.Round(productDetails.RemainingQuantity, 2)}";
        }
    }
}
