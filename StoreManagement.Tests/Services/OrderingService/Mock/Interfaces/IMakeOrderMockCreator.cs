using System.Collections.Generic;
using StoreManagement.API.Entities;
using StoreManagement.API.Models;
using StoreManagement.API.Repositories;
using StoreManagement.API.Services;

namespace StoreManagement.Tests.Services.Mock
{
    public interface IMakeOrderMockCreator
    {
        IEnumerable<ExtendedEntity<Product>> RepositoryMockedResult { get; }

        OrderDetails ExpectedOrderDetailsResult { get; }

        IEnumerable<OrderCreationDTO> OrderList { get; }
    }
}
