using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;

namespace StoreManagement.Tests.Repositories.Mock
{
    public abstract class ProductRepositoryMockCreatorBase : IProductRepositoryMockCreator
    {
        public abstract List<ExtendedEntity<Product>> ExpectedResults { get; }

        public abstract Dictionary<string, float> RequiredProducts { get; }

        public Dictionary<string, float> ProductsSource => new Dictionary<string, float>()
        {
            {"testSku1", 1000 },
            {"testSku2", 2000 }
        };
}
}
