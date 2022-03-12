using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;

namespace StoreManagement.Tests.Repositories.Mock
{
    class ValidDataMock : ProductRepositoryMockCreatorBase
    {
        public override List<ExtendedEntity<Product>> ExpectedResults => new List<ExtendedEntity<Product>>()
        {
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, new Product() { Sku = "testSku1", Quantity = 900 }),
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, new Product() { Sku = "testSku2", Quantity = 1800 })
        };

        public override Dictionary<string, float> RequiredProducts => new Dictionary<string, float>()
        {
            { "testSku1", 100 },
            { "testSku2", 200 }
        };
    }
}
