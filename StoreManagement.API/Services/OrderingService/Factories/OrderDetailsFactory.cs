using System.Collections.Generic;

namespace StoreManagement.API.Services
{ 
    public class OrderDetailsFactory : IOrderDetailsFactory
    {
        public OrderDetails CreateOrderDetails(OrderStatus status, IEnumerable<OrderedProductDetails> productsDetails)
        {
            return new OrderDetails()
            {
                Status = status,
                ProductsDetails = productsDetails
            };
        }
    }
}
