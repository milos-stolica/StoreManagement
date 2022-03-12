using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;
using StoreManagement.API.Utils;
using StoreManagement.Tests.Repositories.Mock;

namespace StoreManagement.Tests.Repositories
{
    public class InMemoryProductsRepositoryTests
    {
        #region Fields

        private static List<IProductRepositoryMockCreator> mockCreators = new List<IProductRepositoryMockCreator>()
        {
            new ValidDataMock(),
            new ValidBondaryCaseDataMock(),
            new NotUpdatableDataMock()
        };

        #endregion Fields

        #region Constructor tests

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CreateProductRepo(CreateProductsLoader(new Dictionary<string, float>())));
        }

        [Test]
        public void Constructor_DoesThrow()
        {
            Assert.Throws<ArgumentNullException>(() => CreateProductRepo(null));
        }

        #endregion Constructor tests

        #region DecreaseProductsQuantity tests

        [TestCaseSource(nameof(DecreaseProductsQuantitySource))]
        public void DecreaseProductsQuantity(Dictionary<string, float> productsSource, 
                                             Dictionary<string, float> requiredProducts,
                                             List<ExtendedEntity<Product>> expectedResults)
        {
            InMemoryProductsRepository repo = CreateProductRepo(CreateProductsLoader(productsSource));

            IEnumerable<ExtendedEntity<Product>> actualProducts = repo.DecreaseProductsQuantity(requiredProducts);

            actualProducts.ToList()
                          .ForEach(product => AssertProductExistsInExpectedResults(product, expectedResults));
        }

        [Test]
        public void DecreaseProductsQuantity_ThrowsNullArgumentException()
        {
            InMemoryProductsRepository repo = CreateProductRepo(CreateProductsLoader(mockCreators[0].ProductsSource));

            Assert.Throws<ArgumentNullException>(() => repo.DecreaseProductsQuantity(null));
        }

        [Test]
        public void DecreaseProductsQuantity_ThrowsArgumentException()
        {
            InMemoryProductsRepository repo = CreateProductRepo(CreateProductsLoader(mockCreators[0].ProductsSource));

            Assert.Throws<ArgumentException>(() => repo.DecreaseProductsQuantity(InvalidProductsQuantities));
        }

        #endregion DecreaseProductsQuantity tests

        #region Mock

        private static IEnumerable<object[]> DecreaseProductsQuantitySource()
        {
            foreach (var creator in mockCreators)
            {
                yield return new object[] { creator.ProductsSource, creator.RequiredProducts, creator.ExpectedResults };
            }
        }

        private Dictionary<string, float> InvalidProductsQuantities => new Dictionary<string, float>()
        {
            { "testSku1", -1000 },
            { "testSku2", 2001 }
        };

        private IProductsLoader CreateProductsLoader(Dictionary<string, float> skuToQuantity)
        {
            IProductsLoader productsLoader = Substitute.For<IProductsLoader>();

            Dictionary<string, Product> products = skuToQuantity.Select(kvp => new Product() { Sku = kvp.Key, Quantity = kvp.Value })
                                                                .ToDictionary(prod => prod.Sku, prod => prod);

            productsLoader.Load().Returns(products);

            return productsLoader;
        }

        #endregion Mock

        #region Private members

        private InMemoryProductsRepository CreateProductRepo(IProductsLoader productsLoader)
        {
            return new InMemoryProductsRepository(productsLoader);
        }

        private void AssertProductExistsInExpectedResults(ExtendedEntity<Product> product, IEnumerable<ExtendedEntity<Product>> expectedProducts)
        {
            Assert.NotNull(expectedProducts.FirstOrDefault(prod => 
                                                           prod.Entity.Sku == product.Entity.Sku &&
                                                           prod.Entity.Quantity == product.Entity.Quantity &&
                                                           prod.StatusCode == product.StatusCode));
        }

        #endregion Private members
    }
}
