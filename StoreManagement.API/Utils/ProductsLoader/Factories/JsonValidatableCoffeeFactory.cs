using System;
using Microsoft.Extensions.Logging;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public class JsonValidatableCoffeeFactory : JsonValidatableProductFactory
    {
        #region Fields

        private ILogger<CoffeeValidator> logger;

        #endregion Fields

        public JsonValidatableCoffeeFactory(ILogger<CoffeeValidator> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region JsonValidatableProductFactory members

        public override IJsonProductFactory Factory => factory ??= new JsonCoffeeFactory();

        public override IValidator Validator => validator ??= new CoffeeValidator(logger);

        #endregion JsonValidatableProductFactory members
    }
}
