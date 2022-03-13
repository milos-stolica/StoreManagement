using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Models;
using StoreManagement.API.Repositories;
using StoreManagement.API.Services;

namespace StoreManagement.Tests.Services.Mock
{
    public class MakeProcessableOrderMock : IMakeOrderMockCreator
    {
        #region Fields

        private readonly List<string> testSkus = new List<string>() { "testSku1", "testSku2" };
        private readonly List<float> testPrices = new List<float>() { 10, 100 };
        private readonly List<float> testUnitWeights = new List<float>() { 1, 2 };
        private readonly List<float> testRemainingQuantities = new List<float>() { 10, 20 };
        private readonly List<float> testOrderedQuantities = new List<float>() { 2, 5 };
        private readonly string testCurrency = "TestCurrency";

        #endregion Fields

        #region IMakeOrderMockCreator mock

        public IEnumerable<ExtendedEntity<Product>> RepositoryMockedResult => new List<ExtendedEntity<Product>>()
        {
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, 
                new Product() 
                { 
                    Sku = testSkus[0], 
                    Price = testPrices[0], 
                    UnitWeight = testUnitWeights[0], 
                    Quantity = testRemainingQuantities[0],
                    Currency = testCurrency
                }),
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, 
                new Product() 
                {
                    Sku = testSkus[1], 
                    Price = testPrices[1], 
                    UnitWeight = testUnitWeights[1], 
                    Quantity = testRemainingQuantities[1],
                    Currency = testCurrency
                })
        };

        public IEnumerable<OrderCreationDTO> OrderList => new List<OrderCreationDTO>()
        {
            new OrderCreationDTO() 
            { 
                ProductSku = testSkus[0], 
                Quantity = testOrderedQuantities[0]
            },
            new OrderCreationDTO() 
            { 
                ProductSku = testSkus[1], 
                Quantity = testOrderedQuantities[1]
            }
        };

        public OrderDetails ExpectedOrderDetailsResult => new OrderDetails()
        {
            Status = OrderStatus.Successful,
            ProductsDetails = new List<OrderedProductDetails>()
            {
                new OrderedProductDetails()
                {
                    Sku = testSkus[0],
                    RemainingQuantity = testRemainingQuantities[0],
                    OrderedQuantity = testOrderedQuantities[0],
                    Price = testPrices[0],
                    UnitWeight = testUnitWeights[0],
                    Status = EntityStatus.UpdatedSuccessfully,
                    Currency = testCurrency
                },
                new OrderedProductDetails()
                {
                    Sku = testSkus[1],
                    RemainingQuantity = testRemainingQuantities[1],
                    OrderedQuantity = testOrderedQuantities[1],
                    Price = testPrices[1],
                    UnitWeight = testUnitWeights[1],
                    Status = EntityStatus.UpdatedSuccessfully,
                    Currency = testCurrency
                }
            }
        };

        #endregion IMakeOrderMockCreator mock
    }
}
