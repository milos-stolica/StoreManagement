using System;
using System.Collections.Generic;

namespace StoreManagement.API.Services
{
    public class OrderDetails
    {
        public OrderStatus Status { get; set; }

        public IEnumerable<OrderedProductDetails> ProductsDetails { get; set; }
    }
}
