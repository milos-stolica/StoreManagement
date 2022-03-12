using System.Collections.Generic;

namespace StoreManagement.API.Services
{
    public interface IOrderDetailsFactory
    {
        OrderDetails CreateOrderDetails(OrderStatus status, IEnumerable<OrderedProductDetails> productsDetails);
    }
}
