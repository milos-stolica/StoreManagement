using System;
using System.Collections.Generic;
using StoreManagement.API.Models;

namespace StoreManagement.API.Services
{
    public interface IOrderingService
    {
        void MakeOrder(IEnumerable<OrderCreationDTO> orderList, Action<OrderDetails> orderFinished);

        OrderReceiptDTO GenerateReceipt(OrderDetails order);

        OrderUnprocessableDTO GenerateError(OrderDetails order);
    }
}
