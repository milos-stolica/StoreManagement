using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public class JsonProductsLoader : IProductsLoader
    {
        #region Fields

        private readonly string filePath;
        private readonly IDictionary<string, IJsonProductFactory> productFactories;
        private readonly IDictionary<string, IValidator> productValidators;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public JsonProductsLoader(string filePath,
                                  IDictionary<string, IJsonProductFactory> productFactories,
                                  IDictionary<string, IValidator> productValidators,
                                  ILogger<JsonProductsLoader> logger)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.productFactories = productFactories ?? throw new ArgumentNullException(nameof(productFactories));
            this.productValidators = productValidators ?? throw new ArgumentNullException(nameof(productValidators));
            this.logger = logger ?? throw new ArgumentNullException(nameof(productValidators));
        }

        #endregion Constructor

        #region IProductsLoader members

        public Dictionary<string, Product> Load()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>();

            try
            {
                string jsonString = LoadProductsJsonString();

                foreach (JObject jObject in JArray.Parse(jsonString))
                {
                    Product product;
                    if(!TryCreateProduct(jObject, out product))
                    {
                        continue;
                    }

                    if(!IsValidProduct(product))
                    {
                        continue;
                    }

                    products.Add(product.Sku, product);
                }

                logger.LogInformation($"{products.Count} products loaded from {filePath}");
                return products;
            }
            catch(Exception ex)
            {
                logger.LogError($"Unable to load products from {filePath}. Exception: {ex}");
                throw;
            } 
        }

        #endregion IProductsLoader members

        #region Private methods

        private string LoadProductsJsonString()
        {
            StreamReader productsReader = new StreamReader(filePath);
            return productsReader.ReadToEnd();
        }

        private bool IsValidProduct(Product product)
        {
            IValidator productValidator;
            if (!productValidators.TryGetValue(product.Type, out productValidator))
            {
                logger.LogWarning($"Validator not registered for product type: {product.Type}");
                return false;
            }

            if(!productValidator.IsValid(product))
            {
                logger.LogInformation($"Some properties of {product.Sku ?? "unknown Sku"} product are not valid");
                return false;
            }

            return true;
        }

        private bool TryCreateProduct(JObject jObject, out Product product)
        {
            string productType;
            try
            {
                productType = (string)jObject["type"] ?? throw new ArgumentNullException(nameof(productType));
            }
            catch(Exception ex)
            {
                logger.LogWarning($"{ex}");
                product = new NullProduct();
                return false;
            }

            IJsonProductFactory productFactory;
            if (!productFactories.TryGetValue(productType, out productFactory))
            {
                logger.LogWarning($"Factory not registered for product type: {productType}");
                product = new NullProduct();
                return false;
            }

            product = productFactory.CreateProduct(jObject);
            return true;
        }

        #endregion Private methods
    }
}
