using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;

namespace StoreManagement.Tests.Repositories.Mock
{
    class ValidBondaryCaseDataMock : ProductRepositoryMockCreatorBase
    {
        public override List<ExtendedEntity<Product>> ExpectedResults => new List<ExtendedEntity<Product>>()
        {
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, new Product() { Sku = "testSku1", Quantity = 1000 }),
            new ExtendedEntity<Product>(EntityStatus.UpdatedSuccessfully, new Product() { Sku = "testSku2", Quantity = 0 })
        };

        public override Dictionary<string, float> RequiredProducts => new Dictionary<string, float>()
        {
            { "testSku1", 0 },
            { "testSku2", 2000 }
        };
    }
}
