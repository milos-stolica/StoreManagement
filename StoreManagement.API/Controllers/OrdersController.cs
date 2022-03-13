using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Models;
using StoreManagement.API.Services;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderingService orderingService;

        public OrdersController(IOrderingService orderingService)
        {
            this.orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
        }

        #region Post methods

        [HttpPost]
        public IActionResult MakeOrder(IEnumerable<OrderCreationDTO> orderList)
        {
            IActionResult result = null;
            orderingService.MakeOrder(orderList, orderDetails => result = GenerateOrderResponse(orderDetails));
            return result;
        }

        #endregion Post methods

        #region Private methods

        private IActionResult GenerateOrderResponse(OrderDetails orderDetails)
        {
            switch(orderDetails.Status)
            {
                case OrderStatus.NotExistingProducts:
                    return BadRequest();
                case OrderStatus.OutOfStock:
                    return UnprocessableEntity(orderingService.GenerateError(orderDetails));
                case OrderStatus.Successful:
                    return Ok(orderingService.GenerateReceipt(orderDetails));
                default:
                    throw new ArgumentException($"Unexpected order status received: {orderDetails.Status}");
            }
        }

        #endregion Private methods
    }
}
