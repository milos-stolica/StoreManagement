using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;

namespace StoreManagement.Tests.Repositories.Mock
{
    class NotUpdatableDataMock : ProductRepositoryMockCreatorBase
    {
        public override List<ExtendedEntity<Product>> ExpectedResults => new List<ExtendedEntity<Product>>()
        {
            new ExtendedEntity<Product>(EntityStatus.UnprocessableUpdate, new Product() { Sku = "testSku1", Quantity = 1000 }),
            new ExtendedEntity<Product>(EntityStatus.NotUpdated, new Product() { Sku = "testSku2", Quantity = 2000 }),
            new ExtendedEntity<Product>(EntityStatus.NotFound, new Product() { Sku = "testSku3", Quantity = new NullProduct().Quantity })
        };

        public override Dictionary<string, float> RequiredProducts => new Dictionary<string, float>()
        {
            { "testSku1", 1000.1f },
            { "testSku2", 200 },
            { "testSku3", 300 }
        };
    }
}
