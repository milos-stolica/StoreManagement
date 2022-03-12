using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;

namespace StoreManagement.Tests.Repositories.Mock
{
    public interface IProductRepositoryMockCreator
    {
        List<ExtendedEntity<Product>> ExpectedResults { get; }

        Dictionary<string, float> RequiredProducts { get; }

        Dictionary<string, float> ProductsSource { get; }
    }
}
