using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;
using StoreManagement.API.Models;
using StoreManagement.API.Repositories;
using StoreManagement.API.Repositories.Extensions;
using StoreManagement.API.Services;
using StoreManagement.API.Services.OrderingService;
using StoreManagement.Tests.Services.Mock;

namespace StoreManagement.Tests.Services
{
    class OrderingServiceTests
    {
        #region Fields

        private static List<IMakeOrderMockCreator> makeOrderMockCreators = new List<IMakeOrderMockCreator>()
        {
            new MakeProcessableOrderMock(),
            new MakeUnprocessableOrderMock(),
            new MakeNotFoundProductOrderMock()
        };

        #endregion Fields

        #region Constructor tests

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Assert.DoesNotThrow(
                () => CreateOrderingService(Substitute.For<IProductsRepository>(),
                                            Substitute.For<IOrderDetailsFactory>(),
                                            CreateErrorFactoriesMock(),
                                            Substitute.For<IMapper>()));
        }

        #endregion Constructor tests

        #region MakeOrder tests

        [TestCaseSource(nameof(MakeOrderDataSource))]
        public void MakeOrder(OrderDetails expectedOrder, 
                              IEnumerable<OrderCreationDTO> orderList, 
                              IEnumerable<ExtendedEntity<Product>> repoReturnedProducts)
        {
            IProductsRepository productRepo = CreateProductsRepoMock(orderList, repoReturnedProducts);
            IOrderDetailsFactory orderFactory = CreateOrderDetailsFactoryMock(expectedOrder);
            IDictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>> errorFactory = CreateErrorFactoriesMock();
            IMapper mapper = CreateMapperMock(repoReturnedProducts.ToEntities(), expectedOrder.ProductsDetails);
            OrderingService service = CreateOrderingService(productRepo, orderFactory, errorFactory, mapper);

            service.MakeOrder(orderList, actualRes => ValidateOrderResults(actualRes, expectedOrder));
        }

        #endregion MakeOrder tests

        #region Mock

        private static IEnumerable<object[]> MakeOrderDataSource()
        {
            foreach (var creator in makeOrderMockCreators)
            {
                yield return new object[] 
                { 
                    creator.ExpectedOrderDetailsResult, 
                    creator.OrderList, 
                    creator.RepositoryMockedResult 
                };
            }
        }

        private IProductsRepository CreateProductsRepoMock(IEnumerable<OrderCreationDTO> orderList, IEnumerable<ExtendedEntity<Product>> returned)
        {
            IProductsRepository productsRepository = Substitute.For<IProductsRepository>();
            productsRepository.DecreaseProductsQuantity(ExpectedInputDictionary(orderList)).Returns(returned);

            return productsRepository;
        }

        private static IDictionary<string, float> ExpectedInputDictionary(IEnumerable<OrderCreationDTO> orderList)
        {
            return Arg.Is<IDictionary<string, float>>(
                dictionary => dictionary.All(kvp => orderList.FirstOrDefault(
                    order => order.ProductSku == kvp.Key && order.Quantity == kvp.Value) != null));
        }

        private IOrderDetailsFactory CreateOrderDetailsFactoryMock(OrderDetails returned)
        {
            IOrderDetailsFactory orderDetailsFactory = Substitute.For<IOrderDetailsFactory>();
            orderDetailsFactory.CreateOrderDetails(Arg.Any<OrderStatus>(), Arg.Any<IEnumerable<OrderedProductDetails>>()).Returns(returned);

            return orderDetailsFactory;
        }

        private static IDictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>> CreateErrorFactoriesMock()
        {
            IErrorMessageFactory<OrderedProductDetails> errorMessageFactory = Substitute.For<IErrorMessageFactory<OrderedProductDetails>>();
            return new Dictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>>()
            {
                { EntityStatus.None, errorMessageFactory }
            };
        }

        private IMapper CreateMapperMock(IEnumerable<Product> products, IEnumerable<OrderedProductDetails> productsDetails)
        {
            IMapper mapper = Substitute.For<IMapper>();
            mapper.Map(products, Arg.Any<Action<IMappingOperationOptions<object, IEnumerable<OrderedProductDetails>>>>()).Returns(productsDetails);

            return mapper;
        }

        #endregion Mock

        #region Private members

        private OrderingService CreateOrderingService(
                                   IProductsRepository productsRepo, 
                                   IOrderDetailsFactory orderDetailsFactory,
                                   IDictionary<EntityStatus,IErrorMessageFactory<OrderedProductDetails>> errorMessageFactory,
                                   IMapper mapper)
        {
            return new OrderingService(productsRepo, orderDetailsFactory, errorMessageFactory, mapper);
        }

        private void ValidateOrderResults(OrderDetails actual, OrderDetails expected)
        {
            Assert.AreEqual(actual.Status, expected.Status);
            foreach (var actualProd in actual.ProductsDetails)
            {
                OrderedProductDetails expectedProd = expected.ProductsDetails.FirstOrDefault(expectedProd => expectedProd.Sku == actualProd.Sku);
                Assert.NotNull(expectedProd);
                Assert.True(AreNearlyEqual(expectedProd.UnitWeight, actualProd.UnitWeight));
                Assert.True(AreNearlyEqual(expectedProd.RemainingQuantity, actualProd.RemainingQuantity));
                Assert.True(AreNearlyEqual(expectedProd.OrderedQuantity, actualProd.OrderedQuantity));
                Assert.True(AreNearlyEqual(expectedProd.Price, actualProd.Price));
                Assert.AreEqual(expectedProd.Currency, actualProd.Currency);
                Assert.AreEqual(expectedProd.Status, actualProd.Status);
            }
        }

        private bool AreNearlyEqual(float first, float second)
        {
            float epsilon = 0.001f;
            return first < second + epsilon && first > second - epsilon;
        }

        #endregion Private members
    }
}
