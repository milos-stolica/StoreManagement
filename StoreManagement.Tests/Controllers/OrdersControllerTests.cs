using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using StoreManagement.API.Controllers;
using StoreManagement.API.Models;
using StoreManagement.API.Services;

namespace StoreManagement.Tests.Controllers
{
    public class OrdersControllerTests
    {
        #region Fields

        private OrdersController orderController;
        private const string TestSku = "TestSku";

        #endregion Fields

        #region Constructor tests

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CreateController(MockOrderingService(OrderStatus.NotExistingProducts)));
        }

        [Test]
        public void Constructor_DoesThrow()
        {
            Assert.Throws<ArgumentNullException>(() => CreateController(null));
        }

        #endregion Constructor tests

        #region MakeOrder tests

        [TestCase(OrderStatus.NotExistingProducts, typeof(BadRequestResult))]
        [TestCase(OrderStatus.OutOfStock, typeof(UnprocessableEntityObjectResult))]
        [TestCase(OrderStatus.Successful, typeof(OkObjectResult))]
        public void MakeOrder_CheckResultType(OrderStatus status, Type type)
        {
            orderController = CreateController(MockOrderingService(status));

            IActionResult result = orderController.MakeOrder(new List<OrderCreationDTO>());

            Assert.AreEqual(type, result.GetType());
        }

        [TestCase(OrderStatus.OutOfStock)]
        [TestCase(OrderStatus.Successful)]
        [TestCase(OrderStatus.NotExistingProducts)]
        public void MakeOrder_CheckOutputGeneration(OrderStatus status)        
        {
            IOrderingService orderingService = MockOrderingService(status);
            orderController = CreateController(orderingService);

            orderController.MakeOrder(new List<OrderCreationDTO>());

            switch(status)
            {
                case OrderStatus.OutOfStock:
                    orderingService.Received(1).GenerateError(ArgIsOrderDetails(status));
                    break;
                case OrderStatus.Successful:
                    orderingService.Received(1).GenerateReceipt(ArgIsOrderDetails(status));
                    break;
                case OrderStatus.NotExistingProducts:
                    orderingService.DidNotReceiveWithAnyArgs().GenerateError(null);
                    orderingService.DidNotReceiveWithAnyArgs().GenerateReceipt(null);
                    break;
                default:
                    break;
            } 
        }

        #endregion MakeOrder tests

        #region Private members

        private IOrderingService MockOrderingService(OrderStatus status)
        {
            OrderDetails orderDetails = new OrderDetails()
            {
                ProductsDetails = new List<OrderedProductDetails>()
                {
                    new OrderedProductDetails()
                    {
                        Sku = TestSku
                    }
                },
                Status = status
            };

            IOrderingService orderingService = Substitute.For<IOrderingService>();
            orderingService.MakeOrder(Arg.Any<IEnumerable<OrderCreationDTO>>(), Arg.Invoke(orderDetails));

            return orderingService;
        }

        private OrdersController CreateController(IOrderingService service)
        {
            return new OrdersController(service);
        }

        private OrderDetails ArgIsOrderDetails(OrderStatus testStatus)
        {
            return Arg.Is<OrderDetails>(od =>
                                od.ProductsDetails.First().Sku == TestSku &&
                                od.Status == testStatus);
        }

        #endregion Private members
    }
}
